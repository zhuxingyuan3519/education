<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="exam_select.aspx.cs" Inherits="Web.m.app.exam_select" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .star-btn {
            background-color: #1FC9FF;
            width: 100%;
            border: #1FC9FF solid 1px;
            padding: 8px 1px;
            color: white;
            border-radius: 8px;
        }

     .tbl-container>tbody>tr>td{font-size:12px;padding-left:0px;padding-right:0px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>测评结果查询</h5>
    </div>

    <div class="row">
        <div class="col-sm-5 col-xs-5 padding-left-right-0">
                <input  id="nMName" placeholder="学生姓名"  type="text" class="form-control text-center"  />
        </div>
    <div class="col-sm-5 col-xs-5 padding-left-right-0">
          <input  id="nTel" placeholder="家长联系电话"  type="text" class="form-control text-center"/>
    </div>
    <div class="col-sm-2 col-xs-2  padding-left-right-0"  style="padding-top: 1px;" >
        <input type="button" class="btn btn-block btn-sm btn-info" style="padding:6px 10px;"  onclick="query()" value="查询" />
    </div>
      <div id="appendView" class="col-sm-12 col-xs-12  padding-left-right-0" ></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
       <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        function getPageList(curr) {
            var pagesi = 10;
            //分页参数，需要传到后台的
            var param = { pageIndex: curr, pageSize: pagesi, nMName: $("#nMName").val(), nTel: $("#nTel").val() };
            //ajax获取
            $.getJSON("exam_select?action=query&rand=" + Math.random(), param, function (res) {
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
                    prev: '上一页',
                    next: '下一页',
                    curr: curr || 1, //当前页
                    jump: function (obj, first) { //触发分页后的回调
                        if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                            getPageList(obj.curr);
                        }
                    }
                });
            });
        }

        $(function () {
            getPageList(1);
        });
        function query() {
            getPageList(1);
        }
        function choiceEvaluation(code) {
            window.location = "exam_result?code="+code;
        }
    </script>
    <script id="demo" type="text/html">
        <table class="table table-bordered tbl-container">
            <thead>
                <tr><td class="col-sm-7 col-xs-7 ">测评名称</td><td class="col-sm-5 col-xs-5">测评时间</td></tr>
            </thead>
            <tbody>
             {{# for (var i = 0; i<d.length; i++){ }}
                 <tr>
                     <td onclick="choiceEvaluation('{{ d[i].Code }}')">{{ d[i].PaperName }}</td>
                     <td onclick="choiceEvaluation('{{ d[i].Code }}')">{{ d[i].EvalBeginTime }}</td>
                 </tr>
                {{# } }}
            </tbody>
        </table>
    </script>
</asp:Content>
