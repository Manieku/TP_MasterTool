using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace TP_MasterTool.Forms.CustomMessageBox
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(CustomMsgBox.MsgType msgType, string title, string msg)
        {
            InitializeComponent();
            this.Text = title;
            this.Owner = Main.interfejs;
            label1.Text = msg;
            switch (msgType)
            {
                case CustomMsgBox.MsgType.Decision:
                    {
                        pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("decision__" + new Random().Next(1, 3) + "_");
                        break;
                    }
                case CustomMsgBox.MsgType.Done:
                    {
                        Cancel_Button.Visible = false;
                        pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("done__" + new Random().Next(1, 7) + "_");
                        break;
                    }
                case CustomMsgBox.MsgType.Error:
                    {
                        Cancel_Button.Visible = false;
                        pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("error__" + new Random().Next(1, 8) + "_");
                        SystemSounds.Exclamation.Play();
                        break;
                    }
                case CustomMsgBox.MsgType.Info:
                    {
                        Cancel_Button.Visible = false;
                        pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("info__" + new Random().Next(1, 7) + "_");
                        break;
                    }
            }
            this.Size = new Size(this.Size.Width + 15, this.Size.Height + 40);
        }
        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            pictureBox1.Image.Dispose();
            pictureBox1.Dispose();
            OK_Button.Dispose();
            Cancel_Button.Dispose();
            label1.Dispose();
            this.Dispose();
            this.Close();
        }
        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            pictureBox1.Image.Dispose();
            pictureBox1.Dispose();
            OK_Button.Dispose();
            Cancel_Button.Dispose();
            label1.Dispose();
            this.Dispose();
            this.Close();
        }
    }
}
