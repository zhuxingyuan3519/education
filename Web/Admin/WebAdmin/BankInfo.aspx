<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankInfo.aspx.cs" Inherits="Web.Admin.WebAdmin.BankInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
    .tabInput{ width:98%;}
    .thCenter{ text-align:center;}
      .appendImg{ width:60px;}
                .AppTB tr
                {
                    height: 30px;
                }
                .AppTB a, #TBSrcAtt a, #TBSqlAtt a
                {
                    color: Blue;
                }
                .AttDiv
                {
                    left: 44%; /*FF IE7*/
                    top: 30%; /*FF IE7*/
                    z-index: 1002;
                    outline: 0px none;
                    height: auto;
                    width: 460px;
                    margin-left: -150px !important; /*FF IE7 该值为本身宽的一半 */
                    margin-top: -60px !important; /*FF IE7 该值为本身高的一半*/
                    margin-top: 0px;
                    position: fixed !important; /*FF IE7*/
                    position: absolute; /*IE6*/
                    background-color:rgb(37, 247, 191);
                }
                .ui-dialog-titlebar-close{ float:right;}
                .AttBg
                {
                    background-color: #FDFDFD;
                    width: 100%;
                    height: 100%;
                    left: 0;
                    top: 0; /*FF IE7*/
                    filter: alpha(opacity=50); /*IE*/
                    opacity: 0.5; /*FF*/
                    z-index: 1;
                    position: fixed !important; /*FF IE7*/
                    position: absolute; /*IE6*/
                }
    </style>
    <link href="../js/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../js/uploadify/jquery.uploadify.js" type="text/javascript"></script>
    <script type="text/javascript">
        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4());
        }
        function addRow(obj) {
            var t = guidGenerator(); //  d + h + x + s + ms;
            var ownParent = $(".arrangeTab");
            var arrangeTable = $("#tabQuestion");
            var orgin = ownParent.html();
            //添加新的行程
            var appendHtml = "<tr id='append" + t + "' class='active realContain'>" + orgin + "</tr>";
            arrangeTable.append(appendHtml);

            $("#append" + t).find(".inputCode").attr("name", "Code_" + t).attr("require-type", "require").attr("require-msg", "银行代码");
            $("#append" + t).find(".inputName").attr("name", "Name_" + t).attr("require-type", "require").attr("require-msg", "银行名称");
            $("#append" + t).find('#photo').attr('name', 'Pic_' + t).removeAttr('id');
            $("#append" + t).find('#link').attr('name', 'link_' + t).removeAttr('id');
            $("#append" + t).find('#remark').attr('name', 'remark_' + t).removeAttr('id');
            $("#append" + t).find('#banktel').attr('name', 'banktel_' + t).removeAttr('id');
            $("#append" + t).find('#bankmail').attr('name', 'bankmail_' + t).removeAttr('id');
            $("#append" + t).find('#btnUpload').attr('title', 'btnUpload_' + t).removeAttr('id');
            reCount()
        }

        function reCount() {
            $(".spCount").each(function (i) {
                $(this).html(i);
            });
        }

        function removeRow(obj) {
            if (confirm("确定要移除该行吗？")) {
                var delId = $(obj).prev('.hidId').val();
                var hidDelIds = $("#hidDelIds").val();

                if (typeof (delId) != 'undefined' && delId != "") {
                    if (typeof (hidDelIds) != 'undefined') {
                        hidDelIds += delId + ',';
                    }
                    $("#hidDelIds").val(hidDelIds);
                }
                $(obj).parent().parent().remove();
                reCount()
            }
        }
        function checkFormVal() {
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            if (checkForm()) {
                $.ajax({
                    type: 'post',
                    url: '/Admin/WebAdmin/' + rek + '?Action=Add',
                    data: $('#form1').serialize(),
                    success: function (info) {
                        layer.alert('保存成功');
                        //setTimeout(function () { v5.clearall(); }, 1000);
                        callhtml('WebAdmin/BankInfo.aspx', '银行管理');
                    }
                });
            }
        }

        var tmp = "";
        var path = "";
        function loadUploadify() {
            var list = "fileQueue2";
            $("#uploadify").uploadify({
                'swf': '../js/uploadify/uploadify.swf',
                'uploader': '../Handler/UploadExcel.ashx', //相对路径的后端脚本，它将处理您上传的文件。绝对路径前缀或'/'或'http'的路径
                'folder': '../Attachment/',
                'cancelImg': '../js/uploadify/uploadify-cancel.png',
                'script': 'UploadExcel.ashx',
                'queueID': list,
                'auto': false,
                'multi': false,
                'fileTypeDesc': '图像文件', //出现在上传对话框中的文件类型描述
                'fileTypeExts': '*.jpg;*.bmp;*.jpeg;*.gif;*.png',  //控制可上传文件的扩展名，启用本项时需同时声明fileDesc
                'buttonText': '选择文件',
                'onUploadSuccess': function (file, data, response) {
                    var img = $("input:hidden[name='Pic_" + tmp + "']").parent().find(".appendImg");
                    var hidId = $("input:hidden[name='Pic_" + tmp + "']").parent().parent().find(".hidId").val();
                    if (hidId != '') {
                        img.attr("src", "../Attachment/" + data);
                    }
                    else {
                        $("input:hidden[name='Pic_" + tmp + "']").parent().find(".appendImg").remove();
                        //alert($("input:hidden[name='Pic_" + tmp + "']").parent().html());
                        $("input:hidden[name='Pic_" + tmp + "']").parent().append(" <img class='appendImg' src='../Attachment/" + data + "' />");
                    }
                    $("input:hidden[name='Pic_" + tmp + "']").val(data);
                }
            });
        }

        function showUpload(obj) {
            //alert($(obj).attr("title"));
            tmp = $(obj).attr("title").split('_')[1];
            var attsType = $(this).attr("AttsType");
            $("#btnSaveAtts").attr("AttsType", attsType);
            $("#AttBg").show();
            $("#AttDiv").show();
            return false;
        }

        function saveatts() {
            jQuery('#uploadify').uploadify('upload', '*');
        }

        $(document).ready(function () {
            loadUploadify();

            //关闭上传附件窗口
            $("#btnCloseAtts").click(function () {
                $("#AttBg").hide();
                $("#AttDiv").hide();
//                $("#hidUpImgURL").val(fileInfo);
            });

            $("#spForClose").click(function () {
                $("#AttBg").hide();
                $("#AttDiv").hide();
            });

        });  
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
            <input type="hidden" runat="server" id="hidDelIds" />
            <div class="ui_table" style="margin: 20px">
                <table cellpadding="0" cellspacing="0" class="tabcolor" id="tabQuestion">
                    <tr>
                        <th class="thCenter">
                            编号
                        </th>
                        <th class="thCenter">
                            银行编码
                        </th>
                        <th class="thCenter">
                            银行名称
                        </th>
                          <th class="thCenter hidden">
                            办卡地址
                        </th>
                        <th class="thCenter">
                            图片展示
                        </th>
                          <th class="thCenter">
                            备注说明
                        </th>
                            <th class="thCenter">
                            联系电话
                        </th>
                          <th class="thCenter" style="display:none">
                            联系邮箱
                        </th>
                        <th class="thCenter" style="display:none">
                            操作
                        </th>
                    </tr>
                    <tr class="arrangeTab" style="display: none">
                        <td width="5%" align="center" class="tabIndex">
                            <span class="spCount"></span>
                        </td>
                        <td>
                            <input class="normal_input tabInput inputCode" type="text" />
                        </td>
                        <td>
                            <input class="normal_input tabInput inputName" type="text" />
                        </td>
                          <td class="hidden">
                            <input class="normal_input tabInput hidden" id="link" type="text" />
                        </td>
                        <td>
                        <a  class="btn btn-small inputPic" href="javascript:void(0)" id="btnUpload"  onclick="showUpload(this)">传照片</a>
                         <input type="hidden"  id="photo" /> 
                        </td>
                          <td>
                            <input class="normal_input tabInput" id="remark" type="text" />
                        </td>
                           <td>
                            <input class="normal_input tabInput" id="banktel" type="text" />
                        </td>
                         <td  style="display:none">
                            <input class="normal_input tabInput" id="bankmail" type="text" />
                        </td>
                        <td  style="display:none">
                            <input type="hidden" class="hidId" />
                            <input type="button" value="删除" class="btn btn-danger"  style="display:none" onclick="removeRow(this)" />
                        </td>
                    </tr>
                    <asp:Repeater ID="rep_List" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td width="5%" align="center">
                                    <span class="spCount">
                                        <%# Container.ItemIndex + 1%></span> :
                                </td>
                                <td>
                                    <input name="Code_<%# Container.ItemIndex + 1%>" value="<%#Eval("Code")%>" require-type="require"
                                        require-msg="银行编码" class="normal_input tabInput" type="text" readonly="readonly" />
                                </td>
                                <td>
                                    <input name="Name_<%# Container.ItemIndex + 1%>" value="<%#Eval("Name")%>" require-type="require"
                                        require-msg="银行名称" class="normal_input tabInput" type="text" />
                                </td>
                                       <td class="hidden">
                                     <input class="normal_input tabInput hidden" name="link_<%#Container.ItemIndex+1%>" value="<%#Eval("LinkUrl")%>" type="text" />
                               </td>
                                <td>
                                        <a  href="javascript:void(0)"  class="btn btn-small" title="btnUpload_<%#Container.ItemIndex+1%>"  onclick="showUpload(this)">选照片</a>
                                          <input type="hidden"  name="Pic_<%#Container.ItemIndex+1%>" value="<%#Eval("PicUrl")%>"  /> 
                                    <img  class="appendImg" src="../Attachment/<%#Eval("PicUrl")%>" />
                                </td>
                                  <td>
                            <input class="normal_input tabInput" name="remark_<%#Container.ItemIndex+1%>" value="<%#Eval("Remark")%>" type="text" />
                        </td>
                                   <td>
                            <input class="normal_input tabInput" name="banktel_<%#Container.ItemIndex+1%>" value="<%#Eval("BankTel")%>" type="text" />
                        </td>
                                   <td  style="display:none">
                            <input class="normal_input tabInput" name="bankmail_<%#Container.ItemIndex+1%>" value="<%#Eval("BankMail")%>" type="text" />
                        </td>
                                <td  style="display:none">
                                    <input type="hidden" class="hidId" value="<%# Eval("Id")%>" name="hidId_<%# Container.ItemIndex + 1%>" />
                                    <input type="button" value="删除" class="btn btn-danger" style="display:none" onclick="removeRow(this)" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <div style="background-color: #fff; text-align: right">
                    <input type="button" value="添加" class="btn btn-info" onclick="addRow(this)" />
                    <input type="button" value="保存" class="btn btn-success" onclick="checkFormVal()"
                        style="margin-right: 15%" />
                </div>
            </div>

            <div id="AttBg" class="AttBg" style="display: none; margin-top: 1%;">
            </div>
            <div id="AttDiv" class="ui-dialog ui-widget ui-widget-content ui-corner-all AttDiv"
                style="display: none;">
                <div class="ui-dialog-titlebar ui-widget-header ui-helper-clearfix" style="width: 100;
                    height: 50;">
                    <span class="ui-dialog-title" id="ui-dialog-title-alertDialog" style="visibility: visible;
                        -moz-user-select: none;">上传附件</span> <a href="javascript:void(0);" class="ui-dialog-titlebar-close ui-corner-all">
                            <span class="ui-icon ui-icon-closethick" id="spForClose">关闭</span> </a>
                </div>
                <div class="ui-dialog-content ui-widget-content" id="flash_uploader" style="max-height: 280px;">
                  <%--  <asp:HiddenField ID="hidUpImgURL" runat="server" ClientIDMode="Static" />--%>
                    <div id="people">
                     
                        <input type="file" name="uploadify" id="uploadify" />
                        <div id="fileQueue2" style="max-height: 50px;">
                        </div>
                    </div>
                </div>
                <div class="ui-dialog-buttonpane ui-widget-content ui-helper-clearfix" style="padding: 0.3em 1em 0.5em">
                   <button type="button" id="btnSaveAtts" class="btn" onclick="saveatts()">
                        上传</button>
                    &nbsp;&nbsp;
                    <button type="button" id="btnCloseAtts" class="btn">
                        关闭</button>
                </div>
            </div>
            </form>
        </div>
    </div>
</body>
</html>
