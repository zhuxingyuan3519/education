<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardNotice.aspx.cs" Inherits="Web.Admin.Message.CardNotice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
  <style type="text/css">
  .imgDiv{ float:left; margin-left:10px;}
  .bankname{ margin-left:30%}
  </style>   
    <script type="text/javascript">
        function onclickCallhtml(bankCode,bankRemark)
        {
            var ctype = '<%=type%>';
            var ctitle = '';
            var caddress="Message/NoticeList.aspx?id=2&bank="+bankCode+"&type="+ctype;
            if (ctype == '0')//信用卡常识
            {
                ctitle="信用卡常识管理—"+bankRemark;
                //callhtml("../Message/NoticeList.aspx?id=2&bank="+bankCode+", '信用卡常识管理—<%#Eval("Remark")%>');
            }
            else if(ctype=="1")
            {
                ctitle = "信用卡提额管理—" + bankRemark;
            }
            callhtml(caddress, ctitle);
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
            <table cellpadding="0" cellspacing="0">
              <tr><td>
              <asp:Repeater ID="repBankList" runat="server">
              <ItemTemplate>
              <div class="imgDiv">
             <a  href="javascript:void(0)" onclick="onclickCallhtml('<%#Eval("Code")%>','<%#Eval("Remark")%>');"><img style="width:150px;height:45px" src="../Attachment/<%#Eval("PicUrl")%>" />
                <p>
              <span class="bankname">
              <%#Eval("Name")%>
              </span>
              </p>
               </a>
</div>
              </ItemTemplate>
              </asp:Repeater>
              </td></tr>
            </table>
            </form>
        </div>
    </div>
     
</body>
</html>

