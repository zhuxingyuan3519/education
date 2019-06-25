<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="guide_contact.aspx.cs" Inherits="Web.m.app.guide_contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
       .marg{text-align:center !important;}
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="row bg-wh">
				<h5><b>|</b>联系我们</h5>
			</div>
    <div class="row list-card">
				 <div class="marg">
					    <span class="col-sm-12 col-xs-12 marg " >
					 <h4>  <%=Service.CacheService.GlobleConfig.Address %></h4>
					    </span>
				    </div>
				 	<div class="marg">
					    <span class="col-sm-12 col-xs-12 marg">
					       联系电话： <%=Service.CacheService.GlobleConfig.Phone %>&emsp;
                       
					    </span>
				    </div>
				    <div class="marg">
				    <span class="col-sm-12 col-xs-12 marg">
					     联系QQ： <%=Service.CacheService.GlobleConfig.QQ %>&emsp;
					    </span>
				    </div>
          <div class="marg">
				    <span class="col-sm-12 col-xs-12 marg">
                             <a href="tel:<%=Service.CacheService.GlobleConfig.Phone %>" class="btn btn-success btn-sm">电话联系</a>&emsp;
                             <a id="showQQMobile" href="mqqwpa://im/chat?chat_type=wpa&uin=<%=Service.CacheService.GlobleConfig.QQ %>&version=1&src_type=web&web_src=bjhuli.com"  class="btn btn-success btn-sm" >QQ联系</a>
					    </span>
				    </div>
			</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
