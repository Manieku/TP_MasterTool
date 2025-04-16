using System;
using System.Collections.Generic;
using System.Drawing;

namespace TP_MasterTool
{
    public static class Globals
    {
        public enum Funkcje
        {
            MainFrom,
            Initialization,
            FiveMinTimer,
            GetMAC,
            RescanSubNet,
            SaveNote,
            ImportNote,
            PingButton,
            RCMD,
            RemoteRestart,
            Tracert,
            DNSRestore,
            LocalCacheClear,
            ParkedTxMove,
            OpenFolder,
            OpenWebAPC,
            OpenServerRT,
            GetWinLogs,
            DiagnosticPing,
            SavePingToTxt,
            ConstantPing,
            DiscSpaceInfo,
            WinDirStatInstall,
            GetFoldersSize,
            EndpointScan,
            GetSystemBootTime,
            BackupCheck,
            EodCheck, 
            TSEWebserviceRestart,
            UpdatePackageInvalidCheck,
            GetSMART,
            MiniDumpAnalyser,
            ShutdownProcedure,
            StartupProcedure,
            ZipAndSteal,
            JposLogsDownload,
            InvalidTransfer,
            TpReportsRegenAndZip,
            SignatorReset,
            ApcServiceFix,
            BackstoreCsvExport,
            UnhandledException,
            LogMachineAction,
            LogUserAction,
            VersionLog,
            LogFunctionUsage,
            ShowHelp,
            ShowChangeLog,
            SendReport,
            ServiceManager,
            TransactionsXMLToCSV,
            MobilePosKill,
            MonitoringSlayer,
            ChangeLayout,
            ToggleNotePad,
            DeployAndExecute,
            DismAndSFC,
            BulkFileMove,
            EsfClientRestart,
            EsfClientReinit,
            TpProcessManagerRestart,
            JposLogsCheck,
            BackupJobsCheck,
            BackupJobsReset,
            DeleteOldBackupFiles,
            IsBackupDriveAccessible,
            DownloadJavaPosLogs,
            CheckEodAbortedStatus,
            WinCrashReasonCheck,
            CheckForKB,
            GetApcLogs,
            Stocktaking,
            DeleteLock,
            GetSqlInfo,
            GetDhcpScope,
            DhcpMacFind,
            DecryptToString,
            AdhocFunction,
            CDriveClean,
            CucDataCollect,
            GetMiniloggerStatus,
            GetMeMoreWork,
            MoveInvalidUpdatePackages,
            Error,
        }

        public static Color errorColor = Color.FromArgb(255, 150, 150);
        public static Color successColor = Color.LightGreen;

        public static string PRODuserName = "";
        public static string PRODpassword = "";
        public static string TESTuserName = "";
        public static string TESTpassword = "";
        public static string SQLuserName = "";
        public static string SQLpassword = "";

        public static Dictionary<string, string> storeId2Tag = new Dictionary<string, string>();

        public static List<string> advUsers = new List<string> { "mariusz.rzemyk", "mariusz.rzemyk.adm", "agnieszka.parzy.adm", "petre.gogarowski.adm" };
        public static List<string> pmUsers = new List<string> { "o.dovgalova.ext.adm" };
        public static List<string> mineAccounts = new List<string> { "mariusz.rzemyk", "u103583", "mariusz.rzemyk.adm" };

        //--------- PATHS ------------
        public static string toolsPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Tools\";
        public static string userTempLogsPath = @".\Logs\";
        public static string logErrorPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\Errors\";
        public static string configPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Config\";
        public static string reportsFolderPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\Reports\";
        public static string telemetryLogPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\Stats\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MMMM") + @"\";
        public static string machineLogPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\MachineLogs\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MMMM") + @"\";
        public static string userSettingsXmlPath = @".\Config\UserSettings.xml";
        public static string versionLogPath = @"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\VersionMetrics\";
    }
}
