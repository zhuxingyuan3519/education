<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="credit_apply.aspx.cs" Inherits="Web.m.app.credit_apply" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(function () {
             setNavIndexChecked("1");
         });
         var isnav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value%>';
         function windowopenloan( webtitle,NContent) {
             if (isnav == "1")
                 window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=" + NContent;
             else
                 window.location.href = NContent;
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row personer text-center">
      <img src="../images/wybktitle.png" class="img-responsive imgshowtitle" />
        <h4>我要办卡</h4>
    </div>
			<!--信用卡中心菜单********************-->
    <div class="row">
        <ul class="list-group">
                <%if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00"||true)
                  { %>
               <li class="list-group-item list-group-li-pater"><a href="javascript:windowopenloan('快卡通道',' http://sys.goodpay.net.cn/mobile/ApplyCredit/ApplyCreditCard.html?token=bacc6146edaaef2daefcf6881e49398d');">
                <img src="../images/kktd.png"  class="indexIcon"/>快卡通道</a></li>

                   <li class="list-group-item list-group-li-pater"><a href="javascript:windowopenloan('金卡通道','http://xlf.mxsj888.com/jktd.asp');">
                <img src="../images/jktd.png"  class="indexIcon"/>金卡通道</a></li>

               <li class="list-group-item list-group-li-pater"><a href="javascript:windowopenloan('白金卡通道','http://xlf.mxsj888.com/bjktd.asp');">
                <img src="../images/bjktd.png"  class="indexIcon"/>白金卡通道</a></li>

              <li class="list-group-item list-group-li-pater"><a href="javascript:windowopenloan('银行通道','credit_banklist?code=1');">
                <img src="../images/yhtd.png"  class="indexIcon"/>银行通道</a></li>
           
                 <%} %>

           
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
