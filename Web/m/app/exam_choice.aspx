<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="exam_choice.aspx.cs" Inherits="Web.m.app.exam_choice" %>

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

        .tbl-container > tbody > tr > td {
            font-size: 12px;
            padding-left: 0px;
            padding-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
          <img id="img_pic" src="../images/learn_english.jpg" class="img-responsive" />
    </div>
   
         <div class="modal fade" id="myChoiceModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
    <div class="row list-card modal-body" >
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <select id="ddl_version" class="form-control" runat="server" require-type="require" require-msg="教材版本" onchange="setUnits()">
                </select>
            </span>
        </div>
        <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_grade" class="form-control" runat="server" require-type="require" require-msg="年级" onchange="setUnits()">
                </select>
            </span>
            <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_leavel" class="form-control" runat="server" require-type="require" require-msg="学期" onchange="setUnits()">
                </select>
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_unit" class="form-control" runat="server">
                    <option>章节</option>
                </select>
            </span>
              <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_unit2" class="form-control" runat="server">
                    <option>章节</option>
                </select>
            </span>
        </div>

          <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
              <input type="button" value="确     定" class="btn btn-info" style="width: 80%" onclick="setupChange()" />
            </span>
        </div>
    </div>
   </div>
    </div>

 <div class="row" style="padding:20px">
        <div class="col-sm-9 col-xs-9 padding-left-right-0">
                <input  id="nMName" placeholder="请输入学生姓名/联系方式查询测评结果"  type="text" class="form-control text-center"  />
        </div>
    <div class="col-sm-3 col-xs-3  padding-left-right-0"  style="padding-top: 1px;padding-left: 5px !important;" >
        <input type="button" class="btn btn-block btn-sm btn-info" style="padding:6px 10px;"  onclick="query()" value="查询" data-toggle="modal" data-target="#myModal"  />
    </div>
</div>



    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <a href="javascript:void(0)" data-toggle="modal" data-target="#myChoiceModal" ><img src="../images/star_exam.gif" style="width:60%;padding-top:10%" /> </a>
            </span>
        </div>
    </div>





    <div id="layerShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                选择测评试卷
            </div>
            <div style="padding: 15px; text-align: center; font-weight: bolder" class="row" id="div_signUpTip">
                <div class="marg">
                    <span class="col-sm-12 col-xs-12">
                        <select id="ddl_paperList" class="form-control" runat="server"  style="width:80%"  onchange="changePaper(this)">
                        </select>
                    </span>
                </div>
            </div>
            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; " class="row">
                 <div class="marg">
                    <span class="col-sm-12 col-xs-12">
                        </span>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hid_paperId" />
    <input type="hidden" id="hid_paperName" />

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
        function setUnits() {
            var version = $("#ddl_version").val();
            var grade = $("#ddl_grade").val();
            var leavel = $("#ddl_leavel").val();
            if (version != "" && grade != "" && leavel != "") {
                var userInfo = {
                    type: 'getUnits',
                    version: version,
                    grade: grade,
                    leavel: leavel
                };
                var result = GetAjaxString(userInfo);

                if (result != "0") {
                    var data = JSON.parse(result);
                    if (data.unitList.length > 0) {
                        var unitList = data.unitList;
                        $("#ddl_unit").empty();
                        $("#ddl_unit2").empty();
                        for (var index = 0; index < unitList.length; index++) {
                            var val = unitList[index];
                            var html = "<option value='" + val.Unit + "'>Unit " + val.Unit + "</option>";
                            $("#ddl_unit").append(html);
                            $("#ddl_unit2").append(html);
                        }
                    }
                }
            }
        }



        function changePaper(obj) {
            $("#hid_paperId").val($(obj).val());
            $("#hid_paperName").val($(obj).find("option:selected").text());
        }
        function query() {
            //closeModel();
            getPageList(1);
        }

        function showChoiceExam() {
            //layer.open({
            //    type: 1
            // , content: $("#layerShowHtml").html()
            // , anim: 'up'
            // , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            //});
        }
        function closeModel() {
            $('#myModal').modal('toggle');
        }

        function getPageList(curr) {
            var pagesi = 10;
            layerLoading();
            //分页参数，需要传到后台的
            var param = { pageIndex: curr, pageSize: pagesi, nMName: $("#nMName").val() };
            //ajax获取
            $.getJSON("exam_choice?action=query&rand=" + Math.random(), param, function (res) {
                closeLayerLoading();
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


        function setupChange() {
            //if ($("#hid_paperId").val() == "") {
            //    layerMsg("请选择测评试卷");
            //    return false;
            //}
            ////layerLoading();
            //var paperCode = $("#hid_paperId").val();
            //var paperName = $("#hid_paperName").val();
            var version = $("#ddl_version").val();
            var grade = $("#ddl_grade").val();
            var leavel = $("#ddl_leavel").val();
            var unit = $("#ddl_unit").val();
            var unit2 = $("#ddl_unit2").val();
            if (version != "" && grade != "" && leavel != "" && unit != "" && unit2 != "") {
                var paperName = $("#ddl_version option:selected").text() + $("#ddl_grade option:selected").text() + $("#ddl_leavel option:selected").text() + $("#ddl_unit option:selected").text() + "至" + $("#ddl_unit2 option:selected").text();
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
                            version: version,
                            grade: grade,
                            leavel: leavel,
                            unit1: unit,
                            unit2: unit2,
                            paperName: escape(paperName)
                        };
                        var resultCreate = GetAjaxString(userInfo);
                        layer.close(layerLoadingshade);
                        var dataJson = JSON.parse(resultCreate);
                        if (dataJson.isSuccess == "false") {
                            layerAlert(dataJson.msg);
                        }
                        else {
                            //跳转到试卷详情页面 dataJson.msg
                            window.location = 'exam_index?code=' + dataJson.msg;
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
        function choiceEvaluation(code) {
            window.location = "exam_result?code=" + code;
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
