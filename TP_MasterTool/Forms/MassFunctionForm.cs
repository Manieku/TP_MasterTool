using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output + backup log file in wrapUp function

    public partial class MassFunctionForm : Form
    {
        List<BackgroundWorker> slaveList;
        List<string> additionalInfo = new List<string>() { "" };
        int masterRow;
        int iterator;
        readonly int bgwCount = 10;
        bool cancel = false;
        public string[] log;
        string functionName;
        public readonly object rowLock = new object();
        public readonly object logLock = new object();

        public MassFunctionForm(string[] functionList)
        {
            InitializeComponent();
            FunctionSelectBox.DataSource = functionList;
            slaveList = new List<BackgroundWorker>();
            for (int i = 0; i < bgwCount; i++)
            {
                slaveList.Add(new BackgroundWorker());
            }
            foreach (BackgroundWorker slave in slaveList)
            {
                slave.DoWork += Slave_DoWork;
                slave.RunWorkerCompleted += Slave_RunWorkerCompleted;
            }
        }

        //-------BACK END--------------------------
        private void Slave_DoWork(object sender, DoWorkEventArgs e)
        {
            int rownr = (int)e.Argument;

            GridChange(rownr, "Connecting");
            ConnectionPara connectionPara = ConnectionPara.EstablishConnection(dataGridView1.Rows[rownr].Cells[0].Value.ToString());
            if (connectionPara == null)
            {
                ErrorLog(rownr, "Unable to establish connection: Invalid TAG");
                return;
            }
            else if (connectionPara.IP == "DNS ERROR")
            {
                ErrorLog(rownr, "Unable to establish connection: DNS Error");
                return;
            }

            GridChange(rownr, "Mapping drive");
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                ErrorLog(rownr, "Unable to map drive");
                return;
            }

            //here you enter your function to execution
            lock (logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.hostname, (Globals.Funkcje)Enum.Parse(typeof(Globals.Funkcje), functionName), String.Join(" | ", additionalInfo));
            }
            Type type = Type.GetType("TP_MasterTool.Klasy.MassFunctions");
            MethodInfo methodInfo = type.GetMethod(functionName);
            if(methodInfo == null)
            {
                ErrorLog(rownr, "No function found");
                return;
            }
            methodInfo.Invoke(this, new object[] { this, rownr, connectionPara, additionalInfo });
        }
        private void Slave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (rowLock)
            {
                label1.Text = ++iterator + " / " + dataGridView1.Rows.Count;
            }

            SlaveMaster();

            if (e.Error != null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backgroundworker encounter error", "Backgroundworker exited with error: " + e.Error.Message);
            }
        }
        private bool PopulateGrid()
        {
            dataGridView1.Rows.Clear();
            log = new string[] { textBox.Text };
            if (MassRadioButton.Checked)
            {
                try
                {
                    log = System.IO.File.ReadAllLines(textBox.Text);
                }
                catch (Exception exp)
                {
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Read Error", "Unable to read selected file with machine list." +
                        Environment.NewLine +
                        "Error: " + exp.Message);
                    return false;
                }
            }
            foreach (string line in log)
            {
                dataGridView1.Rows.Add(line.ToUpper(), "Waiting");
            }
            label1.Text = "0 / " + dataGridView1.Rows.Count;
            return true;
        }
        private void SlaveMaster()
        {
            if (masterRow < dataGridView1.Rows.Count && !cancel)
            {
                foreach (BackgroundWorker slave in slaveList)
                {
                    if (!slave.IsBusy)
                    {
                        lock (rowLock)
                        {
                            slave.RunWorkerAsync(masterRow);
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
            WrapUp();
        }//main logic that controls slaves work
        public void GridChange(int row, string status, Color rowColor)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = rowColor;
        } //set status and row color
        public void GridChange(int row, int cell, string status)
        {
            dataGridView1.Rows[row].Cells[cell].Value = status;
        } //set status of cell
        public void GridChange(int row, string status)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
        } //set status
        public void ErrorLog(int rownr, string errorMsg)
        {
            GridChange(rownr, "Error", Globals.errorColor);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + @"- [ERROR] - " + errorMsg;
            }
        }
        public void ErrorLog(int rownr, string host, string errorMsg)
        {
            GridChange(rownr, "Error", Globals.errorColor);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + @"- [ERROR] - " + errorMsg;
                Telemetry.LogMachineAction(host, Globals.Funkcje.Error, errorMsg);
            }
        }
        public void AddToLog(int rownr, string logThis)
        {
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "- " + logThis;
            }
        }
        private void WrapUp()
        {
            StartStopButton.Text = "Start";
            cancel = false;
            EnableUI();
            string logPath = @".\Logs\" + functionName + " " + Logger.Datownik() + ".txt";
            if (!FileController.SaveTxtToFile(logPath, string.Join(Environment.NewLine, log), out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save log file:" + Environment.NewLine + saveExp.Message);
                return;
            }
            try
            {
                System.IO.File.Delete(@".\Logs\Backup " + functionName + " log.txt");
            }
            catch { }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Finished", "Tool finished all tasks." + Environment.NewLine + "Log file created and saved as: " + Path.GetFullPath(logPath));
        } //fireup at the end of list or after abortion when all slaves done their 

        //-------FORM CONTROLS------------------
        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "Abort")
            {
                cancel = true;
                StartStopButton.Text = "Aborting...";
                StartStopButton.Enabled = false;
                try
                {
                    System.IO.File.WriteAllLines(@".\Logs\Backup " + functionName + " log.txt", log);
                }
                catch { }
                return;
            }

            if (textBox.Text == "")
            {
                textBox.BackColor = Globals.errorColor;
                return;
            }

            
            if(!PopulateGrid())
            {
                return;
            }

            functionName = FunctionSelectBox.SelectedItem.ToString();
            Type type = Type.GetType("TP_MasterTool.Klasy.MassFunctions");
            MethodInfo methodInfo = type.GetMethod("GetInfo_" + functionName);
            if (methodInfo != null)
            {
                additionalInfo = (List<string>)methodInfo.Invoke(this, null);
                if(additionalInfo == null)
                {
                    return;
                }
            }

            DisableUI();
            StartStopButton.Text = "Abort";
            masterRow = 0;
            iterator = 0;
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
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, dataGridView1.Rows[e.RowIndex].Cells[0].Value + " Log", log[e.RowIndex]);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
        }
        private void FetchTxtButton_Click(object sender, EventArgs e)
        {
            textBox.Text = FileController.OpenFileDialog("Text files (*.txt)|*.txt");
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
        private void EnableUI()//function fired up in wrapUp to enable UI elements anew
        {
            FetchTxtButton.Enabled = true;
            MassRadioButton.Enabled = true;
            SingleRadioButton.Enabled = true;
            textBox.Enabled = true;
            StartStopButton.Enabled = true;
            FunctionSelectBox.Enabled = true;
        }
        private void DisableUI()//function fired up after start to disable UI elements
        {
            FetchTxtButton.Enabled = false;
            MassRadioButton.Enabled = false;
            SingleRadioButton.Enabled = false;
            textBox.Enabled = false;
            FunctionSelectBox.Enabled = false;
        }

    }
}
