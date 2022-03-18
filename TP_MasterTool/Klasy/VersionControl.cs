using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{
    public static class VersionControl
    {
        public static bool IsUpdateAvailable(ref Logger myLog)
        {
            myLog.Add("Checking for update");
            Version masterVersion;
            try
            {
                masterVersion = new Version(FileVersionInfo.GetVersionInfo(@"D:\TP_MasterTool\TP_MasterTool.exe").FileVersion);
            }
            catch (Exception exp)
            {
                myLog.wasError = true;
                myLog.Add(@"Unable to read ToolBox version from D:\TP_MasterTool\TP_MasterTool.exe");
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Check for update failed", "Unable to connect to config file in order to check latest version of toolbox." + "\n" + "Error description: " + exp.Message);
                return false;
            }
            Version currVersion = new Version(FileVersionInfo.GetVersionInfo(Logger.EnvironmentVariables.executionLocation).FileVersion);
            myLog.Add("This exe v:" + currVersion.ToString() + " | Master v:" + masterVersion.ToString());
            if (currVersion.CompareTo(masterVersion) < 0)
            {
                myLog.Add("Newer version found");
                return true;
            }
            myLog.Add("No update found");
            return false;
        }
        public static void PreformUpdate(ref Logger myLog)
        {
            if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, "Update available", @"There is newest version of toolbox available do download." + "\n" + "Would you like to update it now?") != DialogResult.OK)
            {
                myLog.Add("Update canceled by user");
                myLog.wasError = true;
                return;
            }
            myLog.Add("Downloading updater");
            if(!FileController.CopyFile(Globals.toolsPath + "TP_Updater.exe", @".\TP_Updater.exe", false, out Exception copyExp))
            {
                myLog.wasError = true;
                myLog.Add("Failed to download");
                myLog.Add(copyExp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Problem with update", "Toolbox encounter problem during downloading updater with error:\n" + copyExp.Message + "\nPlease restart toolbox and try again or inform dev team.");
                return;
            }

            Process.Start(@".\TP_Updater.exe");
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
            Environment.Exit(0);
        }
        public static bool IsBelowMinimalAcceptedVersion(ref Logger myLog)
        {
            myLog.Add("Checking is satisfy minimal version");
            Version currVersion = new Version(FileVersionInfo.GetVersionInfo(Logger.EnvironmentVariables.executionLocation).FileVersion);
            Version minVersion;
            try
            {
                minVersion = new Version(System.IO.File.ReadAllText(Globals.configPath + "VersionControl.ini"));
            }
            catch (Exception exp)
            {
                myLog.wasError = true;
                myLog.Add(@"Unable to read minimal version from VersionControl.ini");
                myLog.Add(exp.ToString());
                return false;
            }
            if (currVersion.CompareTo(minVersion) >= 0)
            {
                myLog.Add("ToolBox satisfy minimal requirement");
                return false;
            }
            myLog.wasError = true;
            myLog.Add("ToolBox don't satisfy minimal requirements");
            return true;
        }
        public static void LogVersion()
        {
            try
            {
                XDocument VersionLogXml = XDocument.Load(Globals.versionLogPath + "VersionLog.xml");
                XElement node = VersionLogXml.Root.Element(Environment.UserName);
                if (node == null)
                {
                    VersionLogXml.Root.Add(new XElement(Environment.UserName, new XElement("Version", FileVersionInfo.GetVersionInfo(Logger.EnvironmentVariables.executionLocation).FileVersion), new XElement("TimeStamp", DateTime.Now.ToString())));
                }
                else
                {
                    node.Element("Version").Value = FileVersionInfo.GetVersionInfo(Logger.EnvironmentVariables.executionLocation).FileVersion;
                    node.Element("TimeStamp").Value = DateTime.Now.ToString();
                }
                VersionLogXml.Save(Globals.versionLogPath + "VersionLog.xml");
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.VersionLog, Globals.versionLogPath + "VersionLog.xml", "", "TelemetryError", exp.ToString());
            }

        }
        public static void CreateVersionLogXML()
        {
            try
            {
                XDocument VersionLogXml = new XDocument();
                VersionLogXml.Add(new XElement("FunctionStats"));
                VersionLogXml.Save(Globals.versionLogPath + "VersionLog.xml");
            }
            catch { }
        }
    }
}
