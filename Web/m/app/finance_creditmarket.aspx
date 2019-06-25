<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="finance_creditmarket.aspx.cs" Inherits="Web.m.app.finance_creditmarket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(function () {
             setNavIndexChecked("1");
         });
		   var isnav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value%>';
		  function windowopenloan(NContent, webtitle) {
             if (isnav == "1")
                 window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=" + NContent;
             else
                 window.location.href = NContent;
         }
    </script>
    <style type="text/css">
        .topheight {
            margin-top:20px;
        }
        .bankimg{width:100px;height:40px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    			<!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b><span id="spTitle">信贷列表</span></h5>
    </div>
			<!--卡片管理菜单********************-->
        <div class="container">
            <div class="row">
            <asp:Repeater ID="repBankList" runat="server">
                <ItemTemplate>
                        <div class="col-sm-6 col-xs-6 topheight">
                            <a href="javascript:windowopenloan('<%#Eval("LinkUrl") %>','<%#Eval("Name") %>');">
                                <img src="/Attachment/<%#Eval("PicUrl") %>" class="img-responsive bankimg" /></a>
                        </div>
                </ItemTemplate>
            </asp:Repeater>
                </div>
        </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
        
</asp:Content>
