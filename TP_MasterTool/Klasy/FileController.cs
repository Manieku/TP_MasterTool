using System;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public static class FileController
    {
        //----------FILE----------------
        public static bool CopyFile(string source, string destination, bool uiVisible, out Exception exp)
        {
            exp = null;
            try
            {
                if(uiVisible)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, destination, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                else
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, destination, true);
                }
            }
            catch(Exception tempExp)
            {
                if (tempExp.GetType().ToString() != "System.OperationCanceledException")
                {
                    exp = tempExp;
                }
                return false;
            }
            return true;
        }
        public static bool MoveFile(string source, string destination, bool uiVisible, out Exception exp)
        {
            exp = null;
            try
            {
                if (uiVisible)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(source, destination, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                else
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(source, destination, true);
                }
            }
            catch (Exception tempExp)
            {
                if (tempExp.GetType().ToString() != "System.OperationCanceledException")
                {
                    exp = tempExp;
                }
                return false;
            }
            return true;
        }
        public static string OpenFileDialog(string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@".\");
                openFileDialog.Filter = filter;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return "";
        }
        public static string OpenFolderBrowserDialog(string description)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = description;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.SelectedPath = System.IO.Path.GetFullPath(@".\Logs");

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    return folderBrowserDialog.SelectedPath;
                }
            }
            return "";

        }
        public static void SaveTxtDialog(string text, ref Logger myLog)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@".\");
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                if (System.IO.File.Exists(saveFileDialog.FileName))
                {
                    try
                    {
                        System.IO.File.Delete(saveFileDialog.FileName);
                    }
                    catch (Exception exp)
                    {
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Override Error", "Unable to override selected file:" + Environment.NewLine + exp.Message);
                        return;
                    }
                }
                if(!FileController.SaveTxtToFile(saveFileDialog.FileName, text, out Exception saveExp))
                {
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file:" + Environment.NewLine + saveExp.Message);
                }
            }
        }
        public static bool SaveTxtToFile(string filePath, string text, out Exception saveExp)
        {
            saveExp = null;
            try
            {
                System.IO.File.WriteAllText(filePath, text);
            }
            catch (Exception exp)
            {
                saveExp = exp;
                return false;
            }
            return true;
        }

        //----------FOLDER-------------
        public static bool CopyFolder(string source, string destination, bool uiVisible, out Exception exp)
        {
            exp = null;
            try
            {
                if (uiVisible)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(source, destination, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                else
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(source, destination, true);
                }
            }
            catch (Exception tempExp)
            {
                if (tempExp.GetType().ToString() != "System.OperationCanceledException")
                {
                    exp = tempExp;
                }
                return false;
            }
            return true;
        }
        public static bool MakeFolder(string path, out Exception makeExp)
        {
            makeExp = null;
            if (System.IO.Directory.Exists(path))
            {
                return true;
            }
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception tempExp)
            {
                makeExp = tempExp;
                return false;
            }
            return true;
        }
        public static bool ClearFolder(string path, bool fileOnly, out string errorList)
        {
            errorList = "";
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
            if (!directory.Exists)
            {
                errorList += "Selected folder doesn't exist";
                return false;
            }
            if (!fileOnly)
            {
                foreach (System.IO.DirectoryInfo folder in directory.EnumerateDirectories())
                {
                    try
                    {
                        folder.Delete(true);
                    }
                    catch (Exception exp)
                    {
                        errorList += "Unable to delete folder: " + exp.Message + Environment.NewLine;
                    }
                }
            }
            foreach (System.IO.FileInfo file in directory.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception exp)
                {
                    errorList += "Unable to delete file: " + exp.Message + Environment.NewLine;
                }
            }
            if(errorList != "")
            {
                return false;
            }
            return true;
        }
        //--------COMPLEX--------------
        public static bool ZipAndStealFolder(string tixnr, string prefix, string remotePath, string absolutePath, ConnectionPara connectionPara, out string outputFilePath)
        {
            outputFilePath = "";
            Logger myLog = new Logger(Globals.Funkcje.ZipAndSteal, prefix + " " + remotePath + " " + absolutePath, connectionPara.TAG);
            if (!System.IO.Directory.Exists(remotePath))
            {
                outputFilePath = "Can't find " + prefix + " folder on targeted host";
                return false;
            }

            string outputFolderName = prefix + " - " + tixnr + "(" + connectionPara.TAG + ") " + Logger.Datownik();

            CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Xcopy /i /e /h /y """ + absolutePath + @""" ""D:\WNI\4GSS\" + tixnr + @"\Log"" && powershell -command ""Compress-Archive 'D:\WNI\4GSS\" + tixnr + @"\Log' 'D:\WNI\4GSS\" + tixnr + @"\" + outputFolderName + @".zip'"" && rmdir /s /q ""D:\WNI\4GSS\" + tixnr + @"\Log""");
            if (cmdOutput.exitCode != 0)
            {
                outputFilePath = "While executing cmd encounter error and exited with code: " + cmdOutput.exitCode.ToString() + Environment.NewLine + "Error message: " + cmdOutput.errorOutputText;
                myLog.Add("Psexec exited with error: " + cmdOutput.errorOutputText);
                myLog.SaveLog("ErrorLog");
                return false;
            }

            outputFilePath = @"\\" + connectionPara.TAG + @"\d$\WNI\4GSS\" + tixnr + @"\" + outputFolderName + @".zip";
            Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.ZipAndSteal, prefix + " | " + tixnr);
            return true;
        }
    }
}
