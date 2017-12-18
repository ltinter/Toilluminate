﻿using System;
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
    public partial class MessageForm : Form
    {
        private bool showMessageFlag = false;

        private bool showMessageEnd = false;

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

        public MessageForm()
        {
            InitializeComponent();

            this.BackColor = ImageApp.BackClearColor;
            this.TopMost = true;
            this.TransparencyKey = ImageApp.BackClearColor;

            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None;


            this.pnlMessage.Left = 0;
            this.pnlMessage.Top = 0;
            this.pnlMessage.Width = this.Width;
            this.pnlMessage.Height = this.Height;
            this.pnlMessage.BackColor = ImageApp.BackClearColor;
            this.pnlMessage.SendToBack();
            
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
                        CloseMessage();
                        return;
                    }
                }

                ThreadShow();

                ImageApp.MyDrawMessage(this.pnlMessage);
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MessageForm", "tmrMessage_Tick", ex);
            }
            finally
            {
                this.tmrShow.Start();
            }
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            this.tmrShow.Interval = 10;

        }
        private void MessageForm_Shown(object sender, EventArgs e)
        {
            ShowApp.MessageBackBitmap = new Bitmap(this.Width, this.Height);
        }

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void MessageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShowApp.NowMediaIsShow = false;
        }

        #region " Show Message "

        public void ThreadShow()
        {
            Thread tmpThread = new Thread(this.ThreadShowMessageVoid);
            tmpThread.IsBackground = true;
            tmpThread.Start();
        }


        private void ThreadShowMessageVoid()
        {
            if (showMessageFlag)
            {
                return;
            }
            try
            {
                showMessageFlag = true;

                if (PlayApp.ExecutePlayList != null)
                {

                    foreach (MessageTempleteItem mtItem in PlayApp.ExecutePlayList.MessageTempleteItemList)
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
                LogApp.OutputErrorLog("MessageForm", "ThreadShowMessageVoid", ex);
            }
            finally
            {
                showMessageFlag = false;
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="mtItem"></param>

        private void CloseMessage()
        {
            try
            {
                ShowApp.NowMediaIsShow = false;
                this.tmrShow.Stop();

                foreach (MessageTempleteItem mtItem in PlayApp.ExecutePlayList.MessageTempleteItemList)
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

        private void MessageForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DrawMessage dmItem in ShowApp.DrawMessageList)
                {
                    dmItem.SetParentSize(this.Width, this.Height);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MessageForm", "MessageForm_SizeChanged", ex);
            }
        }


    }
}
