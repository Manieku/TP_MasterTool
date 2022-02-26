
namespace TP_MasterTool.Forms
{
    partial class KBinstalation
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
            this.SingleRadioButton = new System.Windows.Forms.RadioButton();
            this.MassRadioButton = new System.Windows.Forms.RadioButton();
            this.textBox = new System.Windows.Forms.TextBox();
            this.FetchTxtButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TAG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.kbCheckRadio = new System.Windows.Forms.RadioButton();
            this.copyExecuteRadio = new System.Windows.Forms.RadioButton();
            this.executeRadio = new System.Windows.Forms.RadioButton();
            this.dismSfcRadio = new System.Windows.Forms.RadioButton();
            this.KBCheckTextBox = new System.Windows.Forms.TextBox();
            this.copyExecuteTextBox = new System.Windows.Forms.TextBox();
            this.executeTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.waitCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SingleRadioButton
            // 
            this.SingleRadioButton.AutoSize = true;
            this.SingleRadioButton.Checked = true;
            this.SingleRadioButton.Location = new System.Drawing.Point(34, 5);
            this.SingleRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SingleRadioButton.Name = "SingleRadioButton";
            this.SingleRadioButton.Size = new System.Drawing.Size(72, 21);
            this.SingleRadioButton.TabIndex = 0;
            this.SingleRadioButton.TabStop = true;
            this.SingleRadioButton.Text = "Single:";
            this.SingleRadioButton.UseVisualStyleBackColor = true;
            this.SingleRadioButton.CheckedChanged += new System.EventHandler(this.SingleRadioButton_CheckedChanged);
            // 
            // MassRadioButton
            // 
            this.MassRadioButton.AutoSize = true;
            this.MassRadioButton.Location = new System.Drawing.Point(154, 4);
            this.MassRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MassRadioButton.Name = "MassRadioButton";
            this.MassRadioButton.Size = new System.Drawing.Size(121, 21);
            this.MassRadioButton.TabIndex = 1;
            this.MassRadioButton.TabStop = true;
            this.MassRadioButton.Text = "Mass From Txt";
            this.MassRadioButton.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(17, 43);
            this.textBox.Margin = new System.Windows.Forms.Padding(4);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(308, 22);
            this.textBox.TabIndex = 2;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // FetchTxtButton
            // 
            this.FetchTxtButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchTxtButton.Enabled = false;
            this.FetchTxtButton.Location = new System.Drawing.Point(327, 43);
            this.FetchTxtButton.Margin = new System.Windows.Forms.Padding(4);
            this.FetchTxtButton.Name = "FetchTxtButton";
            this.FetchTxtButton.Size = new System.Drawing.Size(39, 25);
            this.FetchTxtButton.TabIndex = 3;
            this.FetchTxtButton.Text = "...";
            this.FetchTxtButton.UseVisualStyleBackColor = true;
            this.FetchTxtButton.Click += new System.EventHandler(this.FetchTxtButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TAG,
            this.Status});
            this.dataGridView1.Location = new System.Drawing.Point(17, 192);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(344, 277);
            this.dataGridView1.TabIndex = 4;
            // 
            // TAG
            // 
            this.TAG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TAG.FillWeight = 50F;
            this.TAG.HeaderText = "TAG";
            this.TAG.MinimumWidth = 6;
            this.TAG.Name = "TAG";
            this.TAG.ReadOnly = true;
            this.TAG.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.FillWeight = 50F;
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // StartStopButton
            // 
            this.StartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStopButton.Location = new System.Drawing.Point(261, 476);
            this.StartStopButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(100, 28);
            this.StartStopButton.TabIndex = 5;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // kbCheckRadio
            // 
            this.kbCheckRadio.AutoSize = true;
            this.kbCheckRadio.Location = new System.Drawing.Point(17, 72);
            this.kbCheckRadio.Name = "kbCheckRadio";
            this.kbCheckRadio.Size = new System.Drawing.Size(94, 21);
            this.kbCheckRadio.TabIndex = 6;
            this.kbCheckRadio.Text = "KB Check:";
            this.kbCheckRadio.UseVisualStyleBackColor = true;
            // 
            // copyExecuteRadio
            // 
            this.copyExecuteRadio.AutoSize = true;
            this.copyExecuteRadio.Location = new System.Drawing.Point(17, 100);
            this.copyExecuteRadio.Name = "copyExecuteRadio";
            this.copyExecuteRadio.Size = new System.Drawing.Size(115, 21);
            this.copyExecuteRadio.TabIndex = 7;
            this.copyExecuteRadio.Text = "CopyExecute:";
            this.copyExecuteRadio.UseVisualStyleBackColor = true;
            // 
            // executeRadio
            // 
            this.executeRadio.AutoSize = true;
            this.executeRadio.Location = new System.Drawing.Point(17, 128);
            this.executeRadio.Name = "executeRadio";
            this.executeRadio.Size = new System.Drawing.Size(83, 21);
            this.executeRadio.TabIndex = 8;
            this.executeRadio.Text = "Execute:";
            this.executeRadio.UseVisualStyleBackColor = true;
            // 
            // dismSfcRadio
            // 
            this.dismSfcRadio.AutoSize = true;
            this.dismSfcRadio.Location = new System.Drawing.Point(17, 156);
            this.dismSfcRadio.Name = "dismSfcRadio";
            this.dismSfcRadio.Size = new System.Drawing.Size(119, 21);
            this.dismSfcRadio.TabIndex = 9;
            this.dismSfcRadio.Text = "Dism And SFC";
            this.dismSfcRadio.UseVisualStyleBackColor = true;
            // 
            // KBCheckTextBox
            // 
            this.KBCheckTextBox.Location = new System.Drawing.Point(139, 72);
            this.KBCheckTextBox.Name = "KBCheckTextBox";
            this.KBCheckTextBox.Size = new System.Drawing.Size(186, 22);
            this.KBCheckTextBox.TabIndex = 10;
            // 
            // copyExecuteTextBox
            // 
            this.copyExecuteTextBox.Location = new System.Drawing.Point(139, 100);
            this.copyExecuteTextBox.Name = "copyExecuteTextBox";
            this.copyExecuteTextBox.Size = new System.Drawing.Size(186, 22);
            this.copyExecuteTextBox.TabIndex = 11;
            // 
            // executeTextBox
            // 
            this.executeTextBox.Location = new System.Drawing.Point(139, 128);
            this.executeTextBox.Name = "executeTextBox";
            this.executeTextBox.Size = new System.Drawing.Size(186, 22);
            this.executeTextBox.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MassRadioButton);
            this.panel1.Controls.Add(this.SingleRadioButton);
            this.panel1.Location = new System.Drawing.Point(30, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 33);
            this.panel1.TabIndex = 13;
            // 
            // waitCheckBox
            // 
            this.waitCheckBox.AutoSize = true;
            this.waitCheckBox.Location = new System.Drawing.Point(237, 156);
            this.waitCheckBox.Name = "waitCheckBox";
            this.waitCheckBox.Size = new System.Drawing.Size(96, 21);
            this.waitCheckBox.TabIndex = 14;
            this.waitCheckBox.Text = "Wait4Exit?";
            this.waitCheckBox.UseVisualStyleBackColor = true;
            // 
            // KBinstalation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 517);
            this.Controls.Add(this.waitCheckBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.executeTextBox);
            this.Controls.Add(this.copyExecuteTextBox);
            this.Controls.Add(this.KBCheckTextBox);
            this.Controls.Add(this.dismSfcRadio);
            this.Controls.Add(this.executeRadio);
            this.Controls.Add(this.copyExecuteRadio);
            this.Controls.Add(this.kbCheckRadio);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FetchTxtButton);
            this.Controls.Add(this.textBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(395, 564);
            this.Name = "KBinstalation";
            this.Text = "KBinstalation";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton SingleRadioButton;
        private System.Windows.Forms.RadioButton MassRadioButton;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button FetchTxtButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.RadioButton kbCheckRadio;
        private System.Windows.Forms.RadioButton copyExecuteRadio;
        private System.Windows.Forms.RadioButton executeRadio;
        private System.Windows.Forms.RadioButton dismSfcRadio;
        private System.Windows.Forms.TextBox KBCheckTextBox;
        private System.Windows.Forms.TextBox copyExecuteTextBox;
        private System.Windows.Forms.TextBox executeTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox waitCheckBox;
    }
}