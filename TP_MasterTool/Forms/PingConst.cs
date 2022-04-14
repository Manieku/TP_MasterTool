using System;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public partial class PingConst : Form
    {
        string host = "";
        string filename = "";
        int hit = 0;
        int miss = 0;
        int total = 0;
        Logger myLog;

        public PingConst()
        {
            InitializeComponent();
        }
        private void PingConst_Load(object sender, EventArgs e)
        {
            host = Main.GetTAG();
            myLog = new Logger(Globals.Funkcje.ConstantPing, "None", host);
            this.Text = host;
        }
        private void controlButton_Click(object sender, EventArgs e)
        {
            if (controlButton.Text == "Start")
            {
                startPing();
            }
            else if (controlButton.Text == "Stop")
            {
                stopPing();
            }
        }
        private void startPing()
        {
            Telemetry.LogCompleteTelemetryData(host, Globals.Funkcje.ConstantPing, "Start");
            hit = 0;
            miss = 0;
            total = 0;
            filename = @".\Logs\Ping(" + host + ") - " + Logger.Datownik() + @".txt";

            controlButton.Text = "Stop";
            this.Text = host + " - Pinging";
            richTextBox1.Clear();
            richTextBox1.AppendText("Successful: " + hit + "/" + total + " || Timeouts: " + miss + "/" + total + "\nTimeouts at:\n");
            timer.Start();
            return;
        }
        private void stopPing()
        {
            Telemetry.LogCompleteTelemetryData(host, Globals.Funkcje.ConstantPing, "Stop");
            timer.Stop();
            this.Text = host;
            if (backgroundWorker1.IsBusy)
            {
                controlButton.Text = "Stopping...";
                controlButton.Enabled = false;
            }
            else
            {
                controlButton.Text = "Start";
                this.Text = host;
            }
            if (!FileController.SaveTxtToFile(filename, Environment.NewLine + ">>> Results <<<" + Environment.NewLine + string.Join(Environment.NewLine, richTextBox1.Lines), out Exception saveExp))
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File Save Error", "ToolBox encountered error while trying to save file:" + Environment.NewLine + saveExp.Message);
                myLog.Add(saveExp.ToString());
                myLog.SaveLog("ErrorLog");
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Results", "Full log saved at: " + Path.GetFullPath(filename));
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            total++;
            PingReply pr = new Ping().Send(host, 4000, new byte[2048]);
            if (pr.Status.ToString() == "Success")
            {
                hit++;
                try
                {
                    System.IO.File.AppendAllText(filename, Logger.LogTime() + " Reply from " + pr.Address + " with " + pr.Buffer.Length + " bytes in " + pr.RoundtripTime + " ms" + Environment.NewLine);
                }
                catch { }
            }
            else
            {
                miss++;
                richTextBox1.AppendText(Logger.LogTime() + " Request timed out" + Environment.NewLine);
                try
                {
                    System.IO.File.AppendAllText(filename, Logger.LogTime() + " Request timed out" + Environment.NewLine);
                }
                catch { }
            }
            string[] lines = richTextBox1.Lines;
            lines[0] = "Successful: " + hit + "/" + total + " || Timeouts: " + miss + "/" + total;
            richTextBox1.Lines = lines;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!timer.Enabled)
            {
                controlButton.Text = "Start";
                controlButton.Enabled = true;
            }
        }

    }
}
