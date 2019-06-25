<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="ext_main.aspx.cs" Inherits="Web.m.app.ext_main" %>

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
                奖金余额（元）
            </div>
            <h4 style="color: #fff; font-size: 20px" class="text-left"><%=totalLeaveMoney %></h4>
            <h4 style="color: #fff; font-size: 18px" class="text-left">奖励总金额</h4>
            <h4 style="color: #fff; font-size: 20px" class="text-left"> <%=totalFHMoney %></h4>
        </div>

        <div class="col-sm-4 col-xs-4  magin10" style="height: 170px; background-color: #00B5FF; text-align: center">
            <img src="../images/dollor.png" class="img-responsive" style="padding-top: 50px;" />
        </div>
    </div>

    <div class="row hidden">
        <div class="col-sm-6 col-xs-6 text-center border-r  border-none" style="background-color: #fa785a;">
            <div class="div-collect">现金账户</div>
            <div class="div-money"><%=MTModel.MSH %></div>
        </div>
   <div class="col-sm-6 col-xs-6 text-center border-r  border-none hidden" style="background-color: #ffb847;">
            <div class="div-collect">TPay账户</div>
            <div class="div-money"><%=MTModel.MJB %></div>
        </div>
   <div class="col-sm-4 col-xs-4 text-center border-r  border-none hidden" style="background-color: #5bec9f;">
            <div class="div-collect">VPay账户</div>
            <div class="div-money"><%=MTModel.MVB %></div>
        </div>


    </div>

   <div class="row" style="margin-top:10px">
        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content" style="border-radius:15px;">
            <a href="ext_indetail" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl6.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">奖励明细</p>
                    </div>
                </div>
            </a>
        </div>


          <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content " style="border-radius:15px;">
            <a href="prize_indetail" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/hbmx.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">红包明细</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content"  style="border-radius:15px;">
            <a href="tx_apply" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/tx.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">提现</p>
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
