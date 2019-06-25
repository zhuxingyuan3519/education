<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="course_list.aspx.cs" Inherits="Web.m.app.course_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
    <style type="text/css">
        .li-a {
      width: 9%;    display: inline;padding: 5px;
        }
        .li-name{font-weight:bolder;color:black;padding:6px;}
          .li-title{width: 7%;    display: inline;    float: right;padding-top: 5px;}
        
           .li-row{margin-right:3px;    border-bottom: 1px solid #d1d1d1;
    border-top: 1px solid #d1d1d1;padding-top: 5px;background-color: #fff;
    padding-bottom: 5px;margin-top: 5px;}
    </style>
     <script type="text/javascript">
         var paramType = "";
         $(function () {
             paramType = getUrlParam('nid')
             getPageList(1, paramType);
         });

         function getPageList(curr, paramid) {
             var pagesi = 10;
             //分页参数，需要传到后台的
             var param = { pageIndex: curr, pageSize: pagesi };
             //ajax获取
             $.getJSON("/Handler/GetCoursePageList.ashx", param, function (res) {
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
                             getPageList(obj.curr, paramid);
                         }
                     }
                 });
             });
         };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/mrdk.jpg" class="img-responsive" />
        </div>
    </div>
         <div class="row bg-wh">
        <h5><b>|</b>名人大咖</h5>
    </div>
    <div class="row">
        <div id="appendView">
             
        </div>
    </div>
        <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
             <a href="course_detail?code={{ d[i].Code}}" >
                  <div class="row li-row">
                         <div class="col-sm-12 col-xs-12">
                              <img src="../images/mrdk_logo.png" class="img-responsive li-a"  /> 
                              {{ d[i].Name }} 
                              <img src="../images/more.png" class="img-responsive li-title"  />  
                         </div>
                  </div>
                </a>
        {{# } }}
     </script>
</asp:Content>
