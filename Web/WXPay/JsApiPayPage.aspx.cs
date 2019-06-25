using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using Model;

namespace Web.WXPay
{
    public partial class JsApiPayPage: BasePage
    {
        protected string ReturnMsg = string.Empty, out_trade_no = string.Empty;
        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        protected override void SetPowerZone()
        {
            //Log.Debug("JsApiPayPage", "进入SetPowerZone()方法 ");
            #region 设置支付记录表并加入Seesion
            if(!string.IsNullOrEmpty(Request.QueryString["chargeCode"]))
            {
                if(Session["PayModel"] == null)
                {
                    TD_ChargeList course = CommonBase.GetModel<TD_ChargeList>(Request.QueryString["chargeCode"]);
                    ReturnMsg = "该笔订单需要支付" + course.ChargeMoney + "元";
                    //订单表加入一条数据
                    TD_PayLog pay = new TD_PayLog();
                    pay.Code = WxPayApi.GenerateOutTradeNo();
                    out_trade_no = pay.Code;
                    pay.CreatedBy = TModel.ID;
                    pay.CreatedTime = DateTime.Now;
                    pay.IsDeleted = false;
                    pay.PayForMID = "admin";
                    pay.PayID = TModel.ID;
                    pay.PayMID = TModel.MID;
                    pay.PayMoney = MethodHelper.ConvertHelper.ToDecimal(course.ChargeMoney, 0);
                    pay.PayTime = DateTime.Now;
                    pay.PayType = "WXPay";
                    pay.PayWay = "WXPay";
                    pay.ProductCode = course.Code;
                    pay.ProductCount = 1;
                    pay.Status = 0;
                    pay.Remark = "开通记忆训练";
                    if(CommonBase.Insert<TD_PayLog>(pay))
                        Session["PayModel"] = pay;
                }
            }
            else if(!string.IsNullOrEmpty(Request.QueryString["applyrole"]))
            {
                //申请的级别
                if(Session["PayModel"] == null)
                {
                    Sys_Role course = Service.CacheService.RoleList.FirstOrDefault(c => c.Code == Request.QueryString["applyrole"]);
                    ReturnMsg = "该笔订单需要支付" + course.Remark + "元";
                    //订单表加入一条数据
                    //根据参数查询到申请的人
                    Model.Member upMember = CommonBase.GetModel<Model.Member>(Request.QueryString["applymid"]);
                    TD_PayLog pay = new TD_PayLog();
                    pay.Code = WxPayApi.GenerateOutTradeNo();
                    out_trade_no = pay.Code;
                    pay.CreatedBy = TModel.ID;
                    pay.CreatedTime = DateTime.Now;
                    pay.IsDeleted = false;
                    pay.PayForMID = "admin";
                    pay.PayID = upMember.ID;
                    pay.PayMID = upMember.MID;
                    pay.PayMoney = MethodHelper.ConvertHelper.ToDecimal(course.Remark, 0);
                    pay.PayTime = DateTime.Now;
                    pay.PayType = "ApplyStudent";
                    pay.PayWay = "WXPay";
                    pay.ProductCode = course.Code;
                    pay.ProductCount = 1;
                    pay.Status = 0;
                    pay.Remark = "升级到学员";
                    if(CommonBase.Insert<TD_PayLog>(pay))
                        Session["PayModel"] = pay;
                }
            }
            #endregion
            JsApiPay jsApiPay = new JsApiPay(this);
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                //Log.Debug("JsApiPayPage", "开始调用【网页授权获取用户信息】接口获取用户的openid和access_token ");
                jsApiPay.GetOpenidAndAccessToken();
                //获取收货地址js函数入口参数
                //wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                //Log.Debug("JsApiPayPage", "调取结束，开始获取openid ");
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                jsApiPay.openid = jsApiPay.openid;
                //Log.Debug(this.GetType().ToString(), "openid :" + jsApiPay.openid);
                //Log.Debug("JsApiPayPage", "openid :" + jsApiPay.openid);

                //Log.Debug(this.GetType().ToString(), "从session中获取PayModel");
                TD_PayLog payModel = Session["PayModel"] as TD_PayLog;
                out_trade_no = payModel.Code;
                jsApiPay.total_fee = int.Parse((Math.Round(MethodHelper.ConvertHelper.ToDecimal(payModel.PayMoney, 0) * 100, 0)).ToString());//  decimal.Parse(Service.CacheService.GlobleConfig.Field1);
                ReturnMsg = "该笔订单需要支付" + payModel.PayMoney + "元";
                //Log.Debug("JsApiPayPage", "开始调用统一下单接口");
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(out_trade_no, TModel.ID, payModel.ProductCode);
                //out_trade_no = unifiedOrderResult.GetValue("out_trade_no").ToString();
                //Log.Debug("JsApiPayPage", "调用统一下单接口调用完毕，开始获取wxJsApiParam：out_trade_no : " + out_trade_no);
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数      
                Session["PayModel"] = null;
                //Log.Debug("JsApiPayPage", "获取到 wxJsApiParam : " + wxJsApiParam);
                //return;
            }
            catch(Exception ex)
            {
                ReturnMsg = ex.ToString();
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
                btn_pay.Visible = false;
            }
        }

    }
}