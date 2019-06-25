using DBUtility;
using Model;
using Service;
using Service.Alipay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.AliPay
{
    public partial class notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    string body = Request.Form["body"];

                    string payForUse = Request.Form["paytype"];

                    if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        Model.Member TModel = null;
                        if (!string.IsNullOrEmpty(out_trade_no) && out_trade_no.IndexOf('-') > 0)
                        {
                            string memberID = out_trade_no.Split('-')[0];
                            payForUse = out_trade_no.Split('-')[2];
                            //查询到这个会员
                            Model.Member loginMember = CommonBase.GetModel<Model.Member>(memberID);
                            if (loginMember != null)
                            {
                                TModel = loginMember;
                            }
                        }

                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                        //如果有做过处理，不执行商户的业务程序
                        //判断该笔订单是否在商户网站中已经做过处理
                        if (CommonBase.GetModel<TD_PayLog>(trade_no) != null)
                        {
                            MethodHelper.LogHelper.WriteTextLog(typeof(notify_url).ToString(), TModel.MID + "支付宝异步通知执行成功", DateTime.Now);
                            //该笔订单已经处理过了
                            Response.Write("success");
                        }
                        else
                        {
                            List<CommonObject> listComm = new List<CommonObject>();
                            //付款成功
                            PayService.PaySuccess(trade_no, out_trade_no, TModel, Request.Form["total_fee"], Request.Form["notify_time"], body, 1, Request.Form["buyer_logon_id"], payForUse, listComm);
                            if (CommonBase.RunListCommit(listComm))
                            {
                                MethodHelper.LogHelper.WriteTextLog(typeof(notify_url).ToString(), TModel.MID + "支付宝异步通知执行成功", DateTime.Now);
                                Response.Write("fail");
                            }
                        }
                        //注意：
                        //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    }
                    else
                    {
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
