using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public partial class Main : Form
    {
        /**//**//**//**//**//**//**//* USTANDARYZOWANE GLOBALNE *//**//**//**//**//**//**//**//**//**//**//**//**//**/
        public static Main interfejs;
        public ConnectionPara connectionPara;
        public UserSettings userSettings = new UserSettings();
        public Logger myLog = new Logger(Globals.Funkcje.MainFrom, "None", "MainForm");
        private int versionWarning = 0;
        private bool updatePopup = false;
        readonly List<string> uniwersalMenuItems = new List<string>
        {
            //RemoteCMD's//
            "RCMD",
            "Tracert to TAG",
            "Reboot Device",
            "DNS Restore",
            //Quick Access//
            "Drives",
            "ServerRT (Italy)",
            "Transactions",
            //Logs//
            "Windows",
            "Minidump Folder",
            "TP.Net Logs",
            //Diagnostics//
            "Get S.M.A.R.T",
            "Drives Space Info",
            "Install WinDirStat",
            "Scan Store Endpoints",
            "System Boot Time",
            "SQl Queries",
            //Fixes//
            //Tools//
            "Service Manager"
        };
        readonly List<string> tpsMenuItems = new List<string>
        {
            //RemoteCMD's//
            //Quick Access//
            "APC",
            "MiniLogger",
            "TP Reports",
            //Logs//
            "Symantec Backup Logs",
            "Installation Logs",
            "TFTPD Logs",
            "OEMInst Logs",
            "PDCU Data Error Secure",
            //Diagnostics//
            "Backup Checker",
            "EoD Checker",
            "DumpFile Analyse",
            //Fixes//
            "APC Service Fix",
            "Veritas Backup Job Reset",
            "Backstore CSV Export"
            //Tools//
        };
        readonly List<string> stpMenuItems = new List<string>
        {
            //Quick Access//
            "Local Storage (Till)",
            //Logs//
            "S4Fiscal Secure",
            "TSE Logs Secure",
            //Diagnostics//
            //Fixes//
            "POS Colon : Fix",
            "Till Local Cashe Clear",
            "Parked TX Move",
            "TSE Webservice Restart",
            "Signator Reset",
            //Tools//
            "MobilePos App Kill"
        };
        readonly List<string> ipNotSupported = new List<string>
        {
            "Local Storage (Till)",
            "Scan Store Endpoints",
            "Backup Checker",
            "EoD Checker",
            "POS Colon : Fix",
            "Till Local Cashe Clear"
        };

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        public Main()
        {
            InitializeComponent();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Logger.UnhandledError);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            interfejs = this;
        }
        private void Main_Shown(object sender, EventArgs e)
        {
            StartUp.StartUpProcedure();
        }
        private void Test_Button_Click(object sender, EventArgs e)
        {
            //string output = "";
            //foreach (string file in System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\HostData\Download\Data", "*", System.IO.SearchOption.AllDirectories))
            //{
            //    output += file + Environment.NewLine;
            //}

            //MessageBox.Show(output);


            //CtrlFunctions.EncryptFile(@".\mojepasy.txt", "cycuszki", Globals.configPath + "credentials.crypt");
            //MessageBox.Show("krypto krypto superman lezy");

            MassEmergancy massEmergancy = new MassEmergancy();
            massEmergancy.Show();

            //Logger.GeneratePortalReport(@".\EoD_Abort_Test_Report.txt", @".\EoD_Abort_Test_AddInfo.txt", @".\logo.txt", @".\output.txt");
        }
        //--------------------/UI Controls/---------------------------
        private void GetMAC_button_Click(object sender, EventArgs e)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetMAC, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetMAC);
            textBox_MAC.Text = "Stealing MAC...";
            getMAC_button.Enabled = false;
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
                    textBox_MAC.Text = cmdOutput.outputText;
                    getMAC_button.Enabled = true;
                };
                slave.RunWorkerAsync();
            }
        }
        private void Rescan_Button_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.RescanSubNet);
            CtrlFunctions.SubNetScan(connectionPara);
        }
        private void SaveNote_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.SaveNote);
            FileController.SaveTxtDialog(string.Join(Environment.NewLine, notepad.Lines), ref myLog);
        }
        private void ImportNote_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.ImportNote);
            try
            {
                notepad.Lines = System.IO.File.ReadAllLines(FileController.OpenFileDialog("Text files (*.txt)|*.txt", ref myLog));
            }
            catch { }
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------/PING BUTTONS/---------------------------
        private void PingButton_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.PingButton);
            Process.Start("cmd.exe", "/c ping " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], (sender as Button).Tag.ToString()) + " && pause");
        }
        private void PRNPingButton_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.PingButton);
            int de = 1;
            if (connectionPara.country == "DE") { de = 0; }
            Process.Start("cmd.exe", "/c ping " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], (connectionPara.IPbytes[2] + de).ToString(), (sender as Button).Tag.ToString()) + " && pause");
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //---------------------RemoteCMDs ToolBarMenu-----------------------
        private void RCMDMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.RCMD);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.RCMD, "");
            Process.Start("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd");
        }
        private void RebootDeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "CAUTION", "Are you sure you want to RESTART device: " + connectionPara.TAG + "?") == DialogResult.Cancel)
            {
                return;
            }
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.RemoteRestart, "");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c shutdown /r");
            if (cmdOutput.exitCode != 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Command Execution Error", "RCMD encounter a error during execution:" + Environment.NewLine + cmdOutput.errorOutputText);
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "CMD Failed to restart host");
                Logger.QuickLog(Globals.Funkcje.RemoteRestart, "", connectionPara.TAG, "ErrorLog", cmdOutput.errorOutputText);
                return;
            }
            Telemetry.LogFunctionUsage(Globals.Funkcje.RemoteRestart);
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Success", "Command was successful send to remote host");
        }
        private void TracertMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Tracert, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.Tracert);
            Process.Start("cmd.exe", "/c tracert " + connectionPara.TAG + "&& pause");
        }
        private void DNSRestoreMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.DNSRestore, "");
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Restoring DNS");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c ipconfig /flushdns && ipconfig /renew && ipconfig /registerdns && gpupdate /force");
                    ChangeStatusBar("Ready");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "RCMD exited with code: " + cmdOutput.exitCode);
                        Logger.QuickLog(Globals.Funkcje.DNSRestore, "", connectionPara.TAG, "ErrorLog", "RCMD exited with code: " + cmdOutput.exitCode + "\n" + cmdOutput.errorOutputText);
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Execution Error", "RCMD exited with error while executing commands:\n" + cmdOutput.errorOutputText);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "DNS Restored", "Executed:\n- DNS Resolver Cache flushed\n- IP renewed\n- DNS registered\n- Group and user policies updated\n\nPlease check host in 5 to 20 minutes if DNS error is solved");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.DNSRestore);
                };
                slave.RunWorkerAsync();
            }
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //---------------------Quick Access ToolBarMenu-----------------------
        //------------------Last Connected--------------------
        private void LastMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            if (toolStripMenuItem.Text == "empty") { return; }
            textBox_TAG.Text = toolStripMenuItem.Text;
            StartButton_Click(this, e);
        }

        //------------------Drives--------------------
        private void DriveMenuItem_Click(object sender, EventArgs e)
        {
            CtrlFunctions.OpenFolder(connectionPara.TAG, (sender as ToolStripMenuItem).Tag.ToString());
        }

        //------------------APC--------------------
        private void APCMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.OpenWebAPC, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.OpenWebAPC);
            Process.Start(@"Https://" + connectionPara.IP + @":6547/");
        }
        private void ServerRTMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.OpenServerRT, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.OpenServerRT);
            Process.Start(@"Https://" + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2]) + @".178/");
        }

        //------------------TP Raports--------------------
        private void LocalStorageTillMenuItem_Click(object sender, EventArgs e)
        {
            CtrlFunctions.OpenFolder(connectionPara.TAG, @"c$\Users\" + connectionPara.country + connectionPara.storeNr + connectionPara.storeType + @".AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage");
        } //dont support IP MODE

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //---------------------Logs ToolBarMenu-----------------------
        //------------Windows Logs------------------
        private void WindowsLogsMenuItem_Click(object sender, EventArgs e)
        {
            CtrlFunctions.GetWinLogs((sender as ToolStripMenuItem).Text, connectionPara);
        }

        //-----------EPOS Logs-----------------
        private void SecureLogsMenuItem_Click(object sender, EventArgs e)
        {
            FileController.ZipAndStealFolder("TPLogs", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log", @"D:\TPDotnet\Log", connectionPara);
        }
        private void S4FiscalSecureMenuItem_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\c$\Program Files (x86)\Service Plus"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No S4 folder found", "Can't find any S4 folder on target host");
                return;
            }
            string[] directories = System.IO.Directory.GetDirectories(@"\\" + connectionPara.TAG + @"\c$\Program Files (x86)\Service Plus", "S4Fiscal???", System.IO.SearchOption.TopDirectoryOnly);
            if (directories.Length == 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No S4 folder found", "Can't find any S4 folder on target host");
                return;
            }
            if (directories.Length > 1)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "More than one S4 folder found", "Script found more than one S4 folder on target host. Please check manually");
                return;
            }
            FileController.ZipAndStealFolder("S4Fiscal", @"\\" + connectionPara.TAG + @"\c$\Program Files (x86)\Service Plus\" + System.IO.Path.GetFileNameWithoutExtension(directories[0]), @"C:\Program Files (x86)\Service Plus\" + System.IO.Path.GetFileNameWithoutExtension(directories[0]), connectionPara);
        }
        private void TSELogsSecureMenuItem_Click(object sender, EventArgs e)
        {
            FileController.ZipAndStealFolder("TSELogs", @"\\" + connectionPara.TAG + @"\c$\ProgramData\DieboldNixdorf\TSE-Webservice\log", @"C:\ProgramData\DieboldNixdorf\TSE-Webservice\log", connectionPara);
        }
        private void JPOSRFIDLogsSecureMenuItem_Click(object sender, EventArgs e)
        {
            MassJPOSLogs massJPOSLogs = new MassJPOSLogs();
            massJPOSLogs.Show();
        }
        private void pDCUDataErrorSecureMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\StoreApps\CarsData\Cars\pdcudata\error").Length == 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "PDCU Data Secure Error", "No error files found in folder");
                return;
            }
            FileController.ZipAndStealFolder("PdcuError", @"\\" + connectionPara.TAG + @"\d$\StoreApps\CarsData\Cars\pdcudata\error", @"D:\StoreApps\CarsData\Cars\pdcudata\error", connectionPara);
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/
        private void QuickPingTAGMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiagnosticPing);
            Process.Start("cmd.exe", "/c ping " + GetTAG() + @" && pause");
        }
        private void PingWithLoad_TAGMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiagnosticPing);
            Process.Start("cmd.exe", "/c ping " + GetTAG() + @" -t -l 2048");
        }
        private void SavePingToTxtMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusBar("Working...");
            Telemetry.LogFunctionUsage(Globals.Funkcje.SavePingToTxt);

            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + GetTAG());
            if (cmdOutput.exitCode != 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "CMD encountered error", "Unfortunately command encountered an unexpected error while execution." + Environment.NewLine + "Error: " + cmdOutput.errorOutputText);
                Main.ChangeStatusBar("Ready");
                return;
            }
            FileController.SaveTxtToFile(@".\Logs\Ping(" + GetTAG() + ") - " + Logger.Datownik() + ".txt", "Ping executed at: " + DateTime.Now.ToString() + Environment.NewLine + cmdOutput.outputText, ref myLog);
            ChangeStatusBar("Ready");
        }
        private void PingOverTimeMenuItem_Click(object sender, EventArgs e)
        {
            PingConst pingConst = new PingConst();
            pingConst.Show();
        }

        //------------SMARTY--------------------------
        private void GetSMARTMenuItem_Click(object sender, EventArgs e)
        {

            using (BackgroundWorker slave = new BackgroundWorker())
            {
                ConnectionPara connectionPara = Main.interfejs.connectionPara;
                slave.DoWork += (s, args) =>
                {
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSMART, "");
                    sMARTToolStripMenuItem.Enabled = false;
                    ChangeStatusBar("Reading SMART values");
                    if (!CtrlFunctions.Smarty(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSMART, errorMsg);
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SMART Function Error", errorMsg);
                    }
                    else
                    {
                        if (!FileController.MoveFile(@"\\" + connectionPara.TAG + @"\c$\SMART\DiskInfo.txt", @".\Logs\SMART\SMART - " + connectionPara.TAG + ".txt", true, ref myLog))
                        {
                            Logger.QuickLog(Globals.Funkcje.GetSMART, @"\\" + connectionPara.TAG + @"\c$\SMART\DiskInfo.txt | " + @".\Logs\SMART\SMART - " + connectionPara.TAG + ".txt", connectionPara.TAG, "WarningLog", "ToolBox wasn't able to copy command output back from targeted host.");
                            errorMsg = @"ToolBox wasn't able to copy command output back from targeted host. You can check it manually at \\" + connectionPara.TAG + @"\C$\SMART\DiskInfo.txt";
                            ChangeStatusBar("Ready");
                            sMARTToolStripMenuItem.Enabled = true;
                            return;
                        }
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "SMART Values Saved", @"Disc SMART values successfully saved at .\Logs\SMART\SMART - " + connectionPara.TAG + ".txt");
                        Process.Start(@".\Logs\SMART\SMART - " + connectionPara.TAG + ".txt");
                    }
                    ChangeStatusBar("Ready");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.GetSMART);
                    sMARTToolStripMenuItem.Enabled = true;

                };
                slave.RunWorkerAsync();
            }
        }

        //------------Disc managment--------------------------
        private void DrivesSpaceInfoMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiscSpaceInfo);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.DiscSpaceInfo, "");
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string[] drives = { "c", "d", "e", "f" };
                    string output = "";
                    bool error = true;
                    foreach (string letter in drives)
                    {
                        if (System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\" + letter + @"$"))
                        {
                            output += "Drive " + letter.ToUpper() + ":\\ Informations:" + "\n" + CtrlFunctions.GetDiskSpaceInfo(letter, connectionPara) + "\n\n";
                            error = false;
                        }
                    }
                    if (error)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Disc Connection Error");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Disc Connection Error", @"Couldn't establish connection to any disc. Please check if target machine is online or initialize it anew and try again.");
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Disk Space Info", output);
                };
                slave.RunWorkerAsync();
            }

        }
        private void InstallWinDirStatMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusBar("Copying...");
            if (System.IO.File.Exists(@"\\" + connectionPara.TAG + @"\c$\temp\windirstat.exe"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "", "WinDirStat is already on target host");
                ChangeStatusBar("Ready");
                return;
            }
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.WinDirStatInstall, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.WinDirStatInstall);
            Logger winDirStatLog = new Logger(Globals.Funkcje.WinDirStatInstall, "None", connectionPara.TAG);
            if (!FileController.CopyFile(Globals.toolsPath + "windirstat.exe", @"\\" + connectionPara.TAG + @"\c$\temp\windirstat.exe", true, ref winDirStatLog))
            {
                winDirStatLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Copying windirstat failed");
            }
            ChangeStatusBar("Ready");
        }

        //------------------Endpoint Scan--------------------
        private void ScanEndpointsMenuItem_Click(object sender, EventArgs e)
        {
            EndpointsScan endpointsScan = new EndpointsScan();
            endpointsScan.Show(this);
        } //dont support IP MODE

        //------------------BootTime--------------------
        private void SystemBootTimeMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetSystemBootTime);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSystemBootTime, "");
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Working");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c systeminfo | find /i \"Boot Time\"");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "RCMD Error");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "RCMD Error", "Unable to get last boot time - RCMD exited with error:" +
                            Environment.NewLine +
                            cmdOutput.exitCode.ToString() + ": " + cmdOutput.errorOutputText);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "Last Boot Time Info", cmdOutput.outputText);
                    ChangeStatusBar("Ready");
                };
                slave.RunWorkerAsync();
            }
        }

        //------------------Backup Checker--------------------
        private void BackupCheckerMenuItem_Click(object sender, EventArgs e)
        {
            BackupCheck backupCheck = new BackupCheck();
            backupCheck.Show();
        } //dont support IP MODE

        //------------------EoD Checker--------------------
        private void EoDCheckerMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Logger myLog = new Logger(Globals.Funkcje.EodCheck, "None", connectionPara.TAG);
            string[] files;
            try
            {
                myLog.Add("Reading log files");
                files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log", "LOG_*_" + connectionPara.TAG + "_????????_??????_MANUALEOD.xml");
            }
            catch (Exception exp)
            {
                myLog.Add("Error reading log files");
                myLog.Add(exp.ToString());
                myLog.SaveLog("ErrorLog");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading log files", "ToolBox wasn't able to read log files:" + Environment.NewLine + exp.Message);
                return;
            }
            if (files.Length == 0)
            {
                myLog.Add("No log files found");
                myLog.SaveLog("WarningLog");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No log files found", "ToolBox wasn't able to find any log files");
                return;
            }
            List<string> fileList = new List<string>();
            foreach (string file in files)
            {
                fileList.Add(System.IO.Path.GetFileName(file) + " - " + XDocument.Load(file).Root.Element("BATCHRESULT").Element("szFinalResult").Value);
            }

            string selectedFile = null;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Select Eod Raport to analyse", fileList.ToArray()))
            {
                if (dropDownSelect.ShowDialog() == DialogResult.OK)
                {
                    selectedFile = dropDownSelect.ReturnValue1;            //values preserved after close
                }
                else
                {
                    return;
                }
            }
            myLog.Add("Selected file: " + selectedFile);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.EodCheck, selectedFile);
            XDocument eodXml;
            try
            {
                eodXml = XDocument.Load(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log\" + selectedFile.Split(' ')[0]);
            }
            catch (Exception exp)
            {
                myLog.Add("Error reading log file");
                myLog.Add(exp.ToString());
                myLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Error reading log file");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading log file", "ToolBox wasn't able to read log file:" + Environment.NewLine + exp.Message);
                return;
            }
            string output = "";
            foreach (XElement node in eodXml.Root.Elements("ACTIVITYLOG"))
            {
                if (node.Element("szFinalResult").Value == "AbortedCancel" || node.Element("szFinalResult").Value == "Failure")
                {
                    output += node.ToString() + Environment.NewLine;
                }
            }
            output += Environment.NewLine + eodXml.Root.Element("BATCHRESULT").ToString();

            Telemetry.LogFunctionUsage(Globals.Funkcje.EodCheck);
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Eod Result", output);
        } //dont support IP MODE

        //------------------DumpFile analyse--------------------
        private void dumpFileAnaliseMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;

            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\c$\Windows\Minidump"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "No Minidump found", "ToolBox couldn't find minidump folder on host");
                return;
            }

            System.IO.FileInfo[] files;
            Logger myLog = new Logger(Globals.Funkcje.MiniDumpAnalyser, "", connectionPara.TAG);

            try
            {
                files = new System.IO.DirectoryInfo(@"\\" + connectionPara.TAG + @"\c$\Windows\Minidump").GetFiles("*.dmp").OrderBy(p => p.CreationTime).ToArray();
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading minidump files", "ToolBox couldn't find/access minidump files on host with error:" + Environment.NewLine + exp.Message);
                myLog.Add("Error reading minidump files" + Environment.NewLine + exp.ToString());
                myLog.SaveLog("ErrorLog");
                return;
            }
            if (files.Length == 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No minidump files found", "ToolBox couldn't find any minidump files in minidump folder");
                return;
            }
            string selectedMinidump = null;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Select minidump to analyse", files.Select(f => f.FullName).ToArray()))
            {
                var result = dropDownSelect.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedMinidump = dropDownSelect.ReturnValue1;            //values preserved after close
                }
                else
                {
                    return;
                }
            }
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.MiniDumpAnalyser, selectedMinidump);
            Telemetry.LogFunctionUsage(Globals.Funkcje.MiniDumpAnalyser);

            if (!CtrlFunctions.AnalyseMiniDump(selectedMinidump, @".\Logs\" + connectionPara.TAG + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt", out string errorMsg))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error while analysing minidump", "Toolbox encountered problem while processing minidump:" + Environment.NewLine + errorMsg);
                return;
            }

            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Analysis has been completed", @"Minidump file successful processed and saved at .\Logs\" + connectionPara.TAG + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt");
            Process.Start(@".\Logs\" + connectionPara.TAG + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt");
        }

        //-----------------SQL-----------------------------------
        private void lastTxRollOverMenuItem_Click(object sender, EventArgs e)
        {
            if (!CtrlFunctions.SqlGetInfo(connectionPara.TAG, "TPPosDB", "select lLastTaNmbr, lRolloverTransactionNmbrAt from TxControlTransactionNmbr", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetSqlInfo);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
        }
        private void missingTxMenuItem_Click(object sender, EventArgs e)
        {
            if (!CtrlFunctions.SqlGetInfo(connectionPara.TAG, "TPCentralDB", "select lTaNmbr, lWorkstationNmbr, szDate, szTime from TxMissings", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetSqlInfo);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
        }
        private void showLastSignOffFromYesterdayMenuItem_Click(object sender, EventArgs e)
        {
            string data = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            if (!CtrlFunctions.SqlGetInfo(String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"), "TPCentralDB", "select top 1 lWorkstationNmbr, szType, szDate, szTime, bIsDeclaration from TxSignOnOff where lWorkstationNmbr=" + connectionPara.deviceNr.TrimStart('0') + " and szDate=" + data + " and szType='SIGN_OFF' Order By szTime DESC", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetSqlInfo);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Fixes-----------------------------
        private void ColonFixMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Logger colonFixLog = new Logger(Globals.Funkcje.ColonFix, "None", connectionPara.TAG);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.ColonFix, "");

            ChangeStatusBar("Killing MobilePOS");
            colonFixLog.Add("Killing MobilePOS");
            if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "MobilePOS Kill Failed");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                colonFixLog.Add(errorMsg);
                colonFixLog.SaveLog("ErrorLog");
                return;
            }

            ChangeStatusBar("Copying posDB...");

            if (!FileController.CopyFile(@"\\" + connectionPara.TAG + @"\c$\Users\" + connectionPara.country + connectionPara.storeNr + connectionPara.storeType + @".AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage\http_localhost_8088.localstorage", @".\http_localhost_8088.localstorage", true, ref colonFixLog))
            {
                colonFixLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "localstorage file copy error");
                ChangeStatusBar("Ready");
                return;
            }
            if (!FileController.CopyFile(Globals.toolsPath + "ActOperatorID.txt", @".\ActOperatorID.txt", true, ref colonFixLog))
            {
                colonFixLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "ActOperatorID file copy error");
                ChangeStatusBar("Ready");
                return;
            }

            ChangeStatusBar("Waiting for confirmation");
            if (CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "Waiting for file repair and confirmation", "Please repair database file and press OK to copy file back to the till.") != DialogResult.OK)
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Canceled by user");
                ChangeStatusBar("Ready");
                return;
            }
            ChangeStatusBar("Copying DB back to till...");
            if (!FileController.MoveFile(@".\http_localhost_8088.localstorage", @"\\" + connectionPara.TAG + @"\c$\Users\" + connectionPara.country + connectionPara.storeNr + @"P.AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage\http_localhost_8088.localstorage", true, ref colonFixLog))
            {
                colonFixLog.SaveLog("ErrorLog");
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, @"Copying localstorage file back error");
                ChangeStatusBar("Ready");
                return;
            }

            ChangeStatusBar("Cleaning up...");
            try
            {
                System.IO.File.Delete(@".\ActOperatorID.txt");
            }
            catch { }

            Telemetry.LogFunctionUsage(Globals.Funkcje.ColonFix);
            ChangeStatusBar("Ready");
        } //dont support IP MODE
        private void tillLocalCacheClearMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ConnectionPara connectionPara = Main.interfejs.connectionPara;
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Local Cashe Clear", "This function will terminate MobilePOS app on selected host. Do you want to proceed?") != DialogResult.OK)
                    {
                        return;
                    }

                    Logger myLog = new Logger(Globals.Funkcje.LocalCacheClear, "", connectionPara.TAG);
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.LocalCacheClear, "");

                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        ChangeStatusBar("Ready");
                        return;
                    }

                    ChangeStatusBar("Clearing Cache");
                    myLog.Add("Clearing Cache");
                    FileController.ClearFolder(@"\\" + connectionPara.TAG + @"\c$\Users\" + connectionPara.country + connectionPara.storeNr + connectionPara.storeType + @".AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage", false, true, ref myLog);
                    if (myLog.wasError)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to delete files");
                        myLog.SaveLog("ErrorLog");
                    }
                    ChangeStatusBar("Ready");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.LocalCacheClear);
                };
                slave.RunWorkerAsync();
            }
        } //dont support IP MODE
        private void parkedTXMoveMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Parked Tx Move", "This function will terminate MobilePOS app on selected host. Do you want to proceed?") != DialogResult.OK)
                    {
                        return;
                    }

                    Logger myLog = new Logger(Globals.Funkcje.ParkedTxMove, "", connectionPara.TAG);
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.ParkedTxMove, "");

                    myLog.Add("Killing MobilePOS");
                    ChangeStatusBar("Killing MobilePOS");
                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        ChangeStatusBar("Ready");
                        return;
                    }
                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    myLog.Add("Ticket nr: " + tixnr);
                    if (tixnr == "")
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Wrong ticket number");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Wrong ticket number", "Please provide valid ticket number");
                        return;
                    }
                    string outputFolderName = @"\\" + connectionPara.TAG + @"\d$\WNI\4GSS\Parked - " + tixnr + "(" + connectionPara.TAG + ") " + Logger.Datownik();
                    if (!FileController.MakeFolder(outputFolderName, ref myLog))
                    {
                        myLog.SaveLog("ErrorLog");
                        ChangeStatusBar("Ready");
                        return;
                    }
                    myLog.Add("Moving Files");
                    foreach (string file in System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Pos\Transactions\Parked"))
                    {
                        try
                        {
                            System.IO.File.Move(file, outputFolderName + @"\" + System.IO.Path.GetFileName(file));
                        }
                        catch (Exception exp)
                        {
                            myLog.Add(exp.Message);
                            myLog.wasError = true;
                            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, exp.Message);
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Move File Error", "ToolBox was unable to move " + file + Environment.NewLine + exp.Message);
                        }
                    }
                    if (myLog.wasError)
                    {
                        myLog.SaveLog("ErrorLog");
                    }
                    ChangeStatusBar("Ready");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.ParkedTxMove);
                };
                slave.RunWorkerAsync();
            }
        }
        private void TSEWebserviceRestartMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.TSEWebserviceRestart);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.TSEWebserviceRestart, "");
            Process.Start("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c ""cd C:\Program Files (x86)\DieboldNixdorf\TSE-Webservice\bin"" && dn_tsetool.bat restart && pause");
        }
        private void TPRaportsRegenZipMenuItem_Click(object sender, EventArgs e)
        {
            TPReportsRegenZip tPReportsRegenZip = new TPReportsRegenZip();
            tPReportsRegenZip.Show();
        }
        private void InvalidTransferMenuItem_Click(object sender, EventArgs e)
        {
            InvTansactionTransfer invTansactionTransfer = new InvTansactionTransfer();
            invTansactionTransfer.Show();
        }
        private void SignatorResetMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Logger myLog = new Logger(Globals.Funkcje.SignatorReset, "", connectionPara.TAG);
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.SignatorReset, "");

                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        return;
                    }

                    ChangeStatusBar("Resetting Signator");
                    myLog.Add("Resetting Signator");
                    string cashBoxID = "0000" + "43" + connectionPara.storeNr + "00000000" + connectionPara.deviceNr;
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\Program Files (x86)\signator && signatorhelper -closecashbox cashboxid=" + cashBoxID + " reason=7 && net stop signator");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to Reset Signator or stop service");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to reset Signator or stop service", "PSEXEC encountered some problems trying to Unable to reset Signator or stop it service and exited with error:" + Environment.NewLine + cmdOutput.errorOutputText);
                        ChangeStatusBar("Ready");
                        myLog.Add("Resetting Signator Error: " + cmdOutput.exitCode);
                        myLog.Add(cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    ChangeStatusBar("Creating Signator backup folder");
                    myLog.Add("Creating Signator backup folder");
                    string signatorPath = @"\\" + connectionPara.TAG + @"\c$\ProgramData\signator";
                    string backupPath = @"\\" + connectionPara.TAG + @"\c$\ProgramData\signator_backup";
                    if (System.IO.Directory.Exists(backupPath))
                    {
                        myLog.Add("Detected old backup folder - deleting");
                        try
                        {
                            System.IO.Directory.Delete(backupPath, true);
                        }
                        catch (Exception exp)
                        {
                            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to delete old signator backup folder");
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to delete old signator backup folder", "Toolbox was unable to delete old signator backup folder. Error:" + Environment.NewLine + exp.Message);
                            ChangeStatusBar("Ready");
                            myLog.Add("Unable to delete backup folder");
                            myLog.Add(exp.ToString());
                            myLog.SaveLog("ErrorLog");
                            return;
                        }
                        myLog.Add("Deleted");
                    }

                    myLog.Add("Renaming folder to backup");
                    try
                    {
                        System.IO.Directory.Move(signatorPath, backupPath);
                    }
                    catch (Exception exp)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to create signator backup folder");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to create signator backup folder", "Toolbox was unable to create signator backup folder. Error:" + Environment.NewLine + exp.Message);
                        ChangeStatusBar("Ready");
                        myLog.Add("Unable to rename backup folder");
                        myLog.Add(exp.ToString());
                        myLog.SaveLog("ErrorLog");
                        return;
                    }
                    myLog.Add("Rename done");

                    ChangeStatusBar("Starting Signator service");
                    myLog.Add("Starting Signator service");
                    cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net start signator");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to start Signator service");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to start Signator service", "PSEXEC encountered some problems trying to start Signator service and exited with error:" + Environment.NewLine + cmdOutput.errorOutputText);
                        ChangeStatusBar("Ready");
                        myLog.Add("Unable to start service: " + cmdOutput.exitCode);
                        myLog.Add(cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    ChangeStatusBar("Ready");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.SignatorReset);
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Signator successfully resetted", "Signator reset procedure executed without problems. Please start MobilePos and check if signator is working");
                };
                slave.RunWorkerAsync();
            }
        }
        private void aPCServiceFixMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ConnectionPara connectionPara = Main.interfejs.connectionPara;
                    Logger myLog = new Logger(Globals.Funkcje.ApcServiceFix, "", connectionPara.TAG);

                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.ApcServiceFix, "");
                    Main.ChangeStatusBar("Copying config file");
                    if (!FileController.CopyFile(Globals.toolsPath + "m11.cfg", @"\\" + connectionPara.TAG + @"\c$\Program Files (x86)\APC\PowerChute Business Edition\agent\m11.cfg", true, ref myLog))
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Copying config file error");
                        myLog.SaveLog("ErrorLog");
                        Main.ChangeStatusBar("Ready");
                        return;
                    }

                    myLog.Add("Starting APC Agent");
                    Main.ChangeStatusBar("Starting APC Agent");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c net start apcpbeagent");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to start APC Agent");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to start APC Agent", "Error occured while starting APC Agent Service:" + Environment.NewLine + cmdOutput.errorOutputText);
                        myLog.Add("Cmd error: " + cmdOutput.exitCode + Environment.NewLine + cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        Main.ChangeStatusBar("Ready");
                        return;
                    }

                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "APC Agent Started", "Config file fixed and APC Agent Service successfully started");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.ApcServiceFix);
                    Main.ChangeStatusBar("Ready");
                };
                slave.RunWorkerAsync();
            }
        }
        private void veritasBackupJobResetMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Reseting Veritas Jobs");
                    ConnectionPara connectionPara = Main.interfejs.connectionPara;
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.VeritasJobReset, "");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c powershell -ep Bypass c:\service\tools\backup\RemoveImageJob.ps1 && powershell -ep Bypass c:\service\tools\backup\AddImageJob.ps1");
                    ChangeStatusBar("Ready");
                    if (cmdOutput.exitCode != 0)
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Commands Execution Failed", "CMD exited with error code: " + cmdOutput.exitCode + Environment.NewLine + "Error log will be shown on next popup");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backup Job Reset Error Log", cmdOutput.outputText);
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "CMD exited with error code: " + cmdOutput.exitCode);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Backup Jobs Reset Successful", "Veritas Backup Jobs has been reset to proper values");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.VeritasJobReset);
                };
                slave.RunWorkerAsync();
            }
        }
        private void BackstoreCsvExportMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.BackstoreCsvExport, "");
                    if (CtrlFunctions.BackstoreCsvExport(connectionPara))
                    {
                        Telemetry.LogFunctionUsage(Globals.Funkcje.BackstoreCsvExport);
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "CSV Export Result", "CSV Files was successfully exported.");
                    }
                    else
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.BackstoreCsvExport, "Failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "CSV Export Result", "CSV Files export failed.");
                    }
                };
                slave.RunWorkerAsync();
            }
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Tools-----------------------------
        private void ServiceManagerMenuItem_Click(object sender, EventArgs e)
        {
            serviceMgr serviceMgr = new serviceMgr();
            serviceMgr.Show(this);
        }
        private void TransactionsXMLToCSVMenuItem_Click(object sender, EventArgs e)
        {
            Logger myLog = new Logger(Globals.Funkcje.TransactionsXMLToCSV, "none", "none");
            Telemetry.LogFunctionUsage(Globals.Funkcje.TransactionsXMLToCSV);
            string filePath = FileController.OpenFileDialog("XML files (*.xml)|*.xml", ref myLog);
            if (filePath == "")
            {
                return;
            }
            XDocument telemetryXml;
            try
            {
                telemetryXml = XDocument.Load(filePath);
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to read xml file", "Toolbox was unable to load xml file with error:" + Environment.NewLine + exp.Message);
                myLog.Add("Unable to read xml file");
                myLog.Add(exp.ToString());
                myLog.SaveLog("ErrorLog");
                return;
            }
            var nodes = telemetryXml.Root.Elements("Transaction");
            if (nodes == null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Transaction missing", "Toolbox was not able to find any transaction in the file. Please check xml file and try again");
                myLog.Add("None transaction nodes in file");
                myLog.SaveLog("WarningLog");
                return;
            }
            string output = "WorkstationID,SequenceNumber,BusinessDayDate,EndDateTime" + Environment.NewLine;
            foreach (XElement node in nodes)
            {
                output += node.Element("WorkstationID").Value + "," + node.Element("SequenceNumber").Value + "," + node.Element("BusinessDayDate").Value + "," + node.Element("EndDateTime").Value + Environment.NewLine;
            }
            if (!FileController.SaveTxtToFile(Globals.userTempLogsPath + "TransactionsXMLToCSV - " + Logger.Datownik() + ".csv", output, ref myLog))
            {
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Conversion successful", @"CSV file successfully saved at T:\temp\ToolBoxLogs");
        }
        private void MobilePosAppKillMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    Logger myLog = new Logger(Globals.Funkcje.MobilePosKill, "", connectionPara.TAG);
                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.MobilePosKill, "");

                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "MobilePos is ded", "MobilePos App was successfully assassinated");
                    Telemetry.LogFunctionUsage(Globals.Funkcje.MobilePosKill);
                };
                slave.RunWorkerAsync();
            }
        }
        private void MonitoringSlayerMenuItem_Click(object sender, EventArgs e)
        {
            MonitoringAnalizer monitoringAnalizer = new MonitoringAnalizer();
            monitoringAnalizer.Show();
        }
        private void updatePackageInvalidMenuItem_Click(object sender, EventArgs e)
        {
            UpdatePackageInvalid updatePackageInvalid = new UpdatePackageInvalid();
            updatePackageInvalid.Show();
        }
        private void StocktakingMenuItem_Click(object sender, EventArgs e)
        {
            Stocktaking stocktaking = new Stocktaking();
            stocktaking.Show();
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/
        //--------------------ADV-----------------------------
        private void WSUSButBETTERMenuItem_Click(object sender, EventArgs e)
        {
            KBinstalation kbinstall = new KBinstalation();
            kbinstall.Show();
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Preferences-----------------------------
        private void LayoutChangeMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem optionButton = sender as ToolStripMenuItem;
            if (!optionButton.Checked)
            {
                Telemetry.LogFunctionUsage(Globals.Funkcje.ChangeLayout);
                userSettings.skin = optionButton.Tag.ToString();
                userSettings.hideNotePad = false;
                ApplyLayout(optionButton.Tag.ToString());
                userSettings.windowSize = Size;
            }
        }
        private void ShowNotepadMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.ToggleNotePad);
            showNotepadMenuItem.Checked = !showNotepadMenuItem.Checked;
            if (showNotepadMenuItem.Checked)
            {
                userSettings.hideNotePad = false;
                ApplyLayout(userSettings.skin);
            }
            else
            {
                userSettings.hideNotePad = true;
                HideNotePad();
                userSettings.windowSize = Size;

            }
        }
        private void stayOnTopMenuItem_Click(object sender, EventArgs e)
        {
            Main.interfejs.TopMost = !Main.interfejs.TopMost;
            Main.interfejs.userSettings.stayOnTop = !Main.interfejs.userSettings.stayOnTop;
        } // StayOnTop MenuItem
        private void resetSettingsMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Reset Settings", "Are you sure you want to wipe your user settings?") != DialogResult.OK)
            {
                return;
            }
            userSettings = new UserSettings();
            userSettings.ApplySettings();
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------About-----------------------------
        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Globals.configPath + @"ToolBox v2.0 Manual.pdf");
        }
        private void ChangeLogMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLog changeLog = new ChangeLog();
            changeLog.Show(this);
        }
        private void ReportSuggestMenuItem_Click(object sender, EventArgs e)
        {
            Report reportForm = new Report();
            reportForm.Show(this);
        }
        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        /**//**//**//**//**//**//* UI CONTROLS *//**//**//**//**//**//**//**//**//**/
        public static string GetTAG()
        {
            return interfejs.textBox_TAG.Text;
        }
        public static void SetTAG(string tag, Color tlo)
        {
            interfejs.textBox_TAG.Text = tag.ToUpper();
            interfejs.textBox_TAG.BackColor = tlo;
            interfejs.Refresh();
        }
        public static string GetIP()
        {
            return interfejs.textBox_IP.Text;
        }
        public static void SetIP(string ip, Color tlo)
        {
            interfejs.textBox_IP.Text = ip.ToUpper();
            interfejs.textBox_IP.BackColor = tlo;
            interfejs.Refresh();
        }
        public static void ChangeTitle(string text)
        {
            interfejs.Text = text;
            interfejs.Refresh();
        }
        public static void ChangeStatusBar(string text)
        {
            interfejs.StatusBarText.Text = text;
            interfejs.Refresh();
        }
        public static void UIReset()
        {
            Main.SetTAG("", SystemColors.Window);
            Main.SetIP("", SystemColors.Control);
            interfejs.textBox_MAC.Text = "";

            interfejs.Text = "TP MasterTool";
            interfejs.Refresh();
        }
        private void DeDifferencesOnOff()
        {
            if (connectionPara.country == "DE")
            {
                networkPanel.Height = 108;
                SubnetStatusGroup.Height = 105;
                BOF01_button.Visible = true;
                BOF09_button.Visible = true;
                return;
            }
            networkPanel.Height = 81;
            SubnetStatusGroup.Height = 80;
            BOF01_button.Visible = false;
            BOF09_button.Visible = false;
        }
        public void ApplyLayout(string skin)
        {
            if (skin == "modern")
            {
                modernLayoutMenuItem.Checked = true;
                oldLayoutMenuItem.Checked = false;
                showNotepadMenuItem.Checked = true;
                this.MinimumSize = new Size(722, 307);
                this.Size = new Size(722, 307);
                this.FormBorderStyle = FormBorderStyle.Sizable;
                networkPanel.Location = new Point(12, 130);
                notepadGroup.Location = new Point(378, 25);
                notepadGroup.Size = new Size(316, 184);
                notepadGroup.Visible = true;
                importNote.Location = new Point(536, 212);
                importNote.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                importNote.Visible = true;
                saveNote.Location = new Point(616, 212);
                saveNote.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                saveNote.Visible = true;
                //localTempButton.Location = new Point(508, 261);
                //localTempButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);

            }
            else if (skin == "old")
            {
                modernLayoutMenuItem.Checked = false;
                oldLayoutMenuItem.Checked = true;
                showNotepadMenuItem.Checked = true;
                this.Size = new Size(765, 350);
                this.MinimumSize = new Size(765, 350);
                this.FormBorderStyle = FormBorderStyle.Sizable;
                networkPanel.Location = new Point(378, 25);
                notepadGroup.Location = new Point(12, 130);
                notepadGroup.Size = new Size(650, 150);
                notepadGroup.Visible = true;
                importNote.Location = new Point(667, 165);
                importNote.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                importNote.Visible = true;
                saveNote.Location = new Point(667, 140);
                saveNote.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                saveNote.Visible = true;
                //localTempButton.Location = new Point(667, 180);
                //localTempButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            }
        }
        public void HideNotePad()
        {
            saveNote.Visible = false;
            importNote.Visible = false;
            notepadGroup.Visible = false;
            if (userSettings.skin == "modern")
            {
                this.MinimumSize = new Size(400, 302);
                this.Size = this.MinimumSize;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

            if (userSettings.skin == "old")
            {
                this.MinimumSize = new Size(765, 195);
                this.Size = this.MinimumSize;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }
        public void DisableUI()
        {
            List<string> wyjatki = new List<string> { "Last Connected", "Ping's", "JPOSRFID Logs Secure", "TP.Reports Regen+Zip", "Invalid Transfer", "TransactionsXML to CSV", "MonitoringSlayer", "UpdatePackage Invalid Check", "Stocktaking", "WSUS but BETTER", "Random Collection of Randomness" };
            foreach (ToolStripMenuItem menu in menuStrip1.Items)
            {
                if (menu.Text == "Preferences") { break; }
                foreach (var submenuItem in menu.DropDownItems)
                {
                    if (submenuItem is ToolStripMenuItem)
                    {
                        if (wyjatki.Contains((submenuItem as ToolStripMenuItem).Text)) { continue; }
                        (submenuItem as ToolStripMenuItem).Enabled = false;
                    }
                }
            }
        }
        private void EnableUI()
        {
            SetEnableMenuItems(uniwersalMenuItems, true);
            if (connectionPara.deviceType == "TPS")
            {
                SetEnableMenuItems(tpsMenuItems, true);
            }
            else if (connectionPara.deviceType == "STP")
            {
                SetEnableMenuItems(stpMenuItems, true);
            }
            else
            {
                SetEnableMenuItems(stpMenuItems.Concat(tpsMenuItems).ToList(), true);
            }
            if (connectionPara.IPMode)
            {
                SetEnableMenuItems(ipNotSupported, false);
            }
        }
        private void SetEnableMenuItems(List<string> itemList, bool state)
        {
            foreach (ToolStripMenuItem menu in menuStrip1.Items)
            {
                if (menu.Text == "Preferences") { break; }
                foreach (var submenuItem in menu.DropDownItems)
                {
                    if (submenuItem is ToolStripMenuItem)
                    {
                        if (itemList.Contains((submenuItem as ToolStripMenuItem).Text))
                        {
                            (submenuItem as ToolStripMenuItem).Enabled = state;
                        }
                    }
                }
            }
        }
        /**//**//**//**//**//**//* FORM CONTROLS *//**//**//**//**//**//**//**//**//**/
        private void StartButton_Click(object sender, EventArgs e)
        {
            string tempTAG = Main.GetTAG().Trim();
            Main.ChangeStatusBar("Working...");
            Main.UIReset();
            DisableUI();
            Main.SetTAG(tempTAG, SystemColors.Window);

            Telemetry.LogOnMachineAction(tempTAG, Globals.Funkcje.Initialization, "");

            connectionPara = ConnectionPara.EstablishConnection(tempTAG);
            if (connectionPara == null)
            {
                Main.ChangeStatusBar("Ready");
                Telemetry.LogOnMachineAction(tempTAG, Globals.Funkcje.Error, Main.GetIP());
                return;
            }

            if (connectionPara.IP == "DNS ERROR")
            {
                Telemetry.LogOnMachineAction(tempTAG, Globals.Funkcje.Error, Main.GetIP());
                if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.TAG + " - DNS Error", "Would you like to connect to this host via IP?") != DialogResult.OK)
                {
                    Main.ChangeStatusBar("Ready");
                    return;
                }
                string ipGuess = CtrlFunctions.GetIpFromDNSError(connectionPara);
                if (ipGuess == "DNS ERROR")
                {
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to find store IP scope automatically", "ToolBox was unable to find store IP scope based on another device in that store" + Environment.NewLine + "Please check if whole store isn't offline or if there are problems with store router");
                    Main.ChangeStatusBar("Ready");
                    return;
                }
                textBox_TAG.Text = ipGuess;
                StartButton_Click(this, e);
                return;
            }


            interfejs.DeDifferencesOnOff();

            CtrlFunctions.SubNetScan(connectionPara);
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out CtrlFunctions.CmdOutput cmdOutput))
            {
                Main.SetTAG(connectionPara.TAG, Globals.errorColor);
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Connection Error", "Unable to map host drive through net use:" + Environment.NewLine + cmdOutput.errorOutputText);
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to map host drive");
                Main.ChangeStatusBar("Ready");
                return;
            }

            Main.ChangeTitle("TP MasterTool - " + connectionPara.TAG + " (" + connectionPara.IP + ")");
            Main.SetTAG(connectionPara.TAG, Color.LightGreen);
            EnableUI();
            Main.ChangeStatusBar("Ready");
            Telemetry.LogFunctionUsage(Globals.Funkcje.Initialization);
            userSettings.AddNewRecent(connectionPara.TAG);
        } // INITIALIZE BUTTON
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Shutdown.ShutdownProcedure();
        } // onExit 
        private void TextBox_TAG_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                StartButton_Click(this, e);
            }
        } // Initialize on Enter Press
        private void TextBox_TAG_TextChanged(object sender, EventArgs e)
        {
            textBox_TAG.BackColor = SystemColors.Window;
        } // Reset TAG TextBox on change
        private void Main_ResizeEnd(object sender, EventArgs e)
        {
            userSettings.windowSize = this.Size;
        } // Save window size after resize
        private void FiveMinTimer_Tick(object sender, EventArgs e)
        {
            StartUp.MaintenanceModeCheck(ref myLog);

            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    userSettings.SaveUserSettingsToXml(); // autosave user settings

                    Globals.telemetryLogPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\Stats\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MMMM") + @"\"; // path refresh to keep up with changing date
                    Globals.machineLogPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\MachineLogs\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MMMM") + @"\"; // path refresh to keep up with changing date
                    try
                    {
                        System.IO.Directory.CreateDirectory(Globals.telemetryLogPath);
                        System.IO.Directory.CreateDirectory(Globals.machineLogPath);
                    }
                    catch (Exception exp)
                    {
                        Logger.QuickLog(Globals.Funkcje.FiveMinTimer, "Creating Folders", "MainForm", "ErrorLog", exp.ToString());
                    }

                };
                slave.RunWorkerAsync();
            }

        } // Concurrent tasks every 5min
        private void HourTimer_Tick(object sender, EventArgs e)
        {
            Logger myLog = new Logger(Globals.Funkcje.VersionLog, "", "");
            if (VersionControl.IsUpdateAvailable(ref myLog) && !updatePopup)
            {
                updatePopup = true;
                CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "Update available", @"There is newest version of toolbox available to download." + "\n" + "Please restart ToolBox to receive update");
                updatePopup = false;
            }
            if (versionWarning == 1)
            {
                versionWarning++;
                StartButton.Enabled = false;
                menuStrip1.Enabled = false;
                textBox_TAG.KeyDown -= TextBox_TAG_KeyDown;
                Logger.QuickLog(Globals.Funkcje.VersionLog, "", "", "VersionControlLog", "User was lockout because of ToolBox version below minimal acceptable");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Minimal Version Not Satisfied - SoftLocked", "Your ToolBox version don't meet the minimal requirements. Please restart ToolBox and preform the update");
            }
            else if (VersionControl.IsBelowMinimalAcceptedVersion(ref myLog) && versionWarning == 0)
            {
                versionWarning++;
                Logger.QuickLog(Globals.Funkcje.VersionLog, "", "", "VersionControlLog", "User was warmed because of ToolBox version below minimal acceptable");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Softlock Warning", "Your ToolBox version don't meet the minimal requirements. Please restart the ToolBox and preform the update");
            }

        } // Concurrent tasks every 1h
        private void Notepad_TextChanged(object sender, EventArgs e)
        {
            userSettings.notePadLines = notepad.Text;
        } // Save notepad content after change

        private void randomCollectionOfRandomnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MassEmergancy().Show();
        }
    }
}
