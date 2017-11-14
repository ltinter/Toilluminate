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
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ToilluminateClient
{
    public partial class MainForm : Form
    {

        #region " variable "

        private bool pnlImageVisible = false;
        private bool pnlMediaWMPVisible = false;
        private bool pnlMessageVisible = false;
        #endregion


        #region " propert "
        #endregion

        public MainForm()
        {
            InitializeComponent();


#if DEBUG
            #region " DEBUG DATA"
            DateTime dtStart = DateTime.Now;



            PlayApp.Clear();
            PlayList pList1 = new PlayList(true, Utility.GetPlayDateTime(DateTime.Now).AddSeconds(3), 600);
            PlayApp.PlayListArray.Add(pList1);

            string[] imageFileList = new string[] { @"C:\C_Works\Images\A01.jpg", @"C:\C_Works\Images\A02.jpg", @"C:\C_Works\Images\A03.jpg", @"C:\C_Works\Images\A04.jpg", @"C:\C_Works\Images\A05.jpg"
                                            ,@"C:\C_Works\Images\A06.jpg", @"C:\C_Works\Images\A07.jpg", @"C:\C_Works\Images\A08.jpg", @"C:\C_Works\Images\A09.jpg", @"C:\C_Works\Images\A10.jpg" };
            ImageShowStyle[] imageStyleList = new ImageShowStyle[] { ImageShowStyle.Random, ImageShowStyle.TopToDown, ImageShowStyle.Random, ImageShowStyle.Flip_LR };

            ImageTempleteItem itItem11 = new ImageTempleteItem(imageFileList.ToList(), imageStyleList.ToList(), 6);
            //pList1.ad
           // pList1.TempleteItemList.Add(itItem11);


            string[] messageList = new string[] { @"hello world", @"今日は明日の全国に雨が降る。" };
            MessageShowStyle[] messageStyleList = new MessageShowStyle[] { MessageShowStyle.Down, MessageShowStyle.Random };

            MessageTempleteItem itItem21 = new MessageTempleteItem(messageList.ToList(), messageStyleList.ToList(), 0);
            pList1.MessageTempleteItemList.Add(itItem21);


            //ImageTempleteItem itItem12 = new ImageTempleteItem(imageFileList.ToList(), imageStyleList.ToList(), 4);
            //pList1.TempleteItemList.Add(itItem12);

            

            //MediaTempleteItem itItem31 = new MediaTempleteItem(@"C:\C_Works\Medias\mp01.mp4", MediaShowStyle.None);
            //pList1.TempleteItemList.Add(itItem31);

            //pItem3.TempleteItemList.Add(new TempleteItem(@"C:\C_Works\Medias\mp01.mp4", MediaShowStyle.None));
            //pItem3.TempleteItemList.Add(new TempleteItem(@"C:\C_Works\Medias\mp02.mp4", MediaShowStyle.None));
            //pItem3.TempleteItemList.Add(new TempleteItem(@"C:\C_Works\Medias\mp03.mp4", MediaShowStyle.None));
            //pItem3.TempleteItemList.Add(new TempleteItem(@"C:\C_Works\Medias\mp04.mp4", MediaShowStyle.None));
            #endregion
#endif


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
                }
            }
            return false;
        }
        #endregion

        #region " event "


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.tmrPlayList.Start();
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

        private void tmrPlayList_Tick(object sender, EventArgs e)
        {
            this.GetNowVisible();
            try
            {
                this.tmrPlayList.Stop();

                if (PlayApp.PlayListArray.Count > 0)
                {
                    if (PlayApp.CurrentIndex < 0 || PlayApp.CurrentIndex >= PlayApp.PlayListArray.Count)
                    {
                        PlayApp.CurrentIndex = 0;
                    }

                    while (PlayApp.CurrentIndex < PlayApp.PlayListArray.Count && PlayApp.CurrentPlayList().PlayListState == PlayListStateType.Stop)
                    {
                        PlayApp.CurrentIndex++;
                    }

                    if (PlayApp.CurrentIndex < PlayApp.PlayListArray.Count)
                    {
                        if (PlayApp.ExecutePlayList())
                        {
                            this.SetAllVisible(false);
                            this.tmrTemplete_Tick(null, null);
                        }
                    }
                }


                this.tmrPlayList.Start();
            }
            catch (Exception ex)
            {
                this.tmrPlayList.Start();
                VariableInfo.OutputClientLog(ex);
            }
        }

        private void tmrTemplete_Tick(object sender, EventArgs e)
        {
            this.GetNowVisible();

            bool showVisible = false;
            try
            {
                this.tmrTemplete.Stop();

                if (PlayApp.CurrentPlayValid())
                {
                    PlayList pList = PlayApp.NowPlayList;

                    if (pList.CurrentTempleteIndex < 0)
                    {
                        pList.PlayRefreshTemplete(); 
                    }
                    else if (pList.CurrentTempleteIndex >= pList.TempleteItemList.Count)
                    {
                        pList.PlayRefreshTemplete();
                        pList.PlayStop();
                    }

                    pList.PlayNextTemplete();

                    if (pList.CurrentTempleteIndex < pList.TempleteItemList.Count)
                    {
                        showVisible = true;
                    }
                    

                    if (pList.CurrentTempleteItem.TempleteType == TempleteType.Image)
                    {
                        pnlImageVisible = true;
                        (pList.CurrentTempleteItem as ImageTempleteItem).ExecuteStart();
                        this.tmrImage_Tick(null, null);

                        //pnlImageVisible = false;
                        pnlMessageVisible = false;
                        pnlMediaWMPVisible = false;
                    }
                    else if (pList.CurrentTempleteItem.TempleteType == TempleteType.Media)
                    {
                        pnlMediaWMPVisible = true;
                        this.tmrMedia_Tick(null, null);

                        pnlImageVisible = false;
                        pnlMessageVisible = false;
                        //pnlMediaWMPVisible = false;
                    }

                    if (pList.MessageTempleteItemList.Count >0)
                    {
                        pnlMessageVisible = true;
                        this.tmrMessage_Tick(null, null);
                    }

                    if (pList.PlayListState == PlayListStateType.Stop)
                    {
                        return;
                    }
                }


                if (showVisible)
                {
                    this.SetNowVisible();
                }

                

                this.tmrTemplete.Start();
            }
            catch (Exception ex)
            {
                this.tmrTemplete.Start();
                VariableInfo.OutputClientLog(ex);
            }
        }
        private void tmrImage_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrImage.Stop();

                ImageTempleteItem itItem = PlayApp.NowPlayList.CurrentTempleteItem as ImageTempleteItem;

                if (itItem.CurrentIsChanged())
                {
                    ShowImage(itItem);
                    Console.WriteLine(string.Format("{0} - {1} - {2}", itItem.CurrentIndex, itItem.CurrentShowStyleIndex, DateTime.Now.ToLongTimeString()));
                }


                if (itItem.TempleteState == TempleteStateType.Stop)
                {
                    itItem.ExecuteStop();
                    CloseImage();
                    return;
                }

                this.tmrImage.Start();
            }
            catch (Exception ex)
            {
                this.tmrImage.Start();
                VariableInfo.OutputClientLog(ex);
            }
        }


        private void tmrMedia_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrMedia.Stop();

                if (mediaIsReady())
                {
                    //PlayItem pItem = PlayApp.CurrentPlayItem();
                    //if (pItem.PlayType == ShowPlayType.Media && pItem.TempleteItemList.Count > 0)
                    //{
                    //    pItem.CurrentTempleteIndex++;
                    //    if (pItem.CurrentTempleteIndex >= pItem.TempleteItemList.Count)
                    //    {
                    //        pItem.CurrentTempleteIndex = 0;
                    //    }

                    //    //ShowMediaWMP(pItem.CurrentTempleteItem.File);
                    //}

                    //pItem.CycleNumber++;
                }

                this.tmrMedia.Start();
            }
            catch (Exception ex)
            {
                this.tmrMedia.Start();
                VariableInfo.OutputClientLog(ex);
            }
        }


        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrMessage.Stop();

                if (PlayApp.NowPlayList.PlayListState == PlayListStateType.Stop)
                {
                    CloseMessage();
                    return;
                }

                foreach (MessageTempleteItem mtItem in PlayApp.NowPlayList.MessageTempleteItemList)
                {
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


                        Debug.WriteLine(string.Format("{0} - {1} - {2}", mtItem.CurrentIndex, mtItem.CurrentShowStyleIndex, DateTime.Now.ToLongTimeString()));
                    }


                    foreach (Control con in this.Controls)
                    {
                        if (con.Name.Length > Constants.LabelNameHead.Length && Utility.GetLeftString(con.Name, Constants.LabelNameHead.Length) == Constants.LabelNameHead)
                        {
                            //Label lblMessage_tmp = (con as Label);
                            Debug.WriteLine(string.Format("{0} - {1} - {2} - {3}", con.Name, con.Text, con.Left, con.Top));
                            con.Left -= 10;
                            if (con.Left < 0)
                            {
                                con.Left = this.Width;
                            }
                        }
                    }
                }

                //System.Threading.Thread.Sleep(100);
                this.tmrMessage.Start();
            }
            catch (Exception ex)
            {
                this.tmrMessage.Start();
                VariableInfo.OutputClientLog(ex);
            }
        }

        #endregion


        #region " void and function"
        #region " public "
        public void MaxShowThis()
        {
            MaxShowThis(false);

        }
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
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                this.TopMost = false;

                Cursor.Show();
            }
            else
            {
                if (isESC)
                {
                    return;
                }

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
                ////获取屏幕的宽和高
                //Rectangle screenSize = System.Windows.Forms.SystemInformation.VirtualScreen;
                //获取Form的宽和高
                Size screenSize = this.Size;
                this.pnlShowImage.Location = new System.Drawing.Point(0, 0);
                this.pnlShowImage.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);

                this.pnlShowMediaWMP.Location = new System.Drawing.Point(0, 0);
                this.pnlShowMediaWMP.Size = new System.Drawing.Size(screenSize.Width, screenSize.Height);

                this.pnlShowImage.Visible = false;
                this.pnlShowMediaWMP.Visible = false;


                this.picImage.Location = new System.Drawing.Point(0, 0);
                this.picImage.Size = new System.Drawing.Size(pnlShowImage.Width, pnlShowImage.Height);
                //是图片的大小适应控件PictureBox的大小 
                picImage.SizeMode = PictureBoxSizeMode.StretchImage;

                this.axWMP.Location = new System.Drawing.Point(0, 0);
                this.axWMP.Size = new System.Drawing.Size(pnlShowMediaWMP.Width, pnlShowMediaWMP.Height);
                //播放器样式
                this.axWMP.uiMode = "none";
                //禁用播放器右键菜单
                this.axWMP.enableContextMenu = false;

                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  " public "

        #region " private "

        #region " image "
        private void CloseImage()
        {
            try
            {
                this.tmrImage.Stop();
                if (this.picImage.Image != null)
                {
                    this.picImage.Image.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="itItem"></param>
        private void ShowImage(ImageTempleteItem itItem)
        {
            try
            {
                itItem.ShowCurrent(picImage);
                
                //tipBox.SetToolTip(picImage, "这是一张图片");  //当鼠标在图片上的时候，显示图片的信息  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示");
            }
        }


        #endregion

        #region " windows media play "
        private bool mediaIsReady()
        {
            if (this.axWMP.playState == WMPPlayState.wmppsStopped
                || this.axWMP.playState == WMPPlayState.wmppsUndefined)
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


        private void CloseMediaWMP()
        {
            try
            {
                this.tmrMedia.Stop();
                WMPStop();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        #region" message "
        private void CloseMessage()
        {
            try
            {
                this.tmrMessage.Stop();

                List<string> removeControlName = new List<string> { };
                foreach (Control con in this.Controls)
                {
                    if (con.Name.Length > Constants.LabelNameHead.Length && Utility.GetLeftString(con.Name, Constants.LabelNameHead.Length) == Constants.LabelNameHead)
                    {
                        con.Visible = false;
                        removeControlName.Add(con.Name);
                    }
                }
                foreach (string controlName in removeControlName)
                {
                    this.Controls.RemoveByKey(controlName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        #endregion


        #region " visible "
        private void GetNowVisible()
        {
            pnlImageVisible = this.pnlShowImage.Visible;
            pnlMessageVisible = this.lblMessage.Visible;
            pnlMediaWMPVisible = this.pnlShowMediaWMP.Visible;
        }
        private void SetAllVisible(bool visible)
        {
            this.pnlShowImage.Visible = visible;
            this.pnlShowMediaWMP.Visible = visible;
            this.lblMessage.Visible = visible;
            GetNowVisible();

        }
        private void SetNowVisible()
        {
            try
            {
                if (this.pnlShowImage.Visible != pnlImageVisible)
                {
                    this.pnlShowImage.Visible = pnlImageVisible;
                    if (this.pnlShowImage.Visible == false)
                    {
                        CloseImage();
                    }
                }
                if (this.lblMessage.Visible != pnlMessageVisible)
                {
                    this.lblMessage.Visible = pnlMessageVisible;
                    if (this.lblMessage.Visible == false)
                    {
                        CloseMessage();
                    }
                }

                if (this.pnlShowMediaWMP.Visible != pnlMediaWMPVisible)
                {
                    this.pnlShowMediaWMP.Visible = pnlMediaWMPVisible;
                    if (this.pnlShowMediaWMP.Visible == false)
                    {
                        CloseMediaWMP();
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion " visible "
        #endregion " private "

        #endregion " void and function"
    }
}
