<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="finance_posmarket.aspx.cs" Inherits="Web.m.app.finance_posmarket" %>

<%--金融POS超市--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(function () {
             setNavIndexChecked("1");
         });
    </script>
    <link href="/css/swipebox.css" rel="stylesheet" />
    <script src="/js/ios-orientationchange-fix.js"></script>
    <script src="/js/jquery.swipebox.min.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $(".swipebox_app").swipebox();
        });    </script>
    <style type="text/css">
        .showImg{height:200px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>POS超市</h5>
    </div>
    <div class="row border-bot baguetteBoxOne gallery" style="height:100%">
        <asp:Repeater ID="repList" runat="server">
            <ItemTemplate>
                <div class="col-sm-6 col-xs-6" style="padding-top:20px;">
                    <a href="/Attachment/<%#Eval("PicUrl") %>" class="swipebox_app">
                        <img class="img-responsive showImg" src="/Attachment/<%#Eval("PicUrl") %>" alt="" />
                        </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
