using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output + backup log file in wrapUp function

    public partial class MassEmergancy : Form
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
        readonly string backupLog = @".\Logs\MassTemplateBackupLog.txt";
        Logger logger = new Logger(Globals.Funkcje.MassEmergancy, "None", "");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();
        string[] functionList =
        {
            "EsfClient Restart",
            "EsfClient Reinit",
            "TP Process Manager Restart",
            "JPOS Logs Check",
            "Backup Jobs Check",
            "Backup Jobs Reset",
            "Delete Old Backup Files",
            "Get MAC",
            @"Check F:\ Drive",
            "Download JavaPos Logs"
        };

        public MassEmergancy()
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
            functionSelectList.DataSource = functionList;
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
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] Unable to map drive";
                }
                return;
            }

            string selection = functionSelectList.SelectedItem.ToString();
            if(selection == "EsfClient Restart")
            {
                EsfClientRestart(connectionPara, rownr);
            }
            else if(selection == "EsfClient Reinit")
            {
                EsfClientReinit(rownr, connectionPara);
            }
            else if(selection == "TP Process Manager Restart")
            {
                TpProcessManagerRestart(rownr, connectionPara);
            }
            else if(selection == "JPOS Logs Check")
            {
                JposError(rownr, connectionPara);
            }
            else if(selection == "Backup Jobs Check")
            {
                BackupJobsCheck(rownr, connectionPara);
            }
            else if(selection == "Backup Jobs Reset")
            {
                BackupJobsReset(rownr, connectionPara);
            }
            else if(selection == "Delete Old Backup Files")
            {
                DeleteOldBackupFiles(rownr, connectionPara);
            }
            else if(selection == "Get MAC")
            {
                GetMac(rownr, connectionPara);
            }
            else if(selection == @"Check F:\ Drive")
            {
                Check_F_Drive(rownr, connectionPara);
            }
            else if(selection == "Download JavaPos Logs")
            {
                DownloadJavaPosLog(rownr, connectionPara);
            }
            Telemetry.LogFunctionUsage(Globals.Funkcje.AdvRandomness);
            //here you enter your function to execution
            //----------------------- temp --------------------------------
            //DNFiskalRename(rownr, connectionPara);
        }

        private void EsfClientRestart(ConnectionPara connectionPara, int rownr)
        {
            gridChange(rownr, "Restarting Client");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c net stop esfclient && net start esfclient");
            if (cmdOutput.exitCode != 0)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] CMD exited with error code: " + cmdOutput.exitCode;
                }
                return;
            }
            gridChange(rownr, "Done", Color.LightGreen);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] ESF Client Restarted";
            }
        }
        private void EsfClientReinit(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Running script");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\service\agents\esfclient && reinit_esfclient.cmd");
            if (cmdOutput.exitCode != 0)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] CMD exited with error code: " + cmdOutput.exitCode;
                }
                return;
            }
            gridChange(rownr, "Done", Color.LightGreen);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] ESF Client Reinitialized";
            }

        }
        private void TpProcessManagerRestart(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Restarting Process");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net stop ""TPDotnet Process Manager"" && net start ""TPDotnet Process Manager""");
            if (cmdOutput.exitCode != 0)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] CMD exited with error code: " + cmdOutput.exitCode;
                }
                return;
            }
            gridChange(rownr, "Done", Color.LightGreen);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] TP Process Manager Restarted";
            }
        }
        private void JposError(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Looking for files");
            string[] files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService", "JPOSRFIDScannerLogs*");
            if (files.Length == 0)
            {
                ErrorLog(rownr, "No logs found");
                return;
            }
            gridChange(rownr, "Done", Color.LightGreen);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] Logs are present";
            }
        }
        private void BackupJobsCheck(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Looking for files");
            try
            {
                string[] cFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_C*.v2i");
                lock (logLock)
                {
                    log[rownr] += " " + cFiles.Length.ToString() + " ";
                }
            }
            catch
            {
                log[rownr] += "Error ";
            }

            try
            {
                string[] dFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_D*.v2i");
                lock (logLock)
                {
                    log[rownr] += dFiles.Length.ToString() + " ";
                }
            }
            catch
            {
                log[rownr] += "Error ";
            }
            gridChange(rownr, "Done", Color.LightGreen);
        }
        private void BackupJobsReset(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Executing commands");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c powershell -ep Bypass c:\service\tools\backup\RemoveImageJob.ps1 && powershell -ep Bypass c:\service\tools\backup\AddImageJob.ps1");
            if (cmdOutput.exitCode != 0)
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] CMD Exited with error code: " + cmdOutput.exitCode;
                }
                return;
            }
            gridChange(rownr, "Done", Globals.successColor);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] Veritas jobs has been reset";
            }

        }
        private void DeleteOldBackupFiles(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Looking for files");
            try
            {
                string[] cFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_C*.v2i");
                foreach (string cFile in cFiles)
                {
                    if (System.IO.File.GetCreationTime(cFile).Day < DateTime.Today.AddDays(-1).Day)
                    {
                        lock (logLock)
                        {
                            log[rownr] += "," + cFile + " -> " + File.GetCreationTime(cFile);
                        }
                        File.Delete(cFile);
                    }
                }
            }
            catch
            {
                log[rownr] += ",Error";
            }
            try
            {
                string[] cFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_D*.v2i");
                foreach (string cFile in cFiles)
                {
                    if (System.IO.File.GetCreationTime(cFile).Day < DateTime.Today.AddDays(-1).Day)
                    {
                        lock (logLock)
                        {
                            log[rownr] += "," + cFile + " -> " + File.GetCreationTime(cFile);
                        }
                        File.Delete(cFile);
                    }
                }
            }
            catch
            {
                log[rownr] += ",Error";
            }
            gridChange(rownr, "Done", Globals.successColor);
        }
        private void GetMac(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Stealing MAC");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
            string output = cmdOutput.outputText.Replace("\n", "").Replace("\r", "").ToUpper();


            lock (logLock)
            {
                log[rownr] += "," + output;
            }
            gridChange(rownr, "Done", Globals.successColor);
        }
        private void Check_F_Drive(int rownr, ConnectionPara connectionPara)
        {
            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\f$\Backup"))
            {
                gridChange(rownr, "Error", Globals.errorColor);
                lock (logLock)
                {
                    log[rownr] += " - " + Logger.LogTime() + "[ERROR] Unable to read F Drive";
                }
                return;
            }
            gridChange(rownr, "Done", Globals.successColor);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + "[SUCCESS] F Drive Available";
            }

        }
        private void DNFiskalRename(int rownr, ConnectionPara connectionPara)
        {
            string[] xmlFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\c$\DNFiscalAdapter\work", "*AB120001*.xml");
            foreach (string xmlFile in xmlFiles)
            {
                string zawartosc = System.IO.File.ReadAllText(xmlFile);
                System.IO.File.WriteAllText(xmlFile, zawartosc.Replace("AB120001", "08070003"));
                System.IO.File.Move(xmlFile, xmlFile.Replace("AB120001", "08070003"));
            }
        }
        private void DownloadJavaPosLog(int rownr, ConnectionPara connectionPara)
        {
            string[] files = null;
            try
            {
                files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\c$\ProgramData\javapos\wn\log", "javapos.log*");
            }
            catch
            {
                ErrorLog(rownr, "Failed to get files");
                return;
            }
            if(files.Length == 0)
            {
                ErrorLog(rownr, "No logs found");
                return;
            }
            int i = 1;
            foreach(string file in files)
            {
                try
                {
                    gridChange(rownr, "Copying " + i + " of " + files.Length + " files");
                    System.IO.Directory.CreateDirectory(@".\Logs\JavaPosLogs\" + connectionPara.TAG);
                    System.IO.File.Copy(file, @".\Logs\JavaPosLogs\" + connectionPara.TAG + @"\" + Path.GetFileName(file));
                    i++;
                }
                catch
                {
                    ErrorLog(rownr, "Unable to copy file " + file);
                    return;
                }
            }
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
            functionSelectList.Enabled = true;
            SingleRadioButton.Enabled = true;
            MassRadioButton.Enabled = true;
            FetchTxtButton.Enabled = true;
            textBox.Enabled = true;
        }
        private void disableUI()//function fired up after start to disable UI elements
        {
            functionSelectList.Enabled = false;
            SingleRadioButton.Enabled = false;
            MassRadioButton.Enabled = false;
            FetchTxtButton.Enabled = false;
            textBox.Enabled = false;
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
        private void ErrorLog(int rownr, string errorMsg)
        {
            gridChange(rownr, "Error", Globals.errorColor);
            lock (logLock)
            {
                log[rownr] += " - " + Logger.LogTime() + @"[ERROR] " + errorMsg;
            }
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
            enableUI();
            string logPath = @".\Logs\MassTemplateLog " + Logger.Datownik() + ".txt";
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
