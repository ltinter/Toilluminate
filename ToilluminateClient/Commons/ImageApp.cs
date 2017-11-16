using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Net;

namespace ToilluminateClient
{
    public static class ImageApp
    {
        
        /// <summary>
        /// 初始为全灰色
        /// </summary>
        public static Color BackClearColor = Color.Black;

        public static int SepTime=10;

        #region " 画图片 "
        /// <summary>
        /// 画图片
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        public static void MyDrawImage(Graphics g, Bitmap bmp, int left, int top)
        {
            g.DrawImage(bmp, left, top);
            MyDrawMessage(VariableInfo.messageFormInstance);
        }
        public static void MyDrawImage(Graphics g, Bitmap bmp, Rectangle DestRect, Rectangle SrcRect, GraphicsUnit gUnit)
        {
            g.DrawImage(bmp, DestRect, SrcRect, gUnit);
            MyDrawMessage(VariableInfo.messageFormInstance);
        }
        public static void MyDrawImage(Graphics g, Bitmap bmp, Rectangle DestRect, int srcX, int srcY, int srcWidth, int srcHeigth, GraphicsUnit gUnit, ImageAttributes iAttributes)
        {
            g.DrawImage(bmp, DestRect, srcX, srcY, srcWidth, srcHeigth, gUnit, iAttributes);
            MyDrawMessage(VariableInfo.messageFormInstance);
        }
        public static void MyDrawMessage(MessageForm messageForm)
        {
            if (PlayApp.DrawMessageFlag)
            {
                return;
            }

            Graphics g = null;
            try
            {
                LogApp.OutputProcessLog("ImageApp", "MyDrawMessage", "All Message");
                PlayApp.DrawMessageFlag = true;
                g = messageForm.CreateGraphics();
                foreach (DrawMessage dmItem in PlayApp.DrawMessageList)
                {
                    g.DrawString(dmItem.Message, dmItem.Font, new SolidBrush(BackClearColor), dmItem.Left, dmItem.Top);
                    dmItem.MoveMessage();
                    g.DrawString(dmItem.Message, dmItem.Font, new SolidBrush(dmItem.Color), dmItem.Left, dmItem.Top);
                }
                g.Dispose();
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
            }
        }

