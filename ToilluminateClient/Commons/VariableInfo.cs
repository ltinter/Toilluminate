﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// 放送ID
        /// </summary>
        private static int playID = 0;

        /// <summary>
        /// 放送リストID
        /// </summary>
        private static int playListID = 0;

        #endregion

        #region publicプロパティ

        /// <summary>
        /// 放送ID
        /// </summary>
        public static int PlayID
        {
            get
            {
                return playID;
            }
        }

        /// <summary>
        /// 放送リストID
        /// </summary>
        public static int PlayListID
        {
            get
            {
                return playListID;
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

        #endregion
    }

}
