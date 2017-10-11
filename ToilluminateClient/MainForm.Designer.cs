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
            this.pnlShowImage = new System.Windows.Forms.Panel();
            this.pnlShowMessage = new System.Windows.Forms.Panel();
            this.pnlShowMediaWMP = new System.Windows.Forms.Panel();
            this.pnlShowMediaVLC = new System.Windows.Forms.Panel();
            this.pnlShowWeb = new System.Windows.Forms.Panel();
            this.pnlShowPDF = new System.Windows.Forms.Panel();
            this.picImage1 = new System.Windows.Forms.PictureBox();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.picImage2 = new System.Windows.Forms.PictureBox();
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrAll = new System.Windows.Forms.Timer(this.components);
            this.pnlShowImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShowImage
            // 
            this.pnlShowImage.Controls.Add(this.picImage2);
            this.pnlShowImage.Controls.Add(this.picImage1);
            this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
            this.pnlShowImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowImage.Name = "pnlShowImage";
            this.pnlShowImage.Size = new System.Drawing.Size(320, 240);
            this.pnlShowImage.TabIndex = 0;
            this.pnlShowImage.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // pnlShowMessage
            // 
            this.pnlShowMessage.Location = new System.Drawing.Point(320, 0);
            this.pnlShowMessage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMessage.Name = "pnlShowMessage";
            this.pnlShowMessage.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMessage.TabIndex = 1;
            this.pnlShowMessage.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // pnlShowMediaWMP
            // 
            this.pnlShowMediaWMP.Location = new System.Drawing.Point(640, 0);
            this.pnlShowMediaWMP.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMediaWMP.Name = "pnlShowMediaWMP";
            this.pnlShowMediaWMP.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMediaWMP.TabIndex = 2;
            this.pnlShowMediaWMP.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // pnlShowMediaVLC
            // 
            this.pnlShowMediaVLC.Location = new System.Drawing.Point(0, 240);
            this.pnlShowMediaVLC.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowMediaVLC.Name = "pnlShowMediaVLC";
            this.pnlShowMediaVLC.Size = new System.Drawing.Size(320, 240);
            this.pnlShowMediaVLC.TabIndex = 3;
            this.pnlShowMediaVLC.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
            // 
            // pnlShowWeb
            // 
            this.pnlShowWeb.Location = new System.Drawing.Point(320, 240);
            this.pnlShowWeb.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowWeb.Name = "pnlShowWeb";
            this.pnlShowWeb.Size = new System.Drawing.Size(320, 240);
            this.pnlShowWeb.TabIndex = 4;
            this.pnlShowWeb.DoubleClick += new System.EventHandler(this.panle_DoubleClick);
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
            // picImage1
            // 
            this.picImage1.Location = new System.Drawing.Point(51, 51);
            this.picImage1.Name = "picImage1";
            this.picImage1.Size = new System.Drawing.Size(100, 50);
            this.picImage1.TabIndex = 0;
            this.picImage1.TabStop = false;
            // 
            // picImage2
            // 
            this.picImage2.Location = new System.Drawing.Point(110, 95);
            this.picImage2.Name = "picImage2";
            this.picImage2.Size = new System.Drawing.Size(100, 50);
            this.picImage2.TabIndex = 1;
            this.picImage2.TabStop = false;
            // 
            // tmrImage
            // 
            this.tmrImage.Tick += new System.EventHandler(this.tmrImage_Tick);
            // 
            // tmrAll
            // 
            this.tmrAll.Interval = 1000;
            this.tmrAll.Tick += new System.EventHandler(this.tmrAll_Tick);
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
            ((System.ComponentModel.ISupportInitialize)(this.picImage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShowImage;
        private System.Windows.Forms.Panel pnlShowMessage;
        private System.Windows.Forms.Panel pnlShowMediaWMP;
        private System.Windows.Forms.Panel pnlShowMediaVLC;
        private System.Windows.Forms.Panel pnlShowWeb;
        private System.Windows.Forms.Panel pnlShowPDF;
        private System.Windows.Forms.PictureBox picImage1;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.PictureBox picImage2;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrAll;
    }
}

