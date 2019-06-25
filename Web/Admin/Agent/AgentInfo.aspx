<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentInfo.aspx.cs" Inherits="Web.Admin.Agent.AgentInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Agent/' + rek + '?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    if (info == "0")
                        layer.alert("提交失败，请重试！");
                    else if (info == "1") {  //提交成功
                        layer.alert("操作成功！");
                        setTimeout("resetList()", 2000);
                    }
                    else
                        layer.alert(info);
                }
            });
        }
    
        function attrSelcet(va, level) {
            var addInfo = {
                //type: 'GetAddressInfo',
                type: 'GetNewAddressInfo',
                pram: va,
                level: level
            };
            var result = GetAjaxString(addInfo);
            if (result != "0") {
                var ddlcity = $("#ddl_City");
                if (level == "30")
                    ddlcity = $("#ddl_Zone");
                else {
                    $("#ddl_City").empty();
                }
                ddlcity.empty();
                ddlcity.append("<option value=''>--请选择--</option>");
                var data = eval(result);
                for (var index = 0; index < data.length; index++) {
                    var val = data[index];
                    var html = "<option value='" + val.Id + "'>" + val.Name + "</option>";
                    ddlcity.append(html);
                }
            }
        }
        function cityChange(obj, level) {
            if (level != 40)
                attrSelcet($(obj).val(), level);
            //else
            //    $("#hidZone").val($(obj).val());
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding-top:20px">
                <input type="hidden" runat="server" id="hidId" />
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">姓名：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Name"   require-type="require" require-msg="姓名" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">登录账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MID"  disabled="disabled"  require-type="require" require-msg="登录账号" />
                        </div>
                    </div>
                   <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">登录密码：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_pwd"  require-type="require" require-msg="登录账号" />
                        </div>
                    </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_Phone">邮箱：</label>
                                <input runat="server" type="text" class="form-inline" id="txt_Email" />
                            </div>
                        </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_QQ">手机号码：</label>
                                <input runat="server" type="text" class="form-inline" id="txtPhone" require-type="require" require-msg="手机号码" value="11"/>
                            </div>
                        </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_WeiXin">微信号码：</label>
                                <input runat="server" type="text" class="form-inline" id="txt_WeiXin" />
                            </div>
                        </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_QQ">&nbsp;QQ号码：</label>
                                <input runat="server" type="text" class="form-inline" id="txt_QQ" />
                            </div>
                        </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_WeiXin">体验端口数量：</label>
                                <span><%=TModel.LeaveTradePoints %></span>
                            </div>
                        </div>
                        <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_QQ">收费端口数量：</label>
                              <span><%=TModel.TradePoints %></span>
                                 </div>
                        </div>


                               <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_QQ">分销商级别：</label>
                                                           <span id="spAgentLeavel" runat="server"></span>
                                 </div>
                        </div>

                                <div class="col col-md-6">
                            <div class="form-group">
                                <label for="txt_QQ">推荐人代码：</label>
                                      <input runat="server" type="text" class="form-inline" id="txt_MTJ"  disabled="disabled"  require-type="require" require-msg="推荐人代码"/>
                                 </div>
                        </div>
                    <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_MTJ">所在地区：</label>
                                    <select class="frm-field required" id="ddl_Province" runat="server" onchange="cityChange(this,20)">
                                    </select>
                              
                                    <select class="frm-field required" id="ddl_City" runat="server" onchange="cityChange(this,30)">
                                    </select>
                               
                                    <select class="frm-field required" id="ddl_Zone" runat="server" onchange="cityChange(this,40)" >
                                    </select>
                                </div>
                            </div>
                </div>

                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                        </div>
                    </div>
                </div>

            </form>
        </div>
</body>
</html>
