using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;
using TP_MasterTool.Klasy;

namespace TP_MasterTool.Forms
{
    public partial class ChangeLog : Form
    {
        string[] changelog;
        readonly List<int> index = new List<int>();
        public ChangeLog()
        {
            InitializeComponent();
        }

        private void ChangeLog_Load(object sender, EventArgs e)
        {
            Telemetry.LogUserAction("ChangeLog", Globals.Funkcje.ShowChangeLog, "");
            try
            {
                changelog = System.IO.File.ReadAllLines(Globals.configPath + "ChangeLog.txt");
                for (int i = 0; i < changelog.Length; i++)
                {
                    if (changelog[i].StartsWith(">>>"))
                    {
                        comboBox1.Items.Add(changelog[i].Substring(4, changelog[i].Length - 8) + " - " + changelog[i - 1]);
                        index.Add(i);
                    }
                }
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.ShowChangeLog, "none", "none", "ErrorLog", "Encounter error while trying to read change log file from server:" + Environment.NewLine + exp.ToString());
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "File read error", "Encounter error while trying to read change log file from server" + Environment.NewLine + exp.Message);
                this.Close();
                return;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayLog(comboBox1.SelectedIndex);
        }

        private void DisplayLog(int logindex)
        {
            richTextBox1.Clear();
            int start = index[logindex] + 1;
            int stop = changelog.Length;
            if (logindex + 1 != index.Count)
            {
                stop = index[logindex + 1] - 2;
            }
            for (int i = start; i < stop; i++)
            {
                richTextBox1.AppendText(changelog[i] + Environment.NewLine);
            }
        }
    }
}
