<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_regist.aspx.cs" Inherits="Web.m.app.user_regist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .form-control[disabled] {
            background-color: #5cb85c !important;
        }

        @media (min-width: 768px) and (min-width: 992px) and (min-width: 1200px) {
            .marg > img {
                width: 5% !important;
            }

            .gree {
                margin-top: 0px
            }

            .list-card {
                width: 60%;
                padding-top: 20px
            }
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var code = '<%=Request.QueryString["code"] %>';
            var teachercode = '<%=Request.QueryString["teachercode"] %>';
            if (teachercode != "") {
                window.localStorage.setItem("teachercode", teachercode);
                $("#hid_teacherMID").val(teachercode);
            }
            else {
                teachercode = window.localStorage.getItem("teachercode");
                if (teachercode != null && teachercode != '') {
                    var rek = 'user_regist';
                    //获取最后一个/和?之间的内容，就是请求的页面
                    $.ajax({
                        type: 'post',
                        url: rek + '?Action=GET',
                        data: 'mtjcode=' + teachercode,
                        success: function (info) {
                            $("#txt_TeacherMID").val(info);
                        }
                    });
                }
            }
            //alert(code);
            if (code != '') {
                window.localStorage.setItem("mtjcode", code);
                $("#txt_MTJ").val(code);
                window.location = '/m/app/main_mine';
            }
            else {
                code = window.localStorage.getItem("mtjcode");
                if (code != null && code != '') {
                    var rek = 'user_regist';
                    //获取最后一个/和?之间的内容，就是请求的页面
                    $.ajax({
                        type: 'post',
                        url: rek + '?Action=GET',
                        data: 'mtjcode=' + code,
                        success: function (info) {
                            $("#txt_MTJMID").val(info);
                        }
                    });
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" placeholder="请填写您的手机号" id="txt_MID" runat="server" require-type="phone" require-msg="手机号码" />
            </span>
        </div>
        <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
                <input type="text" class="form-control" placeholder="请填写验证码" id="VerificationCode" runat="server" require-type="require" require-msg="验证码" />
            </span>
            <span class="col-sm-6 col-xs-6 marg">
                <input type="button" class="form-control  gree" value="获取验证码" id="btnGetCode" onclick="sendCode(this)" style="padding-left: 5px" />
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input id="txt_Name" type="text" runat="server" class="form-control" placeholder="请输入您的姓名" />
            </span>
        </div>


        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input id="txt_Password" type="password" runat="server" require-type="require" require-msg="设置密码" class="form-control" placeholder="请设置密码" />

            </span>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input id="txt_Password2" type="password" runat="server" require-type="require" require-msg="确认密码" class="form-control" placeholder="请确认密码" />
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" id="txt_MTJMID" runat="server" placeholder="推荐人推荐码" require-type="require" require-msg="推荐人推荐码" />
                <input id="txt_MTJ" type="hidden" runat="server" />
            </span>
        </div>

        <div class="marg hidden">
            <span class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" id="txt_TeacherMID" runat="server" placeholder="老师推荐码" />
                <input id="hid_teacherMID" type="hidden" runat="server" />
            </span>
        </div>

        <div class="marg">
            <div class="col-sm-12 col-xs-12 marg" style="text-align: center">
                <input type="checkbox" id="chk_protrol" style="height: inherit;" />&nbsp;我同意<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal">《服务协议》</a>
            </div>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">注册</a>
            </span>
        </div>
    </div>

    <div id="div_wxInfoShow" style="display: none">
        <div class="marg">
             <span class="col-sm-12 col-xs-12 marg" style="text-align:center">
                注册成功，该账号还未绑定您的微信号，绑定之后可使用微信快捷登录，是否立即绑定微信？
            </span>
            <span class="col-sm-12 col-xs-12 marg">
                <img id="img_coverImg" runat="server" style="width: 60px; height: 60px;" class="img-responsive imgshowtitle img-circle" />
            </span>

            <span class="col-sm-12 col-xs-12 marg">
                <h4 style="color: white" id="sp_wxName" runat="server"></h4>
            </span>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">


        var clock = '';
        var nums = 120;
        var btn;
        function sendCode(thisBtn) {
            var tel = $("#txt_MID").val().trim();
            if (!checkPhoneNum(tel)) {
                layerMsg("请输入正确的手机号");
                return false;
            }
            //校验手机号
            var info = {
                type: 'sendTelCode',
                code: tel,
                sendtype: '1' //用户注册
            };
            var result = GetAjaxString(info);
            if (result == "1") {
                btn = thisBtn;
                btn.disabled = true; //将按钮置为不可点击
                btn.value = nums + '秒后可重新获取';
                //发送验证码
                layerMsg("验证码发送成功");
                clock = setInterval(doLoop, 1000); //一秒执行一次
            }
            else if (result == "2") {
                layerAlert("该手机号已被注册");
            }
            else {
                layerAlert("验证码发送失败，请重试");
            }
        }
        function doLoop() {
            nums--;
            if (nums > 0) {
                btn.value = nums + '秒后可重新获取';
            } else {
                clearInterval(clock); //清除js定时器
                btn.disabled = false;
                btn.value = '点击获取验证码';
                nums = 10; //重置时间
            }
        }

        function setupChange() {
            if (!checkForm())
                return false;
            if ($.trim($("#txt_MTJMID").val()) == '') {
                layerAlert("推荐人代码不能为空，如不知道代码请联系您的推荐人！");
                return false;
            }

            if (!$("#chk_protrol").prop("checked")) {
                layerAlert("您未同意服务协议！");
                return false;
            }

            layerLoading();
            var rek = 'user_regist';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("注册失败");
                    else if (info == "1") {  //提交成功
                        layerMsg("注册成功！！");
                        //注册成功之后需要提示是否与该微信进行绑定
                        var alertInfo = $("#div_wxInfoShow").html();
                        layer.open({
                            content: alertInfo
                            , btn: ['继续绑定', '返回首页']
                            , yes: function (index, layero) {
                                //立即绑定按钮
                                var bindInfo = {
                                    type: 'bingLoginWXUser'
                                };
                                var bindresult = GetAjaxString(bindInfo);
                                if (bindresult == "1") {
                                     window.location.href = "main_mine";
                                }
                                else if (bindresult == "-2") {
                                    layerAlert("绑定失败，请重试");
                                }
                                else if (bindresult == "-1") {
                                    layerAlert("绑定失败，该账号已绑定其他微信");
                                }

                            }, btn2: function (index, layero) {
                                window.location.href = "main_mine";
                            }
                        });


                        //setTimeout(function () {
                        //    //自动跳转到“登录页面
                        //    window.location = "apply_up";
                        //}, 2000);
                    }
                    else if (info == "2")
                        layerAlert("不存在该推荐人！");
                    else if (info == "3")
                        layerAlert("已存在该账号！");
                    else if (info == "4")
                        layerAlert("不存在该老师！");
                    else if (info == "-3")
                        layerAlert("请输入正确的验证码！");
                    else
                        layerAlert("注册失败，请重试");
                }
            });
        }

        function closeModel() {
            $('#myModal').modal('toggle');
        }
    </script>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div>
                    <%=Service.GlobleConfigService.GetWebConfig("serviceprotrol").Value %>
                </div>
                <div style="text-align: center">
                    <input type="button" class="btn btn-info btn-sm" value="关&emsp;闭" onclick="closeModel()" style="width: 30%" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
