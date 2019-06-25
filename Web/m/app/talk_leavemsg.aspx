<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="talk_leavemsg.aspx.cs" Inherits="Web.m.app.talk_leavemsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          $(function () {
              setNavIndexChecked("3");
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>意见反馈</h5>
    </div>

    <div class="row">
        <div style="color: black; font-size: 14px; border-top: 2px solid #e1e1e1; padding: 10px" id="messageTitleDiv">请留下您的宝贵意见，帮助我们不断改进。<br /> 建议一经采纳，必有重奖。</div>
        <div style="padding: 10px">
            <textarea id="myEditor" style="width: 100%; height: 100px"></textarea>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" style="text-align: center">
            <input type="button" class="btn btn-info" value="发送消息"  onclick="replayMsg()"/>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function replayMsg() {
            var responsemsg = $("#myEditor").val();
            if ($.trim(responsemsg) == '') {
                layerMsg("回复内容不能为空！");
                return;
            }
            layerLoading();
            var responseInfo = {
                type: 'SendAdMessage',
                reMsg: responsemsg
            };
            var result = GetAjaxString(responseInfo);
            closeLayerLoading();
            if (result == "1") {
                layerAlert("提交成功，我们将尽快为您答复！");
                $("#myEditor").val('');
                layer.close(index);
            }
            else
                layerAlert("提交失败，请重试！");

        }
    </script>
</asp:Content>
