/*
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：ToilluminateClient
 * プログラム名称：起動
 * 作成日・作成者：2017/10/11  zhangpeng
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;

namespace ToilluminateClient
{
    static class Program
    {

        #region EntryPoint
        /// <summary>
        /// The main entry point for the application.
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // 多重起動防止処理
                Mutex mutex = new Mutex(false, Application.ProductName);
                if (args.Length == 0)
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        GC.KeepAlive(mutex);
                        mutex.Close();
                        return;
                    }
                }
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //共通変数が初期化
                VariableInfo.InitVariableInfo();

                // 設定ファイルのオープン
                if (File.Exists(VariableInfo.IniFile) == false)
                {
                    IniFileInfo.CreateDefaultIniFile(VariableInfo.IniFile);
                }

                // 設定読み込み
                IniFileInfo.GetIniInfo(VariableInfo.IniFile);
                DictionaryInfo.InitMultilingualDictionaryForClient();
                
                //LoginForm loginFormInstance = new LoginForm();
                //Application.Run(loginFormInstance);
                //return;

                // メイン画面起動
                MainForm mainFormInstance = new MainForm();

                Application.Run(mainFormInstance);

                mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("Program", "Main", ex);
            }
        }

        #endregion

    }
}
