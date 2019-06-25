<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="talk_detail.aspx.cs" Inherits="Web.m.app.talk_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          $(function () {
              setNavIndexChecked("3");
          });
    </script>
    <script type="text/javascript">
        $(function () {
            var mty = '<%=Request.QueryString["mty"]%>';
             if (mty == '1') {
                 $("#divSendInfo").hide();
                 $("#messageTitleDiv").css("border-top", "none").html("意见反馈内容：");
             }
             if (isMobile()) {
                 $("#noticeDetailDiv").addClass("noticeDetailDiv");
                 //$("#noticeDetailDiv").css("height","300px");
                 // $("#noticeDetailDiv").css({ "height": "300px", "overflow-y": "scroll","width","300px" });
             }
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>消息详情</h5>
    </div>

    <div class="row" id="divSendInfo">
        <div class="col-md-12">
            <div class="sendName" style="" id="divSender" runat="server"></div>
            <div id="divMContent" class="divMContent" runat="server" style="padding-left: 10px;"></div>
        </div>
    </div>
    <asp:Repeater ID="rep_responseMsgList" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-12" style="">
                    <div class="sendName" style=""><%#Eval("SendName") %>：<span style='float: right;font-size: 12px;'><%#Eval("CreatedTime") %></span></div>
                    <div class="divMContent" style="padding-left: 10px;"><%#Eval("Message") %></div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="row">
        <div style="color: black; font-size: 14px; border-top: 2px solid #e1e1e1; padding-top: 10px" id="messageTitleDiv">回复消息：</div>
        <div style="padding: 10px">
            <textarea id="myEditor" style="width: 100%; height: 100px"></textarea>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" style="text-align: center">
            <input type="button" class="btn btn-info" value="回复消息"  onclick="replayMsg()"/>&emsp;
           <input type="button" class="btn btn-danger" value="已阅"  onclick="setHasRead()"/>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        var args = '<%=Request.QueryString["mcode"]%>';
        var mtype = '<%=Request.QueryString["MType"]%>';
        function replayMsg() {
            var responsemsg = $("#myEditor").val();
            if ($.trim(responsemsg) == '') {
                layerMsg("回复内容不能为空！");
                return;
            }
            var responseInfo = {
                type: 'responseMsg',
                reMsg: responsemsg,
                mCode: args
            };
            layerLoading();
            var result = GetAjaxString(responseInfo);
            closeLayerLoading();
            if (result == "1") {
                layerMsg("回复成功！");
                //跳转到列表页面
                window.location = "talk.aspx";
            }
            else
                layerAlert("回复失败！");
        }
        function setHasRead() {
            var dealNoticeInfo = {
                type: 'dealNoticeInfo',
                mCode: args,
                MType: mtype
            };
            layerLoading();
            var result = GetAjaxString(dealNoticeInfo);
            closeLayerLoading();
            window.location = "talk.aspx";
        }
    </script>
</asp:Content>