        public static void MyDrawBitmap(Graphics g)
        {
            if (PlayApp.DrawBitmapFlag)
            {
                return;
            }
            try
            {
                PlayApp.DrawBitmapFlag = true;
                if (PlayApp.DrawBitmap != null)
                {
                    g.DrawImage(PlayApp.DrawBitmap, 0, 0);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("ImageApp", "MyDrawBitmap", ex);
            }
            finally
            {
                PlayApp.DrawBitmapFlag = false;
            }
        }
        #endregion



        #region " 获得网络图片 "
        /// <summary>
        /// 获得网络图片
        /// </summary>
        /// <param name="picUrl"></param>
        /// <returns></returns>
        public static string GetPictureFromUrl(string picUrl)
        {
            picUrl = picUrl.Replace("&amp;", "&");
            string picFile = Utility.GetRandomFileName(VariableInfo.TempPath, "jpg");
            try
            {
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(picUrl);


                CredentialCache myCache = new CredentialCache();
                myCache.Add(new Uri(picUrl), "Basic", new NetworkCredential("", ""));
                webreq.Credentials = myCache;
                webreq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", "", "")));

                System.Net.WebResponse webres = webreq.GetResponse();

                using (System.IO.Stream stream = webres.GetResponseStream())
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);
                    bitmap.Save(picFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

            return picFile;
        }

        #endregion

        #region " 画像サイズを調整する "
        /// <summary>
        /// 画像サイズを調整する
        /// </summary>
        /// <param name="sourceBmp"></param>
        /// <param name="size"></param>
        /// <param name="fillMode"></param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(Bitmap sourceBmp, Size size)
        {
            return ResizeBitmap(sourceBmp, size, FillMode.Fill);
        }
        public static Bitmap ResizeBitmap(Bitmap sourceBmp, Size size, FillMode fillMode)
        {
            float xRate = (float)sourceBmp.Width / size.Width;
            float yRate = (float)sourceBmp.Height / size.Height;
            if (xRate <= 1 && yRate <= 1 && FillMode.Center == fillMode)
            {
                return sourceBmp;
            }
            else
            {
                float tRate = (xRate >= yRate) ? xRate : yRate;
                Graphics g = null;
                try
                {
                    int newW = 0;
                    int newH = 0;
                    if (FillMode.Zoom == fillMode)
                    {
                        newW = (int)(sourceBmp.Width / tRate);
                        newH = (int)(sourceBmp.Height / tRate);
                    }
                    else //if (FillMode.Zoom== fillMode)
                    {
                        newW = size.Width;
                        newH = size.Height;
                    }

                    Bitmap b = new Bitmap(newW, newH);
                    g = Graphics.FromImage(b);
                    g.Clear(BackClearColor);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(sourceBmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height), GraphicsUnit.Pixel);
                    g.Dispose();
                    return b;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (null != g)
                    {
                        g.Dispose();
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// 马赛克效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void ShowBitmap_Special(Bitmap bmpSource, PictureBox picBox)
        {
            Graphics g = null;
            //以马赛克效果显示图像
            try
            {
                int dw = bmpSource.Width / 50;
                int dh = bmpSource.Height / 50;
                g = picBox.CreateGraphics();
                g.Clear(BackClearColor);
                Point[] MyPoint = new Point[2500];
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        MyPoint[x * 50 + y].X = x * dw;
                        MyPoint[x * 50 + y].Y = y * dh;
                    }
                }

                Bitmap bitmap = new Bitmap(bmpSource.Width, bmpSource.Height);
                for (int i = 0; i < 10000; i++)
                {
                    System.Random MyRandom = new Random();
                    int iPos = MyRandom.Next(2500);
                    for (int m = 0; m < dw; m++)
                        for (int n = 0; n < dh; n++)
                        {
                            bitmap.SetPixel(MyPoint[iPos].X + m, MyPoint[iPos].Y + n, bmpSource.GetPixel(MyPoint[iPos].X + m, MyPoint[iPos].Y + n));
                        }
                    picBox.Refresh();
                    picBox.Image = bitmap;
                }
                for (int i = 0; i < 2500; i++)
                {
                    for (int m = 0; m < dw; m++)
                    {
                        for (int n = 0; n < dh; n++)
                        {
                            bitmap.SetPixel(MyPoint[i].X + m, MyPoint[i].Y + n, bmpSource.GetPixel(MyPoint[i].X + m, MyPoint[i].Y + n));
                        }
                    }
                }
                picBox.Refresh();
                picBox.Image = bitmap;
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
        }

        #region " 显示图像 "

        #region " private "

        /// <summary>
        /// 分块显示
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Block(Bitmap bmpSource, PictureBox picBox)
        {
            Graphics g = null;
            try
            {
                //以分块效果显示图像
                g = picBox.CreateGraphics();

                int width = bmpSource.Width;
                int height = bmpSource.Height;
                //定义将图片切分成四个部分的区域
                RectangleF[] block ={
                    new RectangleF(0,0,width/2,height/2),
                    new RectangleF(width/2,0,width/2,height/2),
                    new RectangleF(0,height/2,width/2,height/2),
                    new RectangleF(width/2,height/2,width/2,height/2)};
                //分别克隆图片的四个部分    
                Bitmap[] MyBitmapBlack ={
                bmpSource.Clone(block[0],System.Drawing.Imaging.PixelFormat.DontCare),
                bmpSource.Clone(block[1],System.Drawing.Imaging.PixelFormat.DontCare),
                bmpSource.Clone(block[2],System.Drawing.Imaging.PixelFormat.DontCare),
                bmpSource.Clone(block[3],System.Drawing.Imaging.PixelFormat.DontCare)};
                //绘制图片的四个部分，各部分绘制时间间隔为0.5秒                    
                MyDrawImage(g, MyBitmapBlack[0], 0, 0);
                System.Threading.Thread.Sleep(500);
                MyDrawImage(g, MyBitmapBlack[1], width / 2, 0);
                System.Threading.Thread.Sleep(500);
                MyDrawImage(g, MyBitmapBlack[3], width / 2, height / 2);
                System.Threading.Thread.Sleep(500);
                MyDrawImage(g, MyBitmapBlack[2], 0, height / 2);
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
        }

        /// <summary>
        /// 淡入效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Fade(Bitmap bmpSource, PictureBox picBox)
        {
            //淡入显示图像
            Graphics g = null;
            try
            {
                g = picBox.CreateGraphics();

                int width = picBox.Width;
                int height = picBox.Height;
                ImageAttributes attributes = new ImageAttributes();
                ColorMatrix matrix = new ColorMatrix();
                //创建淡入颜色矩阵
                matrix.Matrix00 = (float)0.0;
                matrix.Matrix01 = (float)0.0;
                matrix.Matrix02 = (float)0.0;
                matrix.Matrix03 = (float)0.0;
                matrix.Matrix04 = (float)0.0;
                matrix.Matrix10 = (float)0.0;
                matrix.Matrix11 = (float)0.0;
                matrix.Matrix12 = (float)0.0;
                matrix.Matrix13 = (float)0.0;
                matrix.Matrix14 = (float)0.0;
                matrix.Matrix20 = (float)0.0;
                matrix.Matrix21 = (float)0.0;
                matrix.Matrix22 = (float)0.0;
                matrix.Matrix23 = (float)0.0;
                matrix.Matrix24 = (float)0.0;
                matrix.Matrix30 = (float)0.0;
                matrix.Matrix31 = (float)0.0;
                matrix.Matrix32 = (float)0.0;
                matrix.Matrix33 = (float)0.0;
                matrix.Matrix34 = (float)0.0;
                matrix.Matrix40 = (float)0.0;
                matrix.Matrix41 = (float)0.0;
                matrix.Matrix42 = (float)0.0;
                matrix.Matrix43 = (float)0.0;
                matrix.Matrix44 = (float)0.0;
                matrix.Matrix33 = (float)1.0;
                matrix.Matrix44 = (float)1.0;
                //从0到1进行修改色彩变换矩阵主对角线上的数值
                //使三种基准色的饱和度渐增
                Single count = (float)0.0;
                while (count < 1.0)
                {
                    matrix.Matrix00 = count;
                    matrix.Matrix11 = count;
                    matrix.Matrix22 = count;
                    matrix.Matrix33 = count;
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    MyDrawImage(g, bmpSource, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attributes);
                    
                    System.Threading.Thread.Sleep(200);
                    count = (float)(count + 0.04);
                }
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
        }

        /// <summary>
        /// 自动旋转
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Rotate(Bitmap bmpSource, PictureBox picBox)
        {
            Graphics g = null;
            try
            {
                g = picBox.CreateGraphics();
                float MyAngle = 0;//旋转的角度
                while (MyAngle <= 360)
                {
                    TextureBrush MyBrush = new TextureBrush(bmpSource);
                    MyBrush.RotateTransform(MyAngle);
                    g.FillRectangle(MyBrush, 0, 0, bmpSource.Width, bmpSource.Height);
                    MyAngle += 1f;
                    System.Threading.Thread.Sleep(3);
                }
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
        }

        /// <summary>
        /// 左右对接
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Docking_LR(Bitmap bmpSource, PictureBox picBox)
        {
            //以左右对接方式显示图像
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度
                g = picBox.CreateGraphics();

                Bitmap bitmap = new Bitmap(width, height);
                int x = 0;
                while (x <= width / 2)
                {
                    for (int i = 0; i <= height - 1; i++)
                    {
                        bitmap.SetPixel(x, i, bmpSource.GetPixel(x, i));
                    }
                    for (int i = 0; i <= height - 1; i++)
                    {
                        bitmap.SetPixel(width - x - 1, i,
                        bmpSource.GetPixel(width - x - 1, i));
                    }
                    x++;

                    MyDrawImage(g, bitmap, 0, 0);
                    System.Threading.Thread.Sleep(4);
                }
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
        }

        /// <summary>
        /// 上下对接
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Docking_TD(Bitmap bmpSource, PictureBox picBox)
        {
            //以上下对接方式显示图像

            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度
                g = picBox.CreateGraphics();

                Bitmap bitmap = new Bitmap(width, height);
                int y = 0;
                while (y <= height / 2)
                {
                    for (int i = 0; i <= width - 1; i++)
                    {
                        bitmap.SetPixel(i, y, bmpSource.GetPixel(i, y));
                    }
                    for (int i = 0; i <= width - 1; i++)
                    {
                        bitmap.SetPixel(i, height - y - 1,
                        bmpSource.GetPixel(i, height - y - 1));
                    }
                    y++;

                    MyDrawImage(g, bitmap, left, top);
                    System.Threading.Thread.Sleep(6);
                }
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
        }

        /// <summary>
        /// 左右翻转
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Flip_LR(Bitmap bmpSource, PictureBox picBox)
        {
            //以左右反转方式显示图像
            Graphics g = null;
            Graphics gBmpOld = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();

                Bitmap bmpOld = new Bitmap(picBox.Width, picBox.Height);
                gBmpOld = Graphics.FromImage(PlayApp.DrawBitmap);
               
                Bitmap bmpOld_LR = GetBitmapSpecial(bmpOld, BitmapSpecialStyle.LeftRight);

                int addNum = 0;
                int addNumMax = 10;
                int addNumIndex = 0;
                for (int j = -height / 2; j < 0; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(width) / Convert.ToSingle(height)));
                    Rectangle DestRect = new Rectangle(width / 2 - i, 0, 2 * i, height);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpOld.Width, bmpOld.Height);

                    gBmpOld.Clear(BackClearColor); //初始为全灰色
                    gBmpOld.DrawImage(bmpOld_LR, DestRect, SrcRect, GraphicsUnit.Pixel);

                    MyDrawImage(g, bmpOld, left, top);
                    System.Threading.Thread.Sleep(3);

                    if (addNumIndex >= addNumMax)
                    {
                        addNum += 1;
                    }

                    j+=addNum;
                    if (j >= 0)
                    {
                        break;
                    }
                }


                for (int j = 0; j <= height / 2; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(width) / Convert.ToSingle(height)));
                    Rectangle DestRect = new Rectangle(width / 2 - i, 0, 2 * i, height);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    MyDrawImage(g, bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    
                    System.Threading.Thread.Sleep(3);
                }

                MyDrawImage(g, bmpSource, left, top);
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
                if (null != gBmpOld)
                {
                    gBmpOld.Dispose();
                }
            }
        }

        /// <summary>
        /// 上下翻转
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_Flip_TD(Bitmap bmpSource, PictureBox picBox)
        {
            //以上下反转方式显示图像
            Graphics g = null;
            Graphics gBmpOld = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();

                Bitmap bmpOld = new Bitmap(picBox.Width, picBox.Height);
                gBmpOld = Graphics.FromImage(PlayApp.DrawBitmap);
                
                Bitmap bmpOld_TD = GetBitmapSpecial(bmpOld, BitmapSpecialStyle.TopDown);

                for (int j = -width / 2; j < 0; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(0, height / 2 - i, width, 2 * i);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpOld.Width, bmpOld.Height);

                    gBmpOld.Clear(BackClearColor); //初始为全灰色
                    gBmpOld.DrawImage(bmpOld_TD, DestRect, SrcRect, GraphicsUnit.Pixel);

                    MyDrawImage(g, bmpOld, left, top);
                    System.Threading.Thread.Sleep(3);
                }


                for (int j = 0; j <= width / 2; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(0, height / 2 - i, width, 2 * i);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    MyDrawImage(g, bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(3);
                }

                MyDrawImage(g, bmpSource, left, top);
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
                if (null != gBmpOld)
                {
                    gBmpOld.Dispose();
                }
            }
        }

        /// <summary>
        /// 小到大 扩散
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_SmallToLarge(Bitmap bmpSource, PictureBox picBox)
        {
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度
                g = picBox.CreateGraphics();

                for (int i = 0; i <= width / 2; i++)
                {
                    int j = Convert.ToInt32(i * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(width / 2 - i, height / 2 - j, 2 * i, 2 * j);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);
                    MyDrawImage(g, bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(3);
                }

                MyDrawImage(g, bmpSource, left, top);
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

        }

        /// <summary>
        /// 上下
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_TopToDown(Bitmap bmpSource, PictureBox picBox)
        {
            //以从上向下显示图像
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                for (int step = 0; step < height; step++)
                {
                    int y = step;
                    using (Bitmap bitmap = bmpSource.Clone(new Rectangle(0, y, width, 1), PixelFormat.Format24bppRgb))
                    {
                        MyDrawImage(g, bitmap, left, top + y);
                        System.Threading.Thread.Sleep(6);
                    }
                }
                MyDrawImage(g, bmpSource, left, top);
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
        }

        /// <summary>
        /// 下上
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_DownToTop(Bitmap bmpSource, PictureBox picBox)
        {
            //以从下向上显示图像
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                for (int step = 1; step < height; step++)
                {
                    int y = height - step;
                    using (Bitmap bitmap = bmpSource.Clone(new Rectangle(0, y, width, 1), PixelFormat.Format24bppRgb))
                    {
                        MyDrawImage(g, bitmap, left, top + y);
                        System.Threading.Thread.Sleep(6);
                    }
                }
                MyDrawImage(g, bmpSource, left, top);
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
        }

        /// <summary>
        /// 左右
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_LeftToRight(Bitmap bmpSource, PictureBox picBox)
        {
            //以从左向右显示图像
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                for (int step = 0; step < width; step++)
                {
                    int x = step;
                    using (Bitmap bitmap = bmpSource.Clone(new Rectangle(x, 0, 1, height), PixelFormat.Format24bppRgb))
                    {
                        MyDrawImage(g, bitmap, left + x, top);
                        System.Threading.Thread.Sleep(4);
                    }
                }
                MyDrawImage(g, bmpSource, left, top);
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
        }

        /// <summary>
        /// 右左
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        private static void ShowBitmap_RightToLeft(Bitmap bmpSource, PictureBox picBox)
        {
            //以从右向左显示图像
            Graphics g = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                for (int step = 1; step < width; step++)
                {
                    int x = width - step;
                    using (Bitmap bitmap = bmpSource.Clone(new Rectangle(x, 0, 1, height), PixelFormat.Format24bppRgb))
                    {
                        MyDrawImage(g, bitmap, left + x, top);
                        System.Threading.Thread.Sleep(4);
                    }
                }
                MyDrawImage(g, bmpSource, left, top);
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
        }

        #endregion " private "

        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="bmpSource">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void ShowBitmap(Bitmap bmpSource, PictureBox picBox, ImageShowStyle imgShowStyle)
        {

            if (PlayApp.DrawBitmapFlag)
            {
                return;
            }
            
            Graphics g = null;
            //显示图像
            try
            {
                PlayApp.DrawBitmapFlag = true;

                LogApp.OutputProcessLog("ImageApp", "ShowBitmap", "this BitMap");

                if (ImageShowStyle.None == imgShowStyle)
                {
                    int width = bmpSource.Width; //图像宽度    
                    int height = bmpSource.Height; //图像高度
                    int left = (picBox.Size.Width - width) / 2; //图像宽度    
                    int top = (picBox.Size.Height - height) / 2; //图像高度

                    g = picBox.CreateGraphics();
                    MyDrawImage(g, bmpSource, left, top);
                }
                else if (ImageShowStyle.TopToDown == imgShowStyle)
                {
                    ShowBitmap_TopToDown(bmpSource, picBox);
                }
                else if (ImageShowStyle.DownToTop == imgShowStyle)
                {
                    ShowBitmap_DownToTop(bmpSource, picBox);
                }
                else if (ImageShowStyle.LeftToRight == imgShowStyle)
                {
                    ShowBitmap_LeftToRight(bmpSource, picBox);
                }
                else if (ImageShowStyle.RightToLeft == imgShowStyle)
                {
                    ShowBitmap_RightToLeft(bmpSource, picBox);
                }
                else if (ImageShowStyle.SmallToLarge == imgShowStyle)
                {
                    ShowBitmap_SmallToLarge(bmpSource, picBox);
                }
                else if (ImageShowStyle.Gradient == imgShowStyle)
                {

                }
                else if (ImageShowStyle.Flip_LR == imgShowStyle)
                {
                    ShowBitmap_Flip_LR(bmpSource, picBox);
                }
                else if (ImageShowStyle.Flip_TD == imgShowStyle)
                {
                    ShowBitmap_Flip_TD(bmpSource, picBox);
                }
                else if (ImageShowStyle.Docking_LR == imgShowStyle)
                {
                    ShowBitmap_Docking_LR(bmpSource, picBox);
                }
                else if (ImageShowStyle.Docking_TD == imgShowStyle)
                {
                    ShowBitmap_Docking_TD(bmpSource, picBox);
                }
                else if (ImageShowStyle.Rotate == imgShowStyle)
                {
                    ShowBitmap_Rotate(bmpSource, picBox);
                }
                else if (ImageShowStyle.Fade == imgShowStyle)
                {
                    ShowBitmap_Fade(bmpSource, picBox);
                }
                else if (ImageShowStyle.Block == imgShowStyle)
                {
                    ShowBitmap_Block(bmpSource, picBox);
                }
                else if (ImageShowStyle.Special == imgShowStyle)
                {
                    ShowBitmap_Special(bmpSource, picBox);
                }
                else
                {
                    int width = bmpSource.Width; //图像宽度    
                    int height = bmpSource.Height; //图像高度
                    int left = (picBox.Size.Width - width) / 2; //图像宽度    
                    int top = (picBox.Size.Height - height) / 2; //图像高度

                    g = picBox.CreateGraphics();
                    MyDrawImage(g, bmpSource, left, top);
                }


                RectangleF myRect = new RectangleF(0, 0, bmpSource.Width, bmpSource.Height);
                PlayApp.DrawBitmap = bmpSource.Clone(myRect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                PlayApp.DrawBitmapFlag = false;
                if (null != g)
                {
                    g.Dispose();
                }
            }
        }

        #endregion

        #region " 图像特效 "
        /// <summary>
        /// 图像特效
        /// </summary>
        /// <param name="bmpSource">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static Bitmap GetBitmapSpecial(Bitmap bmpSource, BitmapSpecialStyle bmpSpecialStyle)
        {
            Graphics g = null;
            //显示图像
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度

                Bitmap bmpNew = new Bitmap(bmpSource.Width, bmpSource.Height);
                g = Graphics.FromImage(bmpNew);
                g.Clear(BackClearColor);

                if (BitmapSpecialStyle.None == bmpSpecialStyle)
                {
                    g.DrawImage(bmpSource, 0, 0);
                }
                else if (BitmapSpecialStyle.TopDown == bmpSpecialStyle)
                {
                    // 上下反转
                    int i_TD = Convert.ToInt32(-width / 2 * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect_TD = new Rectangle(0, height / 2 - i_TD, width, 2 * i_TD);
                    Rectangle SrcRect_TD = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);
                    g.DrawImage(bmpSource, DestRect_TD, SrcRect_TD, GraphicsUnit.Pixel);
                }
                else if (BitmapSpecialStyle.LeftRight == bmpSpecialStyle)
                {
                    // 左右反转
                    int i_LR = Convert.ToInt32(-height / 2 * (Convert.ToSingle(width) / Convert.ToSingle(height)));
                    Rectangle DestRect_LR = new Rectangle(width / 2 - i_LR, 0, 2 * i_LR, height);
                    Rectangle SrcRect_LR = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);
                    g.DrawImage(bmpSource, DestRect_LR, SrcRect_LR, GraphicsUnit.Pixel);
                }
                else if (BitmapSpecialStyle.Sketch == bmpSpecialStyle)
                {
                    // 黑白剪影特效
                    int i, j, iAvg, iPixel;
                    Color myColor, myNewColor;
                    RectangleF myRect;

                    myRect = new RectangleF(0, 0, width, height);
                    Bitmap bitmap = bmpSource.Clone(myRect, System.Drawing.Imaging.PixelFormat.DontCare);
                    i = 0;
                    while (i < width - 1)
                    {
                        j = 0;
                        while (j < height - 1)
                        {
                            myColor = bitmap.GetPixel(i, j);
                            iAvg = (myColor.R + myColor.G + myColor.B) / 3;
                            iPixel = 0;
                            if (iAvg >= 128)
                                iPixel = 255;
                            else
                                iPixel = 0;
                            myNewColor = Color.FromArgb(255, iPixel, iPixel, iPixel);
                            bitmap.SetPixel(i, j, myNewColor);
                            j = j + 1;
                        }
                        i = i + 1;
                    }
                    g.Clear(Color.WhiteSmoke);
                    g.DrawImage(bitmap, new Rectangle(0, 0, width, height));
                }
                else
                {
                    g.DrawImage(bmpSource, 0, 0);
                }
                return bmpNew;
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
        }


        /// <summary>
        /// 图像
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static Bitmap GetNewBitmap(Size size)
        {
            Graphics g = null;
            //显示图像
            try
            {
                Bitmap bmpNew = new Bitmap(size.Width, size.Height);
                g = Graphics.FromImage(bmpNew);
                g.Clear(BackClearColor);
                
                return bmpNew;
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
        }
        #endregion


        /// <summary>
        /// 显示模式
        /// </summary>
        /// <param name="style">显示模式</param>
        public static ImageShowStyle GetImageShowStyle(ImageShowStyle style)
        {
            ImageShowStyle reStyle = ImageShowStyle.None;
            List<int> disEnumValueList = new List<int> {
                    ImageShowStyle.Random.GetHashCode()
                    , ImageShowStyle.Gradient.GetHashCode()
                    , ImageShowStyle.Docking_LR.GetHashCode()
                    , ImageShowStyle.Docking_TD.GetHashCode()
                    , ImageShowStyle.Block.GetHashCode()
                    , ImageShowStyle.Special.GetHashCode()};
            
            try
            {
                if (style == ImageShowStyle.Random)
                {
                    List<string> enumList = new List<string>();

                    int[] enumValueList = EnumHelper.GetAllEnumValue(typeof(ImageShowStyle));
                    foreach (int enumValue in enumValueList)
                    {
                        if (disEnumValueList.Contains(enumValue) == false)
                        {
                            enumList.Add(enumValue.ToString());
                        }
                    }

                    reStyle = (ImageShowStyle)Utility.ToInt(Utility.GetRandomCode(enumList));
                }
                else
                {
                    reStyle = style;
                }

                return reStyle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}
