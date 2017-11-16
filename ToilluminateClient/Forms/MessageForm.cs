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
    public partial class MessageForm : Form
    {
        private bool showMessageFlag = false;

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
        }

        public void tmrMessage_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrMessage.Stop();
                if (PlayApp.ExecutePlayList.PlayListState == PlayListStateType.Stop)
                {
                    CloseMessage();
                    return;
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MessageForm", "tmrMessage_Tick", ex);
            }
            try
            {
                Graphics g = null;
                try
                {
                    Control objControl = this;

                    g = objControl.CreateGraphics();

                    ImageApp.MyDrawMessage(this);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (null != g)
                    {
                        g.Dispose();
                    }
                }


                System.Threading.Thread.Sleep(10);
                this.tmrMessage.Start();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MessageForm", "tmrMessage_Tick", ex);
            }
            finally
            {
                this.tmrMessage.Start();
            }
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
        }
        private void MessageForm_Shown(object sender, EventArgs e)
        {
        }

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void MessageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PlayApp.NowMediaIsShow = false;
        }

        #region " Show Message "

        public void ThreadShowMessage()
        {
            Thread tmpThread = new Thread(this.ThreadShowMessageVoid);
            tmpThread.IsBackground = true;
            tmpThread.Start();
        }


        private void ThreadShowMessageVoid()
        {
            try
            {
                if (showMessageFlag == false)
                {
                    showMessageFlag = true;

                    try
                    {
                        if (PlayApp.ExecutePlayList != null)
                        {
                            foreach (MessageTempleteItem mtItem in PlayApp.ExecutePlayList.MessageTempleteItemList)
                            {
                                mtItem.ExecuteStart();
                                if (mtItem.TempleteState != TempleteStateType.Stop)
                                {
                                    if (mtItem.IntervalSecond == 0)
                                    {
                                        while (mtItem.CurrentIsChanged())
                                        {
                                            ShowMessage(mtItem);
                                        }
                                    }
                                    else
                                    {
                                        if (mtItem.CurrentIsChanged())
                                        {
                                            ShowMessage(mtItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        showMessageFlag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MessageForm", "ThreadShowMessageVoid", ex);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="mtItem"></param>
        private void ShowMessage(MessageTempleteItem mtItem)
        {
            try
            {

                mtItem.ShowCurrent(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示");
            }
        }
        private void CloseMessage()
        {
            try
            {
                PlayApp.NowMediaIsShow = false;
                this.tmrMessage.Stop();

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


        public void SetParentForm(MainForm mainForm)
        {
            parentForm = mainForm;
        }

        #endregion


    }
}
