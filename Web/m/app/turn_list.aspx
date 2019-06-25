<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="turn_list.aspx.cs" Inherits="Web.m.app.turn_list" %>
<%--我的推荐列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             $(function () {
                 getPageList(1);
             });

             function query() {
                 getPageList(1);
             }

             function getPageList(curr) {
                 var pagesi = 10;
                 //分页参数，需要传到后台的
                 var param = { pageIndex: curr, pageSize: pagesi};
                 //ajax获取
                 $.getJSON("/Handler/GetTurnListPageList.ashx?rand=" + Math.random(), param, function (res) {
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
        <h5><b>|</b>转让记录</h5>
    </div>
    <div id="appendView" ></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-6 col-xs-6 text-left">日期：{{d[i].CutTime }}</span>
                        <span class="col-sm-6 col-xs-6 text-left">数量：{{ d[i].CostCount }}</span>
                          <span class="col-sm-12 col-xs-12 text-left">转让说明：{{ d[i].Remark }}</span>
				    </div>
                </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
