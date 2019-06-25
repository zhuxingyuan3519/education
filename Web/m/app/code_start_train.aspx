<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_start_train.aspx.cs" Inherits="Web.m.app.code_start_train" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hid_codevalue" runat="server" />
    <input type="hidden" id="hid_trainCode" runat="server" />
    <input type="hidden" id="hid_codeType" runat="server" />
    <input type="hidden" id="hid_showTime" runat="server" />
    <input type="hidden" id="hid_reviewcode" runat="server" />
    <div class="row" style="padding-top: 10px; font-weight: bold;">
        <div class="col-sm-12 col-xs-12">
            计时:<span id="time_d" class="time"> </span>天 
            <span id="time_h" class="time"></span>时 
            <span id="time_m" class="time"></span>分 
            <span id="time_s" class="time"></span>秒 
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12 col-xs-12 text-center" id="div_showChange" style="font-size: 32px; padding-top: 80px; padding-bottom: 50px;">
        </div>
    </div>

    <asp:Repeater ID="rep_list" runat="server">
        <ItemTemplate>
            <%#Container.ItemIndex%5==0?"<div class='row row-content'>":"" %>
            <div class="col-sm-2 col-xs-2 padding-left-right-0 text-center word-content">
                <%#Eval("CodeWord") %>
            </div>
            <%#(Container.ItemIndex+1)%5==0?"</div>":"" %>
        </ItemTemplate>
    </asp:Repeater>

    <div class="row div-content">
        <div class="col-sm-12 col-xs-12 padding-left-right-5">
        </div>
    </div>

    <div class="row text-center" style="padding: 10px">
        <input type="button" id="btn_end" class="star-btn" onclick="stopMemory()" value="记忆结束" />
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        var time_start = new Date().getTime();//设定开始时间 
        var timecount;//= $("#hid_showTime").val();// getUrlParam("showTime");
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

        //结束记忆
        function stopMemory() {
            var traincode = $("#hid_trainCode").val();
            var reviewcode = $("#hid_reviewcode").val();
            if (confirm("确定要结束记忆吗？")) {
                var userInfo = {
                    type: 'stopMemory',
                    trainCode: traincode,
                    reviewCode: reviewcode
                };
                var result = GetAjaxString(userInfo);
                if (result == "1") {
                    //跳转到开始答题页面
                    if (trainType == "3") {
                        //扑克牌是单独的答题页面
                        window.location = "code_start_answer2?traincode=" + traincode;
                    }
                    else {
                        window.location = "code_start_answer?traincode=" + traincode;
                    }
                }
                else {
                    layerAlert("操作失败，请重试");
                }
            }
        }
        var trainType = "";
        var endToStartTimer;
        $(function () {
            timecount = $("#hid_showTime").val();// getUrlParam("showTime");
            trainType = $("#hid_codeType").val();// getUrlParam("codeType");
            var errMsg = '<%=ErrMsg%>';
            if (errMsg != "") {
                layerAlert(errMsg);
            }
            else {
                if (parseInt(timecount) > 0) {
                    //设置闪现
                    $("#div_showChange").html("Ready ? Go!!!");
                    //隐藏结束记忆按钮
                    $("#btn_end").hide();
                    //设置321倒计时
                    endToStartTimer= setInterval(function () { show321() }, 1000);
                }
                else {
                    show_time();
                    $("#div_showChange").remove();
                }
            }
        });
        var endToStart = 4;
        function show321() {
            endToStart--;
            if (endToStart == 0) {
                //结束倒计时，开始闪现考题
                if (endToStartTimer)
                    clearInterval(endToStartTimer);
                show_time();
                setShowChange();
            }
            else {
                if ($("#div_showChange").find("#p_show321").length > 0) {
                    $("#p_show321").html(endToStart);
                }
                else {
                    $("#div_showChange").append("<p id='p_show321' style='color:red'>" + endToStart + "</p>");
                }
            }
        }

            //创建闪现
            var jsonArray = [];
            var mytime;
            function setShowChange() {
                var showJson = $("#hid_codevalue").val();
                var jsonValues = JSON.parse(showJson);
                jsonArray = jsonValues.CodeNames;
                //console.log(JSON.stringify(jsonArray.CodeNames));
                var timecount = $("#hid_showTime").val();//  getUrlParam("showTime");
                timecount = parseFloat(timecount) * 1000;
                if (mytime)
                    clearInterval(mytime);
                showme();
                mytime = setInterval(function () { showme(); }, timecount);
            }

            var arrk = 0, index = 0;
            var jsonLength = jsonArray.length;
            function showme() {
                //console.log(arrk);
                if (arrk >= jsonArray.length) {
                    //arrk = 0;
                    //结束了
                    $("#div_showChange").html("The end!!!");
                    $("#btn_end").show();
                    //记忆结束按钮显示出来
                    if (mytime)
                        clearInterval(mytime);
                }
                else {
                    index = arrk;
                    if (trainType == "3") {
                        //如果是扑克牌闪现训练
                        var showimgPic = "<div style='font-size:16px;text-align:left;padding-left:20%'>" + jsonArray[index].RowNumber + "</div>";
                        showimgPic += "<div class='text-center'><img src='/Attachment/Train_Thumb/" + jsonArray[index].CodeImage + "'/></div>";
                        $("#div_showChange").html(showimgPic);
                    } else {
                        //其他的训练
                        $("#div_showChange").html(jsonArray[index].CodeWord);
                    }
                    arrk += 1;
                    if (arrk == jsonArray.length) {
                        //arrk = 0;
                    }
                }
            }
         
            var trainCount = 0;
            var showCount = 0;//闪现速度，如果是0，就是不闪现


            function coming() {
                layerAlert("正在建设中。。。");
            }
    </script>
</asp:Content>
