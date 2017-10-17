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

namespace ToilluminateClient
{
    public static class ImageApp
    {
        /// <summary>
        /// 初始为全灰色
        /// </summary>
        public static Color BackClearColor = Color.Gray;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceBmp"></param>
        /// <param name="picBox"></param>
        /// <returns></returns>

        public static Bitmap ResizeImage(Bitmap sourceBmp, PictureBox picBox)
        {
            return ResizeImage(sourceBmp, picBox, FillMode.Fill);
        }
        public static Bitmap ResizeImage(Bitmap sourceBmp, PictureBox picBox, FillMode fillMode)
        {
            float xRate = (float)sourceBmp.Width / picBox.Size.Width;
            float yRate = (float)sourceBmp.Height / picBox.Size.Height;
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
                        newW = picBox.Size.Width;
                        newH = picBox.Size.Height;
                    }

                    Bitmap b = new Bitmap(newW, newH);
                    g = Graphics.FromImage(b);
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

        /// <summary>
        /// 将图片转换成黑白色效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void HeiBaiSeImage(Bitmap bmp, PictureBox picBox)
        {
            //以黑白效果显示图像
            Bitmap oldBitmap;
            Bitmap newBitmap = null;
            try
            {
                int Height = bmp.Height;
                int Width = bmp.Width;
                newBitmap = new Bitmap(Width, Height);
                oldBitmap = bmp;
                Color pixel;
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        pixel = oldBitmap.GetPixel(x, y);
                        int r, g, b, Result = 0;
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        //实例程序以加权平均值法产生黑白图像
                        int iType = 2;
                        switch (iType)
                        {
                            case 0://平均值法
                                Result = ((r + g + b) / 3);
                                break;
                            case 1://最大值法
                                Result = r > g ? r : g;
                                Result = Result > b ? Result : b;
                                break;
                            case 2://加权平均值法
                                Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                                break;
                        }
                        newBitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                    }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            picBox.Image = newBitmap;
        }

