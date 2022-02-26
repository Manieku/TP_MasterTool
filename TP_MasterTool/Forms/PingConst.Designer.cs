
namespace TP_MasterTool
{
    partial class PingConst
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
            this.components = new System.ComponentModel.Container();
            this.controlButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // controlButton
            // 
            this.controlButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlButton.Location = new System.Drawing.Point(171, 12);
            this.controlButton.Name = "controlButton";
            this.controlButton.Size = new System.Drawing.Size(76, 39);
            this.controlButton.TabIndex = 0;
            this.controlButton.Text = "Start";
            this.controlButton.UseVisualStyleBackColor = true;
            this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(13, 71);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(375, 213);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // timer
            // 
            this.timer.Interval = 60000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // PingConst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 296);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.controlButton);
            this.MaximumSize = new System.Drawing.Size(418, 600);
            this.MinimumSize = new System.Drawing.Size(418, 343);
            this.Name = "PingConst";
            this.Text = "PingConst";
            this.Load += new System.EventHandler(this.PingConst_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button controlButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}