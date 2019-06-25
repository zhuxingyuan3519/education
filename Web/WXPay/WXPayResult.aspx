<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="WXPayResult.aspx.cs" Inherits="Web.WXPay.WXPayResult" %>

<%--微信支付返回页面--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       
    </style>
    <script type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div style="text-align: center; padding-top: 50%; font-size: 20px; color: #a9a1a1; font-weight: bolder;">
            <%=ReturnMsg %><br />

            <div style="padding-top: 20px;">
                <%--<a class="btn btn-block btn-lg btn-success btn-pay" visible="false" runat="server" id="a_tokgj" href="../m/app/to_kgj">访问卡管家</a>--%><br />
                <a class="btn btn-block btn-lg btn-success btn-pay" href="../m/app/main_mine">返回首页</a>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
