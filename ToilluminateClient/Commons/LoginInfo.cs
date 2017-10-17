/***********************************************************************
 * Copyright(c) 2015 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：ModelStation
 * プログラム名称：ユーザー情報管理
 * 作成日・作成者：2015/05/11  張鵬
 ***********************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToilluminateClient
{
   
    /// <summary>
    /// ユーザー情報管理クラス
    /// </summary>
    public class LoginInfo
    {
        #region フィールド
        /// <summary>
        /// 放送ID
        /// </summary>
        private int playID = 0;

        /// <summary>
        /// 放送リストID
        /// </summary>
        private int playListID = 0;

        #endregion

        #region publicプロパティ

        /// <summary>
        /// 放送ID
        /// </summary>
        public int PlayID
        {
            get
            {
                return playID;
            }
        }

        /// <summary>
        /// 放送リストID
        /// </summary>
        public int PlayListID
        {
            get
            {
                return playListID;
            }
        }

        #endregion

        #region publicメソッド
      
        

        #endregion
    }
}