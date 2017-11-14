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
        public static String CurrentPlayListJsonString = string.Empty;

        private static PlayList executePlayList;
        public static List<PlayList> PlayListArray = new List<PlayList>();

        public static List<PlayListMaster> PlayListMasterArray = new List<PlayListMaster>();

        private static int CurrentPlayListIndex = -1;
        private static int CurrentPlayListID = -1;
        private static int ExecutePlayListID = -1;

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
                    if (ExecutePlayListValid(plItem.Settings))
                    {
                        CurrentPlayListIndex = i;
                        CurrentPlayListID = plItem.PlayListID;
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool ExecutePlayListValid(PlayListSettings plSettings)
        {
            try
            {
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
                return false;
            }
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
        /// 持续时间(秒)
        /// </summary>
        private int intervalSecondValue = 0;

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
        public DateTime EndDateTime
        {
            get
            {
                DateTime dtEndValue = this.dtStartValue.AddSeconds(this.intervalSecondValue);

                DayOfWeek week = DateTime.Now.DayOfWeek;

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
                    if (dtEndValue.Hour < Utility.ToInt(weekTimes[1]))
                    {
                        if (this.loopPlayValidValue)
                        {
                            dtEndValue = Utility.GetPlayDateTime(DateTime.Parse(string.Format("{0}{1}:59:59", dtEndValue.ToString("yyyy-MM-dd "), Utility.ToInt(weekTimes[1]) - 1)));
                        }
                    }
                }


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
        public PlayList(int playListID, bool loopPlayValid, DateTime dtStart, int intervalSecond)
        {
            playListIDValue = playListID;
            loopPlayValidValue = loopPlayValid;
            intervalSecondValue = intervalSecond;
        }
        public PlayList(int playListID, PlayListSettings plsValue)
        {
            playListIDValue = playListID;
            plSettings = plsValue;

            loopPlayValidValue = plsValue.Loop == "0" ? true : false;
            intervalSecondValue = Utility.ToInt(plsValue.PlayHours) * 3600 + Utility.ToInt(plsValue.PlayMinites) * 60 + Utility.ToInt(plsValue.PlaySeconds);

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
        }
        public void PlayStop()
        {
            this.dtNowValue = Utility.GetPlayDateTime(DateTime.Now);
            if (loopPlayValidValue == false)
            {
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
        public void PlayMoveNextTemplete()
        {
            while (this.currentTempleteItemIndex < this.TempleteItemList.Count
                && this.CurrentTempleteItem.TempleteType == TempleteType.Message
                && this.CurrentTempleteItem.TempleteState == TempleteStateType.Stop)
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
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    DateTime dtEndValue = this.EndDateTime;

                    if (playListStateValue == PlayListStateType.Wait)
                    {
                        if (nowTime <= dtEndValue)
                        {
                            playListStateValue = PlayListStateType.Execute;
                        }
                    }

                    if (playListStateValue != PlayListStateType.Stop)
                    {
                        if (dtEndValue <= nowTime)
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

        protected List<MessageShowStyle> messageStyleListValue = new List<MessageShowStyle>() { };


        protected TempleteType templeteTypeValue = TempleteType.Image;


        protected TempleteStateType templeteStateValue = TempleteStateType.Wait;

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

        public TempleteItem(List<string> messageList, List<MessageShowStyle> messageStyle, int intervalSecond)
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
                if (this.fileOrMessageListValue.Count > 0)
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

        public MessageTempleteItem(List<string> messageList, List<MessageShowStyle> messageStyleList, int intervalSecond) : base(messageList, messageStyleList, intervalSecond)
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
                dtStartValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Execute;
            }
        }

        public void ExecuteStop()
        {
            this.templeteStateValue = TempleteStateType.Stop;
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
                if (this.fileOrMessageListValue.Count > 0)
                {
                    DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);

                    if (nowTime >= this.dtStartValue && this.currentIndex < this.fileOrMessageListValue.Count)
                    {
                        int nowIndex = this.currentIndex;
                        if (this.intervalSecondValue > 0)
                        {
                            if (nowIndex < 0) nowIndex = 0;
                            while (nowIndex < this.fileOrMessageListValue.Count)
                            {
                                if (nowTime <= this.dtStartValue.AddSeconds(this.intervalSecondValue * (nowIndex + 1)))
                                {
                                    break;
                                }
                                nowIndex++;
                            }
                        }
                        else
                        {
                            if (nowIndex < 0)
                            {
                                nowIndex = 0;
                            }
                            else
                            {
                                nowIndex++;
                            }
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

        public void ShowCurrent(Panel showPanel)
        {
            try
            {
                if (this.loadControlsFlag == false && this.currentIndex >= 0 && this.currentIndex < this.fileOrMessageListValue.Count)
                {
                    currentShowStyleIndex++;
                    if (currentShowStyleIndex >= this.messageStyleListValue.Count)
                    {
                        currentShowStyleIndex = 0;
                    }
                    Label lblMessage = new Label();
                    lblMessage.Name = string.Format("lblMessage{0}", this.currentIndex);

                    int lblTop = 20;
                    if (this.messageStyleListValue[currentShowStyleIndex] == MessageShowStyle.Top)
                    {
                        lblTop = 20;
                    }
                    else if (this.messageStyleListValue[currentShowStyleIndex] == MessageShowStyle.Down)
                    {
                        lblTop = showPanel.Height - lblMessage.Height - 40;
                    }
                    else if (this.messageStyleListValue[currentShowStyleIndex] == MessageShowStyle.Center)
                    {
                        lblTop = showPanel.Height / 2 - 20;
                    }

                    int lblLeft = showPanel.Width;
                    lblMessage.BackColor= System.Drawing.Color.Transparent;
                    lblMessage.Location = new System.Drawing.Point(lblLeft, lblTop);
                    lblMessage.Text = this.fileOrMessageListValue[this.currentIndex];
                    lblMessage.Size = new System.Drawing.Size(100, 23);
                    showPanel.Controls.Add(lblMessage);

                }

                if (this.currentIndex >= this.fileOrMessageListValue.Count - 1)
                {
                    this.loadControlsFlag = true;
                    this.templeteStateValue = TempleteStateType.Stop;
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


        public MediaShowStyle CurrentShowStyle
        {
            get
            {
                return mediaStyleValue;
            }
        }

        #endregion

        public MediaTempleteItem(string file, MediaShowStyle mediaStyle) : base(file, mediaStyle)
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
                if (this.fileOrMessageListValue.Count > 0)
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
