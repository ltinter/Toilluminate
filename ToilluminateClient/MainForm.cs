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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ToilluminateClient
{
    public partial class MainForm : Form
    {

        #region " variable "


        private List<string> playList = new List<string>();

        private int playIndex = -1;

        private string playItem = string.Empty;

        #region " image "

        private int imageOpacity = 0;

        private ImageShowStyle imageShowStyle = ImageShowStyle.None;

        private PictureBox nowPicture;
        private PictureBox nextPicture;

        private List<string> imageList = new List<string>();

        #endregion

        #endregion

        #region " propert "
        #endregion

        public MainForm()
        {
            this.playList.Add("image");
            this.playList.Add("message");
            this.playList.Add("media");
            //this.playList.Add("web");
            //this.playList.Add("pdf");

            imageList.Add(@"C:\C_Works\Images\AAA.jpg");
            imageList.Add(@"C:\C_Works\Images\BBB.jpg");
            imageList.Add(@"C:\C_Works\Images\CCC.jpg");

            InitializeComponent();

            this.ControlsInit();
        }

        #region " event "


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.tmrAll.Start();
        }

        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
            MaxShowThis();
        }

        private void panle_DoubleClick(object sender, EventArgs e)
        {
            MaxShowThis();
        }
        private void control_DoubleClick(object sender, EventArgs e)
        {
            MaxShowThis();
        }
        
        private void tmrAll_Tick(object sender, EventArgs e)
        {
            this.tmrAll.Stop();

            if (this.playList.Count > 0)
            {
                playIndex++;
                if (playIndex >= this.playList.Count)
                {
                    playIndex = 0;
                }
                playItem = this.playList[playIndex];

                if (playItem == "image")
                {
                    this.tmrImage.Start();
                }

            }


            this.tmrAll.Start();
        }

        private void tmrImage_Tick(object sender, EventArgs e)
        {
            if (this.picImage1.Visible)
            {
                nowPicture = picImage1;
                nextPicture = picImage2;
            }else
            {
                nowPicture = picImage2;
                nextPicture = picImage1;
            }


            if (imageShowStyle== ImageShowStyle.None)
            {
                ShowImage("");

                nextPicture.SendToBack();
                nextPicture.Visible = true;
                nowPicture.Visible = false;
            }

            //imageOpacity += 1;
            //this.Opacity = ((double)imageOpacity) / 10;

                //if (this.Opacity == 10)
                //{
                //    tmrImage.Enabled = false;
                //}

                //            picImage1.
        }
        #endregion

        #region " void and function"
        #region " public "
        public void MaxShowThis()
        {
            bool max = false;
            if (this.FormBorderStyle == FormBorderStyle.None
                && this.WindowState == FormWindowState.Maximized)
            {
                max = true;
            }
            if (max)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                this.TopMost = false;
            }
            else
            {
                //如果不把Border设为None,则无法隐藏Windows的开始任务栏
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                //如果不允许运行其他程序,则可设为True,屏蔽其他窗体的显示
                //但必须确保自身所有的窗体的TopMost除了子窗体外都要设置为true,否则也同样会被屏蔽
                this.TopMost = true;

                //隐藏鼠标
                Cursor.Hide();
            }
        }


        public void ControlsInit()
        {
            try
            {
                
                //播放器全屏
                Rectangle screenSize = System.Windows.Forms.SystemInformation.VirtualScreen;//获取屏幕的宽和高
                //this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowImage.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);
                this.picImage1.Location = new System.Drawing.Point(0, 0);
                this.picImage1.Size = new System.Drawing.Size(pnlShowImage.Width, pnlShowImage.Height);
                this.picImage2.Location = new System.Drawing.Point(0, 0);
                this.picImage2.Size = new System.Drawing.Size(pnlShowImage.Width, pnlShowImage.Height);
                nextPicture = this.picImage1;
                nowPicture = this.picImage2;

                //this.pnlShowMessage.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowMessage.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);

                //this.pnlShowMediaWMP.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowMediaWMP.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);
                this.axWMP.Location = new System.Drawing.Point(0, 0);
                this.axWMP.Size = new System.Drawing.Size(pnlShowMediaWMP.Width, pnlShowMediaWMP.Height);
                //播放器样式
                this.axWMP.uiMode = "none";
                //禁用播放器右键菜单
                this.axWMP.enableContextMenu = false;


                //this.pnlShowMediaVLC.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowMediaVLC.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);


                //this.pnlShowWeb.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowWeb.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);

                //this.pnlShowPDF.Location = new System.Drawing.Point(0, 0);
                //this.pnlShowPDF.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  " public "

        #region " private "

        #region " image "
        /// <summary>
        /// 显示本地图片
        /// </summary>
        /// <param name="imageFile"></param>
        private void ShowImage(string imageFile)
        {
            try
            {
                //动态添加图片 
                imageFile = @"C:\Users\Administrator\Pictures\pic.jpg";

                //显示本地图片
                nextPicture.Image = Image.FromFile(imageFile);

                nextPicture.SizeMode = PictureBoxSizeMode.StretchImage;  //是图片的大小适应控件PictureBox的大小  
                tipBox.SetToolTip(nextPicture, "这是一张图片");  //当鼠标在图片上的时候，显示图片的信息  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示网络图片
        /// </summary>
        /// <param name="imageUrl"></param>
        private void ShowImageLocation(string imageUrl)
        {
            try
            {
                imageUrl = @"C:\Users\Administrator\Pictures\pic.jpg";

                //显示网络图片
                nextPicture.ImageLocation = imageUrl;

                nextPicture.SizeMode = PictureBoxSizeMode.StretchImage;  //是图片的大小适应控件PictureBox的大小  
                tipBox.SetToolTip(nextPicture, "这是一张图片");  //当鼠标在图片上的时候，显示图片的信息  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region " windows media play "
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaFile"></param>
        private void ShowMediaWMP(string mediaFile)
        {
            ShowMediaWMP(mediaFile, WMPPlayState.wmppsPlaying, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaFile"></param>
        private void ShowMediaWMP(string mediaFile, WMPPlayState state, double position)
        {
            try
            {
                this.axWMP.URL = mediaFile;
                this.axWMP.Ctlcontrols.currentPosition = position;
                if (state == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    this.axWMP.Ctlcontrols.play();
                }
                else if (state == WMPLib.WMPPlayState.wmppsStopped)
                {
                    this.axWMP.Ctlcontrols.stop();
                }
                else if (state == WMPLib.WMPPlayState.wmppsPaused)
                {
                    this.axWMP.Ctlcontrols.pause();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IWMPPlaylist WMPList;//创建播放列表
        private bool IsLoop = true;//视频是否循环

        //播放状态改变时发生
        private void axWMP_StatusChange(object sender, EventArgs e)
        {
            //判断视频是否已停止播放  
            if ((int)this.axWMP.playState == 1)
            {
                //停顿2秒钟再重新播放  
                //System.Threading.Thread.Sleep(2000);
                //重新播放  
                //this.axWMP.Ctlcontrols.play();
            }
        }
        //播放
        public void WMPStart()
        {
            this.axWMP.Ctlcontrols.play();
        }
        //列表播放
        public void WMPListStart()
        {
            WMPPlayList();//重新获取播放列表
            this.axWMP.Ctlcontrols.play();
        }
        //暂停
        public void WMPPause()
        {
            this.axWMP.Ctlcontrols.pause();
        }
        //重播
        public void WMPReplay()
        {
            WMPStop();
            WMPStart();
        }
        //列表重播
        public void WMPListReplay()
        {
            this.axWMP.currentPlaylist = WMPList;//重新载入播放列表
            WMPStart();
        }
        //停止播放
        public void WMPStop()
        {
            //this.axWMP.currentPlaylist.clear();//清除列表
            this.axWMP.Ctlcontrols.stop();
        }
        //视频静音
        public void WMPMute(bool t)
        {
            this.axWMP.settings.mute = t;
        }
        //播放下一个视频
        public void WMPNext()
        {
            //判断当前所播放的视频是否是列表的最后一个
            if (this.axWMP.currentMedia.name == this.axWMP.currentPlaylist.Item[this.axWMP.currentPlaylist.count - 1].name)
            {
            }
            else
            {
                this.axWMP.Ctlcontrols.next();//播放下一个
            }
        }
        //播放上一个媒体
        public void WMPPrevious()
        {  //判断当前所播放的视频是否是列表的第一个
            if (this.axWMP.currentMedia.name == this.axWMP.currentPlaylist.Item[0].name)
            {
            }
            else
            {
                this.axWMP.Ctlcontrols.previous();//播放上一个
            }
        }

        //获取播放类表及初始化
        public void WMPPlayList()
        {
            WMPList = this.axWMP.playlistCollection.newPlaylist("one");//创建播放列表
            string path = @".\data\video";//媒体路径
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileSystemInfo fsi in dir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    WMPList.appendItem(this.axWMP.newMedia(fi.FullName));
                }
            }
            this.axWMP.currentPlaylist = WMPList;//查找到视频、播放类表
            this.axWMP.settings.setMode("loop", IsLoop);//设置类表循环播放
        }

        #endregion
        #endregion " private "

        #endregion " void and function"
    }
}
