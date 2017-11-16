/***********************************************************************
 * Copyright(c) 2015 SUNCREER Co.,ltd. All rights reserved. / Confidential
 * システム名称　：WebApiInfo
 * プログラム名称：データアクセス
 * 作成日・作成者：2017/11/14  張鵬
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToilluminateClient
{
    public static class WebApiInfo
    {
        public static string HttpPost(string url, string body)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "text/html,application/xhtml+xml,*/*";
                request.ContentType = "application/json";

                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("WebApiInfo", "HttpPost", ex);
                LogApp.OutputErrorLog("WebApiInfo", "HttpPost", url);
                return string.Empty;
            }
        }


        public static string HttpGet(string url)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,*/*";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("WebApiInfo", "HttpGet", ex);
                LogApp.OutputErrorLog("WebApiInfo", "HttpGet", url);
                return string.Empty;
            }
        }


        public static string DownloadFile(string url, string id)
        {
            try
            {
                string file = Utility.GetFullFileName(VariableInfo.FilesPath, Path.GetFileName(url));
                if (File.Exists(file) == false)
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(url, file);
                }

                return file;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("WebApiInfo", "DownloadFile", ex);
                LogApp.OutputErrorLog("WebApiInfo", "DownloadFile", url);
                return string.Empty;
            }
        }

        public static bool DownloadFile2(string url, string id)
        {   
            try
            {
                string file = Utility.GetFullFileName(VariableInfo.FilesPath, Path.GetFileName(url));
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                WebClient client = new WebClient();
                byte[] mbyte = new byte[1000000];
                int allmybyte = (int)mbyte.Length;

                int startmbyte = 0;

                using (Stream str = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(str))
                    {
                        while (allmybyte > 0)
                        {
                            int m = str.Read(mbyte, startmbyte, allmybyte);
                            if (m == 0)
                                break;

                            startmbyte += m;
                            allmybyte -= m;
                        }

                        reader.Dispose();
                    }

                    str.Dispose();
                }

                using (FileStream fStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fStream.Write(mbyte, 0, startmbyte);
                    fStream.Flush();
                    fStream.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogApp.OutputErrorLog("WebApiInfo", "DownloadFile2", ex);
                LogApp.OutputErrorLog("WebApiInfo", "DownloadFile2", url);
                return false;
            }
        }
        

    }
}
