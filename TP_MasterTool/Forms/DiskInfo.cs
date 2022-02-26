using System;
using System.ComponentModel;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool
{
    public partial class DiskInfo : Form
    {
        readonly ConnectionPara connectionPara = Main.interfejs.connectionPara;

        public DiskInfo()
        {
            InitializeComponent();
        }
        private void closeDiscInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DiskInfo_Shown(object sender, EventArgs e)
        {
            Telemetry.LogFunctionUsage(Globals.Funkcje.DiscSpaceInfo);
            Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.DiscSpaceInfo, "");
            using (BackgroundWorker slave = new BackgroundWorker())
            {
                slave.DoWork += (s, args) =>
                {
                    string[] drives = { "c", "d", "e", "f" };
                    bool error = true;
                    foreach (string letter in drives)
                    {
                        if (System.IO.Directory.Exists(@"\\" + connectionPara.TAG + @"\" + letter + @"$"))
                        {
                            richTextBox1.AppendText("Drive " + letter.ToUpper() + ":\\ Informations:" + "\n" + CtrlFunctions.GetDiskSpaceInfo(letter, connectionPara) + "\n\n");
                            this.Refresh();
                            error = false;
                        }
                    }
                    if (error)
                    {
                        Telemetry.LogOnMachineAction(connectionPara.TAG, Globals.Funkcje.Error, "Disc Connection Error");
                        CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Disc Connection Error", @"Couldn't establish connection to any disc. Please check if target machine is online or initialize it anew and try again.");
                    }
                };
                slave.RunWorkerAsync();
            }
        }
    }
}
