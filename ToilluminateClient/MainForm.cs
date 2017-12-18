﻿/*
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

namespace ToilluminateClient
{
    public partial class MainForm : Form
    {

        #region " variable "

        //private delegate void FlushClient();//代理
        
        private bool thisImageVisible = false;
        private bool thisMediaVisible = false;
        private bool thisMessageVisible = false;
        

            private bool thisSetNowVisible = false;
        private bool executeTempleteFlag = false;

        private bool showImageFlag = false;
        private bool showMediaFlag = false;
        private VLCPlayer axVLCPlayer = new ToilluminateClient.VLCPlayer();

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


                MaxShowThis(false);


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
            thisImageVisible = true;
            thisMediaVisible = true;
            thisMessageVisible = true;


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
                    PlayList eplItem = PlayApp.ExecutePlayList;
                    if (eplItem == null || eplItem.PlayListState == PlayListStateType.Wait || eplItem.PlayListState == PlayListStateType.Stop)
                    {
                        if (PlayApp.CurrentPlayValid())
                        {
                            if (PlayApp.ExecutePlayListStart())
                            {
                                ShowApp.NowMessageIsRefresh = true;
                                this.tmrTemplete_Tick(null, null);
                            }
                        }
                        else
                        {
                            CloseAll();
                            return;
                        }
                    }
                    else if (eplItem.PlayListState == PlayListStateType.Last)
                    {
                        if (eplItem.LoopPlayValid)
                        {
                            eplItem.PlayStart();
                            ShowApp.NowMessageIsRefresh = true;
                            this.tmrTemplete_Tick(null, null);
                        }
                        else
                        {
                            eplItem.PlayStop();
                        }
                    }
                    else if (eplItem.PlayListState == PlayListStateType.Execute)
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

                               

                // 
                // axVLC
                // 
                this.axVLCPlayer.OnStopEvent += AxVLCPlayer_OnStopEvent;


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
                MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;
                mtItem.ExecuteStop();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "AxVLCPlayer_OnStopEvent", ex);
            }
        }
        
        #endregion  " public "

        #region " private "
        private void CloseAll()
        {
            this.CloseImage();
            this.CloseMedia();
            this.CloseMessage();
            GetNowVisible();
        }

        #region " image "
        private void CloseImage()
        {
            try
            {
                ShowApp.NowImageIsShow = false;

                this.tmrImage.Stop();

                if (this.picImage.Image != null)
                {
                    this.picImage.Image.Dispose();
                }

                ShowApp.DrawBitmap = ImageApp.GetNewBitmap(this.picImage.Size);

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
        private bool mediaIsReady()
        {
            if (this.axVLCPlayer.IsPlaying == false && this.pnlShowMedia.Visible)
            {
                return true;
            }
            return false;
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
                ShowApp.NowMediaIsShow = false;
                
                MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;
                mtItem.ExecuteStop();

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
                ShowApp.NowMediaIsShow = false;
                ShowApp.DrawMessageList.Clear();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "CloseMessage", ex);
            }
        }
        
        #endregion


        #region " visible "
        private void GetNowVisible()
        {
            thisImageVisible = ShowApp.NowImageIsShow;
            thisMessageVisible = ShowApp.NowMessageIsShow;
            thisMediaVisible = ShowApp.NowMediaIsShow;
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
                if (ShowApp.NowImageIsShow != thisImageVisible)
                {
                    ShowApp.NowImageIsShow = thisImageVisible;

                    if (ShowApp.NowImageIsShow)
                    {
                        PlayList pList = PlayApp.ExecutePlayList;
                        if (pList.CurrentTempleteItem.TempleteState == TempleteStateType.Wait)
                        {
                            ShowApp.DrawBitmap = ImageApp.GetNewBitmap(this.picImage.Size);
                        }
                        pList.CurrentTempleteItem.ExecuteStart();


                        this.pnlShowImage.Visible = true;
                        this.ThreadShowImage();
                        this.tmrImage_Tick(null, null);
                    }
                    else
                    {
                        CloseImage();
                    }

                }

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

                if (ShowApp.NowMediaIsShow != thisMediaVisible)
                {
                    ShowApp.NowMediaIsShow = thisMediaVisible;
                    if (ShowApp.NowMediaIsShow)
                    {
                        PlayList pList = PlayApp.ExecutePlayList;

                        if (pList.CurrentTempleteItem.TempleteState == TempleteStateType.Wait)
                        {
                            ShowApp.DrawBitmap = ImageApp.GetNewBitmap(this.picImage.Size);
                        }
                        pList.CurrentTempleteItem.ExecuteStart();

                        this.pnlShowMedia.Visible = true;
                        System.Threading.Thread.Sleep(10);
                        this.ThreadShowMedia();
                        this.tmrMedia_Tick(null, null);
                    }
                    else
                    {
                        CloseMedia();
                    }

                }

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
                LogApp.OutputErrorLog("MainForm", "ShowMessageForm", ex);
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
                            thisImageVisible = true;
                            thisMediaVisible = false;
                        }
                        else if (pList.CurrentTempleteItem.TempleteType == TempleteItemType.Media)
                        {
                            thisMediaVisible = true;
                            thisImageVisible = false;
                        }
                    }

                    if (pList.MessageTempleteItemList.Count > 0)
                    {
                        thisMessageVisible = true;
                    }
                    else
                    {
                        thisMessageVisible = false;
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
                    thisMediaVisible = false;
                    thisImageVisible = false;
                    thisMessageVisible = false;
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("MainForm", "ThreadExecuteTempleteVoid", ex);
            }
            finally
            {
                executeTempleteFlag = false;
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
                    if (mediaIsReady())
                    {
                        MediaTempleteItem mtItem = PlayApp.ExecutePlayList.CurrentTempleteItem as MediaTempleteItem;
                        
                        if (mtItem.CurrentIsChanged())
                        {
                            axVLCPlayer.SetRenderWindow(this.pnlShowMedia.Handle);
                            axVLCPlayer.LoadFile(mtItem.CurrentFile);//


                            axVLCPlayer.Play();//播放
                        }

                        if (mtItem.ReadaheadOverTime(ShowApp.MediaReadaheadTime))
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
