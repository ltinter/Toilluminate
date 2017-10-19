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
            this.pnlShowMessage = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlShowMediaWMP = new System.Windows.Forms.Panel();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlShowMediaVLC = new System.Windows.Forms.Panel();
            this.axVLC = new AxAXVLC.AxVLCPlugin2();
            this.pnlShowWeb = new System.Windows.Forms.Panel();
            this.wbsBox = new System.Windows.Forms.WebBrowser();
            this.pnlShowPDF = new System.Windows.Forms.Panel();
            this.tipBox = new System.Windows.Forms.ToolTip(this.components);
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrAll = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.tmrWeb = new System.Windows.Forms.Timer(this.components);
            this.tmrPDF = new System.Windows.Forms.Timer(this.components);
            this.pnlShowImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlShowMessage.SuspendLayout();
            this.pnlShowMediaWMP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.pnlShowMediaVLC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axVLC)).BeginInit();
            this.pnlShowWeb.SuspendLayout();
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
            // pnlShowMessage
            // 
            this.pnlShowMessage.Controls.Add(this.lblMessage);
            this.pnlShowMessage.Location = new System.Drawing.Point(320, 0);
            this.pnlShowMessage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMessage.Name = "pnlShowMessage";
            this.pnlShowMessage.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMessage.TabIndex = 1;
            this.pnlShowMessage.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(192, 105);
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
            // pnlShowMediaVLC
            // 
            this.pnlShowMediaVLC.Controls.Add(this.axVLC);
            this.pnlShowMediaVLC.Location = new System.Drawing.Point(0, 240);
            this.pnlShowMediaVLC.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMediaVLC.Name = "pnlShowMediaVLC";
            this.pnlShowMediaVLC.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMediaVLC.TabIndex = 3;
            this.pnlShowMediaVLC.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // axVLC
            // 
            this.axVLC.Enabled = true;
            this.axVLC.Location = new System.Drawing.Point(12, 16);
            this.axVLC.Name = "axVLC";
            this.axVLC.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLC.OcxState")));
            this.axVLC.Size = new System.Drawing.Size(259, 159);
            this.axVLC.TabIndex = 0;
            // 
            // pnlShowWeb
            // 
            this.pnlShowWeb.Controls.Add(this.wbsBox);
            this.pnlShowWeb.Location = new System.Drawing.Point(320, 240);
            this.pnlShowWeb.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowWeb.Name = "pnlShowWeb";
            this.pnlShowWeb.Size = new System.Drawing.Size(320, 240);
            this.pnlShowWeb.TabIndex = 4;
            this.pnlShowWeb.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // wbsBox
            // 
            this.wbsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbsBox.Location = new System.Drawing.Point(0, 0);
            this.wbsBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbsBox.Name = "wbsBox";
            this.wbsBox.Size = new System.Drawing.Size(320, 240);
            this.wbsBox.TabIndex = 0;
            // 
            // pnlShowPDF
            // 
            this.pnlShowPDF.Location = new System.Drawing.Point(640, 240);
            this.pnlShowPDF.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowPDF.Name = "pnlShowPDF";
            this.pnlShowPDF.Size = new System.Drawing.Size(320, 240);
            this.pnlShowPDF.TabIndex = 5;
            this.pnlShowPDF.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // tmrImage
            // 
            this.tmrImage.Interval = 3000;
            this.tmrImage.Tick += new System.EventHandler(this.tmrImage_Tick);
            // 
            // tmrAll
            // 
            this.tmrAll.Interval = 1000;
            this.tmrAll.Tick += new System.EventHandler(this.tmrAll_Tick);
            // 
            // tmrMedia
            // 
            this.tmrMedia.Interval = 1000;
            this.tmrMedia.Tick += new System.EventHandler(this.tmrMedia_Tick);
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 1000;
            this.tmrMessage.Tick += new System.EventHandler(this.tmrMessage_Tick);
            // 
            // tmrWeb
            // 
            this.tmrWeb.Interval = 1000;
            this.tmrWeb.Tick += new System.EventHandler(this.tmrWeb_Tick);
            // 
            // tmrPDF
            // 
            this.tmrPDF.Interval = 1000;
            this.tmrPDF.Tick += new System.EventHandler(this.tmrPDF_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 518);
            this.Controls.Add(this.pnlShowPDF);
            this.Controls.Add(this.pnlShowWeb);
            this.Controls.Add(this.pnlShowMediaVLC);
            this.Controls.Add(this.pnlShowMediaWMP);
            this.Controls.Add(this.pnlShowMessage);
            this.Controls.Add(this.pnlShowImage);
            this.Name = "MainForm";
            this.Text = "表示情報";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.pnlShowImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlShowMessage.ResumeLayout(false);
            this.pnlShowMediaWMP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.pnlShowMediaVLC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axVLC)).EndInit();
            this.pnlShowWeb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShowImage;
        private System.Windows.Forms.Panel pnlShowMessage;
        private System.Windows.Forms.Panel pnlShowMediaWMP;
        private System.Windows.Forms.Panel pnlShowMediaVLC;
        private System.Windows.Forms.Panel pnlShowWeb;
        private System.Windows.Forms.Panel pnlShowPDF;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.ToolTip tipBox;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrAll;
        private System.Windows.Forms.WebBrowser wbsBox;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
        private AxAXVLC.AxVLCPlugin2 axVLC;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Timer tmrWeb;
        private System.Windows.Forms.Timer tmrPDF;
        private System.Windows.Forms.Label lblMessage;
    }
}

