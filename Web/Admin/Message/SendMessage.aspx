<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMessage.aspx.cs" ValidateRequest="false" Inherits="Web.Admin.Message.SendMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        window.UEDITOR_HOME_URL = "/ueditor/";
    </script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script>
    <!--建议手动加在语言，避免在ie下有时因为加载语言失败导致编辑器加载失败-->
    <!--这里加载的语言文件会覆盖你在配置项目里添加的语言类型，比如你在配置项目里配置的是英文，这里加载的中文，那最后就是中文-->
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
    <style type="text/css">
        .divnames {
            float: left;
            margin-left: 10px;
            width: 200px;
        }

        .spnames {
            width: 80px;
        }

        input[type='checkbox'] {
            cursor: pointer;
        }

        #queryResult {
            display: none;
        }

        .inpersonlist {
            float: left;
            margin-left: 10px;
            padding-top: 10px;
        }

        .inpersonspan {
            float: right;
            margin-top: -7px;
            cursor: pointer;
            color: red;
        }
    </style>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="margin-top: 20px">
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label>
                                发送会员：
                                <input type="text" id="txtMID" class="form-inline" />&nbsp;<input type="button" onclick="setQuery(1)" value="查询" class="btn btn-sm btn-info" /></label>&emsp;<input type="checkbox" name="chkAll" id="cheeckAll" />全体发送
                         
                        </div>
                    </div>
                    <div class="col col-md-6">
                        &emsp;选择群组发送：
                          <div id="divRoles" runat="server"></div>
                    </div>
                </div>
                <div class="row">
                    <div class=" col-md-6" style="margin-top: -20px; margin-bottom: 20px;">
                        <div id="divChoicePerson" style="width: 500px">
                        </div>
                    </div>
                    <div class=" col-md-6" style="float: right">
                        <div>
                            按区域发送：
                              <select class="frm-field " id="ddl_Province" runat="server" onchange="cityChange(this,20)">
                              </select>
                            <select class="frm-field " id="ddl_City" runat="server" onchange="cityChange(this,30)">
                            </select>
                            <select class="frm-field " id="ddl_Zone" runat="server" onchange="cityChange(this,40)">
                            </select>
                        </div>
                    </div>
                       <div  class=" col-md-6" >
                            发送给代理商名下会员：
                                  <label>
                                      <input type="text" id="txtAgentMID" class="form-inline" />&nbsp;<input type="button" onclick="setQuery(2)" value="查询代理商" class="btn btn-sm btn-info" /></label>
                            <div id="queryAgentResultTd"></div>
                        </div>
                            <div  class=" col-md-6" >
                            发送给代理商：
                                  <label>
                                      <input type="text" id="txtSendAgentMID" class="form-inline" />&nbsp;<input type="button" onclick="setQuery(3)" value="查询代理商" class="btn btn-sm btn-info" /></label>
                            <div id="querySendAgentResultTd"></div>
                        </div>
                </div>
                <div class="row">
                    <div class="col col-md-12">
                        <div id="queryResult">
                            <div style="width: 40%" id="queryResultTd">
                            </div>
                        </div>
                        <div style="max-height: 100px; overflow-y: auto">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <label>发送内容：</label>
                            <textarea id="editor" style="width: 1000px; height: 300px;" runat="server"></textarea>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-md-12 " style="margin-top: 20px">
                        <div class="form-group">
                            <input type="button" class="btn btn-info" value="发送" id="btnSubmit" onclick="return saveConfig()" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        var ue = UE.getEditor('editor');
        $(function () {
            $("#cheeckAll").click(function () {
                if ($(this).prop("checked")) {
                    $(".divnames").find("input[type='checkbox']").prop("checked", true).attr("checked", "checked");
                }
                else {
                    $(".divnames").find("input[type='checkbox']").prop("checked", false).removeAttr("checked");
                }
            });
        })
        function saveConfig() {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Message/' + rek + '?Action=MODIFY',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    if (info == "0")
                        layer.alert("发送失败，请重试！");
                    else if (info == "1") {  //提交成功
                        layer.alert("发送成功！");
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        function setQuery(ind) {
            var mid = $("#txtMID").val();
            if (ind == 2) {
                mid = $("#txtAgentMID").val();
            }
            if (ind == 3) {
                mid = $("#txtSendAgentMID").val();
            }
            if ($.trim(mid) != "") {
                $("#queryResult").show();
                var userInfo;
                if (ind == 2) {
                    userInfo = {
                        type: 'GetAgentInfoByName',
                        pram: mid,
                    };
                }
                else if (ind == 1) {
                    userInfo = {
                        type: 'GetMemberInfoByName',
                        pram: mid,
                    };
                }
                else if (ind == 3) {
                    userInfo = {
                        type: 'GetAgentInfoByName',
                        pram: mid,
                    };
                }
                var result = GetAjaxString(userInfo);
                if (result != "0") {
                    var arr = result.split('*');
                    var trs = "<ul><li>昵称&emsp;&emsp;&emsp;姓名</li>";
                    $.each(arr, function (n, value) {
                        if (value != null && value != "") {
                            if (ind == 1)
                                trs += "<li><input type='checkbox'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',1)  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                            else if (ind == 2)
                                trs += "<li><input type='checkbox'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',2)  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                            else if (ind == 3)
                                trs += "<li><input type='checkbox'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',3)  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                        }
                    });
                    trs += "</ul>";
                    //if (ind == 1)
                    $("#queryResultTd").html(trs);
                    //else
                    //    $("#queryAgentResultTd").html(trs);
                }
                else {
                    $("#queryResultTd").html('');
                }
            }
        }
        function choicePerson(obj, mid, ind) {
            var isChoice = $(obj).prop("checked");
            var id = $(obj).val();
            if (isChoice) { //选中了就加载到div中
                var appendhtml = "<div id='person_"+ind+"_" + id + "' class='inpersonlist'>" + mid + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (ind == 2)
                    appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>" + mid + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkAgentCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (ind == 3)
                    appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>" + mid + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkSendMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (typeof ($("#person_" + ind + "_" + id).html()) == 'undefined' || $("#person_" + ind + "_" + id).html() == '') {
                    if (ind == 1)
                        $("#divChoicePerson").append(appendhtml);
                    else if (ind == 2)
                        $("#queryAgentResultTd").append(appendhtml);
                    else if (ind == 3)
                        $("#querySendAgentResultTd").append(appendhtml);
                }
            }
            else {
                $("#person_" + ind + "_" + id).remove();
            }
        }
        function removeChoicedPerson(obj) {
            $(obj).parent().remove();
        }

        function attrSelcet(va, level) {
            var addInfo = {
                type: 'GetAddressInfo',
                pram: va,
                level: level
            };
            var result = GetAjaxString(addInfo);
            //var result = GetAjaxString('GetAddressInfo', va + '&level=' + level);
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
</body>
</html>
