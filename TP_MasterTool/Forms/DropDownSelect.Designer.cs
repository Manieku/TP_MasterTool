
namespace TP_MasterTool.Forms
{
    partial class DropDownSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dropDownBox = new System.Windows.Forms.ComboBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dropDownBox
            // 
            this.dropDownBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropDownBox.FormattingEnabled = true;
            this.dropDownBox.Location = new System.Drawing.Point(12, 30);
            this.dropDownBox.Name = "dropDownBox";
            this.dropDownBox.Size = new System.Drawing.Size(612, 24);
            this.dropDownBox.TabIndex = 0;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(641, 30);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(100, 24);
            this.selectButton.TabIndex = 1;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // DropDownSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 83);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.dropDownBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DropDownSelect";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DropDownSelect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox dropDownBox;
        private System.Windows.Forms.Button selectButton;
    }
}