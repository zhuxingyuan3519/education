﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="txlist.aspx.cs" Inherits="Web.txlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/laytpl/laytpl.js"></script>
    <script src="/js/laypage/laypage.js"></script>
    <style type="text/css">
       .innername {
        float:left;margin-left:5px;
        }
		.bankimg{width:130px;height:42px;}
         .spName {
        width:20%; float:left ;text-align:center}
        .nextCount {
              width:30%; float:left ;text-align:center
        }
        .spDate{   width:25%; float:left ;text-align:center}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span>提现明细</span><a href="javascript:void(0)"  class="btn btn-success" style="float:right"  onclick="history.go(-1);">返回</a></h6>
            </div>
        </div>
    </div>

   <div class="container" style="padding-left:1px;padding-right:1px">
                    <div class="col-md-12 booking-top-left">
                                        <ul id="appendView">
    </ul>
                            </div>
                                <div class="clearfix"></div>
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  
     <script id="demo" type="text/html">
          <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text"  style="width:100%">
                                                        <div class="spName"> 提现金额</div> 
                                                             <div class="spName"> 手续费</div> 
                                                        <div class="spDate"> 申请时间</div>
                                                         <div class="nextCount">
                                                      处理状态
                                                             </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
             </li>
     {{# for (var i = 0; i<d.length; i++){ }}
          <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text"  style="width:100%;padding-left:2px">
                                                        <div class="spName">  {{ d[i].TXMoney }}</div> 
                                                            <div class="spName">  {{ d[i].FeeMoney }}</div> 
                                                        <div class="spDate">  {{ d[i].CutTime }}</div>
                                                         <div class="nextCount">
                                                        {{ d[i].Remark }}
                                                             </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
             </li>
        {{# } }}
     </script>
   <script type="text/javascript">
       $(function () {
           if (!isMobile()) {
               $(".brbetween").remove();
               $(".innername").css("padding-top", "10px");
           }
           getPageList(1);
       });
       function getPageList(curr) {
           var loadIndex = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
           var pagesi = 10;
           //分页参数，需要传到后台的
           var param = { cid: 4, pageIndex: curr, pageSize: pagesi };
           //ajax获取
           $.getJSON("Handler/GetTXList.ashx?rand=" + Math.random(), param, function (res) {
               layer.close(loadIndex);
               var gettpl = document.getElementById('demo').innerHTML;
               laytpl(gettpl).render(res.Rows, function (html) {
                   $('#appendView').html(html);
               });
               var pages = Math.ceil(res.Total / pagesi); //得到总页数
               if (isMobile()) {
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
               }
               else {
                   //显示分页
                   laypage({
                       cont: 'pageContent', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                       pages: pages, //通过后台拿到的总页数
                       skin: 'molv', //皮肤
                       curr: curr || 1, //当前页
                       jump: function (obj, first) { //触发分页后的回调
                           if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                               getPageList(obj.curr);
                           }
                       }
                   });
               }
           });
       };
   </script>
</asp:Content>
