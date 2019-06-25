<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebConfigEdit.aspx.cs" Inherits="Web.Admin.WebAdmin.WebConfigEdit"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <style type="text/css">
        .listContent{    padding-left: 20px;width:50%;float:left;
    padding-top: 10px;}
        .confiTextarea{width:90%;height:50px;}
        /*.uploadedimg{height:300px;}*/
        input[type='text']{width:80% !important;}
    </style>
       <link href="/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="/js/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
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
                    $(".img-" + tmp).attr("src", "../Attachment/" + data).css("height","250px");
                    $(".hidimg-"+tmp).val("/Attachment/" + data);
                }
            });
        }

        function showUpload(obj, showIndex) {
            //alert($(obj).attr("title"));
            tmp = showIndex;
            var attsType = $(this).attr("AttsType");
            $("#btnSaveAtts").attr("AttsType", attsType);
            $("#AttBg").show();
            $("#AttDiv").show();
            return false;
        }

        function saveatts() {
            jQuery('#uploadify').uploadify('upload', '*');
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <div id="contentDiv" runat="server">
                </div>


                    <div class="row">
                    <div class="col col-md-12" style="    text-align: center;
    padding-top: 20px;">
                        <input type="button" class="normal_btnok" value="保存" onclick="checkChange();" />

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

    <script type="text/javascript">
        function checkChange() {
            var rek = '<%=Request.RawUrl%>';
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                var index = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
                $.ajax({
                    type: 'post',
                    url: '/Admin/WebAdmin/' + rek + '?Action=Add',
                    data: $('#form1').serialize(),
                    success: function (info) {
                        layer.close(index);
                        if (info == "1") {
                            alert('发布成功');
                        }
                        else {
                            alert('操作失败，请重试！');
                        }
                    }
                });
            }
    </script>
</body>
</html>
