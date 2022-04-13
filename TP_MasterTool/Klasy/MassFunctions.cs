using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms;

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
        public static List<string> GetInfo_MassJposLogsDownload()
        {
            string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:", "Input data");
            if (tixnr == "")
            {
                return null;
            }
            return new List<string> { tixnr };
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
            string file = Microsoft.VisualBasic.Interaction.InputBox(@"Provide file name you want to deply and execute from D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Tools", "Input data");
            if (file == "")
            {
                return null;
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
            if(folder == "")
            {
                return null;
            }
            return new List<string> { folder };
        }

        //-------Mass Functions-------------
        public static void GetMAC(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Stealing MAC");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
            if(cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Failed to steal MAC");
                return;
            }
            string output = cmdOutput.outputText.Replace("\n", "").Replace("\r", "").ToUpper();
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + output);
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.GetMAC, output);
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

            if(!FileController.MakeFolder(@"\\" + connectionPara.TAG + @"\d$\WNI\Invalid_Transfer", out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Unable to create folder \d$\WNI\Invalid_Transfer: " + makeExp.Message);
                return;
            }

            bool error = false;
            foreach (string file in files)
            {
                if(!FileController.MoveFile(file, @"\\" + connectionPara.TAG + @"\d$\WNI\Invalid_Transfer\" + Path.GetFileName(file), false, out Exception moveExp))
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
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalid, "Found Invalid Items");
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
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalid, "Found Invalid Items");
                massFunctionForm.ErrorLog(rownr, "Other Invalid xml found, please check manually and include it in note to MMS team");
                return;
            }

            massFunctionForm.AddToLog(rownr, "[AFFECTED] - Found Invalid Items");
            lock (massFunctionForm.logLock)
            {
                massFunctionForm.log = massFunctionForm.log.Concat(output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ToArray();
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalid, "Found Invalid Items");
            }
            massFunctionForm.GridChange(rownr, "Found Invalid", Globals.errorColor);
        }
        public static void BulkFileMove(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string source = @"\\" + connectionPara.TAG + @"\" + addInfo[0];
            string destination = @"\\" + connectionPara.TAG + @"\" + addInfo[1];
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.BulkFileMove, source + " -> " + destination + " filter: " + addInfo[2]);
            if (!Directory.Exists(source))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Source folder not found");
                return;
            }
            if(!FileController.MakeFolder(destination, out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to create destination folder: " + makeExp.Message);
                return;
            }
            string[] files = Directory.GetFiles(source, addInfo[2]);
            if(files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No file matching the criteria is found");
                return;
            }
            int licznik = 1;
            foreach(string file in files)
            {
                massFunctionForm.GridChange(rownr, "Moving " + licznik + " out of " + files.Length + " files");
                if(!FileController.MoveFile(file, destination + @"\" + Path.GetFileName(file), false, out Exception moveExp))
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
        public static void MassJposLogsDownload(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string[] files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService", "JPOSRFIDScannerLogs*");
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No JPOS logs found");
                return;
            }

            if (Directory.Exists(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs"))
            {
                massFunctionForm.GridChange(rownr, "Deleting old zip");
                try
                {
                    Directory.Delete(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", true);
                }
                catch (Exception exp)
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Can't delete folder with old logs, Please delete D:\TPDotnet\DeviceService\JPOSLogs manually and try again: " + exp.Message);
                    return;
                }
            }

            massFunctionForm.GridChange(rownr, "Gathering Logs");
            System.Threading.Thread.Sleep(150);
            if(!FileController.MakeFolder(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to create folder for logs: " + makeExp.Message);
                return;
            }

            foreach (string file in files)
            {
                if (!FileController.CopyFile(file, @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs\" + Path.GetFileName(file), false, out Exception copyExp))
                {
                    massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy " + file + ": " + copyExp.Message);
                    return;
                }
            }
            massFunctionForm.GridChange(rownr, "Securing Logs");
            if(!FileController.ZipAndStealFolder(addInfo[0], "JposLogs", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", @"D:\TPDotnet\DeviceService\JPOSLogs", connectionPara, out string outputFilePath))
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
            if(cFiles.Length != 2 && dFiles.Length != 2)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, cFiles.Length + " - " + dFiles.Length);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + cFiles.Length + " - " + dFiles.Length);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.BackupJobsCheck, cFiles.Length + " - " + dFiles.Length);
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
            foreach(string drive in new string[] { "_C*.v2i" , "_D*.v2i" })
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
                            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.DeleteOldBackupFiles, msg + " Deleted");
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
            catch(Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Failed to get files: " + exp.Message);
                return;
            }
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No logs found");
                return;
            }
            if(!FileController.MakeFolder(@".\Logs\JavaPosLogs\" + connectionPara.TAG, out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, @"Unable to create folder in .\Logs: " + makeExp.Message);
                return;
            }
            int i = 1;
            foreach (string file in files)
            {
                massFunctionForm.GridChange(rownr, "Copying " + i + " of " + files.Length + " files");
                if(!FileController.CopyFile(file, @".\Logs\JavaPosLogs\" + connectionPara.TAG + @"\" + Path.GetFileName(file), false, out Exception copyExp))
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
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.CheckForKB, addInfo[0] + " Installed");
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
            }
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.DeployAndExecute, addInfo[0] + " Exetuted");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + addInfo[0] + " Exetuted");
        }
        public static void DismAndSFC(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Starting commands");
            CtrlFunctions.RunHiddenCmdWitoutOutput("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Dism /Online /Cleanup-Image /RestoreHealth && sfc /scannow", false);
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
            catch(Exception exp)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to read input file: " + exp.Message);
                return;
            }
            if(inputDates.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "No data found in input file");
                return;
            }

            massFunctionForm.GridChange(rownr, "Installing modded CA.DE.BS.CSVExport.exe");
            if(!FileController.MoveFile(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport_backup.exe", false, out Exception moveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to rename CA.DE.BS.CSVExport.exe: " + moveExp.Message + " - Please check if CA.DE.BS.CSVExport.exe is ok");
                return;
            }
            if(!FileController.CopyFile(Globals.toolsPath + "CA.DE.BS.CSVExport.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", false, out Exception copyExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to copy modded CA.DE.BS.CSVExport.exe on to the host: " + copyExp.Message + " - Please rename original CA.DE.BS.CSVExport.exe");
                return;
            }
            bool wasError = false;
            for(int i = 0; i < inputDates.Length; i++)
            {
                massFunctionForm.GridChange(rownr, "Exporting CSV for " + (i + 1) + " out of " + inputDates.Length + " dates");
                string output = "";
                if(!CtrlFunctions.CsvExport(connectionPara, " " + inputDates[i].Trim() + " 49" + connectionPara.storeNr + @" D:\TPDotnet\Server\Reports\fiscal_files\", out string errorMsg))
                {
                    output = " - [ERROR] - CSV Export Failed: " + errorMsg;
                    wasError = true;
                }
                else
                {
                    output = " - [SUCCESS] - CSV Export successful";
                }
                inputDates[i] += output;
            }
            if(!FileController.SaveTxtToFile(inputFile, String.Join(Environment.NewLine, inputDates), out Exception saveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to save result: " + saveExp.Message);
            }
            massFunctionForm.GridChange(rownr, "Restoring original CA.DE.BS.CSVExport.exe");
            if(!FileController.MoveFile(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport_backup.exe", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\bin\CA.DE.BS.CSVExport.exe", false, out moveExp))
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "Unable to restore CA.DE.BS.CSVExport.exe: " + moveExp.Message + " - Please restore CA.DE.BS.CSVExport.exe manually");
                return;
            }
            if(wasError)
            {
                massFunctionForm.ErrorLog(rownr, connectionPara.TAG, "CSV Export for some dates wasn't successful");
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - All CSV Export was successful");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
    }
}
