using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using Model;
using Service;
using System.Linq;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify: Notify
    {
        public ResultNotify(Page page)
            : base(page)
        {
        }

        public override void ProcessNotify(string courseCode)
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if(!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if(!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                //查询该订单是否存在本系统
                Model.TD_PayLog payModel = CommonBase.GetModel<Model.TD_PayLog>(out_trade_no);
                if(payModel != null)
                {
                    #region 按照订单去查找
                    //查看是否存在此交易号
                    if(payModel.Status == 1)
                    {
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "OK");
                        //Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                        page.Response.Write(res.ToXml());
                        page.Response.End();
                    }
                    else
                    {
                        #region 主要业务处理
                        List<CommonObject> listCommon = new List<CommonObject>();
                        WxPayData orderData = OrderQuery.Query(string.Empty, out_trade_no);//调用订单查询业务逻辑
                        string attach = orderData.GetValue("attach").ToString();
                        Model.Member TModel = CommonBase.GetModel<Model.Member>(attach);
                        //更新订单状态
                        payModel.Status = 1;
                        CommonBase.Update<TD_PayLog>(payModel, new string[] { "Status" }, listCommon);

                        bool successflag = false;
                        if(payModel.PayType == "ApplyStudent")
                        {
                            //学员缴费
                            Sys_Role roles = CacheService.RoleList.FirstOrDefault(c => c.Code == payModel.ProductCode);
                            Model.Member upMember = CommonBase.GetModel<Model.Member>(payModel.PayID);
                            successflag = SignUpService.SignUpNoMRank(payModel, roles, upMember, TModel, "缴费成为" + roles.Name);
                        }
                        else
                        {
                            //更新会员的培训权限
                            MemberService.SetTrainPrivage(TModel, payModel, listCommon);
                            //设置分红
                            SHMoneyService.TrainPayChangeMoney(payModel, TModel, payModel.PayMoney, string.Empty, listCommon);
                        }
                        #endregion

                        if(CommonBase.RunListCommit(listCommon))
                        {
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                            //Log.Info("ResultNotify", "业务处理完成，重新设置Session[Member]：");
                            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
                            page.Session["Member"] = TModel;
                            //HttpContext.Current.Session["Member"] = TModel;
                            page.Session["PayModel"] = null;
                            //Log.Info("ResultNotify", "重新设置Session[Member]之后的Learns：" + (page.Session["Member"] as Model.Member).Learns);
                            page.Response.Write(res.ToXml());
                            page.Response.End();
                        }
                        else
                        {
                            res.SetValue("return_code", "FAIL");
                            res.SetValue("return_msg", "处理失败");
                            page.Response.Write(res.ToXml());
                            page.Response.End();
                        }
                    }
                    #endregion
                }
                else
                {
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "处理失败");
                    page.Response.Write(res.ToXml());
                    page.Response.End();
                }
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if(res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}