<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradeConfig.aspx.cs" Inherits="Web.Admin.WebAdmin.TradeConfig" %>

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
            /*width: 40%;*/
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
                $.ajax({
                    type: 'post',
                    url: 'WebAdmin/' + rek + '?Action=ADD',
                    data: $('#form1').serialize(),
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

        function turnRecord(rolecode,rolename) {
            setUrlCookie();
            rolecode=$.trim(rolecode);
            callhtml('WebAdmin/WebConfig.aspx?edittype=role&id=' + rolecode, '设置【'+rolename+'】图文介绍');
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <div class="ui_table" style="margin: 20px">

                    <div class="divTitle">用户缴费金额及配送名额</div>
                    <table cellpadding="0" cellspacing="0" class="tabcolor">
                        <tr>
                            <th class="thCenter">用户等级
                            </th>
                            <th class="thCenter">缴费金额
                            </th>
                            <th class="thCenter">默认配送数量
                            </th>
                              <th class="thCenter">计入红包池金额
                            </th>
                            <th class="thCenter">图文介绍
                            </th>
                        </tr>
                        <asp:Repeater ID="repRoleConfig" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="10%" align="center"><%#Eval("Name")%>
                                        <input type="hidden" value="<%# Eval("Code")%>" name="RoleCode" />
                                    </td>
                                    <td align="center">
                                        <input name="Remark_<%# Container.ItemIndex%>" value="<%#Eval("Remark")%>" require-type="decimal"
                                            require-msg="缴费金额" class="normal_input tabInput" type="text" />
                                    </td>
                                    <td align="center">
                                        <input name="AreaLeave_<%# Container.ItemIndex%>" value="<%#Eval("AreaLeave")%>" require-type="decimal"
                                            require-msg="默认配送数量" class="normal_input tabInput" type="text" />
                                    </td>
                                     <td align="center">
                                        <input name="ToPrizeMoney_<%# Container.ItemIndex%>" value="<%#Eval("ToPrizeMoney")%>" require-type="decimal"
                                            require-msg="计入红包池金额" class="normal_input tabInput" type="text" />
                                    </td>
                                    <td width="10%" align="center">
                                        <input type="button" onclick="turnRecord('<%# Eval("Code")%>','<%# Eval("Name")%>')" value="设置图文介绍" class="btn btn-success" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>


                    <div class="divTitle">直接推荐奖励设置</div>
                    <table cellpadding="0" cellspacing="0" class="tabcolor">
                        <tr>
                            <th class="thCenter">推荐奖励名称
                            </th>
                            <th class="thCenter">奖金金额
                            </th>
                        </tr>
                        <asp:Repeater ID="rep_KCTJList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="10%" align="center"><%#Eval("Remark")%>
                                    </td>
                                    <td width="40%" align="center">
                                        <input name="KCTJ_<%# Eval("Id")%>" value="<%#Eval("TJFloat")%>" require-type="decimal"
                                            require-msg="奖金金额" class="normal_input tabInput" type="text" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
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
