<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="credit_loanapply.aspx.cs" Inherits="Web.m.app.credit_loanapply" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(function () {
             setNavIndexChecked("1");
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row personer text-center">
      <img src="../images/wydktitile.png" class="img-responsive imgshowtitle" />
        <h4>我要贷款</h4>
    </div>
			<!--信用卡中心菜单********************-->
    <div class="row">
        <ul class="list-group">
                <%if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00"||true)
                  { %>
               <li class="list-group-item list-group-li-pater"><a href="credit_loan.aspx">
                <img src="../images/yhxyd.png"  class="indexIcon"/>银行信用贷</a></li>

                   <li class="list-group-item list-group-li-pater"><a href="out_link.aspx?returnTitle=网贷&returnUrl=http://xlf.mxsj888.com/a02.asp?mkid=2">
                <img src="../images/wd.png"  class="indexIcon"/>网贷</a></li>

               <li class="list-group-item list-group-li-pater"><a href="out_link.aspx?returnTitle=学生贷款&returnUrl=http://xlf.mxsj888.com/a03.asp?mkid=7">
                <img src="../images/xsd.png"  class="indexIcon"/>学生贷款</a></li>

                 <%} %>

           
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
