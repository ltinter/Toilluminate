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
        public static List<PlayList> PlayListArray = new List<PlayList>();

        public static int CurrentIndex = -1;

        public static PlayList NowPlayList = null;

        public static void Clear()
        {
            PlayListArray.Clear();
            CurrentIndex = -1;
        }


        public static bool ExecutePlayList()
        {
            if (NowPlayList != CurrentPlayList())
            {
                NowPlayList = CurrentPlayList();
                NowPlayList.PlayStart();
                return true;
            }
            return false;
        }

        public static PlayList CurrentPlayList()
        {
            if (PlayListArray.Count > 0 && CurrentIndex >= 0 && CurrentIndex < PlayListArray.Count)
            {
                return PlayListArray[CurrentIndex];
            }
            return null;
        }

        public static bool CurrentPlayValid()
        {
            if (PlayListArray.Count > 0 && CurrentIndex >= 0 && CurrentIndex < PlayListArray.Count)
            {
                return true;
            }
            return false;
        }
    }


    public class PlayList
    {
        #region " variable "

        private List<TempleteItem> templeteItemListValue = new List<TempleteItem>();

        private int currentTempleteItemIndex = -1;

        private int nowTempleteItemIndex = -1;

        private PlayListStateType playListStateValue = PlayListStateType.Wait;
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
        /// 循环有效
        /// </summary>
        private bool loopPlayValidValue = true;

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        private int intervalSecondValue = 0;

        #endregion


        #region " propert "


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
        }




        /// <summary>
        /// 循环有效
        /// </summary>
        public bool LoopPlayValid
        {
            get
            {
                return loopPlayValidValue;
            }
            set
            {
                loopPlayValidValue = value;
            }
        }
        #endregion


        #region " Init "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loopPlayValid">循环有效</param>
        /// <param name="intervalSecond">持续时间(秒)</param>
        public PlayList(bool loopPlayValid, DateTime dtStart, int intervalSecond)
        {
            loopPlayValidValue = loopPlayValid;
            intervalSecondValue = intervalSecond;
            dtStartValue = Utility.GetPlayDateTime(dtStart);
            dtEndValue = dtStartValue.AddSeconds(this.intervalSecondValue);
        }
        #endregion

        /// <summary>
        /// 播放
        /// </summary>
        public void PlayStart()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);
            playListStateValue = PlayListStateType.Execute;
        }
        public void PlayStop()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);
            if (loopPlayValidValue == false)
            {
                dtEndValue = dtNowValue;
                playListStateValue = PlayListStateType.Stop;
            }
        }
        public void PlayRefreshTemplete()
        {
            this.currentTempleteItemIndex = 0;
            foreach (TempleteItem titem in this.TempleteItemList)
            {
                titem.ExecuteRefresh();
            }
        }
        public void PlayNextTemplete()
        {
            while (this.currentTempleteItemIndex < this.TempleteItemList.Count && this.CurrentTempleteItem.TempleteState == TempleteStateType.Stop)
            {
                this.currentTempleteItemIndex++;
            }
        }
        #region " void and function "
        /// <summary>
        /// 状态
        /// </summary>
        public PlayListStateType PlayListState
        {
            get
            {
                try
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    if (playListStateValue == PlayListStateType.Wait)
                    {
                        if (this.dtStartValue <= nowTime && this.dtEndValue >= nowTime)
                        {
                            playListStateValue = PlayListStateType.Execute;
                        }
                    }

                    if (playListStateValue != PlayListStateType.Stop)
                    {
                        if (this.dtEndValue <= nowTime)
                        {
                            playListStateValue = PlayListStateType.Stop;
                        }
                    }

                    return playListStateValue;
                }
                catch (Exception ex)
                {
                    return PlayListStateType.Stop;
                }
            }
        }
        #endregion
    }



    public class TempleteItem
    {

        #region " variable "

         protected List<string> fileOrMessageListValue = new List<string>() { };

        protected List<ImageShowStyle> imageStyleListValue = new List<ImageShowStyle>() { };

        protected int currentIndex = -1;

        protected int currentShowStyleIndex = -1;

        protected MediaShowStyle mediaStyleValue = MediaShowStyle.None;

        protected MessageShowStyle messageStyleValue = MessageShowStyle.None;


        protected TempleteType templeteTypeValue = TempleteType.Image;


        protected TempleteStateType templeteStateValue  = TempleteStateType.Wait;

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        protected int intervalSecondValue = 5;


        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime dtStartValue = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime dtEndValue = DateTime.Now;
        #endregion


        #region " propert "

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                return dtStartValue;
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
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public TempleteType TempleteType
        {
            get
            {
                return templeteTypeValue;
            }
        }

        public TempleteStateType TempleteState
        {
            get
            {                
                DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);
                if (templeteTypeValue == TempleteType.Image)
                {
                    if (this.fileOrMessageListValue.Count == 0)
                    {
                        templeteStateValue = TempleteStateType.Stop;
                    }
                    else
                    {
                        if (this.currentIndex > -1)
                        {
                            if (nowTime < dtStartValue)
                            {
                                templeteStateValue = TempleteStateType.Wait;
                            }
                            else
                            {
                                if (nowTime <= dtEndValue)
                                {
                                    templeteStateValue = TempleteStateType.Execute;
                                }
                                else
                                {
                                    templeteStateValue = TempleteStateType.Stop;
                                }
                            }
                        }
                    }
                }
                return templeteStateValue;
            }
        }


        #endregion


        public TempleteItem(string file, MediaShowStyle mediaStyle)
        {
            templeteTypeValue = TempleteType.Media;
            fileOrMessageListValue.Add(file);
            this.mediaStyleValue = mediaStyle;
        }

        public TempleteItem(List<string> fileList, List<ImageShowStyle> imageStyleList, int intervalSecond)
        {
            templeteTypeValue = TempleteType.Image;
            foreach (string file in fileList)
            {
                fileOrMessageListValue.Add(file);
            }
            foreach (ImageShowStyle style in imageStyleList)
            {
                imageStyleListValue.Add(style);
            }
            this.intervalSecondValue = intervalSecond;
        }

        public TempleteItem(List<string> messageList, MessageShowStyle messageStyle, int intervalSecond)
        {
            templeteTypeValue = TempleteType.Message;
            foreach (string message in messageList)
            {
                fileOrMessageListValue.Add(message);
            }
            this.messageStyleValue = messageStyle;
            this.intervalSecondValue = intervalSecond;
        }


        #region " void and function "
        public void ExecuteRefresh()
        {
            dtStartValue = Utility.GetPlayDateTime(DateTime.Now);
            dtEndValue = dtStartValue;

            templeteStateValue = TempleteStateType.Wait;

            this.currentIndex = -1;
            this.currentShowStyleIndex = -1;
        }
        
        #endregion
    }



    public class ImageTempleteItem : TempleteItem
    {

        #region " variable "

        
        #endregion


        #region " propert "

        public List<string> FileList
        {
            get
            {
                return fileOrMessageListValue;
            }
        }

        public List<ImageShowStyle> ImageStyleList
        {
            get
            {
                return imageStyleListValue;
            }
        }

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        public int DurationSecond
        {
            get
            {
                return intervalSecondValue * fileOrMessageListValue.Count;
            }
        }

        /// <summary>
        /// 间隔时间(秒)
        /// </summary>
        public int IntervalSecond
        {
            get
            {
                return intervalSecondValue;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        public int CurrentShowStyleIndex
        {
            get
            {
                return currentShowStyleIndex;
            }
        }


        public string CurrentFile
        {
            get
            {
                return this.FileList[this.currentIndex];
            }
        }

        public ImageShowStyle CurrentShowStyle
        {
            get
            {
                return ImageApp.GetImageShowStyle(this.imageStyleListValue[this.currentShowStyleIndex]);
            }
        }

        #endregion

        public ImageTempleteItem(List<string> fileList, List<ImageShowStyle> imageStyleList, int intervalSecond) : base(fileList, imageStyleList, intervalSecond)
        {
        }
        #region " void and function "

   
        /// <summary>
        /// 播放
        /// </summary>
        public void ExecuteStart()
        {
            if (this.TempleteState != TempleteStateType.Execute)
            {
                dtStartValue = Utility.GetPlayDateTime(DateTime.Now);
                dtEndValue = dtStartValue.AddSeconds(this.intervalSecondValue * this.fileOrMessageListValue.Count);
                this.currentIndex = -1;
            }
        }
        public void ExecuteStop()
        {
            dtEndValue = Utility.GetPlayDateTime(DateTime.Now);
            this.currentIndex = -1;
            this.currentShowStyleIndex = -1;
        }

        public bool CurrentIsChanged()
        {
            try
            {
                if (this.FileList.Count > 0)
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    if (nowTime >= this.dtStartValue && nowTime <= this.dtEndValue)
                    {
                        int nowIndex = this.currentIndex;
                        if (nowIndex < 0) nowIndex = 0;
                        while (nowIndex < this.fileOrMessageListValue.Count)
                        {
                            if (nowTime <= this.dtStartValue.AddSeconds(this.intervalSecondValue * (nowIndex + 1)))
                            {
                                break;
                            }
                            nowIndex++;
                        }

                        if (nowIndex >= this.fileOrMessageListValue.Count)
                        {
                            this.currentIndex = this.fileOrMessageListValue.Count - 1;
                            return false;
                        }
                        if (nowIndex != this.currentIndex)
                        {
                            this.currentIndex = nowIndex;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void ShowCurrent(PictureBox picImage)
        {
            Image nowImageFile = null;
                Bitmap nowBitmap = null;
            try
            {
                if (this.currentIndex >= 0 && this.currentIndex < this.fileOrMessageListValue.Count)
                {
                    currentShowStyleIndex++;
                    if (currentShowStyleIndex >= this.imageStyleListValue.Count)
                    {
                        currentShowStyleIndex = 0;
                    }

                    if (picImage.Image != null)
                    {
                        picImage.Image.Dispose();
                    }
                    //动态添加图片
                    nowImageFile = Image.FromFile(this.CurrentFile);

                    nowBitmap = new Bitmap(nowImageFile);

                    nowBitmap = ImageApp.ResizeBitmap(nowBitmap, picImage.Size);

                    ImageApp.ShowBitmap(nowBitmap, picImage, this.CurrentShowStyle);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (nowBitmap != null)
                {
                    nowBitmap.Dispose();
                }
                if (nowImageFile != null)
                {
                    nowImageFile.Dispose();
                }
            }
        }

        #endregion
    }



    #region play状态类型
    /// <summary>
    /// play状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum PlayListStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        Wait = 0,
        /// <summary>
        /// 播放
        /// </summary>
        [EnumDescription("放送")]
        Execute = 1,
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 2,
    }

    #endregion

    #region 模板类型
    /// <summary>
    /// 模板类型
    /// </summary>
    /// <remarks></remarks>
    public enum TempleteType
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

    #region templete状态类型
    /// <summary>
    /// templete状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum TempleteStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        Wait = 0,
        /// <summary>
        /// 播放
        /// </summary>
        [EnumDescription("放送")]
        Execute = 1,
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 2,
    }

    #endregion
}
