<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_learn.aspx.cs" Inherits="Web.m.app.code_learn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .form-control {
        }

        .text-left {
            padding: 0px;
        }

        .show-content {
            font-size: 20px;
            font-weight: bolder;
        }

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
            padding-left: 2px !important;
            padding-right: 2px !important;
            font-size: 12px;
        }

        .check-item {
            font-size: 16px;
        }

        .check-control {
            zoom: 128%;
        }

        .star-btn {
            background-color: #eaba13;
            width: 90%;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
        }

        .stop-btn {
            background-color: #f55151;
            width: 90%;
            border: #f55151 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
        }

        .chengyu {
            color: #FF5722;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row  bg-wh">
        <div class="col-sm-3 col-xs-3">
            <img src="../images/train_logo.png" style="height: 50px; padding: 5px" />
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="密码速读" class="btn btn-checked learn-btn" />
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="记忆训练" class="btn learn-btn" onclick="window.location = 'code_train'" />
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="成绩查询" class="btn learn-btn" onclick="window.location = 'code_train_result'" />
        </div>
    </div>


    <div class="row padtop10">
        <div class="col-sm-3 col-xs-3 padding-left-right-5">
            <select id="ddl_CodeType" class="form-control  padding-left-right-5" onchange="changePwdType(); changeConditiona();">
                <option value="">选择密码</option>
                <option value="1">数字密码</option>
                <option value="2">字母密码</option>
                <option value="3">扑克密码</option>
                <option value="4">超级密码</option>
            </select>
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-5">
            <select id="ddl_ShowPart" class="form-control  padding-left-right-5" onchange="changeConditiona()">
                <option value="">显示范围</option>
            </select>
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-5">
            <select id="ddl_ShowType" class="form-control  padding-left-right-5" onchange="changeConditiona()">
                <option value="">显示方式</option>
                <option value="1">顺序显示</option>
                <option value="3">倒序显示</option>
                <option value="2">随机显示</option>
            </select>
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-5">
            <select id="ddl_ShowTime" class="form-control  padding-left-right-5" onchange="changeTimer()">
                <option value="">显示速度</option>
                <option value="5">5秒</option>
                <option value="3">3秒</option>
                <option value="2">2秒</option>
                <option value="1">1秒</option>
                <option value="0.8">0.8秒</option>
                <option value="0.7">0.7秒</option>
                <option value="0.6">0.6秒</option>
                <option value="0.5">0.5秒</option>
                <option value="0.3">0.3秒</option>
            </select>
        </div>
    </div>


    <div class="row " style="padding-bottom: 10px; padding-top: 10px">
        <div class="col-sm-3 col-xs-3 padtop10 padding-left-right-0 check-item">
            <input type="checkbox" id="chk_num" class="check-control" checked="checked" />
            <span id="sp_codeType">数字</span>

        </div>
        <div class="col-sm-3 col-xs-3 padtop10 padding-left-right-0 check-item">
            <input type="checkbox" id="chk_img" class="check-control" checked="checked" /><span> 图片</span>
        </div>
        <div class="col-sm-3 col-xs-3 padtop10 padding-left-right-0 check-item">
            <input type="checkbox" id="chk_word" class="check-control" checked="checked" /><span> 文字</span>

        </div>

        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" id="btn_star" class="btn star-btn" value="开始" onclick="startMemory()" />
        </div>
    </div>



    <div class="row" style="padding-top: 10px">
        <div class="col-sm-12 col-xs-12 text-center" style="padding: 2px 2px">
            <span id="div_pkCode" class="show-content text-right"></span>
            <span id="div_codeName" class="show-content text-left"></span>
        </div>
        <div class="col-sm-12 col-xs-12">
            <div id="div_codeImage" class="show-content text-center">
            </div>
        </div>
        <div class="col-sm-12 col-xs-12">
            <div id="div_codePuKeNum" class="show-content text-center chengyu">
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        $(function () {

        });

        function coming() {
            layerAlert("正在建设中。。。");
        }

        function changePwdType() {
            var cotyp = $("#ddl_CodeType").val();
            if (cotyp == "1") {
                $("#ddl_ShowPart").empty();
                var appenOption = "<option value=''>显示范围</option>";
                for (var m = 0; m < 10; m++) {
                    appenOption += "<option value='" + m + "'>" + m + "开头</option>";
                }
                $("#ddl_ShowPart").append(appenOption);
            }
            else if (cotyp == "2") {
                $("#ddl_ShowPart").empty();
                var appenOption = "<option value=''>显示范围</option>";
                for (var i = 0; i < 26; i++) {
                    var cochar = String.fromCharCode((65 + i)).toLocaleUpperCase();
                    appenOption += "<option value='" + cochar + "'>" + cochar + "开头</option>";
                }
                $("#ddl_ShowPart").append(appenOption);
            }
            else if (cotyp == "3") {
                $("#ddl_ShowPart").empty();
                var appenOption = "<option value=''>显示范围</option>";
                appenOption += "<option value='1'>黑桃</option>";
                appenOption += "<option value='2'>红桃</option>";
                appenOption += "<option value='3'>梅花</option>";
                appenOption += "<option value='4'>方片</option>";
                $("#ddl_ShowPart").append(appenOption);
            }
            else if (cotyp == "4") {
                $("#ddl_ShowPart").empty();
                var appenOption = "<option value=''>显示范围</option>";
                for (var m = 0; m < 10; m++) {
                    appenOption += "<option value='" + m + "'>" + m + "开头</option>";
                }
                $("#ddl_ShowPart").append(appenOption);
            }

        }

        function changeConditiona() {
            var codetype = $("#ddl_CodeType").val();
            if (codetype == "2") {
                $("#sp_codeType").html("字母");
            }
            pauseMemory();
        }
        var jsonArray = [];
        function getData() {
            var codeType = $("#ddl_CodeType").val();
            var codeOrder = $("#ddl_ShowType").val();
            var showTime = $("#ddl_showTime").val();
            var showPart = $("#ddl_ShowPart").val();
            if (codeType == "") { layerAlert("请选择密码"); return false; }
            if (codeOrder == "") { layerAlert("请选择显示方式"); return false; }
            if (showTime == "") { layerAlert("请选择显示速度"); return false; }
            getPageList(1, codeType, codeOrder, showPart);
            return true;
        }
        //开始记忆
        var mytime;
        function startMemory() {
            if (isLogin == "1") {
                if (roleCode == "Member") {
                    layerAlert("对不起，您没有权限，详情请咨询客服");
                    return false;
                }

                if (getData()) {
                    var timecount = $("#ddl_ShowTime").val()
                    //设置“暂停”按钮，颜色变红，文字变成暂停，onclick事件改变
                    $("#btn_star").addClass("stop-btn").removeClass("star-btn").val("暂停").attr("onclick", "pauseMemory()");
                    timecount = parseFloat(timecount) * 1000;
                    if (mytime)
                        clearInterval(mytime);
                    mytime = setInterval(function () { showme() }, timecount);
                }
            }
        }

        function changeTimer() {
            pauseMemory();
        }

        var arrk = 0, index = 0, appendImg = "";
        var jsonLength = jsonArray.length;

        function showme() {
            index = arrk;
            if (typeof (jsonArray[index]) == undefined)
                return;
            $("#div_pkCode").html(jsonArray[index].PKCode);
            if ($("#chk_img").prop("checked")) {
                $("#div_codeImage").show();
                appendImg = "<img src='/Attachment/Train_Thumb/" + jsonArray[index].CodeImage + "' />";
                $("#div_codeImage").html(appendImg);
            }
            else {
                $("#div_codeImage").hide();
            }
            if ($("#chk_word").prop("checked")) {
                $("#div_codeName").show();
                $("#div_codeName").html(jsonArray[index].CodeName);
            }
            else {
                $("#div_codeName").hide();
            }
            if ($("#chk_num").prop("checked")) {
                $("#div_pkCode").show();
                $("#div_pkCode").html(jsonArray[index].PKCode);
            }
            else {
                $("#div_pkCode").hide();
            }

            var cotyp = $("#ddl_CodeType").val();
            if (cotyp == "4") {
                $("#div_codePuKeNum").html(jsonArray[index].PuKeNum);
            }
            else {
                $("#div_codePuKeNum").html("");
            }


            arrk += 1;
            if (arrk == jsonArray.length)
                arrk = 0;
            // console.log(arrk);
        }


        //暂停记忆
        function pauseMemory() {
            if (mytime)
                clearInterval(mytime);
            //设置“开始”按钮，颜色变蓝，文字变成开始，onclick事件改变
            $("#btn_star").addClass("star-btn").removeClass("stop-btn").val("开始").attr("onclick", "startMemory()");
        }
        var lastCodeType = "", lastCodeOrder = "", lastShowPart = "";
        function getPageList(curr, codeType, codeOrder, showPart) {

            //console.log(lastCodeType);
            //console.log(codeType);
            //console.log(lastCodeOrder);
            //console.log(codeOrder);

            if (codeType == "" || codeOrder == "") {
                return;
            }
            else {
                if (codeType == lastCodeType && codeOrder == lastCodeOrder && codeOrder != "" && showPart == lastShowPart) {
                    return;
                }
                else {
                    arrk = 0;
                    lastCodeType = codeType;
                    lastCodeOrder = codeOrder;
                    lastShowPart = showPart;
                    var pagesi = 1500;
                    //分页参数，需要传到后台的
                    var param = { pageIndex: curr, pageSize: pagesi, codeType: codeType, codeOrder: codeOrder, showPart: showPart };
                    //ajax获取
                    layerLoading();
                    $.getJSON("/Handler/CodingLearning.ashx", param, function (res) {
                        var jsonObj;
                        jsonArray = [];
                        closeLayerLoading();
                        $.each(res.Rows, function (index, val) {
                            jsonObj = { "PKCode": val.PKCode, "CodeName": val.CodeName, "CodeImage": val.CodeImage, "PuKeNum": val.PuKeNum };
                            jsonArray.push(jsonObj);
                        });
                        //console.log(jsonArray.length);
                    });
                }
            }
        }
    </script>
</asp:Content>
