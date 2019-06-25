using DBUtility;
using MethodHelper;
using Service;
using Service.Alipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.AliPay
{
    public partial class alipay_test : BasePage
    {
        protected override void SetPowerZone()
        {
            if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
            {
                //络胜的缴费功能暂时先屏蔽，2017年2月15日00:02:20
                Response.Write("<script>alert('抱歉，系统升级，暂时停止会员升级，详情请咨询客服。');window.location ='/index';</script>");
                Response.End();
            }


            //查看O单商的收费端口数量，如果是0，就提示系统故障，不能交钱
            Model.Member OMember = CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
            if (OMember != null)
            {
                if (OMember.TradePoints <= 0)
                {
                    Response.Write("<script>alert('抱歉，系统故障，暂时无法支付，详情请咨询客服。');window.location ='/index';</script>");
                    Response.End();
                }
            }


            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //商户订单号，商户网站订单系统中唯一订单号，必填
            string out_trade_no = TModel.ID.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            //订单名称，必填
            string subject = "申请" + CacheService.GlobleConfig.Contacter + "缴费会员";
            string money = CacheService.GlobleConfig.Field1;  //Request.QueryString["money"];
            //付款金额，必填
            string total_fee = money;

            //收银台页面上，商品展示的超链接，必填
            string show_url = GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/posmarket";

            //商品描述，可空
            string body = TModel.MID + "支付VIP会员费用";
            ////////////////////////////////////////////////////////////////////////////////////////////////

            string testUrl = "/AliPay/return_url_test.aspx?out_trade_no=" + out_trade_no + "&trade_no=100000&trade_status=TRADE_SUCCESS&total_fee=" + total_fee + "&notify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS");
            Response.Redirect(testUrl);
            Response.End();

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Service.AliPay.Config.partner);
            sParaTemp.Add("seller_id", Service.AliPay.Config.seller_id);
            sParaTemp.Add("_input_charset", Service.AliPay.Config.input_charset.ToLower());
            sParaTemp.Add("service", Service.AliPay.Config.service);
            sParaTemp.Add("payment_type", Service.AliPay.Config.payment_type);
            sParaTemp.Add("notify_url", Service.AliPay.Config.notify_url);
            sParaTemp.Add("return_url", Service.AliPay.Config.return_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("show_url", show_url);
            //sParaTemp.Add("app_pay","Y");//启用此参数可唤起钱包APP支付。
            sParaTemp.Add("body", body);
            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.2Z6TSk&treeId=60&articleId=103693&docType=1
            //如sParaTemp.Add("参数名","参数值");
            //sParaTemp.Add("user_login_id", TModel.ID.ToString());

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }
    }
}