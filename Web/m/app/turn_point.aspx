<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="turn_point.aspx.cs" Inherits="Web.m.app.turn_point" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .list-card input {
            height: initial !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10" style="height: 150px; background-color: #00B5FF; text-align: center">
            <div style="padding-top: 10%; font-size: 16px; color: #fff;">
                当前剩余VIP名额数量
            </div>
            <h4 style="color: #fff; font-size: 28px"><%=TModel.LeaveTradePoints %></h4>
        </div>
    </div>
    <div class="row bg-wh">
        <h5><b>|</b>名额转让信息  <a href="turn_list" style="    float: right;    margin-right: 10px;    font-size: 15px;    padding-top: 5px;    color: #000;">转让记录</a> </h5>
    </div>
    <div class="row list-card txcardscreen">
        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">转让账户</div>
            <div class=" col-xs-9  marg">
                <input type="text" class="form-control" id="txt_mid" name="txt_mid" require-msg="转让账户" require-type="require" />
            </div>
        </div>


        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">账户姓名</div>
            <div class=" col-xs-9  marg">
                <input type="text" class="form-control"  id="txt_mname"  name="txt_mname" />
            </div>
        </div>

        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">级别选择</div>
            <div class=" col-xs-9  marg">
                <select id="ddl_role" runat="server"  class="form-control" onchange="changeRole()"></select>
            </div>
        </div>


            <div class="marg" id="div_zone" style="display:none">
            <div class="col-sm-3 col-xs-3 margs text-left">区域选择</div>
            <div class=" col-xs-9  marg">
                <select id="ddl_zone" runat="server"  class="form-control"></select>
            </div>
        </div>


        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">转让数量</div>
            <div class=" col-xs-9  marg">
                <input type="text" class="form-control"  id="txt_count" name="txt_count" require-msg="转让数量" require-type="int" />
            </div>
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
        function changeRole()
        {
            var rolecheck = $("#ddl_role").val();
            if (rolecheck == "2F") {
                $("#div_zone").show();
            }
            else {
                $("#div_zone").hide();
            }
        }

        function setupChange() {
            if (!checkForm())
                return false;
            layerLoading();
            var rek = 'turn_point';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("转让失败，请重试");
                    else if (info == "1") {  //提交成功
                        layerAlert("转让成功！！");
                        setTimeout(function () {
                        window.location = 'turn_point';
                        }, 2000);
                    }
                    else if (info == "-1")
                        layerAlert("不存在转让账户！");
                    else if (info == "-2")
                        layerAlert("您的账号名额不足！");
                    else if (info == "-3")
                        layerAlert("请输入正确的验证码！");
                    else if (info == "-4")
                        layerAlert("转让失败，您只能转让到您名下会员！");
                    else
                        layerAlert("转让失败，请重试");
                }
            });
        }
        function alertTip() {
            layerAlert("抱歉，您的账户余额不足，请重新填写金额。");
            return false;
        }

    </script>
</asp:Content>
