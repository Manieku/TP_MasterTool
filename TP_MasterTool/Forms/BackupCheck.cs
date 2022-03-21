using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    public partial class BackupCheck : Form
    {
        readonly ConnectionPara connectionPara = Main.interfejs.connectionPara;
        Logger myLog;
        readonly string spaceLabelDefault = @"F:\ Drive: ";

        public BackupCheck()
        {
            InitializeComponent();
        }

        private void BackupCheck_Shown(object sender, EventArgs e)
        {
            CheckBackups();
        }
        private void CheckBackups()
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.BackupCheck);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.BackupCheck, "");
            myLog = new Logger(Globals.Funkcje.BackupCheck, "None", connectionPara.TAG);
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    rescanButton.Enabled = false;
                    saveButton.Enabled = false;
                    GetDiskSpaceInfo();
                    GetBackupFiles(ref cBackupFilesDataGrid, connectionPara.TAG + "_C*.v2i");
                    GetBackupFiles(ref dBackupFilesDataGrid, connectionPara.TAG + "_D*.v2i");
                    cFilesStatusLabel.Text += AnalizeFiles(ref cBackupFilesDataGrid, ref cSummaryLabel);
                    dFilesStatusLabel.Text += AnalizeFiles(ref dBackupFilesDataGrid, ref dSummaryLabel);
                    try
                    {
                        VeritasLogTextBox.Lines = File.ReadAllLines(@"\\" + connectionPara.TAG + @"\c$\ProgramData\Veritas\VERITAS SYSTEM RECOVERY\LOGS\Veritas System Recovery.log.txt");
                    }
                    catch (Exception exp)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Error reading log file");
                        VeritasLogTextBox.Text = "Error reading log file" + Environment.NewLine + exp.Message;
                    }
                    rescanButton.Enabled = true;
                    saveButton.Enabled = true;
                    if (myLog.wasError)
                    {
                        myLog.SaveLog("ErrorLog");
                    }
                };
                slave.RunWorkerAsync();
            }
        }
        private void GetDiskSpaceInfo()
        {
            myLog.Add("Reading disc space info");
            driveSpaceLabel.Text = spaceLabelDefault + CtrlFunctions.GetDiskSpaceInfo("F", connectionPara, out ulong freeBytes);
            if ((freeBytes / (1024 * 1024)) < 102400)
            {
                statusLabel.Text = "Status: [ERROR] Low disc space - please check if there aren't duplicated backup files.";
            }
            else if ((freeBytes / (1024 * 1024)) < 321536)
            {
                statusLabel.Text = "Status: [CAUTION] Disc usage is beyond typical norm - please check if there aren't any unnesesary files or old backups.";
            }
            else
            {
                statusLabel.Text = "Status: OK";
            }
            myLog.Add("Read successfully: " + (freeBytes / (1024 * 1024)).ToString());
        }
        private void GetBackupFiles(ref DataGridView dataGridView, string filter)
        {
            myLog.Add("Looking for backup files: " + filter);
            try
            {
                string[] files = Directory.GetFiles(@"\\" + connectionPara.TAG + @"\f$\Backup\TPBackup", filter);
                if (files == null)
                {
                    myLog.Add("No files found");
                    myLog.wasError = true;
                    dataGridView.Rows.Add("No files found", "");
                }
                foreach (string file in files)
                {
                    dataGridView.Rows.Add(Path.GetFileName(file), File.GetCreationTime(file).ToString());
                }
            }
            catch (Exception exp)
            {
                Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Error reading files: " + filter);
                myLog.Add("Error reading files");
                myLog.Add(exp.ToString());
                myLog.wasError = true;
                dataGridView.Rows.Add("Error reading files", "");
            }
        }
        private string AnalizeFiles(ref DataGridView dataGrid, ref Label summaryLabel)
        {
            string output = "";
            bool fileCountError = false;
            bool uptodateError = false;
            bool dupicatesError = false;

            if (dataGrid.Rows.Count == 2)
            {
                output += "- Only 2 backup files: OK" + Environment.NewLine;
            }
            else
            {
                output += "- Only 2 backup files: FAILED" + Environment.NewLine;
                fileCountError = true;
            }

            bool today = false;
            bool yesterday = false;
            List<int> dates = new List<int>();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                dates.Add(DateTime.Parse(row.Cells[1].Value.ToString()).Day);
                if (DateTime.Parse(row.Cells[1].Value.ToString()).Day == DateTime.Today.Day)
                {
                    today = true;
                }
                if (DateTime.Parse(row.Cells[1].Value.ToString()).Day == DateTime.Today.AddDays(-1).Day)
                {
                    yesterday = true;
                }
            }
            if (today && yesterday)
            {
                output += "- Up to date: OK" + Environment.NewLine;
            }
            else
            {
                output += "- Up to date: FAILED" + Environment.NewLine;
                uptodateError = true;
            }

            if (dates.Count != dates.Distinct().Count())
            {
                output += "- No Duplicates: FAILED" + Environment.NewLine;
                dupicatesError = true;
            }
            else
            {
                output += "- No Duplicates: OK" + Environment.NewLine;
            }

            summaryLabel.Text += Summary(fileCountError, uptodateError, dupicatesError);
            return output;
        }
        private string Summary(bool fileCountError, bool uptodateError, bool duplicatesError)
        {
            string output = " ";
            if (!fileCountError && !uptodateError && !duplicatesError)
            {
                return " Everything looks ok";
            }
            if (uptodateError)
            {
                output += " Veritas didn't preform recent backup - check in log and preform disc check.";
            }
            else
            {
                output += " Veritas preform backup on daily basis.";
            }

            if (fileCountError && !duplicatesError)
            {
                output += " There are more than 2 backup files but no duplicates - check if backup job have max backups set to 2 or if there isnt any old files.";
            }
            else if (duplicatesError)
            {
                output += "There are more than one backup file per day - check if there aren't duplicated jobs in console";
            }

            return output;
        }
        private void RescanButton_Click(object sender, EventArgs e)
        {
            driveSpaceLabel.Text = spaceLabelDefault + "Calculating...";
            statusLabel.Text = "Status: ";
            cBackupFilesDataGrid.Rows.Clear();
            dBackupFilesDataGrid.Rows.Clear();
            cSummaryLabel.Text = "Summary:";
            dSummaryLabel.Text = "Summary:";
            cFilesStatusLabel.Text = "Status:" + Environment.NewLine;
            dFilesStatusLabel.Text = "Status:" + Environment.NewLine;
            VeritasLogTextBox.Text = "";
            CheckBackups();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            myLog.Add("Save raport");
            string output = @"C:\ Backup Files:" + Environment.NewLine;
            foreach (DataGridViewRow row in cBackupFilesDataGrid.Rows)
            {
                output += " - " + row.Cells[0].Value.ToString() + " (" + row.Cells[1].Value.ToString() + ")" + Environment.NewLine;
            }
            output += cFilesStatusLabel.Text + cSummaryLabel.Text + Environment.NewLine;

            output += Environment.NewLine + @"D:\ Backup Files:" + Environment.NewLine;
            foreach (DataGridViewRow row in dBackupFilesDataGrid.Rows)
            {
                output += " - " + row.Cells[0].Value.ToString() + " (" + row.Cells[1].Value.ToString() + ")" + Environment.NewLine;
            }
            output += dFilesStatusLabel.Text + dSummaryLabel.Text + Environment.NewLine;

            string fileName = "BackupCheck(" + connectionPara.TAG + ") - " + Logger.Datownik() + ".txt";
            if (!FileController.SaveTxtToFile(@".\Logs\" + fileName, output, out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file:" + Environment.NewLine + saveExp.Message);
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Raport Saved", "Raport was successfully saved in Logs folder as: " + fileName);
        }
    }
}
