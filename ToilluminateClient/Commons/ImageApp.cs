using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ToilluminateClient
{
    public static class ImageApp
    {

        /// <summary>
        /// 初始为全灰色
        /// </summary>
        public static Color BackClearColor = Color.Black;

        public static int SepNumber = 100;

        public static int SleepTime = 5;

        public static int MessageShowTime = 3000;
        public static int MessageSleepTime = 1;
        public static int MessageSepNumber = 30;

        #region " 显示递增频度 "
        /// <summary>
        /// 显示递增频度
        /// </summary>
        /// <param name="maxFix"></param>
        /// <param name="sleepTime"></param>
        /// <returns></returns>
        public static int GetShowSepFix(int maxFix)
        {
            int sepFix = 0;
            sepFix = maxFix / SepNumber;
            if (sepFix < 1)
            {
                sepFix = 1;
            }
            return sepFix;
        }
        public static double GetShowSepFix2(int maxFix)
        {
            double sepFix = 0;
            sepFix = Math.Round(Math.Pow(maxFix, 1.0 / SepNumber), 3);
            if (sepFix < 1.001)
            {
                sepFix = 1.001;
            }
            return sepFix;
        }
        public static int GetShowSepFix3(int maxFix)
        {
            int sepFix = 0;
            sepFix = maxFix / (int)Math.Floor(Math.Pow(SepNumber, 0.5));
            if (sepFix < 1)
            {
                sepFix = 1;
            }
            return sepFix;
        }

        #endregion

        #region " 字幕 "
        /// <summary>
        /// 字幕
        /// </summary>
        public static void MyDrawMessage(Control drawingBoard)
        {
            if (ShowApp.DrawMessageFlag)
            {
                return;
            }

            Graphics g = null;
            Graphics gBmpBack = null;
            try
            {
                ShowApp.DrawMessageFlag = true;

                int controlLeft = 0;
                int controlTop = 0;
                MessagePositionType controlShowStyle = MessagePositionType.Bottom;
                bool drawNewImage = false;
                bool drawing = false;

                int newW = drawingBoard.Width;
                int newH = drawingBoard.Height;


                if (ShowApp.DrawMessageList.Count == 0)
                {
                    if (ShowApp.DownLoadDrawMessage != null && ShowApp.DownLoadDrawMessage.DrawStyleList.Count > 0)
                    {
                        DrawMessage dmItem = ShowApp.DownLoadDrawMessage;

                        if (dmItem.MoveState == MoveStateType.NotMove)
                        {
                            newW = dmItem.Width;
                            newH = dmItem.Heigth;
                            ShowApp.MessageBackBitmap = new Bitmap(newW, newH);
                            gBmpBack = Graphics.FromImage(ShowApp.MessageBackBitmap);
                            gBmpBack.Clear(ImageApp.BackClearColor);

                            foreach (DrawMessageStyle dslItem in dmItem.DrawStyleList)
                            {
                                gBmpBack.DrawString(dslItem.Message, dslItem.Font, new SolidBrush(dslItem.Color), dslItem.LeftWidth, 0);
                            }

                            drawNewImage = true;
                            drawing = true;
                            controlLeft = dmItem.Left;
                            controlTop = dmItem.GetStyleTop(dmItem.Heigth);
                            controlShowStyle = dmItem.ShowStyle;
                            dmItem.MoveMessage(ImageApp.MessageSepNumber);
                        }
                        else if (dmItem.MoveState == MoveStateType.Moving)
                        {
                            dmItem.MoveMessage(ImageApp.MessageSepNumber);
                            controlLeft = dmItem.Left;
                            controlTop = dmItem.GetStyleTop(dmItem.Heigth);
                            drawing = true;
                        }
                    }
                }
                else
                {
                    foreach (DrawMessage dmItem in ShowApp.DrawMessageList)
                    {
                        if (dmItem.MoveState == MoveStateType.NotMove)
                        {
                            newW = dmItem.Width;
                            newH = dmItem.Heigth;
                            ShowApp.MessageBackBitmap = new Bitmap(newW, newH);
                            gBmpBack = Graphics.FromImage(ShowApp.MessageBackBitmap);
                            gBmpBack.Clear(ImageApp.BackClearColor);

                            foreach (DrawMessageStyle dslItem in dmItem.DrawStyleList)
                            {
                                gBmpBack.DrawString(dslItem.Message, dslItem.Font, new SolidBrush(dslItem.Color), dslItem.LeftWidth, 0);
                            }

                            drawNewImage = true;
                            drawing = true;
                            controlLeft = dmItem.Left;
                            controlTop = dmItem.GetStyleTop(dmItem.Heigth);
                            controlShowStyle = dmItem.ShowStyle;
                            dmItem.MoveMessage(ImageApp.MessageSepNumber);
                            break;
                        }
                        else if (dmItem.MoveState == MoveStateType.Moving)
                        {
                            dmItem.MoveMessage(ImageApp.MessageSepNumber);
                            controlLeft = dmItem.Left;
                            controlTop = dmItem.GetStyleTop(dmItem.Heigth);
                            drawing = true;
                            break;
                        }
                    }
                }

                if (drawing)
                {
                    if (drawNewImage && ShowApp.MessageBackBitmap != null)
                    {
                        drawingBoard.Width = ShowApp.MessageBackBitmap.Width;
                        drawingBoard.Height = ShowApp.MessageBackBitmap.Height;
                        drawingBoard.Left = controlLeft;
                        drawingBoard.Top = controlTop;
                        drawingBoard.BackgroundImage = ShowApp.MessageBackBitmap;
                    }
                    else
                    {
                        if (drawingBoard.Left != controlLeft)
                        {
                            drawingBoard.Left = controlLeft;
                        }
                        if (drawingBoard.Top != controlTop)
                        {
                            drawingBoard.Top = controlTop;
                        }
                    }
                }
                else
                {
                    if (drawingBoard.BackgroundImage != null)
                    {
                        drawingBoard.BackgroundImage = null;
                        ShowApp.MessageBackBitmap.Dispose();
                        ShowApp.MessageBackBitmap = null;
                    }
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("ImageApp", "MyDrawMessage", ex);
            }
            finally
            {
                ShowApp.DrawMessageFlag = false;
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

        #endregion
        #region " 商标 "
        /// <summary>
        /// 商标
        /// </summary>
        public static void MyDrawTrademark(Control[] drawingBoards)
        {
            if (ShowApp.DrawTrademarkFlag)
            {
                return;
            }

            Graphics g = null;
            Graphics gBmpBack = null;
            try
            {
                ShowApp.DrawTrademarkFlag = true;
                int trademarkCount = 10;

                int[] controlLefts = new int[10] { -10, -10, -10, -10, -10, -10, -10, -10, -10, -10 };
                int[] controlTops = new int[10] { -10, -10, -10, -10, -10, -10, -10, -10, -10, -10 };
                bool drawNewImage = false;
                bool drawing = false;

                int newW = 0;
                int newH = 0;


                foreach (DrawTrademark dtItem in ShowApp.DrawTrademarkList)
                {
                    if (dtItem.ShowState == ShowStateType.NotShow)
                    {
                        foreach (DrawTrademarkStyle dtStyle in dtItem.DrawStyleList)
                        {
                            int trademarkBackBitmapIndex = dtStyle.TrademarkPosition.GetHashCode();
                            newW = dtStyle.Width;
                            newH = dtStyle.Heigth;
                            ShowApp.TrademarkBackBitmaps[trademarkBackBitmapIndex] = new Bitmap(newW, newH);
                            gBmpBack = Graphics.FromImage(ShowApp.TrademarkBackBitmaps[trademarkBackBitmapIndex]);
                            gBmpBack.Clear(ImageApp.BackClearColor);

                            //动态添加图片
                            using (Image imageTemp = ImageApp.GetBitmap(dtStyle.File))
                            {                                
                                gBmpBack.DrawImage(imageTemp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, imageTemp.Width, imageTemp.Height), GraphicsUnit.Pixel);
                            }

                            Point location = dtItem.GetStyleLocation(dtStyle);

                            controlLefts[trademarkBackBitmapIndex] = location.X;
                            controlTops[trademarkBackBitmapIndex] = location.Y;
                        }

                        drawNewImage = true;
                        drawing = true;
                        dtItem.ShowTrademark();
                        break;
                    }
                    else if (dtItem.ShowState == ShowStateType.Showing)
                    {
                        foreach (DrawTrademarkStyle dtStyle in dtItem.DrawStyleList)
                        {
                            int trademarkBackBitmapIndex = dtStyle.TrademarkPosition.GetHashCode();
                           
                            Point location = dtItem.GetStyleLocation(dtStyle);

                            controlLefts[trademarkBackBitmapIndex] = location.X;
                            controlTops[trademarkBackBitmapIndex] = location.Y;
                        }
                        drawing = true;
                        break;
                    }
                }

                if (drawing)
                {
                    if (drawNewImage)
                    {
                        for (int boardIndex= 0; boardIndex < trademarkCount; boardIndex++)
                        {
                            if (ShowApp.TrademarkBackBitmaps[boardIndex] != null)
                            {
                                drawingBoards[boardIndex].Width = ShowApp.TrademarkBackBitmaps[boardIndex].Width;
                                drawingBoards[boardIndex].Height = ShowApp.TrademarkBackBitmaps[boardIndex].Height;
                                drawingBoards[boardIndex].Left = controlLefts[boardIndex];
                                drawingBoards[boardIndex].Top = controlTops[boardIndex];
                                drawingBoards[boardIndex].BackgroundImage = ShowApp.TrademarkBackBitmaps[boardIndex];
                            }
                        }
                    }
                }
                else
                {
                    for (int boardIndex = 0; boardIndex < trademarkCount; boardIndex++)
                    {
                        if (drawingBoards[boardIndex].BackgroundImage != null)
                        {
                            drawingBoards[boardIndex].BackgroundImage = null;
                            ShowApp.TrademarkBackBitmaps[boardIndex].Dispose();
                            ShowApp.TrademarkBackBitmaps[boardIndex] = null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("ImageApp", "MyDrawTrademark", ex);
            }
            finally
            {
                ShowApp.DrawTrademarkFlag = false;
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

        #endregion

        #region " 画图片 "
        public static void MyDrawBitmap(Graphics g)
        {
            if (ShowApp.DrawBitmapFlag)
            {
                return;
            }
            try
            {
                ShowApp.DrawBitmapFlag = true;
                if (ShowApp.DrawBitmap != null)
                {
                    g.DrawImage(ShowApp.DrawBitmap, 0, 0);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("ImageApp", "MyDrawBitmap", ex);
            }
            finally
            {
                ShowApp.DrawBitmapFlag = false;
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
        public static Bitmap ResizeBitmap(Bitmap sourceBmp, Size size, FillOptionStyle fillOption)
        {
            Graphics g = null;

            try
            {
                int newW = size.Width;
                int newH = size.Height;
                if (sourceBmp == null)
                {
                    sourceBmp = GetNewBitmap(size);
                }

                int srcW = sourceBmp.Width;
                int srcH = sourceBmp.Height;

                int left = 0;
                int top = 0;

                Bitmap b = new Bitmap(newW, newH);
                g = Graphics.FromImage(b);
                g.Clear(BackClearColor);

                if (FillOptionStyle.None == fillOption)
                {
                    #region
                    if (newW > srcW)
                    {
                        left = (newW - srcW) / 2;
                        newW = srcW;
                    }
                    else
                    {
                        srcW = newW;
                    }


                    if (newH > srcH)
                    {
                        top = (newH - srcH) / 2;
                        newH = srcH;
                    }
                    else
                    {
                        srcH = newH;
                    }
                    #endregion
                }
                else if (FillOptionStyle.Fill == fillOption)
                {
                }
                else if (FillOptionStyle.Zoom == fillOption)
                {
                    #region
                    if (newW != srcW || newH != srcH)
                    {
                        float xRate = (float)srcW / newW;
                        float yRate = (float)srcH / newH;
                        float tRate = (xRate >= yRate) ? xRate : yRate;
                        int srcW_tmp = (int)(srcW / tRate);
                        int srcH_tmp = (int)(srcH / tRate);

                        if (newW > srcW_tmp)
                        {
                            left = (newW - srcW_tmp) / 2;
                            newW = srcW_tmp;
                        }


                        if (newH > srcH_tmp)
                        {
                            top = (newH - srcH_tmp) / 2;
                            newH = srcH_tmp;
                        }
                    }
                    #endregion
                }


                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourceBmp, new Rectangle(left, top, newW, newH), new Rectangle(0, 0, srcW, srcH), GraphicsUnit.Pixel);
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
        #endregion


        #region " 画像をコピーする "
        /// <summary>
        /// 画像をコピーする
        /// </summary>
        /// <param name="targetBmp"></param>
        /// <param name="sourceBmp"></param>
        /// <returns></returns>
        public static void CopyBitmap(Bitmap targetBmp, Bitmap sourceBmp)
        {
            Graphics g = null;

            try
            {
                int newW = targetBmp.Width;
                int newH = targetBmp.Height;
                int srcW = sourceBmp.Width;
                int srcH = sourceBmp.Height;

                g = Graphics.FromImage(targetBmp);
                g.Clear(BackClearColor);


                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourceBmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, srcW, srcH), GraphicsUnit.Pixel);
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
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                int sepFixW = GetShowSepFix3(width);
                int sepFixH = GetShowSepFix3(height);


                int sepMax = width / sepFixW;

                int sepFixWDL = sepFixW + width - (sepFixW * sepMax);
                int sepFixHDL = sepFixH + height - (sepFixH * sepMax);


                int sepMaxX = sepMax;
                int sepMaxY = sepMax;
                int sepMinX = 1;
                int sepMinY = 1;
                int sepX = sepMinX - 1;
                int sepY = sepMinY;
                int x = 0;
                int y = 0;
                int sepFixWD = sepFixW;
                int sepFixHD = sepFixH;
                int drawState = 1;//1:top, 2:left, 3:bottom, 4:right
                while (sepMinX < sepMaxX)
                {
                    if (drawState == 1)
                    {
                        if (sepX < sepMaxX)
                        {
                            sepX++;
                        }
                        else
                        {
                            drawState = 2;
                            continue;
                        }
                    }

                    else if (drawState == 2)
                    {
                        if (sepY < sepMaxY)
                        {
                            sepY++;
                        }
                        else
                        {
                            drawState = 3;
                            continue;
                        }
                    }

                    else if (drawState == 3)
                    {
                        if (sepX > sepMinX)
                        {
                            sepX--;
                        }
                        else
                        {
                            drawState = 4;
                            continue;
                        }
                    }
                    else if (drawState == 4)
                    {
                        if (sepY > sepMinY)
                        {
                            sepY--;
                        }
                        else
                        {
                            sepMinX++;
                            sepMinY++;
                            sepMaxX--;
                            sepMaxY--;
                            sepX = sepMinX - 1;
                            sepY = sepMinY;
                            drawState = 1;
                            continue;
                        }
                    }



                    x = sepFixW * (sepX - 1);

                    y = sepFixH * (sepY - 1);

                    if (sepX == sepMax)
                    {
                        sepFixWD = sepFixWDL;
                    }
                    else
                    {
                        sepFixWD = sepFixW;
                    }

                    if (sepY == sepMax)
                    {
                        sepFixHD = sepFixHDL;
                    }
                    else
                    {
                        sepFixHD = sepFixH;
                    }

                    Rectangle DestRect = new Rectangle(x, y, sepFixWD, sepFixHD);
                    Rectangle SrcRect = new Rectangle(x + left, y + top, sepFixWD, sepFixHD);
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);

                    System.Threading.Thread.Sleep(SleepTime);
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
            //以马赛克效果显示图像
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();


                int sepFixW = GetShowSepFix3(width);
                int sepFixH = GetShowSepFix3(height);


                int sepMax = width / sepFixW;

                int sepFixWDL = sepFixW + width - (sepFixW * sepMax);
                int sepFixHDL = sepFixH + height - (sepFixH * sepMax);


                int sepMaxX = sepMax;
                int sepMaxY = sepMax;
                int sepMinX = 1;
                int sepMinY = 1;
                int sepX = sepMinX - 1;
                int sepY = sepMinY;
                int x = 0;
                int y = 0;
                int sepFixWD = sepFixW;
                int sepFixHD = sepFixH;
                int drawState = 1;//1:top, 2:left, 3:bottom, 4:right
                while (sepMinX < sepMaxX)
                {
                    if (drawState == 1)
                    {
                        if (sepX < sepMaxX)
                        {
                            sepX++;
                        }
                        else
                        {
                            drawState = 2;
                            continue;
                        }
                    }

                    else if (drawState == 2)
                    {
                        if (sepY < sepMaxY)
                        {
                            sepY++;
                        }
                        else
                        {
                            drawState = 3;
                            continue;
                        }
                    }

                    else if (drawState == 3)
                    {
                        if (sepX > sepMinX)
                        {
                            sepX--;
                        }
                        else
                        {
                            drawState = 4;
                            continue;
                        }
                    }
                    else if (drawState == 4)
                    {
                        if (sepY > sepMinY)
                        {
                            sepY--;
                        }
                        else
                        {
                            sepMinX++;
                            sepMinY++;
                            sepMaxX--;
                            sepMaxY--;
                            sepX = sepMinX - 1;
                            sepY = sepMinY;
                            drawState = 1;
                            continue;
                        }
                    }



                    x = sepFixW * (sepX - 1);

                    y = sepFixH * (sepY - 1);

                    if (sepX == sepMax)
                    {
                        sepFixWD = sepFixWDL;
                    }
                    else
                    {
                        sepFixWD = sepFixW;
                    }

                    if (sepY == sepMax)
                    {
                        sepFixHD = sepFixHDL;
                    }
                    else
                    {
                        sepFixHD = sepFixH;
                    }

                    Rectangle DestRect = new Rectangle(x, y, sepFixWD, sepFixHD);
                    Rectangle SrcRect = new Rectangle(x + left, y + top, sepFixWD, sepFixHD);
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);

                    System.Threading.Thread.Sleep(SleepTime);
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

                    System.Threading.Thread.Sleep(SleepTime);
                    count = (float)(count + 1.0 / SepNumber);
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
                int sepFix = GetShowSepFix(360);
                while (MyAngle <= 360)
                {
                    TextureBrush MyBrush = new TextureBrush(bmpSource);
                    MyBrush.RotateTransform(MyAngle);
                    g.FillRectangle(MyBrush, 0, 0, bmpSource.Width, bmpSource.Height);
                    MyAngle += sepFix;
                    System.Threading.Thread.Sleep(SleepTime);
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
            Graphics gBmpBack = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度
                g = picBox.CreateGraphics();
                
                    Bitmap bmpBack = ResizeBitmap(ShowApp.DrawBitmap, new Size(width, height), FillOptionStyle.Fill);
                gBmpBack = Graphics.FromImage(bmpBack);
                gBmpBack.InterpolationMode = InterpolationMode.HighQualityBicubic;

                int sepFix = GetShowSepFix(width);
                int sepNumber = 0;
                int x = 0;
                while (x < width / 2)
                {
                    gBmpBack.DrawImage(bmpSource, new Rectangle(x, 0, sepFix, height), new Rectangle(x, 0, sepFix, height), GraphicsUnit.Pixel);

                    gBmpBack.DrawImage(bmpSource, new Rectangle(width - x - sepFix, 0, sepFix, height), new Rectangle(width - x - sepFix, 0, sepFix, height), GraphicsUnit.Pixel);

                    g.DrawImage(bmpBack, left, top);
                    System.Threading.Thread.Sleep(SleepTime);


                    sepNumber++;
                    x = sepFix * sepNumber;
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
                if (null != gBmpBack)
                {
                    gBmpBack.Dispose();
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
            Graphics gBmpBack = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度
                g = picBox.CreateGraphics();

                Bitmap bitmap = new Bitmap(width, height);


                Bitmap bmpBack = ResizeBitmap(ShowApp.DrawBitmap, new Size(width, height), FillOptionStyle.Fill);
                gBmpBack = Graphics.FromImage(bmpBack);
                gBmpBack.InterpolationMode = InterpolationMode.HighQualityBicubic;

                int sepFix = GetShowSepFix(height);
                int sepNumber = 0;
                int y = 0;
                while (y < height / 2)
                {
                    gBmpBack.DrawImage(bmpSource, new Rectangle(0, y, width, sepFix), new Rectangle(0, y, width, sepFix), GraphicsUnit.Pixel);

                    gBmpBack.DrawImage(bmpSource, new Rectangle(0, height - y - sepFix, width, sepFix), new Rectangle(0, height - y - sepFix, width, sepFix), GraphicsUnit.Pixel);

                    g.DrawImage(bmpBack, left, top);
                    System.Threading.Thread.Sleep(SleepTime);


                    sepNumber++;
                    y = sepFix * sepNumber;
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
                if (null != gBmpBack)
                {
                    gBmpBack.Dispose();
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
            Graphics gBmpBack = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();

                int sepFix = GetShowSepFix(width) * 2;
                int sepNumber = 1;
                int x = width - sepFix;

                if (ShowApp.DrawBitmap != null)
                {
                    Bitmap bmpOld = ResizeBitmap(ShowApp.DrawBitmap, new Size(picBox.Width, picBox.Height), FillOptionStyle.Fill);
                    Bitmap bmpBack = new Bitmap(picBox.Width, picBox.Height);
                    gBmpBack = Graphics.FromImage(bmpBack);


                    while (x > 0)
                    {
                        Rectangle DestRect = new Rectangle((width - x) / 2, 0, x, height);
                        Rectangle SrcRect = new Rectangle(0, 0, bmpBack.Width, bmpBack.Height);

                        gBmpBack.Clear(BackClearColor); //初始为全灰色
                        gBmpBack.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gBmpBack.DrawImage(bmpOld, DestRect, SrcRect, GraphicsUnit.Pixel);

                        g.DrawImage(bmpBack, left, top);
                        System.Threading.Thread.Sleep(SleepTime);


                        sepNumber++;
                        x = width - sepFix * 4 * sepNumber;
                    }
                }

                sepNumber = 1;
                x = sepFix;
                while (x < width)
                {
                    Rectangle DestRect = new Rectangle((width - x) / 2, 0, x, height);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    x = sepFix * 2 * sepNumber;
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
                if (null != gBmpBack)
                {
                    gBmpBack.Dispose();
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
            Graphics gBmpBack = null;
            try
            {
                int width = bmpSource.Width; //图像宽度    
                int height = bmpSource.Height; //图像高度
                int left = (picBox.Size.Width - width) / 2; //图像宽度    
                int top = (picBox.Size.Height - height) / 2; //图像高度

                g = picBox.CreateGraphics();

                int sepFix = GetShowSepFix(height) * 2;
                int sepNumber = 1;
                int y = height - sepFix;
                if (ShowApp.DrawBitmap != null)
                {
                    Bitmap bmpOld = ResizeBitmap(ShowApp.DrawBitmap, new Size(picBox.Width, picBox.Height), FillOptionStyle.Fill);
                    Bitmap bmpBack = new Bitmap(picBox.Width, picBox.Height);
                    gBmpBack = Graphics.FromImage(bmpBack);


                    while (y > 0)
                    {
                        Rectangle DestRect = new Rectangle(0, (height - y) / 2, width, y);
                        Rectangle SrcRect = new Rectangle(0, 0, bmpBack.Width, bmpBack.Height);

                        gBmpBack.Clear(BackClearColor); //初始为全灰色
                        gBmpBack.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gBmpBack.DrawImage(bmpOld, DestRect, SrcRect, GraphicsUnit.Pixel);

                        g.DrawImage(bmpBack, left, top);
                        System.Threading.Thread.Sleep(SleepTime);
                        
                        sepNumber++;
                        y = height - sepFix * 4 * sepNumber;
                    }
                }

                sepNumber = 1;
                y = sepFix;
                while (y < height)
                {
                    Rectangle DestRect = new Rectangle(0, (height - y) / 2, width, y);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    y = sepFix * 2 * sepNumber;
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
                if (null != gBmpBack)
                {
                    gBmpBack.Dispose();
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


                double sepFix = GetShowSepFix2(width / 2);
                int sepNumber = 1;
                int x = (int)Math.Ceiling(sepFix);

                while (x < width / 2)
                {
                    int j = Convert.ToInt32(x * (Convert.ToSingle(height) / Convert.ToSingle(width)));
                    Rectangle DestRect = new Rectangle(width / 2 - x, height / 2 - j, 2 * x, 2 * j);
                    Rectangle SrcRect = new Rectangle(0, 0, bmpSource.Width, bmpSource.Height);

                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    x = (int)Math.Ceiling(Math.Pow(sepFix, sepNumber));
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


                int sepFix = GetShowSepFix(height);
                int sepNumber = 1;
                int y = sepFix;
                while (y < height)
                {

                    Rectangle DestRect = new Rectangle(0, y, width, sepFix);
                    Rectangle SrcRect = new Rectangle(0, y, width, sepFix);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);


                    sepNumber++;
                    y = sepFix * sepNumber;
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


                int sepFix = GetShowSepFix(height);
                int sepNumber = 1;
                int y = height - sepFix;
                while (y > 0)
                {

                    Rectangle DestRect = new Rectangle(0, y, width, sepFix);
                    Rectangle SrcRect = new Rectangle(0, y, width, sepFix);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    y = height - (sepFix * sepNumber);
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

                int sepFix = GetShowSepFix(width);
                int sepNumber = 1;
                int x = sepFix;
                while (x < width)
                {
                    Rectangle DestRect = new Rectangle(x, 0, sepFix, height);
                    Rectangle SrcRect = new Rectangle(x, 0, sepFix, height);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);

                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    x = sepFix * sepNumber;
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


                int sepFix = GetShowSepFix(width);
                int sepNumber = 1;
                int x = width - sepFix;
                while (x > 0)
                {
                    Rectangle DestRect = new Rectangle(x, 0, sepFix, height);
                    Rectangle SrcRect = new Rectangle(x, 0, sepFix, height);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmpSource, DestRect, SrcRect, GraphicsUnit.Pixel);
                    System.Threading.Thread.Sleep(SleepTime);

                    sepNumber++;
                    x = width - (sepFix * sepNumber);
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
        /// 显示图像
        /// </summary>
        /// <param name="bmpSource">Bitmap 对象</param>
        /// <param name="picBox">PictureBox 对象</param>
        public static void ShowBitmap(Bitmap bmpSource, PictureBox picBox, ImageShowStyle imgShowStyle)
        {

            if (ShowApp.DrawBitmapFlag)
            {
                return;
            }

            Graphics g = null;
            //显示图像
            try
            {
                ShowApp.DrawBitmapFlag = true;

                LogApp.OutputProcessLog("ImageApp", "ShowBitmap", "this BitMap");

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
                    g.DrawImage(bmpSource, left, top);
                }

                
                ShowApp.DrawBitmap = new Bitmap(bmpSource.Width, bmpSource.Height);
                ImageApp.CopyBitmap(ShowApp.DrawBitmap, bmpSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ShowApp.DrawBitmapFlag = false;
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

                    Bitmap bitmap = new Bitmap(width, height);
                    ImageApp.CopyBitmap(bitmap, bmpSource);

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
        #endregion

        #region " 新建图像 " 

        /// <summary>
        /// 新建图像
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

        #region " 读取图像 " 
        /// <summary>
        /// 从流中读取图像
        /// </summary>
        /// <param name="bmpFile"></param>
        public static Image GetBitmap(string bmpFile)
        {
            //显示图像
            Image Image;
            try
            {

                using (FileStream pFileStream = new FileStream(bmpFile, FileMode.Open, FileAccess.Read))
                {
                    Image = Image.FromStream(pFileStream);

                    pFileStream.Close();
                    pFileStream.Dispose();
                }

                return Image;
            }
            catch (Exception ex)
            {
                throw ex;
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
                    , ImageShowStyle.Rotate.GetHashCode()
                    , ImageShowStyle.Gradient.GetHashCode()
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
