using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    //Please don't forget to use logLock when you edit main log array 
    //and to change name of output + backup log file in wrapUp function

    public partial class MonitoringAnalizer : Form
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
        Logger logger = new Logger(Globals.Funkcje.MonitoringSlayer, "None", "");
        private readonly object rowLock = new object();
        int tixNr;
        int status;
        int summary;
        int tag;
        int tixSource;
        string logsPath;
        private readonly object logLock = new object();


        public MonitoringAnalizer()
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

            if (!PopulateGrid())
            {
                return;
            }
            logsPath = @"D:\C&A 2lvl\MonitoringSlayer";
            logger.Add("Creating folder: " + logsPath);
            if (!FileController.MakeFolder(logsPath, out Exception makeExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Execution Error", @"ToolBox was unable to create output folder at D:\C&A 2lvl\MonitoringSlayer" + Environment.NewLine + makeExp.Message);
                logger.Add(makeExp.ToString());
                logger.SaveLog("ErrorLog");
                return;
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
            string thisLogPath = logsPath + @"\" + dataGridView1.Rows[rownr].Cells[0].Value + " - " + dataGridView1.Rows[rownr].Cells[1].Value + ".txt";
            dataGridView1.Rows[rownr].Cells[3].Value = "Looking for a log file";
            if (System.IO.File.Exists(thisLogPath))
            {
                gridChange(rownr, "Ticket analyzed before. Please check log file", Color.LightYellow);
                return;
            }

            try
            {
                System.IO.File.Create(thisLogPath).Close();
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, dataGridView1.Rows[rownr].Cells[1] + " - Log Creation Error", "ToolBox wasn't able to create log file for this station. Error:\n" + exp.Message);
                gridChange(rownr, "Log Creation Error", Globals.errorColor);
                return;
            }
            ConnectionPara connectionPara = ConnectionPara.EstablishConnection((string)dataGridView1.Rows[rownr].Cells[1].Value);
            if (connectionPara == null)
            {
                gridChange(rownr, "Invalid TAG", Globals.errorColor);
                try
                {
                    System.IO.File.Delete(thisLogPath);
                }
                catch { }
                return;
            }

            string output = Logger.LogTime() + "Starting analysis of:" + Environment.NewLine + dataGridView1.Rows[rownr].Cells[0].Value + " - " + dataGridView1.Rows[rownr].Cells[2].Value + Environment.NewLine + Environment.NewLine + "Looking for procedure" + Environment.NewLine;

            //-------checking what monitoring-------
            if ((string)dataGridView1.Rows[rownr].Cells[2].Value == dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] System Connection lost since 1 hour")
            {
                output += AddToLog("Found procedure in database -> Host_Offline_1h");
                HostOffline1h(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "System Connection lost since 1 hour");
            }
            else if ((string)dataGridView1.Rows[rownr].Cells[2].Value == dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] System-TS_ERR_BUILD_ARCHIVE_FAILURE")
            {
                output += AddToLog("Found procedure in database -> Archive_Build_Failure");
                ArchiveBuildFailure(rownr, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "System-TS_ERR_BUILD_ARCHIVE_FAILURE");
            }
            else if ((string)dataGridView1.Rows[rownr].Cells[2].Value == dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] The service TPDotnet Process Manager is down")
            {
                output += AddToLog("Found procedure in database -> TPDotnet_Process_Manager_Down");
                TPDotnetProcessManagerDown(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "The service TPDotnet Process Manager is down");
            }
            else if (dataGridView1.Rows[rownr].Cells[2].Value.ToString().StartsWith(dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] Minilogger no. ") && dataGridView1.Rows[rownr].Cells[2].Value.ToString().Contains(" is Offline"))
            {
                output += AddToLog("Found procedure in database -> CUC_Offline");
                CUCOffline(rownr, dataGridView1.Rows[rownr].Cells[2].Value.ToString().Substring(44, 1), connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "Minilogger no. " + dataGridView1.Rows[rownr].Cells[2].Value.ToString().Substring(44, 1) + " is Offline");
            }
            else if ((string)dataGridView1.Rows[rownr].Cells[2].Value == dataGridView1.Rows[rownr].Cells[1].Value + @";[Canda OmniPOS] Msg from InfoDaemon drive C:\ state has been changed to critical")
            {
                output += AddToLog("Found procedure in database -> C_Drive_Critical");
                CDriveCritical(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, @"Msg from InfoDaemon drive C:\ state has been changed to critical");
            }
            else if ((string)dataGridView1.Rows[rownr].Cells[2].Value == dataGridView1.Rows[rownr].Cells[1].Value + @";[Canda OmniPOS] *TPNAPP* Central Collection Process for TP.Report failed")
            {
                output += AddToLog("Found procedure in database -> CollectionFailed");
                CollectionFailed(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "*TPNAPP* Central Collection Process for TP.Report failed");
            }
            else if (dataGridView1.Rows[rownr].Cells[2].Value.ToString().StartsWith(dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] Missing TA") && dataGridView1.Rows[rownr].Cells[2].Value.ToString().EndsWith("found on " + dataGridView1.Rows[rownr].Cells[1].Value + " (Source: TX Collector)"))
            {
                output += AddToLog("Found procedure in database -> MissingTA");
                MissingTA(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "Missing TA");
            }
            else if (dataGridView1.Rows[rownr].Cells[2].Value.ToString().StartsWith(dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] *TPNAPP* MANUALEOD Failure at"))
            {
                output += AddToLog("Found procedure in database -> EoD_Failed");
                EoDFailed(rownr, connectionPara, ref output);
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.MonitoringSlayer, "*TPNAPP* MANUALEOD Failure");
            }
            //else if (dataGridView1.Rows[rownr].Cells[2].Value.ToString().StartsWith(dataGridView1.Rows[rownr].Cells[1].Value + ";[Canda OmniPOS] *TPNAPP* MANUALEOD Aborted at"))
            //{
            //    output += AddToLog("Found procedure in database -> EoD_Aborted_Test");
            //    EoDAbortTest(rownr, connectionPara, ref output);
            //    gridChange(rownr, "No procedure found for this issue", Globals.errorColor);
            //    try
            //    {
            //        System.IO.File.AppendAllText(@"U:\mrzemyk\EoD_Abort_Test\" + dataGridView1.Rows[rownr].Cells[0].Value + " - " + dataGridView1.Rows[rownr].Cells[1].Value + ".txt", output);
            //        System.IO.File.Delete(thisLogPath);
            //    }
            //    catch { }
            //    return;
            //}
            else
            {
                gridChange(rownr, "No procedure found for this issue", Globals.errorColor);
                try
                {
                    System.IO.File.Delete(thisLogPath);
                }
                catch { }
                return;
            }

            if(!FileController.SaveTxtToFile(thisLogPath, output, out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file " + thisLogPath + Environment.NewLine + saveExp.Message);
                return;
            }
            return;
        }
        private void slave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.QuickLog(Globals.Funkcje.MonitoringSlayer, "Backgroundworker", "", "ErrorLog", e.Error.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backgroundworker encounter error", "Backgroundworker exited with error: " + e.Error.ToString());
            }
            slaveMaster();
        }
        private void FetchTxtButton_Click(object sender, EventArgs e)
        {
            textBox.Text = FileController.OpenFileDialog("TXT files (*.txt)|*.txt");
        }
        private bool PopulateGrid()
        {
            dataGridView1.Rows.Clear();
            string[] ticketList;
            try
            {
                ticketList = System.IO.File.ReadAllLines(textBox.Text);
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Read Error", "Unable to read selected file with ticket list." +
                    Environment.NewLine +
                    "Error: " + exp.Message);
                return false;
            }

            if (!GetIndexes(ticketList, out tixNr, out status, out summary, out tag, out tixSource))
            {
                return false;
            }

            foreach (string templine in ticketList)
            {
                string[] line = templine.Split('\t');
                if (line[status] == "Assigned" && line[tixSource] == "HTTP")
                {
                    dataGridView1.Rows.Add(line[tixNr], line[tag], line[summary].Replace("\"", ""), "Waiting");
                }
            }
            label1.Text = "0 / " + dataGridView1.Rows.Count;
            return true;
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
        private void gridChange(int row, string status)
        {
            dataGridView1.Rows[row].Cells[3].Value = status;
        } //set status
        private void gridChange(int row, string status, Color rowColor)
        {
            dataGridView1.Rows[row].Cells[3].Value = status;
            dataGridView1.Rows[row].DefaultCellStyle.BackColor = rowColor;
        } //set status and row color
        private void wrapUp()
        {
            label1.Text = dataGridView1.Rows.Count + " / " + dataGridView1.Rows.Count;
            StartStopButton.Text = "Start";
            StartStopButton.Enabled = true;
            cancel = false;
            enableUI();
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Slayer finished analyzing tickets", @"All analyzed tickets logs are saved in D:\C&A 2lvl\MonitoringSlayer");
        } //fireup at the end of list or after abortion when all slaves done their 
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Process.Start(@"D:\C&A 2lvl\MonitoringSlayer\" + dataGridView1.Rows[e.RowIndex].Cells[0].Value + " - " + dataGridView1.Rows[e.RowIndex].Cells[1].Value + ".txt");
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error Opening Log", "Unable to open log file:" + Environment.NewLine + exp.Message);
            }
        }
        private string AddToLog(string logThis)
        {
            return logThis + Environment.NewLine;
        }
        //------PROD FUNCTIONS------------
        private void HostOffline1h(int rownr, ConnectionPara connectionPara, ref string log)
        {
            gridChange(rownr, "Pinging TAG");
            log += AddToLog("Is host back online?:");
            try
            {
                if (new Ping().Send(connectionPara.TAG, 4000).Status == IPStatus.Success)
                {
                    log += AddToLog("-> Yes");
                    log += AddToLog(">>> Ticket can be close if issue is not reoccurring, please check history <<<");
                    log += AddToLog("");
                    log += AddToLog(">> Notes for ticket:");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + connectionPara.TAG);
                    log += "Machine is back online:" + cmdOutput.outputText;
                    gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
                    return;
                }
            }
            catch { }
            log += AddToLog("-> No");
            log += AddToLog("Checking DNS");
            log += AddToLog("-> IP: " + connectionPara.IP);
            if (connectionPara.IP == "DNS ERROR")
            {
                DnsRestore(rownr, connectionPara, ref log);
                return;
            }
            log += AddToLog("Pinging IP");
            gridChange(rownr, "Pinging IP");
            try
            {
                if (new Ping().Send(connectionPara.IP, 4000).Status == IPStatus.Success)
                {
                    log += AddToLog("IP Pingable");
                    log += AddToLog("Host is reachable via IP but not TAG without DNS error");
                    log += AddToLog(">>> Ticket needs manual investigation <<<");
                    gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                    return;
                }
            }
            catch { }

            log += AddToLog("IP also not pingable");
            log += AddToLog("Host is still offline");
            CtrlFunctions.CmdOutput cmdOutput3 = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + connectionPara.TAG);
            log += AddToLog(cmdOutput3.outputText);
            log += AddToLog(">>> Create task for L1 with note below and close L2 task <<<");
            log += AddToLog("");
            log += AddToLog(">> Notes for ticket:");
            log += AddToLog("@L1" + Environment.NewLine + "Hello, " + connectionPara.TAG + " is offline. Please check why and perform standard troubleshooting for offline devices.");
            gridChange(rownr, "Task ready to be close. See log.", Color.LightGreen);
        }
        private void ArchiveBuildFailure(int rownr, ref string log)
        {
            log += AddToLog(">>> Relate Problem ticket 83765659 and close this ticket with note below <<<");
            log += AddToLog(Environment.NewLine + ">> Notes for ticket:");
            log += AddToLog("Issue related to PT 83765659");
            gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
            return;
        }
        private void TPDotnetProcessManagerDown(int rownr, ConnectionPara connectionPara, ref string log)
        {
            log += AddToLog("Starting TPDotnet Process Manager");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net start ""TPDotnet Process Manager""");
            if(cmdOutput.exitCode != 0)
            {
                log += AddToLog("-> Failed");
                log += AddToLog(cmdOutput.errorOutputText);
                log += AddToLog(Environment.NewLine + ">>> Unable to start TPDotnet Process Manager, please investigate <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("-> Done");
            log += AddToLog(Environment.NewLine + ">>> TPDotnet Process Manager started, ticket should auto close if not please restart MobilePOS and close ticket <<<");
            gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
        }
        private void CUCOffline(int rownr, string cucNr, ConnectionPara connectionPara, ref string log)
        {
            gridChange(rownr, "Reading CUC number");
            log += AddToLog("Reading CUC number");
            int cucNrInt;
            try
            {
                cucNrInt = int.Parse(cucNr);
                log += AddToLog("-> " + cucNrInt.ToString());
            }
            catch
            {
                log += AddToLog("-> Unable to read CUC number");
                log += AddToLog(">>> Please check if ticket summary is default and unchanged <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            gridChange(rownr, "Pinging CUC");
            log += AddToLog("Is CUC back online?:");
            try
            {
                if (new Ping().Send(connectionPara.country + connectionPara.storeNr + "CUC0" + cucNr, 4000).Status == IPStatus.Success)
                {
                    log += AddToLog("-> Yes");
                    log += AddToLog(">>> Ticket can be close <<<" + Environment.NewLine);
                    log += AddToLog(">> Notes for ticket:");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + connectionPara.country + connectionPara.storeNr + "CUC0" + cucNr);
                    log += "Minilogger is back online:" + cmdOutput.outputText;
                    gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
                    return;
                }
            }
            catch { }
            log += AddToLog("-> No");
            log += AddToLog("CUC is still offline");
            CtrlFunctions.CmdOutput cmdOutput2 = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + connectionPara.country + connectionPara.storeNr + "CUC0" + cucNr);
            log += AddToLog(cmdOutput2.outputText);
            log += AddToLog(">>> Create task for L1 with note below and close L2 task <<<" + Environment.NewLine);
            log += AddToLog(">> Notes for ticket:");
            log += AddToLog("@L1" + Environment.NewLine + "Hello, CUC" + cucNr + " is offline. Please contact the store and ask them to restart affected minilogger.");
            log += AddToLog(Environment.NewLine + ">> Additional information:");
            log += AddToLog(@"-> c$\oeminst\ALL_LOGS\Monitoring\minilogger.csv");
            gridChange(rownr, "Reading monitoring log");
            if (CtrlFunctions.MapEndpointDrive(ref connectionPara, out _))
            {
                try
                {
                    log += AddToLog(System.IO.File.ReadAllText(@"\\" + connectionPara.TAG + @"\c$\oeminst\ALL_LOGS\Monitoring\minilogger.csv"));
                }
                catch
                {
                    log += AddToLog("Unable to read monitoring log");
                }
            }
            else
            {
                log += AddToLog("Unable to read monitoring log");
            }
            gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
        }
        private void CDriveCritical(int rownr, ConnectionPara connectionPara, ref string log)
        {
            if(!ConnectToHost(rownr, connectionPara, ref log))
            {
                return;
            }

            gridChange(rownr, "Clearing drive");
            log += AddToLog(@"Clear C:\Widows\Temp:");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\Temp", false);
            log += AddToLog("-> Executed");
            log += AddToLog(@"Clear C:\Widows\CbsTemp:");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\CbsTemp", false);
            log += AddToLog("-> Executed");
            log += AddToLog(@"Clear C:\Widows\WinSxS:");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\WinSxS", false);
            log += AddToLog("-> Executed");

            log += AddToLog(Environment.NewLine + ">>> Ticket should autoclose in ~15min, if not create task for ADV with this log and current disk space info <<<");
            gridChange(rownr, "Ticket needs action from agent. See log", Color.LightYellow);
        }
        private void CollectionFailed(int rownr, ConnectionPara connectionPara, ref string log)
        {
            if (!ConnectToHost(rownr, connectionPara, ref log))
            {
                return;
            }

            gridChange(rownr, "Checking files");
            log += AddToLog("Checking for zip in output folder:");
            if(System.IO.File.Exists(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip"))
            {
                log += AddToLog("-> Present | Ready to be picked up after next successful run");
                CollectionFailedAutoClosure(connectionPara, ref log, "Local Reports zip was present in output folder - no action needed");

                log += AddToLog(Environment.NewLine + ">>> Ticket should auto close, if not close it manually <<<");
                gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
                return;
            }

            log += AddToLog("-> Missing");
            log += AddToLog("Checking for backup zip in ArchivedReports folder:");
            string[] searchResult = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\ArchivedReports", "collect_tp_reports.zip." + DateTime.Today.ToString("yyyyMMdd") + "*");
            if(searchResult.Length > 1)
            {
                log += AddToLog("-> Found more than one backup zip");
                log += AddToLog(Environment.NewLine + ">>> Please verify proper backup zip in ArchivedReports and copy it to dms_output folder as collect_tp_reports.zip and close the ticket <<<");
                gridChange(rownr, "Ticket require action from agent. See log.", Color.LightYellow);
                return;
            }

            if(searchResult.Length == 1)
            {
                log += AddToLog("-> Found: " + System.IO.Path.GetFileName(searchResult[0]));
                log += AddToLog("Copying backup zip into dms_output folder:");
                if(!FileController.CopyFile(searchResult[0], @"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip", false, out Exception copyExp))
                {
                    log += AddToLog("-> Error: " + copyExp.Message);
                    log += AddToLog(Environment.NewLine + ">>> Unable to copy backup zip | Please copy backup zip from ArchivedReports to dms_output folder as collect_tp_reports.zip and close the ticket <<<");
                    gridChange(rownr, "Ticket require action from agent. See log.", Color.LightYellow);
                    return;
                }
                log += AddToLog("-> Done");
                CollectionFailedAutoClosure(connectionPara, ref log, "Local Reports zip was copied from ArchivedReports (" + System.IO.Path.GetFileName(searchResult[0]) + ") to dms_output folder");

                log += AddToLog(Environment.NewLine + ">>> Ticket should auto close, if not close it manually <<<");
                gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
                return;
            }

            log += AddToLog("-> Missing | Regenerating reports");
            int offset = 0;
            if (connectionPara.country == "BE" || connectionPara.country == "LU")
            {
                offset = -1;
            }
            gridChange(rownr, "Regenerating reports");
            log += AddToLog("Executing runeodreports.bat " + DateTime.Today.AddDays(offset - 1).ToString("yyyyMMdd") + " - " + DateTime.Today.AddDays(offset).ToString("yyyyMMdd"));
            if(!CtrlFunctions.RegenerateEoDReports(connectionPara, DateTime.Today.AddDays(offset - 1).ToString("yyyyMMdd"), DateTime.Today.AddDays(offset).ToString("yyyyMMdd"), out string regenOutput))
            {
                log += AddToLog("-> " + Logger.LogTime() + "- [ERROR] " + regenOutput);
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                log += AddToLog(Environment.NewLine + ">>> Error while executing runeodreports.bat. Please try running it manually and check what and if error occurs <<<");
                return;
            }
            log += AddToLog("-> " + Logger.LogTime() + "- [SUCCESS] " + regenOutput);

            gridChange(rownr, "Zipping reports");
            log += AddToLog("Executing collect_tp_reports.ps1");
            if(!CtrlFunctions.ZipEoDReports(connectionPara, out string zipOutput))
            {
                log += AddToLog("-> " + Logger.LogTime() + "- " + zipOutput);
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                log += AddToLog(Environment.NewLine + ">>> Error while executing collect_tp_reports.ps1. Please try running it manually and check what and if error occurs <<<");
                return;
            }
            log += AddToLog("-> " + Logger.LogTime() + "- " + zipOutput);
            CollectionFailedAutoClosure(connectionPara, ref log, "Local Reports were recreated and collected manually by script on " + DateTime.Now.ToString("d/MM/yyyy HH:mm:ss"));

            log += AddToLog(Environment.NewLine + ">>> Ticket should auto close, if not close with note below <<<");
            log += AddToLog("");
            log += AddToLog(">> Notes for ticket:");
            log += AddToLog(regenOutput + " | " + zipOutput);
            log += AddToLog("Ready to be picked up after next successful run");
            gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
        }
        private void MissingTA(int rownr, ConnectionPara connectionPara, ref string log)
        {
            if (!ConnectToHost(rownr, connectionPara, ref log))
            {
                return;
            }

            gridChange(rownr, "Connecting to " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"));
            log += AddToLog("Connecting to " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"));
            ConnectionPara connectionParaTPS = ConnectionPara.EstablishConnection(String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"));
            if (connectionParaTPS == null)
            {
                log += AddToLog("-> Unable to establish connection");
                log += AddToLog(Environment.NewLine + ">>> Please check if host is online and credentials are working <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            if (!CtrlFunctions.MapEndpointDrive(ref connectionParaTPS, out CtrlFunctions.CmdOutput cmdOutputTPS))
            {
                log += AddToLog("-> Unable to map drive: " + cmdOutputTPS.errorOutputText);
                log += AddToLog(Environment.NewLine + ">>> Please check if host is online and credentials are working <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("-> Done");

            log += AddToLog("Reading TA numbers");
            string[] txnrs = dataGridView1.Rows[rownr].Cells[2].Value.ToString().Remove(dataGridView1.Rows[rownr].Cells[2].Value.ToString().Length - 59).Remove(0, 41).Split(',');
            log += AddToLog("-> " + String.Join(",", txnrs));
            foreach (string txnr in txnrs)
            {
                gridChange(rownr, "Gathering data for TA " + txnr.Trim() + " from " + connectionPara.TAG);
                log += AddToLog(">> Gathering data for TA " + txnr.Trim() + " from " + connectionPara.TAG);
                LookForTA(txnr.Trim(), @"\\" + connectionPara.TAG + @"\d$\TPDotnet\Pos\Transactions", "*_" + txnr + "_*.xml", ref log);
                LookForTA(txnr.Trim(), @"\\" + connectionPara.TAG + @"\d$\TPDotnet\Pos\Transactions\Parked", "*.xml", ref log);
                LookForTA(txnr.Trim(), @"\\" + connectionPara.TAG + @"\d$\WNI", "*.xml", ref log);

                string tillNr = int.Parse(connectionPara.deviceNr).ToString();
                gridChange(rownr, "Gathering data for TA " + txnr.Trim() + " from " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"));
                log += AddToLog(Environment.NewLine + ">> Gathering data for TA " + txnr.Trim() + " from " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"));
                LookForTA(txnr.Trim(), @"\\" + connectionParaTPS.TAG + @"\d$\TPDotnet\Server\Transactions", "*_" + tillNr + "_" + txnr + "_*.xml", ref log);
                LookForTA(txnr.Trim(), @"\\" + connectionParaTPS.TAG + @"\d$\WNI", "*.xml", ref log);
            }

            gridChange(rownr, "Data gathered, proceed with investigation", Color.LightGreen);
            log += AddToLog(Environment.NewLine + ">>> Data gathered, proceed with investigation <<<");
        }
        private void EoDFailed(int rownr, ConnectionPara connectionPara, ref string log)
        {
            if (!ConnectToHost(rownr, connectionPara, ref log))
            {
                return;
            }

            gridChange(rownr, "Searching last EoD Log");
            log += AddToLog("Searching last EoD Log");
            System.IO.FileInfo eodLog = GetLastEodLog(connectionPara);
            if (eodLog == null)
            {
                log += AddToLog("-> Unable to find EoD log xml");
                log += AddToLog(Environment.NewLine + ">>> Please check if host is online, logs are present and credentials are working <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("-> " + eodLog.FullName);
            gridChange(rownr, "Reading EoD Log");
            log += AddToLog("Load EoD Log");
            XDocument eodXml;
            try
            {
                eodXml = XDocument.Load(eodLog.FullName);
            }
            catch (Exception exp)
            {
                log += AddToLog("-> Failed to load EoD Xml");
                log += AddToLog("Error: " + exp.Message);
                log += AddToLog(Environment.NewLine + ">>> Please check file error and proceed with manual investigation <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("-> Done");
            log += AddToLog("Checking Final Result");
            string finalResult = eodXml.Root.Element("BATCHRESULT").Element("szFinalResult").Value;
            log += AddToLog("-> " + finalResult);
            if (finalResult != "Failure")
            {
                log += AddToLog(Environment.NewLine + ">>> Final Result of last EoD isn't \"Failure\" please check if Slayer found good log or if monitoring isn't old <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("Eod Result:");
            string activityName = null;
            var nodes = eodXml.Root.Elements("ACTIVITYLOG");
            foreach (XElement node in nodes)
            {
                if (node.Element("szFinalResult").Value == "Failure")
                {
                    log += AddToLog(node.ToString());
                    activityName = node.Element("szActivityName").Value;
                    break;
                }
            }
            log += AddToLog(eodXml.Root.Element("BATCHRESULT").ToString() + Environment.NewLine);
            if (activityName == null && connectionPara.country == "FR")
            {
                log += AddToLog(">>> Relate Problem ticket 89030889 and close this ticket with note below <<<");
                log += AddToLog(Environment.NewLine + ">> Notes for ticket:");
                log += AddToLog("Failure without failed nodes, as GSS explained issue related to export of reports handled in PT 89030889");
                gridChange(rownr, "Ticket ready to be close. See log.", Color.LightGreen);
            }
            else if (activityName == "TSEExport")
            {
                log += AddToLog(">>> Workaround for MANUALEOD Failure cases for which TSEExport failed CHC2 needs to recreate the fiscal files in BS Explorer <<<");
                gridChange(rownr, "Ticket require action from agent. See log.", Color.LightYellow);
            }
            else if (activityName == "DSFinVKExport")
            {
                log += AddToLog(Environment.NewLine + "Exporting CSV Files:");
                if (!CtrlFunctions.CsvExport(connectionPara, "", out string errorMsg))
                {
                    log += AddToLog("-> Failed: " + errorMsg);
                    log += AddToLog("Restarting TPDotnet Process Manager:");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net stop ""TPDotnet Process Manager"" && net start ""TPDotnet Process Manager""");
                    if (cmdOutput.exitCode != 0)
                    {
                        log += AddToLog("-> Failed");
                        log += AddToLog(Environment.NewLine + ">>> CSV Export failed and ToolBox wasn't able to restart TPDotnet service, please proceed with manual investigation <<<");
                        gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                        return;
                    }
                    log += AddToLog("-> Done");
                    System.Threading.Thread.Sleep(30000);
                    log += AddToLog("Retrying CSV Export:");
                    if (!CtrlFunctions.CsvExport(connectionPara, "", out errorMsg))
                    {
                        log += AddToLog("-> Failed: " + errorMsg);
                        log += AddToLog(Environment.NewLine + ">>> CSV Export failed even after TPDotnet service restart, please pass ticket to GSS accordingly to USU 56402 <<<");
                        gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                        return;
                    }
                }
                log += AddToLog("-> Done" + Environment.NewLine);
                log += AddToLog(" >>> CSV fiscal files exported successfully, please relate PT 85872171, document ticket and close it.<<<");
                gridChange(rownr, "Ticket require action from agent. See log.", Color.LightYellow);
            }
            else if (activityName == "SiiExport")
            {
                log += AddToLog(">>> Please generate missing Sii reports in backstore app and close the ticket <<<");
                gridChange(rownr, "Ticket require action from agent. See log", Color.LightYellow);
            }
            else if (activityName == null)
            {
                log += AddToLog(">>> Slayer didn't found any standard failed nodes please check manually EoD log <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
            }
            else
            {
                log += AddToLog(">>> Slayer doesn't have procedure for this error please proceed with manual investigation <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
            }
        }
        private void EoDAbortTest(int rownr, ConnectionPara connectionPara, ref string log)
        {
            if (!ConnectToHost(rownr, connectionPara, ref log))
            {
                return;
            }

            log += AddToLog("Searching last EoD Log");
            System.IO.FileInfo eodLog = GetLastEodLog(connectionPara);
            if (eodLog == null)
            {
                log += AddToLog("-> Unable to find EoD log xml");
                log += AddToLog(Environment.NewLine + ">>> Please check if host is online, logs are present and credentials are working <<<");
                return;
            }
            log += AddToLog("-> " + eodLog.FullName);
            log += AddToLog("Load EoD Log");
            XDocument eodXml;
            try
            {
                eodXml = XDocument.Load(eodLog.FullName);
            }
            catch (Exception exp)
            {
                log += AddToLog("-> Failed to load EoD Xml");
                log += AddToLog("Error: " + exp.Message);
                log += AddToLog(Environment.NewLine + ">>> Please check file error and proceed with manuall investigation <<<");
                return;
            }
            log += AddToLog("-> Done");
            log += AddToLog("Checking Final Result");
            string finalResult = eodXml.Root.Element("BATCHRESULT").Element("szFinalResult").Value;
            log += AddToLog("-> " + finalResult);
            if (finalResult != "Aborted")
            {
                log += AddToLog(Environment.NewLine + ">>> Final Result of last EoD isn't \"Aborted\" please check if Slayer found good log or if monitoring isn't old <<<");
                return;
            }
            log += AddToLog("Eod Result:");
            string activityName = null;
            var nodes = eodXml.Root.Elements("ACTIVITYLOG");
            foreach (XElement node in nodes)
            {
                if (node.Element("szFinalResult").Value != "Success")
                {
                    log += AddToLog(node.ToString());
                    activityName = node.Element("szActivityName").Value;
                }
            }
            log += AddToLog(eodXml.Root.Element("BATCHRESULT").ToString() + Environment.NewLine);
        }

        //------Backend FUNCTIONS------------
        private bool GetIndexes(string[] tixList, out int tixNr, out int status, out int summary, out int tag, out int tixSource)
        {
            string[] templine = tixList[0].Split('\t');
            tixNr = Array.IndexOf(templine, "SR Number");
            status = Array.IndexOf(templine, "Assignment Status");
            summary = Array.IndexOf(templine, "Summary");
            tag = Array.IndexOf(templine, "Tag");
            tixSource = Array.IndexOf(templine, "SR Creation Channel");
            string[] columsNames = new string[]
            {
                "SR Number", "Assignment Status", "Summary", "Tag", "SR Creation Channel"
            };
            int[] indexArray = new int[]
            {
                tixNr, status, summary, tag, tixSource
            };
            int checkIndexes = Array.IndexOf(indexArray, -1);
            if (checkIndexes == -1)
            {
                return true;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Ticket List Formating Error", "Selected ticket list don't have " + columsNames[checkIndexes] + " column. Please enable it in EBS and export list again");
            return false;
        }
        private bool ConnectToHost(int rownr, ConnectionPara connectionPara, ref string log)
        {
            gridChange(rownr, "Connecting to " + connectionPara.TAG);
            log += AddToLog("Connecting to " + connectionPara.TAG);
            if (connectionPara.IP == "DNS ERROR")
            {
                DnsRestore(rownr, connectionPara, ref log);
                return false;
            }
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out CtrlFunctions.CmdOutput cmdOutput))
            {
                log += AddToLog("-> Unable to map drive: " + cmdOutput.errorOutputText);
                log += AddToLog(Environment.NewLine + ">>> Please check if host is online and credentials are working <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return false;
            }
            log += AddToLog("-> Done");
            return true;
        }
        private void LookForTA(string txnr, string location, string filtr, ref string log)
        {
            log += AddToLog("Files " + filtr + " found in: " + location);
            string[] TxFiles;
            try
            {
                TxFiles = System.IO.Directory.GetFiles(location, filtr, System.IO.SearchOption.AllDirectories);
            }
            catch (Exception exp)
            {
                log += AddToLog("-> Unable to read " + filtr + " files in " + location + ". Error: " + exp.Message);
                return;
            }
            if (TxFiles.Length != 0)
            {
                bool notFound = true;
                foreach (string xmlFile in TxFiles)
                {
                    try
                    {
                        XDocument txXml = XDocument.Load(xmlFile);
                        if (txXml.Descendants("lTaNmbr").ToArray()[0].Value != txnr)
                        {
                            continue;
                        }
                        notFound = false;
                        log += AddToLog("-> " + xmlFile);
                        log += AddToLog("<szTaOperationType> " + txXml.Descendants("szTaOperationType").ToArray()[0].Value);
                        log += AddToLog("<lClientID> " + txXml.Descendants("lClientID").ToArray()[0].Value);
                        log += AddToLog("<lTaNmbr> " + txXml.Descendants("lTaNmbr").ToArray()[0].Value);
                        log += AddToLog("<lWorkstationNmbr> " + txXml.Descendants("lWorkstationNmbr").ToArray()[0].Value);
                        log += AddToLog("<lRetailStoreID> " + txXml.Descendants("lRetailStoreID").ToArray()[0].Value);
                        log += "<lMediaNmbr> ";
                        foreach (XElement node in txXml.Descendants("lMediaNmbr"))
                        {
                            log += node.Value + ",";
                        }
                        if (txXml.Descendants("szSerialNmbr").ToArray().Length != 0)
                        {
                            foreach (XElement node in txXml.Descendants("szSerialNmbr"))
                            {
                                log += AddToLog("<szSerialNmbr> " + node.Value);
                            }
                        }
                        log += AddToLog("");
                    }
                    catch (Exception exp)
                    {
                        log += AddToLog("-> Toolbox was unable to load " + xmlFile + " file with error: " + exp.Message);
                    }
                }
                if (notFound)
                {
                    log += AddToLog("-> Toolbox wasn't able to find any files matching lTaNmbr " + txnr);
                }
            }
            else
            {
                log += AddToLog("-> Toolbox wasn't able to find " + filtr + " files in: " + location);
            }
        }
        private void DnsRestore(int rownr, ConnectionPara connectionPara, ref string log)
        {
            log += AddToLog("-> DNS ERROR");
            gridChange(rownr, "Looking for IP");
            log += AddToLog("Looking for IP based on " + connectionPara.country + connectionPara.storeNr + " store");
            string ipGuess = CtrlFunctions.GetIpFromDNSError(connectionPara);
            if (ipGuess == "DNS ERROR")
            {
                log += AddToLog("Unable to read IP from store");
                log += AddToLog(Environment.NewLine + ">>> Ticket needs manual investigation <<<");
                gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                return;
            }
            log += AddToLog("-> IP: " + ipGuess);
            log += AddToLog("Pinging IP");
            try
            {
                if (new Ping().Send(ipGuess, 4000).Status == IPStatus.Success)
                {
                    log += AddToLog("IP Pingable");
                    log += AddToLog("Executing DNS Restore on " + ipGuess);
                    gridChange(rownr, "Restoring DNS");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + ipGuess + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c ipconfig /flushdns && ipconfig /renew && ipconfig /registerdns && gpupdate /force");
                    if (cmdOutput.exitCode != 0)
                    {
                        log += AddToLog("RCMD failed to execute DNS Restore. Error:" + Environment.NewLine);
                        log += AddToLog(cmdOutput.errorOutputText);
                        log += AddToLog(">>> Ticket needs manual investigation <<<");
                        gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
                        return;
                    }
                    log += AddToLog(Logger.LogTime() + "Executed:");
                    log += AddToLog("- DNS Resolver Cache flushed");
                    log += AddToLog("- IP renewed");
                    log += AddToLog("- DNS registered");
                    log += AddToLog("- Group and user policies updated");
                    log += AddToLog(">>> Please check host in 20 minutes if DNS error is solved <<<");
                    gridChange(rownr, "Ticket require action from agent. See log", Color.LightYellow);
                    return;
                }
            }
            catch { }

            log += AddToLog("Unable to ping IP");
            log += AddToLog(Environment.NewLine + ">>> Ticket needs manual investigation <<<");
            gridChange(rownr, "Ticket needs manual investigation. See log", Globals.errorColor);
        }
        private System.IO.FileInfo GetLastEodLog(ConnectionPara connectionPara)
        {
            try
            {
                System.IO.FileInfo[] files = new System.IO.DirectoryInfo(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log").GetFiles("LOG_*_" + connectionPara.TAG + "_????????_??????_MANUALEOD.xml");
                if (files.Length == 0)
                {
                    return null;
                }
                System.IO.FileInfo myFile = (from f in files orderby f.CreationTime descending select f).First();
                return myFile;
            }
            catch
            {
                return null;
            }
        }
        private void CollectionFailedAutoClosure(ConnectionPara connectionPara, ref string log, string closeMsg)
        {
            try
            {
                System.IO.File.AppendAllText(@"\\" + connectionPara.TAG + @"\c$\service\scripts\MONITORING\Log\COLLECT_TP_REPORTS_" + DateTime.Today.ToString("yyyyMMdd") + ".log", "canda_omnipos_reports_ok|" + DateTime.Now.ToString("yyyyMMddHHmm") + "|" + closeMsg + Environment.NewLine, System.Text.Encoding.ASCII);
            }
            catch (Exception appendExp)
            {
                log += AddToLog("!! Error during logging process: " + appendExp.Message);
                log += AddToLog("!! Autoclosure not possible - please close ticket manually");
                Logger.QuickLog(Globals.Funkcje.MonitoringSlayer, "Collection Failed", connectionPara.TAG, "ErrorLog", "AppendText Error: " + appendExp.ToString());
            }

        }
    }
}
