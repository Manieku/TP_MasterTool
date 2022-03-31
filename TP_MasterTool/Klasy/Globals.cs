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
            EndpointScan,
            GetSystemBootTime,
            BackupCheck,
            EodCheck,
            ColonFix,
            TSEWebserviceRestart,
            UpdatePackageInvalid,
            GetSMART,
            MiniDumpAnalyser,
            ShutdownProcedure,
            StartupProcedure,
            ZipAndSteal,
            MassJPOSLogs,
            MassEmergancy,
            InvTansactionTransfer,
            TPReportsRegenZip,
            SignatorReset,
            ApcServiceFix,
            VeritasJobReset,
            BackstoreCsvExport,
            UnhandledException,
            LogOnMachineAction,
            VersionLog,
            CreateTelemetryXml,
            LogFunctionUsage,
            ShowChangeLog,
            SendReport,
            ServiceManager,
            TransactionsXMLToCSV,
            MobilePosKill,
            MonitoringSlayer,
            ChangeLayout,
            ToggleNotePad,
            ADVKbCheck,
            ADVExecute,
            ADVDismSfc,
            AdvRandomness,
            BulkFileMove,
            EsfClientRestart,
            Stocktaking,
            DeleteLock,
            GetSqlInfo,
            DecryptToString,
            Blank,
            Error
        }

        public static Color errorColor = Color.FromArgb(255, 150, 150);
        public static Color successColor = Color.LightGreen;

        public static string PRODuserName = "";
        public static string PRODpassword = "";
        public static string TESTuserName = "";
        public static string TESTpassword = "";
        public static string SQLuserName = "";
        public static string SQLpassword = "";


        public static List<string> advUsers = new List<string> { "mariusz.rzemyk", "mariusz.rzemyk.adm", "a.parzy.ext.adm", "petre.gogarowski.adm", "s.krochmal.adm" };
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
