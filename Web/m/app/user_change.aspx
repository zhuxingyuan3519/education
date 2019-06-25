<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_change.aspx.cs" Inherits="Web.m.app.user_change" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
      

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

 
    <div class="row list-card" style="padding-top: 20px; padding-left: 10%; padding-right: 10%;">
        <asp:Repeater ID="rep_bindList" runat="server">
            <ItemTemplate>
                <div class="col-sm-12 col-xs-12" style="text-align: center;padding:10px;font-size:16px">
                  <a href="javascript:changeThisMember('<%#Eval("ID") %>','<%#Eval("CurrentStatus") %>')"><%#Eval("MID") %>
                      <span><%#Eval("CurrentStatus") %></span>
                  </a>  
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div class="col-sm-12 col-xs-12 text" style="padding-top: 20px;">
            <a class="btn btn-block gree" href="user_login" style="width: 80%; margin: 0 auto; border-radius: 50px;">使用新的账号登录</a>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function changeThisMember(id,checkstatus) {
            if (checkstatus != '') {
                return false;
            }
              layerLoading(); //0代表加载的风格，支持0-2
            var addInfo = {
                type: 'changeCurrentMember',
                uid: id
            };
            var result = GetAjaxString(addInfo);
            if (result == "0") {
                closeLayerLoading();
                window.location='/m/app/main_mine';
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
    </script>
</asp:Content>
