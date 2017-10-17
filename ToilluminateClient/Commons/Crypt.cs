/***********************************************************************
 * Copyright(c) 2015 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：ModelStation
 * プログラム名称：データアクセス
 * 作成日・作成者：2015/05/11  張鵬
 ***********************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToilluminateClient
{
    /// ====================================================================
    /// クラス名：Crypt
    /// <summary>
    /// 暗号化に関する機能を提供します。
    /// </summary>
    /// <remarks>
    /// ■基本メソッド<br />
    /// 　・共通鍵暗号（対称暗号化方法）：DES<br />
    /// 　・非可逆暗号化：MD5,SHA1<br />
    /// </remarks>
    /// ====================================================================
    public class Crypt
    {
        #region 共通鍵暗号(DES)

        #region EncryptDES()
        /// <summary>
        /// 文字列をDES暗号化します。
        /// </summary>
        /// <param name="value">暗号化する文字列</param>
        /// <param name="key">共通鍵</param>
        /// <returns>暗号化された文字列</returns>
        public static string EncryptDES(string value, string key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            // DES暗号化オブジェクトの作成
            ICryptoTransform desdecrypt = CreateDESCryptoServiceProvider(key).CreateEncryptor();

            // 渡された値を暗号化して返します。
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptStream = new CryptoStream(memoryStream, desdecrypt, CryptoStreamMode.Write))
            {
                byte[] bytesIn = Encoding.UTF8.GetBytes(value);
                cryptStream.Write(bytesIn, 0, bytesIn.Length);
                cryptStream.FlushFinalBlock();
                byte[] bytesOut = memoryStream.ToArray();

                return Convert.ToBase64String(bytesOut);
            }
        }
        #endregion

        #region DecryptDES()
        /// <summary>
        /// 暗号化された文字列をDES復号化します。
        /// </summary>
        /// <param name="value">暗号化された文字列</param>
        /// <param name="key">共通鍵</param>
        /// <returns>復号化された文字列</returns>
        public static string DecryptDES(string value, string key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            try
            {

                // DES復号化オブジェクトの作成
                ICryptoTransform desdecrypt = CreateDESCryptoServiceProvider(key).CreateDecryptor();

                byte[] bytesIn = Convert.FromBase64String(value);

                // 暗号化された値を復号化して返します。
                using (MemoryStream stream = new MemoryStream(bytesIn))
                using (CryptoStream cryptStream = new CryptoStream(stream, desdecrypt, CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region protectedメソッド
        /// <summary>
        /// 共通鍵を元にDESCryptoServiceProviderインスタンスを生成します。
        /// </summary>
        /// <param name="key">共通鍵</param>
        /// <returns>DESCryptoServiceProviderインスタンス</returns>
        protected static DESCryptoServiceProvider CreateDESCryptoServiceProvider(string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] bytesKey = Encoding.UTF8.GetBytes(key);

            des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
            des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

            return des;
        }

        /// <summary>
        /// 共有キー用に、バイト配列のサイズを変更します。
        /// </summary>
        /// <param name="bytes">サイズを変更するバイト配列</param>
        /// <param name="newSize">バイト配列の新しい大きさ</param>
        /// <returns>サイズが変更されたバイト配列</returns>
        protected static byte[] ResizeBytesArray(byte[] bytes, int newSize)
        {
            byte[] newBytes = new byte[newSize];
            if (bytes.Length <= newSize)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    newBytes[i] = bytes[i];
                }
            }
            else
            {
                int pos = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    newBytes[pos++] ^= bytes[i];
                    if (pos >= newBytes.Length)
                    {
                        pos = 0;
                    }
                }
            }
            return newBytes;
        }
        #endregion

        #endregion

        #region 非可逆暗号(MD5)
        /// <summary>
        /// 文字列をMD5暗号化します。<br/>
        /// <br/>
        /// この関数は下位互換性のために残してあります。<br/>
        /// より強固なGetSHA1String()を利用してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32文字の文字列</returns>
        public static string GetMD5String(string value)
        {
            // 文字列をbyte型配列に変換。
            return GetMD5String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 文字列をMD5暗号化します。<br/>
        /// <br/>
        /// この関数は下位互換性のために残してあります。<br/>
        /// より強固なGetSHA1String()を利用してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32文字の文字列</returns>
        public static string GetMD5String(byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(value);
            // byte型配列を16進数の文字列に変換
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
        #endregion

        #region 非可逆暗号(SHA1)
        /// <summary>
        /// 文字列をSHA1暗号化します。<br/>
        /// <br/>
        /// この関数は下位互換性のために残してあります。<br/>
        /// より強固なGetSHA256String()を利用してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>40文字の文字列</returns>
        public static string GetSHA1String(string value)
        {
            // 文字列をbyte型配列に変換。
            return GetSHA1String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 文字列をSHA1暗号化します。<br/>
        /// <br/>
        /// この関数は下位互換性のために残してあります。<br/>
        /// より強固なGetSHA256String()を利用してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>40文字の文字列</returns>
        public static string GetSHA1String(byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 =
                new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytes = sha1.ComputeHash(value);
            // byte型配列を16進数の文字列に変換
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
        #endregion
    }
}