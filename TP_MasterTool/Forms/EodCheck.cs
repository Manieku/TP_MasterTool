using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    public partial class EodCheck : Form
    {
        readonly ConnectionPara connectionPara = Main.interfejs.connectionPara;
        string[] files = null;
        Logger myLog;

        public EodCheck()
        {
            InitializeComponent();
        }

        private void EodCheck_Shown(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    myLog = new Logger(Globals.Funkcje.EodCheck, "None", connectionPara.TAG);
                    try
                    {
                        myLog.Add("Reading log files");
                        files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log", "LOG_*_" + connectionPara.TAG + "_????????_??????_MANUALEOD.xml");
                    }
                    catch (Exception exp)
                    {
                        myLog.Add("Error reading log files");
                        myLog.Add(exp.ToString());
                        myLog.SaveLog("ErrorLog");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading log files", "ToolBox wasn't able to read log files:" + Environment.NewLine + exp.Message);
                        this.Close();
                        return;
                    }
                    if (files.Length == 0)
                    {
                        myLog.Add("No log files found");
                        myLog.SaveLog("WarningLog");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No log files found", "ToolBox wasn't able to find any log files:");
                        this.Close();
                        return;
                    }
                    foreach (string file in files)
                    {
                        XDocument tempXml = XDocument.Load(file);
                        string eodResult = tempXml.Root.Element("BATCHRESULT").Element("szFinalResult").Value;
                        fileNameComboBox.Items.Add(Path.GetFileNameWithoutExtension(file) + " - " + eodResult);
                    }
                    fileNameComboBox.SelectedIndex = 0;
                    CheckButton.Enabled = true;
                };
                slave.RunWorkerAsync();
            }
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.EodCheck);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.EodCheck, files[fileNameComboBox.SelectedIndex]);
            myLog.Add("Check log: " + files[fileNameComboBox.SelectedIndex]);
            ResultTextBox.Text = "";
            XDocument eodXml;
            try
            {
                eodXml = XDocument.Load(files[fileNameComboBox.SelectedIndex]);
            }
            catch (Exception exp)
            {
                myLog.Add("Error reading log files");
                myLog.Add(exp.ToString());
                myLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Error reading log file");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading log file", "ToolBox wasn't able to read log file:" + Environment.NewLine + exp.Message);
                return;
            }
            var nodes = eodXml.Root.Elements("ACTIVITYLOG");
            foreach (XElement node in nodes)
            {
                if (node.Element("szFinalResult").Value == "AbortedCancel" || node.Element("szFinalResult").Value == "Failure")
                {
                    ResultTextBox.AppendText(node.ToString() + Environment.NewLine);
                }
            }
            ResultTextBox.AppendText(Environment.NewLine + eodXml.Root.Element("BATCHRESULT").ToString());
        }
    }
}
