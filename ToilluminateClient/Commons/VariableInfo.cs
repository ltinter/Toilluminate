using Newtonsoft.Json;
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
        private static string filesPath = string.Empty;
        private static string logsPath = string.Empty;

        
        public static MessageForm messageFormInstance = new MessageForm();

        /// <summary>
        /// システムのファイル
        /// </summary>
        private static string iniFile = string.Empty;

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
        public static string FilesPath
        {
            get
            {
                return filesPath;
            }
        }
        public static string LogsPath
        {
            get
            {
                return logsPath;
            }
        }

        #endregion

        #region publicメソッド


        /// <summary>
        /// 共通変数が初期化
        /// </summary>
        public static void InitVariableInfo()
        {
            try
            {
                clientPath = Application.StartupPath;

                iniFile = clientPath + "\\" + Constants.INI_NAME;

                tempPath = Utility.GetFullFileName(clientPath, "Temp");
                filesPath = Utility.GetFullFileName(clientPath, "Files");
                logsPath = Utility.GetFullFileName(clientPath, "Logs");

                if (Directory.Exists(tempPath) == false)
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (Directory.Exists(filesPath) == false)
                {
                    Directory.CreateDirectory(filesPath);
                }

                if (Directory.Exists(logsPath) == false)
                {
                    Directory.CreateDirectory(logsPath);
                }

            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("VariableInfo", "InitVariableInfo", ex);
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
        public string type { get; set; }
        public string DisplayIntevalSeconds { get; set; }
        
        public string[] SildeshowEffects { get; set; }
        public PlaylistItemData itemData { get; set; }
        

        public string SlidingSpeed { get; set; }
        public string TextPostion { get; set; }

        public string itemTextData { get; set; }
  
        public string ZoomOption { get; set; }
    }

    public class PlaylistItemData
    {
        public string name { get; set; }
        public string[] id { get; set; }
        public string[] src { get; set; }

        public string[] fileUrl { get; set; }
        
    }

}
