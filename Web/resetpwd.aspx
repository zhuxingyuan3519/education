<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="resetpwd.aspx.cs" Inherits="Web.resetpwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span>重置密码</span></h6>
            </div>
        </div>
    </div>
    <!--start-booking-->
    <div class="booking">
        <div class="regist_banner"></div>

        <div class="container">
            <div class="booking-main">
                <div class="booking-top">
                    <div class="col-md-12 booking-top-left">
                        <div class="booking-form">
                            <div class="b_room">
                                <div class="booking_room">
                                    <div class="reservation">
                                        <ul>
                                            <li class="span1_of_1 left">
                                                <div class="col-md-6">
                                                    <div class="book-text">
                                                        <h5>原密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_OldPwd" type="text" runat="server"  require-type="require" require-msg="原密码" />
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
                                            </li>
                                            <li class="span1_of_1 left">
                                                <div class="col-md-6">
                                                    <div class="book-text">
                                                        <h5>新密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_Password" type="password" runat="server"  require-type="require" require-msg="新密码"/>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="book-text">
                                                        <h5>确认密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_Password2" type="password" runat="server"  require-type="require" require-msg="确认密码"/>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
                                            </li>
                                            <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text" style="width: 100%; text-align: center">
                                                        <input class="btn btn-success" style="width: 90%;padding:12px"  onclick="setupChange()" type="button" value="注册" />
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
                                            </li>
                                        </ul>

                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
            function setupChange() {
                if (!checkForm())
                    return false;
                if ($.trim($("#txt_Password").val()) != $.trim($("#txt_Password2").val())) {
                    layer.msg("两次密码输入不一样！", { icon: 6 });
                }
                var loadIndex = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var userInfo = {
                    type: 'ResetPwd',
                    oldpwd: $.trim($("#txt_OldPwd").val()),
                    newpwd: $.trim($("#txt_Password").val())
                };
                var result = GetAjaxString(userInfo);
                layer.close(loadIndex);
                if (result == "0") {
                    layer.msg('密码修改成功！', { icon: 6 });
                }
                else if (result == "1") {
                    layer.msg('原始密码不正确！', { icon: 6 });
                }
                else
                    layer.alert(result, { icon: 6 });
            }
    </script>
</asp:Content>

