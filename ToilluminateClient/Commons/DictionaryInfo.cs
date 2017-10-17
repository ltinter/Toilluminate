/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：言語辞書
 * 作成日・作成者：2017/05/26  張鵬
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ToilluminateClient
{

    #region " 言語辞書 "

    #region メッセージボックスのメッセージ文言
    /// <summary>
    /// メッセージボックスのメッセージ文言を提供します。
    /// </summary>
    public static class DictionaryInfo
    {

        public static string GetMesssage(string msgKey)
        {
            return GetDictValueForDictKeyForMessage(string.Format("Message.{0}", msgKey));
        }

        #region "  言語辞書ロード "

        /// <summary>
        /// 言語辞書ロード
        /// </summary>
        /// <param name="multilingualDictionaryList">言語辞書</param>
        /// <param name="dictFile">言語辞書ファイル</param>
        /// <param name="dictType">言語辞書種別</param>
        public static void InitMultilingualDictionary(Dictionary<string, MultilingualDictionary> multilingualDictionaryList, string dictFile, MultilingualDictionaryType dictType)
        {
            try
            {
                if (File.Exists(dictFile) == false)
                {
                    return;
                }
                multilingualDictionaryList.Clear();

                string dictTypeDescription = EnumHelper.GetDescription(dictType);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dictFile);
                XmlElement root = xmlDoc.DocumentElement;

                XmlNodeList dictionaryNodeList = root.SelectNodes("Dictionary");

                foreach (XmlNode dictionaryNode in dictionaryNodeList)
                {
                    string dictKey = dictionaryNode.Attributes["Key"].Value;
                    string dictValue = dictionaryNode.InnerText.Trim().Replace("\\r", "\r").Replace("\\n", "\n");

                    string multiDictKey = string.Format("{0}_{1}", dictTypeDescription, dictKey);
                    if (multilingualDictionaryList.ContainsKey(multiDictKey) == false)
                    {
                        multilingualDictionaryList.Add(multiDictKey, new MultilingualDictionary(multiDictKey, dictValue));
                    }
                    else
                    {
                        ////後は準
                        //multilingualDictionaryList.Remove(multiDictKey);
                        //multilingualDictionaryList.Add(multiDictKey, new MultilingualDictionary(multiDictKey, dictValue));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 言語辞書ロード
        /// </summary>
        /// <param name="multilingualDictionaryList">言語辞書</param>
        /// <param name="dictList">言語辞書List</param>
        /// <param name="dictType">言語辞書種別</param>
        public static void InitMultilingualDictionary(Dictionary<string, MultilingualDictionary> multilingualDictionaryList, List<List<string>> dictList, MultilingualDictionaryType dictType)
        {
            try
            {

                multilingualDictionaryList.Clear();

                string dictTypeDescription = EnumHelper.GetDescription(dictType);


                foreach (List<string> dictionaryNode in dictList)
                {
                    string dictKey = dictionaryNode[0];
                    string dictValue = dictionaryNode[1];

                    string multiDictKey = string.Format("{0}_{1}", dictTypeDescription, dictKey);
                    if (multilingualDictionaryList.ContainsKey(multiDictKey) == false)
                    {
                        multilingualDictionaryList.Add(multiDictKey, new MultilingualDictionary(multiDictKey, dictValue));
                    }
                    else
                    {
                        ////後は準
                        //multilingualDictionaryList.Remove(multiDictKey);
                        //multilingualDictionaryList.Add(multiDictKey, new MultilingualDictionary(multiDictKey, dictValue));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 言語辞書取得
        /// </summary>
        /// <param name="multilingualDictionaryList">言語辞書</param>
        /// <param name="dictKey">言語辞書key</param>
        /// <param name="dictType">言語辞書種別</param>
        /// <returns></returns>
        public static string GetDictValueForDictKey(Dictionary<string, MultilingualDictionary> multilingualDictionaryList, string dictKey, MultilingualDictionaryType dictType)
        {
            try
            {
                string reDictionary = string.Empty;
                string dictTypeDescription = EnumHelper.GetDescription(dictType);
                string multiDictKey = string.Format("{0}_{1}", dictTypeDescription, dictKey);
                if (multilingualDictionaryList.ContainsKey(multiDictKey))
                {
                    reDictionary = (multilingualDictionaryList[multiDictKey] as MultilingualDictionary).DictValue;
                }
                return reDictionary;
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 言語辞書ロード
        /// </summary>
        /// <returns></returns>
        public static void InitMultilingualDictionaryForMessage()
        {
            //default
            InitMultilingualDictionary(IniFileInfo.defaultMessageMultilingualDictionaryList, Constants.Default_Dictionary_Message, IniFileInfo.MultiDictType);


            //言語辞書ファイル
            string dictFile = string.Empty;
            string dictTypeDescription = EnumHelper.GetDescription(IniFileInfo.MultiDictType);
            dictFile = Utility.GetFullFileName(Utility.GetFullFileName(IniFileInfo.IniFileDir, Constants.MultiDictDir), string.Format("MessageMultilingualDictionary_{0}.xml", dictTypeDescription));

            if (File.Exists(dictFile))
            {
                InitMultilingualDictionary(IniFileInfo.messageMultilingualDictionaryList, dictFile, IniFileInfo.MultiDictType);
            }

            InitMultilingualDictionaryForEnmu();
        }
        public static void InitMultilingualDictionaryForClient()
        {
            //default
            InitMultilingualDictionary(IniFileInfo.defaultClientMultilingualDictionaryList, Constants.Default_Dictionary_Client, IniFileInfo.MultiDictType);

            //言語辞書ファイル
            string dictFile = string.Empty;
            string dictTypeDescription = EnumHelper.GetDescription(IniFileInfo.MultiDictType);
            dictFile = Utility.GetFullFileName(Utility.GetFullFileName(IniFileInfo.IniFileDir, Constants.MultiDictDir), string.Format("ClientMultilingualDictionary_{0}.xml", dictTypeDescription));

            if (File.Exists(dictFile))
            {
                InitMultilingualDictionary(IniFileInfo.clientMultilingualDictionaryList, dictFile, IniFileInfo.MultiDictType);
            }
        }
        /// <summary>
        /// 言語辞書ロード
        /// </summary>
        /// <returns></returns>
        public static void InitMultilingualDictionaryForEnmu()
        {
            //default
            InitMultilingualDictionary(IniFileInfo.defaultEnumMultilingualDictionaryList, Constants.Default_Dictionary_Enum, IniFileInfo.MultiDictType);

            //言語辞書ファイル
            string dictFile = string.Empty;
            string dictTypeDescription = EnumHelper.GetDescription(IniFileInfo.MultiDictType);
            dictFile = Utility.GetFullFileName(Utility.GetFullFileName(IniFileInfo.IniFileDir, Constants.MultiDictDir), string.Format("EnmuMultilingualDictionary_{0}.xml", dictTypeDescription));

            if (File.Exists(dictFile))
            {
                InitMultilingualDictionary(IniFileInfo.enumMultilingualDictionaryList, dictFile, IniFileInfo.MultiDictType);
            }
        }


        /// <summary>
        /// 言語辞書取得
        /// </summary>
        /// <param name="dictKey">言語辞書key</param>
        /// <returns></returns>
        public static string GetDictValueForDictKeyForClient(string dictKey)
        {
            string reValue = GetDictValueForDictKey(IniFileInfo.clientMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            if (string.IsNullOrEmpty(reValue))
            {
                reValue = GetDictValueForDictKey(IniFileInfo.defaultClientMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            }
            return reValue;
        }
        public static string GetDictValueForDictKeyForMessage(string dictKey)
        {
            string reValue = GetDictValueForDictKey(IniFileInfo.messageMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            if (string.IsNullOrEmpty(reValue))
            {
                reValue = GetDictValueForDictKey(IniFileInfo.defaultMessageMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            }
            return reValue;
        }
        public static string GetDictValueForDictKeyForEnum(string dictKey)
        {
            string reValue = GetDictValueForDictKey(IniFileInfo.enumMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            if (string.IsNullOrEmpty(reValue))
            {
                reValue = GetDictValueForDictKey(IniFileInfo.defaultEnumMultilingualDictionaryList, dictKey, IniFileInfo.MultiDictType);
            }
            return reValue;
        }

        #endregion
    }
    #endregion

    #region 言語辞書種別
    /// <summary>
    /// 言語辞書種別
    /// </summary>
    public enum MultilingualDictionaryType : int
    {
        /// <summary>
        /// 日本語
        /// </summary>
        [EnumDescription("JP")]
        Japanese = 0,
        /// <summary>
        /// 英語
        /// </summary>
        [EnumDescription("EN")]
        English = 1,
    }


    #endregion

    #region 言語辞書対象
    public class MultilingualDictionary
    {
        private string dictKey = string.Empty;

        private string dictValue = string.Empty;
        public MultilingualDictionary(string key, string value)
        {
            dictKey = key;
            dictValue = value;
        }

        public string DictKey
        {
            get
            {
                return dictValue;
            }
        }

        public string DictValue
        {
            get
            {
                return dictValue;
            }
        }

        public override string ToString()
        {
            return dictValue;
        }
    }
    #endregion


    #region "枚挙に項目をラベル  枚挙に戻る文字列"

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description { get { return description; } }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            return GetDescription(value, IniFileInfo.MultiDictType);
        }
        public static string GetDescription(Enum value, MultilingualDictionaryType dictType)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (dictType == MultilingualDictionaryType.Japanese)
            {
                string description = value.ToString();
                var fieldInfo = value.GetType().GetField(description);
                var attributes =
                    (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }
                return description;
            }
            else
            {
                string dictTypeDescription = GetDescription(dictType);
                string dictTypeKey = string.Format("{0}_Enum.{1}.{2}", dictTypeDescription, value.GetType().ToString(), value.ToString());
                return DictionaryInfo.GetDictValueForDictKeyForEnum(dictTypeKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Enum GetEnumForName(string enumName, Type enumType)
        {
            if (string.IsNullOrEmpty(enumName))
            {
                return null;
            }

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                if (enumName == enumValue.ToString())
                {
                    return enumValue;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Enum GetEnumForDescription(string enumDescription, Type enumType)
        {

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                if (enumDescription == GetDescription(enumValue))
                {
                    return enumValue;
                }
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string[] GetAllEnumDescription(Type enumType)
        {
            List<string> reList = new List<string>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                reList.Add(GetDescription(enumValue));
            }

            return reList.ToArray();
        }

        public static string[] GetAllEnumName(Type enumType)
        {
            List<string> reList = new List<string>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                reList.Add(enumValue.ToString());
            }

            return reList.ToArray();
        }
        public static int[] GetAllEnumValue(Type enumType)
        {
            List<int> reList = new List<int>();

            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                reList.Add(enumValue.GetHashCode());
            }

            return reList.ToArray();
        }
    }
    #endregion

    #endregion

}

