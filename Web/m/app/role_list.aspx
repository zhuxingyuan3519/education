<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="role_list.aspx.cs" Inherits="Web.m.app.role_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .li-a {
            /*background: url(../images/more.png) right no-repeat;
            background-size: 6%;*/
            display: inline-block;
            color: #696969;
        }

        .li-name {
            font-weight: bolder;
            color: black;
            padding-left: 6px;
        }

        .li-title {
            color: black;
            font-size: 10px;
            padding: 6px;
        }

        .sp-buycount {
            float: left;
            font-size: 8px;
            color: #9d9d9d;
             padding: 6px;
        }

        .sp-fee {
            float: right;
            font-size: 8px;
            color: red;
            margin-right: 10%;
        }

        .li-row {
            border-bottom: 1px solid #d1d1d1;
            border-top: 1px solid #d1d1d1;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-top: 5px;
                background-color: #fff;
        }
        .col-sm-3, .col-xs-3{
            padding-left:5px;padding-right:5px;
        }
    </style>
    <script type="text/javascript">
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/sqjm.jpg" class="img-responsive" />
        </div>
    </div>
    <div class="row bg-wh">
        <h5><b>|</b>加盟合作&emsp;(当前级别：<span id="spRoleName" runat="server"></span>)</h5>
    </div>
    <asp:Repeater ID="rep_list" runat="server">
        <ItemTemplate>
            <a href="role_detail?code=<%#Eval("Code") %>" class="li-a">
                <div class="row li-row">
                    <div class="col-sm-3 col-xs-3 ">
                        <img src="<%#Eval("Introduce") %>" class="img-responsive" />
                    </div>
                    <div class="col-sm-9 col-xs-9 padding-left-right-0">
                        <div class="li-name"><%#Eval("Name") %></div>
                        <div class="li-title"><%#Eval("Company") %></div>
                        <div>
                            <span class="sp-buycount"><%#Eval("Status") %>人申请</span>
                            <span class="sp-fee hidden">￥<%#Eval("Remark") %></span>
                        </div>
                    </div>
                </div>
            </a>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
