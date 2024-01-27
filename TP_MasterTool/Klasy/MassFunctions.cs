using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{
    static class MassFunctions
    {
        //-------GetInfo-------------------
        public static List<string> GetInfo_TpReportsRegenAndZip()
        {
            string dayBefore = Microsoft.VisualBasic.Interaction.InputBox("Provide <date-before-eod-run> according to USU 53669" + Environment.NewLine + "Dates must be given in yyyyMMdd format: 4-digits year, 2-digits month(01-12), 2-digits day (01-31)", "Input data", DateTime.Today.AddDays(-1).ToString("yyyyMMdd"));
            if (dayBefore == "")
            {
                return null;
            }

            string dayofEOD = Microsoft.VisualBasic.Interaction.InputBox("Provide <date-of-eod-run> according to USU 53669" + Environment.NewLine + "Dates must be given in yyyyMMdd format: 4-digits year, 2-digits month(01-12), 2-digits day (01-31)", "Input data", DateTime.Today.ToString("yyyyMMdd"));
            if (dayofEOD == "")
            {
                return null;
            }

            return new List<string> { dayBefore, dayofEOD };
        }
        public static List<string> GetInfo_UpdatePackageInvalidCheck()
        {
            string date = Microsoft.VisualBasic.Interaction.InputBox("Provide the date to check for invalid packages (YYYYMMDD)", "Input data");
            if (date == "")
            {
                return null;
            }
            return new List<string> { date };
        }
        public static List<string> GetInfo_BulkFileMove()
        {
            string source = Microsoft.VisualBasic.Interaction.InputBox(@"Provide path to source folder from which you want to move files (e.g. d$\TPDotnet\Log)", "Input data");
            if (source == "")
            {
                return null;
            }

            string destination = Microsoft.VisualBasic.Interaction.InputBox(@"Provide path to destination folder to which you want to move files to (e.g. d$\WNI)", "Input data");
            if (destination == "")
            {
                return null;
            }

            string filter = Microsoft.VisualBasic.Interaction.InputBox("What files should be moved aka search filter (* for all)", "Input data");
            if (filter == "")
            {
                return null;
            }

            return new List<string> { source, destination, filter };
        }
        public static List<string> GetInfo_JposLogsDownload()
        {
            string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:", "Input data");
            if (tixnr == "")
            {
                return null;
            }
            string logType;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Which version of logs to download?", new string[] { "ProBase POS (Old)", "ProBase Store (New)" }))
            {
                var result = dropDownSelect.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return null;
                }
                logType = dropDownSelect.ReturnValue1;            //values preserved after close
            }

            return new List<string> { tixnr, logType };
        }
        public static List<string> GetInfo_CheckForKB()
        {
            string kb = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:", "Input data");
            if (kb == "")
            {
                return null;
            }
            return new List<string> { kb };
        }
        public static List<string> GetInfo_DeployAndExecute()
        {
            string file;
            string[] tempFiles = new DirectoryInfo(Globals.toolsPath).EnumerateFiles().Where(f => f.Extension.Contains("cmd")).Select(f => f.Name).ToArray();
            using (DropDownSelect dropDownSelect = new DropDownSelect(@"Select file you want to deploy and execute from D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Tools", tempFiles))
            {
                var result = dropDownSelect.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return null;
                }
                file = dropDownSelect.ReturnValue1;            //values preserved after close
            }

            string waitForExit;
            using (DropDownSelect dropDownSelect = new DropDownSelect("Should ToolBox wait for cmd exit?", new string[] { "True", "False" }))
            {
                var result = dropDownSelect.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return null;
                }
                waitForExit = dropDownSelect.ReturnValue1;            //values preserved after close
            }

            return new List<string> { file, waitForExit };
        }
        public static List<string> GetInfo_BackstoreCsvExport()
        {
            string folder = FileController.OpenFolderBrowserDialog("Select folder with input data");
            if (folder == "")
            {
                return null;
            }
            return new List<string> { folder };
        }
        public static List<string> GetInfo_CheckEodAbortedStatus()
        {
            string date = Microsoft.VisualBasic.Interaction.InputBox("Provide start date to check till now (YYYYMMDD):", "Input data");
            if (date == "")
            {
                return null;
            }
            return new List<string> { date };
        }
        public static List<string> GetInfo_WinCrashReasonCheck()
        {
            string date = Microsoft.VisualBasic.Interaction.InputBox("Provide start date to check till now (mm.dd.yyyy):", "Input data");
            if (date == "")
            {
                return null;
            }
            string outputFileName = "WinCrashReasonReport " + Logger.Datownik() + ".csv";
            try
            {
                File.AppendAllText(Globals.userTempLogsPath + outputFileName, "TAG,TimeStamp,Bluescreen,PowerButton,Other" + Environment.NewLine);
            }
            catch(Exception exp)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Creating output file Error", "Unable to create output file in logs folder: " + exp.Message);
                return null;
            }
            return new List<string> { date, outputFileName };
        }


        //-------Mass Functions-------------
        public static void GetMAC(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Stealing MAC");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Failed to steal MAC");
                return;
            }
            string output = cmdOutput.outputText.Replace("\n", "").Replace("\r", "").ToUpper();
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + output);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.GetMAC, output);
        }
        public static void InvalidTransfer(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\Transactions\InValid", @"*_0_*.xml", SearchOption.AllDirectories);
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Error while searching files: " + exp.Message);
                return;
            }

            if (files.Length == 0)
            {
                massFunctionForm.GridChange(rownr, "Skipped", Globals.successColor);
                massFunctionForm.AddToLog(rownr, "[SUCCESS] - Skipped (no invalid transactions)");
                return;
            }

            if (!FileController.MakeFolder(@"\\" + connectionPara.TAG + @"\d$\WNI\Invalid_Transfer", out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Unable to create folder \d$\WNI\Invalid_Transfer: " + makeExp.Message);
                return;
            }

            bool error = false;
            foreach (string file in files)
            {
                if (!FileController.MoveFile(file, @"\\" + connectionPara.TAG + @"\d$\WNI\Invalid_Transfer\" + Path.GetFileName(file), false, out Exception moveExp))
                {
                    error = true;
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to move file: " + file + ": " + moveExp.Message);
                    continue;
                }
            }
            if (error)
            {
                return;
            }
            massFunctionForm.GridChange(rownr, "Success", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + files.Length + " invalid transactions was moved");
        }
        public static void TpReportsRegenAndZip(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            //massFunctionForm.GridChange(rownr, "Checking files");
            //if (File.Exists(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip"))
            //{
            //    massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + "Present | Ready to be picked up after next successful run");
            //    massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            //    return;
            //}

            //massFunctionForm.GridChange(rownr, "Checking for backup zip");
            
            massFunctionForm.GridChange(rownr, "Executing runeodreports.bat");
            if (!CtrlFunctions.RegenerateEoDReports(connectionPara, addInfo[0], addInfo[1], out string regenOutput))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, regenOutput);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + regenOutput);

            massFunctionForm.GridChange(rownr, "Executing collect_tp_reports.ps1");
            if (!CtrlFunctions.ZipEoDReports(connectionPara, out string zipOutput))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, zipOutput);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] " + zipOutput);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
        public static void UpdatePackageInvalidCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            if (!System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\UpdatePackages\InValid\" + addInfo[0]))
            {
                massFunctionForm.GridChange(rownr, "Clear", Globals.successColor);
                massFunctionForm.AddToLog(rownr, "[SUCCESS] - Clear");
                return;
            }
            string[] files = System.IO.Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Server\UpdatePackages\InValid\" + addInfo[0], "*.xml", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                massFunctionForm.GridChange(rownr, "Clear", Globals.successColor);
                massFunctionForm.AddToLog(rownr, "[SUCCESS] - Clear");
                return;
            }
            string output = Environment.NewLine + connectionPara.TAG + " Example items:" + Environment.NewLine;
            XDocument tempXml = XDocument.Load(files[0]);

            massFunctionForm.GridChange(rownr, "Scaning XML");
            var nodes = tempXml.Root.Elements("Transaction");
            if (nodes.Count() == 0)
            {
                Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalidCheck, "Found Invalid Items");
                massFunctionForm.ErrorLog(rownr, "Other Invalid xml found, please check manually and include it in note to MMS team");
                return;
            }
            try
            {
                int breaker = 0;
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
                output += tempXml.Root.Element("Trailer").Element("Statistic").Element("UpdateStatements").ToString();
            }
            catch
            {
                Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalidCheck, "Found Invalid Items");
                massFunctionForm.ErrorLog(rownr, "Other Invalid xml found, please check manually and include it in note to MMS team");
                return;
            }

            massFunctionForm.AddToLog(rownr, "[AFFECTED] - Found Invalid Items");
            lock (massFunctionForm.logLock)
            {
                massFunctionForm.log = massFunctionForm.log.Concat(output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ToArray();
            }
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalidCheck, "Found Invalid Items");
            massFunctionForm.GridChange(rownr, "Found Invalid", Globals.errorColor);
        }
        public static void BulkFileMove(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string source = @"\\" + connectionPara.TAG + @"\" + addInfo[0];
            string destination = @"\\" + connectionPara.TAG + @"\" + addInfo[1];
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.BulkFileMove, source + " -> " + destination + " filter: " + addInfo[2]);
            if (!Directory.Exists(source))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Source folder not found");
                return;
            }
            if (!FileController.MakeFolder(destination, out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to create destination folder: " + makeExp.Message);
                return;
            }
            string[] files = Directory.GetFiles(source, addInfo[2]);
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No file matching the criteria is found");
                return;
            }
            int licznik = 1;
            foreach (string file in files)
            {
                massFunctionForm.GridChange(rownr, "Moving " + licznik + " out of " + files.Length + " files");
                if (!FileController.MoveFile(file, destination + @"\" + Path.GetFileName(file), false, out Exception moveExp))
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Operation cancelled, Unable to move file: " + moveExp);
                    return;
                }
                licznik++;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + files.Length + " files were moved");
        }
        public static void EsfClientRestart(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Restarting Client");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c net stop esfclient && net start esfclient");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CMD exited with error code: " + cmdOutput.exitCode);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - ESF Client Restarted");
        }
        public static void EsfClientReinit(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Running script");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\service\agents\esfclient && reinit_esfclient.cmd");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CMD exited with error code: " + cmdOutput.exitCode);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - ESF Client Reinitialized");
        }
        public static void JposLogsDownload(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            string remotePath = @"\d$\TPDotnet\DeviceService";
            string localPath = @"D:\TPDotnet\DeviceService";
            string logFileName = "JPOSRFIDScannerLogs*";
            if (addInfo[1] == "ProBase Store (New)")
            {
                remotePath = @"\c$\ProgramData\javapos\wn\log";
                localPath = @"C:\ProgramData\javapos\wn\log";
                logFileName = "jniwrapper-diagnostics.log";
            }
            massFunctionForm.GridChange(rownr, "Looking for files");
            string[] files = Directory.GetFiles(@"\\" + connectionPara.TAG + remotePath, logFileName);
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No JPOS logs found");
                return;
            }

            if (Directory.Exists(@"\\" + connectionPara.TAG + remotePath + @"\JPOSLogs"))
            {
                massFunctionForm.GridChange(rownr, "Deleting old zip");
                try
                {
                    Directory.Delete(@"\\" + connectionPara.TAG + remotePath + @"\JPOSLogs", true);
                }
                catch (Exception exp)
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Can't delete folder with old logs, Please delete D:\TPDotnet\DeviceService\JPOSLogs manually and try again: " + exp.Message);
                    return;
                }
            }

            massFunctionForm.GridChange(rownr, "Gathering Logs");
            System.Threading.Thread.Sleep(150);
            if (!FileController.MakeFolder(@"\\" + connectionPara.TAG + remotePath + @"\JPOSLogs", out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to create folder for logs: " + makeExp.Message);
                return;
            }

            foreach (string file in files)
            {
                if (!FileController.CopyFile(file, @"\\" + connectionPara.TAG + remotePath + @"\JPOSLogs\" + Path.GetFileName(file), false, out Exception copyExp))
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy " + file + ": " + copyExp.Message);
                    return;
                }
            }
            massFunctionForm.GridChange(rownr, "Securing Logs");
            if (!FileController.ZipAndStealFolder(addInfo[0], "JposLogs", @"\\" + connectionPara.TAG + remotePath + @"\JPOSLogs", localPath + @"\JPOSLogs", connectionPara, out string outputFilePath))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, outputFilePath);
                return;
            }
            massFunctionForm.GridChange(rownr, "Downloading Logs");
            if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), false, out Exception copyExp2))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy log: " + copyExp2.Message);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - JposLogs Downloaded");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
        public static void TpProcessManagerRestart(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Restarting Process");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c net stop ""TPDotnet Process Manager"" && net start ""TPDotnet Process Manager""");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CMD exited with error code: " + cmdOutput.exitCode);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - TP Process Manager Restarted");

        }
        public static void JposLogsCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for logs");
            if (Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService", "JPOSRFIDScannerLogs*").Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No logs found");
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Logs are present");
        }
        public static void BackupJobsCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string[] cFiles;
            string[] dFiles;
            try
            {
                cFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_C*.v2i");
                dFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + "_D*.v2i");
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Error while gathering backup files: " + exp.Message);
                return;
            }
            if (cFiles.Length != 2 && dFiles.Length != 2)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, cFiles.Length + " - " + dFiles.Length);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + cFiles.Length + " - " + dFiles.Length);
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.BackupJobsCheck, cFiles.Length + " - " + dFiles.Length);
        }
        public static void BackupJobsReset(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Resetting Jobs");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c powershell -ep Bypass c:\service\tools\backup\RemoveImageJob.ps1 && powershell -ep Bypass c:\service\tools\backup\AddImageJob.ps1");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CMD Exited with error code: " + cmdOutput.exitCode);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Veritas jobs has been reset");
        }
        public static void DeleteOldBackupFiles(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            foreach (string drive in new string[] { "_C*.v2i", "_D*.v2i" })
            {
                massFunctionForm.GridChange(rownr, "Looking for old backup files");
                try
                {
                    string[] files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", connectionPara.TAG + drive);
                    massFunctionForm.GridChange(rownr, "Deleting files");
                    foreach (string file in files)
                    {
                        if (File.GetCreationTime(file).Day < DateTime.Today.AddDays(-1).Day)
                        {
                            string msg = file + " > " + File.GetCreationTime(file);
                            File.Delete(file);
                            massFunctionForm.AddToLog(rownr, msg + " (Deleted) | ");
                            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.DeleteOldBackupFiles, msg + " Deleted");
                        }
                    }
                }
                catch (Exception exp)
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, exp.Message);
                    return;
                }
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
        public static void IsBackupDriveAccessible(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            if (!Directory.Exists(@"\\" + connectionPara.TAG + @"\f$\Backup"))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to access Backup Drive");
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Backup Drive Available");
        }
        public static void DownloadJavaPosLogs(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            string[] files;
            massFunctionForm.GridChange(rownr, "Looking for files");
            try
            {
                files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\c$\ProgramData\javapos\wn\log", "javapos.log*");
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Failed to get files: " + exp.Message);
                return;
            }
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No logs found");
                return;
            }
            if (!FileController.MakeFolder(@".\Logs\JavaPosLogs\" + connectionPara.TAG, out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Unable to create folder in .\Logs: " + makeExp.Message);
                return;
            }
            int i = 1;
            foreach (string file in files)
            {
                massFunctionForm.GridChange(rownr, "Copying " + i + " of " + files.Length + " files");
                if (!FileController.CopyFile(file, @".\Logs\JavaPosLogs\" + connectionPara.TAG + @"\" + Path.GetFileName(file), false, out Exception copyExp))
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Error during copy: " + copyExp.Message);
                    return;
                }
                i++;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - All logs were copied");
        }
        public static void CheckForKB(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Searching KB");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-HotFix -Id " + addInfo[0] + "\"");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, addInfo[0] + " Not Installed");
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + addInfo[0] + " Installed");
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.CheckForKB, addInfo[0] + " Installed");
        }
        public static void DeployAndExecute(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Copying file");
            if (!FileController.CopyFile(Globals.toolsPath + addInfo[0], @"\\" + connectionPara.TAG + @"\c$\temp\" + addInfo[0], false, out Exception copyExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy " + addInfo[0] + " : " + copyExp.Message);
                return;
            }
            massFunctionForm.GridChange(rownr, "Executing cmd");
            int exitCode = CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c C:\temp\" + addInfo[0], Boolean.Parse(addInfo[1]));
            if (exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Script exited with error code: " + exitCode);
                return;
            }
            Telemetry.LogMachineAction(connectionPara.TAG, Globals.Funkcje.DeployAndExecute, addInfo[0] + " Executed");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + addInfo[0] + " Executed");
        }
        public static void DismAndSFC(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Starting commands");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Dism /Online /Cleanup-Image /RestoreHealth && sfc /scannow", false);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Commands started");
        }
        public static void CDriveClean(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, @"Clearing C:\Windows\Temp");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\Temp", false);
            massFunctionForm.GridChange(rownr, @"Clearing C:\Windows\CbsTemp");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\CbsTemp", false);
            massFunctionForm.GridChange(rownr, @"Clearing C:\Windows\WinSxS");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c del /q /f C:\Windows\WinSxS", false);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Commands started");
        }
        public static void BackstoreCsvExport(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Reading input file");
            string inputFile = addInfo[0] + @"\" + connectionPara.TAG + ".txt";
            string[] inputDates;
            try
            {
                inputDates = File.ReadAllLines(inputFile);
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to read input file: " + exp.Message);
                return;
            }
            if (inputDates.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No data found in input file");
                return;
            }

            massFunctionForm.GridChange(rownr, "Installing modded CA.DE.BS.CSVExport.exe");
            if (!FileController.MoveFile(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport_backup.exe", false, out Exception moveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to rename CA.DE.BS.CSVExport.exe: " + moveExp.Message + " - Please check if CA.DE.BS.CSVExport.exe is ok");
                return;
            }
            if (!FileController.CopyFile(Globals.toolsPath + "CA.DE.BS.CSVExport.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", false, out Exception copyExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy modded CA.DE.BS.CSVExport.exe on to the host: " + copyExp.Message + " - Please rename original CA.DE.BS.CSVExport.exe");
                return;
            }
            bool wasError = false;
            for (int i = 0; i < inputDates.Length; i++)
            {
                massFunctionForm.GridChange(rownr, "Exporting CSV for " + (i + 1) + " out of " + inputDates.Length + " dates");
                string output = "";
                if (inputDates[i].Trim().Length != 10)
                {
                    if (inputDates[i].Contains("[ERROR]"))
                    {
                        inputDates[i] = inputDates[i].Substring(0, 10);
                    }
                    else if (inputDates[i].Contains("[SUCCESS]"))
                    {
                        continue;
                    }
                }
                if (!CtrlFunctions.CsvExport(connectionPara, " " + inputDates[i].Trim() + " 49" + connectionPara.storeNr + @" D:\TPDotnet\Server\Reports\fiscal_files\", out string errorMsg))
                {
                    output = " - [ERROR] - CSV Export Failed: " + errorMsg;
                    wasError = true;
                }
                else
                {
                    output = " - [SUCCESS] - CSV Export successful";
                }
                Telemetry.LogFunctionUsage(Globals.Funkcje.BackstoreCsvExport);
                inputDates[i] += output;
            }
            if (!FileController.SaveTxtToFile(inputFile, String.Join(Environment.NewLine, inputDates), out Exception saveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to save result: " + saveExp.Message);
            }
            massFunctionForm.GridChange(rownr, "Restoring original CA.DE.BS.CSVExport.exe");
            if (!FileController.MoveFile(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport_backup.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", false, out moveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to restore CA.DE.BS.CSVExport.exe: " + moveExp.Message + " - Please restore CA.DE.BS.CSVExport.exe manually");
                return;
            }
            if (wasError)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CSV Export for some dates wasn't successful");
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - All CSV Export was successful");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
        public static void DiscSpaceInfo(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            string[] drives = { "c", "d", "e", "f" };
            string output = "";
            foreach (string letter in drives)
            {
                massFunctionForm.GridChange(rownr, "Reading " + letter + " drive");
                if(CtrlFunctions.GetDiskSpaceInfo(letter, connectionPara, out ulong freeBytes, out ulong totalBytes).Contains("Error"))
                {
                    output += " - " + " - " + " - ";
                }
                else
                {
                    output += (freeBytes / (1024 * 1024 * 1024)).ToString() + " - " + (totalBytes / (1024 * 1024 * 1024)).ToString() + " - " + Math.Round((((totalBytes - freeBytes) * 1.0 / totalBytes) * 100), 2, MidpointRounding.AwayFromZero).ToString() + " - ";
                }
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + output);

        }
        public static void CheckEodAbortedStatus(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
           /* 
            * Czyta daty kiedy eod bylo abort (data od kiedy w sql query)
            * Dla kazdej daty przeszukuje log w poszikuwaniu dlaczego bylo abort 
            * wrzuca wszystko w csv row per abort
            * Na zyczenie Olga Dovgalova (Problem manager)
            */
            massFunctionForm.GridChange(rownr, "Reading DB");
            if (!CtrlFunctions.SqlGetInfo(connectionPara.TAG, "TPCentralDB", "select szStartDateEOD from RetailStoreEODJournal where szStartDateEOD > " + addInfo[0] + " and szComment = 'MANUALEOD - Final result: Aborted'", out string sqlOutput)) // query do DB z data
            {
                massFunctionForm.ErrorLog(rownr, "SQL read error");
                return;
            }

            string[] dates = sqlOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            massFunctionForm.GridChange(rownr, "Checking logs");
            foreach (string tempDate in dates)
            {
                string date = tempDate.Split(':')[1].Substring(1, 8);
                string output = connectionPara.TAG + "," + date + ",";
                string[] logFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\Log", "*TSBatchActWorkstationStatus*.log"); // logi EoD
                foreach (string file in logFiles)
                {
                    try
                    {
                        foreach (string line in File.ReadAllLines(file))
                        {
                            if (line.Contains(date) && line.Contains("frmMonitor|LogInvalidStatus"))
                            {
                                int start = line.IndexOf("frmMonitor|LogInvalidStatus") + 36;
                                int end = line.IndexOf('|', start);
                                output += line.Substring(start, end - start) + " | ";
                            }
                        }
                    }
                    catch(Exception exp)
                    {
                        massFunctionForm.ErrorLog(rownr, "Log read error: " + exp.Message);
                        return;
                    }
                }
                lock (massFunctionForm.logLock)
                {
                    File.AppendAllText(Globals.userTempLogsPath + "EodAbortedCheckOutput.csv", output + Environment.NewLine);
                }
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);

        }
        public static void WinCrashReasonCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Pobiera system event log to windows log folder
             * Przeszukuje log (Query) w poszukiwaniu critical ID 41 nowsze niz konkretna data (mm.dd.yyyy)
             * I wypluwa wszystkie eventy w pliku z nazwa hosta do konkretnego folderu
             * Na prosbe Gogarowski Petre (ADV)
             */
            massFunctionForm.GridChange(rownr, "Downloading Log");
            string fileName = connectionPara.TAG + ".evtx";
            string output = "";
            if (!FileController.CopyFile(@"\\" + connectionPara.TAG + @"\c$\Windows\System32\winevt\Logs\System.evtx", @".\Logs\Windows\" + fileName, false, out Exception copyExp))
            {
                massFunctionForm.ErrorLog(rownr, copyExp.Message);
                return;
            }

            massFunctionForm.GridChange(rownr, "Quering Log");
            string newerThan = DateTime.Parse(addInfo[0]).ToUniversalTime().ToString("o");  // <-------- date from (mm.dd.yyyy)
            string query = string.Format("*[System / Level = 1] and *[System[EventID = 41]] and *[System[TimeCreated[@SystemTime >= '{0}']]]", newerThan);
            EventLogQuery eventsQuery = new EventLogQuery(@".\Logs\Windows\" + fileName, PathType.FilePath, query);
            using (EventLogReader logReader = new EventLogReader(eventsQuery))
            {
                EventRecord entry;
                while ((entry = logReader.ReadEvent()) != null)
                {
                    output += connectionPara.TAG + "," + entry.TimeCreated.ToString() + ",";
                    XDocument logEntry = XDocument.Parse(entry.ToXml());
                    string bugcheck = "0";
                    string powerbutton = "0";
                    string other = "0";
                    foreach (XElement element in logEntry.Root.Element("{http://schemas.microsoft.com/win/2004/08/events/event}EventData").Elements())
                    {
                        if (element.Attribute("Name").Value == "PowerButtonTimestamp")
                        {
                            if (element.Value != "0")
                            {
                                powerbutton = "1";
                                continue;
                            }
                        }
                        if (element.Attribute("Name").Value == "BugcheckCode")
                        {
                            if (element.Value != "0")
                            {
                                bugcheck = "1";
                            }
                        }
                    }
                    if (powerbutton == "0" && bugcheck == "0")
                    {
                        other = "1";
                    }
                    output += string.Join(",", new string[] { bugcheck, powerbutton, other }) + Environment.NewLine;
                }
                if (output != "")
                {
                    lock (massFunctionForm.logLock)
                    {
                        File.AppendAllText(Globals.userTempLogsPath + addInfo[1], output);
                    }
                }
            }
            try
            {
                File.Delete(@".\Logs\Windows\" + fileName);
            }
            catch { }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Event Log Checked");
        }
        public static void AdhocFunction(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Downloading Log");
            string fileName = connectionPara.TAG + ".evtx";
            string output = "";
            if (!FileController.CopyFile(@"\\" + connectionPara.TAG + @"\c$\Windows\System32\winevt\Logs\System.evtx", @".\Logs\Windows\" + fileName, false, out Exception copyExp))
            {
                if (copyExp != null)
                {
                    massFunctionForm.ErrorLog(rownr, copyExp.Message);
                }
                return;
            }
            massFunctionForm.GridChange(rownr, "Quering Log");

            string newerThan = DateTime.Parse("12.01.2023").ToUniversalTime().ToString("o");  // <-------- date from (mm.dd.yyyy)

            string query = string.Format("*[System / Level = 1] and *[System[EventID = 41]] and *[System[TimeCreated[@SystemTime >= '{0}']]]", newerThan);
            EventLogQuery eventsQuery = new EventLogQuery(@".\Logs\Windows\" + fileName, PathType.FilePath, query);
            using (EventLogReader logReader = new EventLogReader(eventsQuery))
            {
                EventRecord entry;
                while ((entry = logReader.ReadEvent()) != null)
                {
                    output += entry.TimeCreated.ToString() + ",";
                    XDocument logEntry = XDocument.Parse(entry.ToXml());
                    string bugcheck = "";
                    string powerbutton = "";
                    foreach (XElement element in logEntry.Root.Element("{http://schemas.microsoft.com/win/2004/08/events/event}EventData").Elements())
                    {
                        if (element.Attribute("Name").Value == "BugcheckCode")
                        {
                            bugcheck = element.Value;
                        }
                        if (element.Attribute("Name").Value == "PowerButtonTimestamp")
                        {
                            powerbutton = element.Value;
                        }
                    }
                    output += bugcheck + "," + powerbutton + Environment.NewLine;
                }
                if (output != "")
                {
                    FileController.SaveTxtToFile(@".\Crash\" + connectionPara.TAG + ".csv", output, out _);
                }
            }
            File.Delete(@".\Logs\Windows\" + fileName);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - Event Log Checked");
        }

        //------------------------Moje wymysly------------------------------//
        public static void ApcFirmwareCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Wyszukuje wszystkie logi w folderze APC energylog
             * Zaczynajac od najnowszego i szuka wpisu pod firmware
             * Jesli wpis jest null przechodzi do kolejnego pliku
             * Na prosbe Gogarowski Petre (ADV)
             */

            massFunctionForm.GridChange(rownr, "Reading log");
            try
            {
                string[] files = new DirectoryInfo(@"\\" + connectionPara.TAG + @"\c$\Program Files (x86)\APC\PowerChute Business Edition\agent\energylog").EnumerateFiles().OrderByDescending(file => file.CreationTime).Select(file => file.FullName).ToArray();
                foreach (string file in files)
                {
                    string[] log = System.IO.File.ReadAllLines(file);
                    foreach (string line in log)
                    {
                        if (line.StartsWith("# $firmware") && line.Split('=')[1] != "null")
                        {
                            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + Path.GetFileName(file) + " - " + line.Split('=')[1]);
                            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
                            return;
                        }
                    }
                }
                massFunctionForm.ErrorLog(rownr, "null");
            }
            catch
            {
                massFunctionForm.ErrorLog(rownr, "Read log error");
            }
        }
        public static void ScanerRomidVersionCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Wyciaga z dlslog.txt wersje firmware scanera
             * Na prosbe Januszka Magdalena (Problem Managment)
             */

            massFunctionForm.GridChange(rownr, "Checking Files");
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(@"\\" + connectionPara.TAG + @"\c$\Retail\Software\dlrmus\logs\dlslog.txt");
            }
            catch (Exception findExp)
            {
                massFunctionForm.ErrorLog(rownr, "Unable to read log: " + findExp.Message);
                return;
            }
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].StartsWith("ApplicationROMID") && !lines[i].Contains("===>"))
                {
                    massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
                    massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + lines[i].Split(':')[1]);
                    return;
                }
            }
            massFunctionForm.ErrorLog(rownr, "Didn't find ROMID in log");
        }
        public static void EdgeIconDeleteCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Sprawdza czy w SWAT logach jest folder z Edge icon delete
             * Na prosbe Gogarowski Petre (ADV)
             */

            massFunctionForm.GridChange(rownr, "Checking");
            if (Directory.Exists(@"\\" + connectionPara.TAG + @"\c$\oeminst\ALL_LOGS\SWAT\Edge_Icon_Delete"))
            {
                massFunctionForm.GridChange(rownr, "Good", Globals.successColor);
                massFunctionForm.AddToLog(rownr, "[SUCCESS] - Folder exist");
            }
            else
            {
                massFunctionForm.ErrorLog(rownr, "Folder don't exist");
            }
        }
        public static void SmartCheck(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Copy CrystalDisk and gets smarts of disks
             * Gets result and puts each disk health status in txt file
             * >> ze wzgledu na duza ilosc kopiowania zaleca sie zmniejszenie ilosci background workerow do 5 <<
             * Moj wymysl (ADV)
             */
            massFunctionForm.GridChange(rownr, "Getting SMART");
            if (!CtrlFunctions.Smarty(connectionPara, out string errorMsg))
            {
                massFunctionForm.ErrorLog(rownr, errorMsg);
            }
            else
            {
                massFunctionForm.GridChange(rownr, "Reading SMART");
                string output = connectionPara.TAG + "\t";
                string[] smartLog = File.ReadAllLines(@"\\" + connectionPara.TAG + @"\c$\SMART\DiskInfo.txt");
                foreach (string line in smartLog)
                {
                    if (line.Contains("Model") || line.Contains("Disk Size") || line.Contains("Health Status"))
                    {
                        output += line.Split(':')[1] + "\t";
                    }
                }
                output += Environment.NewLine;
                File.AppendAllText(@".\Logs\SmartCheck.txt", output);
            }
            massFunctionForm.GridChange(rownr, "Deleting Lock");
            if (!CtrlFunctions.DeleteLock(@"\\" + connectionPara.TAG + @"\c$\SMART\smart.lock"))
            {
                massFunctionForm.GridChange(rownr, "Lock delete error");
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - SMART Data Saved");
        }
        public static void LookForWinSxsFolders(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for folders");
            string[] folders = File.ReadAllLines(@".\folders.txt");
            for (int i = 0; i < folders.Length; i++)
            {
                massFunctionForm.GridChange(rownr, "Looking for folders");
                if (folders[i] == "")
                {
                    continue;
                }
                if (Directory.Exists(@"\\" + connectionPara.TAG + @"\c$\Windows\WinSxS\" + folders[i]))
                {
                    massFunctionForm.GridChange(rownr, "Copying folder");
                    if (!FileController.CopyFolder(@"\\" + connectionPara.TAG + @"\c$\Windows\WinSxS\" + folders[i], @".\Foldery\" + folders[i], false, out Exception copyExp))
                    {
                        massFunctionForm.ErrorLog(rownr, copyExp.Message);
                    }
                    folders[i] = "";
                }
            }
            File.WriteAllLines(@".\folders.txt", folders);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            
        }
        public static void RfidErrorFinder(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Sprawdza i dobiera odpowiednia sciezke w zaleznosci od wersji ProBase
             * Robi kopie logu poniewaz jest blokowany
             * Przeszukuje kazda linijke najnowszego logu jpos i zlicza error 
             * Na koniec usuwa kopie logu i wypluwa ilosc bledow na stacji do output loga
             * Na prosbe Dovgalova Olga (PM)
             */

            string errorToFind = "Bad file descriptor in nativeavailable";

            massFunctionForm.GridChange(rownr, "Reading log");
            string sciezka = @"\\" + connectionPara.TAG + @"\c$\ProgramData\javapos\wn\log\jniwrapper-diagnostics.log";
            if (!File.Exists(sciezka))
            {
                sciezka = @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSRFIDScannerLogs.log";
            }
            string sciezka2 = sciezka + "bak";

            if (!FileController.CopyFile(sciezka, sciezka2, false, out Exception copyExp))
            {
                massFunctionForm.ErrorLog(rownr, "Error copying log: " + copyExp.Message);
                return;
            }

            string[] log;
            try
            {
                log = File.ReadAllLines(sciezka2);
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, "Error reading log - " + exp.Message);
                return;
            }
            int errorCount = 0;
            foreach (string line in log)
            {
                if (line.Contains(errorToFind))
                {
                    errorCount++;
                }
            }
            File.Delete(sciezka2);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + errorCount);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
        public static void GetAndUpdateBios(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Zczytuje wersje BIOS
             * Sprawdza czy odpowiada podanej wartosci
             * Jesli sie nie zgadza sprawdza czy folder z updatem znajduje sie na dysku
             * Jesli nie ma update na dysku kopiuje go do tempa z folderu toola
             * Nastepnie odpala CMD z updatem
             * Na zyczenie Petre Gogarowski (ADV)
             */
            massFunctionForm.GridChange(rownr, "Reading BIOS Version");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c systeminfo | find /i \"BIOS Version\"");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.GridChange(rownr, "Error", Globals.errorColor);
                massFunctionForm.ErrorLog(rownr, "Can't read BIOS version");
                return;
            }
            if (cmdOutput.outputText.Contains("06/08")) // wersja biosu do sprawdzenia
            {
                massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
                massFunctionForm.AddToLog(rownr, "[SUCCESS] - BIOS version is up to date");
                return;
            }
            if (!Directory.Exists(@"\\" + connectionPara.TAG + @"\c$\temp\R12_UEFI_0608_new")) // folder z updatem na dysku
            {
                massFunctionForm.GridChange(rownr, "Copying files");
                if (!FileController.CopyFolder(@".\R12_UEFI_0608_new", @"\\" + connectionPara.TAG + @"\c$\temp\R12_UEFI_0608_new", false, out _)) // sciezki do kopiowania
                {
                    massFunctionForm.GridChange(rownr, "Error", Globals.errorColor);
                    massFunctionForm.ErrorLog(rownr, "Can't copy update files");
                    return;
                }
            }
            massFunctionForm.GridChange(rownr, "Updating BIOS");
            cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c C:\temp\R12_UEFI_0608_new\R12_UEFI_0608_Win\UEFI_Update.cmd"); // sciezka do cmd
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.GridChange(rownr, "Error", Globals.errorColor);
                massFunctionForm.ErrorLog(rownr, "Error while flashing BIOS");
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - BIOS has been updated");

        }
        public static void RepackArchivedReportsToOutputZip(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            /*
             * Gets dates (YYYYMMDD) in .\Dates folder 
             * Create temp folder and collect_tp_reports.zip if not exist in dms_output
             * For each date its look for backup archive in ArchivedReports, if found they are extracted to temp folder
             * Then files from temp folder are packed into output zip and temp folder is deleted
             * Log everything in dates txt, on reruns it skips [SUCCESS] and retry [ERROR]
             * Stworzone na poczet odzyskiwania brakujacych raporow (Olga Dovgalova PM, Novotny Adrian)
             */ 


            massFunctionForm.GridChange(rownr, "Reading dates");
            string[] dates = File.ReadAllLines(@".\Dates\" + connectionPara.TAG + @".txt");
            int iterator = -1;
            try
            {
                Directory.CreateDirectory(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\temp");
                if (!File.Exists(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip"))
                {
                    File.Create(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip").Close();
                }
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, "Error creating output zip");
                dates[iterator] += ",[ERROR],unable to create zip file or temp folder in output folder: " + exp.Message;
                return;
            }
            try
            {
                foreach (string fuckDate in dates)
                {
                    iterator++;
                    string date = fuckDate;
                    if (date.Length > 8)
                    {
                        if (date.Contains("[ERROR]"))
                        {
                            dates[iterator] = date.Substring(0, 8);
                            date = date.Substring(0, 8);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    massFunctionForm.GridChange(rownr, "Restoring date: " + date + "(" + (iterator + 1).ToString() + "/" + dates.Length + ")");

                    string zipDate = (int.Parse(date) + 1).ToString();
                    if (date == "20231231") { zipDate = "20240101"; }
                    string[] zipFiles = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\ArchivedReports", "collect_tp_reports.zip." + zipDate + "030*");
                    if (zipFiles.Length > 1)
                    {
                        massFunctionForm.ErrorLog(rownr, "Error check date txt");
                        dates[iterator] += ",[ERROR],more than one zip file on this date need manual check";
                    }
                    else if (zipFiles.Length == 1)
                    {
                        try
                        {
                            using (ZipArchive backupArchive = ZipFile.OpenRead(zipFiles[0]))
                            using (ZipArchive outputArchive = ZipFile.Open(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\collect_tp_reports.zip", ZipArchiveMode.Update))
                            {
                                foreach (var zipedRaport in backupArchive.Entries)
                                {
                                    if (!zipedRaport.FullName.Contains(connectionPara.country + @"\"))
                                    {

                                        //long datePliku = long.Parse(zipedRaport.Name.Substring(zipedRaport.Name.IndexOf('_') + 1, 14))+1;
                                        //string nazwaPliku = zipedRaport.Name.Substring(0, zipedRaport.Name.IndexOf('_') + 1) + datePliku + zipedRaport.Name.Substring(zipedRaport.Name.LastIndexOf('_'));
                                        zipedRaport.ExtractToFile(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\temp\" + zipedRaport.Name, true);
                                        outputArchive.CreateEntryFromFile(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\temp\" + zipedRaport.Name, zipedRaport.Name);
                                    }
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            massFunctionForm.ErrorLog(rownr, "Error check date txt");
                            dates[iterator] += ",[ERROR],unable to handle zip file check manually: " + exp.Message;
                            continue;
                        }
                        dates[iterator] += ",[SUCCESS],reports found in ArchivedReports - copied to output zip";
                        massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
                    }
                    else
                    {
                        massFunctionForm.ErrorLog(rownr, "Error check date txt");
                        dates[iterator] += ",[FATAL],no reports found manual check needed";
                    }
                }
                File.WriteAllLines(@".\Dates\" + connectionPara.TAG + @".txt", dates);
                try
                {
                    Directory.Delete(@"\\" + connectionPara.TAG + @"\c$\service\dms_output\temp", true);
                }
                catch (Exception exp)
                {
                    massFunctionForm.ErrorLog(rownr, "Temp delete error");
                    dates[iterator] += ",[ERROR],Can't delete some files in temp folder";
                }
            }
            catch (Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, exp.ToString());
            }
        }
    }
}
