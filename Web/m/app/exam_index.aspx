<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="exam_index.aspx.cs" Inherits="Web.m.app.exam_index" %>

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
        .list-card span{    line-height: initial;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b><%=PaperName %>(<span class="spTip"><%=QuestionCount %></span>题)</h5>
    </div>

    <div class="row list-card">
            <span class="col-sm-12 col-xs-12 marg" style="color:black">
                       计时：<span id="time_d" class="time"> </span>天 
            <span id="time_h" class="time"></span>时 
            <span id="time_m" class="time"></span>分 
            <span id="time_s" class="time"></span>秒 
            </span>
    </div>


     <div id="appendView" ></div>
    <div class="clearfix"></div>
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>



     <div class="row text-center" style="padding: 10px">
         <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
        <input type="button" id="btn_end" class="star-btn" onclick="setupChange()" value="交    卷" />
            </span>
        </div>
    </div>
    <div id="layerShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                提&emsp;示
            </div>
            <div style="padding: 30px 10px 30px 10px; text-align: center; font-weight: bolder" id="div_signUpTip">
                该单元已有其他老师编辑过，查看本单元单词以防重复编辑
            </div>
            <div style="text-align: center; font-size: 18px; padding: 10px; border-radius: 15px; outline: 0;">
                <input type="button" value="查看本单元单词" id="btn_signup" class="btn btn-block btn-info" style="width: 90%" onclick="checkWords()" />
            </div>
        </div>
    </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
       <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
    <script type="text/javascript">
        var paperCode = '<%=EvaluationHeaderCode%>';
        function getCurrentPageAnswer() {
            //获取到回答的内容
            var josnArry = [];
            $(".div_questionContainer").each(function () {
                var answer = $.trim($(this).find(".txtQuestion").val());
                if (answer != '') {
                    var jsonObj = { "Code": $(this).find(".hidQCode").val(), "Answer": answer };
                    josnArry.push(jsonObj);
                }
            });
            return josnArry;
        }

        function getPageList(curr) {
            layerLoading();
            var pagesi = 1;
            //获取页面上回答的内容
            var josnArry = getCurrentPageAnswer();

            //分页参数，需要传到后台的
            var param = { pageIndex: curr, pageSize: pagesi, evaluationCode: paperCode, ans: JSON.stringify(josnArry) };
            //ajax获取
            $.getJSON("exam_index?action=query&rand=" + Math.random(), param, function (res) {
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
                    first: '下一题',
                    last: '提交',
                    prev: '上一题',
                    next: '下一题',
                    curr: curr || 1, //当前页
                    jump: function (obj, first) { //触发分页后的回调
                        if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                            getPageList(obj.curr);
                        }
                    }
                });
            });
        }

        var time_start = new Date().getTime();//设定开始时间 
        var timecount;//= $("#hid_showTime").val();// getUrlParam("showTime");

        $(function () {
            getPageList(1);
            show_time();
        });


        function setupChange() {
            layer.open({
                content: '确定提交本次测评试卷吗？'
                             , btn: ['确定', '取消']
                             , yes: function (indexly, layero) {
                                 layer.close(indexly);
                                 var layerLoadingshade = layer.open({ type: 2, content: '正在提交试卷，请稍后' });
                                 var josnArry = getCurrentPageAnswer();
                                 var param = { evaluationCode: paperCode, ans: JSON.stringify(josnArry) };
                                 var rek = 'exam_index';
                                 //获取最后一个/和?之间的内容，就是请求的页面
                                 $.ajax({
                                     type: 'post',
                                     url: rek + '?Action=ADD',
                                     data: param,
                                     success: function (info) {
                                         layer.close(layerLoadingshade);
                                         var dataJson = JSON.parse(info);
                                         if (dataJson.isSuccess == "false") {
                                             layerAlert(dataJson.msg);
                                         }
                                         else {
                                             //跳转到测评结果页面 dataJson.msg
                                             window.location = 'exam_result?code=' + dataJson.msg;
                                         }
                                     }
                                 });
                             }, btn2: function (indexly, layero) {
                                 layer.close(indexly);
                             }
                             , cancel: function (indexly) {
                                 //右上角关闭回调
                                 layer.close(indexly);
                             }
            });

        }

        function show_time() {
            var m = new Date();
            var time_end;
            if (parseInt(timecount) > 0)
                time_end = new Date(m.getTime() - 4000);
            else
                time_end = new Date(m.getTime());
            //var time_end = new Date().getTime(); //设定结束时间(等于系统当前时间) 
            //console.log(time_end);
            //计算时间差 
            var time_distance = time_end - time_start;
            if (time_distance > 0) {
                // 天时分秒换算 
                var int_day = Math.floor(time_distance / 86400000)
                time_distance -= int_day * 86400000;

                var int_hour = Math.floor(time_distance / 3600000)
                time_distance -= int_hour * 3600000;

                var int_minute = Math.floor(time_distance / 60000)
                time_distance -= int_minute * 60000;

                var int_second = Math.floor(time_distance / 1000)
                // 时分秒为单数时、前面加零 
                if (int_day < 10) {
                    int_day = "0" + int_day;
                }
                if (int_hour < 10) {
                    int_hour = "0" + int_hour;
                }
                if (int_minute < 10) {
                    int_minute = "0" + int_minute;
                }
                if (int_second < 10) {
                    int_second = "0" + int_second;
                }
                // 显示时间 
                $("#time_d").html(int_day);
                $("#time_h").html(int_hour);
                $("#time_m").html(int_minute);
                $("#time_s").html(int_second);

                setTimeout("show_time()", 1000);

            } else {
                $("#time_d").html('00');
                $("#time_h").html('00');
                $("#time_m").html('00');
                $("#time_s").html('00');

            }
        }

    </script>
    <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
                <div class="row list-card div_questionContainer">
                    <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                     <span>第{{ d[i].RowNumber }}题：</span>   <span>{{ d[i].WordChinese }}</span>
                    </span>
                </div>
               <input type="hidden" value="{{ d[i].Code }}" class="hidQCode" />
                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                     <input type="text" value="{{ d[i].Answer }}" class="form-control txtQuestion" style="width: 60%;    display: table-cell; text-align:center"  require-type="require" require-msg="英文答案"  />
                    </span>
                </div>
            </div>   
        {{# } }}
    </script>
</asp:Content>
