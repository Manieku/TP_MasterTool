
namespace TP_Updater
{
    partial class Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.CurVersionLabel = new System.Windows.Forms.Label();
            this.NewVersionLabel = new System.Windows.Forms.Label();
            this.CurVersionNrLabel = new System.Windows.Forms.Label();
            this.NewVersionNrLabel = new System.Windows.Forms.Label();
            this.LogTextbox = new System.Windows.Forms.RichTextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurVersionLabel
            // 
            this.CurVersionLabel.AutoSize = true;
            this.CurVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurVersionLabel.Location = new System.Drawing.Point(13, 14);
            this.CurVersionLabel.Name = "CurVersionLabel";
            this.CurVersionLabel.Size = new System.Drawing.Size(155, 25);
            this.CurVersionLabel.TabIndex = 0;
            this.CurVersionLabel.Text = "Current Version:";
            // 
            // NewVersionLabel
            // 
            this.NewVersionLabel.AutoSize = true;
            this.NewVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewVersionLabel.Location = new System.Drawing.Point(13, 50);
            this.NewVersionLabel.Name = "NewVersionLabel";
            this.NewVersionLabel.Size = new System.Drawing.Size(143, 25);
            this.NewVersionLabel.TabIndex = 1;
            this.NewVersionLabel.Text = "Latest Version:";
            // 
            // CurVersionNrLabel
            // 
            this.CurVersionNrLabel.AutoSize = true;
            this.CurVersionNrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurVersionNrLabel.Location = new System.Drawing.Point(173, 14);
            this.CurVersionNrLabel.Name = "CurVersionNrLabel";
            this.CurVersionNrLabel.Size = new System.Drawing.Size(89, 24);
            this.CurVersionNrLabel.TabIndex = 2;
            this.CurVersionNrLabel.Text = "reading...";
            // 
            // NewVersionNrLabel
            // 
            this.NewVersionNrLabel.AutoSize = true;
            this.NewVersionNrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewVersionNrLabel.Location = new System.Drawing.Point(173, 50);
            this.NewVersionNrLabel.Name = "NewVersionNrLabel";
            this.NewVersionNrLabel.Size = new System.Drawing.Size(89, 24);
            this.NewVersionNrLabel.TabIndex = 3;
            this.NewVersionNrLabel.Text = "reading...";
            // 
            // LogTextbox
            // 
            this.LogTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogTextbox.ForeColor = System.Drawing.Color.Black;
            this.LogTextbox.Location = new System.Drawing.Point(19, 92);
            this.LogTextbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogTextbox.Name = "LogTextbox";
            this.LogTextbox.ReadOnly = true;
            this.LogTextbox.Size = new System.Drawing.Size(381, 277);
            this.LogTextbox.TabIndex = 4;
            this.LogTextbox.Text = "";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(301, 385);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 28);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Visible = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 428);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.LogTextbox);
            this.Controls.Add(this.NewVersionNrLabel);
            this.Controls.Add(this.CurVersionNrLabel);
            this.Controls.Add(this.NewVersionLabel);
            this.Controls.Add(this.CurVersionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "TP MasterTool Updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurVersionLabel;
        private System.Windows.Forms.Label NewVersionLabel;
        private System.Windows.Forms.Label CurVersionNrLabel;
        private System.Windows.Forms.Label NewVersionNrLabel;
        private System.Windows.Forms.RichTextBox LogTextbox;
        private System.Windows.Forms.Button closeButton;
    }
}

