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
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.pnlTrademark = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 10;
            this.tmrMessage.Tick += new System.EventHandler(this.tmrMessage_Tick);
            // 
            // pnlMessage
            // 
            this.pnlMessage.BackColor = System.Drawing.Color.Transparent;
            this.pnlMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(200, 100);
            this.pnlMessage.TabIndex = 0;
            // 
            // pnlTrademark
            // 
            this.pnlTrademark.BackColor = System.Drawing.Color.Transparent;
            this.pnlTrademark.Location = new System.Drawing.Point(61, 135);
            this.pnlTrademark.Name = "pnlTrademark";
            this.pnlTrademark.Size = new System.Drawing.Size(200, 100);
            this.pnlTrademark.TabIndex = 1;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pnlTrademark);
            this.Controls.Add(this.pnlMessage);
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

        public System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Panel pnlTrademark;
    }
}