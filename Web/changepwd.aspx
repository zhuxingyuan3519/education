<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="changepwd.aspx.cs" Inherits="Web.changepwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span>修改密码</span></h6>
            </div>
        </div>
    </div>
    <!--start-booking-->
    <div class="booking">
        <div class="container">
            <div class="booking-main">
                <div class="booking-top">
                    <div class="col-md-12">
                        <div class="booking-form">
                            <div class="b_room">
                                <div class="booking_room">
                                    <div class="reservation">
                                        <ul>
                                            <li class="span1_of_1 left">
                                                <div class="col-md-4">
                                                    <div class="book-text">
                                                        <h5>原密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_Pwd" type="password" />
                                                    </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                    <div class="book-text">
                                                        <h5>新密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_Pwd1" type="password"/>
                                                    </div>
                                                </div>
                                                 <div class="col-md-4">
                                                    <div class="book-text">
                                                        <h5>确认密码</h5>
                                                    </div>
                                                    <div class="book_date">
                                                        <input id="txt_Pwd2" type="password"/>
                                                    </div>
                                                </div>

                                                <div class="clearfix"></div>
                                            </li>
                                            <li class="span1_of_1 left">
                                                <div class="col-md-12">
                                                    <div class="book-text" style="width: 100%; text-align: center">
                                                        <input class="btn btn-success" style="width: 30%" onclick="resetPwd()" type="button" value="确认修改" />
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
        function resetPwd() {
            if ($("#txt_Pwd1").val().trim() != $("#txt_Pwd2").val().trim())
            {
                layer.alert("两次密码输入不正确");
                return false;
            }
            var addInfo = {
                type: 'resetpwd',
                pwd: $("#txt_Pwd").val(),
                pwd2: $("#txt_Pwd2").val()
            };
            var result = GetAjaxString(addInfo);
            if (result == "0") {
                layer.msg("修改成功");
            }
            else if (result == "2")
                layer.msg("原始密码不正确");
            else if (result == "-1")
                layer.msg("不存在该用户名");
        }
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }
    </script>
</asp:Content>


