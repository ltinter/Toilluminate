using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ToilluminateClient
{
    public static class PlayApp
    {
        public static String CurrentPlayListJsonString = string.Empty;

        private static PlayList executePlayList;
        public static List<PlayList> PlayListArray = new List<PlayList>();

        public static List<PlayListMaster> PlayListMasterArray = new List<PlayListMaster>();

        private static int CurrentPlayListIndex = -1;
        private static int CurrentPlayListID = -1;
        private static int ExecutePlayListID = -1;

        public static bool RefreshPlayList = false;


        public static int ThreadLoadPlayListTime = 5;
        public static int ThreadLoadPlayListTimeCurrent = 0;


        public static Bitmap DrawBitmap;
        public static bool DrawBitmapFlag = false;

        public static List<DrawMessage> DrawMessageList = new List<DrawMessage>();
        public static bool DrawMessageFlag = false;

        public static bool NowImageIsShow = false;
        public static bool NowMediaIsShow = false;
        public static bool NowMessageIsShow = false;

        public static void Clear()
        {
            PlayListArray.Clear();
            CurrentPlayListIndex = -1;
            CurrentPlayListID = -1;
            ExecutePlayListID = -1;
            executePlayList = null;
        }
        public static PlayList ExecutePlayList
        {
            get
            {
                return executePlayList;
            }
        }

        public static bool ExecutePlayListStart()
        {
            if (ExecutePlayListID != CurrentPlayListID)
            {
                executePlayList = CurrentPlayList();
                executePlayList.PlayStart();

                ExecutePlayListID = CurrentPlayListID;
                return true;
            }
            return false;
        }

        public static PlayList CurrentPlayList()
        {
            if (PlayListArray.Count > 0 && CurrentPlayListIndex >= 0 && CurrentPlayListIndex < PlayListArray.Count)
            {
                return PlayListArray[CurrentPlayListIndex];
            }
            return null;
        }

        public static bool CurrentPlayValid()
        {
            if (PlayListArray.Count > 0)
            {
                for (int i = 0; i < PlayListArray.Count; i++)
                {
                    PlayList plItem = PlayListArray[i];
                    if (plItem.PlayListState != PlayListStateType.Stop)
                    {
                        CurrentPlayListIndex = i;
                        CurrentPlayListID = plItem.PlayListID;
                        return true;
                    }
                }
            }

            return false;
        }
    }


    public class PlayList
    {
        #region " variable "

        private List<MessageTempleteItem> mTempleteItemListValue = new List<MessageTempleteItem>();

        private List<TempleteItem> templeteItemListValue = new List<TempleteItem>();

        private int currentTempleteItemIndex = -1;

        private int nowTempleteItemIndex = -1;

        private PlayListStateType playListStateValue = PlayListStateType.Wait;


        PlayListSettings plSettings = new PlayListSettings();

        private int playListIDValue = -1;
        /// <summary>
        /// 时间
        /// </summary>
        private DateTime dtNowValue = DateTime.Now;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime dtStartValue = DateTime.Now;

        /// <summary>
        /// 循环有效
        /// </summary>
        private bool loopPlayValidValue = true;

        /// <summary>
        /// 强制播放时间有效
        /// </summary>
        private bool fixPlayTimeValidValue = false;

        /// <summary>
        /// 强制播放时间(秒)
        /// </summary>
        private int fixPlayTimeValue = 0;

        #endregion


        #region " propert "

        public int PlayListID
        {
            get
            {
                return playListIDValue;
            }
        }

        public List<TempleteItem> TempleteItemList
        {
            get
            {
                return templeteItemListValue;
            }
        }
        public List<MessageTempleteItem> MessageTempleteItemList
        {
            get
            {
                return mTempleteItemListValue;
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

        public PlayListSettings Settings
        {
            get
            {
                return plSettings;
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
        public bool IsOutExecuteTime()
        {
            bool isOutExectueTime = true;
            try
            {
                DateTime dtEndValue = this.dtStartValue;


                DateTime nowTime = DateTime.Now;
                DayOfWeek week = nowTime.DayOfWeek;

                string weekTimeString = string.Empty;
                if (week == DayOfWeek.Sunday)
                {
                    weekTimeString = plSettings.Sunday;
                }
                else if (week == DayOfWeek.Monday)
                {
                    weekTimeString = plSettings.Monday;
                }
                else if (week == DayOfWeek.Tuesday)
                {
                    weekTimeString = plSettings.Tuesday;
                }
                else if (week == DayOfWeek.Wednesday)
                {
                    weekTimeString = plSettings.Wednesday;
                }
                else if (week == DayOfWeek.Thursday)
                {
                    weekTimeString = plSettings.Thursday;
                }
                else if (week == DayOfWeek.Friday)
                {
                    weekTimeString = plSettings.Friday;
                }
                else if (week == DayOfWeek.Saturday)
                {
                    weekTimeString = plSettings.Saturday;
                }


                if (string.IsNullOrEmpty(weekTimeString) == false)
                {
                    string[] weekTimes = weekTimeString.Split(';');
                    if (Utility.ToInt(weekTimes[0]) <= nowTime.Hour && nowTime.Hour < Utility.ToInt(weekTimes[1]))
                    {
                        isOutExectueTime = false;
                    }
                }

                if (isOutExectueTime == false)
                {
                    if (this.playListStateValue == PlayListStateType.Execute && this.playListStateValue == PlayListStateType.Last)
                    {
                        if (this.fixPlayTimeValidValue)
                        {
                            dtEndValue = this.dtStartValue.AddSeconds(this.FixPlayTime);

                            if (nowTime <= dtEndValue)
                            {
                                isOutExectueTime = false;
                            }
                        }
                    }
                }

                return isOutExectueTime;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "IsOutExecuteTime", ex);
                return true;
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
        }


        /// <summary>
        /// 强制播放时间有效
        /// </summary>
        public bool FixPlayTimeValid
        {
            get
            {
                return fixPlayTimeValidValue;
            }
        }

        /// <summary>
        /// 强制播放时间(秒)
        /// </summary>
        public int FixPlayTime
        {
            get
            {
                return fixPlayTimeValue;
            }
        }

        #endregion


        #region " Init "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loopPlayValid">循环有效</param>
        /// <param name="intervalSecond">持续时间(秒)</param>
        public PlayList(int playListID, bool loopPlayValid, bool fixPlayTimeValid, int fixPlayTime)
        {
            playListIDValue = playListID;
            loopPlayValidValue = loopPlayValid;


            fixPlayTimeValue = fixPlayTime;
            if (fixPlayTimeValue > 0)
            {
                fixPlayTimeValidValue = fixPlayTimeValid;
            }
        }
        public PlayList(int playListID, PlayListSettings plsValue)
        {
            try
            {
                playListIDValue = playListID;
                plSettings = plsValue;

                loopPlayValidValue = plsValue.Loop == "0" ? false : true;

                fixPlayTimeValue = Utility.ToInt(plsValue.PlayHours) * 3600 + Utility.ToInt(plsValue.PlayMinites) * 60 + Utility.ToInt(plsValue.PlaySeconds);
                if (fixPlayTimeValue > 0)
                {
                    fixPlayTimeValidValue = plsValue.Playtime == "0" ? false : true; ;
                }


                if (plSettings.PlaylistItems != null)
                {
                    foreach (PlaylistItem pliTemlete in plSettings.PlaylistItems)
                    {
                        if (Utility.ToInt(pliTemlete.type) == TempleteType.Image.GetHashCode())
                        {
                            #region "Image"
                            List<string> imageFileList = new List<string> { };
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.src)
                                {
                                    string file = WebApiInfo.DownloadFile(url, "");
                                    if (string.IsNullOrEmpty(file) == false)
                                    {
                                        imageFileList.Add(file);
                                    }
                                }
                            }

                            List<ImageShowStyle> imageStyleList = new List<ImageShowStyle> { };
                            if (pliTemlete.SildeshowEffects != null)
                            {
                                foreach (string style in pliTemlete.SildeshowEffects)
                                {
                                    if (string.IsNullOrEmpty(style) == false)
                                    {
                                        int styleValue = Utility.ToInt(style);
                                        if (Enum.IsDefined(typeof(ImageShowStyle), styleValue))
                                        {
                                            imageStyleList.Add((ImageShowStyle)styleValue);
                                        }
                                    }
                                }
                            }
                            if (imageStyleList.Count == 0)
                            {
                                imageStyleList.Add(ImageShowStyle.Random);
                            }

                            ImageTempleteItem itItem = new ImageTempleteItem(imageFileList.ToList(), imageStyleList.ToList(), Utility.ToInt(pliTemlete.DisplayIntevalSeconds));

                            this.PlayAddTemplete(itItem);
                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteType.Message.GetHashCode())
                        {
                            #region "Message"
                            List<string> messageList = new List<string> { };

                            string message = pliTemlete.itemTextData;
                            if (string.IsNullOrEmpty(message) == false)
                            {
                                messageList.Add(message);
                            }


                            List<MessageShowStyle> messageStyleList = new List<MessageShowStyle> { };
                            string style = pliTemlete.TextPostion;
                            if (string.IsNullOrEmpty(style) == false)
                            {
                                int styleValue = Utility.ToInt(style);
                                if (Enum.IsDefined(typeof(MessageShowStyle), styleValue))
                                {
                                    messageStyleList.Add((MessageShowStyle)styleValue);
                                }
                            }
                            if (messageStyleList.Count == 0)
                            {
                                messageStyleList.Add(MessageShowStyle.Random);
                            }

                            MessageTempleteItem itItem = new MessageTempleteItem(messageList.ToList(), messageStyleList.ToList(), Utility.ToInt(pliTemlete.DisplayIntevalSeconds), Utility.ToInt(pliTemlete.SlidingSpeed));

                            this.PlayAddTemplete(itItem);
                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteType.Media.GetHashCode())
                        {
                            #region "Media"
                            List<string> mediaFileList = new List<string> { };
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.src)
                                {
                                    string file = WebApiInfo.DownloadFile(url, "");
                                    if (string.IsNullOrEmpty(file) == false)
                                    {
                                        mediaFileList.Add(file);
                                    }
                                }
                            }

                            int zoomOption = Utility.ToInt(pliTemlete.ZoomOption);
                            ZoomOptionStyle zoStyle = ZoomOptionStyle.None;
                            if (Enum.IsDefined(typeof(ZoomOptionStyle), zoomOption))
                            {
                                zoStyle = (ZoomOptionStyle)zoomOption;
                            }

                            if (mediaFileList.Count > 0)
                            {
                                MediaTempleteItem itItem = new MediaTempleteItem(mediaFileList[0], zoStyle);

                                this.PlayAddTemplete(itItem);
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "PlayList", ex);
            }
        }

        #endregion

        /// <summary>
        /// 播放
        /// </summary>
        public void PlayStart()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);

            this.dtStartValue = this.dtNowValue;

            playListStateValue = PlayListStateType.Execute;

            PlayRefreshTemplete();
        }

        public void PlayLast()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);
            playListStateValue = PlayListStateType.Last;
            if (loopPlayValidValue == false)
            {
                PlayStop();
            }

            this.PlayRefreshTemplete();
        }
        public void PlayStop()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);
            playListStateValue = PlayListStateType.Stop;
        }

        public void PlayRefreshTemplete()
        {
            this.currentTempleteItemIndex = 0;
            foreach (TempleteItem titem in this.TempleteItemList)
            {
                titem.ExecuteRefresh();
            }
        }
        public void PlayMoveNextTemplete()
        {
            while (this.currentTempleteItemIndex < this.TempleteItemList.Count
                && (this.CurrentTempleteItem.TempleteType == TempleteType.Message
                || this.CurrentTempleteItem.TempleteState == TempleteStateType.Stop))
            {
                this.currentTempleteItemIndex++;
            }
        }
        public void PlayAddTemplete(TempleteItem tItem)
        {
            this.templeteItemListValue.Add(tItem);
            if (tItem.TempleteType == TempleteType.Message)
            {
                this.mTempleteItemListValue.Add(tItem as MessageTempleteItem);
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
                    if (playListStateValue == PlayListStateType.Wait)
                    {
                        if (IsInExecuteTime() == false)
                        {
                            playListStateValue = PlayListStateType.Stop;
                        }
                    }
                    else if (playListStateValue == PlayListStateType.Execute || playListStateValue == PlayListStateType.Last)
                    {
                        if (this.IsOutExecuteTime())
                        {
                            playListStateValue = PlayListStateType.Stop;
                        }
                    }
                    else if (playListStateValue == PlayListStateType.Stop)
                    {
                        if (IsInExecuteTime())
                        {
                            playListStateValue = PlayListStateType.Wait;
                        }
                    }
#if DEBUG
                    if (this.templeteItemListValue.Count == 0)
                    {
                        playListStateValue = PlayListStateType.Stop;
                    }
#endif

                    return playListStateValue;
                }
                catch (Exception ex)
                {
                    return PlayListStateType.Stop;
                }
            }
        }

        public bool IsInExecuteTime()
        {
            try
            {
                if (this.plSettings == null)
                {
                    return false;
                }
                DateTime nowTime = DateTime.Now;
                DayOfWeek week = nowTime.DayOfWeek;

                string weekTimeString = string.Empty;
                if (week == DayOfWeek.Sunday)
                {
                    weekTimeString = plSettings.Sunday;
                }
                else if (week == DayOfWeek.Monday)
                {
                    weekTimeString = plSettings.Monday;
                }
                else if (week == DayOfWeek.Tuesday)
                {
                    weekTimeString = plSettings.Tuesday;
                }
                else if (week == DayOfWeek.Wednesday)
                {
                    weekTimeString = plSettings.Wednesday;
                }
                else if (week == DayOfWeek.Thursday)
                {
                    weekTimeString = plSettings.Thursday;
                }
                else if (week == DayOfWeek.Friday)
                {
                    weekTimeString = plSettings.Friday;
                }
                else if (week == DayOfWeek.Saturday)
                {
                    weekTimeString = plSettings.Saturday;
                }


                if (string.IsNullOrEmpty(weekTimeString) == false)
                {
                    string[] weekTimes = weekTimeString.Split(';');
                    if (Utility.ToInt(weekTimes[0]) <= nowTime.Hour && nowTime.Hour <= Utility.ToInt(weekTimes[1]))
                    {
                        return true;
                    }
                }
                else
                {
#if DEBUG
                    return true;
#endif
                }

                return false;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "InExecuteTime", ex);
                return false;
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

        protected ZoomOptionStyle zoomOptionValue = ZoomOptionStyle.None;

        protected List<MessageShowStyle> messageStyleListValue = new List<MessageShowStyle>() { };


        protected TempleteType templeteTypeValue = TempleteType.Image;


        protected TempleteStateType templeteStateValue = TempleteStateType.Wait;

        /// <summary>
        /// 间隔时间(秒)
        /// </summary>
        protected int intervalSecondValue = 5;

        /// <summary>
        /// 上一个时间
        /// </summary>
        protected DateTime previousTimeValue = DateTime.Now;

        private int slidingSpeedValue = 10;

        #endregion


        #region " propert "

        /// <summary>
        /// 类型
        /// </summary>
        public TempleteType TempleteType
        {
            get
            {
                return templeteTypeValue;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
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
                        if (this.currentIndex >= this.fileOrMessageListValue.Count - 1)
                        {
                            if (nowTime <= previousTimeValue.AddSeconds(intervalSecondValue))
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
                return templeteStateValue;
            }
        }


        #endregion


        public TempleteItem(string file, ZoomOptionStyle zoomOption)
        {
            templeteTypeValue = TempleteType.Media;
            fileOrMessageListValue.Add(file);
            this.zoomOptionValue = zoomOption;
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
            if (intervalSecond > 0)
            {
                this.intervalSecondValue = intervalSecond;
            }
            else
            {
                this.intervalSecondValue = 5;
            }
        }

        public TempleteItem(List<string> messageList, List<MessageShowStyle> messageStyle, int intervalSecond, int slidingSpeed)
        {
            templeteTypeValue = TempleteType.Message;
            foreach (string message in messageList)
            {
                fileOrMessageListValue.Add(message);
            }
            foreach (MessageShowStyle style in messageStyle)
            {
                messageStyleListValue.Add(style);
            }
            this.intervalSecondValue = intervalSecond;

            this.slidingSpeedValue = slidingSpeed;

        }


        #region " void and function "
        public void ExecuteRefresh()
        {
            previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);

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
                return this.fileOrMessageListValue[this.currentIndex];
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
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
            }
        }
        public void ExecuteStop()
        {
            previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
            this.currentIndex = -1;
            this.currentShowStyleIndex = -1;
        }

        public bool CurrentIsChanged()
        {
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return false;
                }

                if (this.fileOrMessageListValue.Count > 0)
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    if (nowTime >= previousTimeValue.AddSeconds(intervalSecondValue))
                    {
                        int nowIndex = this.currentIndex;
                        if (nowIndex < 0)
                        {
                            nowIndex = 0;
                            this.currentIndex = nowIndex;
                            return true;
                        }
                        else
                        {
                            nowIndex++;
                        }
                        
                        if (nowIndex >= this.fileOrMessageListValue.Count)
                        {
                            this.templeteStateValue = TempleteStateType.Stop;
                            return false;
                        }

                        if (nowTime < previousTimeValue.AddSeconds(this.intervalSecondValue))
                        {
                            return false;
                        }

                        //while (nowIndex < this.fileOrMessageListValue.Count)
                        //{
                        //    if (nowTime <= previousTimeValue.AddSeconds(this.intervalSecondValue * (nowIndex + 1)))
                        //    {
                        //        break;
                        //    }
                        //    nowIndex++;
                        //}

                        //if (nowIndex >= this.fileOrMessageListValue.Count)
                        //{
                        //    this.templeteStateValue = TempleteStateType.Stop;
                        //    return false;
                        //}
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
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return;
                }

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

                    previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
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

    public class MessageTempleteItem : TempleteItem
    {

        #region " variable "

        private bool loadControlsFlag = false;



        #endregion


        #region " propert "

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


        public string CurrentMessage
        {
            get
            {
                return this.fileOrMessageListValue[this.currentIndex];
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
        public MessageTempleteItem(List<string> messageList, List<MessageShowStyle> messageStyleList, int intervalSecond, int slidingSpeed) : base(messageList, messageStyleList, intervalSecond, slidingSpeed)
        {
        }
        #region " void and function "


        /// <summary>
        /// 播放
        /// </summary>
        public void ExecuteStart()
        {
            if (this.templeteStateValue != TempleteStateType.Execute && this.templeteStateValue != TempleteStateType.Stop)
            {
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Execute;
            }
        }

        public void ExecuteRefresh()
        {
            this.templeteStateValue = TempleteStateType.Wait;
            loadControlsFlag = false;
        }

        public TempleteStateType TempleteState
        {
            get
            {
                if (this.loadControlsFlag)
                {
                    this.templeteStateValue = TempleteStateType.Stop;
                }
                return templeteStateValue;
            }
        }
        public bool CurrentIsChanged()
        {
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return false;
                }

                if (this.fileOrMessageListValue.Count > 0)
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    if (this.intervalSecondValue > 0)
                    {
                        if (nowTime >= previousTimeValue.AddSeconds(intervalSecondValue))
                        {
                            int nowIndex = this.currentIndex;
                            if (nowIndex < 0)
                            {
                                nowIndex = 0;
                                this.currentIndex = nowIndex;
                                return true;
                            }
                            else
                            {
                                nowIndex++;
                            }
                                                      

                            if (nowIndex >= this.fileOrMessageListValue.Count)
                            {
                                this.templeteStateValue = TempleteStateType.Stop;
                                return false;
                            }

                            if (nowTime < previousTimeValue.AddSeconds(this.intervalSecondValue))
                            {
                                return false;
                            }


                            if (nowIndex != this.currentIndex)
                            {
                                this.currentIndex = nowIndex;
                                return true;
                            }

                        }

                    }
                    else
                    {
                        int nowIndex = this.currentIndex;
                        if (nowIndex < 0)
                        {
                            nowIndex = 0;
                        }
                        else
                        {
                            nowIndex++;
                        }

                        if (nowIndex >= this.fileOrMessageListValue.Count)
                        {
                            this.templeteStateValue = TempleteStateType.Stop;
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

        public void ShowCurrent(Control objControl)
        {
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return;
                }

                if (this.loadControlsFlag == false && this.currentIndex >= 0 && this.currentIndex < this.fileOrMessageListValue.Count)
                {
                    currentShowStyleIndex++;
                    if (currentShowStyleIndex >= this.messageStyleListValue.Count)
                    {
                        currentShowStyleIndex = 0;
                    }

                    DrawMessage dmItem = new DrawMessage(this.fileOrMessageListValue[this.currentIndex]
                        , new Font("MS UI Gothic", 12, System.Drawing.FontStyle.Bold)
                        , System.Drawing.Color.Red
                        , objControl.Width, objControl.Height
                        , this.messageStyleListValue[currentShowStyleIndex]
                        );
                    PlayApp.DrawMessageList.Add(dmItem);


                    if (this.intervalSecondValue > 0)
                    {
                        previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    }
                }

                if (this.currentIndex >= this.fileOrMessageListValue.Count - 1)
                {
                    this.loadControlsFlag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        #endregion
    }

    public class MediaTempleteItem : TempleteItem
    {

        #region " variable "


        #endregion


        #region " propert "

        public string CurrentFile
        {
            get
            {
                return fileOrMessageListValue[0];
            }
        }


        public ZoomOptionStyle ZoomOption
        {
            get
            {
                return zoomOptionValue;
            }
        }

        #endregion

        public MediaTempleteItem(string file, ZoomOptionStyle zoomOption) : base(file, zoomOption)
        {
        }
        #region " void and function "


        /// <summary>
        /// 播放
        /// </summary>
        public void ExecuteStart()
        {
            if (this.templeteStateValue != TempleteStateType.Execute)
            {
                this.templeteStateValue = TempleteStateType.Wait;

                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
            }
        }
        public void ExecuteStop()
        {
            previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
            this.templeteStateValue = TempleteStateType.Stop;
        }

        public bool CurrentIsChanged()
        {
            try
            {
                if (this.fileOrMessageListValue.Count > 0)
                {
                    if (this.templeteStateValue == TempleteStateType.Wait)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public void ShowCurrent(AxWindowsMediaPlayer axWMP)
        {
            ShowCurrent(axWMP, WMPLib.WMPPlayState.wmppsPlaying, 0);
        }

        public void ShowCurrent(AxWindowsMediaPlayer axWMP, WMPPlayState state, double position)
        {
            try
            {
                if (this.fileOrMessageListValue.Count > 0)
                {
                    axWMP.URL = this.CurrentFile;
                    axWMP.Ctlcontrols.currentPosition = position;

                    if (state == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        axWMP.Ctlcontrols.play();
                    }
                    else if (state == WMPLib.WMPPlayState.wmppsStopped)
                    {
                        axWMP.Ctlcontrols.stop();
                    }
                    else if (state == WMPLib.WMPPlayState.wmppsPaused)
                    {
                        axWMP.Ctlcontrols.pause();
                    }

                    if (zoomOptionValue == ZoomOptionStyle.None)
                    {
                        axWMP.stretchToFit = false;
                    }
                    else
                    {
                        axWMP.stretchToFit = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.templeteStateValue = TempleteStateType.Execute;
            }
        }

        #endregion
    }

    public class DrawMessage
    {

        #region " variable "
        private string messageValue = string.Empty;
        private Font fontValue;
        private Color colorValue;

        private int leftValue;
        private int leftMaxValue;
        private int topValue;
        private MessageShowStyle showStyleValue;
        private int parentHeigthValue;
        private bool needRefreshTop = false;
        #endregion


        #region " propert "

        public string Message
        {
            get
            {
                return messageValue;
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
        public int Left
        {
            get
            {
                return leftValue;
            }
        }
        public int Top
        {
            get
            {
                return topValue;
            }
        }
        public MessageShowStyle ShowStyle
        {
            get
            {
                return showStyleValue;
            }
        }

        #endregion

        public DrawMessage(string message, Font font, Color color, int left, int parentHeigth, MessageShowStyle showStyle)
        {
            this.messageValue = message;
            this.fontValue = font;
            this.colorValue = color;

            this.leftValue = left;
            this.leftMaxValue = left;
            this.showStyleValue = showStyle;
            this.parentHeigthValue = parentHeigth;
            this.needRefreshTop = true;
            RefreshTop();
        }

        #region " void and function "
        public void SetParentHeigth(int parentHeigth)
        {
            this.parentHeigthValue = parentHeigth;
            this.needRefreshTop = true;
        }
        public void MoveMessage()
        {
            this.leftValue = this.leftValue - 2;
            if (this.leftValue <= 0)
            {
                this.leftValue = this.leftMaxValue;
            }
            if (needRefreshTop)
            {
                RefreshTop();
            }
        }
        private void RefreshTop()
        {
            int top = 30;
            if (this.showStyleValue == MessageShowStyle.Top)
            {
                top = 30;
            }
            else if (this.showStyleValue == MessageShowStyle.Bottom)
            {
                top = this.parentHeigthValue - 70;
            }
            else if (this.showStyleValue == MessageShowStyle.Middle)
            {
                top = this.parentHeigthValue / 2 - 30;
            }
            this.topValue = top;
            this.needRefreshTop = false;
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
        /// 末尾
        /// </summary>
        [EnumDescription("末尾")]
        Last = 2,
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 9,
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
        Stop = 9,
    }

    #endregion
}
