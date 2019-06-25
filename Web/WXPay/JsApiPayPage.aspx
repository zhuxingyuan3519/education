<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="JsApiPayPage.aspx.cs" Inherits="Web.WXPay.JsApiPayPage" %>

<%--微信支付--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       
    </style>
    <script type="text/javascript">
        //调用微信JS api 支付
        function jsApiCall()
        {
            WeixinJSBridge.invoke(
            'getBrandWCPayRequest',
            <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                        WeixinJSBridge.log(res.err_msg);
                        if(res.err_msg == "get_brand_wcpay_request:ok"){
                            //支付成功
                            window.location ='/WXPay/WXPayResult?issuccess=1&out_trade_no=<%=out_trade_no%>';
                        }
                        else{
                            //支付失败
                            window.location ='/WXPay/WXPayResult?issuccess=0&out_trade_no=<%=out_trade_no%>';
                        }
                        //alert(res.err_code +"_"+ res.err_desc+"_" + res.err_msg);
                    }
                    );
                }

                function callpay()
                {
                    if (typeof WeixinJSBridge == "undefined")
                    {
                        if (document.addEventListener)
                        {
                            document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                        }
                        else if (document.attachEvent)
                        {
                            document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                            document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                        }
                    }
                    else
                    {
                        jsApiCall();
                    }
                }
               
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div style="text-align: center; padding-top: 50%; font-size: 20px; color: #a9a1a1; font-weight: bolder;">
            <%=ReturnMsg %><br />

            <div style="padding-top: 20px;">
                <input type="button" runat="server" id="btn_pay" class="btn btn-block btn-lg btn-success btn-pay" style="background-color: #59bb14; border-color: #59bb14;" value="立即支付" onclick="callpay()" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
