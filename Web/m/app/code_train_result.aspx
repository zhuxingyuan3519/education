<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_train_result.aspx.cs" Inherits="Web.m.app.code_train_result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .learn-btn {
            width: 90%;
            border: #999 solid 1px;
            padding: 5px 1px;
            background-color: white;
            border-radius: 8px;
        }

        .btn-checked {
            background-color: #f55151;
            border-color: #f55151;
            color: white;
        }

        .padtop10 {
            padding-top: 10px;
        }

        .padding-left-right-5 {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }
        .div_in_list{margin-top:6px;}
.div_in_list span{font-size:12px;padding-top: 5px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <div class="col-sm-3 col-xs-3">
            <img src="../images/train_logo.png" style="height: 50px; padding: 5px" />

        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="密码速读" class="btn learn-btn" onclick="window.location = 'code_learn'" />
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="记忆训练" class="btn learn-btn" onclick="window.location = 'code_train'"/>
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="成绩查询" class="btn btn-checked learn-btn" />
        </div>
    </div>

    <div class="row" style="margin-top: 8px;">
        <div class="col-sm-8 col-xs-8 padding-left-right-5">
            <select id="ddl_CodeType" class="form-control  padding-left-right-5">
                <option value="">训练项目</option>
                <option value="1">混合词训练</option>
                <option value="2">数字训练</option>
                <option value="3">扑克牌训练</option>
                <option value="4">字母训练</option>
            </select>
        </div>
        <div class="col-sm-4 col-xs-4">
            <input type="button" class="btn btn-info btn-sm" style="width: 100%;height:34px" value="查询" onclick="query()" />
        </div>
    </div>

    <div id="appendView" class="row " style="padding-top: 10px"></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        $(function () {
            getPageList(1, '');
        });
        function query() {
            //查询成绩
            var codetype = $("#ddl_CodeType").val();
            getPageList(1, codetype);
        }
        function getPageList(curr, codeType) {
            var pagesi = 20;
            //分页参数，需要传到后台的
            var param = { type: codeType, pageIndex: curr, pageSize: pagesi };
            //ajax获取
            $.getJSON("/Handler/GetTrainPageList.ashx?rand=" + Math.random(), param, function (res) {
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
 <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
             <div class="col-sm-12 col-xs-12 div_in_list list-border" onclick="window.location='code_start_result?traincode={{ d[i].Code }}'">
				       <span class="col-sm-5 col-xs-5 text-left">  训练项目:{{ d[i].CodeType }}</span>
                        <span class="col-sm-7 col-xs-7 text-left">  训练时间:{{ d[i].CutTime }}</span>
				    </div>
        {{# } }}
    </script>
</asp:Content>
