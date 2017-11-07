using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateClient
{
    public static class PlayApp
    {
        public static List<PlayItem> PlayItemList = new List<PlayItem>();

        public static int CurrentIndex = -1;

        public static void Clear()
        {
            PlayItemList.Clear();
            CurrentIndex = -1;
        }

        public static PlayItem CurrentPlayItem()
        {
            if (CurrentIndex >= 0)
            {
                return PlayItemList[CurrentIndex];
            }
            else
            {
                return null;
            }
        }
    }

    public class PlayItem
    {
        #region " variable "

        private ShowPlayType playTypeValue = ShowPlayType.Image;

        private List<TempleteItem> templeteItemListValue = new List<TempleteItem>();

        private int currentTempleteItemIndex = -1;

        private int nowTempleteItemIndex = -1;


        /// <summary>
        /// 时间
        /// </summary>
        private DateTime dtNowValue = DateTime.Now;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime dtStartValue = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime dtEndValue = DateTime.Now;


        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycleNumberValue = 0;
        /// <summary>
        /// 循环总数
        /// </summary>
        private int cycleTotalNumberValue = 1;

        /// <summary>
        /// 结束时间有效
        /// </summary>
        private bool endDateTimeValidValue = true;
        /// <summary>
        /// 循环总数有效
        /// </summary>
        private bool cycleTotalNumberValidValue = true;
        #endregion


        #region " propert "

        public ShowPlayType PlayType
        {
            get
            {
                return playTypeValue;
            }
            set
            {
                playTypeValue = value;
            }
        }
        public List<TempleteItem> TempleteItemList
        {
            get
            {
                return templeteItemListValue;
            }
        }
        public int CurrentTempleteIndex
        {
            get
            {
                return currentTempleteItemIndex;
            }
            set
            {
                currentTempleteItemIndex = value;
            }
        }
        public int NowTempleteIndex
        {
            get
            {
                return nowTempleteItemIndex;
            }
            set
            {
                nowTempleteItemIndex = value;
            }
        }
        public TempleteItem CurrentTempleteItem
        {
            get
            {
                if (currentTempleteItemIndex >= 0)
                {
                    return templeteItemListValue[currentTempleteItemIndex];
                }
                else
                {
                    return null;
                }
            }
        }
        public TempleteItem NextTempleteItem
        {
            get
            {
                if (currentTempleteItemIndex >= -1 && currentTempleteItemIndex < templeteItemListValue.Count - 1)
                {
                    return templeteItemListValue[currentTempleteItemIndex + 1];
                }
                else
                {
                    return null;
                }
            }
        }

        public TempleteItem LastTempleteItem
        {
            get
            {
                if (templeteItemListValue.Count > 0)
                {
                    return templeteItemListValue[templeteItemListValue.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime NowDateTime
        {
            get
            {
                return dtNowValue;
            }
            set
            {
                dtNowValue = value;
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                return dtStartValue;
            }
            set
            {
                dtStartValue = value;
                dtNowValue = dtStartValue;
            }
        }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDateTime
        {
            get
            {
                return dtEndValue;
            }
            set
            {
                dtEndValue = value;
            }
        }


        /// <summary>
        /// 循环次数
        /// </summary>
        public int CycleNumber
        {
            get
            {
                return cycleNumberValue;
            }
            set
            {
                cycleNumberValue = value;
            }
        }

        /// <summary>
        /// 循环总数
        /// </summary>
        public int CycleTotalNumber
        {
            get
            {
                return cycleTotalNumberValue;
            }
            set
            {
                cycleTotalNumberValue = value;
            }
        }


        /// <summary>
        /// 结束时间有效
        /// </summary>
        public bool EndDateTimeValid
        {
            get
            {
                return endDateTimeValidValue;
            }
            set
            {
                endDateTimeValidValue = value;
            }
        }


        /// <summary>
        /// 循环总数有效
        /// </summary>
        public bool CycleTotalNumberValid
        {
            get
            {
                return cycleTotalNumberValidValue;
            }
            set
            {
                cycleTotalNumberValidValue = value;
            }
        }
        #endregion


        #region " Init "

        public PlayItem()
        {
           
        }
        public PlayItem(ShowPlayType playType)
        {
            playTypeValue = playType;
        }
        
        public PlayItem(ShowPlayType playType, DateTime dtStart, DateTime dtEnd, int cycleTotalNumber)
        {
            playTypeValue = playType;

            dtStartValue = dtStart;
            dtEndValue = dtEnd;
            this.endDateTimeValidValue = true;


            cycleTotalNumberValue = cycleTotalNumber;
            if (cycleTotalNumberValue > 0)
            {
                this.cycleTotalNumberValidValue = true;
            }
            else
            {
                this.cycleTotalNumberValidValue = false;
            }
        }

        public PlayItem(ShowPlayType playType, DateTime dtStart, int cycleTotalNumber)
        {
            playTypeValue = playType;

            dtStartValue = dtStart;
            this.endDateTimeValidValue = false;
            

            cycleTotalNumberValue = cycleTotalNumber;
            if (cycleTotalNumberValue > 0)
            {
                this.cycleTotalNumberValidValue = true;
            }
            else
            {
                this.cycleTotalNumberValidValue = false;
            }
        }
        #endregion

        #region " void and function "

        /// <summary>
        /// 右左
        /// </summary>
        public ShowPlayStateType PlayState
        {
           get
            {
                try
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);
                    
                    if (this.dtStartValue> nowTime)
                    {
                        return ShowPlayStateType.Stop;
                    }

                    if (this.endDateTimeValidValue
                       && this.dtEndValue <= nowTime)
                    {
                        return ShowPlayStateType.Stop;
                    }

                    if (this.cycleTotalNumberValidValue
                       && this.cycleNumberValue>= cycleTotalNumberValue)
                    {
                        return ShowPlayStateType.Stop;
                    }
                    return ShowPlayStateType.Show;
                }
                catch (Exception ex)
                {
                    return ShowPlayStateType.Stop;
                }
            }
        }

        public void ShowNowTemplete(PictureBox picImage)
        {
            try
            {
                this.nowTempleteItemIndex = this.currentTempleteItemIndex;


                if (picImage.Image != null)
                {
                    picImage.Image.Dispose();
                }
                //动态添加图片 
                Image nowImageFile = Image.FromFile(this.CurrentTempleteItem.File);

                Bitmap nowBitmap = new Bitmap(nowImageFile);

                nowBitmap = ImageApp.ResizeBitmap(nowBitmap, picImage.Size);

                ImageApp.ShowBitmap(nowBitmap, picImage, this.CurrentTempleteItem.ImageStyle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }


    public class TempleteItem
    {

        #region " variable "

        private string fileValue = string.Empty;

        private string messageValue = string.Empty;

        private ImageShowStyle imageStyleValue = ImageShowStyle.None;

        private MediaShowStyle mediaStyleValue = MediaShowStyle.None;

        private MessageShowStyle messageStyleValue = MessageShowStyle.None;


        /// <summary>
        /// 几秒后开始
        /// </summary>
        private int startSecondValue = 0;

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        private int durationSecondValue = 0;
        

        #endregion


        #region " propert "

        public string File
        {
            get
            {
                return fileValue;
            }
        }

        public string Message
        {
            get
            {
                return messageValue;
            }
        }
        public ImageShowStyle ImageStyle
        {
            get
            {
                return imageStyleValue;
            }
        }
        public MessageShowStyle MessageStyle
        {
            get
            {
                return messageStyleValue;
            }
        }
        public MediaShowStyle MediaStyle
        {
            get
            {
                return mediaStyleValue;
            }
        }

        /// <summary>
        /// 几秒后开始
        /// </summary>
        public int StartSecond
        {
            get
            {
                return startSecondValue;
            }
        }
        /// <summary>
        /// 几秒后结束
        /// </summary>
        public int EndSecond
        {
            get
            {
                return startSecondValue + durationSecondValue;
            }
        }

        /// <summary>
        /// 结束时间可用
        /// </summary>
        public bool EndSecondValid
        {
            get
            {
                return durationSecondValue>0;
            }
        }
        
        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        public int DurationSecond
        {
            get
            {
                return durationSecondValue;
            }
        }
        #endregion


        public TempleteItem(string file, MediaShowStyle mediaStyle, int startSecond, int durationSecond)
        {
            this.fileValue = file;
            this.mediaStyleValue = mediaStyle;
            this.startSecondValue = startSecond;
            this.durationSecondValue = durationSecond;
        }
        public TempleteItem(string file, MediaShowStyle mediaStyle)
        {
            this.fileValue = file;
            this.mediaStyleValue = mediaStyle;
            this.startSecondValue = 0;
            this.durationSecondValue=0;
        }

        public TempleteItem(string file, ImageShowStyle imageStyle, int startSecond, int durationSecond)
        {
            this.fileValue = file;
            this.imageStyleValue = imageStyle;
            this.startSecondValue = startSecond;
            this.durationSecondValue = durationSecond;
        }
        public TempleteItem(string file, ImageShowStyle imageStyle)
        {
            this.fileValue = file;
            this.imageStyleValue = imageStyle;
            this.startSecondValue = 0;
            this.durationSecondValue = 0;
        }
        public TempleteItem(string message, MessageShowStyle messageStyle, int startSecond, int durationSecond)
        {
            this.messageValue = message;
            this.messageStyleValue = messageStyle;
            this.startSecondValue = startSecond;
            this.durationSecondValue = durationSecond;
        }
        public TempleteItem(string message, MessageShowStyle messageStyle)
        {
            this.messageValue = message;
            this.messageStyleValue = messageStyle;
            this.startSecondValue = 0;
            this.durationSecondValue = 0;
        }

        #region " void and function "

        #endregion
    }

    #region play类型
    /// <summary>
    /// play类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowPlayType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("写真")]
        Image = 0,
        /// <summary>
        /// 文字消息
        /// </summary>
        [EnumDescription("文字")]
        Message = 1,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("映像")]
        Media = 2,
    }

    #endregion


    #region play状态类型
    /// <summary>
    /// play状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowPlayStateType
    {
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 0,
        /// <summary>
        /// 播放
        /// </summary>
        [EnumDescription("放送")]
        Show = 1,
    }

    #endregion

    #region 模板类型
    /// <summary>
    /// 模板类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowTempleteType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("写真")]
        Image = 0,
        /// <summary>
        /// 文字消息
        /// </summary>
        [EnumDescription("文字")]
        Message = 1,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("映像")]
        Media = 2,
    }

    #endregion

}
