/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：ログ管理
 * 作成日・作成者：2017/05/26  張鵬
 ***********************************************************************/
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;

namespace ToilluminateClient
{
    /// <summary>
    /// ログ出力管理クラス
    /// エラーログ/ユーザー処理ログ/自動処理ログの出力をサポートします。
    /// </summary>
    public static class LogApp
    {
        #region 定数

        /// <summary>
        /// 処理ログファイル格納ディレクトリ
        /// </summary>
        public static readonly string ERROR_LOG_FILE_DIRECTORY = @"\Log\ErrorLog";
        public static readonly string PROCESS_LOG_FILE_DIRECTORY = @"\Log\ProcessLog";

        /// <summary>
        /// エラーログファイル名
        /// </summary>
        public static readonly string ERROR_LOG_FILENAME_FORMAT = ERROR_LOG_FILE_DIRECTORY + @"\ErrorLog{0}.log";

        /// <summary>
        /// 工程ログファイル名
        /// </summary>
        private static readonly string PROCESS_LOG_FILENAME_FORMAT = PROCESS_LOG_FILE_DIRECTORY + @"\ProcessLog{0}.log";
        

        #endregion

        #region privateプロパティ

        // 日付ローテーション用のファイル名を作成する場所をまとめるため、
        // 以下のプロパティを追加しました。 YAMAZAKI
        

        /// <summary>
        /// エラーログのファイル名を取得します。
        /// </summary>
        private static string ErrorLogFileName
        {
            get
            {
                return ERROR_LOG_FILENAME_FORMAT;
            }
        }

        /// <summary>
        /// エラーログのフルパスを取得します。
        /// </summary>
        private static string ErrorLogFIleFullPath
        {
            get
            {
                return GetLogFileFullPathName(ErrorLogFileName);
            }
        }

        #endregion

        #region publicメソッド

        #region ログファイルフルパス取得

        /// <summary>
        /// ログファイルのフルパスを取得します
        /// </summary>
        /// <param name="className">ログファイル定数の文字列</param>
        /// <remarks>
        /// ログファイルのフルパス参照を統一するために追加
        /// フルパス参照する必要がある場合は、定数の直接利用ではなく、こちらでフルパス名を取得してください。
        /// インストールパスからの相対が必要な場合は、定数を直接利用してください
        /// </remarks>
        public static String GetLogFileFullPathName(String logFileName)
        {
            return string.Format("{0}\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, logFileName);
        }

        #endregion

        #region エラーログ

        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="methodName">エラー番号</param>
        /// <param name="exception">エラー内容</param>
        public static void OutputErrorLog(string className, string methodName, Exception exception)
        {
            OutputErrorLog(className, methodName, exception.Message);
        }
        public static void OutputErrorLog(string className, string methodName, string messsage)
        {
            try
            {
#if DEBUG
                if (string.IsNullOrEmpty(messsage))
                {
                    Debug.WriteLine(messsage);
                    Console.WriteLine(messsage);
                }
#endif
                if (IniFileInfo.CanOutputLog == false || string.IsNullOrEmpty(messsage))
                {
                    return;
                }

                //・日時（秒単位）：yyyy/mm/dd hh:mm:ss
                //・発生箇所：ﾓｼﾞｭｰﾙ名
                //・ｴﾗｰ内容：exception.Description
                string text = string.Format(
                    "{0}\t{1}\t{2}\t{3}",
                    DateTime.Now.ToString(),
                    className + "." + methodName + "()",
                    messsage
                );

                WriteLogFile(text, ErrorLogFIleFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region 処理ログ

        /// <summary>
        /// ユーザー処理ログを出力します。
        /// DBアクセス・ファイルインポート/エクスポート時に利用します。
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="methodName">メソッド名</param>
        /// <param name="detail">
        /// 処理内容詳細
        /// DBアクセスやファイルインポート/エクスポート時に記述します。
        /// </param>
        public static void OutputProcessLog(string className, string methodName, string detail)
        {
            try
            {
#if DEBUG
                if (string.IsNullOrEmpty(detail))
                {
                    Debug.WriteLine(detail);
                    Console.WriteLine(detail);
                }
#endif

                if (IniFileInfo.CanOutputLog == false || string.IsNullOrEmpty(detail))
                {
                    return;
                }

                //処理日時				：	yyyy/mm/dd hh:mm:ss
                //Version				：	ﾊﾞｰｼﾞｮﾝ番号
                //PCのPC名とIPアドレス	：	IP:XXX.XXX.XXX.XXX
                //メソッド名			    ：	クラス名＋メソッド名。Xxxxx.XxxXxx()
                //処理内容				：	操作内容について（ﾎﾞﾀﾝ、検索条件、検索結果）
                string text = string.Format(
                    "{0}\t{1}\t{2}\t{3}\t{4}",
                    DateTime.Now.ToString(),
                    "Ver" + Constants.Version, // 修正箇所：バージョン情報を保持するので、ここを置き換え。
                    Dns.GetHostName() + " " + GetIPv4Address(),
                    className + "." + methodName + "()",
                    detail
                );

                // 今日の操作ログのファイル名を取得します。
                string todayProcessLogFileFullPath = GetLogFileFullPathName(string.Format(PROCESS_LOG_FILENAME_FORMAT, DateTime.Now.ToString("dd")));

                WriteLogFile(text, todayProcessLogFileFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
  
        
        
        /// <summary>
        /// 指定したパスのファイルが一ヶ月前のものであれば削除します。
        /// </summary>
        /// <param name="fileNameFormat">ログファイルパス</param>
        /// <remarks>
        ///	ローテーション管理するログすべてに利用します。
        ///	TimeSpanだとミリ秒単位の厳密な差での比較となるため
        /// 同日名のファイルを全日に作成したとかでは誤認が発生するが、
        /// 目的は前月のファイルに追記することを避けることなので、TimeSpanを使っても問題は発生しない。
        /// </remarks>
        private static bool DeleteOldFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // 最終更新日が過去の日付の場合はファイルを削除する。
                    TimeSpan span = DateTime.Now - File.GetLastWriteTime(filePath);
                    if (span.Days > 0)
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        #endregion
        
        #endregion

        #region privateメソッド
        
        /// <summary>
        /// ログファイルにログを出力します。
        /// </summary>
        /// <param name="message">出力内容</param>
        /// <param name="fileName">出力ファイル名(フルパス)</param>
        private static void WriteLogFile(string message, string fileName)
        {
            try
            {
                // ディレクトリが見つからなかった場合、ディレクトリを作成
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }

                using (StreamWriter swStreamWriter = new StreamWriter(fileName, true, new System.Text.UTF8Encoding(true)))
                {
                    swStreamWriter.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LogManager:" + ex.Message);
                // 何もしない
            }
        }

        #endregion
        
        /// <summary>
        /// IPv4アドレスを返す。
        /// </summary>
        /// <returns></returns>
        private static string GetIPv4Address()
        {

            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

                
    }
}
