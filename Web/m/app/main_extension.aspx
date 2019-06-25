<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="main_extension.aspx.cs" Inherits="Web.m.app.main_extension" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .list-group li a img{ width:10%;}
    </style>
    <script type="text/javascript">
        $(function () {
            setNavIndexChecked("2");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row personer text-center">
        <img src="../images/gl1.png" class="img-responsive imgshowtitle" />
        <h4>推广</h4>
    </div>
			<!--信用卡中心菜单********************-->
    <div class="row">
        <ul class="list-group">
            <li class="list-group-item list-group-li-pater"><a href="ext_remark.aspx">
                <img src="../images/gl1_1.png"  class="indexIcon"/>推广说明</a></li>
            <li class="list-group-item list-group-li-pater"><a href="ext_link.aspx">
                <img src="../images/gl1_2.png"    class="indexIcon"/>分享推广</a></li>
            <li class="list-group-item list-group-li-pater"><a href="ext_list.aspx">
                <img src="../images/gl1_3.png"  class="indexIcon"/>推广业绩</a></li>
            <li class="list-group-item list-group-li-pater"><a href="ext_indetail.aspx">
                <img src="../images/gl1_4.png"  class="indexIcon"/>收入明细</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
