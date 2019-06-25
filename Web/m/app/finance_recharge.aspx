<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="finance_recharge.aspx.cs" Inherits="Web.m.app.finance_recharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>余额充值</h5>
    </div>
      <div class="row list-card">
        <div class="marg">
            <span class="col-sm-3 col-xs-3  text-left">充值金额</span>
            <span class="col-sm-9 col-xs-9">
               <input type="text" class="form-control" placeholder="充值金额" id="txt_Money" require-type="require" require-msg="充值金额" />
            </span>
        </div>
          </div>
    <div class="row list-card">
          <div class="marg">
            <span class="col-sm-3 col-xs-3  text-left">充值途径</span>
            <span class="col-sm-9 col-xs-9">
                <select id="ddlPayType" class="form-control required" >
                           <option value="alipay">支付宝</option>
                </select>
            </span>
        </div>

           <div class="marg">
          <div class="col-sm-6 col-xs-6 col-sm-offset-3 col-xs-offset-3" style="padding-bottom:20px">
        <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">充     值</a>
              </div>
    </div>
          </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
      <script type="text/javascript">
          function setupChange() {
              if (!checkForm())
                  return false;
              if (parseFloat($.trim($("#txt_Money").val())) < 0) {
                  layerMsg("充值金额不能为负数。");
                  return false;
              }
              if ($("#ddlPayType").val() == "alipay") {
                  window.location = "/Alipay/alipay.aspx?type=recharge&money=" + $("#txt_Money").val();
              }
          }
    </script>
</asp:Content>
