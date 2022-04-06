using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using TP_MasterTool.Forms;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{

    public static class CtrlFunctions
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

        public struct CmdOutput
        {
            public int exitCode;
            public string outputText;
            public string errorOutputText;
            public CmdOutput(int exit, string output, string errorOutput)
            {
                exitCode = exit;
                outputText = output;
                errorOutputText = errorOutput;
            }
        }

        /**//**//**//**//**//**//**//**//**//* BACK-END FUNCTIONS *//**//**//**//**//**//**//**//**//**//**/

        public static string DnsGetIP(string tag)
        {
            try
            {
                return Dns.GetHostEntry(tag).AddressList[0].ToString();
            }
            catch
            {
                return "DNS ERROR";
            }
        }
        public static CmdOutput RunHiddenCmd(string exe, string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            string temp = p.StandardOutput.ReadToEnd();
            string temp2 = p.StandardError.ReadToEnd();
            p.WaitForExit();

            return new CmdOutput(p.ExitCode, temp, temp2);
        }
        public static int RunHiddenCmdWitoutOutput(string exe, string command, bool wait4Exit)
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
                return p.ExitCode;
            }
            return 0;
        }
        public static void OpenFolder(string host, string path)
        {
            Main.ChangeStatusBar("Working...");
            Telemetry.LogOnMachineAction(host, Globals.Funkcje.OpenFolder, path);
            if (!System.IO.Directory.Exists(@"\\" + host + @"\" + path))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Folder Not Found", @"Folder couldn't be found. Please check if target machine is online or initialize it anew and try again.");
                Telemetry.LogOnMachineAction(host, Globals.Funkcje.OpenFolder, "Error");
                Main.ChangeStatusBar("Ready");
                return;
            }
            Process.Start("explorer.exe", @"\\" + host + @"\" + path);
            Telemetry.LogFunctionUsage(Globals.Funkcje.OpenFolder);
            Main.ChangeStatusBar("Ready");

        }
        public static Color PingIP(string ip)
        {
            try
            {
                if (new Ping().Send(ip, 4000).Status == IPStatus.Success)
                {
                    return Color.LightGreen;
                }
            }
            catch { }

            return Globals.errorColor;
        }
        public static void SubNetScan(ConnectionPara connectionPara)
        {
            int de = 1;
            if (connectionPara.country == "DE") { de = 0; }

            foreach (Control control in Main.interfejs.SubnetStatusGroup.Controls)
            {
                if (control is Button)
                {
                    control.BackColor = SystemColors.Control;
                    using (BackgroundWorker slave = new BackgroundWorker())
                    {
                        slave.DoWork += (s, args) =>
                        {
                            if (control.Name == "PRN01_button" || control.Name == "PRN49_button")
                            {
                                control.BackColor = CtrlFunctions.PingIP(string.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], (connectionPara.IPbytes[2] + de).ToString(), control.Tag.ToString()));
                                return;
                            }
                            control.BackColor = CtrlFunctions.PingIP(string.Join(".", connectionPara.IPbytes[0], connectionPara.IPbytes[1], connectionPara.IPbytes[2], control.Tag.ToString()));
                        };
                        slave.RunWorkerAsync();
                    }
                }
            }
        }
        public static bool MapEndpointDrive(ref ConnectionPara connectionPara, out CmdOutput cmdOutput)
        {
            cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe ", @"/c net use \\" + connectionPara.TAG + " " + connectionPara.password + " /user:" + connectionPara.userName);
            if (cmdOutput.exitCode != 0)
            {
                if (cmdOutput.errorOutputText.Contains("The user name or password is incorrect"))
                {
                    return ReMapDriveWithCredSwitch(ref connectionPara, out cmdOutput);
                }
                return false;
            }
            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\c$"))
            {
                CtrlFunctions.RunHiddenCmd("cmd.exe", @"/c net use \\" + connectionPara.TAG + @" /delete");
                return ReMapDriveWithCredSwitch(ref connectionPara, out cmdOutput);
            }
            return true;
        }
        private static bool ReMapDriveWithCredSwitch(ref ConnectionPara connectionPara, out CmdOutput cmdOutput)
        {
            CredentialsSwitch(ref connectionPara);
            cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", @"/c net use \\" + connectionPara.TAG + " " + connectionPara.password + " /user:" + connectionPara.userName);
            if (cmdOutput.exitCode != 0)
            {
                //Main.SetTAG(connectionPara.TAG, Globals.errorColor);
                //CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Connection Error", "Unable to map host drive through net use:" + Environment.NewLine + cmdOutput2.errorOutputText);
                //Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to map host drive");
                return false;
            }
            return true;
        }
        private static void CredentialsSwitch(ref ConnectionPara connectionPara)
        {
            if (connectionPara.storeType == "T")
            {
                connectionPara.userName = Globals.PRODuserName;
                connectionPara.password = Globals.PRODpassword;
            }
            else
            {
                connectionPara.userName = Globals.TESTuserName;
                connectionPara.password = Globals.TESTpassword;
            }
        }
        public static string GetIpFromDNSError(ConnectionPara connectionPara)
        {
            string checkDevice = "STP";
            string lastByte = "180";

            if (connectionPara.deviceType == "STP")
            {
                lastByte = (180 + int.Parse(connectionPara.deviceNr)).ToString();
                checkDevice = "TPS";
            }
            string tempIP = CtrlFunctions.DnsGetIP(String.Join("", connectionPara.country, connectionPara.storeNr, checkDevice, "01", connectionPara.storeType));
            if (tempIP == "DNS ERROR")
            {
                return tempIP;
            }
            byte[] ipBytes = IPAddress.Parse(tempIP).GetAddressBytes();

            return String.Join(".", ipBytes[0], ipBytes[1], ipBytes[2], lastByte);
        }
        public static bool CreateLock(string lockPath, ref Logger myLog)
        {
            try
            {
                myLog.Add("Creating lock");
                System.IO.File.Create(lockPath).Close();
                return true;
            }
            catch (Exception exp)
            {
                myLog.Add("Error Creating lock" + Environment.NewLine + exp.ToString());
                return false;
            }
        }
        public static bool DeleteLock(string lockPath)
        {
            try
            {
                System.IO.File.Delete(lockPath);
                return true;
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.DeleteLock, lockPath, "", "CriticalLog", "Error Deleting lock" + Environment.NewLine + exp.ToString());
                return false;
            }
        }
        public static bool DecryptToString(string filePath, string decryptKey, out string decryptedString)
        {
            byte[] key = new UnicodeEncoding().GetBytes(decryptKey);
            try
            {
                using (FileStream fileReader = new FileStream(filePath, FileMode.Open))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileReader, new RijndaelManaged().CreateDecryptor(key, key), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(cryptoStream))
                        {
                            decryptedString = srDecrypt.ReadToEnd();
                            return true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.DecryptToString, "File: " + filePath + Environment.NewLine + "Key: " + decryptKey, "", "CriticalError", exp.ToString());
                decryptedString = "Unable to decrypt " + Path.GetFileName(filePath) + Environment.NewLine + exp.Message;
                return false;
            }
        }
        public static void EncryptFile(string inputFilePath, string encryptKey, string outputFilePath)
        {
            byte[] key = new UnicodeEncoding().GetBytes(encryptKey);

            using (FileStream fileStreamOutput = new FileStream(outputFilePath, FileMode.Create))
            {
                using (CryptoStream cryptoStream = new CryptoStream(fileStreamOutput, new RijndaelManaged().CreateEncryptor(key, key), CryptoStreamMode.Write))
                {
                    using (FileStream fileStreamInput = new FileStream(inputFilePath, FileMode.Open))
                    {
                        int data;
                        while ((data = fileStreamInput.ReadByte()) != -1)
                        {
                            cryptoStream.WriteByte((byte)data);
                        }
                    }
                }
            }
        }

        /**//**//**//**//**//**//**//**//**//* FRONT-END FUNCTIONS *//**//**//**//**//**//**//**//**//**//**/

        public static bool Smarty(ConnectionPara connectionPara, out string errorMsg)
        {
            errorMsg = "";
            Logger myLog = new Logger(Globals.Funkcje.GetSMART, "", connectionPara.TAG);
            System.IO.Directory.CreateDirectory(@"\\" + connectionPara.TAG + @"\c$\SMART");
            if (System.IO.File.Exists(@"\\" + connectionPara.TAG + @"\c$\SMART\smart.lock"))
            {
                myLog.Add("CrystalDiskInfo is already running");
                myLog.SaveLog("InfoLog");
                errorMsg = @"CrystalDiskInfo is already running on the host, please wait for the result.";
                return false;
            }
            else
            {
                if (!CreateLock(@"\\" + connectionPara.TAG + @"\c$\SMART\smart.lock", ref myLog))
                {
                    myLog.SaveLog("ErrorLog");
                    errorMsg = @"ToolBox wasn't able to lock CrystalDiskInfo process. Please initialize it anew and try again";
                    return false;
                }
            }

            if (!FileController.CopyFolder(Globals.toolsPath + @"CrystalDiskInfo", @"\\" + connectionPara.TAG + @"\c$\SMART", false, out Exception copyExp))
            {
                myLog.Add(copyExp.ToString());
                myLog.SaveLog("ErrorLog");
                errorMsg = @"ToolBox wasn't able to copy CrystalDiskInfo into targeted host. Please initialize it anew and try again";
                return false;
            }

            myLog.Add("Starting RCMD");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + @" -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c C:\SMART\diskinfo64 /copyexit");
            if (cmdOutput.exitCode != 0)
            {
                myLog.Add("RCMD encountered error: " + cmdOutput.errorOutputText);
                myLog.SaveLog("ErrorLog");
                errorMsg = @"ToolBox encountered error while executing remote command. Please contact dev team for more info";
                return false;
            }

            return true;
        }
        public static void GetWinLogs(string type, ConnectionPara connectionPara)
        {
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetWinLogs, type);
            Telemetry.LogFunctionUsage(Globals.Funkcje.GetWinLogs);
            Logger myLog = new Logger(Globals.Funkcje.GetWinLogs, type, connectionPara.TAG);
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string fileName = connectionPara.TAG + " - " + type + " Log.evtx";
                    myLog.Add("Copying: " + type + @".evtx");
                    if (!FileController.CopyFile(@"\\" + connectionPara.TAG + @"\c$\Windows\System32\winevt\Logs\" + type + @".evtx", @".\Logs\Windows\" + fileName, true, out Exception copyExp))
                    {
                        if (copyExp != null)
                        {
                            myLog.Add("Failed:" + Environment.NewLine + copyExp.ToString());
                            myLog.SaveLog("ErrorLog");
                            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Error during copying");
                            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Downloading Error", "ToolBox encountered error during downloading logs:" + Environment.NewLine + copyExp.Message);
                        }
                        return;
                    }
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "File open", "Do you want to open " + fileName + " ?") == DialogResult.Cancel)
                    {
                        return;
                    }
                    myLog.Add("Opening");
                    try
                    {
                        Process.Start(@".\Logs\Windows\" + fileName);
                    }
                    catch (Win32Exception exp)
                    {
                        myLog.Add("Error trying open log file - " + @".\Logs\Windows\" + fileName);
                        myLog.Add(exp.ToString());
                        myLog.SaveLog("ErrorLog");
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Log file open error");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Log file open error", @"System encouter a problem while trying to open log file: " + Environment.NewLine + exp.Message);
                    }
                };
                slave.RunWorkerAsync();
            }
        }
        public static bool KillMobilePos(ConnectionPara connectionPara, out string errorMsg)
        {
            errorMsg = "";
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c taskkill /im TP.UI.MobilePOS.exe /f");
            if (cmdOutput.exitCode != 0 && cmdOutput.exitCode != 128)
            {
                errorMsg = cmdOutput.errorOutputText;
                return false;
            }
            return true;
        }
        public static string GetDiskSpaceInfo(string drive, ConnectionPara connectionPara, out ulong TotalNumberOfFreeBytes)
        {
            if (!GetDiskFreeSpaceEx(@"\\" + connectionPara.TAG + @"\" + drive + @"$\", out _, out ulong TotalNumberOfBytes, out TotalNumberOfFreeBytes))
            {
                return " - Error reading disc space";
            }
            double procent = (((TotalNumberOfBytes - TotalNumberOfFreeBytes) * 1.0 / TotalNumberOfBytes) * 100);
            return " - Free Space: " + (TotalNumberOfFreeBytes / (1024 * 1024)).ToString() + " MB | "
                + ((TotalNumberOfBytes - TotalNumberOfFreeBytes) / (1024 * 1024 * 1024)).ToString() + "/" + (TotalNumberOfBytes / (1024 * 1024 * 1024)).ToString() + " GB | "
                + Math.Round(procent, 2, MidpointRounding.AwayFromZero).ToString() + " % Used Space";
        }
        public static bool RegenerateEoDReports(ConnectionPara connectionPara, string startDate, string endDate, out string regerOutput)
        {
            if (!System.IO.File.Exists(@"\\" + connectionPara.TAG + @"\c$\temp\runeodreports.bat"))
            {
                if(!FileController.CopyFile(Globals.toolsPath + "runeodreports.bat", @"\\" + connectionPara.TAG + @"\c$\temp\runeodreports.bat", false, out Exception copyExp))
                {
                    regerOutput = "Unable to copy script -> " + copyExp.Message;
                    return false;
                }
            }
            if(RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd" +
                @" /c cd C:\temp && runeodreports.bat " + startDate + " " + endDate, true) != 0)
            {
                regerOutput = "Error with execution of runeodreports.bat";
                return false;
            }
            regerOutput = "EoD Reports Regenerated";
            return true;
        }
        public static bool ZipEoDReports(ConnectionPara connectionPara, out string output)
        {
            if(RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd" +
                @" /c cd C:\service\qem\collect_tp_reports && powershell -ExecutionPolicy Bypass -File collect_tp_reports.ps1", true) != 0)
            {
                output = "Error with execute PowerShell script";
                return false;
            }
            output = "EoD Reports Zipped";
            return true;
        }
        public static bool AnalyseMiniDump(string minidumpPath, string logOutputPath, out string error)
        {
            error = null;
            Logger myLog = new Logger(Globals.Funkcje.MiniDumpAnalyser, minidumpPath + " " + logOutputPath, "");
            myLog.Add("Starting cmd");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("cmd.exe", "/c \"" + Globals.toolsPath + @"\dumpchk"" " + minidumpPath);
            if (cmdOutput.exitCode != 0)
            {
                myLog.Add("CMD exited with error code: " + cmdOutput.exitCode);
                myLog.Add(cmdOutput.outputText);
                myLog.Add(cmdOutput.errorOutputText);
                myLog.SaveLog("ErrorLog");
                error = cmdOutput.outputText;
                return false;
            }

            myLog.Add("Processing result");
            string[] lines = cmdOutput.outputText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string result = "";
            bool hit = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Bugcheck Analysis"))
                {
                    result += String.Join(Environment.NewLine, lines[i - 2], lines[i - 1]);
                    hit = true;
                }
                if (hit)
                {
                    if (lines[i].Contains("-----"))
                    {
                        break;
                    }
                    result += Environment.NewLine + lines[i];
                }
            }
            myLog.Add("Saving result to file");
            try
            {
                System.IO.File.WriteAllText(logOutputPath, result);
            }
            catch (Exception exp)
            {
                error = exp.Message;
                myLog.Add("Error while trying to save result");
                myLog.Add(exp.ToString());
                return false;
            }
            return true;
        }
        public static bool BackstoreCsvExport(ConnectionPara connectionPara)
        {
            RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c D:\TPDotnet\bin\CA.DE.BS.CSVExport.exe");
            int starter = 0;
            try
            {
                string[] log = System.IO.File.ReadAllLines(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log\" + connectionPara.TAG + "-CA.DE.BS.CSVExport.log");
                for (int i = log.Length - 1; i >= 0; i--)
                {
                    if (log[i].StartsWith("=============="))
                    {
                        starter = i;
                        break;
                    }
                }
                string[] output = new string[log.Length - starter];
                Array.Copy(log, starter, output, 0, log.Length - starter);
                foreach (string line in output)
                {
                    if (line.StartsWith("Exception"))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SqlGetInfo(string tag, string database, string sqlQuery, out string output)
        {
            output = "";
            string connetionString = @"Data Source=" + tag + @";Initial Catalog=" + database + @";User ID=" + Globals.SQLuserName + ";Password=" + Globals.SQLpassword;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connetionString))
                {
                    sqlConnection.Open();

                    using (SqlDataReader reader = new SqlCommand(sqlQuery, sqlConnection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                output += reader.GetName(i) + ": " + reader[i] + " | ";
                            }
                            output += Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                output = "SQL Query reurned with error:" + Environment.NewLine + exp.Message;
                return false;
            }

            return true;
        }

    }
}
