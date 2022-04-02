using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TP_MasterTool.Forms;

namespace TP_MasterTool.Klasy
{
    static class MassFunctions
    {
        //-------GetInfo-------------------
        public static List<string> GetInfo_Test()
        {
            return new List<string> { "placki", "cycki" };
        }
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

            string destination = Microsoft.VisualBasic.Interaction.InputBox(@"Provide path to source folder from which you want to move files (e.g. d$\WNI)", "Input data");
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


        //-------Mass Functions-------------
        public static void Test(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "outside test");
            lock (massFunctionForm.logLock)
            {
                massFunctionForm.log[rownr] += addInfo[0] + " - " + addInfo[1];
            }
        }
        public static void GetMAC(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Stealing MAC");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c powershell -command \"Get-WmiObject win32_networkadapterconfiguration | where {$_.ipaddress -like '" + connectionPara.IP + "*'} | select macaddress | ft -hidetableheaders\"");
            if(cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, "Failed to steal MAC");
            }
            string output = cmdOutput.outputText.Replace("\n", "").Replace("\r", "").ToUpper();
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + output);

            lock(massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.GetMAC, output);
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
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
                massFunctionForm.ErrorLog(rownr, "Error while searching files: " + exp.Message);
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
                massFunctionForm.ErrorLog(rownr, @"Unable to create folder \d$\WNI\Invalid_Transfer: " + makeExp.Message);
                return;
            }

            bool error = false;
            foreach (string file in files)
            {
                if(!FileController.MoveFile(file, @"\\" + connectionPara.TAG + @"\d$\WNI\Invalid_Transfer\" + Path.GetFileName(file), false, out Exception moveExp))
                {
                    error = true;
                    massFunctionForm.ErrorLog(rownr, "Unable to move file: " + file + ": " + moveExp.Message);
                    continue;
                }
            }
            if (error)
            {
                return;
            }
            massFunctionForm.GridChange(rownr, "Success", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + files.Length + " invalid transactions was moved");
            lock (massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.InvTansactionTransfer, "Successful");
            }
        }
        public static void TpReportsRegenAndZip(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Executing runeodreports.bat");
            if (!CtrlFunctions.RegenerateEoDReports(connectionPara, addInfo[0], addInfo[1], out string regenOutput))
            {
                massFunctionForm.ErrorLog(rownr, regenOutput);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + regenOutput);

            massFunctionForm.GridChange(rownr, "Executing collect_tp_reports.ps1");
            if (!CtrlFunctions.ZipEoDReports(connectionPara, out string zipOutput))
            {
                massFunctionForm.ErrorLog(rownr, zipOutput);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] " + zipOutput);

            lock (massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.TPReportsRegenZip, "");
            }
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
                massFunctionForm.ErrorLog(rownr, "Other Invalid xml found - please check manually and include it in note to MMS team");
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
                massFunctionForm.ErrorLog(rownr, "Other Invalid xml found - please check manually and include it in note to MMS team");
                return;
            }

            lock (massFunctionForm.logLock)
            {
                massFunctionForm.AddToLog(rownr, "[AFFECTED] - Found Invalid Items");
                massFunctionForm.log = massFunctionForm.log.Concat(output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ToArray();
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.UpdatePackageInvalid, "Found Invalid Items");
            }
            massFunctionForm.GridChange(rownr, "Found Invalid", Globals.errorColor);
        }
        public static void BulkFileMove(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string source = @"\\" + connectionPara.TAG + @"\" + addInfo[0];
            string destination = @"\\" + connectionPara.TAG + @"\" + addInfo[1];
            if (!Directory.Exists(source))
            {
                massFunctionForm.ErrorLog(rownr, "Source folder not found");
                return;
            }
            if(!FileController.MakeFolder(destination, out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, "Unable to create destination folder: " + makeExp.Message);
                return;
            }
            string[] files = Directory.GetFiles(source, addInfo[2]);
            if(files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, "No file matching the criteria is found");
                return;
            }
            int licznik = 1;
            foreach(string file in files)
            {
                massFunctionForm.GridChange(rownr, "Moving " + licznik + " out of " + files.Length + " files");
                if(!FileController.MoveFile(file, destination + @"\" + Path.GetFileName(file), false, out Exception moveExp))
                {
                    massFunctionForm.ErrorLog(rownr, "Operation cancelled, Unable to move file: " + moveExp);
                    return;
                }
                licznik++;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - " + files.Length + " files were moved");
            lock(massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.BulkFileMove, source + " -> " + destination);
            }
        }
        public static void EsfClientRestart(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Restarting Client");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + " cmd /c net stop esfclient && net start esfclient");
            if (cmdOutput.exitCode != 0)
            {
                massFunctionForm.ErrorLog(rownr, "CMD exited with error code: " + cmdOutput.exitCode);
                return;
            }

            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - ESF Client Restarted");
            lock (massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.EsfClientRestart, "");
            }
        }
        public static void EsfClientReinit(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Running script");
            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c cd c:\service\agents\esfclient && reinit_esfclient.cmd");
            if (cmdOutput.exitCode != 0)
            { 
                massFunctionForm.ErrorLog(rownr, "CMD exited with error code: " + cmdOutput.exitCode);
                return;
            }
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - ESF Client Reinitialized");
            lock (massFunctionForm.logLock)
            {
                Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.EsfClientReinit, "");
            }
        }
        public static void MassJposLogsDownload(MassFunctionForm massFunctionForm, int rownr, ConnectionPara connectionPara, List<string> addInfo)
        {
            massFunctionForm.GridChange(rownr, "Looking for files");
            string[] files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService", "JPOSRFIDScannerLogs*");
            if (files.Length == 0)
            {
                massFunctionForm.ErrorLog(rownr, "No JPOS logs found");
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
                    massFunctionForm.ErrorLog(rownr, @"Can't delete folder with old logs, Please delete D:\TPDotnet\DeviceService\JPOSLogs manually and try again: " + exp.Message);
                    return;
                }
            }

            massFunctionForm.GridChange(rownr, "Gathering Logs");
            System.Threading.Thread.Sleep(150);
            if(!FileController.MakeFolder(@"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", out Exception makeExp))
            {
                massFunctionForm.ErrorLog(rownr, "Unable to create folder for logs: " + makeExp.Message);
                return;
            }

            foreach (string file in files)
            {
                if (!FileController.CopyFile(file, @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs\" + Path.GetFileName(file), false, out Exception copyExp))
                {
                    massFunctionForm.ErrorLog(rownr, "Unable to copy " + file + ": " + copyExp.Message);
                    return;
                }
            }
            massFunctionForm.GridChange(rownr, "Securing Logs");
            if(!FileController.ZipAndStealFolder(addInfo[0], "JposLogs", @"\\" + connectionPara.TAG + @"\d$\TPDotnet\DeviceService\JPOSLogs", @"D:\TPDotnet\DeviceService\JPOSLogs", connectionPara, out string outputFilePath))
            {
                massFunctionForm.ErrorLog(rownr, outputFilePath);
                return;
            }
            massFunctionForm.GridChange(rownr, "Downloading Logs");
            if (!FileController.CopyFile(outputFilePath, Globals.userTempLogsPath + Path.GetFileName(outputFilePath), false, out Exception copyExp2))
            {
                massFunctionForm.ErrorLog(rownr, "Unable to copy log: " + copyExp2.Message);
                return;
            }
            massFunctionForm.AddToLog(rownr, "[SUCCESS] - JposLogs Downloaded");
            massFunctionForm.GridChange(rownr, "Done", Globals.successColor);
        }
    }
}
