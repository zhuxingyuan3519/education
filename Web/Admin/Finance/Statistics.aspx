<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Web.Admin.Finance.Statistics" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>财务统计</title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <style type="text/css">
        .TXTR{display:none;}
        .TXTRtd{padding-left:50px !important;}
    </style>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                    <table>
                    <tr>
                        <td class="tdlable" colspan="5">
                              <input  id="nPayBeginTime" runat="server" placeholder="开始时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                ~<input  id="nPayEndTime" runat="server" placeholder="结束时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
           <input type="button"  value="统计" class="btn btn-danger" onclick="setupChange()"/>
                            未提现金额合计：<span id="spNoTXTotalMoney" runat="server"></span>
                        </td>
                    </tr>
                        </table>
                <table  class="tabSattic">
                    <tr class="agentNoSee"><td style="width:20%">会员付款总金额</td><td><span id="sp_MemberTotalPayMoney" ></span> </td></tr>
                      <tr class="agentNoSee hidden"><td>支付宝手续费合计</td><td><span id="sp_AliPayFeeTotalMoney" ></span> </td></tr>
                      <tr class="agentNoSee"><td>公司实际收入合计</td><td><span id="sp_CompanyIncomeTotalMoney" ></span> </td></tr>
                    
                      <tr  class="agentNoSee" style="cursor:pointer"><td onclick="lookTX()" colspan="2">会员提现合计：<span id="sp_TXTotalMoney" ></span>  >> </td></tr>
                       <tr class="TXTR agentNoSee"><td class="TXTRtd">微信提现合计</td><td><span id="sp_WeixinTXTotalMoney" ></span> </td></tr>
                       <tr class="TXTR agentNoSee"><td class="TXTRtd">支付宝提现合计</td><td><span id="sp_AliPayTXTotalMoney" ></span> </td></tr>
                    <tr class="TXTR agentNoSee"><td class="TXTRtd">银行卡提现合计</td><td><span id="sp_BankCardTXTotalMoney" ></span> </td></tr>
                     <tr class="TXTR agentNoSee"><td class="TXTRtd">TPay提现合计</td><td><span id="sp_TPayTXTotalMoney" ></span> </td></tr>
                     <tr class="TXTR agentNoSee"><td class="TXTRtd">VPay提现合计</td><td><span id="sp_VPayTXTotalMoney" ></span> </td></tr>
                      
                    <tr class="agentNoSee"><td>提现手续费合计</td><td><span id="sp_TXFeeTotalMoney" ></span> </td></tr>
                    <tr class="agentNoSee"><td>公司实际支出合计</td><td><span id="sp_CompanyOutTotalMoney" ></span> </td></tr>
                    <tr class="agentNoSee" style="display:none"><td>未提现总金额合计</td><td><span id="sp_NoTXTotalMoney" ></span> </td></tr>
                     <tr class="agentNoSee"><td>已提现未转账金额合计</td><td><span id="sp_HasTXNoTranTotalMoney" ></span> </td></tr>
                    <tr class="agentNoSee"><td>公司可支配金额合计</td><td><span id="sp_CompanyUseMoney" ></span> </td></tr>

                        <tr><td colspan="2" style="padding:20px">奖金支出合计：<span id="sp_FHTotalMoney" ></span></td></tr>
                        <tr class="tr_ThreeLeavelTotalMoney" ><td>VIP会员直接推荐奖励</td><td><span id="sp_ThreeLeavelTotalMoney" ></span> </td></tr>
                      <tr class="tr_ThreeLeavelTotalMoney" ><td>代理商直接推荐奖励</td><td><span id="sp_ThreeLeavelTotalMoney1F" ></span> </td></tr>
                      <tr class="tr_ThreeLeavelTotalMoney" ><td>培训机构直接推荐奖励</td><td><span id="sp_ThreeLeavelTotalMoney2F" ></span> </td></tr>

                    <tr class="tr_ThreeLeavelTotalMoney hidden"><td>代理商两级分销支出合计</td><td><span id="sp_AgentThreeLeavelFHMoney" ></span> </td></tr>
                      
                    <tr class="hidden"><td>返还金合计</td><td><span id="sp_AgentFHTotalMoney" ></span> </td></tr>
                      <tr class="hidden"><td>卡管家使用费合计</td><td><span id="sp_KCTrainingMoney" ></span> </td></tr>
                           <tr class="hidden"><td>服务中心管理费合计</td><td><span id="sp_KCOperateMoney" ></span> </td></tr>
                         <tr class="hidden"><td>公司管理费合计</td><td><span id="sp_KCYunlianhuiMoney" ></span> </td></tr>
                </table>


            </form>
        </div>

    </div>
    <script type="text/javascript">
        $(function () {
            var rolePower = '<%=(TModel.Role.AreaLeave!=null&&Convert.ToInt16(TModel.Role.AreaLeave)>=20)?"1":"0"%>';
            var roleCode = '<%=TModel.RoleCode%>';
            if (rolePower == "1")
                $(".agentNoSee").hide();
            if (roleCode == 'Admin' || roleCode == 'Manage') {
                $(".tr_ThreeLeavelTotalMoney").show();
                $(".tr_ThreeLeavelTotalMoneyIn").hide();
            }
            setupChange();
        });
        function lookTX() {
            $(".TXTR").toggle();
        }
        function setupChange() {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Finance/' + rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var jsonObj = eval('(' + info + ')');
                    $.each(jsonObj, function (name, value) {
                        $("#sp_" + name).html(value);
                    });
                }
            });
        }
    </script>
</body>
</html>
