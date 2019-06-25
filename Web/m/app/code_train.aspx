<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_train.aspx.cs" Inherits="Web.m.app.code_train" %>

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

        .check-item {
            font-size: 16px;
        }

        .check-control {
            zoom: 128%;
        }

        .star-btn {
            background-color: #eaba13;
            width: 50%;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
            height: 40px;
        }

        .content-btn {
            height: 60px;
            width: 100%;
            margin-top: 8px;
            border: #999 solid 1px;
            padding: 5px 1px;
            background-color: white;
            border-radius: 8px;
        }

        .content-btn-chk {
            height: 60px;
            width: 100%;
            margin-top: 8px;
            border: #00D7FF solid 1px;
            padding: 5px 1px;
            background-color: #00D7FF;
            border-radius: 8px;
            color: white;
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

        .show-freq {
            display: none;
        }
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
            <input type="button" value="记忆训练" class="btn btn-checked learn-btn" />
        </div>
        <div class="col-sm-3 col-xs-3 padding-left-right-0 padtop10">
            <input type="button" value="成绩查询" class="btn learn-btn" onclick="window.location = 'code_train_result'" />
        </div>
    </div>

    <div class="row bg-wh" style="margin-top: 8px;">
        <h5><b>|</b>训练内容
        </h5>
    </div>

    <div class="row div-content">
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
            <input type="button" value="速记基础训练" class="content-btn" onclick="checkTrainType(1, this)" />
        </div>
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
            <input type="button" value="速记高手训练" class="content-btn" onclick="checkTrainType(5, this)" />
        </div>
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
            <input type="button" value="数字训练" class="content-btn" onclick="checkTrainType(2, this)" />
        </div>
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
            <input type="button" value="扑克牌训练" class="content-btn" onclick="checkTrainType(3, this)" />
        </div>
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
            <input type="button" value="字母训练" class="content-btn" onclick="checkTrainType(4, this)" />
        </div>
    </div>


    <div class="row bg-wh" style="margin-top: 8px;">
        <h5><b>|</b>训练数量
        </h5>
    </div>


    <div class="row" id="div_trainNum" style="padding: 0px 8px; margin-top: 8px;">
        <div class="col-sm-2 col-xs-2 num-content first-num-content" onclick="changeTrainNum(10,this)">
            10
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeTrainNum(20,this)">
            20
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeTrainNum(30,this)">
            30
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeTrainNum(40,this)">
            40
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeTrainNum(52,this)">
            52
        </div>
        <div class="col-sm-2 col-xs-2 num-content last-num-content" onclick="changeTrainNum(108,this)">
            108
        </div>
    </div>



    <div class="row" id="div_choiceShow" style="padding: 10px">
        <input type="checkbox" id="chk_freq" onchange="changeShowStatus(this)" /><span> 闪现</span>
    </div>

    <div class="row bg-wh show-freq">
        <h5><b>|</b>切换频率
        </h5>
    </div>

    <div class="row show-freq" id="div_showNum" style="padding: 0px 8px; margin-top: 8px;">
        <div class="col-sm-2 col-xs-2 num-content first-num-content" onclick="changeShowNum(15,this)">
            15S
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeShowNum(10,this)">
            10S
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeShowNum(8,this)">
            8S
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeShowNum(5,this)">
            5S
        </div>
        <div class="col-sm-2 col-xs-2 num-content" onclick="changeShowNum(3,this)">
            3S
        </div>
        <div class="col-sm-2 col-xs-2 num-content last-num-content" onclick="changeShowNum(2,this)">
            2S
        </div>
    </div>

    <div class="row text-center" style="padding: 10px">
        <input type="button" class="star-btn" value="开始训练" onclick="start_train()" />
    </div>

     <div id="layerShowHtml" style="display: none">
        <div>
              <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 10px; text-align: center; border-bottom: 1px solid #ccc;">
                提示
            </div>
            <div id="div_showPayInfo" style="padding: 20px 10px; text-align: center; font-weight: bolder">
               
            </div>

            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; outline: 0;">
                <input type="button" value="立即开通" id="btn_signup" class="btn btn-block star-btn" style="width: 90%;margin:0 auto;height:25px" onclick="turnPayPage()" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        var checkMsg = '<%=CheckMsg%>';
        var trainType = "";
        var trainCount = 0;
        var showCount = 0;//闪现速度，如果是0，就是不闪现
        var chargeCode = "";
        //跳转到支付界面
        function turnPayPage() {
            if (chargeCode == "") {
                layerAlert("您未选择付费项目");
                return false;
            }
            window.location = "/WXPay/JsApiPayPage?chargeCode=" + chargeCode;
        }
        //显示需要付款的今天
        function showPayInfo() {
            layer.open({
                type: 1
              , content: $("#layerShowHtml").html()
              , anim: 'up'
              , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            });
        }
        //选择训练项目
        var isHasPrivage = "0";
        function checkTrainType(ctype, obj) {
            var checkItem = JSON.parse(checkMsg);
            //console.log(JSON.stringify(checkItem.CodeNames));
            //return false;
            //查看是否已拥有该权限
            var haveMsg = "";
            if (checkItem.CodeNames.length > 0) {
                $.each(checkItem.CodeNames, function (n, value) {
                    if (value.CodeType == ctype && value.IsHave == "0") {
                        isHasPrivage = "0";
                        chargeCode = value.ChargeCode;
                        haveMsg = value.HaveMsg;
                        return false;
                    }
                    isHasPrivage = "1";
                });
                if (isHasPrivage == "0") {
                    $("#div_showPayInfo").html(haveMsg);
                    showPayInfo();
                    return false;
                }
            }
            else {
                isHasPrivage = "1";
            }


            $(".div-content").find("input[type='button']").each(function () {
                $(this).removeClass("content-btn-chk");
            });
            $(obj).addClass("content-btn-chk");
            trainType = ctype;
            //ctype=3扑克牌训练的时候，必须选择闪现
            if (ctype == 3) {
                $(".show-freq").show();
                $("#div_choiceShow").hide();
            }
            else {
                $(".show-freq").hide();
                $("#div_choiceShow").show();
            }
        }
        function start_train() {
            if (trainType == "") {
                layerAlert("请选择训练内容");
                return false;
            }
            if (isHasPrivage == "0") {
                showPayInfo();
                return false;
            }


            if (trainCount == 0) {
                layerAlert("请选择训练数量");
                return false;
            }
            if (trainType == 3 && showCount == 0) {
                layerAlert("请选择闪现频率");
                return false;
            }
            window.location = "code_start_train?codeType=" + trainType + "&trainCount=" + trainCount + "&showTime=" + showCount;
        }
        function changeTrainNum(num, obj) {
            $("#div_trainNum").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });
            $(obj).addClass("num-btn-chk");
            trainCount = num;
        }

        function changeShowNum(num, obj) {
            $("#div_showNum").find("div").each(function () {
                $(this).removeClass("num-btn-chk");
            });
            $(obj).addClass("num-btn-chk");
            showCount = num;
        }

        function changeShowStatus(obj) {
            if ($(obj).prop("checked")) {
                $(".show-freq").show();
            }
            else {
                showCount = 0;
                $(".show-freq").hide();
            }
        }

        function coming() {
            layerAlert("正在建设中。。。");
        }
    </script>
</asp:Content>
