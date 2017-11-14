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
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
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
                Debug.WriteLine(url);
                throw ex;
            }
        }


        public static string HttpGet(string url)
        {
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
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
                Debug.WriteLine(url);
                throw ex;
            }
        }
        public static void Request(string url)
        {            
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

            try
            {
                

                #region "TransmitFileモード"
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                //Response.ContentEncoding = Encoding.UTF8;
                //Response.TransmitFile(file);
                #endregion

                #region "読みファイルモード"
                //FileInfo fileInfo = new FileInfo(file);
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                //Response.AddHeader("Content-Transfer-Encoding", "binary");
                //Response.ContentType = "application/octet-stream";
                //Response.ContentEncoding = Encoding.UTF8;
                //Response.WriteFile(fileInfo.FullName);
                //Response.Flush();
                #endregion

                #region "読みファイルモード - ブロック下載"
                /*
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long dataLengthToRead = fileStream.Length;

                    int bytesLength = Constants.READ_FILE_BYTE_MAX_LENGTH;// 毎回ファイル だけ 100K 読み取り
                    byte[] bytes = new byte[bytesLength];


                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.ContentEncoding = Encoding.UTF8;
                    while (dataLengthToRead > 0 && Response.IsClientConnected)
                    {
                        //読み取り
                        int lengthRead = fileStream.Read(bytes, 0, bytesLength);
                        Response.OutputStream.Write(bytes, 0, lengthRead);
                        Response.Flush();
                        bytes = new Byte[bytesLength];
                        dataLengthToRead = dataLengthToRead - lengthRead;
                    }
                    Response.OutputStream.Close();
                    Response.Close();
                }
                */
                Response.ContentType = "application/ms-download";
                string file = string.Empty;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                //Response.Clear();
                //Response.AddHeader("Content-Type", "application/octet-stream");
                //Response.Charset = "utf-8";
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileInfo.Name, System.Text.Encoding.UTF8));
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                //Response.WriteFile(fileInfo.FullName);
                //Response.Flush();
                //Response.Clear();
                #endregion

                #region "読みファイルモード - 流方式"
                //using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                //{
                //    byte[] bytes = new byte[(int)fileStream.Length];
                //    fileStream.Read(bytes, 0, bytes.Length);
                //    fileStream.Close();
                //    Response.ContentType = "application/octet-stream";
                //    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                //    Response.ContentEncoding = Encoding.UTF8;
                //    Response.BinaryWrite(bytes);
                //    Response.Flush();
                //}
                #endregion


                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        //        private async void button2_Click(object sender, EventArgs e)
        //        {
        //            HttpClient client = new HttpClient();
        //            //由HttpClient发出Delete Method
        //            HttpResponseMessage response = await client.DeleteAsync("http://localhost:41558/api/Demo" + "/1");
        //            if (response.IsSuccessStatusCode)
        //                MessageBox.Show("成功");
        //        }

        //        private async void button3_Click(object sender, EventArgs e)
        //        {
        //            //创建一个处理序列化的DataContractJsonSerializer
        //            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(People));
        //            MemoryStream ms = new MemoryStream();
        //            //将资料写入MemoryStream
        //            serializer.WriteObject(ms, new People() { Id = 1, Name = "Hello ni" });
        //            //一定要在这设定Position
        //            ms.Position = 0;
        //            HttpContent content = new StreamContent(ms);//将MemoryStream转成HttpContent
        //            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //            HttpClient client = new HttpClient();
        //            //由HttpClient发出Put Method
        //            HttpResponseMessage response = await client.PutAsync("http://localhost:41558/api/Demo" + "/1", content);
        //            if (response.IsSuccessStatusCode)
        //                MessageBox.Show("成功");
        //        }

        //        using (WebClient client = new WebClient())
        //{
        //     client.Headers["Type"] = "GET";
        //     client.Headers["Accept"] = "application/json";
        //     client.Encoding = Encoding.UTF8;
        //     client.DownloadStringCompleted += (senderobj, es) =>
        //     {
        //         var obj = es.Result;
        //};
        //client.DownloadStringAsync("http://localhost:41558/api/Demo");
        //}


    }
}
