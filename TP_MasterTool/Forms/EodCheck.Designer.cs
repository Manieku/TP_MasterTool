
namespace TP_MasterTool.Forms
{
    partial class EodCheck
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
            this.fileNameComboBox = new System.Windows.Forms.ComboBox();
            this.CheckButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ResultTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileNameComboBox
            // 
            this.fileNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileNameComboBox.FormattingEnabled = true;
            this.fileNameComboBox.Location = new System.Drawing.Point(13, 13);
            this.fileNameComboBox.MaxDropDownItems = 16;
            this.fileNameComboBox.Name = "fileNameComboBox";
            this.fileNameComboBox.Size = new System.Drawing.Size(566, 24);
            this.fileNameComboBox.TabIndex = 0;
            // 
            // CheckButton
            // 
            this.CheckButton.Enabled = false;
            this.CheckButton.Location = new System.Drawing.Point(585, 13);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(82, 24);
            this.CheckButton.TabIndex = 1;
            this.CheckButton.Text = "Check";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ResultTextBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1150, 403);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultTextBox.Location = new System.Drawing.Point(3, 18);
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.ReadOnly = true;
            this.ResultTextBox.Size = new System.Drawing.Size(1144, 382);
            this.ResultTextBox.TabIndex = 0;
            this.ResultTextBox.Text = "";
            this.ResultTextBox.WordWrap = false;
            // 
            // EodCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CheckButton);
            this.Controls.Add(this.fileNameComboBox);
            this.MinimumSize = new System.Drawing.Size(818, 497);
            this.Name = "EodCheck";
            this.Text = "EodCheck";
            this.Shown += new System.EventHandler(this.EodCheck_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox fileNameComboBox;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox ResultTextBox;
    }
}