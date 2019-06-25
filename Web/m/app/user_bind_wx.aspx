<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_bind_wx.aspx.cs" Inherits="Web.m.app.user_bind_wx" %>

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

    <input type="hidden" id="hid_openid" runat="server" />
    <div class="row " style="padding-top: 10%">
        <div class="col-sm-12 col-xs-12" style="text-align: center">
            <img src="" id="img_wx_headimg" runat="server" style="width: 20%; margin-bottom: 10px;" />
        </div>
        <div class="col-sm-12 col-xs-12" style="text-align: center">
            <span id="sp_wxName" runat="server"></span>
        </div>
    </div>
    <div class="row">
    </div>

    <div class="row list-card" style="padding-top: 20px; padding-left: 10%; padding-right: 10%;">
        <div class="col-sm-12 col-xs-12" style="padding-top: 15px">
            已绑定账号：
        </div>

        <asp:Repeater ID="rep_bindList" runat="server">
            <ItemTemplate>
                <div class="col-sm-12 col-xs-12" style="text-align: center">
                    <%#Eval("MID") %>
                </div>
            </ItemTemplate>
        </asp:Repeater>


        <div class="col-sm-12 col-xs-12" style="padding: 15px">
            绑定新的账号：
        </div>

        <div class="col-sm-12 col-xs-12 text" style="background: url(../images/login_1.png) no-repeat 18% center; background-size: 6%;">
            <input type="text" class="form-control" placeholder="请输入用户名" id="txt_MID" />
        </div>
        <div class="col-sm-12 col-xs-12 text" style="background: url(../images/login_2.png) no-repeat 18% center; background-size: 6%; margin-top: 20px">
            <input type="password" class="form-control" placeholder="请输入密码" id="txt_Pwd" />
        </div>

        <div class="col-sm-12 col-xs-12 text" style="padding-top: 20px;">
            <a class="btn btn-block gree" href="javascript:void(0)" style="width: 80%; margin: 0 auto; border-radius: 50px;"
                onclick="login()">绑定</a>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function login() {
            layerLoading(); //0代表加载的风格，支持0-2
            var addInfo = {
                type: 'bingWXUser',
                openId: $("#hid_openid").val(),
                uname: $("#txt_MID").val(),
                upwd: $("#txt_Pwd").val()
            };
            var result = GetAjaxString(addInfo);
            if (result == "0") {
                closeLayerLoading();
                layerMsg("绑定成功");


                  var alertInfo = '绑定成功，您可绑定多个账号，继续绑定其他账号或返回首页';
                layer.open({
                    content: alertInfo
                    , btn: ['继续绑定', '返回首页']
                    , yes: function (index, layero) {
                        //立即绑定按钮
                        window.location.href = "user_bind_wx";
                    }, btn2: function (index, layero) {
                        window.location.href = "main_mine";
                    }
                });


                window.location.reload();
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
            else if (result == "6") {
                closeLayerLoading();
                layerMsg("该账号已绑定过微信");
            }
            else {
                closeLayerLoading();
                layerMsg("绑定失败，请重试");
            }
        }

        $(document).ready(function () {

        });
    </script>
</asp:Content>
