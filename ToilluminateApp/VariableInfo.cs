using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToilluminateApp
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
        

        /// <summary>
        /// システムのファイル
        /// </summary>
        private static string iniFile = string.Empty;

        #region フィールド
       
        #endregion

        #region publicプロパティ

      

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

                tempPath = VariableInfo.GetFullFileName(clientPath, "Temp");
                filesPath = VariableInfo.GetFullFileName(clientPath, "Files");
                logsPath = VariableInfo.GetFullFileName(clientPath, "Logs");

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

        #region "完全パス"
        /// <summary>
        /// 完全パス
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        public static string GetFullFileName(string path, string file)
        {
            string fullFileName = string.Format("{0}\\{1}", path, file);
            fullFileName = fullFileName.Replace("\\\\", "\\");
            return fullFileName;
        }
        #endregion
        #endregion
    }

}
