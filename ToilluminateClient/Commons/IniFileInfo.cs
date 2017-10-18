﻿/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：iniファイル管理
 * 作成日・作成者：2017/05/26  張鵬
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace ToilluminateClient
{
    /// <summary>
    /// iniファイル管理クラス
    /// </summary>
    public static class IniFileInfo
    {

        /// <summary>
        /// 言語辞書 - Client Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> clientMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();

        /// <summary>
        /// 言語辞書 - Message Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> messageMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();

        /// <summary>
        /// 言語辞書 - Enum Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> enumMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();
        
        /// <summary>
        /// 言語辞書種別
        /// </summary>
        private static MultilingualDictionaryType multiDictType = MultilingualDictionaryType.Japanese;

        /// <summary>
        /// 言語辞書 - Client Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> defaultClientMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();

        /// <summary>
        /// 言語辞書 - Message Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> defaultMessageMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();

        /// <summary>
        /// 言語辞書 - Enum Test
        /// </summary>
        public static Dictionary<string, MultilingualDictionary> defaultEnumMultilingualDictionaryList = new Dictionary<string, MultilingualDictionary>();

        /// <summary>
        /// iniフォルダ
        /// </summary>
        private static string iniFileDir = string.Empty;
        
        #region 変数
        /// <summary>
        /// logが出力することが
        /// </summary>
        private static Boolean canOutputLog = false;

        #endregion 変数

        #region publicプロパティ

        /// <summary>
        /// 臨時フォルダ
        /// </summary>
        public static string TempPath
        {
            get;
            set;
        }

         /// <summary>
        /// 抓图
        /// </summary>
        public static PhotoType Capture
        {
            get;
            set;
        }

        /// <summary>
        /// logが出力することが
        /// </summary>
        public static Boolean CanOutputLog
        {
            get
            {
                return canOutputLog;
            }
        }

        /// <summary>
        /// iniフォルダ
        /// </summary>
        public static string IniFileDir
        {
            get
            {
                return iniFileDir;
            }
        }

        /// <summary>
        /// 言語辞書種別
        /// </summary>
        public static MultilingualDictionaryType MultiDictType
        {
            get
            {
                return multiDictType;
            }
        }
        
        #endregion

        #region publicメソッド

        /// <summary>
        /// 
        /// </summary>
        public static void GetIniInfo(string file)
        {
            //ファイルが存在かどうかをチェック
            if (File.Exists(file))
            {
                #region STATION
                GetStationFromFile(file);
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateDefaultIniFile(string file)
        {
            #region STATION
            UpdateStationToFile(file);
            #endregion
        }

        /// <summary>
        /// ini設定ファイルを読込
        /// </summary>
        public static void GetStationFromFile(string file)
        {
            //iniフォルダ
            iniFileDir = Path.GetDirectoryName(file);

            //アプリのインストールフォルダパス。
            string install_Path = GetIniFileString("DATA_DRIVE", "InstallPath", file);

            // InstallPathの確認
            if (Directory.Exists(install_Path) == false)
            {
                install_Path = Directory.GetParent(iniFileDir).FullName;
                WriteIniFileString("DATA_DRIVE", "InstallPath", install_Path, file);
            }

            //臨時フォルダ
            TempPath = GetIniFileString("DATA_DRIVE", "TempPath", file);
            if (string.IsNullOrEmpty(TempPath) || Directory.Exists(TempPath) == false)
            {
                TempPath = Path.GetTempPath();
            }
            else
            {
                if (TempPath.Substring(TempPath.Length - 1, 1) != "\\" && TempPath.Substring(TempPath.Length - 1, 1) != "/")
                {
                    TempPath = TempPath + "\\";
                }
            }

            //CanOutputLog
            canOutputLog = Utility.ToInt(GetIniFileString("MAINTE", "CanOutputLog", file)) == 0 ? false : true;

            //MultilingualDictionaryType
            string multiDictTypeValue = GetIniFileString("MAINTE", "MultilingualDictionaryType", file);

            foreach (MultilingualDictionaryType type in Enum.GetValues(typeof(MultilingualDictionaryType)))
            {
                if (multiDictTypeValue == EnumHelper.GetDescription(type))
                {
                    multiDictType = type;
                    break;
                }
            }

            DictionaryInfo.InitMultilingualDictionaryForMessage();


            //Device
            string deviceIP = GetIniFileString("MAINTE", "DeviceIP", file);
            int devicePort = Utility.ToInt(GetIniFileString("MAINTE", "DevicePort", file));
            string deviceUser = GetIniFileString("MAINTE", "DeviceUser", file);
            string devicePassword = GetIniFileString("MAINTE", "DevicePassword", file);
            devicePassword = Crypt.DecryptDES(devicePassword, Constants.CRYPT_KEY_FOR_CLIENT_LOGIN);
            int deviceChannel = Utility.ToInt(GetIniFileString("MAINTE", "DeviceChannel", file));
            

            string captureTypeValue = GetIniFileString("MAINTE", "CaptureType", file);

            Capture = PhotoType.JPG;

            foreach (PhotoType type in Enum.GetValues(typeof(PhotoType)))
            {
                if (captureTypeValue == EnumHelper.GetDescription(type)
                     || captureTypeValue==type.ToString())
                {
                    Capture = type;
                    break;
                }
            }
            
        }

        /// <summary>
        /// INI書き込み
        /// </summary>
        public static void UpdateStationToFile(string file)
        {
            bool isNewFile = false;
            try
            {
                string path = Path.GetDirectoryName(file);
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                if (string.IsNullOrEmpty(file) == false && File.Exists(file) == false)
                {
                    FileStream fs;
                    fs = File.Create(file);
                    fs.Close();
                    isNewFile = true;
                }

                //アプリのインストールフォルダパス	
                WriteIniFileString("DATA_DRIVE", "InstallPath", path, file);

                //臨時フォルダ
                WriteIniFileString("DATA_DRIVE", "TempPath", TempPath, file);


                //logが出力することが
                WriteIniFileString("MAINTE", "CanOutputLog", CanOutputLog ? "1" : "0", file);


                WriteIniFileString("MAINTE", "MultilingualDictionaryType", EnumHelper.GetDescription(MultiDictType), file);

                if (isNewFile)
                {
                    WriteIniFileNotesString("MAINTE", "MultilingualDictionaryType", "表示言語選択 {JP, EN}", file);
                }


                if (isNewFile)
                {
                    WriteIniFileNotesString("MAINTE", "DeviceIP", "設備設置", file);
                }


                WriteIniFileString("MAINTE", "CaptureType", EnumHelper.GetDescription(Capture), file);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        #region INI汎用読み書き

        /// <summary>
        /// INIファイルに指定のキーを持つ値を書き込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string file, string section, string key, string value)
        {
            WriteIniFileString(section, key, value, file);
        }

        /// <summary>
        /// INIファイルに指定のキーを持つ整数値を書き込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string file, string section, string key, int value)
        {
            Write(file, section, key, value.ToString());
        }

        /// <summary>
        /// INIファイルに指定のキーを持つbool値を書き込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string file, string section, string key, bool value)
        {
            Write(file, section, key, value.ToString());
        }

        /// <summary>
        /// INIファイルから指定のキーを持つ値を読み込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Read(string file, string section, string key)
        {
            string reValue = GetIniFileString(section, key, file);
            return reValue;
        }

        /// <summary>
        /// INIファイルから指定のキーを持つ整数値を読み込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int ReadInt(string file, string section, string key, int defalt)
        {
            int n = 0;
            string val = Read(file, section, key);
            if (int.TryParse(val, out n))
            {
                return n;
            }
            return defalt;
        }

        /// <summary>
        /// INIファイルから指定のキーを持つbool値を読み込みます。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ReadBool(string file, string section, string key, bool defalt)
        {
            bool b = false;
            string val = Read(file, section, key);
            if (bool.TryParse(val, out b))
            {
                return b;
            }
            return defalt;

        }

        #endregion

        /// <summary>
        /// INIファイルが書き換え可能かを確認します。
        /// </summary>
        public static void CheckIniFileWritable(string file)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(file, FileMode.Open, FileAccess.ReadWrite);
                fs.Close();
            }
            catch
            {
                throw new Exception(DictionaryInfo.GetMesssage("Message.E010"));
            }

        }
        #endregion

        #region privateメソッド


        //ini 書き
        private static void WriteIniFileString(string sectionName, string keyName, string defaultValue, string fileName)
        {
            string strSection = "[" + sectionName + "]";
            List<string> rowStringList = new List<string>();

            string reValue = defaultValue;
            try
            {
                //1.ファイルが存在しない場合は作成する。
                if (!File.Exists(fileName))
                {
                    FileStream fileStream = File.Create(fileName);
                    fileStream.Close();
                }
                //読み取りINIファイル。
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName, System.Text.Encoding.Default);
                string strLine = null;
                while ((strLine = streamReader.ReadLine()) != null)
                {
                    rowStringList.Add(strLine);
                }
                streamReader.Close();


                string[] rows = rowStringList.ToArray();

                string oneRow;
                int rowIndex = 0;
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //見つからず/ない
                    if (strSection != oneRow)
                    {
                        continue;
                    }
                    //見つける
                    break;
                }


                //2.見つからない対応のsectionを作成し、創建
                if (rowIndex >= rows.Length)
                {
                    AppendToFile(fileName, new string[] { strSection, keyName + "=" + defaultValue });
                    return;
                }

                //見つけsection
                rowIndex += 1; //スキップsection
                //属性を探す
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //越界
                    if ('[' == oneRow[0])
                    {
                        break;
                    }

                    int spcIndex = oneRow.IndexOf('=');
                    if (spcIndex <= 0)
                    {
                        continue;
                    }
                    string keyLeft = oneRow.Substring(0, spcIndex);
                    string valueRight = oneRow.Substring(spcIndex + 1);
                    //属性を見つける
                    if (keyLeft == keyName)
                    {
                        rows[rowIndex] = keyName + "=" + defaultValue;
                        WriteArray(fileName, rows);
                        return;
                    }
                }

                WriteArray(fileName, rows, rowIndex, keyName + "=" + defaultValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // 支持の言語
        //ini 書き注釈
        private static void WriteIniFileNotesString(string sectionName, string keyName, string notes, string fileName)
        {
            string strSection = "[" + sectionName + "]";
            List<string> rowStringList = new List<string>();

            string notesValue = string.Format(";{0}", notes);
            try
            {
                //1.ファイルが存在しない場合は作成する。
                if (!File.Exists(fileName))
                {
                    FileStream fileStream = File.Create(fileName);
                    fileStream.Close();
                }
                //読み取りINIファイル。
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName, System.Text.Encoding.Default);
                string strLine = null;
                while ((strLine = streamReader.ReadLine()) != null)
                {
                    rowStringList.Add(strLine);
                }
                streamReader.Close();


                string[] rows = rowStringList.ToArray();

                string oneRow;
                int rowIndex = 0;
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //見つからず/ない
                    if (strSection != oneRow)
                    {
                        continue;
                    }
                    //見つける
                    break;
                }


                //2.見つからない対応のsectionを作成し、創建
                if (rowIndex >= rows.Length)
                {
                    AppendToFile(fileName, new string[] { notesValue });
                    return;
                }

                //見つけsection
                rowIndex += 1; //スキップsection
                //属性を探す
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //越界
                    if ('[' == oneRow[0])
                    {
                        break;
                    }

                    int spcIndex = oneRow.IndexOf('=');
                    if (spcIndex <= 0)
                    {
                        continue;
                    }
                    string keyLeft = oneRow.Substring(0, spcIndex);
                    string valueRight = oneRow.Substring(spcIndex + 1);
                    //属性を見つける
                    if (keyLeft == keyName)
                    {
                        rows[rowIndex] = notesValue + "\r\n" + rows[rowIndex];
                        WriteArray(fileName, rows);
                        return;
                    }
                }

                WriteArray(fileName, rows, rowIndex, notesValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //ini 読み
        public static string GetIniFileString(string sectionName, string keyName, string fileName)
        {
            string strSection = "[" + sectionName + "]";
            List<string> rowStringList = new List<string>();

            string reValue = string.Empty;
            try
            {
                //1.ファイルが存在しない場合は作成する。
                if (!File.Exists(fileName))
                {
                    FileStream fileStream = File.Create(fileName);
                    fileStream.Close();
                }
                //読み取りINIファイル。
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName, System.Text.Encoding.Default);
                string strLine = null;
                while ((strLine = streamReader.ReadLine()) != null)
                {
                    rowStringList.Add(strLine);
                }
                streamReader.Close();


                string[] rows = rowStringList.ToArray();

                string oneRow;
                int rowIndex = 0;
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //見つからず/ない
                    if (strSection != oneRow)
                    {
                        continue;
                    }
                    //見つける
                    break;
                }


                //2.見つからない
                if (rowIndex >= rows.Length)
                {
                    return string.Empty;
                }
                //見つけsection
                rowIndex += 1; //スキップsection
                //属性を探す
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //越界
                    if ('[' == oneRow[0])
                    {
                        break;
                    }

                    int spcIndex = oneRow.IndexOf('=');
                    if (spcIndex <= 0)
                    {
                        continue;
                    }
                    string keyLeft = oneRow.Substring(0, spcIndex);
                    string valueRight = oneRow.Substring(spcIndex + 1);
                    //3.属性を見つけるには値がない
                    if (keyLeft == keyName)
                    {
                        //見つける                      
                        reValue = valueRight.Trim();
                        break;
                    }
                }
                return reValue;
            }
            catch
            {
                return string.Empty;
            }
        }


        //ini 読み
        public static bool CheckIniFileString(string sectionName, string keyName, string fileName)
        {
            string strSection = "[" + sectionName + "]";
            List<string> rowStringList = new List<string>();

            string reValue = string.Empty;
            try
            {
                //1.ファイルが存在しない場合は作成する。
                if (!File.Exists(fileName))
                {
                    FileStream fileStream = File.Create(fileName);
                    fileStream.Close();
                }
                //読み取りINIファイル。
                System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName, System.Text.Encoding.Default);
                string strLine = null;
                while ((strLine = streamReader.ReadLine()) != null)
                {
                    rowStringList.Add(strLine);
                }
                streamReader.Close();


                string[] rows = rowStringList.ToArray();

                string oneRow;
                int rowIndex = 0;
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //見つからず/ない
                    if (strSection != oneRow)
                    {
                        continue;
                    }
                    //見つける
                    break;
                }


                //2.見つからない
                if (rowIndex >= rows.Length)
                {
                    return false;
                }
                //見つけsection
                rowIndex += 1; //スキップsection
                //属性を探す
                for (; rowIndex < rows.Length; rowIndex++)
                {
                    oneRow = rows[rowIndex].Trim();
                    //空行
                    if (0 == oneRow.Length)
                    {
                        continue;
                    }
                    //この行は注釈する
                    if (';' == oneRow[0])
                    {
                        continue;
                    }
                    //越界
                    if ('[' == oneRow[0])
                    {
                        break;
                    }

                    int spcIndex = oneRow.IndexOf('=');
                    if (spcIndex <= 0)
                    {
                        continue;
                    }
                    string keyLeft = oneRow.Substring(0, spcIndex);
                    string valueRight = oneRow.Substring(spcIndex + 1);
                    //3.属性を見つけるには値がない
                    if (keyLeft == keyName)
                    {
                        //見つける                      
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


        private static void AppendToFile(string strPath, string[] strContent)
        {
            FileStream fs = new FileStream(strPath, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fs, new System.Text.UTF8Encoding(true));
            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            for (int i = 0; i < strContent.Length; i++)
            {
                if (strContent[i].Trim() == "/r/n")
                    continue;
                streamWriter.WriteLine(strContent[i].Trim());
            }
            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }
        private static void WriteArray(string strPath, string[] strContent)
        {
            FileStream fs = new FileStream(strPath, FileMode.Truncate);
            StreamWriter streamWriter = new StreamWriter(fs, new System.Text.UTF8Encoding(true));
            streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < strContent.Length; i++)
            {
                if (strContent[i].Trim() == "/r/n")
                    continue;
                streamWriter.WriteLine(strContent[i].Trim());
            }
            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }

        private static void WriteArray(string strPath, string[] strContent, int strIndex, string insertString)
        {
            FileStream fs = new FileStream(strPath, FileMode.Truncate);
            StreamWriter streamWriter = new StreamWriter(fs, new System.Text.UTF8Encoding(true));
            streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < strContent.Length; i++)
            {
                if (strContent[i].Trim() == "/r/n")
                {
                    continue;
                }
                if (i == strIndex)
                {
                    streamWriter.WriteLine(insertString);
                }
                streamWriter.WriteLine(strContent[i].Trim());
            }

            if (strIndex >= strContent.Length)
            {
                streamWriter.WriteLine(insertString);
            }

            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }
        #endregion
    }
    
}