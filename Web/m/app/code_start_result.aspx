<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="code_start_result.aspx.cs" Inherits="Web.m.app.code_start_result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .star-btn {
            background-color: #eaba13;
            width: 80%;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
        }
              .row-content {
            padding-top: 10px;
        }

        .word-content {
            width: 20% !important;
            font-weight: bolder;
        }
        .errormsg{color:red;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hid_trainCode" runat="server" />

    <div class="row" style="padding: 20px; font-size: 18px; font-weight: bolder; text-align: center">训练项目：<span id="sp_trainName" runat="server"></span></div>

    <div class="row">
        <div class="col-sm-12 col-xs-12 text-left">
            训练时间：<span id="sp_trainTime" runat="server"></span>
        </div>
        <div class="col-sm-12 col-xs-12 text-left">
            记忆数量：<span id="sp_trainCount" runat="server"></span>
        </div>

        <div class="col-sm-12 col-xs-12 text-left">
            记忆时间：<span id="sp_memoryTime" runat="server"></span>
        </div>

        <div class="col-sm-12 col-xs-12 text-left">
            复习时间：<span id="sp_reviewTime" runat="server"></span>
        </div>

        <div class="col-sm-12 col-xs-12 text-left">
            答题时间：<span id="sp_answerTime" runat="server"></span>
        </div>


        <div class="col-sm-12 col-xs-12 text-left">
            正确数量：<span id="sp_correctCount" runat="server"></span>
        </div>

        <div class="col-sm-12 col-xs-12 text-left">
            错误数量：<span id="sp_errorCount" runat="server"></span>
        </div>

        <div class="col-sm-12 col-xs-12 text-left">
            正确率：<span id="sp_correctPercent" runat="server"></span>
        </div>
    </div>



    <div class="row text-center" style="padding: 10px">
        <div class="col-sm-6 col-xs-6">
            <input type="button" id="btn_end" class="star-btn" onclick="showDetail()" value="显示详情" />
        </div>
        <div class="col-sm-6 col-xs-6">
            <input type="button" id="btn_retry" class="star-btn" style="background-color:#8BC34A;border-color:#8BC34A" onclick="reTry()" value="再来一次" />
        </div>
    </div>
    <div class="row" style="padding: 10px;    font-size: 12px;">
        提示：<br />
        &emsp;&emsp;点击右上角，向伙伴们分享你的训练成绩吧！
    </div>

    <div id="div_detail" style="padding: 10px;;display:none">
        <asp:Repeater ID="rep_list" runat="server">
            <ItemTemplate>
                <%#Container.ItemIndex%Convert.ToInt16(Eval("SplicCount"))==0?"<div class='row row-content'>":"" %>
                <div class="col-sm-2 col-xs-2 padding-left-right-0 text-center word-content" style="<%#Eval("SplicCount").ToString()=="10"?"width:10% !important":""%>">
                   <span class="<%#(Eval("Status").ToString()=="3"||Eval("Status").ToString()=="1")?"errormsg":"" %>"><%#Eval("CodeName") %></span>
                </div>
                <%#(Container.ItemIndex+1)%Convert.ToInt16(Eval("SplicCount"))==0?"</div>":"" %>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <b></b>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">

    <script type="text/javascript">

        function reTry() {
            window.location = "code_train";
        }

        function showDetail() {
            $("#div_detail").show();
        }

        $(function () {
            var errMsg = '<%=ErrMsg%>';
            if (errMsg != "") {
                layerAlert(ErrMsg);
            }
            else {

            }
        });


        function coming() {
            layerAlert("正在建设中。。。");
        }

        var dataForWeixin = {
            appId: '',
            img: 'http://jwy.0755wgwh.com/m/images/train_logo.png',
            url: window.document.location.href,
            title: '我的成绩',
            desc: '快来挑战我的记忆力吧！',
            fakeid: ''
        };
    </script>
    <script src="/common/js/weixinShare.js"></script>
</asp:Content>
