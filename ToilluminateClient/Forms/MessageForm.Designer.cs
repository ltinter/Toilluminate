namespace ToilluminateClient
{
    partial class MessageForm
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
            this.tmrShow = new System.Windows.Forms.Timer(this.components);
            this.pnlMessage0 = new System.Windows.Forms.Panel();
            this.pnlMessage1 = new System.Windows.Forms.Panel();
            this.pnlMessage2 = new System.Windows.Forms.Panel();
            this.pnlMessage3 = new System.Windows.Forms.Panel();
            this.pnlMessage4 = new System.Windows.Forms.Panel();
            this.pnlMessage5 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tmrShow
            // 
            this.tmrShow.Interval = 10;
            this.tmrShow.Tick += new System.EventHandler(this.tmrShow_Tick);
            // 
            // pnlMessage0
            // 
            this.pnlMessage0.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage0.Location = new System.Drawing.Point(121, 12);
            this.pnlMessage0.Name = "pnlMessage0";
            this.pnlMessage0.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage0.TabIndex = 0;
            // 
            // pnlMessage1
            // 
            this.pnlMessage1.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage1.Location = new System.Drawing.Point(121, 89);
            this.pnlMessage1.Name = "pnlMessage1";
            this.pnlMessage1.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage1.TabIndex = 1;
            // 
            // pnlMessage2
            // 
            this.pnlMessage2.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage2.Location = new System.Drawing.Point(121, 220);
            this.pnlMessage2.Name = "pnlMessage2";
            this.pnlMessage2.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage2.TabIndex = 2;
            // 
            // pnlMessage3
            // 
            this.pnlMessage3.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage3.Location = new System.Drawing.Point(12, 110);
            this.pnlMessage3.Name = "pnlMessage3";
            this.pnlMessage3.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage3.TabIndex = 3;
            // 
            // pnlMessage4
            // 
            this.pnlMessage4.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage4.Location = new System.Drawing.Point(121, 125);
            this.pnlMessage4.Name = "pnlMessage4";
            this.pnlMessage4.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage4.TabIndex = 4;
            // 
            // pnlMessage5
            // 
            this.pnlMessage5.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage5.Location = new System.Drawing.Point(242, 110);
            this.pnlMessage5.Name = "pnlMessage5";
            this.pnlMessage5.Size = new System.Drawing.Size(30, 30);
            this.pnlMessage5.TabIndex = 5;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pnlMessage5);
            this.Controls.Add(this.pnlMessage4);
            this.Controls.Add(this.pnlMessage3);
            this.Controls.Add(this.pnlMessage2);
            this.Controls.Add(this.pnlMessage1);
            this.Controls.Add(this.pnlMessage0);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MessageForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MessageForm_FormClosed);
            this.Load += new System.EventHandler(this.MessageForm_Load);
            this.Shown += new System.EventHandler(this.MessageForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MessageForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer tmrShow;
        private System.Windows.Forms.Panel pnlMessage0;
        private System.Windows.Forms.Panel pnlMessage1;
        private System.Windows.Forms.Panel pnlMessage2;
        private System.Windows.Forms.Panel pnlMessage3;
        private System.Windows.Forms.Panel pnlMessage4;
        private System.Windows.Forms.Panel pnlMessage5;
    }
}