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
    public partial class return_url_test : System.Web.UI.Page
    {
        protected string applyType = "0", payStatus = "0";//是否支付成攻
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SortedDictionary<string, string> sPara = GetRequestGet();
                string body = Request.QueryString["body"];
                if (body.IndexOf("申请分销商") > 0)  //申请分销商
                    applyType = "applyagent";


                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    //Notify aliNotify = new Notify();
                    //bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);
                    //MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), "支付宝返回验证：" + verifyResult.ToString(), DateTime.Now);
                    if (true)//验证成功
                    {
                        //商户订单号
                        string out_trade_no = Request.QueryString["out_trade_no"];
                        Model.Member TModel = null;
                        if (!string.IsNullOrEmpty(out_trade_no) && out_trade_no.IndexOf('-') > 0)
                        {
                            string memberID = out_trade_no.Split('-')[0];
                            //查询到这个会员
                            Model.Member loginMember = CommonBase.GetModel<Model.Member>(memberID);
                            if (loginMember != null)
                            {
                                TModel = loginMember;
                                Session["Member"] = loginMember;
                            }
                        }
                        if (TModel == null)
                        {
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), "支付宝验证成功；但是未获取到用户信息", DateTime.Now);
                            Response.Write("<script>window.location='/login'</script>");
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
                        List<CommonObject> listComm = new List<CommonObject>();
                        if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序
                            //Core.LogResult("支付成功" + Request.QueryString["trade_status"]);
                         
                            //O单商扣除一个端口
                            //查询到O单商
                            Model.Member OMember = CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
                            if (OMember != null)
                            {
                                OMember.TradePoints = OMember.TradePoints - 1;
                                CommonBase.Update<Model.Member>(OMember, new string[] { "TradePoints" }, listComm);
                                LogService.Log(OMember, "10", TModel.MID + "支付宝申请缴费会员成功，消耗1个端口", listComm);
                            }

                            //插入付款纪录表TD_PayLog
                            TD_PayLog payModel = new TD_PayLog();
                            payModel.Code = trade_no;//支付宝交易号
                            payModel.PayType = "1";
                            payModel.PayWay = "支付宝";//支付宝
                            payModel.ProductCode = out_trade_no;//支付宝交易号
                            payModel.Company = 0;
                            payModel.CreatedBy = TModel.MID;
                            payModel.CreatedTime = DateTime.Now;
                            payModel.IsDeleted = false;
                            payModel.PayForMID = "0";
                            //payModel.PayPic = Request.QueryString["buyer_logon_id"];//买家支付宝付款账号
                            payModel.PayMID = TModel.MID; //会员ID
                            payModel.PayMoney = MethodHelper.CommonHelper.GetDecimal(Request.QueryString["total_fee"]);
                            payModel.PayTime = MethodHelper.CommonHelper.GetDateTime(Request.QueryString["notify_time"]);
                            payModel.Status = 1;
                            payModel.PayID = TModel.ID.ToString();
                            payModel.Remark = TModel.MID + "支付宝在线成功支付" + payModel.PayMoney + "元成为缴费会员";
                            //插入表中
                            CommonBase.Insert<TD_PayLog>(payModel, listComm);

                            MemberService.ActiveMemberToVIP(TModel, payModel.PayMoney, listComm);
                          
                            LogService.Log(TModel, "8", TModel.MID + "支付宝支付" + payModel.PayMoney + "成功", listComm);
                            //付款成功，给管理员发送消息
                            List<Model.Member> receiveList = CommonBase.GetList<Model.Member>("RoleCode='Manage'");
                            string message = payModel.Remark;
                            Service.MessageService.SendNewMessage(TModel, receiveList, message, listComm);
                            //洛胜卡管家加入红包模式
                            if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
                            {
                                //缴费之后发放公司红包
                                SHMoneyService.SendCompanyRedBag(TModel, listComm);
                            }

                            Response.Write("<img src='/images/paysuccess.jpg' style='width:100%;padding-top:50%' />");
                        }
                        else
                        {

                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), TModel.MID + "支付宝验证失败：trade_status=" + Request.QueryString["trade_status"], DateTime.Now);
                            Core.LogResult("支付失败" + Request.QueryString["trade_status"]);
                            TD_PayLog payModel = new TD_PayLog();
                            payModel.Code = out_trade_no;
                            payModel.PayType = "1";
                            payModel.PayWay = "AliPay";//支付宝
                            payModel.ProductCode = trade_no;//支付宝交易号
                            payModel.Company = 0;
                            payModel.CreatedBy = TModel.MID;
                            payModel.CreatedTime = DateTime.Now;
                            payModel.IsDeleted = false;
                            payModel.PayForMID = "0";
                            payModel.PayPic = Request.QueryString["buyer_logon_id"];//买家支付宝付款账号
                            payModel.PayMID = TModel.MID; //会员ID
                            payModel.PayMoney = MethodHelper.CommonHelper.GetDecimal(Request.QueryString["total_fee"]);
                            payModel.PayTime = MethodHelper.CommonHelper.GetDateTime(Request.QueryString["notify_time"]);
                            payModel.Remark = TModel.MID + "支付宝在线支付" + payModel.PayMoney + "元，但支付宝返回失败。";
                            payModel.Status = 2;
                            payModel.PayID = TModel.ID.ToString();
                            //插入表中
                            CommonBase.Insert<TD_PayLog>(payModel, listComm);
                            LogService.Log(TModel, "8", TModel.MID + "支付宝支付" + payModel.PayMoney + "失败");
                            Response.Write("<img src='/images/payerror.jpg' style='width:100%;padding-top:50%' />");
                        }

                        //打印页面

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                        try
                        {
                            string log = TModel.MID + "支付后逻辑：开始执行本次业务SQL,共：" + listComm.Count.ToString() + "条SQL" + "\r\n";
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), log, DateTime.Now);
                            CommonBase.RunListCommit(listComm);
                            log = "结束执行本次业务SQL,共：" + listComm.Count.ToString() + "条SQL" + "\r\n";
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), log, DateTime.Now);
                        }
                        catch
                        {
                            string log = TModel.MID + "支付后逻辑：执行本次业务SQL,共：" + listComm.Count.ToString() + "条SQL，执行失败，返回状态参数:" + Request.QueryString["trade_status"] + "；完整返回参数：" + GetRequestGet().ToString() + "\r\n";
                            MethodHelper.LogHelper.WriteTextLog(typeof(return_url).ToString(), log, DateTime.Now);
                            LogService.Log(TModel, "8", TModel.MID + "支付宝支付" + Request.QueryString["total_fee"] + ",返回状态参数:" + Request.QueryString["trade_status"] + "；完整返回参数：" + GetRequestGet().ToString());
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}
