<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="ext_remark.aspx.cs" Inherits="Web.m.app.ext_remark" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .quickimg {
            width: 100%;
        }
    </style>
      <script type="text/javascript">
          function toHaiBao() {
              var branch = '<%=MethodHelper.CommonHelper.DESEncrypt(TModel.ID.ToString()) %>';
              var webtitle = '<%=Service.CacheService.GlobleConfig.Contacter %>';
              var isshownav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value %>';
              if(isshownav=="1")
                  window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=/share?registcode=" + branch;
              else
                  window.location ="/share?registcode=" + branch;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row topheight" onclick="toHaiBao()">
        <img class="img-responsive quickimg" src="/images/tjshow.jpg" id="imgextremark" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
