
namespace TP_MasterTool.Forms
{
    partial class MassEmergancy
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
            this.label1 = new System.Windows.Forms.Label();
            this.functionSelectList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // SingleRadioButton
            // 
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
            this.dataGridView1.Location = new System.Drawing.Point(17, 104);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(344, 365);
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
            // functionSelectList
            // 
            this.functionSelectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionSelectList.FormattingEnabled = true;
            this.functionSelectList.Location = new System.Drawing.Point(17, 73);
            this.functionSelectList.Name = "functionSelectList";
            this.functionSelectList.Size = new System.Drawing.Size(349, 24);
            this.functionSelectList.TabIndex = 7;
            // 
            // MassEmergancy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 517);
            this.Controls.Add(this.functionSelectList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FetchTxtButton);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.MassRadioButton);
            this.Controls.Add(this.SingleRadioButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(395, 564);
            this.Name = "MassEmergancy";
            this.Text = "Random Randomness";
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox functionSelectList;
    }
}