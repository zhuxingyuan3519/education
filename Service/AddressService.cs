using DBUtility;
using MethodHelper;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Service
{
    public class AddressService
    {
        public static Sys_StandardArea GetAddressByAdCode(string adCode)
        {
            Sys_StandardArea add = CacheService.SatandardAddressList.Where(c => c.AdCode == adCode).FirstOrDefault();
            if (add == null)
            {
                return new Sys_StandardArea();
            }
            else
                return add;
        }



        /// <summary>
        //从高德地图接口中同步更新地址信息到本地数据库
        /// </summary>
        public static void SynsAddressFromAMap()
        {
            string postUrl = "http://restapi.amap.com/v3/config/district?";
            //获取到本地的省份信息
            List<Sys_Address> listAddress = CommonBase.GetList<Model.Sys_Address>("Level=20");
            foreach (Sys_Address address in listAddress)
            {
                string paramStr = "keywords=" + address.Name + "&subdistrict=2&key=220d34b6309a0d313caaa1d2b41af109&output=xml";
                string backinfo = GetData(postUrl, paramStr);
                SaveSynsAddressData(backinfo);
                break; ;
            }
        }

        protected static void SaveSynsAddressData(string addXml)
        {
            XmlNode node = XMLHelper.GetXmlNodeList(addXml, "districts")[0];
            string cityList = node.SelectSingleNode("district").InnerXml;

            //node = XMLHelper.GetXmlNodeList(cityList, "citycode");
            //string citycode = node.InnerText;
            //string name = node.InnerText;
            //string provinceNodeXml = node.InnerXml;

        }





        public static string GetData(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/xml;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string PostData(string purl, string str)
        {
            try
            {
                byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(str);
                // 准备请求 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(purl);

                //设置超时
                req.Timeout = 30000;
                req.Method = "GET";
                req.ContentType = "application/xml;charset=UTF-8";
                req.ContentLength = data.Length;
                Stream stream = req.GetRequestStream();
                // 发送数据 
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = rep.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                StringBuilder sb = new StringBuilder("");
                while (count > 0)
                {
                    String readstr = new String(read, 0, count);
                    sb.Append(readstr);
                    count = readStream.Read(read, 0, 256);
                }

                rep.Close();
                readStream.Close();

                return sb.ToString();

            }
            catch (Exception ex)
            {
                return "posterror";
                // ForumExceptions.Log(ex);
            }
        }

    }
}
