<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="video_list.aspx.cs" Inherits="Web.m.app.video_list" %>
<%--视频列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
          .form-control{padding:0px}
          .padding-left-right-0 {
    padding-left: 2px !important;
    padding-right: 2px !important;
}
        .list-border{border:none;}
          .img_coverimg{height:180px !important;width:100% !important;}
            .video_title{position: absolute;
    color: #fff;
    font-weight: bolder;
    padding: 5px 0px 0px 10px;}
                .v-remark{padding-top:5px !important;color:#131111;}
            .see-count{padding-left: 0px;    color: #9e9c9c;    font-size: 10px;text-align:center;padding-top:6px !important;}
           
             .div-playing{position: absolute;
    padding-top: 55px;
    width: 100%;
    text-align: center;}
            .div-playing img{width:72px;height:72px;}
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             var type = getUrlParam('type');
             $(function () {
                 if (type == "0")
                     $("#sp_videotype").html("免费视频");
                 else if (type == "1")
                     $("#sp_videotype").html("学员视频专区");
                 else if (type == "2")
                     $("#sp_videotype").html("VIP视频专区");
                 getPageList(1);
             });
             function query() {
                 getPageList(1);
             }
             function getPageList(curr) {
                 var pagesi = 20;
                 //分页参数，需要传到后台的
                 var param = {type:type,  pageIndex: curr, pageSize: pagesi};
                 //ajax获取
                 $.getJSON("/Handler/GetVideoPageList.ashx?rand=" + Math.random(), param, function (res) {
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
    <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b><span id="sp_videotype"></span>
        </h5>
    </div>
    <div id="appendView"  class="row " ></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
             <div class="col-sm-12 col-xs-12 div_in_list list-border">
				    <a href="video_star?code={{ d[i].Code }}">   <span class="col-sm-12 col-xs-12  padding-left-right-0">
                            <div class="video_title">{{ d[i].Title }}</div>
                      <div class="div-playing">  <img src="../images/nowplaying.png" /></div>
                        <img src="/Attachment/Video/{{ d[i].CoverImage }}"  class="img_coverimg"/></span>
				        <span class="col-sm-8 col-xs-8 text-left v-remark">{{ d[i].Remark }} </span>
                        <span class="col-sm-4 col-xs-4 text-left see-count">已有{{ d[i].Sort }}人观看 </span>
                        </a>
			</div>
        {{# } }}
    </script>
</asp:Content>
