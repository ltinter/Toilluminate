namespace ToilluminateClient
{
    partial class LoginForm
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
            this.tipBox = new System.Windows.Forms.ToolTip(this.components);
            this.tmrImage = new System.Windows.Forms.Timer(this.components);
            this.tmrAll = new System.Windows.Forms.Timer(this.components);
            this.tmrMedia = new System.Windows.Forms.Timer(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.tmrWeb = new System.Windows.Forms.Timer(this.components);
            this.tmrPDF = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrImage
            // 
            this.tmrImage.Interval = 3000;
            // 
            // tmrAll
            // 
            this.tmrAll.Interval = 1000;
            // 
            // tmrMedia
            // 
            this.tmrMedia.Interval = 1000;
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 1000;
            // 
            // tmrWeb
            // 
            this.tmrWeb.Interval = 1000;
            // 
            // tmrPDF
            // 
            this.tmrPDF.Interval = 1000;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 518);
            this.Name = "LoginForm";
            this.Text = "表示情報";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip tipBox;
        private System.Windows.Forms.Timer tmrImage;
        private System.Windows.Forms.Timer tmrAll;
        private System.Windows.Forms.Timer tmrMedia;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Timer tmrWeb;
        private System.Windows.Forms.Timer tmrPDF;
    }
}

