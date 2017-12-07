using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateApp
{
    public partial class MainForm : Form
    {
        private int IntervalTime = 10;

        private int IntervalNumber = 0;
        public MainForm()
        {
            InitializeComponent();
            this.niApp.Visible = true;//在通知区显示Form的Icon


            //this.ShowInTaskbar = false;//使Form不在任务栏上显示
        }

        private bool ProcessExits(string processName)
        {
            try
            {
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                {
                    if (p.ProcessName == processName)
                    {
                        //QQ在运行
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void tmrCounter_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrCounter.Stop();
                int intervalTime = IntervalNumber * tmrCounter.Interval / 1000;
                if (intervalTime > IntervalTime)
                {
                    if (ProcessExits("ToilluminateClient.exe") == false)
                    {
                        Process.Start("ToilluminateClient.exe");
                    }
                    IntervalNumber = 0;
                }
                else
                {
                    IntervalNumber++;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                tmrCounter.Start();
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            tmrCounter.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tmrCounter.Start();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生          
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.niApp.Icon = this.Icon;
                this.Hide();
            }
        }

        private void niApp_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsApp.Show();
            }
            else if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolMenuCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void niApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}
