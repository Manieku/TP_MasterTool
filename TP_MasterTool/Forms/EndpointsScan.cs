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
            Telemetry.LogFunctionUsage(Globals.Funkcje.EndpointScan);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.EndpointScan, "");
            backgroundWorker1.RunWorkerAsync();
        }

        void scanDevices(string store, string deviceType, string storetype, int start, int stop, bool canNone)
        {
            string device = deviceType + "0";
            bool empty = true;
            int breaker = 0;
            for (int i = start; i < stop; i++)
            {
                if (i > 9) { device = deviceType; }
                if (breaker > 2) { break; }
                try
                {
                    PingReply p = new Ping().Send(store + device + i.ToString() + storetype, 4000, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
                    if (p.Status == IPStatus.Success)
                    {
                        OutputTextBox.AppendText("   - " + store + device + i.ToString() + storetype + " (" + CtrlFunctions.DnsGetIP(store + device + i.ToString() + storetype) + ")\n");
                        this.Refresh();
                        System.Threading.Thread.Sleep(50);
                        breaker = 0;
                        empty = false;
                    }
                    else
                    {
                        breaker++;
                    }
                }
                catch
                {
                    breaker++;
                }
                progres++;
                backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));
            }
            if (empty && canNone)
            {
                OutputTextBox.AppendText("   none\n");
            }
        }
        private int pingTag(string tag)
        {
            int result = 0;
            try
            {
                if (new Ping().Send(tag, 4000).Status == IPStatus.Success)
                {
                    OutputTextBox.AppendText("   - " + tag + " (" + CtrlFunctions.DnsGetIP(tag) + ")\n");
                    this.Refresh();
                    System.Threading.Thread.Sleep(100);
                    result = 1;
                }
            }
            catch { }
            progres++;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));
            return result;
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string store = connectionPara.country + connectionPara.storeNr;
            string storetype = connectionPara.storeType;
            OutputTextBox.Text = "Servers:\n";
            this.Refresh();

            int checkIfNone = 0;
            checkIfNone += pingTag(store + "TPS01" + storetype);
            checkIfNone += pingTag(store + "TPS02" + storetype);
            checkIfNone += pingTag(store + "SRV01");
            checkIfNone += pingTag(store + "SRV02");
            if (checkIfNone == 0)
            {
                OutputTextBox.Text += "   none\n";
                this.Refresh();
            }

            OutputTextBox.Text += "\nTills:\n";
            this.Refresh();
            scanDevices(store, "STP", storetype, 1, 27, true);
            scanDevices(store, "SCO", storetype, 1, 27, true);
            progres = 30;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nBackstore PC:\n");
            this.Refresh();
            scanDevices(store, "BPC", storetype, 1, 10, true);
            progres = 39;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nPrinters:\n");
            this.Refresh();

            bool empty = true;
            checkIfNone = pingTag(store + "PRN49");
            if (checkIfNone == 1) { empty = false; }
            scanDevices(store, "PRN", "", 1, 10, empty);
            progres = 49;
            backgroundWorker1.ReportProgress((int)Math.Floor(progres * 1.7));

            OutputTextBox.AppendText("\nCounters:\n");
            this.Refresh();
            scanDevices(store, "CUC", "", 1, 10, true);
            backgroundWorker1.ReportProgress(100);

            OutputTextBox.AppendText("\n>> Scan completed <<");
            this.Refresh();

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Process encountered error", "Scaner exited during execution with error:" + Environment.NewLine + e.Error.ToString());
            }
            closeButton.Visible = true;
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                toolStripProgressBar1.Value = e.ProgressPercentage;
            }
            catch { }
        }
    }
}
