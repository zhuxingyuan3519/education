<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="coming.aspx.cs" Inherits="Web.coming" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span></span></h6>
            </div>
        </div>
    </div>
  <div class="container">	
		<div class="four">		
            <img class="commingimg" src="images/commingsoon2.jpg"  style="width:100%"/>
		  <p> <a href="index" class="hvr-shutter-out-vertical">返回首页 </a></p>
		  
		    </div>
		</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  <script type="text/javascript">
      $(function () {
          if(isMobile())
          {
              $(".commingimg").css("width","100%");
          }
      })
  </script>
</asp:Content>

