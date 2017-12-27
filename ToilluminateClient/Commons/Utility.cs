/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：ユーティリティ
 * 作成日・作成者：2017/05/26  張鵬
 ***********************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace ToilluminateClient
{
    /// ====================================================================
    /// クラス名：Utility
    /// <summary>
    /// ユーティリティ
    /// </summary>
    /// ====================================================================
    public static class Utility
    {
        #region externメソッド
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// ウィンドウのアクティブ化対策関数1
        /// </summary>
        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// ウィンドウのアクティブ化対策関数2
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        /// <summary>
        /// ウィンドウのアクティブ化対策関数3
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, Int32 wFlags);

        // 2013/03/27 K-Ishida:Start
        /// <summary>
        /// ウィンドウのアクティブ化対策関数4
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// ウィンドウのアクティブ化対策関数5
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// ウィンドウのアクティブ化対策関数6
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        /// <summary>
        /// ウィンドウのアクティブ化対策関数7
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        private const int SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000;
        private const int SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001;
        // 2013/03/27 K-Ishida:End

        #endregion

        #region 定数
        //ファイル読み書き権限
        private const int OF_READWRITE = 2;
        //ファイル共有権限
        private const int OF_SHARE_DENY_NONE = 0x40;
        //ファイルエラーハンドル
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        public const Int32 SWP_NOSIZE = 0x0001;

        public const Int32 SWP_NOMOVE = 0x0002;

        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        #endregion

        #region ウィンドウの最前面表示
        /// <summary>
        /// 指定のウィンドウを最前面表示します。
        /// </summary>
        /// <param name="handle">最前面表示するウィンドウのウィンドウハンドル</param>
        public static void ActivateWindow(IntPtr handle)
        {
            SetForegroundWindow(handle);
            SetActiveWindow(handle);
            SetWindowPos(handle, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
        public static void ForceActivateWindow(IntPtr handle)
        {
            IntPtr sp_time = IntPtr.Zero;

            uint nlpdwProcessId;

            // フォアグラウンドウィンドウを作成したスレッドのIDを取得			
            uint nForegroundID = GetWindowThreadProcessId(GetForegroundWindow(), out nlpdwProcessId);
            // 目的のウィンドウを作成したスレッドのIDを取得
            uint nTargetID = GetWindowThreadProcessId(handle, out nlpdwProcessId);

            // スレッドのインプット状態を結び付ける
            // デバッグステップ実行を行うと、ここ移行でデバッガへの入力が行えなくなるため注意。
            // 次のAttachThreadInput終了まで一気に実行すれば問題なし。
            AttachThreadInput(nTargetID, nForegroundID, true);

            // 現在の設定を sp_time に保存
            SystemParametersInfo(SPI_GETFOREGROUNDLOCKTIMEOUT, 0, sp_time, 0);

            // ウィンドウの切り替え時間を 0ms にする		
            SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, IntPtr.Zero, 0);

            // ウィンドウをフォアグラウンドに持ってくる
            ActivateWindow(handle);

            // 設定を元に戻す		
            SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, sp_time, 0);

            // スレッドのインプット状態を切り離す
            AttachThreadInput(nTargetID, nForegroundID, false);
        }
        #endregion

        #region NULLチェック
        /// <summary>
        /// 指定された object オブジェクトが null または DBNull.Value であるかどうかを示します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文字列がブランクまたはnullかどうか
        /// </summary>
        /// <param name="value">文字列</param>
        /// <returns>True:ブランクかnull False:ブランクかnull以外</returns>
        /// <remarks>.NET4.0のIsNullOrWhiteSpaceに合わせて、半角スペースと全角スペースも空白文字として判定します。</remarks>
        public static bool IsNullOrWhiteSpace(string value)
        {
            bool isResult = false;

            if (string.IsNullOrEmpty(value) == false)
            {
                // スペースを置換します。（IsNullOrEmptyの為）
                value = value.Replace(" ", "");		// 半角
                value = value.Replace("　", "");	// 全角
            }

            isResult = string.IsNullOrEmpty(value);

            return isResult;
        }

        #endregion

        #region ２値比較
        /// <summary>
        /// 比較フラグ
        /// </summary>
        [Flags]
        public enum CompareFlags
        {
            None = 0,
            // DBNull.Valueをnullと同一視する
            DBNullAsNull = 1,
            // string.Emptyをnullと同一視する
            EmptyAsNull = 2,
            // すべて
            All = DBNullAsNull | EmptyAsNull
        }

        /// <summary>
        /// ２値比較関数
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>２値が等しい場合はtrueを、それ以外の場合はfalseを返す。</returns>
        public static bool Compare(object value1, object value2)
        {
            return Compare(value1, value2, CompareFlags.None);
        }

        /// <summary>
        /// ２値比較関数
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="compareFlag"></param>
        /// <returns>２値が等しい場合はtrueを、それ以外の場合はfalseを返す。</returns>
        public static bool Compare(object value1, object value2, CompareFlags compareFlag)
        {
            bool isNull1 = (value1 == null);
            bool isNull2 = (value2 == null);

            if ((compareFlag & CompareFlags.DBNullAsNull) == CompareFlags.DBNullAsNull)
            {
                if (value1 == DBNull.Value)
                {
                    isNull1 = true;
                }
                if (value2 == DBNull.Value)
                {
                    isNull2 = true;
                }
            }
            if ((compareFlag & CompareFlags.EmptyAsNull) == CompareFlags.EmptyAsNull)
            {
                if (string.Empty.Equals(value1))
                {
                    isNull1 = true;
                }
                if (string.Empty.Equals(value2))
                {
                    isNull2 = true;
                }
            }

            if (isNull1 == true && isNull2 == true)
            {
                return true;
            }

            if (value1 == null || value2 == null)
            {
                return false;
            }
            else
            {
                return CompareSub(value1, value2);
            }
        }

        /// <summary>
        /// ２値比較関数
        /// </summary>
        /// <param name="value1">前提条件：null/DBNull.Value以外の値が入る</param>
        /// <param name="value2">前提条件：null/DBNull.Value以外の値が入る</param>
        /// <returns>２値が等しい場合はtrueを、それ以外の場合はfalseを返す。</returns>
        private static bool CompareSub(object value1, object value2)
        {
            if (value1.GetType().Equals(value2.GetType()) == false)
            {
                // 型が異なる場合
                return false;
            }
            // 次のソースでは、System.Collections.Genelic.Listが判定されなかった。
            //if (value1.GetType().IsArray)
            if (value1 is System.Collections.IEnumerable
                && value2 is System.Collections.IEnumerable)
            {
                // 配列型の場合
                // 配列型はEqualsでは正しく比較できないので、独自に比較する必要がある。
                return CompareSub((System.Collections.IEnumerable)value1,
                                  (System.Collections.IEnumerable)value2);
                /*
                object[] originalData = Convert.ToObjectArray(value1);
                object[] currentData = Convert.ToObjectArray(value2);
                if (originalData.Length == currentData.Length)
                {
                    int i = 0;
                    while ((i < originalData.Length) && (originalData[i] == currentData[i]))
                    {
                        i += 1;
                    }
                    if (i != originalData.Length)
                    {
                        // 元の値と、現在の値に変更が見つかった場合
                        return false;
                    }
                }
                return true;
                */
            }
            else
            {
                return value1.Equals(value2);
            }
        }

        /// <summary>
        /// ２値比較関数
        /// </summary>
        /// <param name="enumerable1"></param>
        /// <param name="enumerable2"></param>
        /// <returns></returns>
        private static bool CompareSub(System.Collections.IEnumerable enumerable1,
            System.Collections.IEnumerable enumerable2)
        {
            System.Collections.IEnumerator e1 = enumerable1.GetEnumerator();
            System.Collections.IEnumerator e2 = enumerable2.GetEnumerator();
            while (e1.MoveNext())
            {
                if (!e2.MoveNext())
                {
                    return false;
                }
                if (!e1.Current.Equals(e2.Current))
                {
                    return false;
                }
            }
            return !e2.MoveNext();
        }
        #endregion

        #region バイト数取得
        private static System.Text.Encoding _encoding_shiftjis =
            System.Text.Encoding.GetEncoding("Shift_JIS");
        /// <summary>
        /// ShiftJISにおけるバイト数を取得します。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetByteCount_ShiftJIS(string text)
        {
            return GetByteCount(_encoding_shiftjis, text);
        }

        /// <summary>
        /// 指定したエンコードにおけるバイト数を取得します。
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetByteCount(System.Text.Encoding encoding, string text)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                return encoding.GetByteCount(text);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region DOSコマンド実行
        private static StringBuilder _sbDoDosCommandStandardOutput = null;
        private static StringBuilder _sbDoDosCommandStandardError = null;
        /// <summary>
        /// DOSコマンドを実行します。<br />
        /// <br />
        /// isSynchronize = false の時は、戻り値は必ず true となる。<br />
        /// isSynchronize = true の時は、エラーの判断を行い、正しい戻り値を返す。<br />
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isSynchronize">非同期処理を行うかどうか</param>
        /// <returns></returns>
        public static bool DoDosCommand(string command, bool isSynchronize)
        {
            string standardOutput, standardError;
            return DoDosCommand(command, isSynchronize,
                out standardOutput, out standardError);
        }

        /// <summary>
        /// DOSコマンドを実行します。<br />
        /// <br />
        /// isSynchronize = false の時は、戻り値は必ず true となる。<br />
        /// isSynchronize = true の時は、エラーの判断を行い、正しい戻り値を返す。<br />
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isSynchronize">非同期処理を行うかどうか</param>
        /// <param name="standardOutput"></param>
        /// <param name="standardError"></param>
        /// <returns></returns>
        public static bool DoDosCommand(string command, bool isSynchronize,
            out string standardOutput, out string standardError)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // ComSpec(cmd.exe)のパスを取得する
            process.StartInfo.FileName =
                System.Environment.GetEnvironmentVariable("ComSpec");
            // 出力を読み取れるようにする
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            // ウィンドウを表示しないようにする
            process.StartInfo.CreateNoWindow = true;
            // コマンドラインを指定（"/c"は実行後閉じるために必要）
            process.StartInfo.Arguments = string.Format(@"/c {0}", command);

            // イベントのハンドリング
            process.ErrorDataReceived +=
                new System.Diagnostics.DataReceivedEventHandler(process_ErrorDataReceived);
            process.OutputDataReceived +=
                new System.Diagnostics.DataReceivedEventHandler(process_OutputDataReceived);

            // 起動
            process.Start();

            // 出力の読み込み開始
            _sbDoDosCommandStandardOutput = new StringBuilder();
            _sbDoDosCommandStandardError = new StringBuilder();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (isSynchronize && process != null)
            {
                //(親プロセス、子プロセスでブロック防止のため)
                process.WaitForExit();
                standardOutput = _sbDoDosCommandStandardOutput.ToString();
                standardError = _sbDoDosCommandStandardError.ToString();
                return string.IsNullOrEmpty(standardError);
            }
            standardOutput = string.Empty;
            standardError = string.Empty;
            return true;
        }

        private static void process_OutputDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e)
        {
            if (_sbDoDosCommandStandardOutput.Length == 0 && string.IsNullOrEmpty(e.Data))
            {
                // 初回に空文字が渡された場合
                // 初回の空文字は、無視する。
                // Outputがなくても必ず１回は空文字が送られてしまう問題の対策
            }
            else
            {
                _sbDoDosCommandStandardOutput.Append(e.Data + Environment.NewLine);
            }
        }

        private static void process_ErrorDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e)
        {
            if (_sbDoDosCommandStandardError.Length == 0 && string.IsNullOrEmpty(e.Data))
            {
                // 初回に空文字が渡された場合
                // 初回の空文字は、無視する。
                // Errorがなくても必ず１回は空文字が送られてしまう問題の対策
            }
            else
            {
                _sbDoDosCommandStandardError.Append(e.Data + Environment.NewLine);
            }
        }
        #endregion

        #region SQLクエリエスケープ
        /// <summary>
        /// SQLクエリーのエスケープ処理
        /// </summary>
        /// <remarks>
        /// 会社名に’がある会社の場合クエリーでエラーになったので追加。S_ACC_0010にも同じコードがあるので後日統一する
        /// </remarks>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string EscapeString(object original)
        {
            if (Utility.IsNull(original))
            {
                return string.Empty;
            }

            string result = original.ToString();
            result = result.Replace(@"'", @"''");

            return result;
        }
        #endregion

        #region 端数処理
        /// <summary>
        /// 端数処理
        /// 指定された値の小数点以下を切り捨てた値を返します。
        /// </summary>
        /// <param name="original">元の値</param>
        /// <returns>小数点以下を切り捨てた値</returns>
        public static decimal RoundDown(decimal original)
        {
            return Math.Truncate(original);
        }

        /// <summary>
        /// 端数処理
        /// 指定された値の小数点以下を切り上げた値を返します。
        /// </summary>
        /// <param name="original">元の値</param>
        /// <returns>小数点以下を切り上げた値</returns>
        public static decimal RoundUp(decimal original)
        {
            return Math.Ceiling(original);
        }

        /// <summary>
        /// 端数処理
        /// 指定された値の小数点以下を四捨五入した値を返します。
        /// </summary>
        /// <param name="original">元の値</param>
        /// <param name="digit">
        /// 小数点第何位までのデータとするか。
        /// 指定位の次以降が四捨五入されます。
        /// </param>
        /// <returns>四捨五入した値</returns>
        public static decimal RoundOff(decimal original, int digit)
        {
            return Math.Round(original, digit, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region EnableWindow()
        [DllImport("user32.dll")]
        public static extern bool IsWindowEnabled(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool EnableWindow(IntPtr hwnd, bool bEnable);
        #endregion

        #region iniファイルデコード
        /// <summary>
        /// INIファイルのデコード
        /// </summary>
        /// <param name="oldStr"></param>
        /// <returns></returns>
        public static string DecodeString(string oldStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return string.Empty;
            }
            return ExchangeText(ReverseText(Caesar(DecodeKey(oldStr))));
        }

        private const long glEncryptKey = 12;
        private static string DecodeKey(string oldStr)
        {
            string newStr = string.Empty;
            for (int i = 0; i < oldStr.Length; i++)
            {
                newStr += Convert.ToChar(Convert.ToInt32(oldStr[i]) ^ glEncryptKey);
            }
            return newStr;
        }

        private const long glCaesar = 2;
        private static string Caesar(string oldStr)
        {
            string newStr = string.Empty;
            for (int i = 0; i < oldStr.Length; i++)
            {
                newStr += Convert.ToChar(Convert.ToInt32(oldStr[i]) - glCaesar);
            }
            return newStr;
        }

        private static string ReverseText(string oldStr)
        {
            char[] newCharArray = oldStr.ToCharArray();
            Array.Reverse(newCharArray);
            return new string(newCharArray);
        }

        private static string ExchangeText(string oldStr)
        {
            string newStr = string.Empty;
            for (int i = 0; i < oldStr.Length; i += 2)
            {
                if (i + 1 < oldStr.Length)
                {
                    newStr += oldStr[i + 1];
                }
                newStr += oldStr[i];
            }
            return newStr;
        }
        #endregion

        #region 文字列の数値判断
        /// <summary>
        /// 指定文字列が数値変換可能かチェックします。
        /// </summary>
        /// <param name="text">文字列</param>
        /// <returns>
        /// true:数値変換可能
        /// false:数値変換不可能
        /// </returns>
        public static bool IsNumeric(string text)
        {
            decimal tmpValue;
            return decimal.TryParse(text, out tmpValue);
        }

        /// <summary>
        /// 文字列のうち、数値部分を抽出して数値を返します。
        /// 数値変換できない形式の場合は0を返します。
        /// </summary>
        /// <remarks>
        /// 　旧版と同じような数値変換処理を行います。
        /// 　おそらくInputManの仕様と思われます。
        /// 　例：123 → 123
        /// 　例：+123 → 123
        /// 　例：-123 → -123
        /// 　例：--0 → 0
        /// 　例：--123 → -123
        /// 　例：A12 → 12
        /// 　例：12A → 12
        /// 　例：1+2-3 → 1
        /// 　例：ABC → 0
        /// </remarks>
        /// <returns></returns>
        public static int ConvertNumeric(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            int length = value.Length;
            bool isExistNumber = false;
            StringBuilder resultString = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char ch = value[i];
                if (ch == '-')
                {
                    if (isExistNumber)
                    {
                        // 数値の途中で - が出てきた場合
                        break;
                    }
                    if (resultString.Length == 0)
                    {
                        // 初回のみ - を採用します。
                        resultString.Append(ch);
                    }
                }
                if ('0' <= ch && ch <= '9')
                {
                    isExistNumber = true;
                    resultString.Append(ch);
                }
                else
                {
                    if (isExistNumber)
                    {
                        // 数値の途中で数値以外の文字が出てきた場合
                        break;
                    }
                }
            }
            int result;
            int.TryParse(resultString.ToString(), out result);
            return result;
        }
        #endregion

        #region DBの値から変更
        /// <summary>
        /// DBの値からIntに変更
        /// </summary>
        /// <param name="obj">DBの値</param>
        /// <returns>数字</returns>
        public static int GetIntFromDB(object obj)
        {
            int n = 0;

            if (!(null == obj || DBNull.Value == obj))
            {
                if (!int.TryParse(obj.ToString(), out n))
                {
                    n = 0;
                }
            }

            return n;
        }
        /// <summary>
        /// DBの値からStringに変更
        /// </summary>
        /// <param name="obj">DBの値</param>
        /// <returns>文字列</returns>
        public static string GetStrFromDB(object obj)
        {
            string str = string.Empty;

            if (!(null == obj || DBNull.Value == obj))
            {
                str = obj.ToString();
            }
            return str;
        }
        #endregion

        #region 文字列処理
        /// <summary>
        /// 文字列の左側X文字を取得します
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="length">取得文字数</param>
        /// <returns>結果文字列</returns>
        public static string GetLeftString(string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (str.Length < length)
            {
                return str;
            }

            return str.Substring(0, length);
        }

        /// <summary>
        /// 文字列の右側X文字を取得します
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="length">取得文字数</param>
        /// <returns>結果文字列</returns>
        public static string GetRightString(string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (str.Length < length)
            {
                return str;
            }

            return str.Substring(str.Length - length);
        }

        /// <summary>
        /// 文字列の指定位置から指定長の文字列を取得します。(安全なsubstring)
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="startIndex">開始位置</param>
        /// <param name="length">長さ</param>
        /// <returns>結果文字列</returns>
        public static string GetMidString(string str, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (startIndex >= str.Length)
            {
                return string.Empty;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 文字列の指定位置から終端までの文字列を取得します。(安全なsubstring)
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="startIndex">開始位置</param>
        /// <returns>結果文字列</returns>
        public static string GetMidString(string str, int startIndex)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (startIndex >= str.Length)
            {
                return string.Empty;
            }

            return str.Substring(startIndex);
        }

        /// <summary>
        /// 英小文字を大文字に変換します。
        /// </summary>
        /// <param name="c">変換対象の文字</param>
        /// <returns>変換後の文字</returns>
        public static char ToUpper(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (char)(c - 32);
            }
            else
            {
                return c;
            }
        }

        /// <summary>
        /// 英大文字を小文字に変換します。
        /// </summary>
        /// <param name="c">変換対象の文字</param>
        /// <returns>変換後の文字</returns>
        public static char ToLower(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + 32);
            }
            else
            {
                return c;
            }
        }


        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="fontName">フォント名</param>
        /// <param name="fontSize">フォントサイズ</param>
        /// <returns>幅(mm)</returns>
        public static double GetStringWidth(int stringLength, string fontName, int fontSize)
        {
            return GetStringWidth(stringLength, new Font(fontName, fontSize));
        }
        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="font">フォント</param>
        /// <returns>幅(mm)</returns>
        public static double GetStringWidth(int stringLength, Font font)
        {
            SizeF sizeF = GetStringSizeF(stringLength, font);

            double stringLengthPT = sizeF.Width;
            return stringLengthPT;
        }

        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="fontName">フォント名</param>
        /// <param name="fontSize">フォントサイズ</param>
        /// <returns>幅(mm)</returns>
        public static double GetStringWidth(string stringValue, string fontName, int fontSize)
        {
            return GetStringWidth(stringValue, new Font(fontName, fontSize));
        }
        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="font">フォント</param>
        /// <returns>幅(mm)</returns>
        public static double GetStringWidth(string stringValue, Font font)
        {
            try
            {
                SizeF sizeF = GetStringSizeF(stringValue, font);
                double stringWidth = sizeF.Width;
                return stringWidth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="font">フォント</param>
        /// <returns>幅(mm)</returns>
        public static SizeF GetStringSizeF(string stringValue, Font font)
        {
            try
            {

                SizeF sizeF = new SizeF();
                using (Form form = new Form())
                {
                    using (Graphics graphics = form.CreateGraphics())
                    {
                        graphics.PageUnit = GraphicsUnit.Millimeter;

                        graphics.SmoothingMode = SmoothingMode.HighQuality;

                        sizeF = graphics.MeasureString(stringValue, font, 1000, StringFormat.GenericTypographic);
                    }
                }
                return sizeF;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 文字列の幅を取得する (mm)
        /// </summary>
        /// <param name="stringValue">文字列</param>
        /// <param name="font">フォント</param>
        /// <returns>幅(Pt)</returns>
        public static SizeF GetStringSizeF(int stringLength, Font font)
        {
            try
            {
                StringBuilder stringValue = new StringBuilder();
                for (int i = 0; i < stringLength; i++)
                {
                    stringValue.Append("0");
                }

                return GetStringSizeF(stringValue.ToString(), font);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 日付処理
        /// <summary>
        /// 日付をフォーマットする
        /// </summary>
        /// <param name="value">日付</param>
        /// <param name="format">フォーマット型</param>
        /// <returns>フォーマットされた日付を返します。</returns>
        public static string FormatDate(string value, string format)
        {
            DateTime dt;

            // 渡された日付の長さがゼロの場合、""を返す
            if (value.Length == 0)
                return "";

            // 渡されたフォーマット型の長さがゼロの場合、""を返す
            if (format.Length == 0)
                return value;

            // 日付をフォーマット型により、フォーマットする
            if (DateTime.TryParse(value, out dt))
                return dt.ToString(format);

            return "";
        }

        /// <summary>
        /// 日付をフォーマットする
        /// </summary>
        /// <param name="value">日付</param>
        /// <param name="format">フォーマット型</param>
        /// <param name="format">bool</param>
        /// <returns>フォーマットされた日付を返します。</returns>
        public static string FormatDate(string value, string format, bool t)
        {
            if (t == true)
            {
                DateTime dt;

                // 渡された日付の長さがゼロの場合、""を返す
                if (value.Length == 0)
                    return "";

                // 渡されたフォーマット型の長さがゼロの場合、""を返す
                if (format.Length == 0)
                    return value;

                // 日付をフォーマット型により、フォーマットする
                if (DateTime.TryParse(value, out dt))
                    return dt.ToString(format, DateTimeFormatInfo.InvariantInfo);

                return "";
            }
            else
            {
                return FormatDate(value, format);
            }
        }

        #endregion

        #region decimal変換

        /// <summary>
        /// オブジェクトをdecimalに変換します。
        /// 変換に失敗した場合は0を返します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したdecimal型の値</returns>
        public static decimal ToDecimal(object value)
        {
            if (Utility.IsNull(value))
            {
                return 0.0m;
            }

            decimal result;
            if (decimal.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0.0m;
            }
        }

        #endregion

        #region int変換

        /// <summary>
        /// オブジェクトをint型に変換します。
        /// 変換に失敗した場合は0を返します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したint型の値</returns>
        public static int ToInt(object value)
        {
            if (Utility.IsNull(value))
            {
                return 0;
            }

            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 数値を指定するかどうか
        /// </summary>
        /// <param name="value">チェックの値</param>
        /// <returns>
        /// true:Int32型
        /// false:Int32型 てはありません
        /// </returns>
        public static bool IsInt32(object value)
        {
            try
            {
                if (value == null
                 || string.IsNullOrEmpty(value.ToString()))
                {
                    return true;
                }
                Int32 result = 0;
                return Int32.TryParse(value.ToString(), out result);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region long変換

        /// <summary>
        /// オブジェクトをlong型に変換します。
        /// 変換に失敗した場合は0を返します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したlong型の値</returns>
        public static long ToLong(object value)
        {
            if (Utility.IsNull(value))
            {
                return 0;
            }

            long result;
            if (long.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return (long)0;
            }
        }

        #endregion

        #region double変換

        /// <summary>
        /// オブジェクトをdouble型に変換します。
        /// 変換に失敗した場合は0を返します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したint型の値</returns>
        public static double ToDouble(object value)
        {
            if (Utility.IsNull(value))
            {
                return 0;
            }

            double result;
            if (double.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region float変換

        /// <summary>
        /// オブジェクトをfloat型に変換します。
        /// 変換に失敗した場合は0を返します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したfloat型の値</returns>
        public static float ToFloat(object value)
        {
            if (Utility.IsNull(value))
            {
                return 0;
            }

            float result;
            if (float.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return (float)0;
            }
        }

        #endregion

        #region string 変換

        /// <summary>
        /// オブジェクトをstring型に変換します。
        /// </summary>
        /// <param name="value">変換対象オブジェクト</param>
        /// <returns>変換したstring型の値</returns>
        public static string ToString(object value)
        {
            if (Utility.IsNull(value))
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        #endregion

        #region 日時変換

        /// <summary>
        /// 日時を指定するかどうか
        /// </summary>
        /// <param name="objValue">チェックの値</param>
        /// <returns></returns>
        public static bool IsDateTime(object objValue)
        {
            DateTime dateValue = DateTime.Now;
            return IsDateTime(objValue, out dateValue);
        }
        /// <summary>
        /// 日時を指定するかどうか
        /// </summary>
        /// <param name="objValue">チェックの値</param>
        /// <param name="dateValue">日時変換</param>
        /// <returns></returns>
        public static bool IsDateTime(object objValue, out DateTime dateValue)
        {
            dateValue = DateTime.Now;
            try
            {
                string value = objValue.ToString();
                if (DateTime.TryParse(value, out dateValue))
                {
                    return true;
                }
                else
                {
                    if (value.Length >= 4)
                    {
                        string newDateString = value.Substring(0, value.Length - 4) + "-" + value.Substring(value.Length - 4, 2) + "-" + value.Substring(value.Length - 2);
                        if (DateTime.TryParse(newDateString, out dateValue))
                        {
                            return true;
                        }
                    }
                    // "10000-01-01" 364877.0   ; "3000-01-01"  1095362
                    double doubleValue = Utility.ToDouble(value) - 1;
                    //if (364877 <= doubleValue && doubleValue <= 1095362)
                    if (doubleValue > 0)
                    {
                        dateValue = DateTime.Parse("0001-01-01").AddDays(doubleValue);
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

        #endregion

        #region 日時変換

        /// <summary>
        /// 取得日期
        /// </summary>
        /// <param name="srcTime">日時</param>
        /// <returns></returns>
        public static DateTime GetPlayDateTime(DateTime srcTime)
        {
            DateTime reTime = DateTime.Parse(srcTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return reTime;
        }

        #endregion

        #region 生成長指定の任意文字列
        /// <summary>
        /// 生成長指定の任意文字列
        /// </summary>
        /// <param name="length">長指定</param>
        /// <returns>文字列</returns>
        public static string GetRandomString(int length)
        {
            return GetRandomString(length, new List<int> { }, new List<int> { });
        }
        /// <summary>
        /// 生成長指定の任意文字列
        /// </summary>
        /// <param name="length">長指定</param>
        /// <returns>文字列</returns>
        public static string GetRandomString(int length, List<int> regCodeCharIsNumberIndexList, List<int> regCodeCharIsNotNumberIndexList)
        {
            string keyCode = "adgjmpsvybehknqtwzcfilorux";
            string keyNumber = "0246813579";
            string key = keyNumber + keyCode;
            if (length < 1)
                return string.Empty;

            string allChar = string.Empty;
            string allCharNumber = string.Empty;
            string allCharCode = string.Empty;
            for (int keySum = 0; keySum < 3; keySum++)
            {
                allChar += key;
                allCharNumber += keyNumber;
                allCharCode += keyCode;
            }
            string oneChar = string.Empty;

            StringBuilder randomString = new StringBuilder();
            int charIndex = 0;
            while (randomString.Length < length)
            {
                if (regCodeCharIsNumberIndexList.Contains(charIndex))
                {
                    oneChar = GetRandomCode(allCharNumber);
                }
                else if (regCodeCharIsNotNumberIndexList.Contains(charIndex))
                {
                    oneChar = GetRandomCode(allCharCode);
                }
                else
                {
                    oneChar = GetRandomCode(allChar);
                }
                randomString.Append(oneChar);

                allChar = Regex.Replace(allChar, string.Format("(?<=^[^{0}]*){0}", oneChar), "");
                allCharNumber = Regex.Replace(allCharNumber, string.Format("(?<=^[^{0}]*){0}", oneChar), "");
                allCharCode = Regex.Replace(allCharCode, string.Format("(?<=^[^{0}]*){0}", oneChar), "");

                charIndex++;
            }
            return randomString.ToString();
        }

        
        public static string GetRandomCode(string charList)
        {
            if (string.IsNullOrEmpty(charList))
            {
                return string.Empty;
            }
            Random rand = new Random();
            int index = rand.Next(charList.Length - 1);
            return charList[index].ToString();
        }
        public static string GetRandomCode(List<string> stringList)
        {
            if (stringList==null || stringList.Count ==0)
            {
                return string.Empty;
            }
            Random rand = new Random();
            int index = rand.Next(stringList.Count - 1);
            return stringList[index].ToString();
        }
        #endregion

        #region "デジタル変換文字"
        public static string GetCellReference(int rowIndex, int colIndex)
        {
            return NumToLetter(colIndex) + rowIndex.ToString();
        }

        private static string[] Level = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string NumToLetter(int value)
        {
            if (value >= 1 && value <= 26)
            {
                return Level[value - 1];
            }
            else if (value >= 27 && value <= (26 * 27))
            {
                int remainder = value % 26;
                int front = (value - remainder) / 26;
                if (remainder == 0)
                {
                    remainder = 25;
                    front = front - 1;
                }
                else
                {
                    remainder = remainder - 1;
                }
                front = front - 1;

                return Level[front] + Level[remainder];
            }
            else
            {
                return Level[0];
            }
        }
        #endregion

        #region "色変換"
        /// <summary>
        /// 色Stringを取得する
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetColorString(string color)
        {
            string colorString = string.Empty;
            if (string.IsNullOrEmpty(color) == false)
            {
                try
                {
                    Color htmlColor = ColorTranslator.FromHtml(color);
                    if (htmlColor.IsEmpty == false)
                    {
                        colorString = ColorTranslator.ToHtml(htmlColor);
                    }
                }
                catch
                {
                    return "";
                }
            }
            return colorString;
        }
        #endregion

        #region "色の濃淡を変える"
        public static Color GetChangeColor(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            if (red < 0) red = 0;

            if (red > 255) red = 255;

            if (green < 0) green = 0;

            if (green > 255) green = 255;

            if (blue < 0) blue = 0;

            if (blue > 255) blue = 255;



            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
        #endregion

        #region " pt -> cm 変換"
        /// <summary>
        /// pt -> cm 変換を取得する
        /// </summary>
        /// <param name="ptValue"></param>
        /// <returns></returns>
        public static float GetcmFormpt(double ptValue)
        {
            return GetCeilingValue(ptValue * 2.54F / 72F);
        }
        #endregion

        #region " 小数点桁上げ 変換"
        /// <summary>
        /// 小数点桁上げを取得する
        /// </summary>
        /// <param name="ptValue"></param>
        /// <returns></returns>
        public static float GetCeilingValue(double value)
        {
            return GetCeilingValue(value, 2);
        }
        /// <summary>
        /// 小数点桁上げを取得する
        /// </summary>
        /// <param name="ptValue"></param>
        /// <returns></returns>
        public static float GetCeilingValue(double value, int digit)
        {

            return float.Parse((Math.Ceiling(value * Math.Pow(10, digit)) / Math.Pow(10, digit)).ToString());
        }
        #endregion

        #region "ファイル名を合法にチェック"
        /// <summary>
        /// ファイル名を合法にチェック  \/:*?"<>|
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        public static string GetValidFileName(string fileName)
        {
            string errChar = "\\/:*?\"<>|";
            string outFileName = fileName;
            if (string.IsNullOrEmpty(fileName) == false)
            {
                for (int i = 0; i < errChar.Length; i++)
                {
                    if (fileName.Contains(errChar[i].ToString()))
                    {
                        outFileName = outFileName.Replace(errChar[i].ToString(), string.Empty);
                    }
                }
            }
            return outFileName;
        }
        #endregion

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

        #region "string 組み合わせ 変換"
        /// <summary>
        /// string 組み合わせ 変換
        /// </summary>
        /// <param name="messageList">ファイルパス</param>
        /// <param name="maxNumberForLine">ファイル名</param>
        /// <param name="sepChar">ファイル名</param>
        /// <returns></returns>
        public static string GetFormatMessage1(List<string> messageList)
        {
            return GetFormatMessage1(messageList, 3);
        }
        public static string GetFormatMessage1(List<string> messageList, int maxNumberForLine)
        {
            return GetFormatMessage1(messageList, maxNumberForLine, " ");
        }
        /// 
        public static string GetFormatMessage1(List<string> messageList, int maxNumberForLine, string sepChar)
        {
            string formatMessage = string.Empty;
            int number = 0;
            foreach (string message in messageList)
            {
                number++;
                if (number > maxNumberForLine)
                {
                    formatMessage = string.Format("{0}\r\n", formatMessage);
                    number = 1;
                }

                formatMessage = string.Format("{0}{1}{2}", formatMessage, message, sepChar);
            }
            return formatMessage;
        }
        #endregion

        #region " ファイルを読む "
        /// <summary>
        /// read file to byte array
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string file)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        #endregion

        #region " 書きファイル "
        /// <summary>
        /// 書きファイル
        /// </summary>
        /// <param name="file"></param>
        /// <param name="intBuffer"></param>
        /// <param name="size"></param>
        public static void SaveFileFromIntPtr(string file, IntPtr intBuffer, int size)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                byte[] bytes = new byte[size];
                Marshal.Copy(intBuffer, bytes, 0, size);
                fs.Write(bytes, 0, size);
            }
        }
        #endregion

        #region " 書きファイル "
        /// <summary>
        /// 書きファイル
        /// </summary>
        /// <param name="file"></param>
        /// <param name="writeString"></param>
        public static void WriterFile(string file, string writeString)
        {
            try
            {
                // ディレクトリが見つからなかった場合、ディレクトリを作成
                if (Directory.Exists(Path.GetDirectoryName(file)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                }

                using (StreamWriter writer = new StreamWriter(file, true, new System.Text.UTF8Encoding(true)))
                {
                    writer.WriteLine(writeString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Utility:" + ex.Message);
                // 何もしない
            }
        }
        #endregion

        #region " ファイルを読 "
        /// <summary>
        /// ファイルを読
        /// </summary>
        /// <param name="file"></param>
        public static string ReaderFile(string file)
        {
            try
            {
                string readerString = string.Empty;
                if (File.Exists(file))
                {
                    using (StreamReader reader = new StreamReader(file, new System.Text.UTF8Encoding(true)))
                    {
                        readerString = reader.ReadLine();
                    }
                }
                return readerString;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Utility:" + ex.Message);
                return string.Empty;
                // 何もしない
            }
        }
        #endregion
        

        #region "ファイル 読み取 Base64"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadFileToBase64String(string file)
        {
            try
            {
                StringBuilder reString = new StringBuilder();
                string fileName = Path.GetFileName(file);
                string filePath = Path.GetDirectoryName(file);

                #region "読みファイルモード - ブロック下載"

                int bytesLength = Constants.READ_FILE_BYTE_MAX_LENGTH;// 毎回ファイル だけ 3K 読み取り
                byte[] bytes = new byte[bytesLength];

                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long dataLengthToRead = fileStream.Length;
                    while (dataLengthToRead > 0)
                    {

                        if (dataLengthToRead < bytesLength)
                        {
                            bytesLength = Utility.ToInt(dataLengthToRead.ToString());
                        }

                        bytes = new byte[bytesLength];

                        //読み取り
                        int lengthRead = fileStream.Read(bytes, 0, bytesLength);

                        dataLengthToRead = dataLengthToRead - lengthRead;

                        //变为Base64
                        reString.Append(Convert.ToBase64String(bytes));
                    }
                }

                #endregion

                return reString.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region ""
        public static string GetRandomFileName(string path, string description)
        {
            try
            {
                string fileName = string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Utility.GetRandomString(8), description);
                string randomFile = Utility.GetFullFileName(path, fileName);
                while (File.Exists(randomFile))
                {
                    fileName = string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Utility.GetRandomString(8), description);
                    randomFile = Utility.GetFullFileName(path, fileName);
                }

                return randomFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
