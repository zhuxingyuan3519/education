<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradeConfig.aspx.cs" Inherits="Web.Admin.TrainManage.TradeConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .tabInput {
            width: 98%;
        }

        .thCenter {
            text-align: center;
            width: 40%;
        }

        .appendImg {
            width: 60px;
        }

        .divTitle {
            font-size: 14px;
            background-color: #e8ebf2;
            padding: 8px;
        }
    </style>
    <script type="text/javascript">

        function checkFormVal() {
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            if (checkForm()) {
                //getItem();
                //return false;
                var paramInfo = {
                    param: JSON.stringify(getItem()),
                };
                $.ajax({
                    type: 'post',
                    url: 'TrainManage/' + rek + '?Action=ADD',
                    data: paramInfo,
                    success: function (info) {
                        layer.alert(info);
                        //setTimeout(function () { v5.clearall(); }, 1000);
                        //callhtml('TradeManage/TradeConfig.aspx', '分销商配置');
                    }
                });
            }
        }
        $(function () {

        });

        function setUrlCookie() {
            document.cookie = 'lasturl=WebAdmin/TradeConfig'; document.cookie = 'lasturlname=分红配置';
        }

        function turnRecord(rolecode, rolename) {
            setUrlCookie();
            rolecode = $.trim(rolecode);
            callhtml('WebAdmin/WebConfig.aspx?edittype=role&id=' + rolecode, '设置【' + rolename + '】图文介绍');
        }
        function getItem() {
            var shmoneyList = [];
            $(".div_item").each(function () {
                var chargeCode = $(this).find(".TrainChargeCode").val();
                var inde = 1;
                $(this).find("table tr").each(function (n,value) {
                    if (n>0) {
                        var Remark = $(this).find(".Remark").val();
                        var RoleCode = $(this).find(".RoleCode").val();
                        var TJFloat = $(this).find(".TJFloat").val();
                        var TJIndex = inde;
                        inde++;
                        //alert($(this).find(".Field3").prop("checked"));
                        var Field3 = $(this).find(".Field3").prop("checked") ? "1" : "2";
                        var addObj = { "Code": chargeCode, "Remark": Remark, "RoleCode": RoleCode, "TJIndex": TJIndex, "Field3": Field3, "TJFloat": TJFloat };
                        shmoneyList.push(addObj);
                    }
                });
            });
            return shmoneyList;
            //console.log(JSON.stringify(shmoneyList));
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <div class="ui_table" style="margin: 20px">

                    <asp:Repeater ID="repSHMoneyTypeList" runat="server" OnItemDataBound="rptypelist_ItemDataBound">
                        <ItemTemplate>
                            <div class="div_item">
                                <div class="divTitle"><%#Eval("ChargeList") %>缴费奖励配置</div>
                                <input type="hidden" value="<%#Eval("Code") %>" class="TrainChargeCode" />
                                <table cellpadding="0" cellspacing="0" class="tabcolor">
                                    <tr>
                                        <th class="thCenter">奖励名称
                                        </th>
                                        <th class="thCenter">奖金金额
                                        </th>
                                        <th class="thCenter">是否百分比
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="rep_ShoneyList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_Remark" value="<%#Eval("Remark")%>" require-type="require" require-msg="奖励名称" class="normal_input tabInput Remark" type="text" />
                                                    <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_RoleCode" value="<%#Eval("RoleCode")%>" type="hidden" class="RoleCode" />
                                                    <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_TJIndex" value="<%#Eval("TJIndex")%>" type="hidden" class="TJIndex" />
                                                </td>
                                                <td align="center">
                                                    <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_TJFloat" value="<%#Eval("TJFloat")%>" require-type="decimal" require-msg="奖金金额" class="normal_input tabInput TJFloat" type="text" />
                                                </td>
                                                <td align="center">
                                                    <input type="checkbox" name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_Field3" class="Field3" <%#Eval("Field3").ToString()=="1"?"checked='checked'":""%> />是
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>



                    <div style="background-color: #fff; text-align: center">
                        <input type="button" value="保存" class="btn btn-success" onclick="checkFormVal()"
                            style="margin-right: 15%" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
