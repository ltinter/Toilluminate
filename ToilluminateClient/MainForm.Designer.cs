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
            this.pnlShow = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrPlayList = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrTemplete = new System.Windows.Forms.Timer(this.components);
            this.chkRefresh = new System.Windows.Forms.CheckBox();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlShow
            // 
            this.pnlShow.BackColor = System.Drawing.Color.White;
            this.pnlShow.Controls.Add(this.axWMP);
            this.pnlShow.Controls.Add(this.picImage);
            this.pnlShow.Location = new System.Drawing.Point(0, 0);
            this.pnlShow.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(620, 392);
            this.pnlShow.TabIndex = 0;
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
            // tmrTemplete
            // 
            this.tmrTemplete.Interval = 500;
            this.tmrTemplete.Tick += new System.EventHandler(this.tmrTemplete_Tick);
            // 
            // chkRefresh
            // 
            this.chkRefresh.AutoSize = true;
            this.chkRefresh.Location = new System.Drawing.Point(762, 343);
            this.chkRefresh.Name = "chkRefresh";
            this.chkRefresh.Size = new System.Drawing.Size(96, 16);
            this.chkRefresh.TabIndex = 2;
            this.chkRefresh.Text = "新しいPlayList";
            this.chkRefresh.UseVisualStyleBackColor = true;
            this.chkRefresh.Visible = false;
            this.chkRefresh.CheckedChanged += new System.EventHandler(this.chkRefresh_CheckedChanged);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 518);
            this.Controls.Add(this.chkRefresh);
            this.Controls.Add(this.pnlShow);
            this.Name = "MainForm";
            this.Text = "表示情報";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlShow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlShow;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrPlayList;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrTemplete;
        private System.Windows.Forms.CheckBox chkRefresh;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
    }
}

