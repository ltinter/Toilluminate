using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateClient
{
    public partial class TrademarkForm : Form
    {
        private bool showTrademarkFlag = false;

        private bool showTrademarkEnd = false;

        private MainForm parentForm;

        #region " override "
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;
            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:

                        parentForm.MaxShowThis(true);
                        break;

                    case Keys.Space:

                        parentForm.MaxShowThis(false);
                        break;
                }
            }
            return false;
        }
        #endregion

        public TrademarkForm()
        {
            InitializeComponent();

            this.BackColor = ImageApp.BackClearColor;
            this.TopMost = true;
            this.TransparencyKey = ImageApp.BackClearColor;

            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None;


            this.pnlTrademark.Left = 0;
            this.pnlTrademark.Top = 0;
            this.pnlTrademark.Width = this.Width;
            this.pnlTrademark.Height = this.Height;
            this.pnlTrademark.BackColor = ImageApp.BackClearColor;
            this.pnlTrademark.SendToBack();

            
        }

        public void tmrShow_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrShow.Stop();

                if (PlayApp.ExecutePlayList != null)
                {
                    if (PlayApp.ExecutePlayList.PlayListState == PlayListStateType.Stop)
                    {
                        CloseTrademark();
                        return;
                    }
                }

                ThreadShow();

                ImageApp.MyDrawTrademark(this.pnlTrademark);
               
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("TrademarkForm", "tmrTrademark_Tick", ex);
            }
            finally
            {
                this.tmrShow.Start();
            }
        }

        private void TrademarkForm_Load(object sender, EventArgs e)
        {
            this.tmrShow.Interval = 10;

        }
        private void TrademarkForm_Shown(object sender, EventArgs e)
        {
            ShowApp.TrademarkBackBitmap = new Bitmap(this.Width, this.Height);
        }

        private void TrademarkForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void TrademarkForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShowApp.NowTrademarkIsShow = false;
        }

        #region " Show Trademark "

        public void ThreadShow()
        {
            Thread tmpThread = new Thread(this.ThreadShowTrademarkVoid);
            tmpThread.IsBackground = true;
            tmpThread.Start();
        }


        private void ThreadShowTrademarkVoid()
        {
            if (showTrademarkFlag)
            {
                return;
            }
            try
            {
                showTrademarkFlag = true;

                if (PlayApp.ExecutePlayList != null)
                {

                    foreach (TrademarkTempleteItem mtItem in PlayApp.ExecutePlayList.TrademarkTempleteItemList)
                    {
                        if (mtItem.TempleteState != TempleteStateType.Stop)
                        {
                            if (mtItem.TempleteState == TempleteStateType.Wait)
                            {
                                mtItem.ExecuteStart();
                            }
                            mtItem.ShowCurrent(this);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("TrademarkForm", "ThreadShowTrademarkVoid", ex);
            }
            finally
            {
                showTrademarkFlag = false;
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="mtItem"></param>

        private void CloseTrademark()
        {
            try
            {
                ShowApp.NowTrademarkIsShow = false;
                this.tmrShow.Stop();

                foreach (TrademarkTempleteItem mtItem in PlayApp.ExecutePlayList.TrademarkTempleteItemList)
                {
                    mtItem.ExecuteRefresh();
                }

                Control objControl = this;

                List<string> removeControlName = new List<string> { };
                foreach (Control con in objControl.Controls)
                {
                    if (con.Name.Length > Constants.LabelNameHead.Length && Utility.GetLeftString(con.Name, Constants.LabelNameHead.Length) == Constants.LabelNameHead)
                    {
                        con.Visible = false;
                        removeControlName.Add(con.Name);
                    }
                }
                foreach (string controlName in removeControlName)
                {
                    objControl.Controls.RemoveByKey(controlName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SetMainForm(MainForm mainForm)
        {
            parentForm = mainForm;
        }


        #endregion

        private void TrademarkForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DrawTrademark dmItem in ShowApp.DrawTrademarkList)
                {
                    dmItem.SetParentSize(this.Width, this.Height);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("TrademarkForm", "TrademarkForm_SizeChanged", ex);
            }
        }


    }
}
