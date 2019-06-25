<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="prize_indetail.aspx.cs" Inherits="Web.m.app.prize_indetail" %>
<%--我的红包列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/common/mobiledatepicker/css/mobiscroll.css" rel="stylesheet" />
    <link href="/common/mobiledatepicker/css/mobiscroll_date.css" rel="stylesheet" />
      <style type="text/css">
          .form-control{padding:0px}
          .padding-left-right-0 {
    padding-left: 2px !important;
    padding-right: 2px !important;
}
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             var type = getUrlParam('type');
             $(".whshow").hide();
             $(".fwshow").hide();
             $(function () {
                 getPageList(1);
             });

             function query() {
                 getPageList(1);
             }

             function getPageList(curr) {
                 var pagesi = 10;
                 //分页参数，需要传到后台的
                 var param = { pageIndex: curr, pageSize: pagesi, nRegistBeginTime: $("#nRegistBeginTime").val(), nRegistEndTime: $("#nRegistEndTime").val(), ddl_type: $("#ddl_type").val() };
                 //ajax获取
                 $.getJSON("/Handler/GetPrizeDetailPageList.ashx?rand=" + Math.random(), param, function (res) {
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
   <div class="row bg-wh">
        <h5><b>|</b>红包明细</h5>
    </div>
<div class="row">
        <div class="col-sm-3 col-xs-3 padding-left-right-0">
                <input  id="nRegistBeginTime" placeholder="-开始时间-"  type="text" class="form-control text-center"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
        </div>
    <div class="col-sm-3 col-xs-3 padding-left-right-0">
          <input  id="nRegistEndTime" placeholder="-结束时间-"  type="text" class="form-control text-center"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
    </div>
    <div class="col-sm-4 col-xs-4  padding-left-right-0">
        <select id="ddl_type" class="form-control" >
            <option value="">-红包类型-</option>
            <option value="1">游戏红包</option>
            <option value="2">代言红包</option>
            <option value="3">签到红包</option>
            <option value="4">密令答题红包</option>
        </select>
    </div>
    <div class="col-sm-2 col-xs-2  padding-left-right-0"  style="padding-top: 1px;" >
        <input type="button" class="btn btn-block btn-sm btn-info" style="padding:6px 10px;"  onclick="query()" value="查询" />
    </div>
</div>

    <div id="appendView" ></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">

      <script src="/common/mobiledatepicker/js/mobiscroll_date.js" charset="gb2312"></script>
    <script src="/common/mobiledatepicker/js/mobiscroll.js" charset="gb2312"></script>
   <script type="text/javascript">
       $(function () {
           //手机web版日期控件
           $("#nRegistBeginTime").mobiscroll($.extend(opt['date'], opt['default']));
           $("#nRegistEndTime").mobiscroll($.extend(opt['date'], opt['default']));

       });
   </script>

     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-12 col-xs-12 text-left">抽奖日期：{{d[i].CutTime }}</span>
                        <span class="col-sm-6 col-xs-6 text-left">抽中金额： ￥{{ d[i].PrizeMoney }}</span>
                          <span class="col-sm-6 col-xs-6 text-left">红包类型：{{ d[i].TypeName }}</span>
				    </div>
                </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
