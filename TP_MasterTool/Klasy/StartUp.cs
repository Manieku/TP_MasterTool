using System;
using System.Collections.Generic;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Klasy
{
    public static class StartUp
    {
        public static void StartLocationCheck(ref Logger myLog)
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
        public static void PsexecCheck(ref Logger myLog)
        {
            myLog.Add("PsexecCheck");
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
        }
        public static void FolderTreeCheck(ref Logger myLog)
        {
            myLog.Add("FolderTreeCheck");
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

            if (!System.IO.File.Exists(Globals.versionLogPath + "VersionLog.xml"))
            {
                VersionControl.CreateVersionLogXML();
            }
        }
        public static void UpdateProcedure(ref Logger myLog)
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
        public static void UIsetup(ref Logger myLog)
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
        public static void TelemetryFilesChceck(ref Logger myLog)
        {
            myLog.Add("TelemetryFilesChceck");
            if (!System.IO.File.Exists(Globals.telemetryLogPath + @"FunctionStats.xml"))
            {
                Telemetry.CreateFunctionUsageXml(ref myLog);
            }
        }
        public static void UserSettingsInit(ref Logger myLog)
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
        public static void Cleaner(ref Logger myLog)
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

        //-------------------------------------------//
        public static void StartUpProcedure()
        {
            Logger myLog = new Logger(Globals.Funkcje.StartupProcedure, "none", "none");
            StartLocationCheck(ref myLog);
            MaintenanceModeCheck(ref myLog);
            UpdateProcedure(ref myLog);
            PsexecCheck(ref myLog);
            FolderTreeCheck(ref myLog);
            TelemetryFilesChceck(ref myLog);
            Cleaner(ref myLog); //deletes old useless files after previous version 
            UIsetup(ref myLog);
            UserSettingsInit(ref myLog);
            Main.interfejs.DisableUI();
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
        }
    }
}
