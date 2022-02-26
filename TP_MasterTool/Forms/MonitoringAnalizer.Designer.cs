
namespace TP_MasterTool.Forms
{
    partial class MonitoringAnalizer
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
            this.textBox = new System.Windows.Forms.TextBox();
            this.FetchTxtButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ticketNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tixSummary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(17, 13);
            this.textBox.Margin = new System.Windows.Forms.Padding(4);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(1207, 22);
            this.textBox.TabIndex = 2;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // FetchTxtButton
            // 
            this.FetchTxtButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchTxtButton.Location = new System.Drawing.Point(1224, 12);
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
            this.ticketNr,
            this.hostname,
            this.tixSummary,
            this.toolStatus});
            this.dataGridView1.Location = new System.Drawing.Point(17, 45);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1243, 424);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            // 
            // ticketNr
            // 
            this.ticketNr.HeaderText = "Ticket Nr";
            this.ticketNr.MinimumWidth = 6;
            this.ticketNr.Name = "ticketNr";
            this.ticketNr.ReadOnly = true;
            this.ticketNr.Width = 125;
            // 
            // hostname
            // 
            this.hostname.HeaderText = "TAG";
            this.hostname.MinimumWidth = 6;
            this.hostname.Name = "hostname";
            this.hostname.ReadOnly = true;
            this.hostname.Width = 125;
            // 
            // tixSummary
            // 
            this.tixSummary.HeaderText = "Summary";
            this.tixSummary.MinimumWidth = 6;
            this.tixSummary.Name = "tixSummary";
            this.tixSummary.ReadOnly = true;
            this.tixSummary.Width = 450;
            // 
            // toolStatus
            // 
            this.toolStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.toolStatus.HeaderText = "Status";
            this.toolStatus.MinimumWidth = 6;
            this.toolStatus.Name = "toolStatus";
            this.toolStatus.ReadOnly = true;
            // 
            // StartStopButton
            // 
            this.StartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStopButton.Location = new System.Drawing.Point(1160, 476);
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
            // MonitoringAnalizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 517);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FetchTxtButton);
            this.Controls.Add(this.textBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(395, 564);
            this.Name = "MonitoringAnalizer";
            this.Text = "MonitoringAnalizer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button FetchTxtButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostname;
        private System.Windows.Forms.DataGridViewTextBoxColumn tixSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolStatus;
    }
}