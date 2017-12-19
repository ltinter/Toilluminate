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

        private Control[] trademarkControls;
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

            trademarkControls = new Control[] { this.pnlTrademark0, this.pnlTrademark1, this.pnlTrademark2, this.pnlTrademark3, this.pnlTrademark4, this.pnlTrademark5, this.pnlTrademark6, this.pnlTrademark7, this.pnlTrademark8, this.pnlTrademark9 };

            foreach (Control pnlTrademark in trademarkControls)
            {
                pnlTrademark.Left = -10;
                pnlTrademark.Top = -10;
                pnlTrademark.Width = 1;
                pnlTrademark.Height = 1;
                pnlTrademark.BackColor = ImageApp.BackClearColor;
                pnlTrademark.SendToBack();
            }

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

                ImageApp.MyDrawTrademark(this.trademarkControls);
               
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
                    foreach (TrademarkTempleteItem ttItem in PlayApp.ExecutePlayList.TrademarkTempleteItemList)
                    {
                        if (ttItem.TempleteState != TempleteStateType.Stop)
                        {
                            if (ttItem.TempleteState == TempleteStateType.Wait)
                            {
                                ttItem.ExecuteStart();
                            }
                            ttItem.ShowCurrent(this);
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

                foreach (TrademarkTempleteItem ttItem in PlayApp.ExecutePlayList.TrademarkTempleteItemList)
                {
                    ttItem.ExecuteRefresh();
                }

                foreach (Control board in this.trademarkControls)
                {
                    if (board.BackgroundImage != null)
                    {
                        board.BackgroundImage = null;
                    }
                }
                for (int boardIndex = 0; boardIndex < ShowApp.TrademarkBackBitmaps.Count(); boardIndex++)
                {
                    if (ShowApp.TrademarkBackBitmaps[boardIndex] != null)
                    {
                        ShowApp.TrademarkBackBitmaps[boardIndex].Dispose();
                        ShowApp.TrademarkBackBitmaps[boardIndex] = null;
                    }
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
                foreach (DrawTrademark dtItem in ShowApp.DrawTrademarkList)
                {
                    dtItem.SetParentSize(this.Width, this.Height);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("TrademarkForm", "TrademarkForm_SizeChanged", ex);
            }
        }


    }
}
