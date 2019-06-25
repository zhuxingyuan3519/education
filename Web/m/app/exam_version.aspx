<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="exam_version.aspx.cs" Inherits="Web.m.app.exam_version" %>

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

        .word-content {
            font-weight: bolder;
            border: #ccc 1px solid;
            padding-left: 5px;
            padding-right: 5px;
            float: left;
            margin-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>添加试卷版本</h5>
    </div>

    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <select id="ddl_version" class="form-control" runat="server" require-type="require" require-msg="教材版本" onchange="checkVersionWord()">
                </select>
            </span>
        </div>
        <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_grade" class="form-control" runat="server" require-type="require" require-msg="年级" onchange="checkVersionWord()">
                </select>
            </span>
            <span class="col-sm-6 col-xs-6 marg">
                <select id="ddl_leavel" class="form-control" runat="server" require-type="require" require-msg="学期" onchange="checkVersionWord()">
                </select>
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <select id="ddl_unit" class="form-control" runat="server" require-type="require" require-msg="章节" onchange="checkVersionWord()">
                </select>
            </span>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12 col-xs-12 text-center" style="padding-top: 10px;">
            <textarea id="txt_writeContent" runat="server" class="form-control" style="border-radius: 8px; height: 100px"></textarea>
        </div>
    </div>
    <div class="row">
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg" style="color: red">提示：请填写单词（仅填写英文字母拼写，单词与单词之间用“，”隔开）
            </span>
        </div>
    </div>

    <div class="row text-center" style="padding: 10px">
        <input type="button" id="btn_end" class="star-btn" onclick="setupChange()" value="提   交" />
    </div>
    <div class="row text-center" style="padding: 10px">
        <a href="javascript:checkWords();">查看本单元已维护单词</a>
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
    <div id="layerShowWord" style="display: none">
        <div style="width: 100%">
            <div id="div_wordUnitList" class="modal-body">
            
            </div>
            <div style="text-align: center; padding-top: 20px">
                <input type="button" class="btn star-btn " value="关&emsp;闭" onclick="closeModel()" style="width: 50%" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        var unitWords = [];
        function checkVersionWord() {
            unitWords = [];
            var version = $("#ddl_version").val();
            var grade = $("#ddl_grade").val();
            var leavel = $("#ddl_leavel").val();
            var unit = $("#ddl_unit").val();
            if (version != "" && grade != "" && leavel != "" && unit != "") {
                layerLoading();
                var userInfo = {
                    type: 'getBookVersionVsWord',
                    version: version,
                    grade: grade,
                    leavel: leavel,
                    unit: unit
                };
                var result = GetAjaxString(userInfo);
                closeLayerLoading();

                if (result != "0") {
                    var data = JSON.parse(result);
                    if (data.wordList.length > 0) {
                        unitWords = data.wordList;
                        layer.open({
                            type: 1
                           , content: $("#layerShowHtml").html()
                           , anim: 'up'
                           , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
                        });
                    }
                }
            }
        }
        var closeWordLayer;
        function checkWords() {
            var appendHtml = "<div class='row'  style='padding-left: 10px; height: 450px; overflow-y: scroll;'>";
            if (unitWords.length > 0) {
                appendHtml += "<div class='col-sm-12 col-xs-12'>本章节已录入" + unitWords.length + "个单词</div><div class='col-sm-12 col-xs-12 padding-left-right-0'>";
                $.each(unitWords, function (index, value) {
                    var resu = value.English;
                    if (resu.indexOf("|") > 0) {
                        resu = value.English.replace('|','"');
                    }
                    if (resu.indexOf("$") > 0) {
                        resu = value.English.replace("$", "'");
                    }
                    appendHtml += "<span class='word-content'>" + resu + "</span>";
                });
                appendHtml += "</div>";
            }
            else {
                appendHtml += "<div class='col-sm-12 col-xs-12'>该单元暂无老师维护单词</div>";
            }
            appendHtml += "</div>";
            $("#div_wordUnitList").html(appendHtml);
            closeWordLayer = layer.open({
                type: 1
                 , content: $("#layerShowWord").html()
                , anim: 'up'
                 , style: 'margin: 0 auto;top:5%; width:98%; border-radius:8px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            });
        }
        function closeModel() {
            layer.close(closeWordLayer);
        }

        function setupChange() {
            if (!checkForm())
                return false;
            if ($.trim($("#txt_writeContent").val()) == "") {
                layer("请填写单词");
            }
            layerLoading();
            var rek = 'exam_version';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("添加失败");
                    else if (info == "1") {  //提交成功
                        layerAlert("添加成功！");
                        setTimeout(function () {
                            window.location.reload();
                        }, 1500);

                    }
                    else
                        layerAlert("添加失败，请重试");
                }
            });
        }
    </script>
</asp:Content>
