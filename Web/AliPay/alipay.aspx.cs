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
using Model;

namespace Web.AliPay
{
    public partial class alipay: BasePage
    {
        protected override void SetPowerZone()
        {
            //课程编号
            string type = Request.QueryString["type"];

            //获取到课程
            EN_Course course = CommonBase.GetModel<EN_Course>(type);

            //商户订单号，商户网站订单系统中唯一订单号，必填
            string out_trade_no = TModel.ID.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + type;
            //订单名称，必填
            string subject = "购买" + course.Name + "课程";
            string money = course.Fee.ToString();  //Request.QueryString["money"];
            //收银台页面上，商品展示的超链接，必填
            string show_url = GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/m/app/course_detail?code=" + course.Code;

            //商品描述，可空
            string body = TModel.MID + subject;
            ////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////
            string rechargemoney = Request.QueryString["money"];

            //付款金额，必填
            string total_fee = money;

            if(ConfigHelper.GetAppSettings("IsTest") == "1")
            {
                //作为测试语句
                string testUrl = "/AliPay/return_url.aspx";
                testUrl += "?out_trade_no=" + out_trade_no;
                testUrl += "&trade_no=" + MethodHelper.CommonHelper.CreateNo();
                testUrl += "&trade_status=TRADE_SUCCESS&total_fee=" + total_fee;
                testUrl += "&notify_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                testUrl += "&seller_id=" + Service.AliPay.Config.seller_id;
                testUrl += "&buyer_logon_id=ceshizhanghao";
                testUrl += "&body=" + body;
                Response.Redirect(testUrl);
                Response.End();
            }
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
            //sParaTemp.Add("extend_params", addParm);
            //string biz_content = "{";
            //biz_content += "\"is_recharge\":\"1\",";
            //biz_content += "\"user_login_id\":\"" + TModel.ID + "\"";
            //biz_content += "}";
            //sParaTemp.Add("biz_content", biz_content);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }
    }
}