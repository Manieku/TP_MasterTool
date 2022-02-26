using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

    public partial class KBinstalation : Form
    {
        List<BackgroundWorker> slaveList;
        int masterRow;
        readonly int bgwCount = 20;
        bool cancel = false;
        string[] log;
        Logger logger = new Logger(Globals.Funkcje.Blank, "none", "none");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();

        public KBinstalation()
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
            //here you enter your function to execution
            int rownr = (int)e.Argument;
            dataGridView1.Rows[rownr].Cells[1].Value = "Connecting";
            ConnectionPara connectionPara = ConnectionPara.EstablishConnectionSilent(dataGridView1.Rows[rownr].Cells[0].Value.ToString());
            if (connectionPara == null)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[ERROR],Unable to establish connection -> Invalid TAG";
                }
                return;
            }
            else if (connectionPara.IP == "DNS ERROR")
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[ERROR],Unable to establish connection -> DNS Error";
                }
                return;
            }

            dataGridView1.Rows[rownr].Cells[1].Value = "Mapping drive";
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[ERROR],Unable to map drive";
                }
                return;
            }

            if (kbCheckRadio.Checked)
            {
                KbCheck(rownr, connectionPara, KBCheckTextBox.Text);
            }
            else if (copyExecuteRadio.Checked)
            {
                DeployAndExecute(rownr, connectionPara, copyExecuteTextBox.Text.Split(','), waitCheckBox.Checked);
            }
            else if (executeRadio.Checked)
            {
                Execute(rownr, connectionPara, executeTextBox.Text.Split(','), waitCheckBox.Checked);
            }
            else if (dismSfcRadio.Checked)
            {
                DismAndSFC(rownr, connectionPara);
            }

        }

        private void KbCheck(int rownr, ConnectionPara connectionPara, string kbId)
        {
            dataGridView1.Rows[rownr].Cells[1].Value = "Checking for KB";
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-HotFix -Id " + kbId + "\"");
            if (cmdOutput.exitCode != 0)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[ERROR],Not installed";
                }
                return;
            }
            gridChange(rownr, "Done", Color.LightGreen);
            lock (logLock)
            {
                log[rownr] += "," + Logger.LogTime() + ",[SUCCESS],Installed";
                Telemetry.LogFunctionUsage(Globals.Funkcje.ADVKbCheck);
            }
        }
        private void DismAndSFC(int rownr, ConnectionPara connectionPara)
        {
            dataGridView1.Rows[rownr].Cells[1].Value = "Executing commands";
            try
            {
                runCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Dism /Online /Cleanup-Image /RestoreHealth && sfc /scannow", false);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[SUCCESS],Commands started";
                    Telemetry.LogFunctionUsage(Globals.Funkcje.ADVDismSfc);
                }
                gridChange(rownr, "Done", Color.LightGreen);
            }
            catch (Exception exp)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[ERROR],RCMD encountered error: " + exp.Message;
                }
            }
        }
        private void DeployAndExecute(int rownr, ConnectionPara connectionPara, string[] cmds, bool wait4exit)
        {
            foreach (string cmdFile in cmds)
            {
                dataGridView1.Rows[rownr].Cells[1].Value = "Copying " + cmdFile;
                try
                {
                    System.IO.File.Copy(Globals.toolsPath + cmdFile, @"\\" + connectionPara.TAG + @"\c$\temp\" + cmdFile, true);
                }
                catch (Exception exp)
                {
                    gridChange(rownr, "Error", Globals.errorColor);
                    lock (logLock)
                    {
                        log[rownr] += "," + Logger.LogTime() + ",[ERROR],Unable to copy " + cmdFile + "," + exp.Message;
                    }
                    return;
                }
            }

            foreach (string cmdFile in cmds)
            {
                dataGridView1.Rows[rownr].Cells[1].Value = "Executing " + cmdFile;
                try
                {
                    runCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c C:\temp\" + cmdFile, wait4exit);
                }
                catch (Exception exp)
                {
                    gridChange(rownr, "Error", Globals.errorColor);
                    lock (logLock)
                    {
                        log[rownr] += "," + Logger.LogTime() + ",[ERROR],Executing " + cmdFile + " failed," + exp.Message;
                    }
                    return;
                }

                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[SUCCESS]," + cmdFile + " executed";
                    Telemetry.LogFunctionUsage(Globals.Funkcje.ADVExecute);
                }
            }
            gridChange(rownr, "Done", Color.LightGreen);
        }
        private void Execute(int rownr, ConnectionPara connectionPara, string[] cmds, bool wait4exit)
        {
            foreach (string cmdFile in cmds)
            {
                dataGridView1.Rows[rownr].Cells[1].Value = "Executing " + cmdFile;
                try
                {
                    runCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c C:\temp\" + cmdFile, wait4exit);
                }
                catch (Exception exp)
                {
                    gridChange(rownr, "Error", Globals.errorColor);
                    lock (logLock)
                    {
                        log[rownr] += "," + Logger.LogTime() + ",[ERROR],Executing " + cmdFile + " failed," + exp.Message;
                    }
                    return;
                }

                lock (logLock)
                {
                    log[rownr] += "," + Logger.LogTime() + ",[SUCCESS]," + cmdFile + " executed";
                    Telemetry.LogFunctionUsage(Globals.Funkcje.ADVExecute);
                }
            }
            gridChange(rownr, "Done", Color.LightGreen);

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
                dataGridView1.Rows.Add(line, "Waiting");
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
        }
        private void enableUI()//function fired up in wrapUp to enable UI elements anew
        {
            panel1.Enabled = true;
            kbCheckRadio.Enabled = true;
            copyExecuteRadio.Enabled = true;
            executeRadio.Enabled = true;
            dismSfcRadio.Enabled = true;
            waitCheckBox.Enabled = true;
            KBCheckTextBox.Enabled = true;
            copyExecuteTextBox.Enabled = true;
            executeTextBox.Enabled = true;

        }
        private void disableUI()//function fired up after start to disable UI elements
        {
            panel1.Enabled = false;
            kbCheckRadio.Enabled = false;
            copyExecuteRadio.Enabled = false;
            executeRadio.Enabled = false;
            dismSfcRadio.Enabled = false;
            waitCheckBox.Enabled = false;
            KBCheckTextBox.Enabled = false;
            copyExecuteTextBox.Enabled = false;
            executeTextBox.Enabled = false;
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
        private void wrapUp()
        {
            StartStopButton.Text = "Start";
            StartStopButton.Enabled = true;
            cancel = false;
            enableUI();
            string logPath = @".\Logs\MassLogKBinstall " + Logger.Datownik() + ".csv";
            if (FileController.SaveTxtToFile(logPath, string.Join(Environment.NewLine, log), ref logger))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Finished", "Tool finished all tasks." + Environment.NewLine + "Log file created and saved as: " + Path.GetFullPath(logPath));
            }
        } //fireup at the end of list or after abortion when all slaves done their 
        private void runCmd(string exe, string command, bool wait4Exit)
        {
            Process p = new Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            if (wait4Exit)
            {
                p.WaitForExit();
            }
        }

    }
}
