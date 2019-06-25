<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="apply_up.aspx.cs" Inherits="Web.m.app.apply_up" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
      

        .btn-checked {
            background-color: #f55151;
            border-color: #f55151;
            color: white;
        }

        .padtop10 {
            padding-top: 10px;
        }

        .padding-left-right-5 {
            padding-left: 5px !important;
            padding-right: 5px !important;
        }

     

        .star-btn {
            background-color: #eaba13;
            border: #eaba13 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
            height: 40px;
            width:80%;
        }

          .learn-btn {
                background-color: #999;
            border: #999 solid 1px;
            padding: 5px 1px;
            color: white;
            border-radius: 8px;
            height: 40px;
            width:80%;
        }

       

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="row ">
        <div class="col-sm-8 col-xs-8  magin10" style="height: 170px; background-color: #00B5FF; text-align: center; padding-top: 30px">
            <div style="padding-top: 10%; font-size: 16px; color: #fff;" class="text-left">
                        在线缴费<%=shmoney %> 元成为学员，享受更优级的服务和更多奖励。
            </div>
        </div>
        <div class="col-sm-4 col-xs-4  magin10" style="height: 170px; background-color: #00B5FF; text-align: center">
            <img src="../images/dollor.png" class="img-responsive" style="padding-top: 50px;" />
        </div>
    </div>

    <input type="hidden" id="hid_mid" runat="server" />
    
     <div class="row" >
        <div class="col-sm-12 col-xs-12" style="padding:30px;text-align:center">
           确定要为账号<br /><span id="sp_mid" style="color:red;font-size:18px" runat="server"></span>
            <br />缴费升级学员吗？
        </div>
    </div>


    <div class="row" style="padding-bottom: 50px;">
          <div class="col-sm-6 col-xs-6 padding-left-right-5">
              <input type="button" class="star-btn" value="立即缴费" onclick="turnPayPage()" />
          </div>
        <div class="col-sm-6 col-xs-6 padding-left-right-5">
             <input type="button" class="learn-btn" value="我再逛逛" onclick="window.location='main_mine'" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        //跳转到支付界面
        function turnPayPage() {
            var mid = $("#hid_mid").val();
            if (mid == "") {
                layerAlert("参数错误");
                return false;
            }
            window.location = "/WXPay/JsApiPayPage?applyrole=Student&applymid="+mid;
        }
       
    </script>
</asp:Content>
