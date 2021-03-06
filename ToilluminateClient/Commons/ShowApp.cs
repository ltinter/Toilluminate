﻿using System.Collections.Generic;
using System.Drawing;
using AxAXVLC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ToilluminateClient
{
    public static class ShowApp
    {

        #region "Bitmap"
        public static Bitmap DrawBitmap;
        public static bool DrawBitmapFlag = false;

        #endregion

        #region "Message"
        public static Bitmap MessageBackBitmap;


        public static DrawMessage DownLoadDrawMessage = null;
        public static int DownLoadTotalNumber = 0;
        public static int DownLoadIndexNumber = 0;


        public static List<DrawMessage> DrawMessageList = new List<DrawMessage>();

        public static bool DrawMessageFlag = false;
        public static bool DrawMessageMoveFlag = false;
        #endregion

        #region "Trademark"
        public static Bitmap[] TrademarkBackBitmaps = new Bitmap[10];

        public static List<DrawTrademark> DrawTrademarkList = new List<DrawTrademark>();
        public static bool DrawTrademarkFlag = false;
        #endregion

        public static bool NowImageIsShow = false;
        public static bool NowMediaIsShow = false;



        public static bool NowMessageIsShow = false;
        public static bool NowTrademarkIsShow = false;


        public static bool NowMessageIsRefresh = false;

        public static bool NowTrademarkIsRefresh = false;

        public static TempleteItemType NowShowTempleteItemType = TempleteItemType.None;
        public static TempleteItemType NextShowTempleteItemType = TempleteItemType.None;


        /// <summary>
        /// 刷新下载进度信息
        /// </summary>
        public static void DownloadMessageRefresh()
        {
            try
            {
                if (ShowApp.DownLoadDrawMessage == null || ShowApp.DownLoadDrawMessage.DrawStyleList.Count == 0)
                {
                    string messageString = "<span style=\"font-family: MS PGothic; font-size: 18px; \" ><b>ファイルダウンロード中。</b></span>";
                    MessageTempleteItem mtItem = new MessageTempleteItem(messageString, MessagePositionType.Bottom, 0, 0);

                    ShowApp.DownLoadDrawMessage = new DrawMessage(VariableInfo.messageFormInstance.Width, VariableInfo.messageFormInstance.Height, mtItem);

                    ShowApp.DownLoadDrawMessage.AddDrawMessage(mtItem.MessageList[0], mtItem.MessageStyleList[0]);
                }
                else
                {
                    string messageString = string.Format("<span style=\"font-family: MS PGothic; font-size: 18px; \" ><b>ファイルダウンロード中。({0}/{1})</b></span>", ShowApp.DownLoadIndexNumber, ShowApp.DownLoadTotalNumber);
                    MessageTempleteItem mtItem = new MessageTempleteItem(messageString, MessagePositionType.Bottom, 0, 0);

                    ShowApp.DownLoadDrawMessage.SetDrawMessageStyle(mtItem.MessageList[0], mtItem.MessageStyleList[0], 0);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DownloadMessageRefresh", ex);
            }
        }

        public static void DownloadMessageCountTotalNumber(List<PlayListMaster> temp_PlayListMasterArray)
        {
            try
            {
                int totalNumber = 0;
                ShowApp.DownLoadIndexNumber = 0;
                ShowApp.DownLoadTotalNumber = 0;
                foreach (PlayListMaster plmItem in temp_PlayListMasterArray)
                {
                    PlayListSettings plsStudent = plmItem.plsStudent;

                    foreach (PlaylistItem pliTemlete in plsStudent.PlaylistItems)
                    {
                        if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Image.GetHashCode())
                        {
                            #region "Image"
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    totalNumber++;
                                }
                            }

                            #endregion
                        }
                        if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Message.GetHashCode())
                        {
                            #region "Message"

                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Media.GetHashCode())
                        {
                            #region "Media"
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    totalNumber++;
                                }
                            }
                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Trademark.GetHashCode())
                        {
                            #region "Trademark"
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    totalNumber++;
                                }
                            }
                            #endregion
                        }
                    }
                }

                ShowApp.DownLoadTotalNumber = totalNumber;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DownloadMessageCountTotalNumber", ex);
            }
        }


        public static void DownloadMessageDispose()
        {
            try
            {
                if (ShowApp.DownLoadDrawMessage != null && ShowApp.DownLoadDrawMessage.DrawStyleList.Count > 0)
                {
                    ShowApp.DownLoadDrawMessage = null;
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DownloadMessageDispose", ex);
            }
        }
    }


    #region " 前置信息 "
    #region " DrawImage 前置文本 "
    public class DrawMessage
    {

        #region " variable "
        private MessageTempleteItem parentTempleteValue;

        private List<DrawMessageStyle> drawStyleListValue = new List<DrawMessageStyle> { };


        private int leftValue;
        private int topValue;
        private int widthValue;
        private int heightValue;

        private int parentWidthValue;
        private int parentHeightValue;
        private bool needRefreshLocation = false;

        private int slidingSpeedValue = 10;

        private MoveDirectionStyle moveDirectionValue = MoveDirectionStyle.RightToLeft;

        private MoveStateType moveState = MoveStateType.NotMove;


        /// <summary>
        /// 纵向分隔距离
        /// </summary>
        private int verticalRangeValue = 30;
        /// <summary>
        /// 横向分隔距离
        /// </summary>
        private int horizontalRangeValue = 30;
        #endregion


        #region " propert "
        /// <summary>
        /// 纵向分隔距离
        /// </summary>
        public int VerticalRange
        {
            get
            {
                return verticalRangeValue;
            }
        }
        /// <summary>
        /// 横向分隔距离
        /// </summary>
        public int HorizontalRange
        {
            get
            {
                return horizontalRangeValue;
            }
        }
        public List<DrawMessageStyle> DrawStyleList
        {
            get
            {
                return drawStyleListValue;
            }
        }

        public MoveStateType MoveState
        {
            get
            {
                return moveState;
            }
        }

        public int Left
        {
            get
            {
                return leftValue;
            }
        }
        public int Height
        {
            get
            {
                return heightValue;
            }
        }
        public int Top
        {
            get
            {
                return topValue;
            }
        }

        public int Width
        {
            get
            {
                return widthValue;
            }
        }
        public MessagePositionType ShowStyle
        {
            get
            {
                return this.parentTempleteValue.ShowStyle;
            }
        }
        #endregion


        public DrawMessage(int parentWidth, int parentHeight, MessageTempleteItem parentTemplete, int verticalRange, int horizontalRange)
        {
            drawStyleListValue.Clear();
            this.widthValue = 0;

            this.leftValue = parentWidth;
            this.parentTempleteValue = parentTemplete;


            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;
            this.slidingSpeedValue = parentTemplete.SlidingSpeed;


            this.verticalRangeValue = verticalRange;
            this.horizontalRangeValue = horizontalRange;

            this.needRefreshLocation = true;
            RefreshLocation();
        }
        public DrawMessage(int parentWidth, int parentHeight, MessageTempleteItem parentTemplete)
        {
            drawStyleListValue.Clear();
            this.widthValue = 0;

            this.leftValue = parentWidth;
            this.parentTempleteValue = parentTemplete;


            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;
            this.slidingSpeedValue = parentTemplete.SlidingSpeed;

            this.needRefreshLocation = true;
            RefreshLocation();
        }

        #region " void and function "
        public void AddDrawMessage(string message, MessageStyle drawStyle)
        {
            this.drawStyleListValue.Add(new ToilluminateClient.DrawMessageStyle(message, widthValue, drawStyle.Font, drawStyle.Color, drawStyle.Width, drawStyle.Height));
            this.widthValue = this.widthValue + drawStyle.Width;
            if (this.heightValue < drawStyle.Height)
            {
                this.heightValue = drawStyle.Height;
            }
        }

        public void SetDrawMessageStyle(string message, MessageStyle drawStyle, int drawStyleIndex)
        {
            if (drawStyleIndex >= 0 && drawStyleIndex < this.drawStyleListValue.Count)
            {
                DrawMessageStyle drawMessageStyle = this.drawStyleListValue[drawStyleIndex];
                int oldWidthValue = drawMessageStyle.Width;
                drawMessageStyle.SetDrawMessageStyle(message, drawStyle.Width);

                this.widthValue = this.widthValue - oldWidthValue + drawMessageStyle.Width;

                if (this.heightValue < drawStyle.Height)
                {
                    this.heightValue = drawStyle.Height;
                }
                else
                {
                    this.heightValue = 0;
                    foreach (DrawMessageStyle drawStyleTemp in this.drawStyleListValue)
                    {
                        if (this.heightValue < drawStyleTemp.Height)
                        {
                            this.heightValue = drawStyleTemp.Height;
                        }
                    }
                }

                this.moveState = MoveStateType.NotMove;
            }
        }


        public void SetParentSize(int parentWidth, int parentHeight)
        {
            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;
            this.needRefreshLocation = true;
        }

        public void MoveMessage(int moveNumber)
        {

            if (this.slidingSpeedValue == 0 || IniFileInfo.MoveMessage == false || moveDirectionValue == MoveDirectionStyle.None)
            {
                #region "not move"
                if (parentTempleteValue.CheckTempleteState() == TempleteStateType.Stop)
                {
                    moveState = MoveStateType.MoveFinish;
                    parentTempleteValue.ExecuteStop();
                }
                else
                {
                    this.leftValue = (this.parentWidthValue - this.widthValue) / 2;
                    moveState = MoveStateType.Moving;
                }
                #endregion
            }
            else
            {
                #region "right left "
                if (moveDirectionValue == MoveDirectionStyle.RightToLeft || moveDirectionValue == MoveDirectionStyle.LeftToRight)
                {
                    int sepValue = (this.parentWidthValue + this.widthValue) / (moveNumber * this.slidingSpeedValue);
                    if (sepValue < 1)
                    {
                        sepValue = 1;
                    }
                    if (moveDirectionValue == MoveDirectionStyle.RightToLeft)
                    {
                        #region "right to left "
                        this.leftValue = this.leftValue - sepValue;
                        if (this.leftValue <= -this.widthValue)
                        {
                            if (parentTempleteValue.CheckTempleteState() == TempleteStateType.Stop)
                            {
                                moveState = MoveStateType.MoveFinish;
                                parentTempleteValue.ExecuteStop();
                            }
                            else
                            {
                                this.leftValue = this.parentWidthValue;
                                moveState = MoveStateType.Moving;
                            }

                        }
                        else
                        {
                            moveState = MoveStateType.Moving;
                        }
                        #endregion
                    }
                    else if (moveDirectionValue == MoveDirectionStyle.LeftToRight)
                    {
                        #region "right to left "
                        this.leftValue = this.leftValue + sepValue;
                        if (this.leftValue > this.parentWidthValue)
                        {
                            if (parentTempleteValue.CheckTempleteState() == TempleteStateType.Stop)
                            {
                                moveState = MoveStateType.MoveFinish;
                                parentTempleteValue.ExecuteStop();
                            }
                            else
                            {
                                this.leftValue = -this.widthValue;
                                moveState = MoveStateType.Moving;
                            }
                        }
                        else
                        {
                            moveState = MoveStateType.Moving;
                        }
                        #endregion
                    }
                }
                #endregion


                #region "down top "
                if (moveDirectionValue == MoveDirectionStyle.DownToTop || moveDirectionValue == MoveDirectionStyle.TopToDown)
                {
                    int sepValue = (this.parentHeightValue + this.heightValue) / (moveNumber * this.slidingSpeedValue);
                    if (sepValue < 1)
                    {
                        sepValue = 1;
                    }
                    if (moveDirectionValue == MoveDirectionStyle.DownToTop)
                    {
                        #region "down to top "
                        this.topValue = this.topValue - sepValue;
                        if (this.topValue <= -this.heightValue)
                        {
                            if (parentTempleteValue.CheckTempleteState() == TempleteStateType.Stop)
                            {
                                moveState = MoveStateType.MoveFinish;
                                parentTempleteValue.ExecuteStop();
                            }
                            else
                            {
                                this.topValue = this.parentHeightValue;
                                moveState = MoveStateType.Moving;
                            }

                        }
                        else
                        {
                            moveState = MoveStateType.Moving;
                        }
                        #endregion
                    }
                    else if (moveDirectionValue == MoveDirectionStyle.TopToDown)
                    {
                        #region "right to left "
                        this.topValue = this.topValue + sepValue;
                        if (this.topValue > this.parentHeightValue)
                        {
                            if (parentTempleteValue.CheckTempleteState() == TempleteStateType.Stop)
                            {
                                moveState = MoveStateType.MoveFinish;
                                parentTempleteValue.ExecuteStop();
                            }
                            else
                            {
                                this.topValue = -this.heightValue;
                                moveState = MoveStateType.Moving;
                            }
                        }
                        else
                        {
                            moveState = MoveStateType.Moving;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            if (needRefreshLocation)
            {
                RefreshLocation();
            }
        }

        private void RefreshLocation()
        {
            if (this.parentTempleteValue.ShowStyle == MessagePositionType.Top)
            {
                this.topValue = verticalRangeValue;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Bottom)
            {
                this.topValue = this.parentHeightValue - verticalRangeValue;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Middle)
            {
                this.topValue = this.parentHeightValue / 2;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Left)
            {
                this.leftValue = horizontalRangeValue;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Right)
            {
                this.leftValue = this.parentWidthValue - verticalRangeValue;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Center)
            {
                this.leftValue = this.parentWidthValue / 2;
            }

            this.needRefreshLocation = false;
        }

        public int GetStyleTop(int styleHeight)
        {
            int top = this.topValue;
            if (this.parentTempleteValue.ShowStyle == MessagePositionType.Top)
            {
                top = this.topValue;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Bottom)
            {
                top = this.topValue - styleHeight;
            }
            else if (this.parentTempleteValue.ShowStyle == MessagePositionType.Middle)
            {
                top = this.topValue - (styleHeight / 2);
            }
            return top;
        }

        public int GetStyleLeft(int styleLeftWidth)
        {
            int left = this.leftValue + styleLeftWidth;

            return left;
        }


        public bool CheckStyleShow(DrawMessageStyle dmsValue)
        {
            int left = GetStyleLeft(dmsValue.LeftWidth);

            if (left <= this.parentWidthValue && (left + dmsValue.Width) > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }


    public class DrawMessageStyle
    {

        #region " variable "
        private string messageValue = string.Empty;
        private int leftWidthValue;


        private Font fontValue;
        private Color colorValue;
        private int widthValue;
        private int heightValue;


        #endregion


        #region " propert "

        public string Message
        {
            get
            {
                return messageValue;
            }
        }
        public int LeftWidth
        {
            get
            {
                return leftWidthValue;
            }
        }
        public Font Font
        {
            get
            {
                return fontValue;
            }
        }
        public Color Color
        {
            get
            {
                return colorValue;
            }
        }
        public int Width
        {
            get
            {
                return widthValue;
            }
        }
        public int Height
        {
            get
            {
                return heightValue;
            }
        }

        #endregion

        public DrawMessageStyle(string message, int leftWidth, Font font, Color color, int width, int height)
        {

            this.messageValue = message;
            this.leftWidthValue = leftWidth;

            this.fontValue = font;
            this.colorValue = color;

            this.widthValue = width;
            this.heightValue = height;

        }
        public void SetDrawMessageStyle(string message, int width)
        {

            this.messageValue = message;

            this.widthValue = width;
        }
    }
    #endregion
    #region " DrawTrademark 前置商标 "
    public class DrawTrademark
    {

        #region " variable "
        private TrademarkTempleteItem parentTempleteValue;

        private List<DrawTrademarkStyle> drawStyleListValue = new List<DrawTrademarkStyle> { };

        private ShowStateType showStateValue = ShowStateType.NotShow;

        private int parentWidthValue = 0;
        private int parentHeightValue = 0;
        private bool            newParentSizeValue = true;
        /// <summary>
        /// 纵向分隔距离
        /// </summary>
        private int verticalRangeValue = 30;
        /// <summary>
        /// 横向分隔距离
        /// </summary>
        private int horizontalRangeValue = 30;
        #endregion


        #region " propert "
        /// <summary>
        /// 纵向分隔距离
        /// </summary>
        public int VerticalRange
        {
            get
            {
                return verticalRangeValue;
            }
        }
        /// <summary>
        /// 横向分隔距离
        /// </summary>
        public int HorizontalRange
        {
            get
            {
                return horizontalRangeValue;
            }
        }

        public List<DrawTrademarkStyle> DrawStyleList
        {
            get
            {
                return drawStyleListValue;
            }
        }

        public ShowStateType ShowState
        {
            get
            {
                return showStateValue;
            }
        }
        public Size ParentSize
        {
            get
            {
                this.newParentSizeValue = false;
                return new Size(this.parentWidthValue, this.parentHeightValue);
            }
        }
        public bool NewParentSize
        {
            get
            {
                return this.newParentSizeValue;
            }
        }
        #endregion

        public DrawTrademark(int parentWidth, int parentHeight, TrademarkTempleteItem parentTemplete)
        {
            drawStyleListValue.Clear();

            this.parentTempleteValue = parentTemplete;

            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;
            this.newParentSizeValue = true;

        }
        public DrawTrademark(int parentWidth, int parentHeight, TrademarkTempleteItem parentTemplete, int verticalRange, int horizontalRange)
        {
            drawStyleListValue.Clear();

            this.parentTempleteValue = parentTemplete;

            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;
            this.newParentSizeValue = true;

            this.verticalRangeValue = verticalRange;
            this.horizontalRangeValue = horizontalRange;

        }

        #region " void and function "
        public void AddDrawTrademark(string file, TrademarkStyle drawStyle)
        {
            drawStyleListValue.Add(new ToilluminateClient.DrawTrademarkStyle(file, drawStyle.Percent, drawStyle.TrademarkPosition));
        }


        public void SetParentSize(int parentWidth, int parentHeight)
        {
            this.parentHeightValue = parentHeight;
            this.parentWidthValue = parentWidth;

            this.newParentSizeValue = true;
        }

    

        public Point GetStyleLocation(DrawTrademarkStyle style)
        {
            int left = style.Left;
            int top = style.Top;

            if (style.TrademarkPosition == TrademarkPositionType.TopLeft
                || style.TrademarkPosition == TrademarkPositionType.TopCenter
                || style.TrademarkPosition == TrademarkPositionType.TopRight)
            {
                top = verticalRangeValue;
            }
            else if (style.TrademarkPosition == TrademarkPositionType.MiddleLeft
                || style.TrademarkPosition == TrademarkPositionType.MiddleCenter
                || style.TrademarkPosition == TrademarkPositionType.MiddleRight)
            {
                top = (this.parentHeightValue - style.Height(this.ParentSize)) / 2;
            }
            else if (style.TrademarkPosition == TrademarkPositionType.BottomLeft
                || style.TrademarkPosition == TrademarkPositionType.BottomCenter
                || style.TrademarkPosition == TrademarkPositionType.BottomRight)
            {
                top = this.parentHeightValue - verticalRangeValue - style.Height(this.ParentSize);
            }


            if (style.TrademarkPosition == TrademarkPositionType.TopLeft
                || style.TrademarkPosition == TrademarkPositionType.MiddleLeft
                || style.TrademarkPosition == TrademarkPositionType.BottomLeft)
            {
                left = horizontalRangeValue;
            }
            else if (style.TrademarkPosition == TrademarkPositionType.TopCenter
                || style.TrademarkPosition == TrademarkPositionType.MiddleCenter
                || style.TrademarkPosition == TrademarkPositionType.BottomCenter)
            {
                left = (this.parentWidthValue - style.Width(this.ParentSize)) / 2;
            }
            else if (style.TrademarkPosition == TrademarkPositionType.TopRight
                || style.TrademarkPosition == TrademarkPositionType.MiddleRight
                || style.TrademarkPosition == TrademarkPositionType.BottomRight)
            {
                left = this.parentWidthValue - horizontalRangeValue - style.Width(this.ParentSize);
            }

            if (left < 0) left = 0;
            if (top < 0) top = 0;

            return new Point(left, top);
        }

        public void ShowTrademark()
        {
            this.showStateValue = ShowStateType.Showing;
        }

        #endregion
    }


    public class DrawTrademarkStyle
    {

        #region " variable "
        private string fileValue = string.Empty;

        private int topValue = 0;
        private int leftValue = 0;


        private int percentValue = 0;
        private int widthValue = 0;
        private int heightValue = 0;

        private TrademarkPositionType trademarkPositionValue;
        #endregion


        #region " propert "

        public string File
        {
            get
            {
                return fileValue;
            }
        }
        public TrademarkPositionType TrademarkPosition
        {
            get
            {
                return trademarkPositionValue;
            }
        }
        public int Top
        {
            get
            {
                return topValue;
            }
        }
        public int Left
        {
            get
            {
                return topValue;
            }
        }
        public int SizePercent
        {
            get
            {
                return percentValue;
            }
        }


        #endregion

        public DrawTrademarkStyle(string file, int percent, int left, int top)
        {
            this.fileValue = file;
            this.percentValue = percent;

            Size size = ImageApp.GetBitmapSize(file);
            this.widthValue = size.Width;
            this.heightValue = size.Height;

            this.leftValue = left;
            this.topValue = top;
            this.trademarkPositionValue = TrademarkPositionType.None;
        }
        public DrawTrademarkStyle(string file, int percent, TrademarkPositionType trademarkPosition)
        {
            this.fileValue = file;
            this.percentValue = percent;

            Size size = ImageApp.GetBitmapSize(file);
            this.widthValue = size.Width;
            this.heightValue = size.Height;

            this.leftValue = 0;
            this.topValue = 0;
            this.trademarkPositionValue = trademarkPosition;
        }


        public int Width(Size parentSize)
        {
            int width = widthValue;
            if (this.percentValue > 0)
            {
                width = parentSize.Width * percentValue / 100;
            }
            return width;

        }
        public int Height(Size parentSize)
        {
            int height = heightValue;
            if (this.percentValue > 0)
            {
                height = heightValue * parentSize.Width * percentValue / widthValue / 100;
            }
            return height;
        }
    }
    #endregion
    #endregion

}
