<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="to_kgj.aspx.cs" Inherits="Web.m.app.to_kgj" %>

<%--我的推荐列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       
    </style>
    <script type="text/javascript">
        var retCode = '<%=ReturnMsg %>';
        $(function () {
         
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div style="text-align: center; padding-top: 50%; font-size: 20px; color: red; font-weight: bolder;">
            卡管家访问失败！<br />
            <%=ReturnMsg %><br />

            <div style="padding-top: 20px;">
                <a class="btn btn-block btn-lg btn-success btn-pay" style="background-color: #59bb14; border-color: #59bb14;" href="course_list.aspx">购买课程</a>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
