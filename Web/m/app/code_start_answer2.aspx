<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_start_answer2.aspx.cs" Inherits="Web.m.app.code_start_answer2" %>

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
        .puke-type{font-size: 22px;
    margin-right: 10px;}
         .choice-puke-type{font-size: 22px;}
        #div_trainNum .num-content{padding-top:1px;}
         #div_trainType .num-content{padding-top:1px;}
        .width7{width:14.28%}
        .result-width10{width:10%;float:left;}
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
        <div class="col-sm-4 col-xs-4 num-content last-num-content" onclick="changeShowNum(3,this)">
            复习考题
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12 col-xs-12 text-center" id="div_choiceIndex" style="padding-top: 10px; display: none">
            第<span id="sp_index"></span>个是：
        </div>
    </div>

      <div class="row" id="div_trainType" style="padding:10px 10px;">
        <div class="col-sm-3 col-xs-3 num-content first-num-content" onclick="changePuKeType('1',this)">
            <span class="puke-type" style="color:black"> ♠</span>黑桃
        </div>
        <div class="col-sm-3 col-xs-3 num-content" onclick="changePuKeType('2',this)">
            <span class="puke-type" style="color:red">♥</span>红桃
        </div>
        <div class="col-sm-3 col-xs-3 num-content" onclick="changePuKeType('3',this)">
            <span class="puke-type" style="color:black">♣</span>梅花
        </div>
        <div class="col-sm-3 col-xs-3 num-content last-num-content" onclick="changePuKeType('4',this)">
            <span class="puke-type" style="color:red">♦</span>方片
        </div>
    </div>

    <div class="row puke-num"  style="padding: 10px 30px;">
        <div class="col-sm-2 col-xs-2 num-content first-num-content" onclick="changePkNum('A',this)">
            A
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changePkNum('2',this)">
            2
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changePkNum('3',this)">
            3
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changePkNum('4',this)">
            4
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changePkNum('5',this)">
            5
        </div>
        <div class="col-sm-2 col-xs-2 num-content last-num-content" onclick="changePkNum('6',this)">
            6
        </div>
    </div>


    <div class="row puke-num"  style="padding: 0px 10px;">
        <div class="col-sm-2 col-xs-2 num-content first-num-content width7" onclick="changePkNum('7',this)">
            7
        </div>
        <div class="col-sm-2 col-xs-2 num-content width7" onclick="changePkNum('8',this)">
            8
        </div>
        <div class="col-sm-2 col-xs-2 num-content width7" onclick="changePkNum('9',this)">
            9
        </div>
        <div class="col-sm-2 col-xs-2 num-content width7" onclick="changePkNum('10',this)">
            10
        </div>
        <div class="col-sm-2 col-xs-2 num-content width7" onclick="changePkNum('J',this)">
            J
        </div>
        <div class="col-sm-2 col-xs-2 num-content width7" onclick="changePkNum('Q',this)">
            Q
        </div>
           <div class="col-sm-2 col-xs-2 num-content last-num-content width7" onclick="changePkNum('K',this)">
            K
        </div>
    </div>

     <div class="row" id="div_pkResult"  style="padding: 10px 10px;text-align:center;height:100px;overflow-y:auto">
      <%--   <div class="result-width10" >
             <div>♠</div>
             <div>A</div>
             <div>✓</div>
         </div>
          <div class="result-width10" >
             <div>♥</div>
             <div>7</div>
             <div>×</div>
         </div>--%>
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
                <select id="ddl_reviewFreq" class="form-control"  style="width:80%;margin:0 auto" onchange="setShowTime(this)">
                    <option value="0">选择闪现频率</option>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function setShowTime(obj) {
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
            if ($("#hid_setShowTime").val() == "0") {
                layerMsg("请选择闪现频率");
                return false;
            }
            window.location = "code_start_train?reviewCode=" + $("#hid_trainCode").val() + "&reviewType=&showTime=" + $("#hid_setShowTime").val();
        }

        var selectPKType = '', selectPKNun = '';
        var selectPKIndex = 0;
        var choiceResult = [];
        //展示选择结果
        function showPKResult(pkType, pkNum, pkIndex) {
            //console.log("pkIndex：" + pkIndex);
            if (tainIndex == -1) {
                if (pkIndex > jsonArray.length) {
                    layerAlert("您已答题结束，请提交答卷");
                    return false;
                }
            }
            var appendPKHtml = "<div class=\"result-width10\" >";
            switch (pkType) {
                case '1': appendPKHtml += "<div class=\"choice-puke-type\"  style=\"color:black\">♠</div>"; break;
                case '2': appendPKHtml += "<div class=\"choice-puke-type\"  style=\"color:red\">♥</div>"; break;
                case '3': appendPKHtml += "<div class=\"choice-puke-type\"  style=\"color:black\">♣</div>"; break;
                case '4': appendPKHtml += "<div class=\"choice-puke-type\"   style=\"color:red\">♦</div>"; break;
            }
            appendPKHtml += "<div>" + pkNum + "</div>";
            //判断对错，根据pkIndex查找到json数组中Sort中
            var selectObj;
            if (tainIndex == -1)
                selectObj = selectJsonObjectBySort(pkIndex);
            else
                selectObj = selectJsonObjectBySort(pkIndex - 1);
            if (selectObj != null) {
                if (selectObj.CodeName == (pkType + '' + pkNum)) {
                    appendPKHtml += "<div>✓</div>";
                }
                else {
                    appendPKHtml += "<div>×</div>";
                }
            }
            else {
                appendPKHtml += "<div>×</div>";
            }
            //console.log(pkIndex);
            appendPKHtml += "</div>";
            var addSort = pkIndex;
            if (tainIndex == -1)
                addSort = pkIndex;
            else
                addSort = pkIndex-1;
            var objResult = { "Remark": pkType + '' + pkNum, "Sort": addSort };
            choiceResult.push(objResult);
            //console.log(JSON.stringify(choiceResult));
            $("#div_pkResult").append(appendPKHtml);
            //$("#div_pkResult").scrollIntoView();
            //document.getElementById("div_pkResult").scrollIntoView(false);
            $('#div_pkResult').scrollTop($('#div_pkResult')[0].scrollHeight);
            //removeAllCheckedNum();
            removeAllCheckedPKColor();
        }
        //查找json数组中Sort特定值的json
        function selectJsonObjectBySort(sort) {
            var objResult = null;
            $.each(jsonArray, function (n, value) {
                if (value.Sort == sort) {
                    objResult = value;
                    return false;
                }
            });
            return objResult;
        }
        
        function removeAllCheckedNum(){
            $(".puke-num").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });
            selectPKNun = "";
        }
            function removeAllCheckedPKColor(){
                $("#div_trainType").find("div").each(function () {
                    $(this).removeClass("num-btn-chk");
                });
                selectPKType = "";
            }

       //先选择黑红梅方，然后再选择扑克数字
        function changePkNum(pkNum, obj) {
            removeAllCheckedNum();
            if (selectPKType == '') {
                layerMsg("请先选择扑克花色");
                return false;
            }
            selectPKNun = pkNum;
            if (obj != null) {
                $(obj).addClass("num-btn-chk");
                //这一次选中之后，该校验下一张
                selectPKIndex++;
                //console.log("tainIndex："+tainIndex);
                if (tainIndex == -1) {//tainIndex=-1时，是全部抽查，选择之后加载到结果区域
                    showPKResult(selectPKType, selectPKNun, selectPKIndex);
                }
                else {
                    //changeShowNum(2, null);
                }
            }
        }
        //选择黑红梅方
        function changePuKeType(pkType, obj) {
            removeAllCheckedPKColor();
            if (obj != null) {
                $(obj).addClass("num-btn-chk");
            }
            selectPKType = pkType;
            //选择完黑红梅方之后，需要把扑克数字清空，取消已经选中的
            selectPKNun = '';
            changePkNum('',null);
        }

        //获取随机数字
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
        var orginPKJsonArray = [];
        var choiceQustion;
        //切换随机抽查或者全部抽查
        function changeShowNum(num, obj) {
            if (obj != null) {
                $("#div_showNum").find("div").each(function () {
                    $(this).removeClass("num-btn-chk");
                });
                $(obj).addClass("num-btn-chk");
            }

            if (num == 2) {//随机抽查
                if (obj != null) {
                    //console.log(selectPKIndex);
                    $("#div_pkResult").empty();
                    selectPKIndex = '';
                    selectPKNun = '';
                    selectPKNun = '';
                    tainIndex = -2;
                }
                randomCheck();
            }
            else if (num == 3) {
                //复习考题
                showReview();
            }
            else if (num == 1) {
                //全部内容重置，全部抽查
                $("#div_pkResult").empty();
                selectPKIndex = '';
                selectPKNun = '';
                selectPKNun = '';
                tainIndex = -1;
                jsonNums = [];
                answerArray = [];
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
            //tainIndex = randNum;
            selectPKIndex = randNum;
            //console.log(randNum);
            choiceQustion = getJsonObject(randNum);
            //点了随机抽查，就把下面的已选中的取消选中

            $("#div_trainType").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });
            selectPKNun = '';
            selectPKType = '';

            $(".puke-num").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });

            //changePuKeType('', null);
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
        //tainIndex=-1时，是全部抽查，tainIndex=0时候是随机抽查
        var tainIndex = -1, numIndex = -1;
        var answerArray = [];
        //t=提交试卷
        function confirmAnswer() {
            var traincode = $("#hid_trainCode").val();
            if (tainIndex != -1) {
                if (selectPKType == '' || selectPKNun == '') {
                    layerAlert("请选择扑克牌的花色或数字");
                    return false;
                }
                //删除一个下标
                //console.log(tainIndex);
                if (jsonNums.length > 0) {
                    showPKResult(selectPKType, selectPKNun, selectPKIndex);
                    jsonNums.splice(numIndex, 1);
                    randomCheck();
                }
                else {
                    //查看是否已经回答完了
                    //console.log(choiceResult.length);
                    //console.log(orginPKJsonArray.length);
                    if (choiceResult.length < orginPKJsonArray.length) {
                        layerAlert("您还没有回答完");
                        return false;
                    }
                    //该提交了，随机遍历完了
                    if (confirm("确定要提交吗？")) {
                        var userInfo = {
                            type: 'confirmAnswer',
                            trainCode: traincode,
                            trainIndex: tainIndex,
                            answer: JSON.stringify(choiceResult)
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
                //查看是否已经回答完了
                //console.log(choiceResult.length);
                //console.log(orginPKJsonArray.length);
                if (choiceResult.length < orginPKJsonArray.length) {
                    layerAlert("您还没有回答完");
                    return false;
                }
                if (confirm("确定要提交吗？")) {
                    var userInfo = {
                        type: 'confirmAnswer',
                        trainCode: traincode,
                        trainIndex: tainIndex,
                        answer: JSON.stringify(choiceResult)
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
                orginPKJsonArray = jsonNums;
            }
        });


            function coming() {
                layerAlert("正在建设中。。。");
            }
    </script>
</asp:Content>
