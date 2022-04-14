using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public partial class EndpointsScan : Form
    {
        int progres = 0;
        ConnectionPara connectionPara;

        public EndpointsScan()
        {
            InitializeComponent();
        }

        private void EndpointsScan_Shown(object sender, EventArgs e)
        {
            connectionPara = Main.interfejs.connectionPara;
            Telemetry.LogCompleteTelemetryData(connectionPara.TAG, Globals.Funkcje.EndpointScan, "");
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string store = connectionPara.country + connectionPara.storeNr;
            string storetype = connectionPara.storeType;
            OutputTextBox.Text = "Servers:\n";

            if(scanDevices(store, "TPS", storetype, 1, 2) && scanDevices(store, "SRV", "", 1, 2))
            {
                OutputTextBox.Text += "   none\n";
            }

            OutputTextBox.Text += "\nTills:\n";
            if(scanDevices(store, "STP", storetype, 1, 27) && scanDevices(store, "SCO", storetype, 1, 27))
            {
                OutputTextBox.Text += "   none\n";
            }
            progres = 30;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nBackstore PC:\n");
            if(scanDevices(store, "BPC", storetype, 1, 9))
            {
                OutputTextBox.Text += "   none\n";
            }
            progres = 39;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nPrinters:\n");

            if (!pingTag(store + "PRN49") && scanDevices(store, "PRN", "", 1, 9))
            {
                OutputTextBox.Text += "   none\n";
            }
            progres = 49;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nCounters:\n");
            if(scanDevices(store, "CUC", "", 1, 5))
            {
                OutputTextBox.Text += "   none\n";
            }
            backgroundWorker1.ReportProgress(100);

            OutputTextBox.AppendText("\n>> Scan completed <<");
            this.Refresh();

        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                toolStripProgressBar1.Value = e.ProgressPercentage;
            }
            catch { }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Process encountered error", "Scaner exited during execution with error:" + Environment.NewLine + e.Error.ToString());
            }
            closeButton.Visible = true;
        }

        bool scanDevices(string store, string deviceType, string storetype, int start, int stop)
        {
            string device = deviceType + "0";
            bool empty = true;
            int breaker = 0;
            for (int i = start; i <= stop; i++)
            {
                if (i > 9) { device = deviceType; }
                if (breaker > 2) { break; }
                if(pingTag(store + device + i.ToString() + storetype))
                {
                    breaker = 0;
                    empty = false;
                }
                else
                {
                    breaker++;
                }
                progres++;
                backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));
                this.Refresh();
            }
            return empty;
        }
        private bool pingTag(string tag)
        {
            try
            {
                if (new Ping().Send(tag, 4000, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")).Status == IPStatus.Success)
                {
                    OutputTextBox.AppendText("   - " + tag + " (" + CtrlFunctions.DnsGetIP(tag) + ")\n");
                    System.Threading.Thread.Sleep(100);
                    return true;
                }
            }
            catch { }
            return false;
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
