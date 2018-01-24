using AxAXVLC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxWMPLib;
using WMPLib;

namespace ToilluminateClient
{

    /// <summary>
    /// 播放列表
    /// </summary>
    public static class PlayApp
    {
        #region "play"
        public static String CurrentPlayListJsonString = string.Empty;
        public static String CurrentPlayerJsonString = string.Empty;
        public static bool CurrentPlayListNeedDownloadFile = false;

        public static PlayerInfo CurrentPlayerInfo = new PlayerInfo();


        private static PlayList executePlayList;
        public static List<PlayList> PlayListArray = new List<PlayList>();

        public static List<PlayListMaster> PlayListMasterArray = new List<PlayListMaster>();

        private static int currentPlayListIndex = -1;
        private static int currentPlayListID = -1;
        private static int executePlayListID = -1;

        private static bool nowLoadPlayList = false;
        private static bool newPlayListExist = false;


        public static int ThreadLoadPlayListTime = 5;
        public static int ThreadLoadPlayListTimeCurrent = 0;
        #endregion


        public static void Clear()
        {
            PlayApp.PlayListArray.Clear();
            PlayApp.PlayListMasterArray.Clear();
            currentPlayListIndex = -1;
            currentPlayListID = -1;
            executePlayListID = -1;
            executePlayList = null;
        }

        public static int CurrentPlayListIndex
        {
            get
            {
                return currentPlayListIndex;
            }
        }
        public static int CurrentPlayListID
        {
            get
            {
                return currentPlayListID;
            }
        }
        public static int ExecutePlayListID
        {
            get
            {
                return executePlayListID;
            }
        }

        public static bool NewPlayListExist
        {
            get
            {
                return newPlayListExist;
            }
        }

        public static bool NowLoadPlayList
        {
            get
            {
                return PlayApp.nowLoadPlayList;
            }
        }


        public static PlayList ExecutePlayList
        {
            get
            {
                return executePlayList;
            }
        }



        #region " load play list "
        /// <summary>
        /// 
        /// </summary>
        public static void DebugLoadPlayListInfo()
        {

            #region " DEBUG DATA"

            PlayApp.Clear();
            PlayListSettings plsStudent = new PlayListSettings();
            string settings = "{\"Loop\":\"1\",\"Playtime\":\"0\",\"PlayHours\":\"20\",\"PlayMinites\":\"0\",\"PlaySeconds\":\"0\",\"Monday\":\"0; 24\",\"MondayisCheck\":true,\"Tuesday\":\"0; 24\",\"TuesdayisCheck\":true,\"Wednesday\":\"0; 24\",\"WednesdayisCheck\":true,\"Thursday\":\"0; 24\",\"ThursdayisCheck\":true,\"Friday\":\"0; 20\",\"FridayisCheck\":true,\"Saturday\":\"0; 24\",\"SaturdayisCheck\":true,\"Sunday\":\"0; 24\",\"SundayisCheck\":true,\"PlaylistItems\":[]}";
            plsStudent = JsonConvert.DeserializeAnonymousType(settings, plsStudent);

            PlayList pList1 = new PlayList(0, plsStudent);

            PlayApp.PlayListArray.Add(pList1);

            #region "images"
            try
            {
                List<string> imageFileList = new List<string> { };
                string imageDir = Utility.GetFullFileName(VariableInfo.TempPath, "Images");
                if (Directory.Exists(imageDir))
                {
                    string[] files = Directory.GetFiles(imageDir, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".bmp")).ToArray();

                    foreach (string file in files)
                    {
                        imageFileList.Add(file);
                    }
                }
                ImageShowStyle[] imageStyleList = new ImageShowStyle[] { ImageShowStyle.Docking_LR };
                if (imageFileList.Count > 0)
                {
                    ImageTempleteItem itItem = new ImageTempleteItem(imageFileList, imageStyleList.ToList(), 3, FillOptionStyle.Zoom);
                    pList1.PlayAddTemplete(itItem);
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DebugLoadPlayListInfo:Images", ex);
            }
            #endregion


            #region "medias"
            try
            {
                List<string> mediaFileList = new List<string> { };
                string mediaDir = Utility.GetFullFileName(VariableInfo.TempPath, "Medias");
                if (Directory.Exists(mediaDir))
                {
                    string[] files = Directory.GetFiles(mediaDir, "*.*", SearchOption.AllDirectories)
                                    .Where(s => s.EndsWith(".wmv") || s.EndsWith(".asf") || s.EndsWith(".avi")
                                    || s.EndsWith(".rmvb") || s.EndsWith(".mp4") || s.EndsWith(".vod") || s.EndsWith(".mpg")).ToArray();


                    foreach (string file in files)
                    {
                        MediaTempleteItem mtItem = new MediaTempleteItem(file, ZoomOptionStyle.Full);
                        pList1.PlayAddTemplete(mtItem);
                    }

                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DebugLoadPlayListInfo:Medias", ex);
            }
            #endregion


            #region "message"
            try
            {
                List<string> imageFileList = new List<string> { };
                string messageFile = Utility.GetFullFileName(VariableInfo.TempPath, "Message.txt");
                if (File.Exists(messageFile))
                {

                    StreamReader sr = new StreamReader(messageFile, Encoding.Default);
                    string messageString = string.Empty;
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        messageString = string.Format("{0}{1}", messageString, line.ToString());
                    }

                    if (string.IsNullOrEmpty(messageString) == false)
                    {
                        MessageTempleteItem mtItem = new MessageTempleteItem(messageString, MessagePositionType.Top, 600, 1);
                        pList1.PlayAddTemplete(mtItem);
                    }
                }
                else
                {
                    string messageString = "<span style=\"font-family: MS PGothic; font-size: 18px; \" ><b color=\"red\"> テストデータを放送している。</b></span>";
                    MessageTempleteItem mtItem = new MessageTempleteItem(messageString, MessagePositionType.Top, 0, 5);
                    pList1.PlayAddTemplete(mtItem);
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DebugLoadPlayListInfo:message", ex);
            }
            #endregion

            #region " trademarks "
            try
            {
                List<string> trademarkFileList = new List<string> { };
                List<TrademarkStyle> trademarkStyleList = new List<TrademarkStyle> { };
                string trademarkDir = Utility.GetFullFileName(VariableInfo.TempPath, "Trademarks");
                if (Directory.Exists(trademarkDir))
                {
                    string[] files = Directory.GetFiles(trademarkDir, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".bmp")).ToArray();

                    int trademarkSizeTypeIndex = 1;
                    int trademarkPositionTypeIndex = 8;
                    foreach (string file in files)
                    {
                        trademarkFileList.Add(file);

                        trademarkStyleList.Add(new TrademarkStyle((TrademarkSizeType)trademarkSizeTypeIndex, (TrademarkPositionType)trademarkPositionTypeIndex));
                        trademarkPositionTypeIndex++;
                        if (trademarkPositionTypeIndex > 8)
                        {
                            trademarkPositionTypeIndex = 0;
                        }
                    }
                }
                if (trademarkFileList.Count > 0)
                {
                    TrademarkTempleteItem ttItem = new TrademarkTempleteItem(trademarkFileList.ToList(), trademarkStyleList.ToList(), 0);
                    pList1.PlayAddTemplete(ttItem);
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "DebugLoadPlayListInfo:Images", ex);
            }
            #endregion
            //Trademarks

            #endregion

        }
        /// <summary>
        /// 共通変数が初期化
        /// </summary>
        public static bool LoadPlayListInfo()
        {
            if (PlayApp.nowLoadPlayList)
            {
                return false;
            }
            try
            {
                PlayApp.nowLoadPlayList = true;

                // 設定読み込み
                IniFileInfo.GetIniInfo(VariableInfo.IniFile);

                if (string.IsNullOrEmpty(IniFileInfo.PlayerID) == false)
                {

                    LogApp.OutputProcessLog("VariableInfo", "LoadPlayListInfo", "Begin");

                    try
                    {
                        string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYERMASTERS_SEND, IniFileInfo.PlayerID));
                        string getJsonString = WebApiInfo.HttpGet(urlString);
                    }
                    catch (Exception ex)
                    {
                        LogApp.OutputErrorLog("VariableInfo", "LoadPlayListInfo:1", ex);
                    }

                    try
                    {
                        string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYERMASTERS_GET_INFO, IniFileInfo.PlayerID));
                        string getJsonString = WebApiInfo.HttpGet(urlString);

