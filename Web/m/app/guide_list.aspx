<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="guide_list.aspx.cs" Inherits="Web.m.app.guide_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          $(function () {
              setNavIndexChecked("3");
          });
    </script>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
     <script type="text/javascript">
         $(function () {
          
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row">
            <img src="../images/bg2.jpg" class="img-responsive" />
    </div>
    <div class="row">
       <ul class="list-group">
          
            <li class="list-group-item list-group-li-pater"><a href="ext_list?type=tj">
                <img src="../images/tjjlcx.png" />推荐奖励查询</a></li>

              <li class="list-group-item list-group-li-pater"><a href="ext_indetail?type=wh">
                <img src="../images/fhcx.png" />维护奖励查询</a></li>

              <li class="list-group-item list-group-li-pater <%=Session["Member"]==null?"":(((Model.Member)Session["Member"]).Learns=="service"?"":"hidden") %>"><a href="ext_indetail?type=fw">
                <img src="../images/fwjlcx.png" />服务奖励查询</a></li>

        </ul>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
   
</asp:Content>
