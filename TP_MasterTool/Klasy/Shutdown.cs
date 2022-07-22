namespace TP_MasterTool.Klasy
{
    static class Shutdown
    {
        public static void ShutdownProcedure()
        {
            Logger myLog = new Logger(Globals.Funkcje.ShutdownProcedure, "none", "none");
            myLog.Add("Saving user settings");
            Main.interfejs.userSettings.SaveUserSettingsToXml();
            if (myLog.wasError)
            {
                myLog.SaveLog("ErrorLog");
            }
        }
    }
}