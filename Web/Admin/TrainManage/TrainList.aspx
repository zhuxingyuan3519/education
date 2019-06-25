<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainList.aspx.cs" Inherits="Web.Admin.TrainManage.TrainList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
    
    </style>
    <script type="text/javascript">
        function seeDetail(code) {
            callhtml('TrainManage/TrainEdit?ID=' + code, '编码信息');
        }
        function addNew() {
            callhtml('TrainManage/TrainEdit', '新增编码');
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                <input type="button" value="新增" class="btn btn-danger hidden" onclick="addNew()" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                <thead>
                    <tr>
                        <th>密码类型
                        </th>
                        <th>密码数量
                        </th>
                        <th>操作
                        </th>
                    </tr>
                </thead>
                <tbody id="layerAppendView">
                    <asp:Repeater ID="rep_list" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("CodeName") %></td>
                                <td><%#Eval("CodeCount") %></td>
                                <td>
                                    <input type='button' class='btn btn-info' value='查看明细' onclick="seeDetail('<%#Eval("CodeType") %>')" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </div>
</body>
</html>