                        if (PlayApp.CurrentPlayerJsonString != getJsonString)
                        {
                            PlayApp.CurrentPlayerJsonString = getJsonString;


                            CurrentPlayerInfo = new PlayerInfo();

                            CurrentPlayerInfo = JsonConvert.DeserializeAnonymousType(PlayApp.CurrentPlayerJsonString, CurrentPlayerInfo);

                            CurrentPlayerInfo.playerSettings = JsonConvert.DeserializeAnonymousType(CurrentPlayerInfo.Settings, CurrentPlayerInfo.playerSettings);
                        }

                        if (CurrentPlayerInfo.IsInExecuteTime() == false)
                        {
                            PlayApp.CurrentPlayListJsonString = string.Empty;
                            PlayApp.Clear();
                            return true;
                        }

                    }
                    catch (Exception ex)
                    {
                        LogApp.OutputErrorLog("VariableInfo", "LoadPlayListInfo:2", ex);
                    }


                    try
                    {
                        string oldJsonString = Utility.ReaderFile(VariableInfo.JsonFile);

                        string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYLISTMASTERS_GET_INFO, IniFileInfo.PlayerID));
                        string getJsonString = WebApiInfo.HttpPost(urlString, "");

                        if (string.IsNullOrEmpty(getJsonString))
                        {
                            getJsonString = oldJsonString;
                        }

                        if (PlayApp.CurrentPlayListJsonString != getJsonString || PlayApp.CurrentPlayListNeedDownloadFile)
                        {
                            Utility.WriterNewFile(VariableInfo.JsonFile, getJsonString);
                            PlayApp.CurrentPlayListNeedDownloadFile = false;
                            ShowApp.DownloadMessageRefresh();

                            List<PlayList> temp_PlayListArray = new List<PlayList>();
                            List<PlayListMaster> temp_PlayListMasterArray = new List<PlayListMaster> { };

                            JArray plmArray = (JArray)JsonConvert.DeserializeObject(getJsonString);
                            foreach (JObject obj in plmArray)
                            {
                                PlayListMaster plmStudent = new PlayListMaster();

                                plmStudent = JsonConvert.DeserializeAnonymousType(obj.ToString(), plmStudent);

                                if (string.IsNullOrEmpty(plmStudent.Settings) == false)
                                {
                                    temp_PlayListMasterArray.Add(plmStudent);

                                    plmStudent.plsStudent = new ToilluminateClient.PlayListSettings();
                                    plmStudent.plsStudent = JsonConvert.DeserializeAnonymousType(plmStudent.Settings, plmStudent.plsStudent);
                                }
                            }

                            ShowApp.DownloadMessageCountTotalNumber(temp_PlayListMasterArray);

                            foreach (PlayListMaster plmItem in temp_PlayListMasterArray)
                            {
                                PlayListSettings plsStudent = plmItem.plsStudent;

                                PlayList plItem = new PlayList(plmItem.PlayListID, plsStudent);

                                temp_PlayListArray.Add(plItem);
                            }

                            ShowApp.DownloadMessageDispose();
                            
                            PlayApp.CurrentPlayListJsonString = getJsonString;
                            PlayApp.Clear();

                            foreach (PlayListMaster plmItem in temp_PlayListMasterArray)
                            {
                                PlayApp.PlayListMasterArray.Add(plmItem);
                            }
                            foreach (PlayList plItem in temp_PlayListArray)
                            {
                                PlayApp.PlayListArray.Add(plItem);
                            }


                            PlayApp.newPlayListExist = true;
                            return true;
                        }

                    }
                    catch (Exception ex)
                    {
                        LogApp.OutputErrorLog("VariableInfo", "LoadPlayListInfo:3", ex);
                    }
                }

                return false;
            }
            finally
            {
                PlayApp.nowLoadPlayList = false;
                LogApp.OutputProcessLog("VariableInfo", "LoadPlayListInfo", "End");
            }
        }



        /// <summary>
        /// 共通変数が初期化
        /// </summary>
        public static void RefreshPlayListInfo()
        {
            try
            {

                foreach (PlayList plItem in PlayApp.PlayListArray)
                {
                    plItem.PlayRefresh();
                }
            }
            finally
            {
                LogApp.OutputProcessLog("PlayApp", "RefreshPlayListInfo", "End");
            }
        }



        public static string GetUrlFile(string url)
        {
            return string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, url);
        }
        #endregion

        public static bool ExecutePlayListStart()
        {
            if (ExecutePlayListID != CurrentPlayListID)
            {
                ShowApp.DrawMessageList.Clear();

                executePlayList = CurrentPlayList();
                executePlayList.PlayStart();

                executePlayListID = CurrentPlayListID;
                PlayApp.newPlayListExist = false;
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
                if (CurrentPlayListIndex < 0)
                {
                    currentPlayListIndex = 0;
                }

                bool currentPlayListExist = false;

                for (int i = CurrentPlayListIndex; i < PlayListArray.Count; i++)
                {
                    PlayList plItem = PlayListArray[i];
                    if (plItem.CheckPlayListState == PlayListStateType.Execute)
                    {
                        currentPlayListIndex = i;
                        currentPlayListID = plItem.PlayListID;
                        currentPlayListExist = true;
                        return true;
                    }
                }

                if (currentPlayListExist == false)
                {
                    executePlayListID = -1;
                    currentPlayListIndex = 0;

                    for (int i = 0; i < PlayListArray.Count; i++)
                    {
                        PlayList plItem = PlayListArray[i];
                        plItem.PlayRefresh();
                    }

                    for (int i = CurrentPlayListIndex; i < PlayListArray.Count; i++)
                    {
                        PlayList plItem = PlayListArray[i];
                        if (plItem.CheckPlayListState == PlayListStateType.Execute)
                        {
                            currentPlayListIndex = i;
                            currentPlayListID = plItem.PlayListID;
                            currentPlayListExist = true;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }


    public class PlayList
    {
        #region " variable "

        private List<MessageTempleteItem> messageListValue = new List<MessageTempleteItem>();
        private List<TrademarkTempleteItem> trademarkListValue = new List<TrademarkTempleteItem>();


        private List<TempleteItem> templeteItemListValue = new List<TempleteItem>();

        private int currentTempleteItemIndex = -1;

        private int executeTempleteItemIndex = -1;

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
                return messageListValue;
            }
        }
        public List<TrademarkTempleteItem> TrademarkTempleteItemList
        {
            get
            {
                return trademarkListValue;
            }
        }
        public int CurrentTempleteIndex
        {
            get
            {
                return currentTempleteItemIndex;
            }
        }
        public int ExecuteTempleteIndex
        {
            get
            {
                return executeTempleteItemIndex;
            }
        }

        public PlayListSettings Settings
        {
            get
            {
                return plSettings;
            }
        }

        public TempleteItem ExecuteTempleteItem
        {
            get
            {
                if (executeTempleteItemIndex >= 0)
                {
                    return templeteItemListValue[executeTempleteItemIndex];
                }
                else
                {
                    return null;
                }
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
        public bool IsOutFixPlayTime()
        {
            bool isOutFixPlayTime = false;
            try
            {

                if (this.playListStateValue == PlayListStateType.Execute || this.playListStateValue == PlayListStateType.Last)
                {
                    if (this.fixPlayTimeValidValue)
                    {
                        DateTime dtEndValue = this.dtStartValue.AddSeconds(this.FixPlayTime);
                        DateTime nowTime = DateTime.Now;
                        if (nowTime > dtEndValue)
                        {
                            isOutFixPlayTime = true;
                        }
                    }
                }

                return isOutFixPlayTime;
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
                        if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Image.GetHashCode())
                        {
                            #region "Image"
                            List<string> imageFileList = new List<string> { };
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    string file = WebApiInfo.DownloadFile(PlayApp.GetUrlFile(url), "");
                                    if (string.IsNullOrEmpty(file) == false)
                                    {
                                        imageFileList.Add(file);
                                    }
                                    ShowApp.DownLoadIndexNumber++;
                                    ShowApp.DownloadMessageRefresh();
                                }
                            }

                            List<ImageShowStyle> imageStyleList = new List<ImageShowStyle> { };
                            if (pliTemlete.SildeshowEffects != null)
                            {
                                foreach (string seStyle in pliTemlete.SildeshowEffects)
                                {
                                    if (string.IsNullOrEmpty(seStyle) == false)
                                    {
                                        int styleValue = Utility.ToInt(seStyle);
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

                            FillOptionStyle fillOption = FillOptionStyle.Fill;
                            string ppStyle = pliTemlete.PicturePostion;
                            if (string.IsNullOrEmpty(ppStyle) == false)
                            {
                                int styleValue = Utility.ToInt(ppStyle);
                                if (Enum.IsDefined(typeof(FillOptionStyle), styleValue))
                                {
                                    fillOption = (FillOptionStyle)styleValue;
                                }
                            }

                            ImageTempleteItem itItem = new ImageTempleteItem(imageFileList.ToList(), imageStyleList.ToList(), Utility.ToInt(pliTemlete.DisplayIntevalSeconds), fillOption);

                            this.PlayAddTemplete(itItem);
                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Message.GetHashCode())
                        {
                            #region "Message"
                            List<string> messageList = new List<string> { };

                            string message = pliTemlete.itemTextData;

                            MessagePositionType messageShowStyleValue = MessagePositionType.Bottom;
                            string tpStyle = pliTemlete.TextPostion;
                            if (string.IsNullOrEmpty(tpStyle) == false)
                            {
                                int styleValue = Utility.ToInt(tpStyle);
                                if (Enum.IsDefined(typeof(MessagePositionType), styleValue))
                                {
                                    messageShowStyleValue = (MessagePositionType)styleValue;
                                }
                            }

                            MessageTempleteItem itItem = new MessageTempleteItem(message, messageShowStyleValue, Utility.ToInt(pliTemlete.DisplayIntevalSeconds), Utility.ToInt(pliTemlete.SlidingSpeed));

                            this.PlayAddTemplete(itItem);
                            #endregion
                        }
                        else if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Media.GetHashCode())
                        {
                            #region "Media"
                            List<string> mediaFileList = new List<string> { };
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    string file = WebApiInfo.DownloadFile(PlayApp.GetUrlFile(url), "");
                                    if (string.IsNullOrEmpty(file) == false)
                                    {
                                        mediaFileList.Add(file);
                                    }
                                    ShowApp.DownLoadIndexNumber++;
                                    ShowApp.DownloadMessageRefresh();
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
                        else if (Utility.ToInt(pliTemlete.type) == TempleteItemType.Trademark.GetHashCode())
                        {
                            #region "Trademark"
                            List<string> trademarkFileList = new List<string> { };
                            List<TrademarkStyle> trademarkStyleList = new List<TrademarkStyle> { };
                            if (pliTemlete.itemData != null)
                            {
                                foreach (string url in pliTemlete.itemData.fileUrl)
                                {
                                    string file = WebApiInfo.DownloadFile(PlayApp.GetUrlFile(url), "");
                                    if (string.IsNullOrEmpty(file) == false)
                                    {
                                        trademarkFileList.Add(file);

                                        TrademarkSizeType sizeType = (TrademarkSizeType)Utility.ToInt(pliTemlete.DisplaySize);

                                        TrademarkPositionType positionType = (TrademarkPositionType)Utility.ToInt(pliTemlete.DisplayPostion);

                                        trademarkStyleList.Add(new TrademarkStyle(sizeType, positionType));
                                    }



                                    ShowApp.DownLoadIndexNumber++;
                                    ShowApp.DownloadMessageRefresh();
                                }
                            }


                            if (trademarkFileList.Count > 0)
                            {
                                TrademarkTempleteItem ttItem = new TrademarkTempleteItem(trademarkFileList.ToList(), trademarkStyleList.ToList(), Utility.ToInt(pliTemlete.DisplayIntevalSeconds));

                                this.PlayAddTemplete(ttItem);
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

            if (playListStateValue != PlayListStateType.Execute)
            {
                this.dtStartValue = this.dtNowValue;
                PlayRefreshTemplete();
            }

            playListStateValue = PlayListStateType.Execute;

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

        public void PlayRefresh()
        {
            playListStateValue = PlayListStateType.Wait;
            this.PlayRefreshTemplete();
        }

        public void PlayRefreshTemplete()
        {
            this.currentTempleteItemIndex = 0;
            foreach (TempleteItem titem in this.TempleteItemList)
            {
                titem.ExecuteRefresh();
            }

        }
        public bool CurrentTempleteValid()
        {
            try
            {
                if (this.ExecuteTempleteItem != null && this.ExecuteTempleteItem.TempleteState != TempleteStateType.Stop)
                {
                    this.currentTempleteItemIndex = this.ExecuteTempleteIndex;
                    return true;
                }

                this.currentTempleteItemIndex = this.ExecuteTempleteIndex;
                if (this.currentTempleteItemIndex < 0)
                {
                    this.currentTempleteItemIndex = 0;
                }

                while (this.currentTempleteItemIndex < this.TempleteItemList.Count
                && (this.CurrentTempleteItem.TempleteType == TempleteItemType.Message
                || this.CurrentTempleteItem.TempleteType == TempleteItemType.Trademark
                || this.CurrentTempleteItem.TempleteState == TempleteStateType.Stop))
                {
                    this.currentTempleteItemIndex++;
                }

                if (this.currentTempleteItemIndex >= this.TempleteItemList.Count)
                {
                    if (this.MessageTempleteItemList.Count == this.TempleteItemList.Count)
                    {
                        bool messageAllStop = true;
                        foreach (MessageTempleteItem mtItem in this.MessageTempleteItemList)
                        {
                            if (mtItem.TempleteState != TempleteStateType.Stop)
                            {
                                messageAllStop = false;
                                break;
                            }
                        }
                        if (messageAllStop)
                        {
                            this.PlayLast();
                        }
                        return false;
                    }
                    else
                    {
                        this.PlayLast();
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public void PlayAddTemplete(TempleteItem tItem)
        {
            this.templeteItemListValue.Add(tItem);
            if (tItem.TempleteType == TempleteItemType.Message)
            {
                this.messageListValue.Add(tItem as MessageTempleteItem);
            }
            else if (tItem.TempleteType == TempleteItemType.Trademark)
            {
                this.trademarkListValue.Add(tItem as TrademarkTempleteItem);
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
                return playListStateValue;
            }
        }

        public PlayListStateType CheckPlayListState
        {
            get
            {
                try
                {
                    if (this.templeteItemListValue.Count == 0)
                    {
                        return PlayListStateType.Stop;
                    }

                    if (this.IsInExecuteTime())
                    {
                        if (playListStateValue == PlayListStateType.Stop)
                        {
                            return PlayListStateType.Stop;
                        }

                        if (this.IsOutFixPlayTime())
                        {
                            playListStateValue = PlayListStateType.Stop;
                            return PlayListStateType.Stop;
                        }


                        if (playListStateValue == PlayListStateType.Execute)
                        {
                            bool allTempleteStop = true;
                            foreach (TempleteItem tItem in this.templeteItemListValue)
                            {
                                if (tItem.TempleteState != TempleteStateType.Stop)
                                {
                                    allTempleteStop = false;
                                    break;
                                }
                            }
                            if (allTempleteStop)
                            {
                                this.playListStateValue = PlayListStateType.Stop;
                                return PlayListStateType.Stop;
                            }
                        }

                        return PlayListStateType.Execute;
                    }
                    else
                    {
                        return PlayListStateType.Stop;
                    }

                }
                catch (Exception ex)
                {
                    LogApp.OutputErrorLog("PlayApp", "CheckPlayListState", ex);
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
                bool weekTimeisCheck = false;
                if (week == DayOfWeek.Sunday)
                {
                    weekTimeString = plSettings.Sunday;
                    weekTimeisCheck = plSettings.SundayisCheck;
                }
                else if (week == DayOfWeek.Monday)
                {
                    weekTimeString = plSettings.Monday;
                    weekTimeisCheck = plSettings.MondayisCheck;
                }
                else if (week == DayOfWeek.Tuesday)
                {
                    weekTimeString = plSettings.Tuesday;
                    weekTimeisCheck = plSettings.TuesdayisCheck;
                }
                else if (week == DayOfWeek.Wednesday)
                {
                    weekTimeString = plSettings.Wednesday;
                    weekTimeisCheck = plSettings.WednesdayisCheck;
                }
                else if (week == DayOfWeek.Thursday)
                {
                    weekTimeString = plSettings.Thursday;
                    weekTimeisCheck = plSettings.ThursdayisCheck;
                }
                else if (week == DayOfWeek.Friday)
                {
                    weekTimeString = plSettings.Friday;
                    weekTimeisCheck = plSettings.FridayisCheck;
                }
                else if (week == DayOfWeek.Saturday)
                {
                    weekTimeString = plSettings.Saturday;
                    weekTimeisCheck = plSettings.SaturdayisCheck;
                }


                if (string.IsNullOrEmpty(weekTimeString) == false && weekTimeisCheck)
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


    #region " 模板 "
    /// <summary>
    /// 模板
    /// </summary>
    public class TempleteItem
    {

        #region " variable "

        protected List<string> fileOrMessageListValue = new List<string>() { };

        protected List<ImageShowStyle> imageStyleListValue = new List<ImageShowStyle>() { };

        protected int currentIndex = -1;

        protected int currentShowStyleIndex = -1;

        protected ZoomOptionStyle zoomOptionValue = ZoomOptionStyle.None;

        protected List<TrademarkStyle> trademarkStyleListValue = new List<TrademarkStyle>() { };

        protected FillOptionStyle fillOptionValue = FillOptionStyle.None;

        protected MessagePositionType messageShowStyleValue = MessagePositionType.Bottom;

        protected List<MessageStyle> messageStyleListValue = new List<MessageStyle>() { };



        protected TempleteItemType templeteTypeValue = TempleteItemType.Image;


        protected TempleteStateType templeteStateValue = TempleteStateType.Wait;

        protected bool loadControlsFlag = false;
        /// <summary>
        /// 间隔时间(秒)
        /// </summary>
        protected int intervalSecondValue = 5;

        /// <summary>
        /// 上一个时间
        /// </summary>
        protected DateTime previousTimeValue = DateTime.Now;

        protected int slidingSpeedValue = 10;

        #endregion


        #region " propert "

        /// <summary>
        /// 类型
        /// </summary>
        public TempleteItemType TempleteType
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
                return templeteStateValue;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public TempleteStateType CheckTempleteState()
        {
            TempleteStateType stateType = TempleteStateType.Wait;
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    stateType = TempleteStateType.Stop;
                }

                DateTime nowTime = Utility.GetPlayDateTime(DateTime.Now);
                if (templeteTypeValue == TempleteItemType.Image)
                {
                    if (this.fileOrMessageListValue.Count == 0)
                    {
                        stateType = TempleteStateType.Stop;
                    }
                    else
                    {
                        if (this.currentIndex < this.fileOrMessageListValue.Count - 1)
                        {
                            stateType = TempleteStateType.Execute;
                        }
                        else
                        {
                            if (nowTime <= previousTimeValue.AddSeconds(intervalSecondValue))
                            {
                                stateType = TempleteStateType.Execute;
                            }
                            else
                            {
                                stateType = TempleteStateType.Stop;
                            }

                        }
                    }
                }
                else if (templeteTypeValue == TempleteItemType.Message)
                {
                    if (this.loadControlsFlag)
                    {
                        if (this.intervalSecondValue > 0)
                        {
                            if (previousTimeValue.AddSeconds(this.intervalSecondValue) < Utility.GetPlayDateTime(DateTime.Now))
                            {
                                stateType = TempleteStateType.Stop;
                            }
                        }
                        else
                        {
                            stateType = TempleteStateType.Execute;
                        }
                    }
                }
                else if (templeteTypeValue == TempleteItemType.Trademark)
                {
                    if (this.loadControlsFlag)
                    {
                        if (this.intervalSecondValue > 0)
                        {
                            if (previousTimeValue.AddSeconds(this.intervalSecondValue) < Utility.GetPlayDateTime(DateTime.Now))
                            {
                                stateType = TempleteStateType.Stop;
                            }
                        }
                        else
                        {
                            stateType = TempleteStateType.Execute;
                        }
                    }
                }

                return stateType;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "CheckTempleteState", ex);
                return stateType;
            }
        }


        #endregion

        /// <summary>
        /// 视频
        /// </summary>
        /// <param name="file"></param>
        /// <param name="zoomOption"></param>
        public TempleteItem(string file, ZoomOptionStyle zoomOption)
        {
            templeteTypeValue = TempleteItemType.Media;
            fileOrMessageListValue.Add(file);
            this.zoomOptionValue = zoomOption;
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="imageStyleList"></param>
        /// <param name="intervalSecond"></param>
        /// <param name="fillOption"></param>
        public TempleteItem(List<string> fileList, List<ImageShowStyle> imageStyleList, int intervalSecond, FillOptionStyle fillOption)
        {
            templeteTypeValue = TempleteItemType.Image;
            fillOptionValue = fillOption;
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

        /// <summary>
        /// 字幕
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="messageStyle"></param>
        /// <param name="intervalSecond"></param>
        /// <param name="slidingSpeed"></param>
        public TempleteItem(string htmlString, MessagePositionType messageStyle, int intervalSecond, int slidingSpeed)
        {
            templeteTypeValue = TempleteItemType.Message;

            ParseHtmlStringFormat(htmlString);

            messageShowStyleValue = messageStyle;

            this.intervalSecondValue = intervalSecond;

            this.slidingSpeedValue = slidingSpeed;

        }

        /// <summary>
        /// 商标
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="trademarkStyleList"></param>
        /// <param name="intervalSecond"></param>
        public TempleteItem(List<string> fileList, List<TrademarkStyle> trademarkStyleList, int intervalSecond)
        {
            templeteTypeValue = TempleteItemType.Trademark;

            foreach (string file in fileList)
            {
                fileOrMessageListValue.Add(file);
            }
            foreach (TrademarkStyle style in trademarkStyleList)
            {
                trademarkStyleListValue.Add(style);
            }

            this.intervalSecondValue = intervalSecond;
        }


        #region " void and function "
        public void ExecuteRefresh()
        {

            this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
            this.templeteStateValue = TempleteStateType.Wait;

            this.loadControlsFlag = false;
            this.currentIndex = -1;
            this.currentShowStyleIndex = -1;
        }


        /// <summary>
        /// 播放
        /// </summary>
        public void ExecuteStart()
        {
            if (this.TempleteState == TempleteStateType.Wait)
            {
                if (this.templeteTypeValue == TempleteItemType.Image)
                {
                    this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    this.currentIndex = -1;
                    this.currentShowStyleIndex = -1;
                    this.templeteStateValue = TempleteStateType.Execute;
                }
                else if (this.templeteTypeValue == TempleteItemType.Message)
                {
                    this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    this.currentIndex = -1;
                    this.currentShowStyleIndex = -1;
                    this.templeteStateValue = TempleteStateType.Execute;
                    loadControlsFlag = false;
                }
                else if (this.templeteTypeValue == TempleteItemType.Media)
                {
                    this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    this.currentIndex = -1;
                    this.currentShowStyleIndex = -1;
                    this.templeteStateValue = TempleteStateType.Execute;
                }
                else if (this.templeteTypeValue == TempleteItemType.Trademark)
                {
                    this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    this.currentIndex = -1;
                    this.currentShowStyleIndex = -1;
                    this.templeteStateValue = TempleteStateType.Execute;
                    loadControlsFlag = false;
                }
            }
        }

        public void ExecuteStop()
        {
            if (this.templeteTypeValue == TempleteItemType.Image)
            {
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Stop;
            }
            else if (this.templeteTypeValue == TempleteItemType.Message)
            {
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Stop;
                loadControlsFlag = false;
            }
            else if (this.templeteTypeValue == TempleteItemType.Media)
            {
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Stop;
            }
            else if (this.templeteTypeValue == TempleteItemType.Trademark)
            {
                previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                this.currentIndex = -1;
                this.currentShowStyleIndex = -1;
                this.templeteStateValue = TempleteStateType.Stop;
                loadControlsFlag = false;
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

                #region "Image"
                if (this.templeteTypeValue == TempleteItemType.Image)
                {

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

                            if (nowIndex != this.currentIndex)
                            {
                                this.currentIndex = nowIndex;
                                return true;
                            }
                        }
                    }
                }
                #endregion

                #region "Message"
                if (this.templeteTypeValue == TempleteItemType.Message)
                {
                    if (this.fileOrMessageListValue.Count > 0)
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

                #endregion

                #region "Media"
                if (this.templeteTypeValue == TempleteItemType.Media)
                {
                    if (this.fileOrMessageListValue.Count > 0)
                    {
                        if (this.templeteStateValue == TempleteStateType.Execute)
                        {
                            return true;
                        }
                    }
                }
                #endregion

                #region "Trademark"
                if (this.templeteTypeValue == TempleteItemType.Trademark)
                {
                    if (this.fileOrMessageListValue.Count > 0)
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

                #endregion
                return false;
            }
            catch
            {
                return false;
            }
        }



        private void ParseHtmlStringFormat(string htmlString)
        {
            try
            {
                fileOrMessageListValue.Clear();
                messageStyleListValue.Clear();

                string html = string.Format("<html>{0}</html>", htmlString);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlString);

                MessageFontStyle mfStyle = new MessageFontStyle();
                foreach (HtmlAgilityPack.HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                {
                    ParseHtmlNodeStringFormat(node, mfStyle);
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "ParseHtmlStringFormat", ex);
            }

        }


        private void ParseHtmlNodeStringFormat(HtmlAgilityPack.HtmlNode parentNode, MessageFontStyle parentMFStyle)
        {
            try
            {

                MessageFontStyle mfStyle = new MessageFontStyle();
                mfStyle.Copy(parentMFStyle);

                if (parentNode.Name.ToLower() == "b")
                {
                    mfStyle.AddFontStyle(FontStyle.Bold);
                }
                else if (parentNode.Name.ToLower() == "i")
                {
                    mfStyle.AddFontStyle(FontStyle.Italic);
                }


                foreach (HtmlAgilityPack.HtmlAttribute att in parentNode.Attributes)
                {
                    string attName = att.Name.ToLower().Trim();
                    if (attName == "style")
                    {
                        string[] attList = att.Value.Split(';');
                        foreach (string attValue in attList)
                        {
                            string[] attValueList = attValue.Split(':');
                            if (attValueList.Length > 1)
                            {
                                string styleName = attValueList[0].ToLower().Trim();
                                string styleValue = attValueList[1].Trim();
                                if (styleName == "font-family")
                                {
                                    mfStyle.SetFontFamily(styleValue);
                                }
                                else if (styleName == "font-size")
                                {
                                    mfStyle.SetFontSize(Utility.ToInt(styleValue.Replace("px", "")));
                                }
                                else if (styleName == "font-weight")
                                {
                                    if (styleValue.ToLower() == "bolder")
                                    {
                                        mfStyle.AddFontStyle(FontStyle.Bold);
                                    }
                                }
                                else if (styleName == "font-style")
                                {
                                    if (styleValue.ToLower() == "italic")
                                    {
                                        mfStyle.AddFontStyle(FontStyle.Italic);
                                    }
                                }
                            }
                        }
                    }
                    else if (attName == "color")
                    {
                        mfStyle.SetFontColor(att.Value.Trim());
                    }
                    else if (attName == "face")
                    {
                        mfStyle.SetFontFamily(att.Value.Trim());
                    }
                }

                if (parentNode.ChildNodes.Count == 0)
                {
                    if (string.IsNullOrEmpty(parentNode.InnerText) == false)
                    {
                        fileOrMessageListValue.Add(parentNode.InnerText);

                        SizeF size = new SizeF();

                        using (Label label = new Label())
                        {
                            label.Padding = new Padding(0);
                            label.Margin = new Padding(0);
                            size = label.CreateGraphics().MeasureString(parentNode.InnerText, mfStyle.Font);
                        }

                        messageStyleListValue.Add(new MessageStyle(mfStyle.Font, mfStyle.FontColor, (int)size.Width, (int)size.Height));
                    }

                }
                else
                {
                    foreach (HtmlAgilityPack.HtmlNode node in parentNode.ChildNodes)
                    {
                        ParseHtmlNodeStringFormat(node, mfStyle);
                    }
                }


            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayApp", "ParseHtmlStringFormat", ex);
            }

        }

        #endregion
    }


    /// <summary>
    /// 图片
    /// </summary>
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


        private string CurrentFile
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

        public ImageTempleteItem(List<string> fileList, List<ImageShowStyle> imageStyleList, int intervalSecond, FillOptionStyle fillOption) : base(fileList, imageStyleList, intervalSecond, fillOption)
        {
        }
        #region " void and function "


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
                    previousTimeValue = Utility.GetPlayDateTime(DateTime.Now).AddMinutes(1);

                    currentShowStyleIndex++;
                    if (currentShowStyleIndex >= this.imageStyleListValue.Count)
                    {
                        currentShowStyleIndex = 0;
                    }

                    if (picImage.Image != null)
                    {
                        picImage.Image.Dispose();
                    }

                    if (File.Exists(this.CurrentFile) == false)
                    {
                        PlayApp.CurrentPlayListNeedDownloadFile = true;
                    }

                    //动态添加图片
                    nowImageFile = ImageApp.GetBitmap(this.CurrentFile);

                    nowBitmap = new Bitmap(nowImageFile);

                    nowBitmap = ImageApp.ResizeBitmap(nowBitmap, picImage.Size, fillOptionValue);

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

    /// <summary>
    ///  前置 文字
    /// </summary>
    public class MessageTempleteItem : TempleteItem
    {

        #region " variable "




        #endregion


        #region " propert "

        public MessagePositionType ShowStyle
        {
            get
            {
                return messageShowStyleValue;
            }
        }

        public List<string> MessageList
        {
            get
            {
                return fileOrMessageListValue;
            }
        }

        public List<MessageStyle> MessageStyleList
        {
            get
            {
                return messageStyleListValue;
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
        public int SlidingSpeed
        {
            get
            {
                return slidingSpeedValue;
            }
        }

        #endregion
        public MessageTempleteItem(string htmlString, MessagePositionType messageStyle, int intervalSecond, int slidingSpeed) : base(htmlString, messageStyle, intervalSecond, slidingSpeed)
        {
        }
        #region " void and function "



        public void ShowCurrent(Control parentControl)
        {
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return;
                }

                if (this.loadControlsFlag == false)
                {
                    DrawMessage dmItem = new DrawMessage(parentControl.Width, parentControl.Height, this);
                    for (int messageIndex = 0; messageIndex < this.fileOrMessageListValue.Count; messageIndex++)
                    {
                        dmItem.AddDrawMessage(this.fileOrMessageListValue[messageIndex], this.messageStyleListValue[messageIndex]);
                    }

                    ShowApp.DrawMessageList.Add(dmItem);


                    if (this.intervalSecondValue > 0)
                    {
                        previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    }

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

    /// <summary>
    /// 视频
    /// </summary>
    public class MediaTempleteItem : TempleteItem
    {

        #region " variable "


        #endregion


        #region " propert "

        private string CurrentFile
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
        public FillOptionStyle FillOption
        {
            get
            {
                return fillOptionValue;
            }
        }

        #endregion

        public MediaTempleteItem(string file, ZoomOptionStyle zoomOption) : base(file, zoomOption)
        {
        }
        #region " void and function "



        public void ShowCurrent(AxWindowsMediaPlayer axWMP, WMPPlayState state, double position)
        {
            try
            {
                if (File.Exists(this.CurrentFile) == false)
                {
                    PlayApp.CurrentPlayListNeedDownloadFile = true;
                }

                if (this.fileOrMessageListValue.Count > 0)
                {
                    if (state == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        if (axWMP.playState != WMPPlayState.wmppsPlaying)
                        {
                            axWMP.URL = this.CurrentFile;
                            axWMP.Ctlcontrols.currentPosition = position;
                            axWMP.Ctlcontrols.play();
                        }
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


        public void ShowCurrent(VLCPlayer axVLCPlayer, IntPtr handle)
        {
            try
            {
                if (File.Exists(this.CurrentFile) == false)
                {
                    PlayApp.CurrentPlayListNeedDownloadFile = true;
                }

                axVLCPlayer.SetRenderWindow(handle);

                axVLCPlayer.Volume = 100;
                axVLCPlayer.SetFullScreen(false);

                axVLCPlayer.LoadFile(this.CurrentFile);//


                axVLCPlayer.Play();//播放

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


        public bool ReadaheadOverTime(int readaheadTime)
        {
            if (this.templeteStateValue == TempleteStateType.Execute)
            {
                if (DateTime.Now > this.previousTimeValue.AddSeconds(readaheadTime))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }


    /// <summary>
    ///  商标 (前置 图片)
    /// </summary>
    public class TrademarkTempleteItem : TempleteItem
    {

        #region " variable "




        #endregion


        #region " propert "


        #endregion
        public TrademarkTempleteItem(List<string> fileList, List<TrademarkStyle> trademarkStyleList, int intervalSecond) : base(fileList, trademarkStyleList, intervalSecond)
        {
        }
        #region " void and function "



        public void ShowCurrent(Control parentControl)
        {
            try
            {
                if (this.templeteStateValue == TempleteStateType.Stop)
                {
                    return;
                }

                if (this.loadControlsFlag == false)
                {
                    DrawTrademark dtItem = new DrawTrademark(parentControl.Width, parentControl.Height, this);
                    for (int trademarkIndex = 0; trademarkIndex < this.fileOrMessageListValue.Count; trademarkIndex++)
                    {
                        if (File.Exists(this.fileOrMessageListValue[trademarkIndex]) == false)
                        {
                            PlayApp.CurrentPlayListNeedDownloadFile = true;
                        }
                        dtItem.AddDrawTrademark(this.fileOrMessageListValue[trademarkIndex], this.trademarkStyleListValue[trademarkIndex]);
                    }


                    ShowApp.DrawTrademarkList.Add(dtItem);


                    if (this.intervalSecondValue > 0)
                    {
                        this.previousTimeValue = Utility.GetPlayDateTime(DateTime.Now);
                    }

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

    #endregion


    #region " 文本 "
    public class MessageStyle
    {

        #region " variable "
        private Font fontValue;
        private Color colorValue;
        private int widthValue;
        private int heightValue;

        #endregion


        #region " propert "

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

        public MessageStyle(Font font, Color color, int width, int height)
        {

            this.fontValue = font;
            this.colorValue = color;

            this.widthValue = width;
            this.heightValue = height;

        }
    }

    public class MessageFontStyle
    {

        #region " variable "
        string fontFamilyValue = Constants.MESSAGE_FONT_Family;
        int fontSizeValue = Constants.MESSAGE_FONT_Size;
        Color fontColorValue = Constants.MESSAGE_FONT_Color;
        FontStyle fontStyleValue = FontStyle.Regular;
        #endregion


        #region " propert "

        public string FontFamily
        {
            get
            {
                return fontFamilyValue;
            }
        }
        public int FontSize
        {
            get
            {
                return fontSizeValue;
            }
        }
        public Color FontColor
        {
            get
            {
                return fontColorValue;
            }
        }
        public FontStyle FontStyle
        {
            get
            {
                return fontStyleValue;
            }
        }
        public Font Font
        {
            get
            {
                return new Font(fontFamilyValue, fontSizeValue, fontStyleValue);
            }
        }

        #endregion

        public MessageFontStyle()
        {
        }
        public void Copy(MessageFontStyle mfStyle)
        {
            SetFontStyle(mfStyle.FontStyle);
            SetFontFamily(mfStyle.FontFamily);
            SetFontSize(mfStyle.FontSize);
            SetFontColor(mfStyle.FontColor);
        }
        public void AddFontStyle(FontStyle fontStyle)
        {
            fontStyleValue = fontStyleValue | fontStyle;
        }
        public void SetFontStyle(FontStyle fontStyle)
        {
            fontStyleValue = fontStyle;
        }
        public void SetFontFamily(string fontFamily)
        {
            try
            {
                Font fontTemp = new Font(fontFamily, 10);
                if (fontTemp.FontFamily.Name == fontFamily)
                {
                    fontFamilyValue = fontFamily;
                }
            }
            catch
            {

            }
        }
        public void SetFontSize(int fontSize)
        {
            fontSizeValue = fontSize;
        }
        public void SetFontColor(string fontColor)
        {
            try
            {
                Color colorTemp = ColorTranslator.FromHtml(fontColor);

                fontColorValue = colorTemp;
            }
            catch
            {
            }
        }
        public void SetFontColor(Color fontColor)
        {
            fontColorValue = fontColor;
        }
    }
    #endregion



    #region " 商标 (前置 图片) "

    /// <summary>
    /// 商标 (前置 图片)
    /// </summary>
    public class TrademarkStyle
    {

        #region " variable "
        private TrademarkPositionType trademarkPositionValue;
        private int percentValue;

        #endregion


        #region " propert "

        public TrademarkPositionType TrademarkPosition
        {
            get
            {
                return trademarkPositionValue;
            }
        }
        public int Percent
        {
            get
            {
                return percentValue;
            }
        }

        #endregion

        public TrademarkStyle(TrademarkSizeType sizeType, TrademarkPositionType trademarkPosition)
        {
            this.trademarkPositionValue = trademarkPosition;

            this.percentValue = Utility.ToInt( EnumHelper.GetDescription(sizeType));

        }
    }

    #endregion

}
