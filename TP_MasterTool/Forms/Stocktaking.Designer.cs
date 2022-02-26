
namespace TP_MasterTool.Forms
{
    partial class Stocktaking
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
            this.StartStopButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TAG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Started = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Finished = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delivered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // SingleRadioButton
            // 
            this.SingleRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SingleRadioButton.AutoSize = true;
            this.SingleRadioButton.Checked = true;
            this.SingleRadioButton.Location = new System.Drawing.Point(393, 12);
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
            this.MassRadioButton.Location = new System.Drawing.Point(513, 11);
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
            this.textBox.Size = new System.Drawing.Size(915, 22);
            this.textBox.TabIndex = 2;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // FetchTxtButton
            // 
            this.FetchTxtButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchTxtButton.Enabled = false;
            this.FetchTxtButton.Location = new System.Drawing.Point(934, 43);
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
            this.Status,
            this.Started,
            this.Finished,
            this.Column1,
            this.Column2,
            this.Delivered,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView1.Location = new System.Drawing.Point(17, 73);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(951, 394);
            this.dataGridView1.TabIndex = 4;
            // 
            // StartStopButton
            // 
            this.StartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStopButton.Location = new System.Drawing.Point(868, 476);
            this.StartStopButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(100, 28);
            this.StartStopButton.TabIndex = 5;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 482);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "0 / 0";
            // 
            // TAG
            // 
            this.TAG.FillWeight = 20F;
            this.TAG.HeaderText = "TAG";
            this.TAG.MinimumWidth = 6;
            this.TAG.Name = "TAG";
            this.TAG.ReadOnly = true;
            this.TAG.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TAG.Width = 125;
            // 
            // Status
            // 
            this.Status.FillWeight = 20F;
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 125;
            // 
            // Started
            // 
            this.Started.FillWeight = 20F;
            this.Started.HeaderText = "8E File 1";
            this.Started.MinimumWidth = 6;
            this.Started.Name = "Started";
            this.Started.ReadOnly = true;
            this.Started.Width = 125;
            // 
            // Finished
            // 
            this.Finished.FillWeight = 20F;
            this.Finished.HeaderText = "8E File 2";
            this.Finished.MinimumWidth = 6;
            this.Finished.Name = "Finished";
            this.Finished.ReadOnly = true;
            this.Finished.Width = 125;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "8E File 3";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "8E File 4";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Delivered
            // 
            this.Delivered.FillWeight = 20F;
            this.Delivered.HeaderText = "8E.D File 1";
            this.Delivered.MinimumWidth = 6;
            this.Delivered.Name = "Delivered";
            this.Delivered.ReadOnly = true;
            this.Delivered.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "8E.D File 2";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "8E.D File 3";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "8E.D File 4";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 125;
            // 
            // Stocktaking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 517);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FetchTxtButton);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.MassRadioButton);
            this.Controls.Add(this.SingleRadioButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(395, 564);
            this.Name = "Stocktaking";
            this.Text = "Stocktaking";
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
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Started;
        private System.Windows.Forms.DataGridViewTextBoxColumn Finished;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delivered;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}