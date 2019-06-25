<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="talk.aspx.cs" Inherits="Web.m.app.talk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          $(function () {
              setNavIndexChecked("3");
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row bg-wh">
        <h5><b>|</b>消息列表</h5>
    </div>
    <asp:Repeater ID="repMsgList" runat="server">
        <ItemTemplate>
            <div   class="row marg list-border itemlist" >
                <div class="clickButton" >
				    <div class="col-sm-12 col-xs-12">
                           <a href="talk_detail?mtype=<%#Eval("MType") %>&mcode=<%#Eval("Code") %>" style="text-decoration:none"><img src="/images/new918.gif" class="newmsgtip"/>&nbsp;&nbsp;来自<%#Eval("MID") %>的未读消息。</a>
				    </div>
                </div>
			</div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