        /// <summary>
        /// 雾化效果
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="picBox"></param>
        public static void WuHuaImage(Bitmap bmp, PictureBox picBox)
        {
            //雾化效果
            Bitmap oldBitmap;
            Bitmap newBitmap = null;
            try
            {
                int Height = bmp.Height;
                int Width = bmp.Width;
                newBitmap = new Bitmap(Width, Height);
                oldBitmap = bmp;
                Color pixel;
                for (int x = 1; x < Width - 1; x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        System.Random MyRandom = new Random();
                        int k = MyRandom.Next(123456);
                        //像素块大小
                        int dx = x + k % 19;
                        int dy = y + k % 19;
                        if (dx >= Width)
                            dx = Width - 1;
                        if (dy >= Height)
                            dy = Height - 1;
                        pixel = oldBitmap.GetPixel(dx, dy);
                        newBitmap.SetPixel(x, y, pixel);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            picBox.Image = newBitmap;
        }


        /// <summary>
        /// 锐化效果
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="picBox"></param>
        public static void RuiHuaImage(Bitmap bmp, PictureBox picBox)
        {
            Bitmap oldBitmap;
            Bitmap newBitmap = null;
            try
            {
                int Height = bmp.Height;
                int Width = bmp.Width;
                newBitmap = new Bitmap(Width, Height);
                oldBitmap = bmp;
                Color pixel;
                //拉普拉斯模板
                int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
                for (int x = 1; x < Width - 1; x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        int Index = 0;
                        for (int col = -1; col <= 1; col++)
                            for (int row = -1; row <= 1; row++)
                            {
                                pixel = oldBitmap.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
                                g += pixel.G * Laplacian[Index];
                                b += pixel.B * Laplacian[Index];
                                Index++;
                            }
                        //处理颜色值溢出
                        r = r > 255 ? 255 : r;
                        r = r < 0 ? 0 : r;

                        g = g > 255 ? 255 : g;
                        g = g < 0 ? 0 : g;

                        b = b > 255 ? 255 : b;
                        b = b < 0 ? 0 : b;

                        newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            picBox.Image = newBitmap;
        }

        /// <summary>
        ///底片效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void DiPianImage(Bitmap bmp, PictureBox picBox)
        {
            Bitmap oldBitmap;
            Bitmap newBitmap = null;
            try
            {
                int Height = bmp.Height;
                int Width = bmp.Width;
                newBitmap = new Bitmap(Width, Height);
                oldBitmap = bmp;
                Color pixel;
                for (int x = 1; x < Width; x++)
                {
                    for (int y = 1; y < Height; y++)
                    {
                        int r, g, b;
                        pixel = oldBitmap.GetPixel(x, y);
                        r = 255 - pixel.R;
                        g = 255 - pixel.G;
                        b = 255 - pixel.B;
                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            picBox.Image = newBitmap;
        }


        /// <summary>
        ///浮雕效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void FuDiaoImage(Bitmap bmp, PictureBox picBox)
        {
            Bitmap oldBitmap;
            Bitmap newBitmap = null;
            try
            {
                int Height = bmp.Height;
                int Width = bmp.Width;
                newBitmap = new Bitmap(Width, Height);
                oldBitmap = bmp;
                Color pixel1, pixel2;
                for (int x = 0; x < Width - 1; x++)
                {
                    for (int y = 0; y < Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;
                        pixel1 = oldBitmap.GetPixel(x, y);
                        pixel2 = oldBitmap.GetPixel(x + 1, y + 1);
                        r = Math.Abs(pixel1.R - pixel2.R + 128);
                        g = Math.Abs(pixel1.G - pixel2.G + 128);
                        b = Math.Abs(pixel1.B - pixel2.B + 128);
                        if (r > 255)
                            r = 255;
                        if (r < 0)
                            r = 0;
                        if (g > 255)
                            g = 255;
                        if (g < 0)
                            g = 0;
                        if (b > 255)
                            b = 255;
                        if (b < 0)
                            b = 0;
                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            picBox.Image = newBitmap;
        }


        /// <summary>
        /// 日光照射效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void RiGuangZhaoSheImage(Bitmap bmp, PictureBox picBox)
        {
            //以光照效果显示图像
            Graphics MyGraphics = picBox.CreateGraphics();
            MyGraphics.Clear(Color.White);
            Bitmap MyBmp = new Bitmap(bmp, bmp.Width, bmp.Height);
            int MyWidth = MyBmp.Width;
            int MyHeight = MyBmp.Height;
            Bitmap MyImage = MyBmp.Clone(new RectangleF(0, 0, MyWidth, MyHeight), System.Drawing.Imaging.PixelFormat.DontCare);
            int A = MyWidth / 2;
            int B = MyHeight / 2;
            //MyCenter图片中心点，发亮此值会让强光中心发生偏移
            Point MyCenter = new Point(MyWidth / 2, MyHeight / 2);
            //R强光照射面的半径，即”光晕”
            int R = Math.Min(MyWidth / 2, MyHeight / 2);
            for (int i = MyWidth - 1; i >= 1; i--)
            {
                for (int j = MyHeight - 1; j >= 1; j--)
                {
                    float MyLength = (float)Math.Sqrt(Math.Pow((i - MyCenter.X), 2) + Math.Pow((j - MyCenter.Y), 2));
                    //如果像素位于”光晕”之内
                    if (MyLength < R)
                    {
                        Color MyColor = MyImage.GetPixel(i, j);
                        int r, g, b;
                        //220亮度增加常量，该值越大，光亮度越强
                        float MyPixel = 220.0f * (1.0f - MyLength / R);
                        r = MyColor.R + (int)MyPixel;
                        r = Math.Max(0, Math.Min(r, 255));
                        g = MyColor.G + (int)MyPixel;
                        g = Math.Max(0, Math.Min(g, 255));
                        b = MyColor.B + (int)MyPixel;
                        b = Math.Max(0, Math.Min(b, 255));
                        //将增亮后的像素值回写到位图
                        Color MyNewColor = Color.FromArgb(255, r, g, b);
                        MyImage.SetPixel(i, j, MyNewColor);
                    }
                }
                //重新绘制图片
                MyGraphics.DrawImage(MyImage, new Rectangle(0, 0, MyWidth, MyHeight));
            }
        }


        /// <summary>
        /// 油画效果
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void YouHuaImage(Bitmap bmp, PictureBox picBox)
        {
            //以油画效果显示图像
            Graphics g = picBox.CreateGraphics();
            int width = bmp.Width;
            int height = bmp.Height;
            RectangleF rect = new RectangleF(0, 0, width, height);
            Bitmap bmpSource = bmp;
            Bitmap img = bmpSource.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
            //产生随机数序列
            Random rnd = new Random();
            //取不同的值决定油画效果的不同程度
            int iModel = 2;
            int i = width - iModel;
            while (i > 1)
            {
                int j = height - iModel;
                while (j > 1)
                {
                    int iPos = rnd.Next(100000) % iModel;
                    //将该点的RGB值设置成附近iModel点之内的任一点
                    Color color = img.GetPixel(i + iPos, j + iPos);
                    img.SetPixel(i, j, color);
                    j = j - 1;
                }
                i = i - 1;
            }
            //重新绘制图像
            g.Clear(Color.White);
            g.DrawImage(img, new Rectangle(0, 0, width, height));
        }


        /// <summary>
        /// 垂直百叶窗
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void BaiYeChuang1(Bitmap bmp, PictureBox picBox)
        {
            //垂直百叶窗显示图像
            Graphics g = null;
            try
            {
                Bitmap bmpSource = (Bitmap)bmp.Clone();
                int dw = bmpSource.Width / 30;
                int dh = bmpSource.Height;
                g = picBox.CreateGraphics();
                g.Clear(BackClearColor);
                Point[] MyPoint = new Point[30];
                for (int x = 0; x < 30; x++)
                {
                    MyPoint[x].Y = 0;
                    MyPoint[x].X = x * dw;
                }
                Bitmap bitmap = new Bitmap(bmpSource.Width, bmpSource.Height);
                for (int i = 0; i < dw; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        for (int k = 0; k < dh; k++)
                        {
                            bitmap.SetPixel(MyPoint[j].X + i, MyPoint[j].Y + k, bmpSource.GetPixel(MyPoint[j].X + i, MyPoint[j].Y + k));
                        }
                    }
                    picBox.Refresh();
                    picBox.Image = bitmap;
                    System.Threading.Thread.Sleep(120);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (null != g)
                //{
                //    g.Dispose();
                //}
            }

        }


        /// <summary>
        /// 水平百叶窗
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void BaiYeChuang2(Bitmap bmp, PictureBox picBox)
        {
            //水平百叶窗显示图像
            Graphics g = null;
            try
            {
                Bitmap bmpSource = (Bitmap)bmp.Clone();
                int dh = bmpSource.Height / 20;
                int dw = bmpSource.Width;
                g = picBox.CreateGraphics();
                g.Clear(BackClearColor);
                Point[] MyPoint = new Point[20];
                for (int y = 0; y < 20; y++)
                {
                    MyPoint[y].X = 0;
                    MyPoint[y].Y = y * dh;
                }
                Bitmap bitmap = new Bitmap(bmpSource.Width, bmpSource.Height);
                for (int i = 0; i < dh; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        for (int k = 0; k < dw; k++)
                        {
                            bitmap.SetPixel(MyPoint[j].X + k, MyPoint[j].Y + i, bmpSource.GetPixel(MyPoint[j].X + k, MyPoint[j].Y + i));
                        }
                    }
                    picBox.Refresh();
                    picBox.Image = bitmap;
                    System.Threading.Thread.Sleep(100);
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
        /// 逆时针旋转
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void XuanZhuan90(Bitmap bmp, PictureBox picBox)
        {
            try
            {
                Graphics g = picBox.CreateGraphics();
                bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
                g.Clear(Color.White);
                g.DrawImage(bmp, 0, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 顺时针旋转
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void XuanZhuan270(Bitmap bmp, PictureBox picBox)
        {
            try
            {
                Graphics g = picBox.CreateGraphics();
                bmp.RotateFlip(RotateFlipType.Rotate270FlipXY);
                g.Clear(Color.White);
                g.DrawImage(bmp, 0, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 分块显示
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void Block(Bitmap bmpSource, PictureBox picBox)
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
                g.DrawImage(MyBitmapBlack[0], 0, 0);
                System.Threading.Thread.Sleep(500);
                g.DrawImage(MyBitmapBlack[1], width / 2, 0);
                System.Threading.Thread.Sleep(500);
                g.DrawImage(MyBitmapBlack[3], width / 2, height / 2);
                System.Threading.Thread.Sleep(500);
                g.DrawImage(MyBitmapBlack[2], 0, height / 2);
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

        #region " private "

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
                    g.DrawImage(bmpSource, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attributes);
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
                    System.Threading.Thread.Sleep(30);
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
                    
                    g.DrawImage(bitmap, 0, 0);
                    System.Threading.Thread.Sleep(10);
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

                    g.DrawImage(bitmap, left, top);
                    System.Threading.Thread.Sleep(10);
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
                gBmpOld = Graphics.FromImage(bmpOld);
                //转换成控件在屏幕上的坐标
                var screenPoint = picBox.Parent.PointToScreen(picBox.Location);
                gBmpOld.CopyFromScreen(screenPoint, new Point(0, 0), picBox.Size);
                
                Bitmap bmpOld_LR = GetBitmapSpecial(bmpOld, BitmapSpecialStyle.LeftRight);

                for (int j = -height / 2; j < 0; j++)
                {
                    
                    int i = Convert.ToInt32(j * (Convert.ToSingle(width) / Convert.ToSingle(height)));
                    Rectangle DestRect = new Rectangle(width / 2 - i, 0, 2 * i, height);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpOld.Width, bmpOld.Height);

                    gBmpOld.Clear(BackClearColor); //初始为全灰色
                    gBmpOld.DrawImage(bmpOld_LR, DestRect, SrcRect, GraphicsUnit.Pixel);

                    g.DrawImage(bmpOld, left, top);
                    System.Threading.Thread.Sleep(15);
                }


                for (int j = 0; j <= height / 2; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(width) / Convert.ToSingle(height)));
                    Rectangle DestRect = new Rectangle(width / 2 - i, 0, 2 * i, height);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(15);
                }

                g.DrawImage(bmpSource, left, top);
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
                gBmpOld = Graphics.FromImage(bmpOld);
                //转换成控件在屏幕上的坐标
                var screenPoint = picBox.Parent.PointToScreen(picBox.Location);
                gBmpOld.CopyFromScreen(screenPoint, new Point(0, 0), picBox.Size);

                Bitmap bmpOld_TD = GetBitmapSpecial(bmpOld, BitmapSpecialStyle.TopDown);

                for (int j = -width / 2; j < 0; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(0, height / 2 - i, width,  2 * i);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpOld.Width, bmpOld.Height);

                    gBmpOld.Clear(BackClearColor); //初始为全灰色
                    gBmpOld.DrawImage(bmpOld_TD, DestRect, SrcRect, GraphicsUnit.Pixel);

                    g.DrawImage(bmpOld, left, top);
                    System.Threading.Thread.Sleep(15);
                }


                for (int j = 0; j <= width / 2; j++)
                {
                    int i = Convert.ToInt32(j * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(0, height / 2 - i, width, 2 * i);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(15);
                }

                g.DrawImage(bmpSource, left, top);
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
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(10);
                }

                g.DrawImage(bmpSource, left, top);
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
                        g.DrawImage(bitmap, left, top + y);
                        System.Threading.Thread.Sleep(10);
                    }
                }
                g.DrawImage(bmpSource, left, top);
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
                        g.DrawImage(bitmap, left, top + y);
                        System.Threading.Thread.Sleep(10);
                    }
                }
                g.DrawImage(bmpSource, left, top);
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
                        g.DrawImage(bitmap, left + x, top);
                        System.Threading.Thread.Sleep(10);
                    }
                }
                g.DrawImage(bmpSource, left, top);
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
                        g.DrawImage(bitmap, left + x, top);
                        System.Threading.Thread.Sleep(10);
                    }
                }
                g.DrawImage(bmpSource, left, top);
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
        /// 
        /// </summary>
        /// <param name="bmp">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void ShowBitmap(Bitmap bmpSource, PictureBox picBox, ImageShowStyle imgShowStyle)
        {
            Graphics g = null;
            //显示图像
            try
            {
                if (ImageShowStyle.None == imgShowStyle)
                {
                    int width = bmpSource.Width; //图像宽度    
                    int height = bmpSource.Height; //图像高度
                    int left = (picBox.Size.Width - width) / 2; //图像宽度    
                    int top = (picBox.Size.Height - height) / 2; //图像高度

                    g = picBox.CreateGraphics();
                    g.DrawImage(bmpSource, left, top);
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
                    g.DrawImage(bmpSource, left, top);
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
    }


    /// <summary>
    /// 填充模式
    /// </summary>
    /// <remarks></remarks>
    public enum FillMode
    {
        /// <summary>
        /// 平铺
        /// </summary>
        /// <remarks></remarks>
        Fill = 0,
        /// <summary>
        /// 居中
        /// </summary>
        /// <remarks></remarks>
        Center = 1,
        /// <summary>
        /// 缩放
        /// </summary>
        /// <remarks></remarks>
        Zoom = 2,
    }


    /// <summary>
    /// 
    /// </summary>
    public enum ImageShowStyle
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        TopToDown = 1,
        /// <summary>
        /// 
        /// </summary>
        DownToTop = 2,
        /// <summary>
        /// 
        /// </summary>
        LeftToRight = 3,
        /// <summary>
        /// 
        /// </summary>
        RightToLeft = 4,
        /// <summary>
        /// 
        /// </summary>
        SmallToLarge = 5,
        /// <summary>
        /// 
        /// </summary>
        Gradient = 6,
        /// <summary>
        /// 
        /// </summary>
        Flip_LR = 7,
        /// <summary>
        /// 
        /// </summary>
        Flip_TD = 8,
        /// <summary>
        /// 
        /// </summary>
        Docking_LR = 9,
        /// <summary>
        /// 
        /// </summary>
        Docking_TD = 10,
        /// <summary>
        /// 
        /// </summary>
        Rotate = 11,
        /// <summary>
        /// 
        /// </summary>
        Fade = 12,
        /// <summary>
        /// 
        /// </summary>
        Special = 99,

    }

    /// <summary>
    /// 
    /// </summary>
    public enum BitmapSpecialStyle
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 上下反转
        /// </summary>
        TopDown = 1,
        /// <summary>
        /// 左右反转
        /// </summary>
        LeftRight = 2,
        /// <summary>
        /// 黑白剪影特效
        /// </summary>
        Sketch = 3,
        /// <summary>
        /// 
        /// </summary>
        //Docking_TD = 13,
        /// <summary>
        /// 
        /// </summary>
        //Docking_DT = 14,

    }
}
