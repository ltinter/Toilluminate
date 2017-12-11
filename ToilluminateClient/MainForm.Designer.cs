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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlShowImage = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlShowMedia = new System.Windows.Forms.Panel();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrPlayList = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrTemplete = new System.Windows.Forms.Timer(this.components);
            this.pnlShowImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlShowMedia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShowImage
            // 
            this.pnlShowImage.BackColor = System.Drawing.Color.White;
            this.pnlShowImage.Controls.Add(this.picImage);
            this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
            this.pnlShowImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowImage.Name = "pnlShowImage";
            this.pnlShowImage.Size = new System.Drawing.Size(620, 392);
            this.pnlShowImage.TabIndex = 0;
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
            // pnlShowMedia
            // 
            this.pnlShowMedia.BackColor = System.Drawing.Color.White;
            this.pnlShowMedia.Controls.Add(this.axWMP);
            this.pnlShowMedia.Location = new System.Drawing.Point(0, 0);
            this.pnlShowMedia.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMedia.Name = "pnlShowMedia";
            this.pnlShowMedia.Size = new System.Drawing.Size(620, 392);
            this.pnlShowMedia.TabIndex = 1;
            // 
            // axWMP
            // 
            this.axWMP.Enabled = true;
            this.axWMP.Location = new System.Drawing.Point(234, 103);
            this.axWMP.Name = "axWMP";
            this.axWMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWMP.OcxState")));
            this.axWMP.Size = new System.Drawing.Size(292, 230);
            this.axWMP.TabIndex = 2;
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
            this.Controls.Add(this.pnlShowMedia);
            this.Controls.Add(this.pnlShowImage);
            this.Name = "MainForm";
            this.Text = "表示情報";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.pnlShowImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlShowMedia.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShowImage;
        private System.Windows.Forms.Panel pnlShowMedia;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrPlayList;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrTemplete;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
    }
}

