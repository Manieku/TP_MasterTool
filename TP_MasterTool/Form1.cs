using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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
            "OEMInst Logs",
            "TP.Net Logs",
            //Diagnostics//
            "Get S.M.A.R.T",
            "Drives Space Info",
            "Install WinDirStat",
            "Get Folders Size",
            "Scan Store Endpoints",
            "System Boot Time",
            "DumpFile Analyse",
            "SQL Queries",
            "DHCP Scope Info",
            //Fixes//
            //Tools//
            "Service Manager" // not working for now
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
            "TFTPD Logs",
            "Installation Logs",
            "Get APC Logs",
            "PDCU Data Error Secure",
            //Diagnostics//
            "Backup Checker",
            "EoD Checker",
            //Fixes//
            "APC Service Fix",
            "Veritas Backup Job Reset",
            "Backstore CSV Export",
            //Tools//
            "MiniLogger Data Collect"
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
            "Till Local Cache Clear",
            "Parked TX Move",
            "TSE Webservice Restart",
            "Signator Reset",
            //Tools//
            "MobilePos App Kill"
        };
        List<string> ipNotSupported = new List<string>
        {
            "Local Storage (Till)",
            "Scan Store Endpoints",
            "Backup Checker",
            "EoD Checker",
            "POS Colon : Fix",
            "Till Local Cache Clear"
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
            
            
            
            
            
            //CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c sc query apcpbeagent | find /i \"state\"");
            //if (cmdOutput.exitCode != 0)
            //{
            //    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "RCMD Error", "Unable to get last boot time - RCMD exited with error:" +
            //        Environment.NewLine + cmdOutput.errorOutputText);
            //    return;
            //}
            //CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "Output", cmdOutput.outputText.Split(':').Last());






            //List<string> errors = new List<string>();
            //foreach (string line in File.ReadAllLines(@".\input.txt"))
            //{
            //    string[] numbers = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //    for (int i = 1; i < numbers.Length; i++)
            //    {
            //        if (Globals.storeId2Tag.ContainsKey(numbers[i]))
            //        {
            //            File.AppendAllText(@".\Dates\" + Globals.storeId2Tag[numbers[i]] + ".txt", numbers[0] + Environment.NewLine);
            //        }
            //        else if (!errors.Contains(numbers[i]))
            //        {
            //            errors.Add(numbers[i]);
            //        }
            //    }
            //}
            //File.AppendAllLines(@".\errors.txt", errors);

            //List<string> errors = new List<string>();
            //foreach (string line in File.ReadAllLines(@".\input.txt"))
            //{
            //    string[] numbers = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (Globals.storeId2Tag.ContainsKey(numbers[0]))
            //    {
            //        File.AppendAllText(@".\Dates\" + Globals.storeId2Tag[numbers[0]] + ".txt", numbers[1] + Environment.NewLine);
            //    }
            //    else if (!errors.Contains(numbers[0]))
            //    {
            //        errors.Add(numbers[0]);
            //    }
            //}
            //File.AppendAllLines(@".\errors.txt", errors);

            //if (File.Exists(@".\input.txt"))
            //{
            //    foreach (string line in File.ReadAllLines(@".\input.txt"))
            //    {
            //        string[] numbers = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //        File.AppendAllText(@".\Dates\" + numbers[0] + ".txt", numbers[1] + Environment.NewLine);
            //    }

            //    foreach (string file in Directory.GetFiles(@".\Dates"))
            //    {
            //        File.AppendAllText(@".\dates_tps.txt", Path.GetFileNameWithoutExtension(file) + Environment.NewLine);
            //    }
            //}
            //else
            //{
            //    string output = "";
            //    foreach (string file in Directory.GetFiles(@".\Dates"))
            //    {
            //        foreach (string line in File.ReadAllLines(file))
            //        {
            //            output += Path.GetFileNameWithoutExtension(file) + "," + line + Environment.NewLine;
            //        }
            //    }
            //    File.WriteAllText(@".\output.csv", output);
            //}


            //CtrlFunctions.EncryptFile(@".\mojepasy.txt", "cycuszki", Globals.configPath + "credentials.crypt");
            //MessageBox.Show("krypto krypto superman lezy");


            //Logger.GeneratePortalReport(@".\EoD_Abort_Test_Report.txt", @".\EoD_Abort_Test_AddInfo.txt", @".\logo.txt", @".\output.txt");
        }
        //--------------------/UI Controls/---------------------------
        private void GetMAC_button_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = interfejs.connectionPara;
            textBox_MAC.Text = "Stealing MAC...";
            getMAC_button.Enabled = false;
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    CtrlFunctions.CmdOutput macCmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
                    if(macCmdOutput.exitCode != 0 || macCmdOutput.outputText == "")
                    {
                        textBox_MAC.Text = "ERROR";
                        getMAC_button.Enabled = true;
                        return;
                    }
                    try
                    {
                        CtrlFunctions.CmdOutput dhcpCmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c powershell -command \"Get-DhcpServerv4Reservation -ComputerName 10.58.50.13 -IPAddress " + connectionPara.IP + " | select ClientId | ft -HideTableHeaders\"");
                        if (macCmdOutput.outputText.Trim().ToLower().Replace(":", "-") != dhcpCmdOutput.outputText.Trim())
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "DHCP Warning", "DHCP MAC Reservation for this IP address is different than MAC of this station. Please check DHCP and correct any errors.");
                        }
                    }
                    catch(Exception exp)
                    {
                        Logger.QuickLog(Globals.Funkcje.GetMAC, connectionPara.IP, connectionPara.hostname, "WarningLog", "Get DHCP info error:" + Environment.NewLine + exp.ToString());
                    }
                    textBox_MAC.Text = macCmdOutput.outputText.Replace(":", "-").Trim();
                    getMAC_button.Enabled = true;
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetMAC, macCmdOutput.outputText.Replace(":", "-").Trim().ToUpper());
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
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.SaveNote, "");
            FileController.SaveTxtDialog(string.Join(Environment.NewLine, notepad.Lines), ref myLog);
        }
        private void ImportNote_Click(object sender, EventArgs e)
        {
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.SaveNote, "");
            Telemetry.LogFunctionUsage(Globals.Funkcje.SaveNote);
            try
            {
                notepad.Lines = System.IO.File.ReadAllLines(FileController.OpenFileDialog("Text files (*.txt)|*.txt"));
            }
            catch { }
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------/PING BUTTONS/---------------------------
        private void PingButton_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.PingButton, (sender as Button).Text);
            Process.Start("cmd.exe", "/c ping " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], (sender as Button).Tag.ToString()) + " && pause");
        }
        private void PRNPingButton_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.PingButton, (sender as Button).Text);
            Process.Start("cmd.exe", "/c ping " + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], (connectionPara.IPbytes[2] + 1).ToString(), (sender as Button).Tag.ToString()) + " && pause");
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //---------------------RemoteCMDs ToolBarMenu-----------------------
        private void RCMDMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.RCMD, "");
            Process.Start("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd");
        }
        private void RebootDeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "CAUTION", "Are you sure you want to RESTART device: " + connectionPara.hostname + "?") == DialogResult.Cancel)
            {
                return;
            }
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.RemoteRestart, "");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c shutdown /r");
            if (cmdOutput.exitCode != 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Command Execution Error", "RCMD encounter a error during execution:" + Environment.NewLine + cmdOutput.errorOutputText);
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "CMD Failed to restart host");
                Logger.QuickLog(Globals.Funkcje.RemoteRestart, "", connectionPara.hostname, "ErrorLog", cmdOutput.errorOutputText);
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Success", "Command was successful send to remote host");
        }
        private void TracertMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.Tracert, "");
            Process.Start("cmd.exe", "/c tracert " + connectionPara.fullNetworkName + "&& pause");
        }
        private void DNSRestoreMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.DNSRestore, "");
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Restoring DNS");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c ipconfig /flushdns && ipconfig /renew && ipconfig /registerdns && gpupdate /force");
                    ChangeStatusBar("Ready");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "RCMD exited with code: " + cmdOutput.exitCode);
                        Logger.QuickLog(Globals.Funkcje.DNSRestore, "", connectionPara.hostname, "ErrorLog", "RCMD exited with code: " + cmdOutput.exitCode + "\n" + cmdOutput.errorOutputText);
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Execution Error", "RCMD exited with error while executing commands:\n" + cmdOutput.errorOutputText);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "DNS Restored", "Executed:\n- DNS Resolver Cache flushed\n- IP renewed\n- DNS registered\n- Group and user policies updated\n\nPlease check host in 5 to 20 minutes if DNS error is solved");
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
            CtrlFunctions.OpenFolder(connectionPara.fullNetworkName, (sender as ToolStripMenuItem).Tag.ToString());
        }

        //------------------APC--------------------
        private void APCMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.OpenWebAPC, "");
            Process.Start(@"Https://" + connectionPara.IP + @":6547/");
        }
        private void ServerRTMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.OpenServerRT, "");
            Process.Start(@"Https://" + String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2]) + @".178/");
        }

        //------------------TP Raports--------------------
        private void LocalStorageTillMenuItem_Click(object sender, EventArgs e)
        {
            CtrlFunctions.OpenFolder(connectionPara.fullNetworkName, @"c$\Users\" + connectionPara.country + connectionPara.storeNr + connectionPara.storeType + @".AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage");
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
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    if (tixnr == "")
                    {
                        return;
                    }
                    if(!FileController.ZipAndStealFolder(tixnr, "TPLogs", @"\\" + connectionPara.fullNetworkName + @"\d$\TPDotnet\Log", @"D:\TPDotnet\Log", connectionPara, out string outputFilePath))
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Secure Log Error", outputFilePath);
                        return;
                    }
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.hostname + " - Logs secured", "Log files were successfully zipped and secured in WNI folder." + Environment.NewLine + "Do you want to download then on your drive?") == DialogResult.OK)
                    {
                        if (!System.IO.File.Exists(outputFilePath))
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Download File Error", "ToolBox lost connection to host drive, please initialize it anew and copy logs manually from:" + Environment.NewLine + outputFilePath);
                            return;
                        }
                        if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), true, out Exception copyExp))
                        {
                            if (copyExp != null)
                            {
                                Logger.QuickLog(Globals.Funkcje.ZipAndSteal, "TPLogs", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                            }
                        }
                    }
                    Process.Start("explorer.exe", Path.GetDirectoryName(outputFilePath));

                };
                slave.RunWorkerAsync();
            }

        }
        private void S4FiscalSecureMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;

            if (!System.IO.Directory.Exists(@"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\Service Plus"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No S4 folder found", "Can't find any S4 folder on target host");
                return;
            }
            string[] directories = System.IO.Directory.GetDirectories(@"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\Service Plus", "S4Fiscal???", System.IO.SearchOption.TopDirectoryOnly);
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

            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    if (tixnr == "")
                    {
                        return;
                    }
                    if (!FileController.ZipAndStealFolder(tixnr, "S4Fiscal", @"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\Service Plus\" + Path.GetFileNameWithoutExtension(directories[0]), @"C:\Program Files (x86)\Service Plus\" + System.IO.Path.GetFileNameWithoutExtension(directories[0]), connectionPara, out string outputFilePath))
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Secure Log Error", outputFilePath);
                        return;
                    }
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.hostname + " - Logs secured", "Log files were successfully zipped and secured in WNI folder." + Environment.NewLine + "Do you want to download then on your drive?") == DialogResult.OK)
                    {
                        if (!System.IO.File.Exists(outputFilePath))
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Download File Error", "ToolBox lost connection to host drive, please initialize it anew and copy logs manually from:" + Environment.NewLine + outputFilePath);
                            return;
                        }
                        if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), true, out Exception copyExp))
                        {
                            if (copyExp != null)
                            {
                                Logger.QuickLog(Globals.Funkcje.ZipAndSteal, "S4Fiscal", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                            }
                        }
                    }
                    Process.Start("explorer.exe", Path.GetDirectoryName(outputFilePath));

                };
                slave.RunWorkerAsync();
            }
        }
        private void TSELogsSecureMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    if (tixnr == "")
                    {
                        return;
                    }
                    if (!FileController.ZipAndStealFolder(tixnr, "TSELogs", @"\\" + connectionPara.fullNetworkName + @"\c$\ProgramData\DieboldNixdorf\TSE-Webservice\log", @"C:\ProgramData\DieboldNixdorf\TSE-Webservice\log", connectionPara, out string outputFilePath))
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Secure Log Error", outputFilePath);
                        return;
                    }
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.hostname + " - Logs secured", "Log files were successfully zipped and secured in WNI folder." + Environment.NewLine + "Do you want to download then on your drive?") == DialogResult.OK)
                    {
                        if (!System.IO.File.Exists(outputFilePath))
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Download File Error", "ToolBox lost connection to host drive, please initialize it anew and copy logs manually from:" + Environment.NewLine + outputFilePath);
                            return;
                        }
                        if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), true, out Exception copyExp))
                        {
                            if (copyExp != null)
                            {
                                Logger.QuickLog(Globals.Funkcje.ZipAndSteal, "TSELogs", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                            }
                        }
                    }
                    Process.Start("explorer.exe", Path.GetDirectoryName(outputFilePath));

                };
                slave.RunWorkerAsync();
            }
        }
        private void pDCUDataErrorSecureMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    if (Directory.GetFiles(@"\\" + connectionPara.fullNetworkName + @"\d$\StoreApps\CarsData\Cars\pdcudata\error").Length == 0)
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "PDCU Data Secure Error", "No error files found in folder");
                        return;
                    }

                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    if (tixnr == "")
                    {
                        return;
                    }
                    if (!FileController.ZipAndStealFolder(tixnr, "PdcuError", @"\\" + connectionPara.fullNetworkName + @"\d$\StoreApps\CarsData\Cars\pdcudata\error", @"D:\StoreApps\CarsData\Cars\pdcudata\error", connectionPara, out string outputFilePath))
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Secure Log Error", outputFilePath);
                        return;
                    }
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.hostname + " - Logs secured", "Log files were successfully zipped and secured in WNI folder." + Environment.NewLine + "Do you want to download then on your drive?") == DialogResult.OK)
                    {
                        if (!System.IO.File.Exists(outputFilePath))
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Download File Error", "ToolBox lost connection to host drive, please initialize it anew and copy logs manually from:" + Environment.NewLine + outputFilePath);
                            return;
                        }
                        if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), true, out Exception copyExp))
                        {
                            if (copyExp != null)
                            {
                                Logger.QuickLog(Globals.Funkcje.ZipAndSteal, "PdcuError", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                            }
                        }
                    }
                    Process.Start("explorer.exe", Path.GetDirectoryName(outputFilePath));

                };
                slave.RunWorkerAsync();
            }
        }

        //-----------------APC Logs-------------------
        private void ApcLogsMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Main.ChangeStatusBar("Working...");
            Dictionary<int, string> idMapping = new Dictionary<int, string>();
            try
            {
                foreach (string line in File.ReadAllLines(Globals.configPath + "ApcEventIdMapping.txt"))
                {
                    string[] temp = line.Split('\t');
                    idMapping.Add(int.Parse(temp[0]), temp[1]);
                }
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.GetApcLogs, "Dictionary Creation", connectionPara.hostname, "CriticalError", exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Dictionary Creation Error", "Toolbox wasn't able to map Apc events IDs to dictionary:" + Environment.NewLine + exp.Message);
                Main.ChangeStatusBar("Ready");
                return;
            }
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetApcLogs, "");
            if(!File.Exists(@"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\APC\PowerChute Business Edition\agent\DataLog"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "No logs found", "No logs found, please check if software is properly installed.");
                Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.Error, "No logs found");
                return;
            }
            if (!FileController.CopyFile(@"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\APC\PowerChute Business Edition\agent\DataLog", @".\Logs\" + connectionPara.hostname + " - APC DataLog.txt", false, out Exception copyExp))
            {
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "DataLogs: " + copyExp.Message);
                Logger.QuickLog(Globals.Funkcje.GetApcLogs, "DataLog", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "Failed to download Data logs with error: " + copyExp.Message);
            }
            if (!FileController.CopyFile(@"\\" + connectionPara.fullNetworkName + @"\c$\Windows\System32\winevt\Logs\Application.evtx", @".\Logs\Windows\" + connectionPara.hostname + " - Application Log.evtx", true, out copyExp))
            {
                if (copyExp != null)
                {
                    Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "EventLogs: " + copyExp.Message);
                    Logger.QuickLog(Globals.Funkcje.GetApcLogs, "EventLog", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                }
                Main.ChangeStatusBar("Ready");
                return;
            }
            EventLogQuery eventsQuery = new EventLogQuery(@".\Logs\Windows\" + connectionPara.hostname + " - Application Log.evtx", PathType.FilePath, "*[System/Provider/@Name=\"APCPBEAgent\"]");
            using (var reader = new EventLogReader(eventsQuery))
            {
                string output = "";
                EventRecord record;
                while ((record = reader.ReadEvent()) != null)
                {
                    output += record.TimeCreated + "\t" + idMapping[record.Id] + Environment.NewLine;
                }
                if (!FileController.SaveTxtToFile(@".\Logs\" + connectionPara.hostname + " - APC EventLog.txt", output, out Exception saveExp))
                {
                    Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Save EventLog: " + saveExp.Message);
                    Logger.QuickLog(Globals.Funkcje.GetApcLogs, "Save EventLog", connectionPara.hostname, "ErrorLog", saveExp.ToString());
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered while saving logs:" + Environment.NewLine + saveExp.Message);
                    return;
                }
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Logs Downloaded", "APC Logs were successfully downloaded and saved in ToolBox Logs folder");
            Main.ChangeStatusBar("Ready");
        }


        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/
        private void QuickPingTAGMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogUserAction(GetTAG(), Globals.Funkcje.DiagnosticPing, GetTAG());
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiagnosticPing);
            Process.Start("cmd.exe", "/c ping " + GetTAG() + ".candadnpos.biz" + @" && pause");
        }
        private void PingWithLoad_TAGMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogUserAction(GetTAG(), Globals.Funkcje.DiagnosticPing, GetTAG());
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiagnosticPing);
            Process.Start("cmd.exe", "/c ping " + GetTAG() + ".candadnpos.biz" + @" -t -l 2048");
        }
        private void SavePingToTxtMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusBar("Working...");
            Telemetry.LogUserAction(GetTAG(), Globals.Funkcje.SavePingToTxt, GetTAG());
            Telemetry.LogFunctionUsage(Globals.Funkcje.SavePingToTxt);

            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c ping " + GetTAG() + ".candadnpos.biz");
            if (cmdOutput.exitCode != 0)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "CMD encountered error", "Unfortunately command encountered an unexpected error while execution." + Environment.NewLine + "Error: " + cmdOutput.errorOutputText);
                Main.ChangeStatusBar("Ready");
                return;
            }
            if(!FileController.SaveTxtToFile(@".\Logs\Ping(" + GetTAG() + ") - " + Logger.Datownik() + ".txt", "Ping executed at: " + DateTime.Now.ToString() + Environment.NewLine + cmdOutput.outputText, out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file:" + Environment.NewLine + saveExp.Message);
            }
            ChangeStatusBar("Ready");
        }

        //------------SMARTY--------------------------
        private void GetSMARTMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                ConnectionPara connectionPara = Main.interfejs.connectionPara;
                slave.DoWork += (s, args) =>
                {
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetSMART, "");
                    sMARTToolStripMenuItem.Enabled = false;
                    ChangeStatusBar("Reading SMART values");
                    if (!CtrlFunctions.Smarty(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, errorMsg);
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SMART Function Error", errorMsg);
                    }
                    else
                    {
                        if (!FileController.MoveFile(@"\\" + connectionPara.fullNetworkName + @"\c$\SMART\DiskInfo.txt", @".\Logs\SMART\SMART - " + connectionPara.hostname + ".txt", false, out Exception moveExp))
                        {
                            Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.GetSMART, "Retrieving result error");
                            Logger.QuickLog(Globals.Funkcje.GetSMART, @"\\" + connectionPara.hostname + @"\c$\SMART\DiskInfo.txt | " + @".\Logs\SMART\SMART - " + connectionPara.hostname + ".txt", connectionPara.hostname, "WarningLog", "ToolBox wasn't able to copy command output back from targeted host." + Environment.NewLine + moveExp.ToString());
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Retrieving result error", @"ToolBox wasn't able to copy command output back from targeted host. You can check it manually at \\" + connectionPara.hostname + @"\C$\SMART\DiskInfo.txt");
                        }
                        else
                        {
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "SMART Values Saved", @"Disc SMART values successfully saved at .\Logs\SMART\SMART - " + connectionPara.hostname + ".txt");
                            Process.Start(@".\Logs\SMART\SMART - " + connectionPara.hostname + ".txt");
                        }
                    }

                    if(!CtrlFunctions.MapEndpointDrive(ref connectionPara, out CtrlFunctions.CmdOutput cmdOutput))
                    {
                        Logger.QuickLog(Globals.Funkcje.DeleteLock, @"\\" + connectionPara.hostname + @"\c$\SMART\smart.lock", connectionPara.hostname, "CriticalLog", "Error Deleting lock: Unable to map drive before delete");
                    }
                    CtrlFunctions.DeleteLock(@"\\" + connectionPara.fullNetworkName + @"\c$\SMART\smart.lock");

                    ChangeStatusBar("Ready");
                    sMARTToolStripMenuItem.Enabled = true;
                };
                slave.RunWorkerAsync();
            }
        }

        //------------Disc managment--------------------------
        private void DrivesSpaceInfoMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.DiscSpaceInfo, "");
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string[] drives = { "c", "d", "e", "f" };
                    string output = "";
                    foreach (string letter in drives)
                    {
                        if (System.IO.Directory.Exists(@"\\" + connectionPara.fullNetworkName + @"\" + letter + @"$"))
                        {
                            output += "Drive " + letter.ToUpper() + ":\\ Informations:" + "\n" + CtrlFunctions.GetDiskSpaceInfo(letter, connectionPara, out _, out _) + "\n\n";
                        }
                    }
                    if (output == "")
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Disc Connection Error");
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
            if (File.Exists(@"\\" + connectionPara.fullNetworkName + @"\c$\temp\windirstat.exe"))
            {
                return;
            }
            ChangeStatusBar("Copying...");
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.WinDirStatInstall, "");
            if (!FileController.CopyFile(Globals.toolsPath + "windirstat.exe", @"\\" + connectionPara.fullNetworkName + @"\c$\temp\windirstat.exe", false, out Exception copyExp))
            {
                Logger.QuickLog(Globals.Funkcje.WinDirStatInstall, "Copying exe to host", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Copying windirstat failed");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Copying Error", "ToolBox encountered error while trying to copy winDirStat:" + Environment.NewLine + copyExp.Message);
            }
            ChangeStatusBar("Ready");
        }
        private void GetFoldersSize(object sender, EventArgs e)
        {
            ChangeStatusBar("Working");
            string drive = null;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Select minidump to analyse", new string[] {"C", "D", "F"}))
            {
                var result = dropDownSelect.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return;
                }
                drive = dropDownSelect.ReturnValue1;            //values preserved after close
            }

            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetFoldersSize, "");
            string[] outputFiles = { @"result.csv", @"result.png" };
            string[] toCopy = { Globals.toolsPath + "wiztree64.exe", Globals.toolsPath + "wiztreestart_" + drive + ".cmd" };
            try
            {
                foreach(string file in outputFiles)
                {
                    File.Delete(@"\\" + connectionPara.fullNetworkName + @"\c$\temp\" + file);
                }
            }
            catch
            {
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to delete old results from temp");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Old results error", "Toolbox wasn't able to delete old results. Please delete result.csv and result.png from temp folder and try again.");
                return;
            }
            foreach (string file in toCopy)
            {
                if (!FileController.CopyFile(file, @"\\" + connectionPara.fullNetworkName + @"\c$\temp\" + Path.GetFileName(file), false, out Exception copyExp))
                {
                    Logger.QuickLog(Globals.Funkcje.GetFoldersSize, "Copying exe to host", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                    Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Copying WizTree failed");
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Copying Error", "ToolBox encountered error while trying to copy " + Path.GetFileName(file) + Environment.NewLine + copyExp.Message);
                    ChangeStatusBar("Ready");
                    return;
                }
            }
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\temp && .\" + Path.GetFileName(toCopy[1]));
            if(cmdOutput.exitCode !=0)
            {
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "WizTree execution error: " + cmdOutput.exitCode);
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "WizTree execution error", "RCMD failed to execute WizTree." + Environment.NewLine + cmdOutput.errorOutputText);
                ChangeStatusBar("Ready");
                return;
            }
            Thread.Sleep(2000);
            try
            {
                File.Delete(@"\\" + connectionPara.fullNetworkName + @"\c$\temp\wiztree64.exe");
                File.Delete(@"\\" + connectionPara.fullNetworkName + @"\c$\temp\" + Path.GetFileName(toCopy[1]));
            }
            catch (Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "error", exp.Message);
            }
            bool error = false;
            List<string> localFiles = new List<string>();
            string date = Logger.Datownik();
            foreach(string file in outputFiles)
            {
                localFiles.Add(Globals.userTempLogsPath + connectionPara.hostname + "_" + date + "_" + Path.GetFileName(file));
                if(!FileController.MoveFile(@"\\" + connectionPara.fullNetworkName + @"\c$\temp\" + file, Globals.userTempLogsPath + connectionPara.hostname + "_" + date + "_" + Path.GetFileName(file), false, out Exception moveExp))
                {
                    Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to move output file: " + moveExp.Message);
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to retrieve output files", "Toolbox wasn't able to retrieve outputfiles from the host. Please check temp folder." + Environment.NewLine + moveExp.Message);
                    error = true;
                }
            }
            if(!error)
            {
                foreach(string file in localFiles)
                {
                    Process.Start(file);
                }
            }
            ChangeStatusBar("Ready");
        }
        //------------------Endpoint Scan--------------------
        private void ScanEndpointsMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusBar("Scanning...");
            string output = "Endpoint Status:" + Environment.NewLine;
            string db = "TPCentralDB";
            if(connectionPara.deviceType != "TPS")
            {
                db = "TPPosDB";
            }
            if (CtrlFunctions.SqlGetInfo(connectionPara.fullNetworkName, db, "select szComputerName from Workstation where szWorkstationID like '" + connectionPara.country + connectionPara.storeNr + "%' and lInstanceNmbr=0", out string dbOutput))
            {
                string[] endpointList = dbOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in endpointList)
                {
                    string tag = line.Split(':')[1].Trim().Substring(0, 12);
                    try
                    {
                        if (new Ping().Send(tag + ".candadnpos.biz", 4000, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")).Status == IPStatus.Success)
                        {
                            output += tag + " - Online" + Environment.NewLine;
                        }
                        else
                        {
                            if (new Ping().Send(CtrlFunctions.DnsGetIP(tag), 4000, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")).Status == IPStatus.Success)
                            {
                                output += tag + " DNS Issue - IP Online" + Environment.NewLine;
                            }
                            else
                            {
                                output += tag + " - Offline" + Environment.NewLine;
                            }
                        }
                    }
                    catch
                    {
                        output += tag + " - Offline" + Environment.NewLine;
                    }
                }
            }
            else
            {
                output = dbOutput;
            }
            ChangeStatusBar("Ready");
            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "Endpoint Scan", output);
        }

        //------------------BootTime--------------------
        private void SystemBootTimeMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetSystemBootTime, "");
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Working");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c systeminfo | find /i \"Boot Time\"");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "RCMD Error");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "RCMD Error", "Unable to get last boot time - RCMD exited with error:" +
                            Environment.NewLine + cmdOutput.errorOutputText);
                        ChangeStatusBar("Ready");
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
            new BackupCheck().Show();
        } //dont support IP MODE

        //------------------EoD Checker--------------------
        private void EoDCheckerMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Logger myLog = new Logger(Globals.Funkcje.EodCheck, "None", connectionPara.hostname);
            string[] files;
            try
            {
                myLog.Add("Reading log files");
                files = new DirectoryInfo(@"\\" + connectionPara.fullNetworkName + @"\d$\TPDotnet\Log").EnumerateFiles("LOG_*_" + connectionPara.hostname + "_????????_??????_MANUALEOD.xml").OrderByDescending(file => file.CreationTime).Select(file => file.FullName).ToArray();
            }
            catch (Exception exp)
            {
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
                string result = "";
                try
                {
                    result = XDocument.Load(file).Root.Element("BATCHRESULT").Element("szFinalResult").Value;
                }
                catch
                {
                    result = "Error reading file";
                }
                fileList.Add(System.IO.Path.GetFileName(file) + " - " + result);
            }

            string selectedFile = null;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Select Eod Raport to analyse", fileList.ToArray()))
            {
                if (dropDownSelect.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                selectedFile = dropDownSelect.ReturnValue1;            //values preserved after close
            }
            myLog.Add("Selected file: " + selectedFile);
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.EodCheck, selectedFile);
            if(selectedFile.Contains("Error"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Faulty file selected", "Selected file can't be read, please check manually");
                return;
            }
            XDocument eodXml;
            string output = "";
            try
            {
                eodXml = XDocument.Load(@"\\" + connectionPara.fullNetworkName + @"\d$\TPDotnet\Log\" + selectedFile.Split(' ')[0]);
                foreach (XElement node in eodXml.Root.Elements("ACTIVITYLOG"))
                {
                    if (node.Element("szFinalResult").Value == "AbortedCancel" || node.Element("szFinalResult").Value == "Failure")
                    {
                        output += node.ToString() + Environment.NewLine;
                    }
                }
                output += Environment.NewLine + eodXml.Root.Element("BATCHRESULT").ToString();
            }
            catch (Exception exp)
            {
                myLog.Add("Error reading log file");
                myLog.Add(exp.ToString());
                myLog.SaveLog("ErrorLog");
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Error reading log file");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error reading log file", "ToolBox wasn't able to read log file:" + Environment.NewLine + exp.Message);
                return;
            }

            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Eod Result", output);
        } //dont support IP MODE

        //------------------DumpFile analyse--------------------
        private void dumpFileAnaliseMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;

            System.IO.FileInfo[] files;
            Logger myLog = new Logger(Globals.Funkcje.MiniDumpAnalyser, "", connectionPara.hostname);

            try
            {
                files = new System.IO.DirectoryInfo(@"\\" + connectionPara.fullNetworkName + @"\c$\Windows\Minidump").GetFiles("*.dmp").OrderBy(p => p.CreationTime).ToArray();
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
                if (result != DialogResult.OK)
                {
                    return;
                }
                selectedMinidump = dropDownSelect.ReturnValue1;            //values preserved after close

            }
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.MiniDumpAnalyser, selectedMinidump);

            if (!CtrlFunctions.AnalyseMiniDump(selectedMinidump, @".\Logs\" + connectionPara.hostname + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt", out string errorMsg))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error while analysing minidump", "Toolbox encountered problem while processing minidump:" + Environment.NewLine + errorMsg);
                return;
            }

            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Analysis has been completed", @"Minidump file successful processed and saved at .\Logs\" + connectionPara.hostname + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt");
            Process.Start(@".\Logs\" + connectionPara.hostname + " - DumpFile " + System.IO.Path.GetFileNameWithoutExtension(selectedMinidump) + @".txt");
        }

        //-----------------SQL-----------------------------------
        private void lastTxRollOverMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
            if (!CtrlFunctions.SqlGetInfo(connectionPara.fullNetworkName, "TPPosDB", "select lLastTaNmbr, lRolloverTransactionNmbrAt from TxControlTransactionNmbr", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
        }
        private void missingTxMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
            if (!CtrlFunctions.SqlGetInfo(connectionPara.fullNetworkName, "TPCentralDB", "select lTaNmbr, lWorkstationNmbr, szDate, szTime from TxMissings", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
        }
        private void showLastSignOffFromYesterdayMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetSqlInfo, (sender as ToolStripMenuItem).Text);
            string data = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            if (!CtrlFunctions.SqlGetInfo(String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "180"), "TPCentralDB", "select top 1 lWorkstationNmbr, szType, szDate, szTime, bIsDeclaration from TxSignOnOff where lWorkstationNmbr=" + connectionPara.deviceNr.TrimStart('0') + " and szDate=" + data + " and szType='SIGN_OFF' Order By szTime DESC", out string output))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "SQL Query Error", output);
                return;
            }
            if (output == "") { output = "No data found in database"; }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Sql Query result", output);
        }

        //------------------------------------------------------------
        private void DhcpPScopeInfoMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = interfejs.connectionPara;
            ChangeStatusBar("Working...");
            string scope = String.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], "0");
            string outputPath = @".\Logs\DhcpScope " + connectionPara.country + connectionPara.storeNr + " (" + scope + ") " + Logger.Datownik() + ".txt";

            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.GetDhcpScope, scope);
            try
            {
                PowerShell.Create().AddCommand("Get-DhcpServerv4lease").AddParameter("ComputerName", "10.58.50.13").AddParameter("ScopeId", scope).AddCommand("Out-File").AddParameter("FilePath", Path.GetFullPath(outputPath)).Invoke();
            }
            catch(Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "PowerShell Error", "ToolBox wasn't able to retrieve information from DHCP: " + Environment.NewLine + exp.Message);
                Logger.QuickLog(Globals.Funkcje.GetDhcpScope, scope, connectionPara.hostname, "ErrorLog", exp.ToString());
            }
            ChangeStatusBar("Ready");
            Process.Start(outputPath);
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Fixes-----------------------------
        private void tillLocalCacheClearMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ConnectionPara connectionPara = Main.interfejs.connectionPara;
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Local Cache Clear", "This function will terminate MobilePOS app on selected host. Do you want to proceed?") != DialogResult.OK)
                    {
                        return;
                    }

                    Logger myLog = new Logger(Globals.Funkcje.LocalCacheClear, "", connectionPara.hostname);
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.LocalCacheClear, "");

                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        ChangeStatusBar("Ready");
                        return;
                    }

                    ChangeStatusBar("Clearing Cache");
                    myLog.Add("Clearing Cache");
                    if(!FileController.ClearFolder(@"\\" + connectionPara.fullNetworkName + @"\c$\Users\" + connectionPara.country + connectionPara.storeNr + connectionPara.storeType + @".AL\AppData\Local\Diebold_Nixdorf\mobile_cache\Local Storage", false, out string errorList))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to delete files");
                        myLog.Add(errorList);
                        myLog.SaveLog("ErrorLog");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Clearing Folder Error", "ToolBox was unable to delete files in folder:" + Environment.NewLine + errorList);
                    }
                    ChangeStatusBar("Ready");
                };
                slave.RunWorkerAsync();
            }
        } //dont support IP MODE
        private void parkedTXMoveMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                ConnectionPara connectionPara = Main.interfejs.connectionPara;
                slave.DoWork += (s, args) =>
                {
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Parked Tx Move", "This function will terminate MobilePOS app on selected host. Do you want to proceed?") != DialogResult.OK)
                    {
                        return;
                    }

                    Logger myLog = new Logger(Globals.Funkcje.ParkedTxMove, "", connectionPara.hostname);
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.ParkedTxMove, "");

                    myLog.Add("Killing MobilePOS");
                    ChangeStatusBar("Killing MobilePOS");
                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        ChangeStatusBar("Ready");
                        return;
                    }
                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    myLog.Add("Ticket nr: " + tixnr);
                    if (tixnr == "")
                    {
                        ChangeStatusBar("Ready");
                        return;
                    }
                    string outputFolderName = @"\\" + connectionPara.fullNetworkName + @"\d$\WNI\4GSS\Parked - " + tixnr + "(" + connectionPara.hostname + ") " + Logger.Datownik();
                    myLog.Add("Creating folder: " + outputFolderName);
                    if (!FileController.MakeFolder(outputFolderName, out Exception makeExp))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Creating folder error");
                        myLog.Add(makeExp.ToString());
                        myLog.SaveLog("ErrorLog");
                        ChangeStatusBar("Ready");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Creating Folder Error", "ToolBox was unable to create folder " + outputFolderName + Environment.NewLine + makeExp.Message);
                        return;
                    }
                    myLog.Add("Moving Files");
                    foreach (string file in Directory.GetFiles(@"\\" + connectionPara.fullNetworkName + @"\d$\TPDotnet\Pos\Transactions\Parked"))
                    {
                        if(!FileController.MoveFile(file, outputFolderName + @"\" + Path.GetFileName(file), false, out Exception moveExp))
                        {
                            myLog.Add(moveExp.Message);
                            myLog.wasError = true;
                            Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, moveExp.Message);
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Move File Error", "ToolBox was unable to move " + file + Environment.NewLine + moveExp.Message);
                        }
                    }
                    if (myLog.wasError)
                    {
                        myLog.SaveLog("ErrorLog");
                    }
                    ChangeStatusBar("Ready");
                };
                slave.RunWorkerAsync();
            }
        }
        private void TSEWebserviceRestartMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.TSEWebserviceRestart, "");
            Process.Start("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c ""cd C:\Program Files (x86)\DieboldNixdorf\TSE-Webservice\bin"" && dn_tsetool.bat restart && pause");
        }
        private void SignatorResetMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionPara connectionPara = Main.interfejs.connectionPara;
            Logger myLog = new Logger(Globals.Funkcje.SignatorReset, "", connectionPara.hostname);
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.SignatorReset, "");

                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        return;
                    }

                    ChangeStatusBar("Resetting Signator");
                    myLog.Add("Resetting Signator");
                    string deviceID = connectionPara.deviceNr;
                    if(connectionPara.deviceType == "SCO")
                    {
                        deviceID = (int.Parse(connectionPara.deviceNr) + 49).ToString();
                    }
                    string cashBoxID = "0000" + "43" + connectionPara.storeNr + "00000000" + deviceID;
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\Program Files (x86)\signator && signatorhelper -closecashbox cashboxid=" + cashBoxID + " reason=7 && net stop signator");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to Reset Signator or stop service");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to reset Signator or stop service", "PSEXEC encountered some problems trying to Unable to reset Signator or stop it service and exited with error:" + Environment.NewLine + cmdOutput.errorOutputText);
                        ChangeStatusBar("Ready");
                        myLog.Add("Resetting Signator Error: " + cmdOutput.exitCode);
                        myLog.Add(cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    ChangeStatusBar("Creating Signator backup folder");
                    myLog.Add("Creating Signator backup folder");
                    string signatorPath = @"\\" + connectionPara.fullNetworkName + @"\c$\ProgramData\signator";
                    string backupPath = @"\\" + connectionPara.fullNetworkName + @"\c$\ProgramData\signator_backup";
                    if (System.IO.Directory.Exists(backupPath))
                    {
                        myLog.Add("Detected old backup folder - deleting");
                        try
                        {
                            System.IO.Directory.Delete(backupPath, true);
                        }
                        catch (Exception exp)
                        {
                            Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to delete old signator backup folder");
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
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to create signator backup folder");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to create signator backup folder", "Toolbox was unable to create signator backup folder. Error:" + Environment.NewLine + exp.Message);
                        ChangeStatusBar("Ready");
                        myLog.Add(exp.ToString());
                        myLog.SaveLog("ErrorLog");
                        return;
                    }
                    myLog.Add("Rename done");

                    ChangeStatusBar("Starting Signator service");
                    myLog.Add("Starting Signator service");
                    cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net start signator");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to start Signator service");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to start Signator service", "PSEXEC encountered some problems trying to start Signator service and exited with error:" + Environment.NewLine + cmdOutput.errorOutputText);
                        ChangeStatusBar("Ready");
                        myLog.Add("Unable to start service: " + cmdOutput.exitCode);
                        myLog.Add(cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    ChangeStatusBar("Ready");
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
                    Logger myLog = new Logger(Globals.Funkcje.ApcServiceFix, "", connectionPara.hostname);

                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.ApcServiceFix, "");
                    Main.ChangeStatusBar("Copying config file");
                    myLog.Add("Copying config file");
                    if (!FileController.CopyFile(Globals.toolsPath + "m11.cfg", @"\\" + connectionPara.fullNetworkName + @"\c$\Program Files (x86)\APC\PowerChute Business Edition\agent\m11.cfg", false, out Exception copyExp))
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Copying config file error");
                        myLog.Add(copyExp.ToString());
                        myLog.SaveLog("ErrorLog");
                        Main.ChangeStatusBar("Ready");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Config Copying Error", "ToolBox encountered error while copying config file:" + Environment.NewLine + copyExp.Message);
                        return;
                    }

                    myLog.Add("Starting APC Agent");
                    Main.ChangeStatusBar("Starting APC Agent");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c net start apcpbeagent");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to start APC Agent");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unable to start APC Agent", "Error occured while starting APC Agent Service:" + Environment.NewLine + cmdOutput.errorOutputText);
                        myLog.Add("Cmd error: " + cmdOutput.exitCode + Environment.NewLine + cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        Main.ChangeStatusBar("Ready");
                        return;
                    }

                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "APC Agent Started", "Config file fixed and APC Agent Service successfully started");
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
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.BackupJobsReset, "");
                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c powershell -ep Bypass c:\service\tools\backup\RemoveImageJob.ps1 && powershell -ep Bypass c:\service\tools\backup\AddImageJob.ps1");
                    ChangeStatusBar("Ready");
                    if (cmdOutput.exitCode != 0)
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Commands Execution Failed", "CMD exited with error code: " + cmdOutput.exitCode + Environment.NewLine + "Error log will be shown on next popup");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Backup Job Reset Error Log", cmdOutput.outputText);
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "CMD exited with error code: " + cmdOutput.exitCode);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Backup Jobs Reset Successful", "Veritas Backup Jobs has been reset to proper values");
                };
                slave.RunWorkerAsync();
            }
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Tools-----------------------------
        private void ServiceManagerMenuItem_Click(object sender, EventArgs e)
        {
            new serviceMgr().Show(this);
        }
        private void TransactionsXMLToCSVMenuItem_Click(object sender, EventArgs e)
        {
            Logger myLog = new Logger(Globals.Funkcje.TransactionsXMLToCSV, "none", "none");
            Telemetry.LogFunctionUsage(Globals.Funkcje.TransactionsXMLToCSV);
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.TransactionsXMLToCSV, "");
            string filePath = FileController.OpenFileDialog("XML files (*.xml)|*.xml");
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
            if (!FileController.SaveTxtToFile(Globals.userTempLogsPath + "TransactionsXMLToCSV - " + Logger.Datownik() + ".csv", output, out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file:" + Environment.NewLine + saveExp.Message);
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Conversion successful", @"CSV file successfully saved at .\Logs");
        }
        private void MobilePosAppKillMenuItem_Click(object sender, EventArgs e)
        {
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    Logger myLog = new Logger(Globals.Funkcje.MobilePosKill, "", connectionPara.hostname);
                    ChangeStatusBar("Killing MobilePOS");
                    myLog.Add("Killing MobilePOS");
                    Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.MobilePosKill, "");

                    if (!CtrlFunctions.KillMobilePos(connectionPara, out string errorMsg))
                    {
                        myLog.Add(errorMsg);
                        myLog.SaveLog("ErrorLog");
                        Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "MobilePOS app kill failed");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "MobilePOS Kill Failed", errorMsg);
                        return;
                    }
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "MobilePos is ded", "MobilePos App was successfully assassinated");
                };
                slave.RunWorkerAsync();
            }
        }
        private void MonitoringSlayerMenuItem_Click(object sender, EventArgs e)
        {
            new MonitoringAnalizer().Show();
        }
        private void StocktakingMenuItem_Click(object sender, EventArgs e)
        {
            new Stocktaking().Show();
        }
        private void massFunctionsMenuItem_Click(object sender, EventArgs e)
        {
            string[] functionList = new string[]
            {
                "InvalidTransfer",
                "TpReportsRegenAndZip",
                "UpdatePackageInvalidCheck",
                "JposLogsDownload",
                "TpProcessManagerRestart",
            };
            new MassFunctionForm(functionList).Show();
        }
        private void FindMacInDhcpMenuItem_Click(object sender, EventArgs e)
        {
            string mac = Microsoft.VisualBasic.Interaction.InputBox("Enter MAC that our hounds should find in DHCP" + Environment.NewLine + "Format: aa-aa-aa-aa-aa-aa");
            if (mac == "")
            {
                return;
            }
            Telemetry.LogUserAction("", Globals.Funkcje.DhcpMacFind, mac);
            Telemetry.LogFunctionUsage(Globals.Funkcje.DhcpMacFind);
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c powershell -command \"Get-DhcpServerv4Scope -ComputerName 10.58.50.13 | Get-DhcpServerv4Reservation -ComputerName 10.58.50.13 | where {$_.ClientId -like '" + mac + "'} | Format-Table -Property IPAddress,ScopeId,Name\"");
            string output = cmdOutput.outputText;
            if (cmdOutput.outputText == "")
            {
                output = "None host found with MAC: " + mac;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Info, "DHCP Search Result", output);
        }
        private void MiniLoggerDataCollectMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogCompleteTelemetryData(connectionPara.hostname, Globals.Funkcje.CucDataCollect, "");
            if (!FileController.CopyFile(Globals.toolsPath + @"mccs.run", @"\\" + connectionPara.fullNetworkName + @"\d$\StoreApps\pfm\programs\mccs.run", false, out Exception copyExp))
            {
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, copyExp.Message);
                Logger.QuickLog(Globals.Funkcje.CucDataCollect, "", connectionPara.hostname, "ErrorLog", copyExp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Mccs.run Copy Error", "ToolBox was unable to copy file to targeted host." + Environment.NewLine + copyExp.Message);
                return;
            }
            Process.Start("explorer.exe", @"\\" + connectionPara.fullNetworkName + @"\d$\StoreApps\pfm\programs");
        }


        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------ADV-----------------------------
        private void randomCollectionOfRandomnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] functionList = new string[]
            {
                "GetMAC",
                "BulkFileMove",
                "EsfClientRestart",
                "EsfClientReinit",
                "JposLogsCheck",
                "BackupJobsCheck",
                "BackupJobsReset",
                "DeleteOldBackupFiles",
                "IsBackupDriveAccessible",
                "DownloadJavaPosLogs",
                "CheckForKB",
                "DeployAndExecute",
                "DismAndSFC",
                "CDriveClean",
                "BackstoreCsvExport",
                "DiscSpaceInfo",
                "GetSqlInfo",
                "GetMiniloggerStatus",
                "AdhocFunction"
            };
            new MassFunctionForm(functionList).Show();
        }
        private void getMeMoreWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] functionList = new string[]
            {
                "GetMeMoreWork",
                "MoveInvalidUpdatePackages",
            };
            new MassFunctionForm(functionList).Show();
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------PM-----------------------------
        private void raportsMenuItem_Click(object sender, EventArgs e)
        {
            string[] functionList = new string[]
            {
                "CheckEodAbortedStatus",
                "WinCrashReasonCheck",
            };
            new MassFunctionForm(functionList).Show();
        }

        /**//**//**//**//**//**//**//**//**//**//**//**//**//**//**//**/

        //--------------------Preferences-----------------------------
        private void ShowNotepadMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.ToggleNotePad);
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.ToggleNotePad, "");
            showNotepadMenuItem.Checked = !showNotepadMenuItem.Checked;
            if (showNotepadMenuItem.Checked)
            {
                userSettings.hideNotePad = false;
                ApplyLayout("modern");
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
            Telemetry.LogFunctionUsage(Globals.Funkcje.ShowHelp);
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.ShowHelp, "");
            Help.ShowHelp(this, @".\help.chm");
            //Process.Start(Globals.configPath + @"ToolBox v2.0 Manual.pdf");
        }
        private void ChangeLogMenuItem_Click(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.ShowChangeLog);
            Telemetry.LogUserAction("MainForm", Globals.Funkcje.ShowChangeLog, "");
            new ChangeLog().Show(this);
        }
        private void ReportSuggestMenuItem_Click(object sender, EventArgs e)
        {
            new Report().Show(this);
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
            }
        }
        public void HideNotePad()
        {
            saveNote.Visible = false;
            importNote.Visible = false;
            notepadGroup.Visible = false;
            this.MinimumSize = new Size(400, 302);
            this.Size = this.MinimumSize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        public void DisableUI()
        {
            List<string> wyjatki = new List<string> 
            { 
                "Last Connected",
                "Ping's",
                "TransactionsXML to CSV",
                "MonitoringSlayer",
                "Stocktaking",
                "Mass Functions",
                "Find MAC in DHCP",
                "Random Collection of Randomness",
                "Get Me More Work",
                "Reports"
            };
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
            Main.interfejs.getMAC_button.Enabled = false;
            Main.interfejs.SubnetStatusGroup.Enabled = false;
            Main.interfejs.Rescan_Button.Enabled = false;
        }
        private void EnableUI()
        {
            SetEnableMenuItems(uniwersalMenuItems, true);
            if (connectionPara.deviceType == "TPS")
            {
                SetEnableMenuItems(tpsMenuItems, true);
            }
            else if (connectionPara.deviceType == "STP" || connectionPara.deviceType == "SCO")
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
            Main.interfejs.getMAC_button.Enabled = true;
            Main.interfejs.SubnetStatusGroup.Enabled = true;
            Main.interfejs.Rescan_Button.Enabled = true;
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


            connectionPara = ConnectionPara.EstablishConnection(tempTAG);
            if (connectionPara == null)
            {
                Main.ChangeStatusBar("Ready");
                Main.SetIP("Invalid TAG", Globals.errorColor);
                Main.SetTAG(tempTAG, Globals.errorColor);
                return;
            }

            Telemetry.LogMachineAction(tempTAG, Globals.Funkcje.Initialization, "");
            Telemetry.LogUserAction(tempTAG, Globals.Funkcje.Initialization, "");

            if (connectionPara.IP == "DNS ERROR")
            {
                Main.SetIP("DNS ERROR", Globals.errorColor);
                Telemetry.LogMachineAction(tempTAG, Globals.Funkcje.Error, Main.GetIP());
                if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.hostname + " - DNS Error", "Would you like to connect to this host via IP?") != DialogResult.OK)
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
            if (connectionPara.IPMode)
            {
                Main.SetIP("IP MODE", Globals.errorColor);
                CtrlFunctions.CmdOutput cmdHostname = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.fullNetworkName + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c hostname");
                if (cmdHostname.exitCode == 0)
                {
                    connectionPara.hostname = cmdHostname.outputText.Trim();
                    connectionPara.country = connectionPara.hostname.Substring(0, 2);
                    connectionPara.storeNr = connectionPara.hostname.Substring(2, 4);
                    connectionPara.deviceType = connectionPara.hostname.Substring(6, 3);
                    connectionPara.deviceNr = connectionPara.hostname.Substring(9, 2);
                    connectionPara.storeType = connectionPara.hostname.Substring(11);

                    ipNotSupported = new List<string>
                    {
                        //"Local Storage (Till)",
                        //"Scan Store Endpoints",
                        //"Backup Checker",
                        //"EoD Checker",
                        //"Till Local Cache Clear"
                    };

                }
            }
            else
            {
                Main.SetIP(connectionPara.IP, Color.LightGreen);
            }
            interfejs.DeDifferencesOnOff();

            CtrlFunctions.SubNetScan(connectionPara);
            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out CtrlFunctions.CmdOutput cmdOutput))
            {
                Main.SetTAG(connectionPara.hostname, Globals.errorColor);
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Connection Error", "Unable to map host drive through net use:" + Environment.NewLine + cmdOutput.errorOutputText);
                Telemetry.LogMachineAction(connectionPara.hostname, Globals.Funkcje.Error, "Unable to map host drive");
                Main.ChangeStatusBar("Ready");
                return;
            }

            Main.ChangeTitle("TP MasterTool - " + connectionPara.hostname + " (" + connectionPara.IP + ")");
            Main.SetTAG(tempTAG, Color.LightGreen);
            EnableUI();
            Main.ChangeStatusBar("Ready");
            Telemetry.LogFunctionUsage(Globals.Funkcje.Initialization);
            userSettings.AddNewRecent(tempTAG);
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

    }
}
