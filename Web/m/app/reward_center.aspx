<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="reward_center.aspx.cs" Inherits="Web.m.app.reward_center" %>

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
        function turnSetting() {
            layer.open({
                content: '选择要添加的用户类型'
                     , btn: ['股东', '老师']
                     , yes: function (index, layero) {
                         //立即绑定按钮
                         window.location = 'reward_user_setting?type=1';
                     }, no: function (index, layero) {
                         window.location = 'reward_user_setting?type=2';
                         return false;
                     }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-8 col-xs-8  magin10" style="height: 170px; background-color: #00B5FF; text-align: center; padding-top: 30px">
            <div style="padding-top: 10%; font-size: 16px; color: #fff;" class="text-left">
               设置股东及老师奖金分配比例
            </div>
        </div>

        <div class="col-sm-4 col-xs-4  magin10" style="height: 170px; background-color: #00B5FF; text-align: center">
            <img src="../images/dollor.png" class="img-responsive" style="padding-top: 50px;" />
        </div>

    </div>


   <div class="row" style="margin-top:10px">
        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content" style="border-radius:15px;">
            <a href="reward_setting" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl6.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">奖金比例</p>
                    </div>
                </div>
            </a>
        </div>


          <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content" style="border-radius:15px;">
            <a href="javascript:turnSetting()" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/hbmx.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">分润用户</p>
                    </div>
                </div>
            </a>
        </div>

    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
 
    </script>
</asp:Content>
