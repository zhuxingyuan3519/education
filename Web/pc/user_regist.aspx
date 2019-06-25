<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master" AutoEventWireup="true" Inherits="Web.m.app.user_regist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contact" style="padding: 2em 0px;">
        <div class="container">
            <div class="contact-top heading">
                <h2>用户登录</h2>
                <p>请输入用户名/手机号和密码进行校验登录！</p>
            </div>
            <div class="contact-bottom">
                <div class="col-md-6 contact-left">
                    <input type="text" placeholder="请填写您的手机号" id="txt_MID" runat="server" require-type="phone" require-msg="手机号码" />
                    <input type="text" placeholder="请填写验证码" id="VerificationCode" runat="server" require-type="require" require-msg="验证码" style="width: 55%" />
                    <input type="button" value="获取验证码" id="btnGetCode" onclick="sendCode(this)" style="width: 44%" />
                    <input id="txt_Name" type="text" runat="server" class="hidden" placeholder="请输入您的姓名" />
                    <input id="txt_Password" type="password" runat="server" require-type="require" require-msg="设置密码" placeholder="请设置密码" />
                    <input id="txt_Password2" type="password" runat="server" require-type="require" require-msg="确认密码" placeholder="请确认密码" />
                    <input type="text" id="txt_MTJMID" runat="server" placeholder="推荐人推荐码" require-type="require" require-msg="推荐人推荐码" />
                    <input id="txt_MTJ" type="hidden" runat="server" />
                </div>
                <div class="col-md-6 contact-left">
                    <div style="height: 230px; overflow-y: scroll;"
                        id="txtProtrol">
                    </div>
                    <div style="padding: 15px 20px;">
                        <input type="checkbox" id="chk_protrol" style="height: inherit;" />&nbsp;我同意<a href="javascript:void(0)">《服务协议》</a>
                    </div>
                    <input type="button" onclick="setupChange()" value="立即注册" />
                    <input type="button" onclick="turnLogin()" value="重新登录" style="float: right;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function turnLogin() {
            window.location.href = "user_login";
        }
        $(function () {
            var protrolHtml = $("#divProtrolContainer").html();
            $("#txtProtrol").html(protrolHtml);

            var code = '<%=Request.QueryString["code"] %>';
            //alert(code);
            if (code != '') {
                window.localStorage.setItem("mtjcode", code);
                $("#txt_MTJ").val(code);
                window.location = '/website/index.html';
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
                        setTimeout(function () {
                            //自动跳转到“登录页面
                            window.location = "main_mine";
                        }, 2000);
                    }
                    else if (info == "2")
                        layerAlert("不存在该推荐人！");
                    else if (info == "3")
                        layerAlert("已存在该账号！");
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
                <div id="divProtrolContainer">
                    <%=Service.GlobleConfigService.GetWebConfig("serviceprotrol").Value %>
                </div>
                <div style="text-align: center">
                    <input type="button" class="btn btn-info btn-sm" value="关&emsp;闭" onclick="closeModel()" style="width: 30%" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
