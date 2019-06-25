<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>网站后台管理系统</title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="LoginAssets/js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="LoginAssets/js/bootstrap.min.js" type="text/javascript" charset="utf-8"></script>
    <link rel="stylesheet" type="text/css" href="LoginAssets/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="LoginAssets/css/mystyle.css">

    <style type="text/css">
        * {
            font-family: 'Microsoft YaHei';
        }
        body{
            background-color:#dde1ea;
        }

        @media (min-width: 768px) and (min-width: 992px) and (min-width: 1200px) {
            .bor-circle {
                margin-left: 70px;
            }

            body {
                background-size: cover;
                background-image: url('LoginAssets/img/card.jpg');
                background-position: 0px -60px;
            }

            .bold {
                color: white;
            }
            .bor-circle{
	background-color:none;
}
        }
    </style>
    <script src="<%=ResolveClientUrl("~/Admin/js/jquery.cookie.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function Login() {
            //window.location.href = "/Admin/Default.aspx";
            //return false;
            if ($("#txtname").val() == "") {
                alert('用户不能为空');
            } else if (pwd = $("#txtpwd").val() == "") {
                alert('密码不能为空');
            }
            else if ($("#checkCode").val() == "") {
                alert('验证码不能为空');
            }
            else {
                var data = GetAdminAjaxString("Login", encodeURI($("#txtname").val()) + escape("|") + $("#txtpwd").val() + escape("|") + $("#checkCode").val() + escape("|") + window.location.href);
                switch (data) {
                    case "1":
                        alert('用户名不存在');
                        break;
                    case "2":
                        alert('密码不正确');
                        break;
                    case "3":
                        alert('验证码错误');
                        $("#imgcode").click();
                        break;
                    case "-1":
                        alert('限制登录');
                        break;
                    case "0":
                        window.location.href = "/Admin/Index";
                        break;
                    case "6"://店长登录
                        window.location.href = "/pc/Index";
                        break;
                    case "5":
                        alert('贵公司的使用期限已到，账号限制登陆，请联系您的上级分销商。');
                        break;
                    default:
                        alert('用户名或密码错误');
                        break;
                }
            }
            return false;
        }
        function keyLogin() {
            if (event.keyCode == 13)   //回车键的键值为13   
                Login();
        }
        function saveUserName() {
            if ($("#chk_rememberme").attr("checked")) {
                var userName = $("#txtname").val();
                $.cookie("rmUser", "true", { expires: 7 });//7天期限的cookie
                $.cookie("userName", userName, { expires: 7 });//7天期限的cookie
            }
            else {
                $.cookie("rmUser", "false", { expires: -1 });
                $.cookie("userName", "", { expires: -1 });
            }
        }
        $(document).ready(function () {
            if ($.cookie("rmUser") == "true") {
                $("#chk_rememberme").attr("checked", true);
                $("#txtname").val($.cookie("userName"));
            }
            $(".li-has-sub").mouseenter(function () {
                $(".ul-sub").hide();
                $(this).find(".ul-sub").show();
            });

            $(".ul-sub").mouseenter(function () {
                console.log(1);
                $(this).show();
            });
            $(".ul-sub").mouseout(function () {
                console.log(2);
                $(this).hide();
            });
        });
    </script>
</head>
<body onkeydown="keyLogin();">
    <div class="container-fluid">
        <!--头部logo-->
        <div class="row top">
            <div class="col-lg-12 col-md-12 col-sm-12 col-sm-12">
                <div class="col-lg-3 col-md-3  bor-circle" style="top: 30px; border-radius: 0px;">
                    <h3 class="text-center bold"><%=Service.CacheService.GlobleConfig.Contacter %></h3>
                </div>

            </div>
        </div>
        <!--登录部分-->
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-sm-12 bg-logn ">
                <p>&nbsp;</p>
                <div class="col-lg-3 col-md-3  bor-circle" style="top: 30px;">
                    <h3 class="text-center bold">登录中心</h3>
                    <p>&nbsp;</p>
                    <div class="input-group input-group-lg">
                        <span class="input-group-addon">
                            <img src="LoginAssets/img/username.png" /></span>
                        <input type="text" class="form-control" placeholder="用户名" id="txtname" />
                    </div>
                    <p>&nbsp;</p>
                    <div class="input-group input-group-lg">
                        <span class="input-group-addon">
                            <img src="LoginAssets/img/password.png" /></span>
                        <input type="password" class="form-control" placeholder="密码" id="txtpwd" />
                    </div>
                    <p>&nbsp;</p>

                    <div class="input-group input-group-lg ">
                        <span class="input-group-addon">
                            <img src="LoginAssets/img/confirm.png" /></span>
                        <input type="text" class="form-control " placeholder="验证码" id="checkCode"  />
                        <span class="input-group-addon" style="padding: 0px; padding-left: 2px;">
                            <img id="imgcode" src="/Admin/checkCode.aspx" onclick="this.src='/Admin/CheckCode.aspx?'+Math.random()"
                                style="cursor: pointer; height: 40px; position: relative;" /></span>
                    </div>
                    <p>&nbsp;</p>
                    <input type="button" class="btn btn-info btn-block btn-lg" value="登录" onclick="Login();" />
                    <div style="padding-top: 10px; text-align: center;"><a href="../m/app/user_findagentpwd.aspx">找回密码</a></div>
                </div>
            </div>

        </div>
    </div>
</body>

<script src="<%=ResolveClientUrl("~/Admin/pop/js/ajax.js") %>" type="text/javascript"></script>
</html>
