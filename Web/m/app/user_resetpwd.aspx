<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_resetpwd.aspx.cs" Inherits="Web.m.app.user_resetpwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--修改密码--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>修改密码</h5>
    </div>
    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-3 col-xs-3 margs text-left">原密码</span>
            <span class="col-sm-9 col-xs-9 marg">
                <input type="password" class="form-control" id="txt_OldPwd" require-type="require" placeholder="请输入原来密码" require-msg="原密码" />
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-3 col-xs-3 margs text-left">新密码</span>
            <span class="col-sm-9 col-xs-9 marg">
                <input id="txt_Password" type="password" require-type="require" class="form-control" placeholder="请输入新密码" require-msg="新密码" />
            </span>
        </div>
        <div class="marg">
            <span class="col-sm-3 col-xs-3 margs text-left">确认新密码</span>
            <span class="col-sm-9 col-xs-9 marg">
                <input class="form-control" id="txt_Password2" type="password" require-type="require" placeholder="请确认新密码" require-msg="确认新密码" />
            </span>
        </div>
        <div class="marg">
            <div class="col-sm-12 col-xs-12 marg">
                <a class="btn btn-block gree" onclick="setupChange()" href="javascript:void(0)">提&emsp;交</a>
            </div>
        </div>
    </div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function setupChange() {
            if (!checkForm())
                return false;
            if ($.trim($("#txt_Password").val()) != $.trim($("#txt_Password2").val())) {
                layerMsg("两次密码输入不一样！");
                return false;
            }
            if ($.trim($("#txt_OldPwd").val()) == $.trim($("#txt_Password2").val())) {
                layerMsg("新密码请勿与旧密码一样！");
                return false;
            }
            layerLoading();
            var userInfo = {
                type: 'ResetPwd',
                oldpwd: $.trim($("#txt_OldPwd").val()),
                newpwd: $.trim($("#txt_Password").val())
            };
            var result = GetAjaxString(userInfo);
            closeLayerLoading();
            if (result == "0") {
                layerAlert('密码修改成功！请牢记您的新密码。');
            }
            else if (result == "1") {
                layerMsg('原始密码不正确！');
            }
            else
                layerAlert(result);
        }
    </script>
</asp:Content>
