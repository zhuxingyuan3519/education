﻿<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master" AutoEventWireup="true" Inherits="Web.m.app.ext_list" %>
<%--我的推荐列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             $(function () {
                 getPageList(1);
             });
             function getPageList(curr) {
                 var pagesi = 30;
                 //分页参数，需要传到后台的
                 var param = { cid: '', pageIndex: curr, pageSize: pagesi };
                 //ajax获取
                 $.getJSON("/Handler/GetTDPageList.ashx?rand=" + Math.random(), param, function (res) {
                     var gettpl = document.getElementById('demo').innerHTML;
                     laytpl(gettpl).render(res.Rows, function (html) {
                         $('#appendView').html(html);
                     });
                     var pages = Math.ceil(res.Total / pagesi); //得到总页数
                     //显示分页
                     laypage({
                         cont: 'pageContent', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                         pages: pages, //通过后台拿到的总页数
                         groups: 0,
                         first: false,
                         last: false,
                         curr: curr || 1, //当前页
                         jump: function (obj, first) { //触发分页后的回调
                             if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                                 getPageList(obj.curr);
                             }
                         }
                     });
                 });
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="headdings">
            <h3 class="ghj">推荐列表</h3>
            <div class="bs-example">
                <table class="table">
                    <tbody id="appendView" >
                 
                    </tbody>
                </table>
            </div>
        </div>
    </div>

        
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
		<tr>
                            <td>
                                <h4 class="head" >学员：{{ d[i].MID }}；  姓名：{{ d[i].NAME }}</h4>
                                <h4>{{ d[i].SignUpCourese }}</h4>
                                <h4>推荐人账号：{{ d[i].MTJMID }}；推荐人姓名：{{ d[i].MTJName }}</h4>
                            </td>
                            <td class="type-info">注册时间：{{ d[i].MCreateTime }}</td>
                        </tr>
        {{# } }}
    </script>
</asp:Content>
