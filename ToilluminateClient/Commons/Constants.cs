﻿/***********************************************************************
 * Copyright(c) 2017 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：FaceRecognitionClass
 * プログラム名称：共通定数 共通列挙型(Enum/ユーザー定義クラス)
 * 作成日・作成者：2017/05/26  張鵬
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// 設定ファイルの名
        /// </summary>
        public const string JSON_NAME = "ToilluminateClient.jsonstring";

        /// <summary>
        /// アプリケーション名
        /// </summary>
        public const string APPLICATION = "ToilluminateClient";

        /// <summary>
        /// エクスポートフォルダ
        /// </summary>
        public const string OutputDir = "OutputDir";


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


        #region Message 

        public static readonly string MESSAGE_FONT_Family = "poppins";
        public static readonly int MESSAGE_FONT_Size = 14;
        public static readonly Color MESSAGE_FONT_Color = Color.Red;

        #endregion Message

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
        public const string API_PLAYLISTMASTERS_GET_INFO = "api/PlayListMasters/GetTotalPlayListByPlayerIDForClient/{0}";

        /// <summary>
        /// 得到player调用
        /// </summary>
        public const string API_PLAYERMASTERS_GET_INFO = "api/PlayerMasters/{0}";


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


    #region 视频设备種別

    /// <summary>
    /// 视频设备種別
    /// </summary>
    public enum MediaDeivceType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("VideoLAN")]
        VLC = 0,
        /// <summary>
        /// 
        /// </summary>
        [EnumDescription("Wodows Media Play")]
        WMP = 1,
    }
    #endregion

    #region PlayApp

    #region play状态类型
    /// <summary>
    /// play状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum PlayListStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        Wait = 0,
        /// <summary>
        /// 播放
        /// </summary>
        [EnumDescription("放送")]
        Execute = 1,
        /// <summary>
        /// 末尾
        /// </summary>
        [EnumDescription("末尾")]
        Last = 2,
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 9,
    }

    #endregion

    #region 模板类型
    /// <summary>
    /// 模板类型
    /// </summary>
    /// <remarks></remarks>
    public enum TempleteItemType
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumDescription("")]
        None = -1,
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("写真")]
        Image = 0,
        /// <summary>
        /// 文字消息
        /// </summary>
        [EnumDescription("文字")]
        Message = 1,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("映像")]
        Media = 2,
        /// <summary>
        /// 商标
        /// </summary>
        [EnumDescription("商标")]
        Trademark = 3,

    }

    #endregion

    #region templete状态类型
    /// <summary>
    /// templete状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum TempleteStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        Wait = 0,
        /// <summary>
        /// 播放
        /// </summary>
        [EnumDescription("放送")]
        Execute = 1,
        /// <summary>
        /// 停止
        /// </summary>
        [EnumDescription("停止")]
        Stop = 9,
    }

    #endregion

    #endregion

    #region ShowApp



    #region 图片填充模式
    /// <summary>
    /// 图片填充模式
    /// </summary>
    /// <remarks></remarks>
    public enum FillOptionStyle
    {
        /// <summary>
        /// 原比例
        /// </summary>
        [EnumDescription("原比例")]
        None = 0,
        /// <summary>
        /// 拉伸
        /// </summary>
        [EnumDescription("拉伸")]
        Fill = 1,
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

    #region 移动方向类型
    /// <summary>
    ///移动方向类型
    /// </summary>
    /// <remarks></remarks>
    public enum MoveDirectionStyle
    {
        RightToLeft = 0,
        LeftToRight = 1,
        TopToDown = 2,
        DownToTop = 3,
        None = 9,
    }

    #endregion

    #region 信息位置类型
    /// <summary>
    ///信息位置类型
    /// </summary>
    /// <remarks></remarks>
    public enum MessagePositionType
    {
        Top = 0,
        Middle = 1,
        Bottom = 2,
        Left = 3,
        Center = 4,
        Right = 5,
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

    #region 商标位置类型
    /// <summary>
    /// 商标位置类型
    /// </summary>
    /// <remarks></remarks>
    public enum TrademarkPositionType
    {
        TopLeft = 0,
        TopCenter = 1,
        TopRight = 2,
        MiddleLeft = 3,
        MiddleCenter = 4,
        MiddleRight = 5,
        BottomLeft = 6,
        BottomCenter = 7,
        BottomRight = 8,
        None = 9,
    }

    #endregion

    #region 商标位置类型
    /// <summary>
    /// 商标位置类型
    /// </summary>
    /// <remarks></remarks>
    public enum TrademarkSizeType
    {
        /// <summary>
        /// 小
        /// </summary>
        [EnumDescription("10")]
        Small = 0,
        /// <summary>
        /// 中
        /// </summary>
        [EnumDescription("15")]
        Medium = 1,
        /// <summary>
        /// 大
        /// </summary>
        [EnumDescription("20")]
        Large = 2,
    }

    #endregion


    #region 移动状态类型
    /// <summary>
    /// 移动状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum MoveStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        NotMove = 0,
        /// <summary>
        /// 移动
        /// </summary>
        [EnumDescription("移动")]
        Moving = 1,
        /// <summary>
        /// 结束
        /// </summary>
        [EnumDescription("结束")]
        MoveFinish = 2,
    }

    #endregion

    #region 显示状态类型
    /// <summary>
    /// 显示状态类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowStateType
    {
        /// <summary>
        /// 等待
        /// </summary>
        [EnumDescription("等待")]
        NotShow = 0,
        /// <summary>
        /// 显示
        /// </summary>
        [EnumDescription("显示")]
        Showing = 1,
        /// <summary>
        /// 结束
        /// </summary>
        [EnumDescription("结束")]
        ShowFinish = 2,
    }

    #endregion
    #endregion
}

