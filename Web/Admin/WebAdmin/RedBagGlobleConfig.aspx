<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RedBagGlobleConfig.aspx.cs" Inherits="Web.Admin.WebAdmin.RedBagGlobleConfig" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <style type="text/css">
        .divLabel {
            width: 80px;
            float: left;
            background-color: #e7ebee;
            height: 30px;
            padding-top: 5px;
            padding-left: 5px;
            border-radius: 3px;
            margin-left: 5px;
            cursor: pointer;
        }

    </style>
    <script type="text/javascript">
        $(function () {
          
        });
    
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                <input type="hidden" runat="server" id="hidId" />
                <table>
                      <tr class="">
                        <td class="tdlable">公司红包金额:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_CompanyReturnMoney" type="text" runat="server" />
                        </td>
                      
                        <td class="tdlable">个人红包金额:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_PersonalReturnMoney" type="text" runat="server" />
                        </td>
                    </tr>
                    <tr class="">
                        <td class="tdlable">激活红包需要推荐的缴费会员数量:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_ActiveForTJVIPCount" type="text" runat="server" />
                        </td>
                      
                        <td class="tdlable">激活红包需要推荐的注册会员数量:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_ActiveForTJCount" type="text" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="保存" onclick="setupChange()" />&emsp;&emsp;
                        </div>
                    </div>
                </div>
           </form>
        </div>

    </div>
    <script type="text/javascript">
        function setupChange() {
            if (!checkForm()) return false;
           var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'WebAdmin/' + rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var res = info.split('≌');
                    if (res[0] == "0")
                        layer.alert("操作失败，请重试");
                    else if (res[0] == "1") {  //提交成功
                        layer.alert("操作成功！");
                    }
                    else
                        layer.alert(info);
                }
            });
        }
    </script>
</body>
</html>
