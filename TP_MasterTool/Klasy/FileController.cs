using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public static class FileController
    {
        //----------FILE----------------
        public static bool CopyFileWithUI(string source, string destination, ref Logger myLog)
        {
            myLog.Add("CopyWithUI: " + source + " - " + destination);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, destination, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            }
            catch (System.IO.FileNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Not Found Exception", "File couldn't be found. Please check if needed file is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to file you try to copy or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (OperationCanceledException)
            {
                myLog.Add("User canceled copying");
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            myLog.Add("Copied successfully");
            return true;
        }
        public static bool CopyFile(string source, string destination, bool nadpis, ref Logger myLog)
        {
            myLog.Add("CopyFile: " + source + " - " + destination + " with override: " + nadpis);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(source, destination, nadpis);
            }
            catch (System.IO.FileNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Not Found Exception", "File couldn't be found. Please check if needed file is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to file you try to copy or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "The destination file already exists", "The destination file already exists. Please check if file: " + destination + "should be there or if function wasn't executed already.");
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            myLog.Add("Copied successfully");
            return true;
        }
        public static bool MoveFileWithUI(string source, string destiniation, ref Logger myLog)
        {
            myLog.Add("MoveWithUI: " + source + " - " + destiniation);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(source, destiniation, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            }
            catch (System.IO.FileNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Not Found Exception", "File couldn't be found. Please check if needed file is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to file you try to move or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "I/O Exception", "The file is in use by another process, or an I/O error occured." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (OperationCanceledException)
            {
                myLog.Add("User canceled copying");
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            return true;
        }
        public static bool MoveFile(string source, string destiniation, bool nadpis, ref Logger myLog)
        {
            myLog.Add("MoveFile: " + source + " - " + destiniation + " with override: " + nadpis);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(source, destiniation, nadpis);
            }
            catch (System.IO.FileNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Not Found Exception", "File couldn't be found. Please check if needed file is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to file you try to move or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "I/O Exception", "The file is in use by another process, or an I/O error occured." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            return true;
        }
        public static string OpenFileDialog(string filter, ref Logger myLog)
        {
            myLog.Add("openTxtDialog: ");
            string filePath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@".\");
                openFileDialog.Filter = filter;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    myLog.log += filePath;
                }
                else
                {
                    myLog.log += "Canceled";
                }
            }
            return filePath;
        }
        public static bool SaveTxtDialog(string text, ref Logger myLog)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@".\");
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
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
                        return false;
                    }
                }
                return FileController.SaveTxtToFile(saveFileDialog.FileName, string.Join(Environment.NewLine, text), ref myLog);
            }
        }
        public static bool SaveTxtToFile(string filePath, string text, ref Logger myLog)
        {
            myLog.Add("saveTxtToFile: " + filePath);
            try
            {
                System.IO.File.AppendAllText(filePath, text);
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path or filename exceed the system-defined maximum length." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.DirectoryNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Directory Not Found Exception", "The specified path is invalid (for example, the directory doesn't exist or it is on an unmapped drive)." + Environment.NewLine + "Please check if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "I/O Exception", "An I/O error occurred while opening the file." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            myLog.Add("Writing to File successful");
            return true;
        }
        //----------FOLDER-------------
        public static bool CopyFolder(string source, string destination, bool nadpis, ref Logger myLog)
        {
            myLog.Add("copyFolder: " + source + " - " + destination + " with override: " + nadpis);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(source, destination, nadpis);
            }
            catch (System.IO.DirectoryNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Directory Not Found Exception", "Directory couldn't be found. Please check if needed directory is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to directory you try to copy or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "The destination file already exists", "The destination file already exists. Please check if file: " + destination + "should be there or if function wasn't executed already.");
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            return true;
        }
        public static bool CopyFolderWithUI(string source, string destination, ref Logger myLog)
        {
            myLog.Add("copyFolderWithUI: " + source + " - " + destination);
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(source, destination, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            }
            catch (System.IO.DirectoryNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Directory Not Found Exception", "Directory couldn't be found. Please check if needed directory is created and if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "Path to directory you try to copy or target path is too long." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.IOException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "The destination file already exists", "The destination file already exists. Please check if file: " + destination + "should be there or if function wasn't executed already.");
                return false;
            }
            catch (OperationCanceledException)
            {
                myLog.Add("User canceled copying");
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            return true;
        }
        public static bool MakeFolder(string path, ref Logger myLog)
        {
            if (System.IO.Directory.Exists(path)) { return true; }
            myLog.Add("Create Folder: " + path);
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (System.IO.PathTooLongException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Path Too Long Exception", "The specified path exceed the system-defined maximum length." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (System.IO.DirectoryNotFoundException exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Directory Not Found Exception", "The specified path is invalid (for example, it is on an unmapped drive)." + Environment.NewLine + "Please check if machine is properly initialized." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            catch (Exception exp)
            {
                myLog.Add(exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Unknown error occurred", "Unfortunately program encountered an unexpected error. Logs are collected and send to dev team." + Environment.NewLine + "Error: " + exp.Message);
                return false;
            }
            myLog.Add("Folder creation successful");
            return true;
        }
        public static bool ClearFolder(string path, bool silent, bool fileOnly, ref Logger myLog)
        {
            bool error = false;
            myLog.Add("Clear Folder(" + silent + " " + fileOnly + "): " + path);
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
            if (!directory.Exists)
            {
                myLog.Add("Folder don't exists");
                if (!silent)
                {
                    CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Folder don't exists", "Folder you're trying to clear don't exists");
                }
                return false;
            }
            if (!fileOnly)
            {
                foreach (System.IO.DirectoryInfo folder in directory.GetDirectories())
                {
                    try
                    {
                        folder.Delete(true);
                    }
                    catch (Exception exp)
                    {
                        myLog.Add("Unable to delete folder: " + exp.Message);
                        error = true;
                    }
                }
            }
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception exp)
                {
                    myLog.Add("Unable to delete file: " + exp.Message);
                    error = true;
                }
            }
            if (error && !silent)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Error encounter during deleting", "Some files/folders couldn't be deleted from:" + Environment.NewLine + path);
            }
            if (error)
            {
                myLog.wasError = true;
                return error;
            }
            myLog.Add("Deleting files successful");
            return error;
        }
        //--------COMPLEX--------------
        public static bool ZipAndStealFolder(string prefix, string remotePath, string absolutePath, ConnectionPara connectionPara)
        {
            bool successState = false;
            Logger myLog = new Logger(Globals.Funkcje.ZipAndSteal, prefix + " " + remotePath + " " + absolutePath, connectionPara.TAG);
            Telemetry.LogFunctionUsage(Globals.Funkcje.ZipAndSteal);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.ZipAndSteal, remotePath);
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    myLog.Add("Background slave started");
                    if (!System.IO.Directory.Exists(remotePath))
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Folder not found");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Folder not found", "Can't find " + prefix + " folder on targeted host");
                        myLog.Add("Can't find " + prefix + " folder on targeted host");
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    string tixnr = Microsoft.VisualBasic.Interaction.InputBox("Provide ticket number:" + Environment.NewLine + "Window will disappear while scritp it's doing his magic in background. You are free to enjoy other Toolbox functions while waiting for result.");
                    if (tixnr == "")
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Wrong ticket number");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Wrong ticket number", "Please provide valid ticket number");
                        return;
                    }
                    string outputFolderName = prefix + " - " + tixnr + "(" + connectionPara.TAG + ") " + Logger.Datownik();

                    CtrlFunctions.CmdOutput cmdOutput = CtrlFunctions.RunHiddenCmd("psexec.exe", @"\\" + connectionPara.TAG + " -u " + connectionPara.userName + " -P " + connectionPara.password + @" cmd /c Xcopy /i /e /h /y """ + absolutePath + @""" ""D:\WNI\4GSS\" + tixnr + @"\Log"" && powershell -command ""Compress-Archive 'D:\WNI\4GSS\" + tixnr + @"\Log' 'D:\WNI\4GSS\" + tixnr + @"\" + outputFolderName + @".zip'"" && rmdir /s /q ""D:\WNI\4GSS\" + tixnr + @"\Log""");
                    if (cmdOutput.exitCode != 0)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "RCMD Encounter Problem");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "RCMD Encounter Problem", "While executing cmd encounter error and exited with code: " + cmdOutput.exitCode.ToString() + Environment.NewLine +
                            "Error message: " + cmdOutput.errorOutputText);
                        myLog.Add("Psexec exited with error: " + cmdOutput.errorOutputText);
                        myLog.SaveLog("ErrorLog");
                        return;
                    }

                    string grabFromPath = @"\\" + connectionPara.TAG + @"\d$\WNI\4GSS\" + tixnr;
                    if (CustomMsgBox.Show(CustomMsgBox.MsgType.Decision, connectionPara.TAG + " - Logs secured", "Log files were successfully zipped and secured in WNI folder." + Environment.NewLine + "Do you want to download then on your drive?") == DialogResult.OK)
                    {
                        if (!System.IO.File.Exists(grabFromPath + @"\" + outputFolderName + @".zip"))
                        {
                            if (!CtrlFunctions.MapEndpointDrive(ref connectionPara, out cmdOutput))
                            {
                                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Unable to map disc second time: " + cmdOutput.errorOutputText);
                                myLog.Add("Unable to map disc second time");
                                myLog.SaveLog("ErrorLog");
                                return;
                            }
                        }
                        if (!FileController.CopyFileWithUI(grabFromPath + @"\" + outputFolderName + @".zip", Globals.userTempLogsPath + outputFolderName + @".zip", ref myLog))
                        {
                            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Copy error");
                            myLog.SaveLog("ErrorLog");
                        }
                    }
                    Process.Start("explorer.exe", grabFromPath);
                    successState = true;
                };
                slave.RunWorkerAsync();

            }
            return successState;
        }
    }
}
