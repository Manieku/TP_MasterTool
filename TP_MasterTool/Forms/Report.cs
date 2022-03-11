using System;
using System.Windows.Forms;
using TP_MasterTool.Forms.CustomMessageBox;

namespace TP_MasterTool.Forms
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (radioButton1.Checked)
            {
                filename = "Report - ";
            }
            else if (radioButton2.Checked)
            {
                filename = "Suggestion - ";
            }

            try
            {
                System.IO.File.AppendAllLines(Globals.reportsFolderPath + filename + Environment.UserName + " - " + Logger.Datownik() + ".txt", richTextBox1.Lines);
            }
            catch (Exception exp)
            {
                Logger.QuickLog(Globals.Funkcje.SendReport, "none", "none", "ErrorLog", "Saving feedback error:" + Environment.NewLine + exp.Message);
                CustomMsgBox.Show(CustomMsgBox.MsgType.Error, "Saving feedback error", "We are very sorry but we encounter a error while saving your report." + Environment.NewLine + exp.Message);
                return;
            }
            CustomMsgBox.Show(CustomMsgBox.MsgType.Done, "Feedback saved", "Thank you for your feedback. Your input is very important to us and we review your report as soon as we can.");
            this.Close();
        }
    }
}
