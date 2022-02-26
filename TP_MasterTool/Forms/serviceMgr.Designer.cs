
namespace TP_MasterTool
{
    partial class serviceMgr
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dispName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.servName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rescanSlave = new System.ComponentModel.BackgroundWorker();
            this.rescanButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.refreshButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dispName,
            this.servName,
            this.State});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 43);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 75;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(527, 363);
            this.dataGridView1.TabIndex = 0;
            // 
            // dispName
            // 
            this.dispName.DataPropertyName = "dispName";
            this.dispName.FillWeight = 50F;
            this.dispName.HeaderText = "Name";
            this.dispName.MinimumWidth = 6;
            this.dispName.Name = "dispName";
            this.dispName.ReadOnly = true;
            // 
            // servName
            // 
            this.servName.DataPropertyName = "servName";
            this.servName.FillWeight = 25F;
            this.servName.HeaderText = "Service";
            this.servName.MinimumWidth = 6;
            this.servName.Name = "servName";
            this.servName.ReadOnly = true;
            // 
            // State
            // 
            this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.State.DataPropertyName = "state";
            this.State.FillWeight = 25F;
            this.State.HeaderText = "State";
            this.State.MinimumWidth = 6;
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // rescanSlave
            // 
            this.rescanSlave.WorkerReportsProgress = true;
            this.rescanSlave.DoWork += new System.ComponentModel.DoWorkEventHandler(this.rescanSlave_DoWork);
            this.rescanSlave.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.rescanSlave_ProgressChanged);
            // 
            // rescanButton
            // 
            this.rescanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rescanButton.Location = new System.Drawing.Point(470, 417);
            this.rescanButton.Name = "rescanButton";
            this.rescanButton.Size = new System.Drawing.Size(69, 33);
            this.rescanButton.TabIndex = 1;
            this.rescanButton.Text = "Rescan";
            this.rescanButton.UseVisualStyleBackColor = true;
            this.rescanButton.Click += new System.EventHandler(this.rescanButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StopButton.Location = new System.Drawing.Point(320, 417);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(69, 33);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(245, 417);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(69, 33);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(12, 417);
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Maximum = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(197, 18);
            this.progressBar.Step = 5;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 4;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Location = new System.Drawing.Point(395, 417);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(69, 33);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(395, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(144, 22);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(337, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Filter:";
            // 
            // serviceMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 459);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.rescanButton);
            this.Controls.Add(this.dataGridView1);
            this.MinimumSize = new System.Drawing.Size(572, 506);
            this.Name = "serviceMgr";
            this.Text = "Service Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.serviceMgr_FormClosing);
            this.Shown += new System.EventHandler(this.ServiceManager_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dispName;
        private System.Windows.Forms.DataGridViewTextBoxColumn servName;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.ComponentModel.BackgroundWorker rescanSlave;
        private System.Windows.Forms.Button rescanButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}