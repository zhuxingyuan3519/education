<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="indetail.aspx.cs" Inherits="Web.indetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .innername {
        float:left;margin-left:5px;
        }
		.bankimg{width:130px;height:42px;}
         .spName {
        width:15%; float:left }
        .nextCount {
              width:60%; float:left 
        }
        .spDate{   width:25%; float:left }
    </style>
      <script src="js/laypage/laypage.js"></script>
    <script src="js/laytpl/laytpl.js"></script>
     <script type="text/javascript">
         var bank = '<%=Request.QueryString["bank"]%>';
         $(function () {
             getPageList(1);
         });
         function getPageList(curr) {
             var pagesi = 10;
             //分页参数，需要传到后台的
             var param = { cid: '', pageIndex: curr, pageSize: pagesi, bank: bank };
             //ajax获取
             $.getJSON("Handler/GetInDetailPageList.ashx?rand=" + Math.random(), param, function (res) {
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
             if (!isMobile()) {
                 $(".brbetween").remove();
                 $(".innername").css("padding-top", "10px");
             }
         };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span>财富收入明细</span><a href="javascript:void(0)"  class="btn btn-success" style="float:right"  onclick="history.go(-1);">返回</a></h6>
            </div>
        </div>
    </div>
    <!--start-booking-->
      <div class="container" style="padding-left:1px;padding-right:1px">
                    <div class="col-md-12 booking-top-left">
                                        <ul id="appendView">
                                        
                                        </ul>
                                        
                                    </div>
                                <div class="clearfix"></div>
                            </div>
      <div class="row-fluid" style="margin-top: 20px;text-align:center" id="pageContent">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        $(function () {
            if (!isMobile()) {
                $(".brbetween").remove();
                $(".innername").css("padding-top", "10px");
            }
            else {
            }
        });
    </script>
     <script id="demo" type="text/html">
          <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text"  style="width:100%">
                                                        <div class="spName"> 财富</div> 
                                                        <div class="spDate"> 获得时间</div>
                                                         <div class="nextCount">
                                                      途径
                                                             </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
             </li>
     {{# for (var i = 0; i<d.length; i++){ }}
          <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text"  style="width:100%;padding-left:2px">
                                                        <div class="spName">  {{ d[i].FHMoney }}</div> 
                                                        <div class="spDate">  {{ d[i].FHDate.substring(0,10) }}</div>
                                                         <div class="nextCount">
                                                        {{ d[i].Remark }}
                                                             </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
             </li>
        {{# } }}
     </script>
</asp:Content>
