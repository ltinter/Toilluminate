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
                string tempFile =  string.Format("{0}.temp", file);
                if (File.Exists(file) == false)
                {
                    if (File.Exists(tempFile))
                    {
                        try
                        {
                            File.Delete(tempFile);
                        }
                        catch { }
                    }


                    WebClient client = new WebClient();
                    client.DownloadFile(url, tempFile);

                    File.Move(tempFile, file);
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


    }
}
