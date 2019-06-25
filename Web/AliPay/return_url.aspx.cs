using DBUtility;
using MethodHelper;
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
    public partial class return_url: System.Web.UI.Page
    {
        protected string applyType = "0", payStatus = "0";//是否支付成攻
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                SortedDictionary<string, string> sPara = GetRequestGet();
                string body = Request.QueryString["body"];

                string payForUse = Request.QueryString["paytype"];

                //if (body.IndexOf("申请分销商") > 0)  //申请分销商
                //    applyType = "applyagent";


                if(sPara.Count > 0)//判断是否有带返回参数
                {
                    Notify aliNotify = new Notify();
                    bool verifyResult = false;
                    if(ConfigHelper.GetAppSettings("IsTest") == "1")
                        verifyResult = true;
                    else
                        verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                    //MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), "支付宝返回验证：" + verifyResult.ToString(), DateTime.Now);
                    if(verifyResult)//验证成功
                    {
                        //商户订单号
                        string out_trade_no = Request.QueryString["out_trade_no"];
                        Model.Member TModel = null;
                        if(!string.IsNullOrEmpty(out_trade_no) && out_trade_no.IndexOf('-') > 0)
                        {
                            string memberID = out_trade_no.Split('-')[0];
                            payForUse = out_trade_no.Split('-')[2];
                            //查询到这个会员
                            Model.Member loginMember = CommonBase.GetModel<Model.Member>(memberID);
                            if(loginMember != null)
                            {
                                TModel = loginMember;
                                Session["Member"] = loginMember;
                            }
                        }
                        if(TModel == null)
                        {
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), "支付宝验证成功；但是未获取到用户信息", DateTime.Now);
                            //Response.Write("<script>window.location='/login'</script>");
                            if(MethodHelper.ConfigHelper.GetAppSettings("version") == "2.0")
                            {
                                Response.Write("<script>window.location='/m/app/user_login'</script>");
                            }
                            else
                            {
                                Response.Write("<script>window.location='/login'</script>");
                            }
                            Response.End();
                        }

                        MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), TModel.MID + "支付宝验证成功", DateTime.Now);
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码
                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表
                        //支付宝交易号
                        string trade_no = Request.QueryString["trade_no"];
                        //交易状态
                        string trade_status = Request.QueryString["trade_status"];
                        if(Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                        {
                            //校验seller_id
                            if(Request.QueryString["seller_id"] != Service.AliPay.Config.seller_id)
                            {
                                MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), TModel.MID + "支付宝验证成功：但seller_id验证失败", DateTime.Now);
                                Core.LogResult("支付失败" + Request.QueryString["trade_status"]);
                                Response.Write("<img src='/images/payerror.jpg' style='width:100%;padding-top:50%' />");
                            }
                            else
                            {
                                //判断该笔订单是否在商户网站中已经做过处理
                                if(CommonBase.GetModel<TD_PayLog>(trade_no) != null)
                                {
                                    string webIndex = Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/m/app/main_index.aspx";
                                    Response.Write("<script type='text/javascript'>alert('您已支付成功');window.location='" + webIndex + "'<script/>");
                                    Response.End();
                                }

                                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                                //如果有做过处理，不执行商户的业务程序
                                //Core.LogResult("支付成功" + Request.QueryString["trade_status"]);

                                //付款成功
                                payStatus = "1";
                                try
                                {
                                    //if(SignUpService.SignUp(null, TModel, TModel, "支付宝支付"))
                                    {
                                        Response.Write("<img src='/images/paysuccess.jpg' style='width:100%;padding-top:50%' />");
                                    }
                                    //else
                                    //{
                                    //    Response.Write("<img src='/images/payerror.jpg' style='width:100%;padding-top:50%' />");
                                    //}
                                }
                                catch
                                {
                                    Response.Write("<img src='/images/payerror.jpg' style='width:100%;padding-top:50%' />");
                                    MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), "支付宝支付失败", DateTime.Now);
                                    LogService.Log(TModel, "8", TModel.MID + "支付宝支付" + Request.QueryString["total_fee"] + ",返回状态参数:" + Request.QueryString["trade_status"] + "；完整返回参数：" + GetRequestGet().ToString());
                                }
                            }
                        }
                        else
                        {
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), TModel.MID + "支付宝验证失败：trade_status=" + Request.QueryString["trade_status"], DateTime.Now);
                            Core.LogResult("支付失败" + Request.QueryString["trade_status"]);
                            //付款失败
                            //PayService.PayFail(trade_no, out_trade_no, TModel, Request.QueryString["total_fee"], Request.QueryString["notify_time"], string.Empty, 1, Request.QueryString["buyer_logon_id"], listComm);
                            Response.Write("<img src='/images/payerror.jpg' style='width:100%;padding-top:50%' />");
                        }
                    }
                    else//验证失败
                    {
                        Core.LogResult("支付验证失败" + Request.QueryString["trade_status"]);
                        Response.Write("验证失败");
                    }
                }
                else
                {
                    Core.LogResult("支付宝无返回参数" + Request.QueryString["trade_status"]);
                    Response.Write("无返回参数");
                }
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for(i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}
