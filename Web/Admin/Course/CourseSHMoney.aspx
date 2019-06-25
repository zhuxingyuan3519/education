<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseSHMoney.aspx.cs" Inherits="Web.Admin.Course.CourseSHMoney" %>

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
            width:40%;
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
                    url: 'Course/' + rek + '?Action=ADD',
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
       
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <input type="hidden" runat="server" id="hisSHType" />
                <input type="hidden" runat="server" id="hidCcode" />
                <div class="ui_table" style="margin: 20px">
                    <asp:Repeater ID="repSHMoneyTypeList" runat="server" OnItemDataBound="rptypelist_ItemDataBound">
                        <ItemTemplate>
                             <div class="divTitle"><%#Eval("Name") %>配置</div>
                            <table cellpadding="0" cellspacing="0" class="tabcolor" >
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
                                    <td  align="center">
                                            <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_Remark" value="<%#Eval("Remark")%>" require-type="require"    require-msg="奖励名称" class="normal_input tabInput" type="text" />
                                        <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_RoleCode" value="<%#Eval("RoleCode")%>"  type="hidden" />
                                        <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_TJIndex" value="<%#Eval("TJIndex")%>"  type="hidden" />
                                    </td>
                                    <td align="center">
                                        <input name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_TJFloat" value="<%#Eval("TJFloat")%>" require-type="decimal"    require-msg="奖金金额" class="normal_input tabInput" type="text" />
                                    </td>
                                      <td  align="center">
                                          <input type="checkbox"  name="<%# DataBinder.Eval((Container.NamingContainer.NamingContainer as RepeaterItem).DataItem, "Code") %>_<%# Eval("Id")%>_Field3" <%#Eval("Field3").ToString()=="1"?"checked='checked'":""%>/>是
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                        </ItemTemplate>
                    </asp:Repeater>

                    <div style="background-color: #fff; text-align: center">
                        <input type="button" value="保存" class="btn btn-success" onclick="checkFormVal()" />&emsp;
                            <input type="button" value="返回" class="btn btn-danger" onclick="callhtml('Course/CourseList', '课程列表');" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
