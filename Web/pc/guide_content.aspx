<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master" AutoEventWireup="true"  Inherits="Web.m.app.guide_content" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .marg {
            text-align: center !important;
        }
    </style>
    <script type="text/javascript">
     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="contact" style="padding: 1.5em 0px;">
        <div class="container">
                <div class="contact-top heading">
                <h2><%=webConfig.Remark %></h2>
            </div>
              <div class="contact-bottom">
                       <div class=" contact-left" style="word-break:break-all">
                            <%=webConfig.Value %>
                       </div>
              </div>
            </div>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
