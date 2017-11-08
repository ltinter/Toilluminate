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
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlShowMediaWMP = new System.Windows.Forms.Panel();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.tipBox = new System.Windows.Forms.ToolTip(this.components);
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrPlayList = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.tmrTemplete = new System.Windows.Forms.Timer(this.components);
            this.pnlShowImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlShowMediaWMP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShowImage
            // 
            this.pnlShowImage.Controls.Add(this.picImage);
            this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
            this.pnlShowImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowImage.Name = "pnlShowImage";
            this.pnlShowImage.Size = new System.Drawing.Size(320, 240);
            this.pnlShowImage.TabIndex = 0;
            this.pnlShowImage.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(51, 51);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(100, 50);
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(489, 129);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(100, 23);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "lblMessage";
            this.lblMessage.DoubleClick += new System.EventHandler(this.control_DoubleClick);
            // 
            // pnlShowMediaWMP
            // 
            this.pnlShowMediaWMP.Controls.Add(this.axWMP);
            this.pnlShowMediaWMP.Location = new System.Drawing.Point(640, 0);
            this.pnlShowMediaWMP.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMediaWMP.Name = "pnlShowMediaWMP";
            this.pnlShowMediaWMP.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMediaWMP.TabIndex = 2;
            this.pnlShowMediaWMP.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // axWMP
            // 
            this.axWMP.Enabled = true;
            this.axWMP.Location = new System.Drawing.Point(39, 32);
            this.axWMP.Name = "axWMP";
            this.axWMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWMP.OcxState")));
            this.axWMP.Size = new System.Drawing.Size(207, 150);
            this.axWMP.TabIndex = 0;
            // 
            // tmrImage
            // 
            this.tmrImage.Interval = 500;
            this.tmrImage.Tick += new System.EventHandler(this.tmrImage_Tick);
            // 
            // tmrPlayList
            // 
            this.tmrPlayList.Interval = 500;
            this.tmrPlayList.Tick += new System.EventHandler(this.tmrPlayList_Tick);
            // 
            // tmrMedia
            // 
            this.tmrMedia.Interval = 500;
            this.tmrMedia.Tick += new System.EventHandler(this.tmrMedia_Tick);
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 500;
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
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlShowImage);
            this.Controls.Add(this.pnlShowMediaWMP);
            this.Name = "MainForm";
            this.Text = "表示情報";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.pnlShowImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlShowMediaWMP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShowImage;
        private System.Windows.Forms.Panel pnlShowMediaWMP;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.ToolTip tipBox;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrPlayList;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer tmrTemplete;
    }
}

