using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBUtility;
using Model;
using MethodHelper;
using System.Net;
using System.IO;

namespace Service
{
    public class WebRemoteRequest
    {
        public static string RequestWebApi(string url, string bodyJson)
        {
            var contentType = "application/json; charset=utf-8";
            //string bodyJson = "{ \"user_id\": \"" + fhlog.MID + "\", \"phone\": \"" + fhlog.FHMCode + "\", \"prize_type\": 2, \"prize_money\": " + (int)(fhlog.FHMoney * 100) + ", \"at_time\": " + new Random().Next(100000, 999999) + " }";
            //由于接口需要格林威治时间故将当前时间转为格林威治时间再将格林威治时间字符串转成日期类型传入头部
            var nowDateStr = DateTime.Now.ToString("r");
            var nowTimeGMT = DateTime.Parse(nowDateStr);
            var utf8 = Encoding.UTF8;
            var headerAuthorization = "Bearer Udhekishe7763gdheu77h8j";
            //var url = "http://jwy.u1200.com/api/v1/user/prize/rate";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "post";
            req.Headers.Add("Authorization", headerAuthorization);
            req.ContentType = contentType;
            req.Date = nowTimeGMT;

            byte[] bytes = utf8.GetBytes(bodyJson);
            req.ContentLength = bytes.Length;
            Stream reqstream = req.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            string result = string.Empty;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                using(StreamReader sr = new StreamReader(resp.GetResponseStream(), utf8))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
            }
            return result;
        }
    }
}
