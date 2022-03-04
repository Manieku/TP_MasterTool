using System;
using System.Collections.Generic;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{
    public static class StartUp
    {
        private static void StartLocationCheck(ref Logger myLog)
        {
            myLog.Add("StartLocationCheck");
            if (Environment.CurrentDirectory.Equals(@"D:\TP_MasterTool"))
            {
                myLog.Add("Started in default location");
                myLog.SaveLog("ErrorLog");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Startup Execution Error", "Please don't open toolbox in it's default folder. Copy TP_MasterTool.exe to other location to prevent unexpected interaction with other users.");
                Environment.Exit(0);
                return;
            }
        }
        public static void MaintenanceModeCheck(ref Logger myLog)
        {
            myLog.Add("Maintenance Check");
            if(System.IO.File.Exists(Globals.configPath + "MaintenanceConfig.ini"))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Maintenance", System.IO.File.ReadAllText(Globals.configPath + "MaintenanceConfig.ini"));
                Environment.Exit(0);
                return;
            }
        }
        private static void FolderTreeCheck(ref Logger myLog)
        {
            myLog.Add("FolderTreeCheck");

            if (!System.IO.Directory.Exists(Globals.configPath))
            {
                myLog.Add("ToolBox cannot access Configuration folder on the server");
                myLog.SaveLog("CriticalError");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File access error", @"ToolBox cannot access Configuration folder on the server. Please chceck if D:\ drive is available and notify Dev Team, because ToolBox can't function without access to those files");
                Environment.Exit(0);
            }

            if (!System.IO.Directory.Exists(Globals.toolsPath))
            {
                myLog.Add("ToolBox cannot access Tools folder on the server");
                myLog.SaveLog("CriticalError");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File access error", @"ToolBox cannot access Tools folder on the server. Please chceck if D:\ drive is available and notify Dev Team, because ToolBox can't function without access to those files");
                Environment.Exit(0);
            }

            List<bool> checkList = new List<bool>
            {
                FileController.MakeFolder(@".\Logs\SMART", ref myLog),
                FileController.MakeFolder(@".\Logs\Windows", ref myLog),
                FileController.MakeFolder(@".\Config", ref myLog),
                FileController.MakeFolder(@".\Notes", ref myLog),
                FileController.MakeFolder(Globals.telemetryLogPath, ref myLog),
                FileController.MakeFolder(Globals.machineLogPath, ref myLog),
                FileController.MakeFolder(Globals.logErrorPath, ref myLog),
                FileController.MakeFolder(Globals.versionLogPath, ref myLog)
            };
            if (checkList.Contains(false))
            {
                myLog.wasError = true;
            }
        }
        private static void FilesCheck(ref Logger myLog)
        {
            myLog.Add("FilesChceck");
            if (!System.IO.File.Exists(@".\psexec.exe"))
            {
                myLog.Add("Psexec not found");
                if (!FileController.CopyFile(Globals.toolsPath + "PsExec.exe", @".\PsExec.exe", true, ref myLog))
                {
                    myLog.wasError = true;
                    myLog.Add("psexec.exe is missing and couldn't be copy into tool folder");
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "PsExec.exe not found", "PsExec.exe could not be found in main folder with aplication. Please chceck if files are present otherwise most of funcionality will be unavaible");
                }
            }

            if (!System.IO.File.Exists(Globals.telemetryLogPath + @"FunctionStats.xml"))
            {
                Telemetry.CreateFunctionUsageXml(ref myLog);
            }

            if (!System.IO.File.Exists(Globals.versionLogPath + "VersionLog.xml"))
            {
                VersionControl.CreateVersionLogXML();
            }
        }
        private static void UpdateProcedure(ref Logger myLog)
        {
            if (VersionControl.IsUpdateAvailable(ref myLog))
            {
                VersionControl.PreformUpdate(ref myLog);
            }
            if (VersionControl.IsBelowMinimalAcceptedVersion(ref myLog))
            {
                if (myLog.wasError)
                {
                    myLog.SaveLog("ErrorLog");
                }
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Minimal Version Not Satisfied", "Your ToolBox version don't meet the minimal requirements. Please restart the ToolBox and preform the update");
                Environment.Exit(0);
            }
            VersionControl.LogVersion();
        }
        private static void UIsetup(ref Logger myLog)
        {
            myLog.Add("UIsetup");
            if (!Globals.mineAccounts.Contains(Logger.EnvironmentVariables.activeUser))
            {
                Main.interfejs.Test_Button.Visible = false;
            }
            if (Globals.advUsers.Contains(Logger.EnvironmentVariables.activeUser))
            {
                Main.interfejs.ADVMenuItem.Visible = true;
            }
            Main.interfejs.menuStrip1.CanOverflow = true;
            Main.interfejs.currentVersionMenuItem.Text = "Current Version: v" + Logger.EnvironmentVariables.programVersion;
        }
        private static void UserSettingsInit(ref Logger myLog)
        {
            myLog.Add("UserSettingsInit");
            if (!System.IO.File.Exists(Globals.userSettingsXmlPath))
            {
                myLog.Add("No user settings file found - Creating");
                Main.interfejs.userSettings.SaveUserSettingsToXml();
            }
            else
            {
                myLog.Add("Reading settings from file");
                Main.interfejs.userSettings = UserSettings.ReadUserSettingFromXml();
            }
            myLog.Add("Applying settings");
            Main.interfejs.userSettings.ApplySettings();
        }
        private static void Cleaner(ref Logger myLog)
        {
            myLog.Add("Cleaner");
            try
            {
                System.IO.File.Delete(@".\TP_Updater.exe"); //delete updater if update was preformed
            }
            catch (Exception exp)
            {
                myLog.wasError = true;
                myLog.Add("Error during deleting old files");
                myLog.Add(exp.ToString());
            }
        }
        private static void ToolBoxSetup(ref Logger myLog)
        {
            myLog.Add("Credentials initialization");
            if(!CtrlFunctions.DecryptToString(Globals.configPath + "credentials.crypt", "cycuszki", out string tempCredentials))
            {
                myLog.Add("Unable to decrypt credentials from encrypted file" + Environment.NewLine + tempCredentials);
                myLog.SaveLog("CriticalError");
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "ToolBox setup failed", tempCredentials + Environment.NewLine + "Please contact Dev Team, because ToolBox can't function without access to that file");
                Environment.Exit(0);
            }
            string[] credentials = tempCredentials.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Globals.PRODuserName = credentials[0];
            Globals.PRODpassword = credentials[1];
            Globals.TESTuserName = credentials[2];
            Globals.TESTpassword = credentials[3];
            Globals.SQLuserName = credentials[4];
            Globals.SQLpassword = credentials[5];
        }

        //-------------------------------------------//
        public static void StartUpProcedure()
        {
            Logger myLog = new Logger(Globals.Funkcje.StartupProcedure, "none", "none");
            StartLocationCheck(ref myLog);
            MaintenanceModeCheck(ref myLog);
            UpdateProcedure(ref myLog);
            FolderTreeCheck(ref myLog);
            FilesCheck(ref myLog);
            Cleaner(ref myLog); //deletes old useless files after previous version 
            UIsetup(ref myLog);
            UserSettingsInit(ref myLog);
            ToolBoxSetup(ref myLog);
            Main.interfejs.DisableUI();
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
        }
    }
}
