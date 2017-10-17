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
    }
}
