/*
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：ToilluminateClient
 * プログラム名称：主フォーム
 * 作成日・作成者：2017/10/11  zhangpeng
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using WMPLib;

namespace ToilluminateClient
{
    public partial class MainForm : Form
    {

        #region " variable "

        private bool thisMessageVisible = false;
        private bool thisTrademarkVisible = false;



        private bool thisSetNowVisible = false;
        private bool executeTempleteFlag = false;

        private bool showImageFlag = false;
        private bool showMediaFlag = false;
        private VLCPlayer axVLCPlayer;

        private AxWMPLib.AxWindowsMediaPlayer axWMP;


        #endregion


        #region " propert "
        #endregion

        public MainForm()
        {
            InitializeComponent();

            this.GetNowVisible();

            this.ControlsInit();

        }

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

                        MaxShowThis(true);
                        break;

                    case Keys.Space:

                        MaxShowThis(false);
                        break;

                }
            }
            return false;
        }
        #endregion

        #region " event "


        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (IniFileInfo.ShowExample)
                {
                    if (PlayApp.PlayListArray.Count == 0)
                    {
                        PlayApp.DebugLoadPlayListInfo();
                        //PlayApp.ThreadLoadPlayListTimeCurrent = PlayApp.ThreadLoadPlayListTime - 2;
                    }
                }

                Thread tmpThread = new Thread(this.ThreadLoadPlayListVoid);
                tmpThread.IsBackground = true;
                tmpThread.Start();

                this.tmrPlayList.Start();

#if !DEBUG
                MaxShowThis(false);
#endif

                //商标 - 在最下层                
                this.ShowTrademarkForm();

                //字幕 - 在第二层
                this.ShowMessageForm();

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "MainForm_Load", ex);
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            #region " messageFormInstance "
            VariableInfo.ReSizeForm(this, VariableInfo.messageFormInstance);

            #endregion
            #region " trademarkFormInstance "
            VariableInfo.ReSizeForm(this, VariableInfo.trademarkFormInstance);
            #endregion
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            thisMessageVisible = true;
            thisTrademarkVisible = true;


            thisSetNowVisible = true;
            executeTempleteFlag = true;

            CloseAll();
        }

        private void tmrPlayList_Tick(object sender, EventArgs e)
        {
            this.GetNowVisible();
            try
            {
                this.tmrPlayList.Stop();

                this.ThreadLoadPlayList();

                if (PlayApp.NewPlayListExist)
                {
                    CloseAll();

                    PlayApp.ExecutePlayListStart();
                }

                if (PlayApp.PlayListArray.Count > 0)
                {
                    PlayList eplObject = PlayApp.ExecutePlayList;
                    if (eplObject == null || eplObject.PlayListState == PlayListStateType.Wait || eplObject.PlayListState == PlayListStateType.Stop)
                    {
                        if (PlayApp.CurrentPlayValid())
                        {
                            if (PlayApp.ExecutePlayListStart())
                            {
                                ShowApp.NowMessageIsRefresh = true;
                                ShowApp.NowTrademarkIsRefresh = true;
                                executeTempleteFlag = false;
                                this.tmrTemplete_Tick(null, null);
                            }
                        }
                        else
                        {
                            CloseAll();
                            return;
                        }
                    }
                    else if (eplObject.PlayListState == PlayListStateType.Last)
                    {
                        if (eplObject.LoopPlayValid)
                        {
                            eplObject.PlayStart();
                            ShowApp.NowMessageIsRefresh = true;
                            ShowApp.NowTrademarkIsRefresh = true;
                            executeTempleteFlag = false;
                            this.tmrTemplete_Tick(null, null);
                        }
                        else
                        {
                            eplObject.PlayStop();
                        }
                    }
                    else if (eplObject.PlayListState == PlayListStateType.Execute)
                    {

                    }

                }

                System.Threading.Thread.Sleep(100);
                this.tmrPlayList.Start();
            }
            catch (Exception ex)
            {
                this.tmrPlayList.Start();
                LogApp.OutputErrorLog("MainForm", "tmrPlayList_Tick", ex);
            }
        }

        private void tmrTemplete_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrTemplete.Stop();

                this.ThreadExecuteTemplete();

                this.SetNowVisible();

                System.Threading.Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "tmrTemplete_Tick", ex);
            }
            finally
            {
                this.tmrTemplete.Start();
            }
        }
        private void tmrImage_Tick(object sender, EventArgs e)
        {
            this.tmrImage.Stop();
            try
            {
                if (PlayApp.ExecutePlayList.PlayListState == PlayListStateType.Stop)
                {
                    CloseImage();
                    return;
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "tmrImage_Tick", ex);
            }
            try
            {
                this.ThreadShowImage();

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "tmrImage_Tick", ex);
            }
            finally
            {
                this.tmrImage.Start();

            }
        }


        private void tmrMedia_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrMedia.Stop();

                this.ThreadShowMedia();

                this.tmrMedia.Start();
            }
            catch (Exception ex)
            {
                this.tmrMedia.Start();
                LogApp.OutputErrorLog("MainForm", "tmrMedia_Tick", ex);
            }
        }



        #endregion


        #region " void and function"
        #region " public "
        public void MaxShowThis(bool isESC)
        {
            bool max = false;
            if (this.FormBorderStyle == FormBorderStyle.None
                && this.WindowState == FormWindowState.Maximized)
            {
                max = true;
            }
            if (max)
            {
                if (this.FormBorderStyle != FormBorderStyle.Sizable)
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = false;

                    Cursor.Show();

                }
            }
            else
            {
                if (isESC)
                {
                    return;
                }
                if (this.FormBorderStyle != FormBorderStyle.None)
                {


                    //如果不把Border设为None,则无法隐藏Windows的开始任务栏
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    this.TopMost = false;

                    //隐藏鼠标
                    Cursor.Hide();

                }
            }


            VariableInfo.ReSizeForm(this, VariableInfo.messageFormInstance);

            VariableInfo.ReSizeForm(this, VariableInfo.trademarkFormInstance);

        }


        public void ControlsInit()
        {
            try
            {
                this.DoubleBuffered = true;
                this.BackColor = ImageApp.BackClearColor;

                //播放器全屏
                ////获取屏幕的宽和高
                //Rectangle screenSize = System.Windows.Forms.SystemInformation.VirtualScreen;
                //获取Form的宽和高
                Size screenSize = this.Size;
                this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
                this.pnlShowImage.Size = new System.Drawing.Size(screenSize.Width - 15, screenSize.Height - 38);
                this.pnlShowImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                                                                           | System.Windows.Forms.AnchorStyles.Bottom)
                                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                                           | System.Windows.Forms.AnchorStyles.Right)));
                this.pnlShowImage.BackColor = ImageApp.BackClearColor;

                this.pnlShowImage.Visible = false;

                this.pnlShowMedia.Location = new System.Drawing.Point(0, 0);
                this.pnlShowMedia.Size = new System.Drawing.Size(screenSize.Width - 15, screenSize.Height - 38);
                this.pnlShowMedia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                                                                           | System.Windows.Forms.AnchorStyles.Bottom)
                                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                                           | System.Windows.Forms.AnchorStyles.Right)));
                this.pnlShowMedia.BackColor = ImageApp.BackClearColor;
                this.pnlShowMedia.Visible = false;


                this.picImage.Location = new System.Drawing.Point(0, 0);
                this.picImage.Size = new System.Drawing.Size(pnlShowImage.Width, pnlShowImage.Height);
                //是图片的大小适应控件PictureBox的大小 
                this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
                this.picImage.BackColor = ImageApp.BackClearColor;
                this.picImage.Visible = true;
                this.picImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                                                                            | System.Windows.Forms.AnchorStyles.Bottom)
                                                                            | System.Windows.Forms.AnchorStyles.Left)
                                                                            | System.Windows.Forms.AnchorStyles.Right)));

                if (IniFileInfo.MediaDevice == MediaDeivceType.WMP)
                {

                    #region "WMP"

                    // 
                    // axWMP
                    // 
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                    this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
                    this.pnlShowMedia.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
                    this.pnlShowMedia.Controls.Add(this.axWMP);

                    this.axWMP.Enabled = true;
                    this.axWMP.TabIndex = 1;
                    this.axWMP.Name = "axWMP";
                    this.axWMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWMP.OcxState")));
                    this.pnlShowMedia.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();

                    try
                    {
                        //播放器样式
                        this.axWMP.uiMode = "none";
                        //禁用播放器右键菜单
                        this.axWMP.enableContextMenu = false;
                    }
                    catch (Exception ex)
                    {
                        LogApp.OutputErrorLog("MainForm", "ControlsInit", ex);
                    }

                    this.axWMP.Location = new System.Drawing.Point(0, 0);
                    this.axWMP.PlayStateChange += this.AxWMP_PlayStateChange;

                    this.axWMP.settings.autoStart = false; //是否自动播放
                    this.axWMP.Visible = true;
                    this.axWMP.settings.volume = 100;
                    this.axWMP.Size = new System.Drawing.Size(pnlShowImage.Width, pnlShowImage.Height);

                    this.axWMP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                                                                                | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                | System.Windows.Forms.AnchorStyles.Left)
                                                                                | System.Windows.Forms.AnchorStyles.Right)));

                    #endregion

                }
                else if (IniFileInfo.MediaDevice == MediaDeivceType.VLC)
                {
                    #region "VLC"

                    // 
                    // axVLC
                    // 
                    this.axVLCPlayer = new ToilluminateClient.VLCPlayer();
                    this.axVLCPlayer.OnStopEvent += AxVLCPlayer_OnStopEvent;



                    #endregion
                }

                this.tmrPlayList.Interval = 500;
                this.tmrTemplete.Interval = 100;
                this.tmrImage.Interval = 100;
                this.tmrMedia.Interval = 100;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "ControlsInit", ex);
            }
        }

        private void AxVLCPlayer_OnStopEvent(object sender, EventArgs e)
        {
            try
            {
                this.showMediaFlag = true;

                MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;
                mtItem.ExecuteStop();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "AxVLCPlayer_OnStopEvent", ex);
            }
            finally
            {
                this.showMediaFlag = false;
            }
        }

        private void AxWMP_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            //判断视频是否已停止播放  
            if (this.axWMP.playState == WMPPlayState.wmppsStopped || this.axWMP.playState == WMPPlayState.wmppsUndefined)
            {
                CloseMedia();
            }
        }

        #endregion  " public "

        #region " private "
        private void CloseAll()
        {
            this.CloseImage();
            this.CloseMedia();
            this.CloseMessage();
            this.CloseTrademark();
            GetNowVisible();
        }

        #region " image "
        private void CloseImage()
        {
            try
            {
                if (ShowApp.NowImageIsShow)
                {
                    ShowApp.NowImageIsShow = false;

                    this.tmrImage.Stop();

                    if (this.picImage.Image != null)
                    {
                        this.picImage.Image.Dispose();
                    }
                }

                while (this.pnlShowImage.Visible)
                {
                    try
                    { this.pnlShowImage.Visible = false; }
                    catch { }

                    if (this.pnlShowImage.Visible)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "CloseImage", ex);
            }
        }



        #endregion

        #region " windows media play "
        private bool MediaIsReady()
        {
            if (IniFileInfo.MediaDevice == MediaDeivceType.WMP)
            {
                if (this.axWMP.playState != WMPPlayState.wmppsPlaying && ShowApp.NowMediaIsShow)
                {
                    return true;
                }
            }
            else if (IniFileInfo.MediaDevice == MediaDeivceType.VLC)
            {
                if (this.axVLCPlayer.IsPlaying == false && ShowApp.NowMediaIsShow)
                {
                    return true;
                }
            }
            return false;
        }

        private void MediaStop()
        {
            try
            {
                if (IniFileInfo.MediaDevice == MediaDeivceType.WMP)
                {
                    this.axWMP.Ctlcontrols.stop();
                }

                else if (IniFileInfo.MediaDevice == MediaDeivceType.VLC)
                {
                    this.axVLCPlayer.Stop();
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "MediaStop", ex);
            }
        }

        /*
        [基本属性] 　 
            URL:String; 指定媒体位置，本机或网络地址 
            uiMode:String; 播放器界面模式，可为 full,mini,none,invisible 
            playState:integer; 播放状态，1=停止，2=暂停，3=播放，6=正在缓冲，9=正在连接，10=准备就绪 
            enableContextMenu:Boolean; 启用/禁用右键菜单 
            fullScreen:boolean; 是否全屏显示 
        [controls] wmp.controls //播放器基本控制 
            controls.play; 播放 
            controls.pause; 暂停 
            controls.stop; 停止 
            controls.currentPosition:double; 当前进度 
            controls.currentPositionString:string; 当前进度，字符串格式。如“00:23” 
            controls.fastForward; 快进 
            controls.fastReverse; 快退 
            controls.next; 下一曲 
            controls.previous; 上一曲 
        [settings] wmp.settings //播放器基本设置 
            settings.volume:integer; 音量，0-100 
            settings.autoStart:Boolean; 是否自动播放 
            settings.mute:Boolean; 是否静音 
            settings.playCount:integer; 播放次数 
            [currentMedia] wmp.currentMedia //当前媒体属性 
            currentMedia.duration:double; 媒体总长度 
            currentMedia.durationString:string; 媒体总长度，字符串格式。如“03:24” 
            currentMedia.getItemInfo(const string); 获取当前媒体信息"Title"=媒体标题，"Author"=艺术家，"Copyright"=版权信息，"Description"=媒体内容描述，"Duration"=持续时间（秒），"FileSize"=文件大小，"FileType"=文件类型，"sourceURL"=原始地址 
            currentMedia.setItemInfo(const string); 通过属性名设置媒体信息 
            currentMedia.name:string; 同 currentMedia.getItemInfo("Title") 
        [currentPlaylist] wmp.currentPlaylist //当前播放列表属性 
            currentPlaylist.count:integer; 当前播放列表所包含媒体数 
            currentPlaylist.Item[integer]; 获取或设置指定项目媒体信息，其子属性同wmp.currentMedia 
            AxWindowsMediaPlayer控件的属性
            MediaPlayer1.Play　　　　　　　　　　播放  
            MediaPlayer1.Stop　　　　　　　　　　停止  
            MediaPlayer1.Pause　　　　　　　　　 暂停  
            MediaPlayer1.PlayCount　　　　　　　　文件播放次数  
            MediaPlayer1.AutoRewind　　　　　　　是否循环播放  
            MediaPlayer1.Balance　　　　　　　　　声道  
            MediaPlayer1.Volume　　　　　　　　　音量  
            MediaPlayer1.Mute　　　　　　　　　　静音  
            MediaPlayer1.EnableContextMenu　　　　是否允许在控件上点击鼠标右键时弹出快捷菜单  
            MediaPlayer1.AnimationAtStart　　　　是否在播放前先播放动画  
            MediaPlayer1.ShowControls　　　　　　是否显示控件工具栏  
            MediaPlayer1.ShowAudioControls　　　　是否显示声音控制按钮  
            MediaPlayer1.ShowDisplay　　　　　　　是否显示数据文件的相关信息  
            MediaPlayer1.ShowGotoBar　　　　　　　是否显示Goto栏  
            MediaPlayer1.ShowPositionControls　　是否显示位置调节按钮  
            MediaPlayer1.ShowStatusBar　　　　　　是否显示状态栏  
            MediaPlayer1.ShowTracker　　　　　　　是否显示进度条  
            MediaPlayer1.FastForward　　　　　　　快进  
            MediaPlayer1.FastReverse　　　　　　　快退  
            MediaPlayer1.Rate　　　　　　　　　　快进／快退速率  
            MediaPlayer1.AllowChangeDisplaySize　是否允许自由设置播放图象大小  
            MediaPlayer1.DisplaySize　　　　　　　设置播放图象大小  
　　　　            1-MpDefaultSize　　　　　　　　　原始大小  
　　　　            2-MpHalfSize　　　　　　　　　　 原始大小的一半  
　　　　            3-MpDoubleSize　　　　　　　　　 原始大小的两倍  
　　　　            4-MpFullScreen　　　　　　　　　 全屏  
　　　　            5-MpOneSixteenthScreen　　　　　 屏幕大小的1/16  
　　　　            6-MpOneFourthScreen　　　　　　　屏幕大小的1/4  
　　　　            7-MpOneHalfScreen　　　　　　　　屏幕大小的1/2  
            MediaPlayer1.ClickToPlay　　　　　　　是否允许单击播放窗口启动Media Player 
         
         */


        private void CloseMedia()
        {
            try
            {
                this.MediaStop();

                if (ShowApp.NowMediaIsShow)
                {
                    ShowApp.NowMediaIsShow = false;

                    MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;
                    mtItem.ExecuteStop();
                }

                while (this.pnlShowMedia.Visible)
                {
                    try
                    { this.pnlShowMedia.Visible = false; }
                    catch { }

                    if (this.pnlShowMedia.Visible)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "CloseMedia", ex);
            }
        }


        #endregion

        #region" message "
        private void CloseMessage()
        {
            try
            {
                ShowApp.NowMessageIsShow = false;
                ShowApp.DrawMessageList.Clear();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "CloseMessage", ex);
            }
        }

        #endregion
        #region" trademark "
        private void CloseTrademark()
        {
            try
            {
                ShowApp.NowTrademarkIsShow = false;
                ShowApp.DrawTrademarkList.Clear();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "CloseTrademark", ex);
            }
        }

        #endregion


        #region " visible "
        private void GetNowVisible()
        {

            thisMessageVisible = ShowApp.NowMessageIsShow;
            thisTrademarkVisible = ShowApp.NowTrademarkIsShow;
        }


        private void SetNowVisible()
        {
            if (thisSetNowVisible && executeTempleteFlag)
            {
                return;
            }
            try
            {
                thisSetNowVisible = true;

                if (ShowApp.NextShowTempleteItemType != ShowApp.NowShowTempleteItemType)
                {
                    ShowApp.NowShowTempleteItemType = ShowApp.NextShowTempleteItemType;

                    #region " Image "
                    if (ShowApp.NowShowTempleteItemType == TempleteItemType.Image)
                    {
                        this.pnlShowImage.Visible = true;
                        this.ThreadShowImage();
                    }
                    else
                    {
                        this.CloseImage();
                    }

                    #endregion


                    #region " Media "
                    if (ShowApp.NowShowTempleteItemType == TempleteItemType.Media)
                    {
                        ShowApp.DrawBitmap = ImageApp.GetNewBitmap(this.picImage.Size);

                        this.pnlShowMedia.Visible = true;
                        System.Threading.Thread.Sleep(10);
                        this.ThreadShowMedia();
                    }
                    else
                    {
                        this.CloseMedia();
                    }

                    #endregion
                }

                #region "open timer"
                if (ShowApp.NowShowTempleteItemType == TempleteItemType.Image)
                {
                    ShowApp.NowImageIsShow = true;
                    if (this.tmrImage.Enabled == false)
                    {
                        this.tmrImage_Tick(null, null);
                    }
                }
                else if (ShowApp.NowShowTempleteItemType == TempleteItemType.Media)
                {
                    ShowApp.NowMediaIsShow = true;
                    if (this.tmrMedia.Enabled == false)
                    {
                        this.tmrMedia_Tick(null, null);
                    }
                }
                #endregion


                #region " Message "
                if (ShowApp.NowMessageIsRefresh)
                {
                    ShowApp.DrawMessageList.Clear();
                    if (VariableInfo.messageFormInstance != null && VariableInfo.messageFormInstance.IsDisposed == false)
                    {
                        VariableInfo.messageFormInstance.ThreadShow();
                        VariableInfo.messageFormInstance.tmrShow_Tick(null, null);
                    }

                    ShowApp.NowMessageIsRefresh = false;
                }

                if (ShowApp.NowMessageIsShow != thisMessageVisible)
                {
                    ShowApp.NowMessageIsShow = thisMessageVisible;
                    if (ShowApp.NowMessageIsShow)
                    {
                        this.ShowMessageForm();
                    }
                    else
                    {
                        this.CloseMessage();
                    }
                }

                #endregion


                #region " Trademark "

                if (ShowApp.NowTrademarkIsRefresh)
                {
                    ShowApp.DrawTrademarkList.Clear();
                    if (VariableInfo.trademarkFormInstance != null && VariableInfo.trademarkFormInstance.IsDisposed == false)
                    {
                        VariableInfo.trademarkFormInstance.ThreadShow();
                        VariableInfo.trademarkFormInstance.tmrShow_Tick(null, null);
                    }

                    ShowApp.NowTrademarkIsRefresh = false;
                }

                if (ShowApp.NowTrademarkIsShow != thisTrademarkVisible)
                {
                    ShowApp.NowTrademarkIsShow = thisTrademarkVisible;
                    if (ShowApp.NowTrademarkIsShow)
                    {
                        this.ShowTrademarkForm();
                    }
                    else
                    {
                        this.CloseTrademark();
                    }
                }

                #endregion

                #region " main visible"
                try
                {
                    if (ShowApp.NowImageIsShow == false)
                    {
                        this.pnlShowImage.Visible = false;
                    }
                    else
                    {
                        this.pnlShowImage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    LogApp.OutputErrorLog("MainForm", "SetNowVisible", ex);
                }
                try
                {
                    if (ShowApp.NowMediaIsShow == false)
                    {
                        this.pnlShowMedia.Visible = false;
                    }
                    else
                    {
                        this.pnlShowMedia.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    LogApp.OutputErrorLog("MainForm", "SetNowVisible", ex);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "SetNowVisible", ex);
            }
            finally
            {
                thisSetNowVisible = false;
            }
        }



        private void ShowMessageForm()
        {
            try
            {
                if (VariableInfo.messageFormInstance == null || VariableInfo.messageFormInstance.IsDisposed)
                {
                    VariableInfo.messageFormInstance = new MessageForm();
                }

                VariableInfo.ReSizeForm(this, VariableInfo.messageFormInstance);

                VariableInfo.messageFormInstance.SetMainForm(this);
                VariableInfo.messageFormInstance.Show();
                VariableInfo.messageFormInstance.ThreadShow();
                VariableInfo.messageFormInstance.tmrShow_Tick(null, null);

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "ShowMessageForm", ex);
            }

        }
        private void ShowTrademarkForm()
        {
            try
            {
                if (VariableInfo.trademarkFormInstance == null || VariableInfo.trademarkFormInstance.IsDisposed)
                {
                    VariableInfo.trademarkFormInstance = new TrademarkForm();
                }

                VariableInfo.ReSizeForm(this, VariableInfo.trademarkFormInstance);

                VariableInfo.trademarkFormInstance.SetMainForm(this);
                VariableInfo.trademarkFormInstance.Show();
                VariableInfo.trademarkFormInstance.ThreadShow();
                VariableInfo.trademarkFormInstance.tmrShow_Tick(null, null);

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "ShowTrademarkForm", ex);
            }

        }

        #endregion " visible "



        #endregion " private "

        #endregion " void and function"

        #region " thread void "

        #region " Load PlayList "

        private void ThreadLoadPlayList()
        {
            if (PlayApp.ThreadLoadPlayListTimeCurrent >= PlayApp.ThreadLoadPlayListTime)
            {
                PlayApp.ThreadLoadPlayListTimeCurrent = 0;
            }
            else
            {
                PlayApp.ThreadLoadPlayListTimeCurrent++;
                return;
            }

            Thread tmpThread = new Thread(this.ThreadLoadPlayListVoid);
            tmpThread.IsBackground = true;
            tmpThread.Start();
        }


        private void ThreadLoadPlayListVoid()
        {
            try
            {
                if (PlayApp.NowLoadPlayList == false)
                {
                    PlayApp.LoadPlayListInfo();
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "ThreadLoadPlayListVoid", ex);
            }
        }

        #endregion



        #region " Show Templete "

        private void ThreadExecuteTemplete()
        {
            Thread tmpThread = new Thread(this.ThreadExecuteTempleteVoid);
            tmpThread.IsBackground = true;
            tmpThread.Start();
        }


        private void ThreadExecuteTempleteVoid()
        {
            if (executeTempleteFlag)
            {
                return;
            }

            try
            {
                executeTempleteFlag = true;

                if (PlayApp.ExecutePlayList != null && PlayApp.ExecutePlayList.PlayListState == PlayListStateType.Execute)
                {
                    PlayList pList = PlayApp.ExecutePlayList;

                    if (pList.CurrentTempleteValid())
                    {
                        if (pList.CurrentTempleteItem.TempleteType == TempleteItemType.Image)
                        {
                            ShowApp.NextShowTempleteItemType = TempleteItemType.Image;
                        }
                        else if (pList.CurrentTempleteItem.TempleteType == TempleteItemType.Media)
                        {
                            ShowApp.NextShowTempleteItemType = TempleteItemType.Media;
                        }

                        pList.CurrentTempleteItem.ExecuteStart();
                    }

                    if (pList.MessageTempleteItemList.Count > 0)
                    {
                        thisMessageVisible = true;
                    }
                    else
                    {
                        thisMessageVisible = false;
                    }

                    if (pList.TrademarkTempleteItemList.Count > 0)
                    {
                        thisTrademarkVisible = true;
                    }
                    else
                    {
                        thisTrademarkVisible = false;
                    }

                    if (pList.CurrentTempleteItem.CheckTempleteState() == TempleteStateType.Stop)
                    {
                        pList.CurrentTempleteItem.ExecuteStop();
                    }

                    if (pList.CheckPlayListState == PlayListStateType.Stop)
                    {
                        pList.PlayStop();
                        return;
                    }
                }
                else
                {
                    ShowApp.NextShowTempleteItemType = TempleteItemType.None;

                    thisMessageVisible = false;
                    thisTrademarkVisible = false;
                }

                executeTempleteFlag = false;
            }
            catch (Exception ex)
            {
                executeTempleteFlag = false;
                LogApp.OutputErrorLog("MainForm", "ThreadExecuteTempleteVoid", ex);
            }
        }

        #endregion

        #region " Show Image "

        private void ThreadShowImage()
        {
            if (this.pnlShowImage.Visible)
            {
                Thread tmpThread = new Thread(this.ThreadShowImageVoid);
                tmpThread.IsBackground = true;
                tmpThread.Start();
            }
        }


        private void ThreadShowImageVoid()
        {
            if (showImageFlag == false)
            {
                showImageFlag = true;

                try
                {
                    ImageTempleteItem itItem = PlayApp.ExecutePlayList.CurrentTempleteItem as ImageTempleteItem;

                    if (itItem.CurrentIsChanged())
                    {
                        LogApp.OutputProcessLog("MainForm", "ThreadShowImageVoid", string.Format("{0} - {1} - {2}", itItem.CurrentIndex, itItem.CurrentShowStyleIndex, DateTime.Now.ToLongTimeString()));

                        itItem.ShowCurrent(picImage);
                    }


                    if (itItem.TempleteState == TempleteStateType.Stop)
                    {
                        itItem.ExecuteStop();
                        CloseImage();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogApp.OutputErrorLog("MainForm", "ThreadShowImageVoid", ex);
                }
                finally
                {
                    showImageFlag = false;
                }
            }
        }

        #endregion
        #region " Show Media "

        private void ThreadShowMedia()
        {
            if (this.pnlShowMedia.Visible)
            {
                //Thread tmpThread = new Thread(this.ThreadShowMediaVoid);
                //tmpThread.IsBackground = true;
                //tmpThread.Start();
                this.ThreadShowMediaVoid();
            }
        }


        private void ThreadShowMediaVoid()
        {
            if (showMediaFlag == false)
            {
                showMediaFlag = true;

                try
                {
                    if (MediaIsReady())
                    {
                        MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;

                        if (mtItem.CurrentIsChanged())
                        {
                            if (IniFileInfo.MediaDevice == MediaDeivceType.WMP)
                            {
                                mtItem.ShowCurrent(this.axWMP, WMPLib.WMPPlayState.wmppsPlaying, 0);
                            }
                            else if (IniFileInfo.MediaDevice == MediaDeivceType.VLC)
                            {
                                mtItem.ShowCurrent(this.axVLCPlayer, this.pnlShowMedia.Handle);
                            }
                        }

                        if (mtItem.ReadaheadOverTime(IniFileInfo.MediaReadaheadTime))
                        {
                            this.CloseMedia();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogApp.OutputErrorLog("MainForm", "ThreadShowMediaVoid", ex);
                }
                finally
                {
                    showMediaFlag = false;
                }
            }
        }

        #endregion

        #endregion

    }
}
