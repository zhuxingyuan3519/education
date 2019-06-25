<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="credit_loan.aspx.cs" Inherits="Web.m.app.credit_loan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .spanHeight {
            height:50px;
            line-height:50px;
        }
        .bankimg{height:inherit !important;padding-top:9px;}
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
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
             $.getJSON("/Handler/GetLoanPageList.ashx", param, function (res) {
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
         };
         var isnav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value%>';
         function windowopenloan(NContent, webtitle) {
             if (isnav == "1")
                 window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=" + NContent;
             else
                 window.location.href = NContent;
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>贷款办理</h5>
    </div>
    <div id="appendView" ></div>
    <div class="clearfix"></div>
        <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
         <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist"  onclick="windowopenloan('{{ d[i].NContent}}','{{ d[i].NTitle }}')" >
                <div class="clickButton" >
                    <div class="col-sm-4 col-xs-4 div_in_list">
					    <img src="/{{ d[i].Remark }}" class="img-responsive bankimg"/>
				    </div>
				    <div class="col-sm-8 col-xs-8 div_in_list">
				       <span class="col-sm-12 col-xs-12 text-left spanHeight">{{ d[i].NTitle }}</span>
				    </div>
                </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
