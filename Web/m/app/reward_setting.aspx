<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="reward_setting.aspx.cs" Inherits="Web.m.app.reward_setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .list-card input {
            height: initial !important;
        }
             .indexIconleft {
            width: 120%;
            margin: 0 auto;
        }
                .p-right {
            padding-top: 5% !important;
            font-weight: bolder;
        }
                .div-collect{    font-size: 16px;
    padding: 10px 0px 5px 0px;}
                .div-money{font-size:16px;padding-bottom: 6px;}
                .list-content{padding:8px;    background-color: #f5f5f5;}
        .list-content a{background-color: #fff;    padding-top: 10px;
    padding-bottom: 5px;
    padding-left: 15px;
    border-radius: 10px;}
    </style>
    <script type="text/javascript">
        $(function () {

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-8 col-xs-8  magin10" style="height: 170px; background-color: #00B5FF; text-align: center; padding-top: 30px">
            <div style="padding-top: 10%; font-size: 16px; color: #fff;" class="text-left">
              设置学员在线缴费报名获得的100元奖励分配比例
            </div>
        </div>

        <div class="col-sm-4 col-xs-4  magin10" style="height: 170px; background-color: #00B5FF; text-align: center">
            <img src="../images/dollor.png" class="img-responsive" style="padding-top: 50px;" />
        </div>

    </div>


   <div class="row" style="margin-top:10px">
        <div class="col-sm-12 col-xs-12 text-center  bg-wh" style="padding:10px">
           股东分红比例&emsp;<input type="text" id="txt_gudong"  runat="server" class="form-control"  style="    width: 25%;    display: inline;"   require-type="decimal" require-msg="股东分红比例" />&nbsp;&nbsp;%
        </div>


          <div class="col-sm-12 col-xs-12 text-center bg-wh " style="padding:10px">
            老师分红比例&emsp;<input type="text"  id="txt_laoshi" runat="server" class="form-control"  style="    width: 25%;    display: inline;"   require-type="decimal" require-msg="老师分红比例"/>&nbsp;&nbsp;%
        </div>

    </div>

       <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">提交</a>
            </span>
        </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function setupChange() {
            if (!checkForm())
                return false;

            //查看两个比例加起来是不是大于100
            var gudong = parseFloat($("#txt_gudong").val());
            var laoshi = parseFloat($("#txt_laoshi").val());
            if (gudong + laoshi > 100) {
                layerAlert("股东和老师分红比例之和不能大于100");
                return false;
            }

            layerLoading();
            var rek = 'reward_setting';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("提交失败");
                    else if (info == "1") {  //提交成功
                        layerMsg("提交成功");
                    }
                    else if (info == "-3") {  //提交成功
                        layerMsg("股东和老师分红比例之和不能大于100");
                    }
                    else
                        layerAlert("注册失败，请重试");
                }
            });
        }

    </script>
</asp:Content>
