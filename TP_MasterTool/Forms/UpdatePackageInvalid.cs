using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output + backup log file in wrapUp function

    public partial class UpdatePackageInvalid : Form
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        List<BackgroundWorker> slaveList;
        int masterRow;
        readonly int bgwCount = 20;
        bool cancel = false;
        string[] log;
        string raport1 = "Affected stations:" + Environment.NewLine;
        string raport2 = "";
        readonly string backupLog = @".\Logs\MassTemplateBackupLog.txt";
        Logger logger = new Logger(Globals.Funkcje.UpdatePackageInvalid, "None", "");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();

        public UpdatePackageInvalid()
        {
            InitializeComponent();
            slaveList = new List<BackgroundWorker>();
            for (int i = 0; i < bgwCount; i++)
            {
                slaveList.Add(new BackgroundWorker());
            }
            foreach (BackgroundWorker slave in slaveList)
            {
                slave.DoWork += slave_DoWork;
                slave.RunWorkerCompleted += slave_RunWorkerCompleted;
            }
        }
        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "Abort")
            {
                cancel = true;
                StartStopButton.Text = "Aborting...";
                StartStopButton.Enabled = false;
                try
                {
                    System.IO.File.WriteAllLines(backupLog, log);
                }
                catch { }
                return;
            }

            if (textBox.Text == "")
            {
                textBox.BackColor = Globals.errorColor;
                return;
            }

            disableUI();
            if (SingleRadioButton.Checked)
            {
                log = new string[] { textBox.Text };
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(textBox.Text, "Waiting");
                label1.Text = "0 / 1";
            }
            else
            {
                PopulateGrid();
            }
            StartStopButton.Text = "Abort";
            masterRow = 0;
            int bgwLimit = Math.Min(bgwCount, dataGridView1.Rows.Count);
            for (int i = 0; i < bgwLimit; i++)
            {
                slaveList[i].RunWorkerAsync(masterRow);
                lock (rowLock)
                {
                    masterRow++;
                }
            }
        }
        private void slave_DoWork(object sender, DoWorkEventArgs e)
        {
            int rownr = (int)e.Argument;
            dataGridView1.Rows[rownr].Cells[1].Value = "Connecting";

            ConnectionPara connectionPara = ConnectionPara.EstablishConnectionSilent(dataGridView1.Rows[rownr].Cells[0].Value.ToString());
            if (connectionPara == null)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] Unable to establish connection -> Invalid TAG";
                }
                return;
            }
            else if (connectionPara.IP == "DNS ERROR")
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] Unable to establish connection -> DNS Error";
                }
                return;
            }

            dataGridView1.Rows[rownr].Cells[1].Value = "Mapping drive";
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out CtrlFunctions.CmdOutput cmdOutput))
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] Unable to map drive";
                }
                return;
            }

            //here you enter your function to execution
            //----------------------- temp --------------------------------
            CheckInvalidUpdatePackage(rownr, connectionPara);
        }

        private void CheckInvalidUpdatePackage(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Looking for files");
            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\UpdatePackages\InValid\" + dateTextBox.Text))
            {
                lock (logLock)
                {
                    log[rownr] += " - Clear";
                }
                gridChange(rownr, "Clear", Color.LightGreen);
                return;
            }
            string[] files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\UpdatePackages\InValid\" + dateTextBox.Text, "*.xml", SearchOption.AllDirectories);
            if (files.Length != 0)
            {
                string output = connectionPara.TAG + " Example items:" + Environment.NewLine;
                XDocument tempXml = XDocument.Load(files[0]);
                gridChange(rownr, "Loading Xml");
                int breaker = 0;
                var nodes = tempXml.Root.Elements("Transaction");
                gridChange(rownr, "Scaning XML");
                string additionInfo = "";
                if (nodes.Count() == 0)
                {
                    additionInfo = " - Other Invalid xml found - please check manually and include it in note to MMS team";
                }
                else
                {
                    foreach (XElement node in nodes)
                    {
                        output += "ItemID: " + node.Element("Update").Element("Where").Element("szItemID").Value + Environment.NewLine;
                        output += "SQL Error: " + node.Element("SQLError").Value + Environment.NewLine;
                        breaker++;
                        if (breaker >= 3)
                        {
                            break;
                        }
                    }
                }

                output += tempXml.Root.Element("Trailer").Element("Statistic").Element("UpdateStatements").ToString() + Environment.NewLine;

                lock (logLock)
                {
                    log[rownr] += " - Found";
                    raport1 += connectionPara.TAG + additionInfo + Environment.NewLine;
                    raport2 += output + Environment.NewLine;
                }
                gridChange(rownr, "Found Invalid", Globals.errorColor);
                return;
            }
            lock (logLock)
            {
                log[rownr] += " - Clear";
            }
            gridChange(rownr, "Clear", Color.LightGreen);
        }


        private void slave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backgroundworker encounter error", "Backgroundworker exited with error: " + e.Error.Message);
            }
            slaveMaster();
        }
        private void SingleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            textBox.Text = "";
            if (SingleRadioButton.Checked)
            {
                FetchTxtButton.Enabled = false;
                textBox.Enabled = true;
                return;
            }
            FetchTxtButton.Enabled = true;
            textBox.Enabled = false;
        }
        private void FetchTxtButton_Click(object sender, EventArgs e)
        {
            textBox.Text = FileController.OpenFileDialog("Text files (*.txt)|*.txt", ref logger);
        }
        private void PopulateGrid()
        {
            dataGridView1.Rows.Clear();
            try
            {
                log = System.IO.File.ReadAllLines(textBox.Text);
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Read Error", "Unable to read selected file with machine list." +
                    Environment.NewLine +
                    "Error: " + exp.Message);
                return;
            }
            foreach (string line in log)
            {
                dataGridView1.Rows.Add(line.ToUpper(), "Waiting");
            }
            label1.Text = "0 / " + dataGridView1.Rows.Count;
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
        }
        private void enableUI()//function fired up in wrapUp to enable UI elements anew
        {

        }
        private void disableUI()//function fired up after start to disable UI elements
        {

        }
        private void slaveMaster()
        {
            if (masterRow < dataGridView1.Rows.Count && !cancel)
            {
                foreach (BackgroundWorker slave in slaveList)
                {
                    if (!slave.IsBusy)
                    {
                        slave.RunWorkerAsync(masterRow);
                        lock (rowLock)
                        {
                            label1.Text = masterRow + " / " + dataGridView1.Rows.Count;
                            masterRow++;
                        }
                        return;
                    }
                }
            }
            if (slaveList.Any(slave => slave.IsBusy))
            {
                return;
            }
            wrapUp();
        }//main logic that controls slave work
        private void gridChange(int row, string status, Color rowColor)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = rowColor;
        } //set status and row color
        private void gridChange(int row, string status)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
        } //set status

        private void wrapUp()
        {
            label1.Text = dataGridView1.Rows.Count + " / " + dataGridView1.Rows.Count;
            StartStopButton.Text = "Start";
            StartStopButton.Enabled = true;
            cancel = false;
            try
            {
                System.IO.File.Delete(backupLog);
            }
            catch { }
            enableUI();
            string logPath = @".\Logs\UpdatePackageInvalidCheckLog " + Logger.Datownik() + ".txt";
            string logPath2 = @".\Logs\UpdatePackageInvalidCheckResult " + Logger.Datownik() + ".txt";
            FileController.SaveTxtToFile(logPath2, string.Join(Environment.NewLine, raport1, raport2), ref logger);
            if (FileController.SaveTxtToFile(logPath, string.Join(Environment.NewLine, log), ref logger))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Finished", "Tool finished all tasks." + Environment.NewLine + "Log file created and saved as: " + Path.GetFullPath(logPath));
            }
        } //fireup at the end of list or after abortion when all slaves done their 

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, dataGridView1.Rows[e.RowIndex].Cells[0].Value + " Log", log[e.RowIndex]);
        }
    }
}
