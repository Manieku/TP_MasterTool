using System;
using System.Diagnostics;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool
{
    public class Logger
    {
        public string hostName;
        public string log = "";
        public string functionName;
        public bool wasError;
        public static class EnvironmentVariables
        {
            public static string activeUser = Environment.UserName;
            public static string executionLocation = Environment.GetCommandLineArgs()[0];
            public static string programVersion = FileVersionInfo.GetVersionInfo(executionLocation).FileVersion;
            public static string AddEnviromentData()
            {
                string data = Environment.NewLine + Environment.NewLine +
                    "============= EnviromentData =============" + Environment.NewLine +
                    "User: " + activeUser + Environment.NewLine +
                    "Tool Version: " + programVersion + Environment.NewLine +
                    "Exetution Path: " + executionLocation + Environment.NewLine +
                    "Psexec in folder: " + System.IO.File.Exists(@".\psexec.exe").ToString() + Environment.NewLine +
                    "Memory Usage: " + (((Process.GetCurrentProcess().WorkingSet64) / 1024) / 1024).ToString() + "MB" + Environment.NewLine +
                    "=====================================";
                return data;
            }
        }
        public static void UnhandledError(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger myLog = new Logger(Globals.Funkcje.UnhandledException, "None", "");
            myLog.log += Environment.NewLine + e.Exception.ToString();
            myLog.SaveLog("CriticalError");
            CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unhandled exception encountered", "Congratuation! You found unhandled error in application. Logs are collected and send to dev team. Application will restart to recover full functionality" + "\n" + "Error: " + e.Exception.Message);
            Application.Restart();
            Environment.Exit(0);
        }
        public Logger(Globals.Funkcje funkcje, string parameters, string host) //constructor
        {
            functionName = funkcje.ToString();
            log = Logger.LogTime() + "Function start: " + funkcje.ToString() + " on host: " + host + Environment.NewLine + "Parameters: " + parameters;
            wasError = false;
        }
        public static void QuickLog(Globals.Funkcje funkcje, string parameters, string host, string logPrefix, string logThis)
        {
            Logger myLog = new Logger(funkcje, parameters, host);
            myLog.Add(logThis);
            myLog.SaveLog(logPrefix);
        }
        public static string Datownik()
        {
            return DateTime.Now.ToString("d/MM/yyyy HH:mm:ss").Replace(":", "-").Replace("/", "-");
        }
        public static string LogTime()
        {
            return "[" + DateTime.Now.ToString("T") + "] ";
        }
        public void Add(string logThis)
        {
            log += Environment.NewLine + Logger.LogTime() + logThis;
        }
        public void SaveLog(string logPrefix)
        {
            log += EnvironmentVariables.AddEnviromentData();
            try
            {
                System.IO.File.WriteAllText(Globals.logErrorPath + logPrefix + "(" + functionName + ") - " + Datownik() + ".txt", log);
            }
            catch { }
        }

        //---------------------------------------
        public static void GeneratePortalReport(string textInputPath, string additionalInfoInputPath, string asciiEmblemPath, string outputPath)
        {
            char[] input = System.IO.File.ReadAllText(textInputPath).Replace(Environment.NewLine, " ").ToCharArray();
            string temp = Spliter(input, 50);
            string output = "".PadRight(52, '-') + Environment.NewLine;
            foreach (string line in temp.Split('\n'))
            {
                output += String.Format("|{0,-50}|", line) + Environment.NewLine;
            }
            output += "".PadRight(52, '-') + Environment.NewLine + Environment.NewLine + System.IO.File.ReadAllText(asciiEmblemPath);

            System.IO.File.WriteAllText(outputPath, output);
            string[] outputLines = System.IO.File.ReadAllLines(outputPath);
            outputLines[0] += "".PadRight(32, '-');
            string[] addInfoLines = System.IO.File.ReadAllLines(additionalInfoInputPath);
            for (int i = 1; i <= addInfoLines.Length; i++)
            {
                outputLines[i] += String.Format("|{0,-30}|", addInfoLines[i - 1]);
            }
            outputLines[addInfoLines.Length + 1] += "".PadRight(32, '-');
            System.IO.File.WriteAllLines(outputPath, outputLines);
        }
        private static string Spliter(char[] input, int index)
        {
            bool breaker = true;
            for (int i = index - 50; i < index; i++)
            {
                if (input[i] == '$')
                {
                    input[i] = '\n';
                    if (i + 50 > input.Length)
                    {
                        return new string(input);
                    }
                    Spliter(input, i + 50);
                    breaker = false;
                }
            }
            if (breaker)
            {
                for (int i = index; i >= index - 50; i--)
                {
                    if (input[i] == ' ')
                    {
                        input[i] = '\n';
                        if (i + 50 > input.Length)
                        {
                            break;
                        }
                        Spliter(input, i + 50);
                        break;
                    }
                }
            }
            return new string(input);
        }
    }
}
