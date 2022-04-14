using System;
using System.Xml.Linq;

namespace TP_MasterTool.Klasy
{
    class Telemetry
    {
        public static void CreateFunctionUsageXml(ref Logger myLog)
        {
            myLog.Add("CreateFunctionUsageXml");
            XDocument telemetryXml = new XDocument();
            telemetryXml.Add(new XElement("FunctionStats"));
            try
            {
                telemetryXml.Save(Globals.telemetryLogPath + @"FunctionStats.xml");
            }
            catch (Exception exp)
            {
                myLog.wasError = true;
                myLog.Add("Error during saving xml file");
                myLog.Add(exp.ToString());
            }
        }
        public static void LogMachineAction(string host, Globals.Funkcje funkcja, string comments)
        {
            string filePath = Globals.machineLogPath + host + ".csv";
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.AppendAllText(filePath, @"TimeStamp,User,Function,Comments" + Environment.NewLine);
                }
                System.IO.File.AppendAllText(filePath, string.Join(",", DateTime.Now.ToString("G"), Logger.EnvironmentVariables.activeUser, funkcja, comments) + Environment.NewLine);
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.LogMachineAction, funkcja.ToString(), host, "TelemetryError", exp.ToString());
            }

        }
        public static void LogUserAction(string host, Globals.Funkcje funkcja, string comments)
        {
            string filePath = Globals.telemetryLogPath + Logger.EnvironmentVariables.activeUser + ".csv";
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.AppendAllText(filePath, @"TimeStamp,Host,Function,Comments" + Environment.NewLine);
                }
                System.IO.File.AppendAllText(filePath, string.Join(",", DateTime.Now.ToString("G"), host, funkcja, comments) + Environment.NewLine);
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.LogUserAction, funkcja.ToString(), host, "TelemetryError", exp.ToString());
            }

        }
        public static void LogFunctionUsage(Globals.Funkcje funkcja)
        {
            if (!System.IO.File.Exists(Globals.telemetryLogPath + @"FunctionStats.xml"))
            {
                Logger myLog = new Logger(funkcja, "No FunctionStats.xml Found", "");
                CreateFunctionUsageXml(ref myLog);
            }
            try
            {
                XDocument telemetryXml = XDocument.Load(Globals.telemetryLogPath + @"FunctionStats.xml");
                XElement node = telemetryXml.Root.Element(funkcja.ToString());
                if (node == null)
                {
                    telemetryXml.Root.Add(new XElement(funkcja.ToString(), "1"));
                    telemetryXml.Save(Globals.telemetryLogPath + @"FunctionStats.xml");
                    return;
                }
                int count = int.Parse(node.Value);
                count++;
                node.Value = count.ToString();
                telemetryXml.Save(Globals.telemetryLogPath + @"FunctionStats.xml");
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.LogFunctionUsage, funkcja.ToString(), "", "TelemetryError", exp.ToString());
            }
        }
        public static void LogCompleteTelemetryData(string host, Globals.Funkcje funkcja, string comment)
        {
            LogMachineAction(host, funkcja, comment);
            LogUserAction(host, funkcja, comment);
            LogFunctionUsage(funkcja);
        }
    }
}
