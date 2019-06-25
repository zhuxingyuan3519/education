<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="credit_banklist.aspx.cs" Inherits="Web.m.app.credit_banklist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topheight {
            margin-top:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    			<!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b><span id="spTitle">银行列表</span></h5>
    </div>
			<!--卡片管理菜单********************-->
        <div class="container">
            <asp:Repeater ID="repBankList" runat="server">
                <ItemTemplate>
                        <div class="col-sm-6 col-xs-6 topheight">
                            <a href="javascript:void(0)" onclick="turnPage('<%#Eval("Code") %>','<%#Eval("LinkUrl") %>','<%#Eval("Remark") %>')">
                                <img src="/Attachment/<%#Eval("PicUrl") %>" class="img-responsive bankimg" /></a>
                        </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
        <script type="text/javascript">
            var code = '<%=Request.QueryString["code"]%>';
            var isnav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value%>';
            $(function () {
                switch (code) {
                    case '1': $("#spTitle").html("我要办卡"); break;//我要办卡
                    case '2': $("#spTitle").html("我要贷款"); break;//我要贷款
                    case '3': $("#spTitle").html("学习资料"); break;//学习资料
                }
            });
            function turnPage(bankCode, linkUrl, webtitle) {
                if (code != '') {
                    switch (code) {
                        case '1':
                            if (isnav == "1")
                                      window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=" + linkUrl;
                                else
                                     window.location.href = linkUrl;
                            break;
                        case '2': window.location = 'credit_loan?bank=' + bankCode; break;//我要贷款
                        case '3': window.location = 'credit_material?bank=' + bankCode; break;//学习资料
                    }
                }
            }
    </script>
</asp:Content>
