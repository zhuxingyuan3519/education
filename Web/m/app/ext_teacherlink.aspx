<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="ext_teacherlink.aspx.cs" Inherits="Web.m.app.ext_teacherlink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
              #content {
            background-image: url('../images/haibao.jpg');
            background-repeat: repeat;
            background-size: cover;
            background-repeat-y: no-repeat;
        }

        .spName {
            padding-top: 5%;
            padding-left: 21%;
            font-size: 20px;
        }

        .spMID {
            padding-top: 7%;
            padding-left: 25%;
            font-size: 20px;
        }

        .divTel {
            bottom: 7%;
            position: fixed;
            vertical-align: bottom;
            padding-left: 29%;
            font-size: 20px;
        }

        .divImg {
            text-align: center;
            padding: 5px 0px 20px 0px;
            margin: 0 auto;
            left: 0;
            right: 0;
            margin-top: 70%;
            margin-left: 20%;
            margin-right: 20%;
            padding-bottom: 100px;
        }

        .hidden {
            display: none;
        }
         .col-xs-12,.col-sm-12 {padding-left:0px;padding-right:0px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 class="text-center hidden"  id="spLink" runat="server" ></h5>
     
    <div id="content">
            <div class="spName">
            </div>
            <div class="spMID">
            </div>
            <div class="divImg">
                   <img id="imgEcode" style="width: 70%;
    padding: 8px;
    background-color: #fff;"   src="/Handler/QRCode.ashx?mid=<%=TModel.ID %>" />
                <p style="padding-top: 10px;">
                    <strong id="spTel"  style="font-weight: bolder;color:white; font-size: 14px;">推荐码：<%=TModel!=null?TModel.MID:"" %></strong>
                </p>
            </div>
        </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
      <script type="text/javascript">
          function toHaiBao() {
              var branch = '<%=MethodHelper.CommonHelper.DESEncrypt(TModel.ID.ToString()) %>';
              var webtitle = '<%=Service.CacheService.GlobleConfig.Contacter %>';
              var isshownav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value %>';
              if (isshownav == "1")
                  window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=/share?registcode=" + branch;
              else
                  window.location = "/share?registcode=" + branch;
          }
          
  </script>
</asp:Content>
