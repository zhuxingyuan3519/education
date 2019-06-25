<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempleteEdit.aspx.cs" Inherits="Web.Admin.Templete.TempleteEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="js/uploadify/jquery.uploadify.min.js"></script>
    <style type="text/css">
        .appendImg {
            width: 200px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            loadUploadify();
            //关闭上传附件窗口
            $("#btnCloseAtts").click(function () {
                $("#AttBg").hide();
                $("#AttDiv").hide();
            });

            $("#spForClose").click(function () {
                $("#AttBg").hide();
                $("#AttDiv").hide();
            });
        });
        function deletePic(code, obj) {
            layer.confirm('确定要删除吗?', function (index) {
                $.ajax({
                    type: 'get',
                    url: '/Handler/DeleteUPFile.ashx?picName=' + code,
                    success: function (info) {
                        layer.close(index);
                        if (info == "0")
                            layer.alert("删除失败，请重试！");
                        else if (info == "1") {
                            $(obj).parent().remove();
                        }
                        else
                            layer.alert(info);
                    }
                });
                layer.close(index);
            });
        }
        var index = "1";
        function loadUploadify() {
            var list = "fileQueue2";
            $("#uploadify").uploadify({
                'swf': 'js/uploadify/uploadify.swf',
                'uploader': '../Handler/UploadExcel.ashx', //相对路径的后端脚本，它将处理您上传的文件。绝对路径前缀或'/'或'http'的路径
                'folder': '/Attachment/',
                'cancelImg': 'js/uploadify/uploadify-cancel.png',
                'script': 'UploadExcel.ashx',
                'queueID': list,
                'auto': false,
                'multi': false,
                'fileTypeDesc': '图像文件', //出现在上传对话框中的文件类型描述
                'fileTypeExts': '*',  //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
                'buttonText': '选择文件',
                'onUploadSuccess': function (file, data, response) {
                    if (index == "1")//上传模板附件
                    {
                        $("#hidAtt").val("/Attachment/" + data);
                        $("#divAttrContainer").html(file.name);
                    }
                    else if (index == "0")//上传模板附件
                    {
                        $("#hidAttShow").val("/Attachment/" + data);
                        $("#divAttrShowContainer").html(file.name);
                    }
                    else if (index == "2") {
                        $("#divImgContainer").append("<div><img src='../images/button-cross.png'  class='imgdel' onclick=deletePic('Attachment*" + data + "',this)><input type='hidden' name='uploadImg' value='/Attachment/" + data + "'/><img class='appendImg' src='/Attachment/" + data + "'/></div>");
                    }
                }
            });
        }
        function saveatts() {
            jQuery('#uploadify').uploadify('upload', '*');
        }
        function showUpload(obj, inde) {
            index = inde;
            var attsType = $(this).attr("AttsType");
            $("#btnSaveAtts").attr("AttsType", attsType);
            $("#AttBg").show();
            $("#AttDiv").show();
            return false;
        }
        function resetList() {
            callhtml('Templete/TempleteList.aspx', '模板列表');
        }
        function setupChange() {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'post',
                url: 'Templete/TempleteEdit.aspx?Action=MODIFY',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var res = info.split('≌');
                    if (res[0] == "0")
                        layer.alert(res[1]);
                    else if (res[0] == "1") {  //提交成功
                        layer.msg("保存成功！");
                        // 重新进入该页面
                        callhtml('Templete/TempleteEdit.aspx?id=' + $("#hidId").val(), '模板信息');
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4());
        }
        function checkChange() {
            if (!checkForm())
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'post',
                url: 'Templete/TempleteEdit.aspx?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var res = info.split('≌');
                    if (res[0] == "0")
                        layer.alert(res[1]);
                    else if (res[0] == "1") {  //提交成功
                        $("#hidId").val(res[1]);
                        layer.alert("操作成功！");
                        //if (res[1] == "1") {
                        //    var dataObj = eval("(" + res[2] + ")");
                        //    $.each(dataObj.TempContainer, function (idx, item) {
                        //        idx = guidGenerator();
                        //        $("#containertable").append("<tr class='trpage " + item.Field1 + "'><td><input type='hidden' name='hidSearchFileld_" + idx + "' value='" + item.Field1 + "'/> <input type='hidden' name='hidContainerId_" + idx + "' value=''/>   <input type='hidden' name='hidContainerCode_" + idx + "' value='" + item.ContainerCode + "'/> " + item.ContainerCode + "</td> " + "<td><input type='text' name='ContainerName_" + idx + "' style='width:80%' /></td>" + "<td><input type='hidden' name='hidContainerPage_" + idx + "' value='" + item.ContainerPage + "'/>" + item.ContainerPage + "</td>");
                        //    });
                        //    //加载查询页面
                        //    var pageObj = eval("(" + res[3] + ")");
                        //    $("#seleSearch").empty();
                        //    $("#seleSearch").append("<option value=''>--选择页面--</option>");
                        //    $.each(pageObj.PageContainer, function (idx, item) {
                        //        $("#seleSearch").append("<option value='" + item.salt + "'>" + item.page + "</option>");
                        //    });
                        //}
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        function seeTempFlag()
        {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'post',
                url: 'Templete/TempleteEdit.aspx?Action=OTHER',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var res = info.split('≌');
                    if (res[0] == "0")
                        layer.alert(res[1]);
                    else if (res[0] == "1") {  //提交成功
                        if (res[1] == "1") {
                            var dataObj = eval("(" + res[2] + ")");
                            //alert(dataObj.TempContainer.length);
                            $.each(dataObj.TempContainer, function (idx, item) {
                                idx = guidGenerator();
                                $("#containertable").append("<tr class='trpage " + item.Field1 + "'><td><input type='hidden' name='hidSearchFileld_" + idx + "' value='" + item.Field1 + "'/> <input type='hidden' name='hidContainerId_" + idx + "' value=''/>   <input type='hidden' name='hidContainerCode_" + idx + "' value='" + item.ContainerCode + "'/> " + item.ContainerCode + "</td> " + "<td><input type='text' name='ContainerName_" + idx + "' style='width:80%' /></td>" + "<td><input type='hidden' name='hidContainerPage_" + idx + "' value='" + item.ContainerPage + "'/>" + item.ContainerPage + "</td>");
                            });
                            //加载查询页面
                            var pageObj = eval("(" + res[3] + ")");
                            $("#seleSearch").empty();
                            $("#seleSearch").append("<option value=''>--选择页面--</option>");
                            $.each(pageObj.PageContainer, function (idx, item) {
                                $("#seleSearch").append("<option value='" + item.salt + "'>" + item.page + "</option>");
                            });
                        }
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        function choicePage(obj) {
            var thisVal = $(obj).val();
            if (thisVal == '') {
                $(".trpage").show();
            } else {
                $(".trpage").hide();
                $("." + thisVal).show();
            }
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                <input type="hidden" runat="server" id="hidId" />
                <input type="hidden" runat="server" id="hidDelIds" />
                <ul id="myTab" class="nav nav-tabs">
                    <li class="active"><a href="#mbjbxx" data-toggle="tab">模板基本信息</a></li>
                    <li><a href="#mbnrpz" data-toggle="tab">模板内容配置</a></li>
                </ul>
                <div id="myTabContent" class="tab-content">
                    <%--模板基本信息--%>
                    <div class="tab-pane fade in active" id="mbjbxx">
                        <table>
                            <tr>
                                <td class="tdlable">模板名称:
                                </td>
                                <td class="tdvalue">
                                    <input id="txtName" runat="server" class="" type="text" require-type="require"
                                        require-msg="模板名称" maxlength="20" /><span class="spred">*</span>
                                </td>

                                <td class="tdlable">模板价格:
                                </td>
                                <td class="tdvalue">
                                    <input id="txtPrice" runat="server" class="" type="text" require-type="decimal"
                                        require-msg="模板价格" maxlength="20" /><span class="spred">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdlable">所属类型:
                                </td>
                                <td class="tdvalue">
                                    <select id="ddlType" runat="server">
                                    </select>
                                </td>


                                <td class="tdlable">可用行业:
                                </td>
                                <td class="tdvalue">
                                    <select id="ddlInduty" runat="server">
                                    </select>
                                </td>
                            </tr>

                            <tr>
                                <td class="tdlable">描述说明:
                                </td>
                                <td class="tdvalue" colspan="3">
                                    <textarea id="txtRemark" runat="server" style="width: 600px; height: 60px"></textarea>
                                </td>

                            </tr>

                            <tr id="trTempUpload" runat="server">
                                <td class="tdlable">模板附件上传(修改过的):
                                </td>
                                <td class="tdvalue">
                                    <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,1)">选择文件</a>
                                    <input type="hidden" runat="server" id="hidAtt" />
                                    <br />
                                    <div id="divAttrContainer"></div>
                                </td>
                                <td class="tdlable">展示模板上传(原模板):
                                </td>
                                <td class="tdvalue">
                                    <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,0)">选择文件</a>
                                    <input type="hidden" runat="server" id="hidAttShow" />
                                    <br />
                                    <div id="divAttrShowContainer"></div>
                                </td>
                            </tr>

                            <tr>
                                <td class="tdlable">封面图片上传:
                                </td>
                                <td class="tdvalue">
                                    <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,2)">选择照片</a>
                                    <input type="hidden" runat="server" id="hidCoverImg" /><br />
                                    <div id="divImgContainer" runat="server"></div>
                                </td>
                                <td class="tdlable">模板首页:
                                </td>
                                <td class="tdvalue">
                                    <input id="txtIndex" runat="server" class="" type="text" require-type="require"
                                        require-msg="模板首页" maxlength="20" /><span class="spred">*</span>
                                </td>
                            </tr>
                        </table>
                        <div class="row">
                            <div class="col col-md-12 text-center">
                                <div class="form-group">
                                    <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                               <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--模板内容配置--%>
                    <div class="tab-pane fade" id="mbnrpz">
                        <div class="control">
                            <div class="select">
                                <a href="javascript:void(0);" onclick="seeTempFlag();" class="btn btn-success" id="yishehen">查看模板标记</a>
                            </div>
                            <div class="search">
                                页面：<select id="seleSearch" runat="server" onchange="choicePage(this)"></select>
                            </div>
                        </div>
                        <div class="ui_table">
                            <table class="tabcolor" id="containertable">
                                <tr>
                                    <th>编号
                                    </th>
                                    <th style="width: 60%">说明
                                    </th>
                                    <th>所属页面
                                    </th>
                                </tr>
                                <asp:Repeater ID="rep_ConyainerList" runat="server">
                                    <ItemTemplate>
                                        <tr class="trpage <%#Eval("Field1") %>">
                                            <td>
                                                <input type='hidden' name="hidSearchFileld_<%#Container.ItemIndex%>" value="<%#Eval("Field1")%>" />
                                                <input type="hidden" name="hidContainerId_<%#Container.ItemIndex%>" value="<%#Eval("Id") %>" />
                                                <input type="text" name="hidContainerCode_<%#Container.ItemIndex%>" value="<%#Eval("ContainerCode") %>" readonly="readonly" />
                                            </td>
                                            <td style="width: 60%">
                                                <input type="text" name="ContainerName_<%#Container.ItemIndex%>" style="width: 80%" value="<%#Eval("ContainerName") %>" />
                                            </td>
                                            <td>
                                                <input type="text" name="hidContainerPage_<%#Container.ItemIndex%>" value="<%#Eval("ContainerPage") %>" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col col-md-12 text-center">
                                <div class="form-group">
                                    <input type="button" class="btn btn-primary" value="保存配置" onclick="setupChange()" />&emsp;&emsp;
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <div id="AttBg" class="AttBg" style="display: none;">
                </div>
                <div id="AttDiv" class="ui-dialog ui-widget ui-widget-content ui-corner-all AttDiv"
                    style="display: none;">
                    <div class="ui-dialog-titlebar ui-widget-header ui-helper-clearfix" style="padding-top: 10px; padding-right: 10px;">
                        <a href="javascript:void(0);" class="ui-dialog-titlebar-close ui-corner-all ">
                            <span class="ui-icon ui-icon-closethick " id="spForClose">
                                <img src="js/uploadify/uploadify-cancel.png" /></span> </a>
                    </div>
                    <div class="ui-dialog-content ui-widget-content" id="flash_uploader" style="max-height: 280px;">
                        <div id="people">
                            <input type="file" name="uploadify" id="uploadify" />
                            <div id="fileQueue2">
                            </div>
                        </div>
                    </div>
                    <div class="ui-dialog-buttonpane ui-widget-content ui-helper-clearfix" style="padding: 0.3em 1em 0.5em">
                        <button type="button" id="btnSaveAtts" class="btn btn-info" onclick="saveatts()">
                            上传</button>
                        &nbsp;&nbsp;
                    <button type="button" id="btnCloseAtts" class="btn btn-danger">
                        关闭</button>
                    </div>
                </div>
            </form>
        </div>
    </div>

</body>
</html>

