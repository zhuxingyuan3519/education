<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master" AutoEventWireup="true"  Inherits="Web.m.app.user_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contact" style="padding: 3em 0px;">
        <div class="container">
            <div class="contact-top heading">
                <h2>用户登录</h2>
                <p>请输入用户名/手机号和密码进行校验登录！</p>
            </div>
            <div class="contact-bottom">
                <div class="col-md-6 contact-left">
                    <input type="text" placeholder="请输入用户名" id="txt_MID" />
                    <input type="password" placeholder="请输入密码" id="txt_Pwd" />
                    <input type="checkbox" id="chk_rememberme" class="reChk" />
                    <span>记住用户名 </span>
                    &emsp;
                        <input type="checkbox" id="chkRemberPWD" class="reChk" />
                    <span>记住密码 </span>
                </div>
                <div class="col-md-6 contact-left">
                    <input type="button" onclick="login()" value="登  录" />
                </div>
                 <div class="col-md-6 contact-left" style="padding-top: 30px;">
                        <a href="user_findpwd.aspx">忘记密码&ensp;</a>|<a href="user_regist.aspx">&ensp;用户注册</a>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
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
            closeLayerLoading();
            if (result == "0") {
                layerMsg("登录成功");
                saveUserName();
                saveUserPWD();
                localStorage.getItem("redirectUrl");
                var redUri = localStorage.getItem("redirectUrl");
                window.location = redUri;
            }
            else if (result == "2")
                layerMsg("密码不正确");
            else if (result == "1")
                layerMsg("不存在该用户名");
            else if (result == "3")
                layerMsg("该账号已被禁用");
            else if (result == "4")
                layerMsg("该账号未到可用时间");
            else if (result == "5")
                layerMsg("该账号已过可用时间");
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
