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

        public const string LabelNameHead = "lblMessage";
        
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
        // 下载
        public static int Upload = 5;
        // 提交
        public static int Commit = 60;
        // 执行
        public static int Execting = 5;
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

    #region 認証結果
    /// <summary>
    /// 認証結果列挙
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// 認証成功
        /// </summary>
        Succeed,
        /// <summary>
        /// 認証失敗
        /// </summary>
        Failed,
        /// <summary>
        /// 認証Err
        /// </summary>
        Error,
    }
    #endregion
    
    #region 填充模式
    /// <summary>
    /// 填充模式
    /// </summary>
    /// <remarks></remarks>
    public enum FillMode
    {
        /// <summary>
        /// 平铺
        /// </summary>
        [EnumDescription("平铺")]
        Fill = 0,
        /// <summary>
        /// 居中
        /// </summary>
        [EnumDescription("居中")]
        Center = 1,
        /// <summary>
        /// 缩放
        /// </summary>
        [EnumDescription("缩放")]
        Zoom = 2,
    }

    #endregion

    #region 图片显示模式

    /// <summary>
    /// 图片显示模式
    /// </summary>
    public enum ImageShowStyle
    {
        /// <summary>
        /// 随机
        /// </summary>
        Random = 0,
        /// <summary>
        /// 左右 覆盖
        /// </summary>
        LeftToRight = 1,
        /// <summary>
        /// 右左 覆盖
        /// </summary>
        RightToLeft = 2,
        /// <summary>
        /// 上下 覆盖
        /// </summary>
        TopToDown = 3,
        /// <summary>
        /// 下上 覆盖
        /// </summary>
        DownToTop = 4,
        /// <summary>
        /// 小大 扩散
        /// </summary>
        SmallToLarge = 5,
        /// <summary>
        /// 淡入效果
        /// </summary>
        Fade = 6,
        /// <summary>
        /// 左右 翻转
        /// </summary>
        Flip_LR = 7,
        /// <summary>
        /// 上下 翻转
        /// </summary>
        Flip_TD = 8,

        /// <summary>
        /// 
        /// </summary>
        Gradient = 9,
        /// <summary>
        /// 左右 对接
        /// </summary>
        Docking_LR = 10,
        /// <summary>
        /// 上下 对接
        /// </summary>
        Docking_TD = 11,
        /// <summary>
        /// 左上旋转
        /// </summary>
        Rotate = 12,
        /// <summary>
        /// 分块显示
        /// </summary>
        Block = 13,
        /// <summary>
        /// 马赛克效果
        /// </summary>
        Special = 14,

        /// <summary>
        /// 
        /// </summary>
        None = 99,
    }
    #endregion

    #region 特效模式
    /// <summary>
    /// 特效模式
    /// </summary>
    public enum BitmapSpecialStyle
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 上下反转
        /// </summary>
        TopDown = 1,
        /// <summary>
        /// 左右反转
        /// </summary>
        LeftRight = 2,
        /// <summary>
        /// 黑白剪影特效
        /// </summary>
        Sketch = 3,
        /// <summary>
        /// 
        /// </summary>
        //Docking_TD = 13,
        /// <summary>
        /// 
        /// </summary>
        //Docking_DT = 14,

    }
    #endregion


    #region 信息显示模式

    /// <summary>
    /// 信息显示模式
    /// </summary>
    public enum MessageShowStyle
    {
        /// <summary>
        /// 
        /// </summary>
        Down = 0,
        /// <summary>
        /// 
        /// </summary>
        Top = 1,
        /// <summary>
        /// 
        /// </summary>
        Center = 2,
        /// <summary>
        /// 
        /// </summary>
        Random = 99,
    }
    #endregion


    #region 视频显示模式

    /// <summary>
    /// 视频显示模式
    /// </summary>
    public enum ZoomOptionStyle
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Full = 1,
        /// <summary>
        /// 
        /// </summary>
        Full2 = 2,
    }
    #endregion
}

