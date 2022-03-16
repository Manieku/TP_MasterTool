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
    //and to change name of output + backup log file in wrapUp function

    public partial class MassJPOSLogs : Form
    {
        List<BackgroundWorker> slaveList;
        int masterRow;
        readonly int bgwCount = 15;
        bool cancel = false;
        string[] log;
        readonly string backupLog = @".\Logs\MassJPOSBackupLog.txt";
        Logger logger = new Logger(Globals.Funkcje.MassJPOSLogs, "None", "");
        private readonly object rowLock = new object();
        private readonly object logLock = new object();

        public MassJPOSLogs()
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

            if (TicketNrTextBox.Text == "")
            {
                TicketNrTextBox.BackColor = Globals.errorColor;
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
                ErrorLog(rownr, "Unable to establish connection -> Invalid TAG");
                return;
            }
            else if (connectionPara.IP == "DNS ERROR")
            {
                ErrorLog(rownr, "Unable to establish connection -> DNS Error");
                return;
            }

            dataGridView1.Rows[rownr].Cells[1].Value = "Mapping drive";
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                ErrorLog(rownr, "Unable to map drive");
                return;
            }

            //here you enter your function to execution
            //----------------------- temp --------------------------------
            MassJposLogs(rownr, connectionPara);

        }


        private void MassJposLogs(int rownr, ConnectionPara connectionPara)
        {
            gridChange(rownr, "Looking for files");
            string[] files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService", "JPOSRFIDScannerLogs*");
            if (files.Length == 0)
            {
                ErrorLog(rownr, "No JPOS logs found");
                return;
            }

            if (System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs"))
            {
                gridChange(rownr, "Deleting old zip");
                try
                {
                    System.IO.Directory.Delete(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", true);
                }
                catch (Exception exp)
                {
                    ErrorLog(rownr, @"Can't delete folder with old logs -> Please delete D:\TPDotnet\DeviceService\JPOSLogs manually and try again -> " + exp.Message);
                    return;
                }
            }

            gridChange(rownr, "Gathering Logs");
            System.Threading.Thread.Sleep(150);
            try
            {
                System.IO.Directory.CreateDirectory(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs");
            }
            catch (Exception exp)
            {
                ErrorLog(rownr, "Unable to create folder for logs -> " + exp.Message);
                return;
            }

            foreach (string file in files)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(file, @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs\" + System.IO.Path.GetFileName(file));
                }
                catch (Exception exp)
                {
                    ErrorLog(rownr, "Unable to copy log: " + file + " -> " + exp.Message);
                    return;
                }
            }
            ZipAndStealFolder(rownr, "JPOSRFID_Logs", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", @"D:\TPDotnet\DeviceService\JPOSLogs", TicketNrTextBox.Text, connectionPara);
        }
        private void ZipAndStealFolder(int rownr, string prefix, string remotePath, string absolutePath, string tixnr, ConnectionPara connectionPara)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.ZipAndSteal, remotePath);
            if (!System.IO.Directory.Exists(remotePath))
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Folder not found");
                ErrorLog(rownr, "Log Folder not found");
                return;
            }

            string outputFolderName = prefix + " - " + tixnr + "(" + connectionPara.TAG + ") " + Logger.Datownik();

            gridChange(rownr, "Zipping Logs");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Xcopy /i /e /h /y """ + absolutePath + @""" ""D:\WNI\4GSS\" + tixnr + @"\Log"" && powershell -command ""Compress-Archive 'D:\WNI\4GSS\" + tixnr + @"\Log' 'D:\WNI\4GSS\" + tixnr + @"\" + outputFolderName + @".zip'"" && rmdir /s /q ""D:\WNI\4GSS\" + tixnr + @"\Log""");
            if (cmdOutput.exitCode != 0)
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "RCMD Encounter Problem");
                ErrorLog(rownr, "While executing cmd encounter error and exited with code: " + cmdOutput.exitCode.ToString());
                return;
            }

            gridChange(rownr, "Copying Logs");
            string grabFromPath = @"\\" + connectionPara.TAG + @"\d$\WNI\4GSS\" + tixnr;
            if (!System.IO.File.Exists(grabFromPath + @"\" + outputFolderName + @".zip"))
            {
                if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
                {
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to map disc second time");
                    ErrorLog(rownr, "Unable to map drive");
                    return;
                }
            }
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(grabFromPath + @"\" + outputFolderName + @".zip", Globals.userTempLogsPath + outputFolderName + @".zip", true);
            }
            catch (Exception exp)
            {
                ErrorLog(rownr, "Unable to copy log: " + grabFromPath + @"\" + outputFolderName + @".zip" + " -> " + exp.Message);
                return;
            }

            if (SingleRadioButton.Checked)
            {
                Process.Start("explorer.exe", grabFromPath);
            }
            lock (logLock)
            {
                log[rownr] += "[SUCCESS] Logs secured and copied to " + Globals.userTempLogsPath;
                Telemetry.LogFunctionUsage(Globals.Funkcje.ZipAndSteal);
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
                dataGridView1.Rows.Add(line.ToUpper(), "Waiting");
            }
            label1.Text = "0 / " + dataGridView1.Rows.Count;
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textBox.BackColor = SystemColors.Window;
        }
        private void TicketNrTextBox_TextChanged(object sender, EventArgs e)
        {
            TicketNrTextBox.BackColor = SystemColors.Window;
        }
        private void enableUI()//function fired up in wrapUp to enable UI elements anew
        {
            FetchTxtButton.Enabled = true;
            MassRadioButton.Enabled = true;
            SingleRadioButton.Enabled = true;
            TicketNrTextBox.Enabled = true;
            textBox.Enabled = true;
        }
        private void disableUI()//function fired up after start to disable UI elements
        {
            FetchTxtButton.Enabled = false;
            MassRadioButton.Enabled = false;
            SingleRadioButton.Enabled = false;
            TicketNrTextBox.Enabled = false;
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
            string logPath = @".\Logs\MassJPOSLog " + Logger.Datownik() + ".txt";
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
