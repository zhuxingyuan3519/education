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
using Service;
using Model;

namespace Web.WXPay
{
    public partial class WXPayResult: BasePage
    {
        protected string ReturnMsg = string.Empty;

        protected override void SetPowerZone()
        {
            //Model.EN_Course course = (EN_Course)Session["TransCourse"];
            //string courseCode = Request.QueryString["coursecode"];
            //if (course != null)
            //{
            //    courseCode = course.Code;
            //}
            string out_trade_no = Request.QueryString["out_trade_no"];
            string issuccess = Request.QueryString["issuccess"];
            List<CommonObject> listCommon = new List<CommonObject>();
            if(issuccess == "1")
            {
                WxPayData orderData = OrderQuery.Query(string.Empty, out_trade_no);//调用订单查询业务逻辑
                string trade_no = orderData.GetValue("transaction_id").ToString();
                out_trade_no = orderData.GetValue("out_trade_no").ToString();
                string total_fee = orderData.GetValue("total_fee").ToString();
                //courseCode = course.Code;// orderData.GetValue("goods_tag").ToString();
                string notify_time = orderData.GetValue("time_end").ToString();
                decimal _fee = MethodHelper.ConvertHelper.ToDecimal(total_fee, 0) / 100;
                DateTime time = MethodHelper.ConvertHelper.ToDateTime(notify_time, DateTime.Now);
                string openid = orderData.GetValue("openid").ToString();

                TD_PayLog payModel = CommonBase.GetModel<TD_PayLog>(out_trade_no);
                //Log.Info("WXPayResult", "根据out_trade_no：" + out_trade_no + "获取到本地订单");
                //查看是否存在此交易号
                if(payModel.Status == 1)
                {
                    Session["Member"] = TModel;
                    Session["PayModel"] = null;
                    ReturnMsg = "支付成功";
                    return;
                }
                else
                {
                    if(TModel == null)
                    {
                        string attach = orderData.GetValue("attach").ToString();
                        TModel = CommonBase.GetModel<Model.Member>(attach);//获取到当前登陆人
                        if(TModel != null)
                            Session["Member"] = TModel;
                        else
                        {
                            //Session["TransCourse"] = null;
                            Service.LogService.Log(TModel, "1", "支付成功，但业务处理失败：未查询到支付人信息");
                            ReturnMsg = "支付成功，但业务处理失败：未查询到支付人信息，请联系客服。";
                            return;
                        }
                    }
                    bool successflag = false;
                    //更新订单状态
                    payModel.Status = 1;
                    CommonBase.Update<TD_PayLog>(payModel, new string[] { "Status" }, listCommon);
                    if(payModel.PayType == "ApplyStudent")
                    {
                        //学员缴费
                        Sys_Role roles = CacheService.RoleList.FirstOrDefault(c => c.Code == payModel.ProductCode);
                        //获取到支付人
                        Model.Member upMember = CommonBase.GetModel<Model.Member>(payModel.PayID);

                        successflag = SignUpService.SignUpNoMRank(payModel, roles, upMember, TModel, "缴费成为" + roles.Name);
                    }
                    else
                    {
                        //购买训练内容
                        //更新会员的培训权限
                        MemberService.SetTrainPrivage(TModel, payModel, listCommon);
                        //设置分红
                        SHMoneyService.TrainPayChangeMoney(payModel, TModel, payModel.PayMoney, string.Empty, listCommon);
                        successflag = CommonBase.RunListCommit(listCommon);
                    }

                    if(successflag)
                    {
                        //Log.Info("WXPayResult", "业务处理完成，重新设置Session[Member]：");
                        Session["Member"] = TModel;
                        Session["PayModel"] = null;
                        Log.Info("WXPayResult", "重新设置Session[Member]之后的Learns：" + (Session["Member"] as Model.Member).Learns);
                        ReturnMsg = "支付成功";
                        //a_tokgj.Visible = true;
                    }
                    else
                    {
                        Service.LogService.Log(TModel, "1", "支付成功，但业务处理失败");
                        ReturnMsg = "支付成功，但业务处理失败，请联系客服。";
                    }
                }
            }
            else
            {
                ReturnMsg = "支付失败";
            }
        }

    }
}