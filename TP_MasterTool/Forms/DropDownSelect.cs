using System;
using System.Windows.Forms;

namespace TP_MasterTool.Forms
{
    public partial class DropDownSelect : Form
    {
        public string ReturnValue1 = null;
        public DropDownSelect(string title, string[] values)
        {
            InitializeComponent();
            this.Text = title;
            dropDownBox.Items.AddRange(values);
            dropDownBox.SelectedIndex = 0;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            this.ReturnValue1 = dropDownBox.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
