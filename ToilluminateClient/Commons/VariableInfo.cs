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
                

                if (Directory.Exists(tempPath) == false)
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (Directory.Exists(filesPath) == false)
                {
                    Directory.CreateDirectory(filesPath);
                }
                
                string logsPath = Utility.GetFullFileName(clientPath, LogApp.LOG_FILE_DIRECTORY); 
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

    public class PlayerInfo
    {
        public string PlayerID { get; set; }
        public string GroupID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerAddress { get; set; }
        public string ActiveFlag { get; set; }
        public string OnlineFlag { get; set; }
        public string Comments { get; set; }
        public string UpdateDate { get; set; }
        public string InsertDate { get; set; }
        public string ErrorFlag { get; set; }
        public string PlayerLog { get; set; }
        public string UseFlag { get; set; }


        public string Settings { get; set; }


        public PlayerSettings playerSettings { get; set; }

        public bool IsInExecuteTime()
        {
            try
            {
                bool isInExecuteTime = true;
                isInExecuteTime = playerSettings.IsInExecuteTime();



                return isInExecuteTime;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("PlayerInfo", "InExecuteTime", ex);
                return false;
            }
        }
    }

    public class PlayerSettings
    {
        public string resolution { get; set; }
        public string ActiveFlag { get; set; }
        public string OnlineFlag { get; set; }

        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

        public bool MondayisCheck { get; set; }
        public bool TuesdayisCheck { get; set; }
        public bool WednesdayisCheck { get; set; }
        public bool ThursdayisCheck { get; set; }
        public bool FridayisCheck { get; set; }
        public bool SaturdayisCheck { get; set; }
        public bool SundayisCheck { get; set; }


        public bool IsInExecuteTime()
        {
            try
            {
                DateTime nowTime = DateTime.Now;
                DayOfWeek week = nowTime.DayOfWeek;

                string weekTimeString = string.Empty;
                bool weekTimeisCheck = false;
                if (week == DayOfWeek.Sunday)
                {
                    weekTimeString = this.Sunday;
                    weekTimeisCheck = this.SundayisCheck;
                }
                else if (week == DayOfWeek.Monday)
                {
                    weekTimeString = this.Monday;
                    weekTimeisCheck = this.MondayisCheck;
                }
                else if (week == DayOfWeek.Tuesday)
                {
                    weekTimeString = this.Tuesday;
                    weekTimeisCheck = this.TuesdayisCheck;
                }
                else if (week == DayOfWeek.Wednesday)
                {
                    weekTimeString = this.Wednesday;
                    weekTimeisCheck = this.WednesdayisCheck;
                }
                else if (week == DayOfWeek.Thursday)
                {
                    weekTimeString = this.Thursday;
                    weekTimeisCheck = this.ThursdayisCheck;
                }
                else if (week == DayOfWeek.Friday)
                {
                    weekTimeString = this.Friday;
                    weekTimeisCheck = this.FridayisCheck;
                }
                else if (week == DayOfWeek.Saturday)
                {
                    weekTimeString = this.Saturday;
                    weekTimeisCheck = this.SaturdayisCheck;
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
                LogApp.OutputErrorLog("PlayerSettings", "InExecuteTime", ex);
                return false;
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
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

        public bool MondayisCheck { get; set; }
        public bool TuesdayisCheck { get; set; }
        public bool WednesdayisCheck { get; set; }
        public bool ThursdayisCheck { get; set; }
        public bool FridayisCheck { get; set; }
        public bool SaturdayisCheck { get; set; }
        public bool SundayisCheck { get; set; }


        public PlaylistItem[] PlaylistItems { get; set; }

    }
    public class PlaylistItem
    {
        public string PlaylistItemName { get; set; }
        public string type { get; set; }
        public string DisplayIntevalSeconds { get; set; }


        public string PicturePostion { get; set; }
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
