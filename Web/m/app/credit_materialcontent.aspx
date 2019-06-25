<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="credit_materialcontent.aspx.cs" Inherits="Web.m.app.credit_materialcontent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .topheight {
            margin-top: 20px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var paramType = getUrlParam('nid');
            var paramId = getUrlParam('id')
            if (paramType == '7') {
                $("#spLable").html("快卡口子");
                $("#bookimgtitle").attr("src", "../images/kkkz2.png");
            }
            if (paramType == '8') {
                $("#spLable").html("提额口子");
                $("#bookimgtitle").attr("src", "../images/tekz2.png");
            }
            if (paramType == '9') {
                $("#spLable").html("贷款口子");
                $("#bookimgtitle").attr("src", "../images/dkkz2.png");
            }
            if (paramType == '7' || paramType == '8' || paramType == '9') {
                //更新阅读状态
                var userInfo = {
                    type: 'UpdateRemindStatus',
                    rid: paramId
                };
                GetAjaxString(userInfo);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row personer text-center">
        <img src="../images/gl0_6.png" class="img-responsive imgshowtitle" id="bookimgtitle" />
        <h4><span id="spLable" runat="server"></span></h4>
    </div>
    <div class="row topheight">
        <h4 class="text-center" runat="server" id="divTitle"></h4>
        <div class="col-md-12 booking-top-left" runat="server" id="divContent" style="padding: 5px 10px;"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
