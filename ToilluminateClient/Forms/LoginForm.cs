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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ToilluminateClient
{
    public partial class LoginForm : Form
    {


        private VLCPlayer axVLCPlayer = new ToilluminateClient.VLCPlayer();
        public LoginForm()
        {
                     InitializeComponent();
            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string file = @"C:\Users\zhangpeng\Source\Repos\Toilluminate\ToilluminateClient\bin\Debug\Temp\Medias\Example01mp4.mp4";
            VLCPlayer vlcPlayer = new VLCPlayer();
            vlcPlayer.SetRenderWindow(this.Handle);
            vlcPlayer.LoadFile(file);//播放 参数1rtsp地址，参数2 窗体句柄

            //vlcPlayer.SnapShot(file);//抓图，参数1为存储路径

            vlcPlayer.Play();//停止播放

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
