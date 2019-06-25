<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_login.aspx.cs" Inherits="Web.m.app.user_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .reChk {
            height: 20px !important;
        }

        .text {
            height: 40px;
            width: 100%;
            text-align: center;
        }

            .text input {
                border-radius: 30px;
                background: none;
                height: 40px;
                line-height: 30px;
                width: 80%;
                text-indent: 16%;
                margin: 0px auto;
                /*color: white;*/
            }

        .marg {
            color: #555;
        }

            .marg a {
                font-size: 16px;
                color: #555;
            }

            .marg span {
                color: #555;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row " style="position: fixed">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/login_bg.jpg" class="img-responsive" id="img_bg" />
        </div>
    </div>

    <section style="padding-top: 20%;">
        <div class="row  visible-xs" style="padding-top: 10%">
            <div class="col-sm-12 col-xs-12" style="text-align: center">
                <img src="<%=Service.CacheService.GlobleConfig.Field4 %>" style="width: 20%; margin-bottom: -10px;" />
            </div>
        </div>
        <div class="row list-card" style="padding-top: 20px; padding-left: 10%; padding-right: 10%;">

            <div class="col-sm-12 col-xs-12 text" style="background: url(../images/login_1.png) no-repeat 18% center; background-size: 6%;">
                <input type="text" class="form-control" placeholder="请输入用户名" id="txt_MID" />
            </div>
            <div class="col-sm-12 col-xs-12 text" style="background: url(../images/login_2.png) no-repeat 18% center; background-size: 6%; margin-top: 20px">
                <input type="password" class="form-control" placeholder="请输入密码" id="txt_Pwd" />
            </div>

            <div class="col-sm-6 col-xs-6 marg" style="text-align: center;">
                <input type="checkbox" id="chk_rememberme" class="reChk" />
                <span>记住用户名 </span>
            </div>
            <div class="col-sm-6 col-xs-6 marg" style="text-align: left">
                <input type="checkbox" id="chkRemberPWD" class="reChk" />
                <span>记住密码 </span>
            </div>

            <div class="col-sm-12 col-xs-12 text">
                <a class="btn btn-block gree" href="javascript:void(0)" style="width: 80%; margin: 0 auto; border-radius: 50px;"
                    onclick="login()">登&emsp;录</a>
            </div>
            <div class="col-sm-12 col-xs-12 marg" style="text-align: center; margin-top: 30px">
                <a href="user_findpwd.aspx">忘记密码&ensp;</a>|<a href="user_regist.aspx">&ensp;用户注册</a>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">

        function turnUrl() {
            var redUri = localStorage.getItem("redirectUrl");
            if (redUri == null)
                window.location = 'main_mine';
            else
                window.location = redUri;
        }

        function login() {
            layerLoading(); //0代表加载的风格，支持0-2
            var addInfo = {
                type: 'Login',
                uname: $("#txt_MID").val(),
                upwd: $("#txt_Pwd").val(),
                hid_location_town: $("input[name='hid_location_town']").val(),
                hid_location_adcode: $("input[name='hid_location_adcode']").val(),
                hid_location_pointer: $("input[name='hid_location_pointer']").val()
            };
            var result = GetAjaxString(addInfo);
            if (result == "0" || result == "6") {
                closeLayerLoading();
                layerMsg("登录成功");
                saveUserName();
                saveUserPWD();
                if (result == "6") {
                    var alertInfo = '该账号还未绑定您的微信号，绑定之后可使用微信快捷登录，是否立即绑定微信';
                    layer.open({
                        content: alertInfo
                        , btn: ['立即绑定', '稍后绑定']
                        , yes: function (index, layero) {
                            //立即绑定按钮
                            var bindInfo = {
                                type: 'bingLoginWXUser'
                            };
                            var bindresult = GetAjaxString(bindInfo);
                            if (bindresult == "1") {
                                turnUrl();
                            }
                            else if(bindresult=="-2") {
                                layerAlert("绑定失败，请重试");
                            }
                              else if(bindresult=="-1") {
                                layerAlert("绑定失败，该账号已绑定其他微信");
                            }

                        }, btn2: function (index, layero) {
                            turnUrl();
                        }
                    });
                }
                else {
                   turnUrl();
                }
            }
            else if (result == "2") {
                closeLayerLoading();
                layerMsg("密码不正确");
            }
            else if (result == "1") {
                closeLayerLoading();
                layerMsg("不存在该用户名");
            }
            else if (result == "3") {
                closeLayerLoading();
                layerMsg("该账号已被禁用");
            }
            else if (result == "4") {
                closeLayerLoading();
                layerMsg("该账号未到可用时间");
            }
            else if (result == "5") {
                closeLayerLoading();
                layerMsg("该账号已过可用时间");

            }
        }
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }
        function saveUserPWD() {
            if ($("#chkRemberPWD").prop("checked")) {
                var userName = $("#txt_Pwd").val();
                localStorage.setItem("rmUserPWD", "true");
                localStorage.setItem("userPWD", userName);
            }
            else {
                localStorage.setItem("rmUserPWD", "false");
                localStorage.setItem("userPWD", "");
            }
        }

        function saveUserName() {
            if ($("#chk_rememberme").prop("checked")) {
                var userName = $("#txt_MID").val();
                localStorage.setItem("rmUser", "true");
                localStorage.setItem("userName", userName);
            }
            else {
                localStorage.setItem("rmUser", "false");
                localStorage.setItem("userName", "");
            }
        }
        $(document).ready(function () {

            var teachercode = '<%=Request.QueryString["teachercode"] %>';
            if (teachercode != "") {
                window.localStorage.setItem("teachercode", teachercode);
            }


            $("#img_bg").css("height", ($(window).height() - 35) + "px").css("width", "100%");

            if (localStorage.getItem("rmUser") == "true") {
                $("#chk_rememberme").attr("checked", true);
                $("#txt_MID").val(localStorage.getItem("userName"));
            }
            if (localStorage.getItem("rmUserPWD") == "true") {
                $("#chkRemberPWD").attr("checked", true);
                $("#txt_Pwd").val(localStorage.getItem("userPWD"));
            }
        });
    </script>
</asp:Content>
