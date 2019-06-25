<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempEidt.aspx.cs" Inherits="Web.Admin.Temp.TempEidt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .tdlable {
            width: 15%;
            text-align: right;
        }

        .tdlable {
            width: 15%;
        }

        .rateval {
            width: 10%;
        }

        .ratepencent, .pencent {
            width: 10%;
        }
    </style>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <input type="hidden" id="hidId" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="tdlable">银行:
                        </td>
                        <td class="tdvalue">
                            <select id="ddlBank" runat="server"></select>
                            &emsp;
                        刷卡笔数:
                     
                        <input type="text" id="txtCostCount" runat="server" class="normal_input" require-type="int" require-msg="刷卡笔数" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdlable">额度区间:
                        </td>
                        <td class="tdvalue">
                            <input id="txtMinMoney" runat="server" class="normal_input" type="text" maxlength="20"
                                require-type="decimal" require-msg="额度区间-最小值" />~
                          <input id="txtMaxMoney" runat="server" class="normal_input" type="text" maxlength="20"
                              require-type="decimal" require-msg="额度区间-最大值" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdlable" style="width: 20%">提额规划卡内预留额度百分比:
                        </td>
                        <td class="tdvalue">
                            <input id="txtLeavePencent" runat="server" class="normal_input pencent" type="text" maxlength="20"
                                require-type="decimal" require-msg="提额规划卡内预留额度百分比" />%&emsp;
                       <input id="txtLeaveMoney" runat="server" style="display: none" class="normal_input" type="text" maxlength="20" value="0" />
                            还款规划卡内预留额度百分比：
                        <input id="txtLeavePencent2" runat="server" class="normal_input pencent" type="text" maxlength="20"
                            require-type="decimal" require-msg="还款规划卡内预留额度百分比" />%
                        </td>
                    </tr>
                    <tr>
                        <td class="tdlable" style="width: 20%">提额规划网络消费比例:
                        </td>
                        <td class="tdvalue">
                            <input id="txtOnlineTake1" runat="server" class="normal_input pencent tiplan" type="text" maxlength="20"
                                require-type="decimal" require-msg="提额规划网络消费比例" />%&emsp;
                        提额规划线下消费比例：
                        <input id="txtRealTake1" runat="server" class="normal_input pencent tiplan" type="text" maxlength="20"
                            require-type="decimal" require-msg="提额规划线下消费比例" />%
                        </td>
                    </tr>

                    <tr>
                        <td class="tdlable" style="width: 20%">还款规划网络消费比例:
                        </td>
                        <td class="tdvalue">
                            <input id="txtOnlineTake2" runat="server" class="normal_input pencent huanplan" type="text" maxlength="20"
                                require-type="decimal" require-msg="还款规划网络消费比例" />%&emsp;
                        还款规划线下消费比例：
                        <input id="txtRealTake2" runat="server" class="normal_input pencent huanplan" type="text" maxlength="20"
                            require-type="decimal" require-msg="还款规划线下消费比例" />%
                        </td>
                    </tr>
                    <tr>
                        <td class="tdlable">刷卡比例:
                        </td>
                        <td class="tdvalue">
                            <asp:Repeater ID="rep_rates" runat="server">
                                <ItemTemplate>
                                    <input type="hidden" name="rate_<%#Container.ItemIndex %>" value="<%#Eval("Code") %>" /><%#Eval("Name") %>
                                    <input type="text" class="normal_input ratepencent" value="<%#GetBandValue(Eval("Code")) %>" name="ratePencent_<%#Container.ItemIndex %>" />%<br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr style="height: 50px;">
                        <td></td>
                        <td colspan="2" align="center">
                            <input class="normal_btnok" id="btnOK" type="button" runat="server" value="提交" onclick="checkChange();" />
                            <input class="btn btn-danger" id="Button1" type="button" value="返回" onclick="returnChange();" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {

        });
        function returnChange() {
            callhtml("/Admin/Temp/TempList.aspx", "模板列表");
        }


        function checkHuanPlan() {
            var totalPencent = 0;
            var isFalse = false;
            $(".huanplan").each(function () {
                if ($.trim($(this).val()) != "") {
                    if (parseFloat($(this).val()) % 10 != 0) {
                        isFalse = true;
                        return false;
                    }
                    totalPencent += parseFloat($(this).val());
                }

            });
            if (isFalse) {
                layer.alert("还款规划的消费比例应是10的倍数。");
                return false;
            }
            if (totalPencent != 100) {
                layer.alert("还款规划的消费比例不是100%，请重新配置。");
                return false;
            }
            return true;
        }

        function checkTiePlan() {
            var totalPencent = 0;
            var isFalse = false;
            $(".tiplan").each(function () {
                if ($.trim($(this).val()) != "") {
                    if (parseFloat($(this).val()) % 10 != 0) {
                        isFalse = true;
                        return false;
                    }
                    totalPencent += parseFloat($(this).val());
                }

            });
            if (isFalse) {
                layer.alert("提额规划的消费比例应是10的倍数。");
                return false;
            }
            if (totalPencent != 100) {
                layer.alert("提额规划的消费比例不是100%，请重新配置。");
                return false;
            }
            return true;
        }

        function checkChange() {
            if (checkTiePlan()) {
                if (checkHuanPlan()) {
                    var totalPencent = 0;
                    var isFalse = false;
                    $(".ratepencent").each(function () {
                        if ($.trim($(this).val()) != "") {
                            if (parseFloat($(this).val()) % 10 != 0) {
                                isFalse = true;
                                return false;
                            }
                            totalPencent += parseFloat($(this).val());
                        }

                    });
                    if (isFalse) {
                        layer.alert("配置的刷卡比例百分比应是10的倍数。");
                        return;
                    }
                    if (totalPencent != 100) {
                        layer.alert("配置的刷卡比例百分比不是100%，请重新配置。");
                        return;
                    }
                    //查看额度区间是否在该银行重复
                    var minMoney = $.trim($("#txtMinMoney").val());
                    var maxMoney = $.trim($("#txtMaxMoney").val());
                    var bankCode = $("#ddlBank").val();
                    var code = $("#hidId").val();
                    var result = GetAjaxString('checkBankTemplate', bankCode + "&minMoney=" + minMoney + "&maxMoney=" + maxMoney + "&tcode=" + code);
                    if (result == "1") {
                        layer.alert("该银行额度区间重复与已有模板重合金额。");
                        return;
                    }

                    if (checkForm()) {
                        var rek = '<%=Request.RawUrl%>';
                        rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                        $.ajax({
                            type: 'post',
                            url: '/Admin/Temp/' + rek + '?Action=Add',
                            data: $('#form1').serialize(),
                            success: function (info) {
                                if (info == "1") {
                                    layer.alert('保存成功');
                                    setTimeout(function () {
                                        //callhtml('/Admin/Temp/TempList', '银行管理');
                                    }, 1000);
                                }
                            }
                        });
                        //ActionModelAndBack('/Admin/Temp/TempEidt.aspx?Action=Modify', $('#form1').serialize(), '/Admin/Temp/TempList.aspx', '模板列表');
                    }
                }
            }
        }
    </script>
</body>
</html>
