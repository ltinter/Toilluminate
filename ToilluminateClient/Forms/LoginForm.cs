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

namespace ToilluminateClient
{
    public partial class LoginForm : Form
    {
        private int number = 0;
        public LoginForm()
        {
            InitializeComponent();

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            this.BackColor = ImageApp.BackClearColor;
            this.TransparencyKey = ImageApp.BackClearColor;
            

                this.panel1.BackColor = ImageApp.BackClearColor;

            this.panel2.BackColor = ImageApp.BackClearColor;
            timer2.Stop();

        }
        /// <summary>
        /// 画图片
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        private void MyDrawMessage(Control drawingBoard)
        {
            Graphics g = null;
            Graphics gBmpBack = null;
            try
            {
                int newW = drawingBoard.Width;
                int newH = drawingBoard.Height;
                int srcW = drawingBoard.Width;
                int srcH = drawingBoard.Height;

                PlayApp.MessageBackBitmap = new Bitmap(drawingBoard.Width, drawingBoard.Height);
                gBmpBack = Graphics.FromImage(PlayApp.MessageBackBitmap);
                gBmpBack.Clear(ImageApp.BackClearColor);

                
                if (number == 0)
                {
                    gBmpBack.DrawString("AAAAABBBBBBCCCCCCDDDDD", new Font("MS PGothic", 18), new SolidBrush(Color.Red), 30, 60);
                }
                else if (number == 1)
                {
                    gBmpBack.DrawString("11111111122222222223333333335", new Font("MS PGothic", 18), new SolidBrush(Color.Green), 30, 60);
                }


                // g = drawingBoard.CreateGraphics();

                //g.DrawImage(PlayApp.MessageBackBitmap, 0, 0);
                drawingBoard.BackgroundImage = PlayApp.MessageBackBitmap;


            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("ImageApp", "MyDrawMessage", ex);
            }
            finally
            {
                PlayApp.DrawMessageFlag = false;
                if (null != g)
                {
                    g.Dispose();
                }
                if (null != gBmpBack)
                {
                    gBmpBack.Dispose();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
  
                this.MyDrawMessage(this.panel1);
                this.MyDrawMessage(this.panel2);
            
    

            number++;

            timer2.Start();
            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

            this.panel1.Left -= 10;
            if ((this.panel1.Left + this.panel1.Width) < 0)
            {
                //this.MyDrawMessage(this.panel1);
                this.panel1.Left = this.Width;
            }

            timer2.Start();
        }
    }
}
