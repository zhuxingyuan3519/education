<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="guide_content.aspx.cs" Inherits="Web.m.app.guide_content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .marg {
            text-align: center !important;
        }
        img{width:100%;}
    </style>
    <script type="text/javascript">
        $(function () {
            //setNavIndexChecked(3);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b><%=webConfig.Remark %></h5>
    </div>
    <div class="row list-card">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 ">
            <%=webConfig.Value %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
