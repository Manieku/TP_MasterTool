using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TP_Updater
{
    public partial class Updater : Form
    {
        bool updateDone = false;
        public Updater()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            LogTextbox.Text = "Checking program version.\n";
            this.Refresh();
            System.Threading.Thread.Sleep(750);
            try
            {
                CurVersionNrLabel.Text = FileVersionInfo.GetVersionInfo(@".\TP_MasterTool.exe").FileVersion;
                LogTextbox.AppendText(" >> " + CurVersionNrLabel.Text + "\n");
            }
            catch
            {
                CurVersionNrLabel.Text = "Can't access version data.";
                CurVersionNrLabel.ForeColor = Color.Red;
                LogTextbox.Text += "[ERROR] Can't read current program version.\n";
                this.Refresh();
                System.Threading.Thread.Sleep(750);

            }

            LogTextbox.Text += "Checking latest available version.\n";
            this.Refresh();
            System.Threading.Thread.Sleep(750);

            try
            {
                NewVersionNrLabel.Text = FileVersionInfo.GetVersionInfo(@"D:\TP_MasterTool\TP_MasterTool.exe").FileVersion;
                LogTextbox.AppendText(" >> " + NewVersionNrLabel.Text + "\n");
            }
            catch
            {
                NewVersionNrLabel.Text = "Can't access master version info.";
                NewVersionNrLabel.ForeColor = Color.Red;
                LogTextbox.Text += "[ERROR] Unable to access congifuration file with version data on network drive.";
                errorLog(LogTextbox.Lines);
                closeButton.Visible = true;
                return;
            }

            Version currVersion;
            try
            {
                currVersion = new Version(CurVersionNrLabel.Text);
            }
            catch
            {
                currVersion = new Version("0.0.0.0");
            }
            Version lateVersion = new Version(NewVersionNrLabel.Text);

            if (currVersion.CompareTo(lateVersion) < 0)
            {
                CurVersionNrLabel.ForeColor = Color.Red;
                LogTextbox.Text += "Newer version is available.\nUpdating...\n";
                this.Refresh();
                System.Threading.Thread.Sleep(750);

                try
                {
                    System.IO.File.Delete(@".\TP_MasterTool.exe");
                }
                catch { }
                int breaker = 0;
                while (System.IO.File.Exists(@".\TP_MasterTool.exe"))
                {
                    System.Threading.Thread.Sleep(1);
                    breaker++;
                    if(breaker > 15000)
                    {
                        break;
                    }
                }
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(@"D:\TP_MasterTool\TP_MasterTool.exe", @".\TP_MasterTool.exe", true);
                }
                catch(Exception exp)
                {
                    LogTextbox.Text += "[ERROR] Updater encounter problem during updating to new version with error:\n" + exp.Message + "\n";
                    errorLog(LogTextbox.Lines);
                    closeButton.Visible = true;
                    return;
                }

                CurVersionNrLabel.Text = FileVersionInfo.GetVersionInfo(Environment.CurrentDirectory + @"\TP_MasterTool.exe").FileVersion;
                CurVersionNrLabel.ForeColor = Color.Green;
                LogTextbox.Text += "Update completed yay\n\n >> Please get familiar with the newest changes in About -> ChangeLog <<\n";
                updateDone = true;
                closeButton.Visible = true;
            }
            else if(currVersion.CompareTo(lateVersion) >= 0)
            {
                CurVersionNrLabel.ForeColor = Color.Green;
                LogTextbox.Text += "You already have newer version.";
                updateDone = true;
                closeButton.Visible = true;
            }

        }
        void errorLog(string[] log)
        {
            string filename = "UpdateErrorLog - " + Environment.UserName + " - " + DateTime.Now.ToString("d/MM/yyyy HH:mm:ss").Replace(":", "-").Replace("/", "-") + @".txt";
            try
            {
                System.IO.File.WriteAllLines(@"D:\C&A BLF\Rzemyk Mariusz\ToolBox Files\Telemetry\Errors\" + filename, log);
            }
            catch (Exception)
            {

            }

        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            if(updateDone)
            {
                LogTextbox.Text += "Starting Toolbox...\n";
                this.Refresh();
                try
                {
                    Process.Start(@".\TP_MasterTool.exe");
                }
                catch (Exception exp)
                {
                    LogTextbox.Text += "[ERROR] " + exp.Message + "\n";
                    errorLog(LogTextbox.Lines);
                }
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
