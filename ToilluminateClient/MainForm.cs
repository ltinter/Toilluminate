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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            nextPicture = this.picImage1;
            nowPicture = this.picImage2;
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
        #region " public void "
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
            }
        }
        #endregion

        #region " private void "

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
                myToolTip.SetToolTip(nextPicture, "这是一张图片");  //当鼠标在图片上的时候，显示图片的信息  
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
                myToolTip.SetToolTip(nextPicture, "这是一张图片");  //当鼠标在图片上的时候，显示图片的信息  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region " public function "
        #endregion

        #region " private function "
        #endregion

        #endregion

    }
}
