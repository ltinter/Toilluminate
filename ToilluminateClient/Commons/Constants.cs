/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：共通定数 共通列挙型(Enum/ユーザー定義クラス)
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
        public const string APPLICATION = "ToilluminateClient";
        
        /// <summary>
        /// エクスポートフォルダ
        /// </summary>
        public const string OutputDir = "OutputDir";
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

        #region "Default Dictionary"

        #region "Default Dictionary Message"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Message = new List<List<string>> {
                                            new List<string> { "Device_IP", "設備IP" }
                                          , new List<string> { "Device_Port", "設備ポート" }
                                          , new List<string> { "User_Name", "ユーザー名" }
                                          , new List<string> { "Password", "パスワード" }
                                          , new List<string> { "Channel", "チャネル" }
                                          , new List<string> { "Preview", "プレビュー" }
                                          , new List<string> { "Capture", "抓图" }
                                          , new List<string> { "Capture_BMP", "BMP抓图" }
                                          , new List<string> { "Capture_JPEG", "JPEG抓图" }
                                          , new List<string> { "BMP", "BMP" }
                                          , new List<string> { "JPG", "JPEG" }
                                          , new List<string> { "Record", "レコード" }
                                          , new List<string> { "PTZ", "雲制御" }
                                          , new List<string> { "PTZ_Speed", "雲速度" }
                                          , new List<string> { "Login", "ログイン" }
                                          , new List<string> { "Logout", "ログアウト" }
                                          , new List<string> { "OK", "OK" }
                                          , new List<string> { "Cancel", "キャンセル" }
                                          , new List<string> { "Settings", "設置" }
                                          , new List<string> { "Register", "登記" }
                                          , new List<string> { "FaceRecognition", "顔認識" }
                                          , new List<string> { "Video", "ビデオ" }
                                          , new List<string> { "Alarm", "アラーム" }
                                          , new List<string> { "State", "状態" }
                                          , new List<string> { "Refresh", "リフレッシュ" }
                                          , new List<string> { "List", "リスト" }
                                          , new List<string> { "Stream", "ストリーム" }
                                          , new List<string> { "Factory", "工場" }
                                          , new List<string> { "Vender", "ベンダー" }                                          
                                          , new List<string> { "Protocol", "プロトコル" }
                                          , new List<string> { "Listen", "監視" }
                                          , new List<string> { "Guard", "警備" }
                                          , new List<string> { "Alert", "アラーム" }
                                          , new List<string> { "BirthDate", "誕生日" }
                                          , new List<string> { "BornTime", "誕生日" }
                                          , new List<string> { "Age", "歳" }
                                          , new List<string> { "Name", "名前" }
                                          , new List<string> { "Sex", "性別" }
                                          , new List<string> { "CertificateType", "証明書種別" }
                                          , new List<string> { "CertificateNumber", "証明書番号" }
                                          , new List<string> { "CertType", "証明書種別" }
                                          , new List<string> { "CertNumber", "証明書番号" }
                                          , new List<string> { "RegID", "番号" }
                                          , new List<string> { "RegNumber", "No." }
                                          , new List<string> { "Add", "追加" }
                                          , new List<string> { "Edit", "編集" }
                                          , new List<string> { "Delete", "削除" }
                                          , new List<string> { "Search", "検索" }
                                          , new List<string> { "Query", "検索" }
                                          , new List<string> { "Success!", "アップロード成功!" }
                                          , new List<string> { "FaceList", "顔写真集" }
                                          , new List<string> { "picURL", "写真url" }
                                          , new List<string> { "PID", "写真id" }
                                          , new List<string> { "FDID", "カード番号" }
                                          , new List<string> { "Select Picture File", "写真ファイルの選択" }
                                          , new List<string> { "Add Picture", "写真を追加" }
                                          , new List<string> { "Picture File", "写真ファイル" }
                                          , new List<string> { "Picture File", "写真ファイル" }
                                            };

        #endregion

        #region "Default Dictionary Client"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Client = new List<List<string>> {
                                            new List<string> { "Device_IP", "設備IP" }
                                          , new List<string> { "Device_Port", "設備ポート" }
                                          , new List<string> { "User_Name", "ユーザー名" }
                                          , new List<string> { "Password", "パスワード" }
                                          , new List<string> { "Channel", "チャネル" }
                                          , new List<string> { "Preview", "プレビュー" }
                                          , new List<string> { "Capture", "抓图" }
                                          , new List<string> { "Capture_BMP", "BMP抓图" }
                                          , new List<string> { "Capture_JPEG", "JPEG抓图" }
                                          , new List<string> { "BMP", "BMP" }
                                          , new List<string> { "JPG", "JPEG" }
                                          , new List<string> { "Record", "レコード" }
                                          , new List<string> { "PTZ", "雲制御" }
                                          , new List<string> { "PTZ_Speed", "雲速度" }
                                          , new List<string> { "Login", "ログイン" }
                                          , new List<string> { "Logout", "ログアウト" }
                                          , new List<string> { "OK", "OK" }
                                          , new List<string> { "Cancel", "キャンセル" }
                                          , new List<string> { "Settings", "設置" }
                                          , new List<string> { "Register", "登記" }
                                          , new List<string> { "FaceRecognition", "顔認識" }
                                          , new List<string> { "Video", "ビデオ" }
                                          , new List<string> { "Alarm", "アラーム" }
                                          , new List<string> { "State", "状態" }
                                          , new List<string> { "Refresh", "リフレッシュ" }
                                          , new List<string> { "List", "リスト" }
                                          , new List<string> { "Stream", "ストリーム" }
                                          , new List<string> { "Factory", "工場" }
                                          , new List<string> { "Vender", "ベンダー" }
                                          , new List<string> { "Protocol", "プロトコル" }
                                          , new List<string> { "Listen", "監視" }
                                          , new List<string> { "Guard", "警備" }
                                          , new List<string> { "Info", "情報" }
                                          , new List<string> { "Image", "写真" }
                                          , new List<string> { "Alert", "アラーム" }
                                          , new List<string> { "BirthDate", "誕生日" }
                                          , new List<string> { "BornTime", "誕生日" }
                                          , new List<string> { "RecordDate", "登記日" }
                                          , new List<string> { "Age", "歳" }
                                          , new List<string> { "Name", "名前" }
                                          , new List<string> { "Sex", "性別" }
                                          , new List<string> { "CertificateType", "証明書種別" }
                                          , new List<string> { "CertificateNumber", "証明書番号" }
                                          , new List<string> { "CertType", "証明書種別" }
                                          , new List<string> { "CertNumber", "証明書番号" }
                                          , new List<string> { "RegID", "番号" }
                                          , new List<string> { "RegNumber", "No." }
                                          , new List<string> { "Add", "追加" }
                                          , new List<string> { "Edit", "編集" }
                                          , new List<string> { "Delete", "削除" }
                                          , new List<string> { "Search", "検索" }
                                          , new List<string> { "Query", "検索" }
                                          , new List<string> { "Success!", "アップロード成功!" }
                                          , new List<string> { "FaceList", "顔写真集" }
                                          , new List<string> { "picURL", "写真url" }
                                          , new List<string> { "PID", "写真id" }
                                          , new List<string> { "FDID", "カード番号" }
                                          , new List<string> { "Select Picture File", "写真ファイルの選択" }
                                          , new List<string> { "Add Picture", "写真を追加" }
                                          , new List<string> { "Picture File", "写真ファイル" }
                                          , new List<string> { "First", "|<<" }
                                          , new List<string> { "Previous", "<<" }
                                          , new List<string> { "Next", ">>" }
                                          , new List<string> { "Last", ">>|" }
                                          , new List<string> { "FaceSearch", "顔写真集検索" }

                                            };


        #endregion

        #region "Default Dictionary Enum"
        /// <summary>
        /// Message
        /// </summary>
        public static List<List<string>> Default_Dictionary_Enum = new List<List<string>> {
                                            new List<string> { "male", "Man" }
                                          , new List<string> { "female", "Woman" } };

        #endregion
        
        #endregion

    }
    #endregion

    #region タイムアウト時間
    /// <summary>
    /// タイムアウト時間
    /// </summary>
    public static class TimeOut
    {
        // データコール
        public static int Data_Call = 5;
        // データ更新
        public static int Data_Execute = 60;
        // プロシージャ
        public static int Data_StoredProcedure = 5;
    }
    #endregion

    #region 操作モード
    /// <summary>
    /// 操作モード
    /// </summary>
    public enum OperationMode : int
    {
        /// <summary>
        /// 既存データを追加 
        /// </summary>
        New = 0,

        /// <summary>
        /// 改修 - 既存データを削除してから保存
        /// </summary>
        Edit = 1,

        /// <summary>
        /// 参照 - 既存データを削除してから保存
        /// </summary>
        Reference = 2,
    }
    #endregion

    #region ComboItem対象
    public class ComboItem
    {
        public string Text = "";

        public string Value = "";
        public ComboItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
    #endregion
        
    #region 属性は存在種別
    /// <summary>
    /// 属性は存在種別
    /// </summary>
    public enum PropertyFlagType : int
    {
        /// <summary>
        /// 存在しない / 無効 / 無し / 削除しない
        /// </summary>
        [EnumDescription("No")]
        False = 0,
        /// <summary>
        /// 存在 / 有効 / 有り / 削除済み
        /// </summary> 
        [EnumDescription("Yes")]
        True = 1,
    }
    #endregion

    #region 認証結果列挙型
    /// <summary>
    /// 認証結果列挙型
    /// </summary>
    public enum LoginAuthenticateResult
    {
        /// <summary>
        /// 認証成功
        /// </summary>
        Succeed,
        /// <summary>
        /// パスワード期限切れ
        /// </summary>
        ExpirePassword,
        /// <summary>
        /// 認証失敗
        /// </summary>
        Failed,
        /// <summary>
        /// 認証失敗
        /// </summary>
        Error,
    }
    #endregion

    #region 写真列挙型
    /// <summary>
    /// 写真列挙型
    /// </summary>
    public enum PhotoType : int
    {
        /// <summary>
        /// JPEG
        /// </summary>
        [EnumDescription("jpg")]
        JPG = 0,
        /// <summary>
        /// BMP
        /// </summary>
        [EnumDescription("bmp")]
        BMP = 1,
    }
    #endregion

    #region 画像列挙型
    /// <summary>
    /// 画像列挙型
    /// </summary>
    public enum PictureType : int
    {
        /// <summary>
        /// Null
        /// </summary>
        [EnumDescription("")]
        Null = 0,
        /// <summary>
        /// JPEG
        /// </summary>
        [EnumDescription("jpg")]
        JPG = 1,
        /// <summary>
        /// BMP
        /// </summary>
        [EnumDescription("bmp")]
        BMP = 2,
        /// <summary>
        /// PNG
        /// </summary>
        [EnumDescription("png")]
        PNG = 3,
        /// <summary>
        /// SWF
        /// </summary>
        [EnumDescription("swf")]
        SWF = 4,
        /// <summary>
        /// GIF
        /// </summary>
        [EnumDescription("gif")]
        GIF = 5,
    }
    #endregion

    #region ビデオ列挙型
    /// <summary>
    /// ビデオ列挙型
    /// </summary>
    public enum VideoType : int
    {
        /// <summary>
        /// MP4
        /// </summary>
        [EnumDescription("mp4")]
        MP4 = 0,
    }
    #endregion

    #region 性別は種別
    /// <summary>
    /// 性別は種別
    /// </summary>
    public enum SexType : int
    {
        /// <summary>
        /// 男
        /// </summary>
        [EnumDescription("男")]
        male = 0, 
        /// <summary>
        /// 女
        /// </summary>
        [EnumDescription("女")]
        female = 1, 
    }
    #endregion

    #region ビデオ列挙型
    /// <summary>
    /// ビデオ列挙型
    /// </summary>
    public enum CertificateType : int
    {
        /// <summary>
        /// 軍人証
        /// </summary>
        [EnumDescription("軍人証")]
        officerID = 0,
        /// <summary>
        /// 身分証
        /// </summary>
        [EnumDescription("市民証")]
        ID = 1,
    }

    #endregion
}

