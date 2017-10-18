/***********************************************************************
 * Copyright(c) 2015 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * �V�X�e�����́@�FModelStation
 * �v���O�������́F�f�[�^�A�N�Z�X
 * �쐬���E�쐬�ҁF2015/05/11  ���Q
 ***********************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToilluminateClient
{
    /// ====================================================================
    /// �N���X���FCrypt
    /// <summary>
    /// �Í����Ɋւ���@�\��񋟂��܂��B
    /// </summary>
    /// <remarks>
    /// ����{���\�b�h<br />
    /// �@�E���ʌ��Í��i�Ώ̈Í������@�j�FDES<br />
    /// �@�E��t�Í����FMD5,SHA1<br />
    /// </remarks>
    /// ====================================================================
    public class Crypt
    {
        #region ���ʌ��Í�(DES)

        #region EncryptDES()
        /// <summary>
        /// �������DES�Í������܂��B
        /// </summary>
        /// <param name="value">�Í������镶����</param>
        /// <param name="key">���ʌ�</param>
        /// <returns>�Í������ꂽ������</returns>
        public static string EncryptDES(string value, string key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            // DES�Í����I�u�W�F�N�g�̍쐬
            ICryptoTransform desdecrypt = CreateDESCryptoServiceProvider(key).CreateEncryptor();

            // �n���ꂽ�l���Í������ĕԂ��܂��B
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
        /// �Í������ꂽ�������DES���������܂��B
        /// </summary>
        /// <param name="value">�Í������ꂽ������</param>
        /// <param name="key">���ʌ�</param>
        /// <returns>���������ꂽ������</returns>
        public static string DecryptDES(string value, string key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            try
            {

                // DES�������I�u�W�F�N�g�̍쐬
                ICryptoTransform desdecrypt = CreateDESCryptoServiceProvider(key).CreateDecryptor();

                byte[] bytesIn = Convert.FromBase64String(value);

                // �Í������ꂽ�l�𕜍������ĕԂ��܂��B
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

        #region protected���\�b�h
        /// <summary>
        /// ���ʌ�������DESCryptoServiceProvider�C���X�^���X�𐶐����܂��B
        /// </summary>
        /// <param name="key">���ʌ�</param>
        /// <returns>DESCryptoServiceProvider�C���X�^���X</returns>
        protected static DESCryptoServiceProvider CreateDESCryptoServiceProvider(string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] bytesKey = Encoding.UTF8.GetBytes(key);

            des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
            des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

            return des;
        }

        /// <summary>
        /// ���L�L�[�p�ɁA�o�C�g�z��̃T�C�Y��ύX���܂��B
        /// </summary>
        /// <param name="bytes">�T�C�Y��ύX����o�C�g�z��</param>
        /// <param name="newSize">�o�C�g�z��̐V�����傫��</param>
        /// <returns>�T�C�Y���ύX���ꂽ�o�C�g�z��</returns>
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

        #region ��t�Í�(MD5)
        /// <summary>
        /// �������MD5�Í������܂��B<br/>
        /// <br/>
        /// ���̊֐��͉��ʌ݊����̂��߂Ɏc���Ă���܂��B<br/>
        /// ��苭�ł�GetSHA1String()�𗘗p���Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32�����̕�����</returns>
        public static string GetMD5String(string value)
        {
            // �������byte�^�z��ɕϊ��B
            return GetMD5String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// �������MD5�Í������܂��B<br/>
        /// <br/>
        /// ���̊֐��͉��ʌ݊����̂��߂Ɏc���Ă���܂��B<br/>
        /// ��苭�ł�GetSHA1String()�𗘗p���Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32�����̕�����</returns>
        public static string GetMD5String(byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(value);
            // byte�^�z���16�i���̕�����ɕϊ�
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
        #endregion

        #region ��t�Í�(SHA1)
        /// <summary>
        /// �������SHA1�Í������܂��B<br/>
        /// <br/>
        /// ���̊֐��͉��ʌ݊����̂��߂Ɏc���Ă���܂��B<br/>
        /// ��苭�ł�GetSHA256String()�𗘗p���Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns>40�����̕�����</returns>
        public static string GetSHA1String(string value)
        {
            // �������byte�^�z��ɕϊ��B
            return GetSHA1String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// �������SHA1�Í������܂��B<br/>
        /// <br/>
        /// ���̊֐��͉��ʌ݊����̂��߂Ɏc���Ă���܂��B<br/>
        /// ��苭�ł�GetSHA256String()�𗘗p���Ă��������B
        /// </summary>
        /// <param name="value"></param>
        /// <returns>40�����̕�����</returns>
        public static string GetSHA1String(byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 =
                new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytes = sha1.ComputeHash(value);
            // byte�^�z���16�i���̕�����ɕϊ�
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
        #endregion
    }
}