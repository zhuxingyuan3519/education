<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="exam_result.aspx.cs" Inherits="Web.m.app.exam_result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         .star-btn {
            background-color: #eaba13;
            width: 80%;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
        }

       .table-bordered>tbody>tr>td{padding:5px 0px 5px 3px;font-size:12px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>测评结果</h5>
    </div>

    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-12 col-xs-12">
               <%=EvaluationHeader.PaperCode %>
            </span>
        </div>
        <div class="marg">
            <table class="table table-bordered">
                  <tr><td>测评时间：</td><td colspan="3"><%=EvaluationHeader.EvalBeginTime %> </td></tr>
                <tr><td>考生姓名：</td><td><%=UserModel.MName %> </td><td> 考题数量：</td><td><%=EvaluationHeader.QuestionCount %></td></tr>
                  <tr><td> 答题时间：</td><td><%=AnswerTime %> </td><td>正确数量：</td><td><%=EvaluationHeader.CorrectCount %> </td></tr>
                  <tr><td> 错误数量：</td><td><%=EvaluationHeader.QuestionCount-EvaluationHeader.CorrectCount %></td><td>正确率：</td><td><%= (((double)EvaluationHeader.CorrectCount / EvaluationHeader.QuestionCount) * 100.00).ToString("F2") + "%"  %>    </td></tr>
            </table>
        </div>
    </div>


    <div class="row text-center" style="padding: 10px">
          <div class="col-sm-6 col-xs-6  hidden">
              <input type="button" class="star-btn" onclick="setupChange()" value="打印结果" />
            </div>
           <div class="col-sm-12 col-xs-12 ">
              <input type="button"  class="star-btn" data-toggle="modal" data-target="#myModal"  value="显示错误答案" />
            </div>
    </div>





     <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">

    <div id="appendView" class="modal-body" style="    padding-left: 0px; height:400px;overflow-y:scroll;    padding-right: 0px;"></div>
    <div class="clearfix"></div>
    <div class="row-fluid" style=" text-align: center" id="pageContent">
    </div>
            <div style="text-align: center; padding-top: 20px">
                <input type="button" class="btn star-btn " value="关&emsp;闭" onclick="closeModel()" style="width: 50%" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
      <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        //var closeWordLayer;
        //function showErrorAnswerList() {
            //closeWordLayer = layer.open({
            //    type: 1
            //    , content: $("#layerShowWord").html()
            //   , anim: 'up'
            //    , style: 'margin: 0 auto;top:5%; width:98%; border-radius:8px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            //});
        //}
        function closeModel() {
            $('#myModal').modal('toggle');
        }

        var paperCode = '<%=EvaluationHeader.Code %>';
        function getPageList(curr) {
            var pagesi = 10;
            //分页参数，需要传到后台的
            var param = { pageIndex: curr, pageSize: pagesi, evaluationCode: paperCode };
            //ajax获取
            $.getJSON("exam_result?action=query&rand=" + Math.random(), param, function (res) {
                var gettpl = document.getElementById('demo').innerHTML;
                laytpl(gettpl).render(res.Rows, function (html) {
                    var totalSum = "<div style='font-weight:bolder'>总错题数量：" + res.Total + "</div>";
                    if (res.Rows.length<=0)
                        $('#appendView').html(totalSum);
                    else
                        $('#appendView').html(totalSum + html);
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

        function setupChange() {
            if (!checkForm())
                return false;

            layerLoading();
            var rek = 'exam_choice';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    var data = JSON.parse(info);
                    if (data.isSuccess == "false") {
                        layerAlert(data.msg);
                    }
                    else {
                        var paperCode = data.msg.split('$')[0];
                        var paperName = data.msg.split('$')[1];
                        //提示是否确认测评
                        layer.open({
                            content: '确定进入【' + paperName + '】测评吗？'
                            , btn: ['确定', '取消']
                            , yes: function (indexly, layero) {
                                layer.close(indexly);
                                //ajax生成试卷信息
                                //********************开始*********************
                                var layerLoadingshade = layer.open({ type: 2, content: '正在生成测评试卷，请稍后' });
                                var userInfo = {
                                    type: 'createEvaluationPaper',
                                    code: paperCode
                                };
                                var resultCreate = GetAjaxString(userInfo);
                                layer.close(layerLoadingshade);
                                var dataJson = JSON.parse(resultCreate);
                                if (dataJson.isSuccess == "false") {
                                    layer.alert(dataJson.msg);
                                }
                                else {
                                    //跳转到试卷详情页面 dataJson.msg
                                    window.location = 'exam_index?code=' + paperCode;
                                }
                                //********************结束*********************
                            }, btn2: function (indexly, layero) {
                                layer.close(indexly);
                            }
                            , cancel: function (indexly) {
                                //右上角关闭回调
                                layer.close(indexly);
                            }
                        });

                    }
                }
            });
        }
    </script>
    <script id="demo" type="text/html">
        <table class="table table-bordered">
            <thead>
                <tr><td class="col-sm-4 col-xs-4">中文</td><td class="col-sm-4 col-xs-4">英文答案</td><td class="col-sm-4 col-xs-4">考生答案</td></tr>
            </thead>
            <tbody>
             {{# for (var i = 0; i<d.length; i++){ }}
                 <tr>
                     <td>{{ d[i].WordChinese }}</td>
                     <td>{{ d[i].WordEnglish.replace("&sgnquot;","'") }}</td>
                     <td>{{ d[i].Answer }}</td>
                 </tr>
                {{# } }}
            </tbody>
        </table>
    </script>
</asp:Content>
