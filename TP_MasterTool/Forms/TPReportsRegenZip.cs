using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output log file in wrapUp function

    public partial class TPReportsRegenZip : Form
    {
        List<BackgroundWorker> slaveList;
        int masterRow;
        readonly int bgwCount = 15;
        bool cancel = false;
        string[] log;
        string backupLog = @".\Logs\TPRegenZipBackupLog.txt";
        Logger logger = new Logger(Globals.Funkcje.TPReportsRegenZip, "None", "");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();

        public TPReportsRegenZip()
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

            if (!checkInput())
            {
                return;
            }
            disableall();

            if (SingleRadioButton.Checked)
            {
                log = new string[] { textBox.Text };
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(textBox.Text, "Waiting");
                label1.Text = "0 / 1";
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
            ConnectionPara connectionPara = ConnectionPara.EstablishConnection(dataGridView1.Rows[rownr].Cells[0].Value.ToString());
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

            if (regenerateCheckbox.Checked)
            {
                dataGridView1.Rows[rownr].Cells[1].Value = "Executing runeodreports.bat";
                string regenResult = CtrlFunctions.RegenerateEoDReports(connectionPara, beforeEOD.Text, dayofEOD.Text);
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.TPReportsRegenZip, "Regen");
                if (regenResult.StartsWith("[ERROR]"))
                {
                    gridChange(rownr, "Error", Globals.errorColor);
                    lock (logLock)
                    {
                        log[rownr] += " - " + Logger.LogTime() + regenResult;
                    }
                    return;
                }
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + regenResult;
                }
            }

            if (ZipCheckbox.Checked)
            {
                dataGridView1.Rows[rownr].Cells[1].Value = "Executing collect_tp_reports.ps1";
                string zipResult = CtrlFunctions.ZipEoDReports(connectionPara);
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.TPReportsRegenZip, "Zip");
                if (zipResult.StartsWith("[ERROR]"))
                {
                    gridChange(rownr, "Error", Globals.errorColor);
                    lock (logLock)
                    {
                        log[rownr] += " - " + Logger.LogTime() + zipResult;
                    }
                    return;
                }
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + zipResult;
                }
            }
            gridChange(rownr, "Done", Color.LightGreen);
            Telemetry.LogFunctionUsage(Globals.Funkcje.TPReportsRegenZip);
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
            textBox.Text = FileController.OpenFileDialog("Text files (*.txt)|*.txt");
            PopulateGrid();
        }
        private void PopulateGrid()
        {
            dataGridView1.Rows.Clear();
            if (SingleRadioButton.Checked)
            {
                dataGridView1.Rows.Clear();
                log = new string[] { textBox.Text };
                dataGridView1.Rows.Add(textBox.Text, "Waiting");
                return;
            }

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
                dataGridView1.Rows.Add(line, "Waiting");
            }
            label1.Text = "0 / " + dataGridView1.Rows.Count;
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
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
        }
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
            enableall();
            string logPath = @".\Logs\TPReportsRegenZip " + Logger.Datownik() + ".txt";
            if (FileController.SaveTxtToFile(logPath, string.Join(Environment.NewLine, log), ref logger))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Finished", "Tool finished all tasks." + Environment.NewLine + "Log file created and saved as: " + Path.GetFullPath(logPath));
            }
        }
        private bool checkInput()
        {
            bool result = true;
            if (textBox.Text == "")
            {
                textBox.BackColor = Globals.errorColor;
                result = false;
            }
            if (!regenerateCheckbox.Checked && !ZipCheckbox.Checked)
            {
                regenerateCheckbox.BackColor = Globals.errorColor;
                ZipCheckbox.BackColor = Globals.errorColor;
                result = false;
            }
            if (beforeEOD.Text.Length != 8 && regenerateCheckbox.Checked)
            {
                beforeEOD.BackColor = Globals.errorColor;
                result = false;
            }
            if (dayofEOD.Text.Length != 8 && regenerateCheckbox.Checked)
            {
                dayofEOD.BackColor = Globals.errorColor;
                result = false;
            }
            return result;
        }
        private void disableall()
        {
            regenerateCheckbox.Enabled = false;
            ZipCheckbox.Enabled = false;
            beforeEOD.Enabled = false;
            dayofEOD.Enabled = false;
        }
        private void enableall()
        {
            regenerateCheckbox.Enabled = true;
            ZipCheckbox.Enabled = true;
            beforeEOD.Enabled = true;
            dayofEOD.Enabled = true;
        }
        private void gridChange(int row, string status, Color rowColor)
        {
            dataGridView1.Rows[row].Cells[1].Value = status;
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = rowColor;
        }
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, dataGridView1.Rows[e.RowIndex].Cells[0].Value + " Log", log[e.RowIndex]);
        }
        private void TPReportsRegenZip_Shown(object sender, EventArgs e)
        {
            beforeEOD.Text = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
            dayofEOD.Text = DateTime.Today.ToString("yyyyMMdd");
        }
    }
}
