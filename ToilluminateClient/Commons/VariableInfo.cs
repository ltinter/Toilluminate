﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateClient
{
    public static class VariableInfo
    {
        /// <summary>
        /// client path
        /// </summary>
        private static string clientPath = string.Empty;
        private static string tempPath = string.Empty;

        /// <summary>
        /// システムのファイル
        /// </summary>
        private static string iniFile = string.Empty;

        public static void OutputClientLog(Exception ex)
        {

        }

        #region フィールド
        /// <summary>
        /// 用户ID
        /// </summary>
        private static string playerID = "0";
        
        #endregion

        #region publicプロパティ

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string PlayerID
        {
            get
            {
                return playerID;
            }
        }
        

        /// <summary>
        /// 放送リストID
        /// </summary>
        public static string ClientPath
        {
            get
            {
                return clientPath;
            }
        }
        /// <summary>
        /// 放送リストID
        /// </summary>
        public static string IniFile
        {
            get
            {
                return iniFile;
            }
        }

        public static string TempPath
        {
            get
            {
                return tempPath;
            }
        }

        #endregion

        #region publicメソッド


        /// <summary>
        /// 共通変数が初期化
        /// </summary>
        public static void InitVariableInfo()
        {
            clientPath = Application.StartupPath;
#if DEBUG
            clientPath = new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.FullName;
#endif
            iniFile = clientPath + "\\" + Constants.INI_NAME;

            tempPath = Utility.GetFullFileName(clientPath, "temp");
        }


        /// <summary>
        /// 共通変数が初期化
        /// </summary>
        public static void RefreshPlayListInfo()
        {
            return;
            playerID = "2";
            try
            {
                string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYERMASTERS_SEND, playerID));
                string getJsonString = WebApiInfo.HttpGet(urlString);
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("VariableInfo", "RefreshPlayListInfo:1", ex);
            }

            //try
            //{
            //    string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYERMASTERS_GET_STATUS, playerID));
            //    string getJsonString = WebApiInfo.HttpGet(urlString);
            //}
            //catch (Exception ex)
            //{
            //    LogApp.OutputErrorLog("VariableInfo", "RefreshPlayListInfo:2", ex);
            //}


            try
            {
                string urlString = string.Format("http://{0}/{1}", IniFileInfo.WebApiAddress, string.Format(Constants.API_PLAYLISTMASTERS_GET_LIST, playerID));
                string getJsonString = WebApiInfo.HttpPost(urlString, "");

                if (PlayApp.CurrentPlayListJsonString != getJsonString)
                {
                    PlayApp.CurrentPlayListJsonString = getJsonString;
                    PlayApp.Clear();


                    PlayApp.PlayListMasterArray = new List<PlayListMaster> { };

                    JArray plmArray = (JArray)JsonConvert.DeserializeObject(PlayApp.CurrentPlayListJsonString);
                    foreach (JObject obj in plmArray)
                    {
                        PlayListMaster plmStudent = new PlayListMaster();

                        plmStudent = JsonConvert.DeserializeAnonymousType(obj.ToString(), plmStudent);

                        if (string.IsNullOrEmpty(plmStudent.Settings) == false)
                        {
                            PlayApp.PlayListMasterArray.Add(plmStudent);
                        }
                    }

                    foreach (PlayListMaster plmItem in PlayApp.PlayListMasterArray)
                    {
                        PlayListSettings plsStudent = new PlayListSettings();
                        plsStudent = JsonConvert.DeserializeAnonymousType(plmItem.Settings, plsStudent);


                        PlayList plItem = new PlayList(plmItem.PlayListID,plsStudent);



                        PlayApp.PlayListArray.Add(plItem);
                    }

                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("VariableInfo", "RefreshPlayListInfo:3", ex);
            }
        }
        #endregion
    }

    public class PlayListSettings
    {
        public string Loop { get; set; }
        public string Playtime { get; set; }
        public string PlayHours { get; set; }
        public string PlayMinites { get; set; }
        public string PlaySeconds { get; set; }

        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

        public PlaylistItem[] PlaylistItems { get; set; }
        
    }
    public class PlaylistItem
    {
        public string PlaylistItemName { get; set; }
        public string DisplayIntevalSeconds { get; set; }
        public string SildeshowEffects { get; set; }
        public string type { get; set; }
        public PlaylistItemData itemData { get; set; }
    }
    
    public class PlaylistItemData
    {
        public string name { get; set; }
        public string[] id { get; set; }
        public string[] src { get; set; }
    }

}
