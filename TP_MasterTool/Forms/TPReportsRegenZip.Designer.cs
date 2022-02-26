
namespace TP_MasterTool.Forms
{
    partial class TPReportsRegenZip
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
            this.dayofEOD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.beforeEOD = new System.Windows.Forms.TextBox();
            this.ZipCheckbox = new System.Windows.Forms.CheckBox();
            this.regenerateCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // SingleRadioButton
            // 
            this.SingleRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SingleRadioButton.AutoSize = true;
            this.SingleRadioButton.Checked = true;
            this.SingleRadioButton.Location = new System.Drawing.Point(64, 15);
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
            this.MassRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MassRadioButton.AutoSize = true;
            this.MassRadioButton.Location = new System.Drawing.Point(184, 14);
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
            this.FetchTxtButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TAG,
            this.Status});
            this.dataGridView1.Location = new System.Drawing.Point(17, 134);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(344, 335);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
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
            // dayofEOD
            // 
            this.dayofEOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dayofEOD.Location = new System.Drawing.Point(277, 71);
            this.dayofEOD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dayofEOD.MaxLength = 8;
            this.dayofEOD.Name = "dayofEOD";
            this.dayofEOD.Size = new System.Drawing.Size(84, 22);
            this.dayofEOD.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "-";
            // 
            // beforeEOD
            // 
            this.beforeEOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.beforeEOD.Location = new System.Drawing.Point(174, 71);
            this.beforeEOD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beforeEOD.MaxLength = 8;
            this.beforeEOD.Name = "beforeEOD";
            this.beforeEOD.Size = new System.Drawing.Size(78, 22);
            this.beforeEOD.TabIndex = 10;
            // 
            // ZipCheckbox
            // 
            this.ZipCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipCheckbox.AutoSize = true;
            this.ZipCheckbox.BackColor = System.Drawing.SystemColors.Control;
            this.ZipCheckbox.Location = new System.Drawing.Point(17, 98);
            this.ZipCheckbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ZipCheckbox.Name = "ZipCheckbox";
            this.ZipCheckbox.Size = new System.Drawing.Size(104, 21);
            this.ZipCheckbox.TabIndex = 9;
            this.ZipCheckbox.Text = "Zip Reports";
            this.ZipCheckbox.UseVisualStyleBackColor = false;
            // 
            // regenerateCheckbox
            // 
            this.regenerateCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.regenerateCheckbox.AutoSize = true;
            this.regenerateCheckbox.Location = new System.Drawing.Point(17, 71);
            this.regenerateCheckbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.regenerateCheckbox.Name = "regenerateCheckbox";
            this.regenerateCheckbox.Size = new System.Drawing.Size(163, 21);
            this.regenerateCheckbox.TabIndex = 8;
            this.regenerateCheckbox.Text = "Regenerate Reports:";
            this.regenerateCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 482);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "0 / 0";
            // 
            // TPReportsRegenZip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 517);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dayofEOD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.beforeEOD);
            this.Controls.Add(this.ZipCheckbox);
            this.Controls.Add(this.regenerateCheckbox);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FetchTxtButton);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.MassRadioButton);
            this.Controls.Add(this.SingleRadioButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(395, 564);
            this.Name = "TPReportsRegenZip";
            this.Text = "TPReportsRegenZip";
            this.Shown += new System.EventHandler(this.TPReportsRegenZip_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.TextBox dayofEOD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox beforeEOD;
        private System.Windows.Forms.CheckBox ZipCheckbox;
        private System.Windows.Forms.CheckBox regenerateCheckbox;
        private System.Windows.Forms.Label label1;
    }
}