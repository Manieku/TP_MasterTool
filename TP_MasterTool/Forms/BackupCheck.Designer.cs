
namespace TP_MasterTool.Forms
{
    partial class BackupCheck
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
            this.driveSpaceLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cBackupFilesDataGrid = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cSummaryLabel = new System.Windows.Forms.Label();
            this.cFilesStatusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dSummaryLabel = new System.Windows.Forms.Label();
            this.dFilesStatusLabel = new System.Windows.Forms.Label();
            this.dBackupFilesDataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VeritasLogTextBox = new System.Windows.Forms.RichTextBox();
            this.rescanButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cBackupFilesDataGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dBackupFilesDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // driveSpaceLabel
            // 
            this.driveSpaceLabel.AutoSize = true;
            this.driveSpaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driveSpaceLabel.Location = new System.Drawing.Point(13, 13);
            this.driveSpaceLabel.Name = "driveSpaceLabel";
            this.driveSpaceLabel.Size = new System.Drawing.Size(235, 18);
            this.driveSpaceLabel.TabIndex = 0;
            this.driveSpaceLabel.Text = "F:\\ Drive Free Space: Calculating...";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(13, 39);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(54, 18);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Status:";
            // 
            // cBackupFilesDataGrid
            // 
            this.cBackupFilesDataGrid.AllowUserToAddRows = false;
            this.cBackupFilesDataGrid.AllowUserToDeleteRows = false;
            this.cBackupFilesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cBackupFilesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.CreationDate});
            this.cBackupFilesDataGrid.Location = new System.Drawing.Point(16, 21);
            this.cBackupFilesDataGrid.Name = "cBackupFilesDataGrid";
            this.cBackupFilesDataGrid.ReadOnly = true;
            this.cBackupFilesDataGrid.RowHeadersVisible = false;
            this.cBackupFilesDataGrid.RowHeadersWidth = 51;
            this.cBackupFilesDataGrid.RowTemplate.Height = 24;
            this.cBackupFilesDataGrid.Size = new System.Drawing.Size(528, 130);
            this.cBackupFilesDataGrid.TabIndex = 2;
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileName.FillWeight = 60F;
            this.FileName.HeaderText = "FileName";
            this.FileName.MinimumWidth = 6;
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // CreationDate
            // 
            this.CreationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CreationDate.FillWeight = 40F;
            this.CreationDate.HeaderText = "CreationDate";
            this.CreationDate.MinimumWidth = 6;
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cSummaryLabel);
            this.groupBox1.Controls.Add(this.cFilesStatusLabel);
            this.groupBox1.Controls.Add(this.cBackupFilesDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(16, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(846, 202);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "C:\\ Drive Backups";
            // 
            // cSummaryLabel
            // 
            this.cSummaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cSummaryLabel.Location = new System.Drawing.Point(16, 158);
            this.cSummaryLabel.Name = "cSummaryLabel";
            this.cSummaryLabel.Size = new System.Drawing.Size(824, 41);
            this.cSummaryLabel.TabIndex = 4;
            this.cSummaryLabel.Text = "Summary:";
            // 
            // cFilesStatusLabel
            // 
            this.cFilesStatusLabel.AutoSize = true;
            this.cFilesStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cFilesStatusLabel.Location = new System.Drawing.Point(550, 21);
            this.cFilesStatusLabel.Name = "cFilesStatusLabel";
            this.cFilesStatusLabel.Size = new System.Drawing.Size(54, 18);
            this.cFilesStatusLabel.TabIndex = 3;
            this.cFilesStatusLabel.Text = "Status:\r\n";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dSummaryLabel);
            this.groupBox2.Controls.Add(this.dFilesStatusLabel);
            this.groupBox2.Controls.Add(this.dBackupFilesDataGrid);
            this.groupBox2.Location = new System.Drawing.Point(16, 284);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 202);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "D:\\ Drive Backups";
            // 
            // dSummaryLabel
            // 
            this.dSummaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dSummaryLabel.Location = new System.Drawing.Point(16, 158);
            this.dSummaryLabel.Name = "dSummaryLabel";
            this.dSummaryLabel.Size = new System.Drawing.Size(824, 41);
            this.dSummaryLabel.TabIndex = 4;
            this.dSummaryLabel.Text = "Summary:";
            // 
            // dFilesStatusLabel
            // 
            this.dFilesStatusLabel.AutoSize = true;
            this.dFilesStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dFilesStatusLabel.Location = new System.Drawing.Point(550, 21);
            this.dFilesStatusLabel.Name = "dFilesStatusLabel";
            this.dFilesStatusLabel.Size = new System.Drawing.Size(54, 18);
            this.dFilesStatusLabel.TabIndex = 3;
            this.dFilesStatusLabel.Text = "Status:\r\n";
            // 
            // dBackupFilesDataGrid
            // 
            this.dBackupFilesDataGrid.AllowUserToAddRows = false;
            this.dBackupFilesDataGrid.AllowUserToDeleteRows = false;
            this.dBackupFilesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dBackupFilesDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dBackupFilesDataGrid.Location = new System.Drawing.Point(16, 21);
            this.dBackupFilesDataGrid.Name = "dBackupFilesDataGrid";
            this.dBackupFilesDataGrid.ReadOnly = true;
            this.dBackupFilesDataGrid.RowHeadersVisible = false;
            this.dBackupFilesDataGrid.RowHeadersWidth = 51;
            this.dBackupFilesDataGrid.RowTemplate.Height = 24;
            this.dBackupFilesDataGrid.Size = new System.Drawing.Size(528, 130);
            this.dBackupFilesDataGrid.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "FileName";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 40F;
            this.dataGridViewTextBoxColumn2.HeaderText = "CreationDate";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // VeritasLogTextBox
            // 
            this.VeritasLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VeritasLogTextBox.Location = new System.Drawing.Point(16, 503);
            this.VeritasLogTextBox.Name = "VeritasLogTextBox";
            this.VeritasLogTextBox.ReadOnly = true;
            this.VeritasLogTextBox.Size = new System.Drawing.Size(840, 222);
            this.VeritasLogTextBox.TabIndex = 0;
            this.VeritasLogTextBox.Text = "";
            // 
            // rescanButton
            // 
            this.rescanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rescanButton.Location = new System.Drawing.Point(743, 7);
            this.rescanButton.Name = "rescanButton";
            this.rescanButton.Size = new System.Drawing.Size(113, 32);
            this.rescanButton.TabIndex = 7;
            this.rescanButton.Text = "Rescan";
            this.rescanButton.UseVisualStyleBackColor = true;
            this.rescanButton.Click += new System.EventHandler(this.RescanButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(743, 45);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(113, 32);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save Raport";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // BackupCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 753);
            this.Controls.Add(this.VeritasLogTextBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.rescanButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.driveSpaceLabel);
            this.MinimumSize = new System.Drawing.Size(892, 800);
            this.Name = "BackupCheck";
            this.Text = "BackupCheck";
            this.Shown += new System.EventHandler(this.BackupCheck_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.cBackupFilesDataGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dBackupFilesDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label driveSpaceLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.DataGridView cBackupFilesDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label cFilesStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label dFilesStatusLabel;
        private System.Windows.Forms.DataGridView dBackupFilesDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label cSummaryLabel;
        private System.Windows.Forms.Label dSummaryLabel;
        private System.Windows.Forms.RichTextBox VeritasLogTextBox;
        private System.Windows.Forms.Button rescanButton;
        private System.Windows.Forms.Button saveButton;
    }
}