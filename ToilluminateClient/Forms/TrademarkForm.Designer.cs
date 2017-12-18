namespace ToilluminateClient
{
    partial class TrademarkForm
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
            this.pnlTrademark = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tmrShow
            // 
            this.tmrShow.Interval = 10;
            this.tmrShow.Tick += new System.EventHandler(this.tmrShow_Tick);
            // 
            // pnlTrademark
            // 
            this.pnlTrademark.BackColor = System.Drawing.Color.Transparent;
            this.pnlTrademark.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlTrademark.Location = new System.Drawing.Point(0, 0);
            this.pnlTrademark.Name = "pnlTrademark";
            this.pnlTrademark.Size = new System.Drawing.Size(200, 100);
            this.pnlTrademark.TabIndex = 0;
            // 
            // TrademarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pnlTrademark);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TrademarkForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrademarkForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TrademarkForm_FormClosed);
            this.Load += new System.EventHandler(this.TrademarkForm_Load);
            this.Shown += new System.EventHandler(this.TrademarkForm_Shown);
            this.SizeChanged += new System.EventHandler(this.TrademarkForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer tmrShow;
        private System.Windows.Forms.Panel pnlTrademark;
    }
}