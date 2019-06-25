<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/SiteNoFooter.Master" AutoEventWireup="true" CodeBehind="user_findagentpwd.aspx.cs" Inherits="Web.m.app.user_findagentpwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .form-control[disabled] {
            background-color: #5cb85c !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#h5_title").html("分销商密码找回");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row list-card">
                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input type="text" class="form-control" placeholder="请输入您的手机号" id="txt_MID" runat="server" require-type="phone" require-msg="手机号码" />
                    </span>
                </div>
                <div class="marg">
                    <span class="col-sm-6 col-xs-6 marg">
                        <input type="text" class="form-control" placeholder="请填写验证码" id="VerificationCode" runat="server" require-type="require" require-msg="验证码" />
                    </span>
                    <span class="col-sm-6 col-xs-6 marg">
                        <input type="button" class="form-control btn-success" value="获取验证码" id="btnGetCode" onclick="sendCode(this)" style="padding-left: 5px" />
                    </span>
                </div>

                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input id="txt_Password" type="password" runat="server" require-type="require" require-msg="输入新密码" class="form-control" placeholder="新密码" />

                    </span>
                </div>

                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input id="txt_Password2" type="password" runat="server" require-type="require" require-msg="确认新密码" class="form-control" placeholder="确认新密码" />
                    </span>
                </div>


                <div class="marg">
                    <div class="col-sm-12 col-xs-12">
                        <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">重置密码</a>
                    </div>
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
     <script type="text/javascript">
         var clock = '';
         var nums = 120;
         var btn;
         function sendCode(thisBtn) {
             var tel = $("#txt_MID").val().trim();
             if (!tel.TryTel()) {
                 layerMsg("请输入正确的手机号");
                 return false;
             }
             //校验手机号
             var info = {
                 type: 'sendTelCode',
                 code: tel,
                 sendtype: '2' //用户找回密码
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
                 layerAlert("该手机号未注册");
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
             layerLoading();
             var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("密码重置失败，请重试");
                    else if (info == "1") {  //提交成功
                        layerAlert("密码重置成功，请牢记您的新密码！");
                        setTimeout(function () {
                            //自动跳转到“登录页面
                            window.location = "/Admin/login.aspx";
                        }, 2000);
                    }
                    else if (info == "3")
                        layerAlert("不存在该账号！");
                    else if (info == "-3")
                        layerAlert("请输入正确的验证码！");
                    else if (info == "2")
                        layerAlert("不存在该手机号码！");
                    else
                        layerAlert("密码找回失败，请重试");
                }
            });
        }

    </script>
</asp:Content>
