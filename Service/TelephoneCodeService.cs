using DBUtility;
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
    /// <summary>
    /// 手机验证码相关
    /// </summary>
    public class TelephoneCodeService
    {
        public static string SendCode(string phoneNum, string createBy, string remark)
        {
            string result = string.Empty;
            string postUrl = "http://service2.winic.org/service.asmx/SendMessages?";
            string smsuid = GlobleConfigService.GetWebConfig("SMSUserCode").Value;// MethodHelper.ConfigHelper.GetAppSettings("SmsUid");// "AAAAAAA";
            string smspwd = GlobleConfigService.GetWebConfig("SMSUserPwd").Value;// MethodHelper.ConfigHelper.GetAppSettings("SmsPwd");// "PPPPPP";
            string randomCode = new MethodHelper.RandomHelper().GetRandomInt(1000, 9999).ToString();
            string msg = GlobleConfigService.GetWebConfig("SmsContent").Value;// MethodHelper.ConfigHelper.GetAppSettings("SmsContent");
            if (remark == "用户注册")
            {
                // msg = GlobleConfigService.GetWebConfig("SmsContent").Value;//MethodHelper.ConfigHelper.GetAppSettings("SmsContent");
            }
            else if (remark == "找回密码")
            {
                msg = GlobleConfigService.GetWebConfig("SmsFindPwdContent").Value;//MethodHelper.ConfigHelper.GetAppSettings("SmsFindPwdContent");
            }
            else if (remark == "用户提现")
            {
                msg = GlobleConfigService.GetWebConfig("SmsTXContent").Value;//MethodHelper.ConfigHelper.GetAppSettings("SmsTXContent");
            }
            //把占位符替换掉
            msg = msg.Replace("{{SendCode}}", randomCode);
            string paramStr = "uid=" + System.Web.HttpUtility.UrlEncode(smsuid) + "&pwd=" + smspwd + "&tos=" + phoneNum + "&msg=" + System.Web.HttpUtility.UrlEncode(msg) + "&otime=";
            string backinfo = PostData(postUrl, paramStr);
            //发送成功会返回16位的短信编码
            //            <?xml version="1.0" encoding="UTF-8"?>
            //<string xmlns="http://tempuri.org/">
            //0106120218011881
            //</string>
            XmlNodeList nodeList = MethodHelper.XMLHelper.GetXmlNodeList(backinfo, "string");
            if (nodeList.Count > 0)
            {
                string content = nodeList[0].InnerText;
                if (content.Trim().Length > 4)
                {
                    try
                    {
                        //只有发送成功了保存到表中
                        Sys_SendCode sendCode = new Sys_SendCode();
                        sendCode.CreatedBy = createBy;
                        sendCode.CreatedTime = DateTime.Now;
                        sendCode.IsUsed = false;
                        sendCode.Remark = remark;
                        sendCode.SendCode = randomCode;
                        sendCode.SendTime = DateTime.Now;
                        sendCode.Telephone = phoneNum;
                        //把这个手机号其他发的都设为已使用过的
                        List<CommonObject> listComm = new List<CommonObject>();
                        listComm.Add(new CommonObject("update Sys_SendCode set IsUsed=1 where Telephone='" + phoneNum + "' and IsUsed=0 and Remark='" + remark + "'", null));
                        CommonBase.Insert<Sys_SendCode>(sendCode, listComm);
                        if (CommonBase.RunListCommit(listComm))
                        {
                            result = randomCode;
                        }
                    }
                    catch
                    {
                        result = string.Empty;
                    }
                }
            }
            return result;
        }

        public static bool CheckValidCode(string phoneNum, string validCode, string remark)
        {
            string sql = "select SendCode from Sys_SendCode where Telephone='" + phoneNum + "' and SendCode='" + validCode + "' and IsUsed=0 and Remark='" + remark + "'";
            object obj = CommonBase.GetSingle(sql);
            if (obj != null && obj.ToString() == validCode)
                return true;
            else
                return false;
        }

        public static string PostData(string purl, string str)
        {
            try
            {
                byte[] data = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
                // 准备请求 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(purl);

                //设置超时
                req.Timeout = 30000;
                req.Method = "Post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                Stream stream = req.GetRequestStream();
                // 发送数据 
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = rep.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
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


        public static string SendSMS(string phoneNum, string createBy, string sendContent)
        {
            string result = string.Empty;
            string postUrl = "http://service2.winic.org/service.asmx/SendMessages?";
            string smsuid = GlobleConfigService.GetWebConfig("SMSUserCode").Value;// MethodHelper.ConfigHelper.GetAppSettings("SmsUid");// "AAAAAAA";
            string smspwd = GlobleConfigService.GetWebConfig("SMSUserPwd").Value;// MethodHelper.ConfigHelper.GetAppSettings("SmsPwd");// "PPPPPP";

            string paramStr = "uid=" + System.Web.HttpUtility.UrlEncode(smsuid) + "&pwd=" + smspwd + "&tos=" + phoneNum + "&msg=" + System.Web.HttpUtility.UrlEncode(sendContent) + "&otime=";
            string backinfo = PostData(postUrl, paramStr);
            //发送成功会返回16位的短信编码
            //            <?xml version="1.0" encoding="UTF-8"?>
            //<string xmlns="http://tempuri.org/">
            //0106120218011881
            //</string>
            XmlNodeList nodeList = MethodHelper.XMLHelper.GetXmlNodeList(backinfo, "string");
            if (nodeList.Count > 0)
            {
                string content = nodeList[0].InnerText;
                if (content.Trim().Length > 4)
                {
                    result = "1";
                }
            }
            return result;
        }
    }
}
