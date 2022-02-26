namespace TP_MasterTool.Klasy
{
    static class Shutdown
    {
        public static bool DeleteWindowsLogs(ref Logger myLog)
        {
            myLog.Add("Deleting windows logs");
            return FileController.ClearFolder(@".\Logs\Windows", true, false, ref myLog);
        }
        public static void ShutdownProcedure()
        {
            Logger myLog = new Logger(Globals.Funkcje.ShutdownProcedure, "none", "none");
            myLog.Add("Saving user settings");
            Main.interfejs.userSettings.SaveUserSettingsToXml();
            DeleteWindowsLogs(ref myLog);
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
        }
    }
}