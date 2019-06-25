<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="ext_list.aspx.cs" Inherits="Web.m.app.ext_list" %>
<%--我的推荐奖励列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="/common/mobiledatepicker/css/mobiscroll.css" rel="stylesheet" />
    <link href="/common/mobiledatepicker/css/mobiscroll_date.css" rel="stylesheet" />
  
    <style type="text/css">
          .form-control{padding:0px}
          .padding-left-right-0 {
    padding-left: 2px !important;
    padding-right: 2px !important;
}
          .div_in_list span{padding-top:5px;}
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             var type = getUrlParam('type');
             $(function () {
                 getPageList(1);
             });
             function query() {
                 getPageList(1);
             }
             function getPageList(curr) {
                 var pagesi = 20;
                 //分页参数，需要传到后台的
                 var param = { type: 'tj', pageIndex: curr, pageSize: pagesi, nRegistBeginTime: $("#nRegistBeginTime").val(), nRegistEndTime: $("#nRegistEndTime").val(), ddl_type: $("#ddl_type").val() };
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
    <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b>我的客户

            <a href="student_add" style="    float: right;    margin-right: 10px;    font-size: 15px;    padding-top: 5px;    color: #000;">[创建会员]</a>
            <%if(TModel.RoleCode=="2F"||TModel.RoleCode=="3F"||TModel.RoleCode=="1F"){ %>
            <a href="select_member" style="    float: right;    margin-right: 10px;    font-size: 15px;    padding-top: 5px;    color: #000;">[会员查询]</a>
            <%} %>
        </h5>
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
            <option value="">-付款类型-</option>
            <option value="0">未付款</option>
            <option value="1">已付款</option>
        </select>
    </div>
    <div class="col-sm-2 col-xs-2  padding-left-right-0"  style="padding-top: 1px;" >
        <input type="button" class="btn btn-block btn-sm btn-info" style="padding:6px 10px;" onclick="query()" value="查询" />
    </div>
</div>

     <div class="row bg-wh" style="padding-left:20px;margin-top:-8px;display:none">
           <asp:Repeater ID="repTJList" runat="server">
               <ItemTemplate>
                   <h5><%#Eval("RandRemark") %>：<%#Eval("RankCount") %>
                   </h5>
               </ItemTemplate>
           </asp:Repeater>
    </div>
       
    <div id="appendView"  class="row " style="padding-top:10px"></div>
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
             <div class="col-sm-12 col-xs-12 div_in_list list-border">
				       <span class="col-sm-12 col-xs-12 text-left">  姓名：{{ d[i].NAME }}&nbsp;&nbsp; &nbsp;&nbsp; 电话：{{ d[i].MID }}</span>
				        <span class="col-sm-12 col-xs-12 text-left ">级别：{{ d[i].RoleCode }} &nbsp;&nbsp; {{ d[i].PayRemark }}：{{ d[i].CutTime }}</span>
                         <span class="col-sm-12 col-xs-12 text-left ">推荐人：{{ d[i].MTJName }} &nbsp;&nbsp; 推荐人账号：{{ d[i].MTJMID }}</span>
				    </div>
               
        {{# } }}
    </script>
</asp:Content>
