namespace ToilluminateApp
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
            this.tmrCounter = new System.Windows.Forms.Timer(this.components);
            this.btnEnter = new System.Windows.Forms.Button();
            this.niApp = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsApp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolMenuCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrCounter
            // 
            this.tmrCounter.Interval = 500;
            this.tmrCounter.Tick += new System.EventHandler(this.tmrCounter_Tick);
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(36, 90);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 0;
            this.btnEnter.Text = "Enter";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // niApp
            // 
            this.niApp.ContextMenuStrip = this.cmsApp;
            this.niApp.Icon = ((System.Drawing.Icon)(resources.GetObject("niApp.Icon")));
            this.niApp.Text = "ToilluminateApp";
            this.niApp.Visible = true;
            this.niApp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.niApp_MouseClick);
            this.niApp.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niApp_MouseDoubleClick);
            // 
            // cmsApp
            // 
            this.cmsApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuCancel});
            this.cmsApp.Name = "cmsApp";
            this.cmsApp.Size = new System.Drawing.Size(115, 26);
            // 
            // toolMenuCancel
            // 
            this.toolMenuCancel.Name = "toolMenuCancel";
            this.toolMenuCancel.Size = new System.Drawing.Size(114, 22);
            this.toolMenuCancel.Text = "Cancel";
            this.toolMenuCancel.Click += new System.EventHandler(this.toolMenuCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnEnter);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.cmsApp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrCounter;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.NotifyIcon niApp;
        private System.Windows.Forms.ContextMenuStrip cmsApp;
        private System.Windows.Forms.ToolStripMenuItem toolMenuCancel;
    }
}

