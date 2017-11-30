using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilluminateApp
{

    #region 共通定数
    /// <summary>
    /// 共通定数(Const)定義クラス
    /// </summary>
    /// <remarks>
    /// ※注意
    /// 関連する項目がある場合、
    /// Enum/ユーザー定義列挙型にすること。
    /// </remarks>
    public class Constants
    {
        #region Iniファイル
        /// <summary>
        /// 設定ファイルの名
        /// </summary>
        public const string INI_NAME = "ToilluminateClient.ini";

        /// <summary>
        /// アプリケーション名
        /// </summary>
        public const string APPLICATION = "ToilluminateApp";

        /// <summary>
        /// ログフォルダ
        /// </summary>
        public const string LogDir = "Log";

        /// <summary>
        /// 言語辞書のフォルダ
        /// </summary>
        public const string MultiDictDir = "MultilingualDictionary";

        /// <summary>
        /// バージョン情報
        /// </summary>
        public const string Version = "1.0.0";

        #endregion


        #region Output

        /// <summary>
        /// 分割線
        /// </summary>
        public const string VAL_Dividing = "|";
        public const char SPACE_CHAR = (char)11;
        public const char SPACE_CHAR_UNDERLINE = '_';

        /// <summary>
        /// 毎回ファイル だけ 3K 読み取り 最大数
        /// </summary>
        public static readonly int READ_FILE_BYTE_MAX_LENGTH = 3096;
        #endregion Output


        #region パスワード
        public const int PASSWORD_LENGTH_MIN = 7;

        /// <summary>
        /// クライアントの登録の暗号鍵
        /// </summary>
        public const string CRYPT_KEY_FOR_CLIENT_LOGIN = "1710";

        #endregion

        #region API Function

        /// <summary>
        /// 播放器发送心跳包
        /// </summary>
        public const string API_PLAYERMASTERS_SEND = "api/PlayerMasters/SendHeartBeatPkg/{0}";

        /// <summary>
        /// 得到所有播放器的playlist调用
        /// </summary>
        public const string API_PLAYLISTMASTERS_GET_LIST = "api/PlayListMasters/GetTotalPlayListByPlayerID/{0}";

        /// <summary>
        /// 得到播放器状态数据
        /// </summary>
        public const string API_PLAYERMASTERS_GET_STATUS = "api/PlayerMasters/GetPlayerStatusReportData";


        #endregion

        #region "Default Dictionary"

        #region "Default Dictionary Message"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Message = new List<List<string>> { };

        #endregion

        #region "Default Dictionary Client"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Client = new List<List<string>> { };


        #endregion

        #region "Default Dictionary Enum"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Enum = new List<List<string>> { };

        #endregion

        #endregion

    }
    #endregion
}
