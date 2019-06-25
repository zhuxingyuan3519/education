<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="course_detail.aspx.cs" Inherits="Web.m.app.course_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .marg {
            text-align: center !important;
        }

        img {
            width: 100% !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/mrdk.jpg" class="img-responsive" />
        </div>
    </div>
    <div class="row bg-wh">
        <h5><b>|</b><%=course.Name %></h5>
    </div>
    <div class="row" >
        <div class="col-sm-12 col-xs-12  padding-left-right-0">
            <%=course.Remark %>
        </div>
    </div>
    <div class="navbar navbar-fixed-bottom header text-center nav-head-hewhitewhitewhite" style="background-color: red;display:none">
        <a href="course_buy?course=<%=course.Code %>">
            <h5 id="h5_title" style="color:white">立即购买
            </h5>
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
