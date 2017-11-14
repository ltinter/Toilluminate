namespace ToilluminateClient
{
    partial class MainForm
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
            this.pnlShow = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrPlayList = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.tmrTemplete = new System.Windows.Forms.Timer(this.components);
            this.pnlShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShow
            // 
            this.pnlShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.pnlShow.Controls.Add(this.picImage);
            this.pnlShow.Location = new System.Drawing.Point(0, 0);
            this.pnlShow.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(320, 240);
            this.pnlShow.TabIndex = 0;
            this.pnlShow.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // picImage
            // 
            this.picImage.BackColor = System.Drawing.Color.Transparent;
            this.picImage.Location = new System.Drawing.Point(51, 51);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(100, 50);
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // tmrImage
            // 
            this.tmrImage.Tick += new System.EventHandler(this.tmrImage_Tick);
            // 
            // tmrPlayList
            // 
            this.tmrPlayList.Interval = 500;
            this.tmrPlayList.Tick += new System.EventHandler(this.tmrPlayList_Tick);
            // 
            // tmrMedia
            // 
            this.tmrMedia.Tick += new System.EventHandler(this.tmrMedia_Tick);
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 50;
            this.tmrMessage.Tick += new System.EventHandler(this.tmrMessage_Tick);
            // 
            // tmrTemplete
            // 
            this.tmrTemplete.Interval = 500;
            this.tmrTemplete.Tick += new System.EventHandler(this.tmrTemplete_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 518);
            this.Controls.Add(this.pnlShow);
            this.Name = "MainForm";
            this.Text = "表示情報";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.pnlShow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShow;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrPlayList;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Timer tmrTemplete;
    }
}

