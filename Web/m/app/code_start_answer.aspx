<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_start_answer.aspx.cs" Inherits="Web.m.app.code_start_answer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .star-btn {
            background-color: #eaba13;
            width: 50%;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
        }

        .row-content {
            padding-top: 10px;
        }

        .word-content {
            width: 20% !important;
            font-weight: bolder;
        }


        .num-content {
            height: 40px;
            padding-left: 0px;
            padding-right: 0px;
            text-align: center;
            padding-top: 10px;
            border-left: #00D7FF solid 1px;
            border-top: #00D7FF solid 1px;
            border-bottom: #00D7FF solid 1px;
        }

        .first-num-content {
            border-top-left-radius: 8px;
            border-bottom-left-radius: 8px;
        }

        .last-num-content {
            border-top-right-radius: 8px;
            border-bottom-right-radius: 8px;
            border-right: #00D7FF solid 1px;
        }

        .num-btn-chk {
            border: #00D7FF solid 1px;
            background-color: #00D7FF;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hid_codevalue" runat="server" />
    <input type="hidden" id="hid_trainCode" runat="server" />

    <div class="row" style="padding: 20px; font-size: 26px; font-weight: bolder; text-align: center">开始答题</div>
    <div class="row" style="padding: 0px 8px;" id="div_showNum">
        <div class="col-sm-4 col-xs-4 num-content first-num-content num-btn-chk" onclick="changeShowNum(1,this)">
            全部内容
        </div>
        <div class="col-sm-4 col-xs-4 num-content" onclick="changeShowNum(2,this)">
            随机检查
        </div>
        <div class="col-sm-4 col-xs-4 num-content last-num-content" onclick="changeShowNum(3,this);">
            复习考题
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12 col-xs-12 text-center" id="div_choiceIndex" style="padding-top: 10px; display: none">
            第<span id="sp_index"></span>个是：
        </div>
        <div class="col-sm-12 col-xs-12 text-center" style="padding-top: 10px;">
            <textarea id="txt_writeContent" class="form-control" style="border-radius: 8px;height:65px"></textarea>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12 text-left" style="padding-top: 10px; color: red">
            提示：<span id="sp_tip" runat="server"></span>之间请用空格分隔
        </div>
    </div>

    <div class="row text-center" style="padding: 10px">
        <input type="button" id="btn_end" class="star-btn" onclick="confirmAnswer()" value="提交" />
    </div>


    <div id="layerShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                复习考题
            </div>
            <div style="padding: 10px 0px 10px 0px; text-align: center; font-weight: bolder">
                <select id="ddl_reviewType"  class="form-control" style="width:80%;margin:0 auto" onchange="showFreq(this)">
                    <option value="">选择复习方式</option>
                    <option value="1">全部显示</option>
                    <option value="2">逐一显示</option>
                </select>
            </div>

            <div id="div_showFreq" style="padding: 10px 0px 10px 0px; text-align: center; font-weight: bolder;display:none">
                <select id="ddl_reviewFreq" class="form-control"  style="width:80%;margin:0 auto" onchange="setShowTime(this)">
                    <option value="0">选择切换频率</option>
                    <option value="2">2S</option>
                    <option value="3">3S</option>
                    <option value="5">5S</option>
                    <option value="8">8S</option>
                    <option value="10">10</option>
                    <option value="15">15</option>
                </select>
            </div>

            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; outline: 0;">
                <input type="button" value="确定" id="btn_signup" class="btn btn-block star-btn" style="width: 90%;margin:0 auto" onclick="turnReviewPage()" />
            </div>
        </div>
    </div>
    <input type="hidden" value="0" id="hid_setShowTime" />
    <input type="hidden" value="" id="hid_setShowType" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">

        function showFreq(obj) {
            $("#hid_setShowType").val($(obj).val());
            if ($(obj).val() == "2") {
                $(obj).parent().next().show();
            }
            else {
                $(obj).parent().next().hide();
            }
        }

        function setShowTime(obj)
        {
            $("#hid_setShowTime").val($(obj).val());
        }
        function showReview() {
            layer.open({
                type: 1
              , content: $("#layerShowHtml").html()
              , anim: 'up'
              , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            });
        }
        //跳转到复习页面
        function turnReviewPage() {
            if ($("#hid_setShowTime").val() == "0" && $("#hid_setShowType").val() == "2") {
                layerMsg("请选择切换频率");
                return false;
            }
            else if ($("#hid_setShowType").val() == "")
            {
                layerMsg("请选择复习方式");
                return false;
            }
            window.location = "code_start_train?reviewCode=" + $("#hid_trainCode").val() + "&reviewType=" + $("#ddl_reviewType").val() + "&showTime=" + $("#hid_setShowTime").val();
        }


        function GetRandomNum(Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        }
        //从json数组中删除指定顺序的对象
        function deleteObj(a) {
            $.each(jsonArray, function (n, value) {
                if (value != null && typeof (value) != undefined) {
                    if (value.Sort == a) {
                        jsonArray.splice(n, 1);
                    }
                }
            })
        }

        var jsonArray = [];
        var jsonNums = [];
        var choiceQustion;

        function changeShowNum(num, obj) {
            $("#div_showNum").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });
            $(obj).addClass("num-btn-chk");

            if (num == 2) {
                //随机抽查
                $("#txt_writeContent").val("");
                randomCheck();
            }
            else if (num == 3) {
                //复习考题
                showReview();
            }
            else if (num == 1) {
                //全部内容重置
                tainIndex = -1;
                jsonNums = [];
                answerArray = [];
                $("#txt_writeContent").val("");
                $.each(jsonArray, function (n, value) {
                    jsonNums.push(value.Sort);
                });
                $("#div_choiceIndex").hide();
            }
        }

        function randomCheck() {
            //从索引下标中随机取出来一个
            var index = GetRandomNum(0, jsonNums.length - 1);//  Math.floor((Math.random() * jsonNums.length));
            //console.log(index);
            numIndex = index;
            var randNum = jsonNums[index];
            //console.log(jsonNums);
            $("#div_choiceIndex").show();
            $("#sp_index").html(randNum);
            tainIndex = randNum;
            choiceQustion = getJsonObject(randNum);
            //deleteObj(randNum - 1);
        }
        function getJsonObject(sort) {
            var objResult;
            $.each(jsonArray, function (n, value) {
                if (value.Sort == sort) {
                    objResult = value;
                    return false;
                }
            });
            return objResult;
        }


        //结束记忆
        var tainIndex = -1, numIndex = -1;
        var answerArray = [];
        function confirmAnswer() {
            var traincode = $("#hid_trainCode").val();
            var answerContainer = $("#txt_writeContent").val();
            //校验
            //去掉空格
            answerContainer = answerContainer.replace(/\ +/g, "");
            //去掉回车换行        
            answerContainer = answerContainer.replace(/[\r\n]/g, "");
            if (tainIndex != -1) {
                //删除一个下标
                //console.log(tainIndex);
                jsonNums.splice(numIndex, 1);
                //随机抽查，一个一个训练
                var addAnswer = { "Sort": tainIndex, "Remark": answerContainer };
                //var ansindex = $.isArray(addAnswer, answerArray);
                //if (ansindex < 0)
                answerArray.push(addAnswer);
                $("#txt_writeContent").val("");
                //判断是否存在
                //console.log(JSON.stringify(answerArray));
                if (jsonNums.length > 0)
                    randomCheck();
                else {
                    //该提交了，随机遍历完了
                    if (confirm("确定要提交吗？")) {
                        var userInfo = {
                            type: 'confirmAnswer',
                            trainCode: traincode,
                            trainIndex: "0",
                            answer: JSON.stringify(answerArray)
                        };
                        layerLoading();
                        var result = GetAjaxString(userInfo);
                        closeLayerLoading();
                        if (result == "1") {
                            //跳转到答题结果页面
                            window.location = "code_start_result?traincode=" + traincode;
                        }
                    }

                }
            }
            else {
              
                //console.log(answerContainer);
                //原题字符串
                var orginQuestionString="";
                $.each(jsonArray, function (n, value) {
                    orginQuestionString+=value.CodeName;
                });
                //console.log(orginQuestionString);
                if (answerContainer.length < orginQuestionString.length) {
                    layerAlert("您的题目没答完，请核对之后再提交");
                    return false;
                }

                //return false;
                if (confirm("确定要提交吗？")) {
                    var userInfo = {
                        type: 'confirmAnswer',
                        trainCode: traincode,
                        trainIndex: tainIndex,
                        answer: answerContainer
                    };
                    layerLoading();
                    var result = GetAjaxString(userInfo);
                    closeLayerLoading();
                    if (result == "1") {
                        //跳转到答题结果页面
                        window.location = "code_start_result?traincode=" + traincode;
                    }
                }
            }
        }
        $(function () {
            var errMsg = '<%=ErrMsg%>';
            if (errMsg != "") {
                layerAlert(ErrMsg);
            }
            else {
                var jsonstring = $("#hid_codevalue").val();
                jsonArray = JSON.parse(jsonstring).CodeNames;
                $.each(jsonArray, function (n, value) {
                    jsonNums.push(value.Sort);
                });
            }
        });


            function coming() {
                layerAlert("正在建设中。。。");
            }
    </script>
</asp:Content>
