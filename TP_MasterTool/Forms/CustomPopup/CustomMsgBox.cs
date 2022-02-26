using System.Windows.Forms;

namespace TP_MasterTool.Forms.CustomMessageBox
{
    public static class CustomMsgBox
    {
        public enum MsgType
        {
            Decision,
            Done,
            Error,
            Info
        }
        public static DialogResult Show(MsgType type, string title, string msg)
        {
            using (CustomMessageBox cmb = new CustomMessageBox(type, title, msg))
            {
                DialogResult result = cmb.ShowDialog();
                if (Main.interfejs.TopMost != true)
                {
                    Main.interfejs.TopMost = true;
                    Main.interfejs.TopMost = false;
                }
                return result;
            }
        }
    }
}
