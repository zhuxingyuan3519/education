<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobleConfig.aspx.cs" Inherits="Web.Admin.WebAdmin.GlobleConfig" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <link href="/js/uploadify/uploadify.css" rel="stylesheet" />
    <script src="/js/uploadify/jquery.uploadify.min.js"></script>
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

        .appendImg {
        }

        #divImgContainer div {
            margin-top: 20px;
        }

        .imgdel {
            float: left;
            cursor: pointer;
        }
    
        .uploadifive-button {
            float: left;
            margin-right: 10px;
        }

        #queue {
            border: 1px solid #E5E5E5;
            overflow: auto;
            margin-bottom: 10px;
            padding: 0 3px 3px;
        }

        .filename, .fileinfo {
            color: black;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            if ($("#uploadImg").val() == "") {
                $("#imhDelimg").hide();
            }
            else {
                $("#imhDelimg").attr("onclick", "deletePic('" + $("#uploadImg").val() + "',this)");
            }
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
                            if (code == $("#hidMainPic").val())
                                $("#hidMainPic").val("");
                            $(obj).parent().remove();
                        }
                        else
                            layer.alert(info);
                    }
                });
                layer.close(index);
            });
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
                    if (tmp == "2") {
                        var img = $("#img_Yunpay");
                        var hidId = $("#hid_YunPayQCord").val("/Attachment/" + data);
                        img.attr("src", "../Attachment/" + data);
                    }
                    else if(tmp=="3"){
                        var img = $("#imgQRCodeCombine");
                        var hidId = $("#hidQRCodeCombine").val("~/Attachment/" + data);
                        img.attr("src", "../Attachment/" + data);
                    }
                }
            });
        }

        function showUpload(obj,showIndex) {
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
        <div id="finance" class="container">
            <form id="form1">
                <input type="hidden" runat="server" id="hidId" />
                <table>
                      <tr class="">
                        <td class="tdlable">网站/公司名称:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Contacter" type="text" runat="server" />
                        </td>
                      
                        <td class="tdlable">联系电话:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Phone" type="text" runat="server" />
                        </td>
                    </tr>
                    <tr class="">
                        <td class="tdlable">公司地址:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Address" type="text" runat="server" />
                        </td>
                      
                        <td class="tdlable">邮箱:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Email" type="text" runat="server" />
                        </td>
                    </tr>
                       <tr class="">
                        <td class="tdlable">QQ:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_QQ" type="text" runat="server" />
                        </td>
                      
                        <td class="tdlable">微信:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Weixin" type="text" runat="server" />
                        </td>
                    </tr>


                  <tr>
                        <td class="tdlable">提现金额基数(必须是100/50的倍数):
                        </td>
                        <td class="tdvalue">
                           <input id="txt_BaseJifen" type="text" runat="server"  require-type="decimal" require-msg="提现金额基数"/>
                        </td>
                         <td class="tdlable">缴费会员缴费金额:
                        </td>
                        <td class="tdvalue">
                           <input id="txt_Field1" type="text" runat="server"  require-type="decimal" require-msg="缴费会员缴费金额"/>
                        </td>
                    </tr>

                      <tr  >
                        <td class="tdlable">提现手续费:
                        </td>
                        <td class="tdvalue">
                            <input type="text"  id="txt_TXFloat" runat="server" require-type="decimal" require-msg="提现手续费"/>元/笔
                        </td>
                               <td class="tdlable">最低提现金额:
                        </td>
                        <td class="tdvalue">
                            <input type="text"  id="txt_MinTXMoney" runat="server" require-type="decimal" require-msg="最低提现金额"/>
                        </td>
                    </tr>

                           <tr  >
                        <td class="tdlable">体验会员使用时限:
                        </td>
                        <td class="tdvalue">
                            <input type="text"  id="txt_Field2" runat="server" require-type="int" require-msg="体验会员使用时限"/>天
                        </td>
                               <td class="tdlable">缴费会员使用时限:
                        </td>
                        <td class="tdvalue"> 
                              <input type="text"  id="txt_Field3" runat="server" require-type="int" require-msg="缴费会员使用时限"/>天
                        </td>
                    </tr>

                     <tr  style="display:none">
                     <td class="tdlable">提现积分比例:
                        </td>
                        <td class="tdvalue">
                            <input type="text"  id="txt_TXPart" runat="server"/>
                        </td>
                        
                    </tr>

                     <tr >
                        <td class="tdlable">网站LOGO:
                        </td>
                        <td class="tdvalue">
                            <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,2)">选择文件</a>
                            <br />
                            <div   class="row">
                                        <div class='col col-md-5  mainpiccontain' >
                                            <input type='hidden' id="hid_YunPayQCord"   runat="server"/>
                                            <img class='appendImg' id="img_Yunpay"  runat="server" /></div>
                            </div>
                        </td>
                         <td class="tdlable ">二维码中间附加小图片:
                        </td>
                        <td class="tdvalue ">
                            <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,3)">选择文件</a>
                            <span>*png格式(30px*30px)</span>
                            <br />
                            <div   class="row">
                                        <div class='col col-md-5  mainpiccontain' >
                                            <input type='hidden' id="hidQRCodeCombine"   runat="server"/>
                                            <img class='appendImg' id="imgQRCodeCombine"  runat="server" /></div>
                            </div>
                        </td>
                    </tr>

                    <tr style="display:none">
                        <td class="tdlable">支付宝收款:
                        </td>
                        <td class="tdvalue">
                            <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,0)">选择文件</a>
                            <br />
                            <div   class="row">
                                        <div class='col col-md-5  mainpiccontain' >
                                            <input type='hidden' id="uploadImg"   runat="server"/>
                                            <img class='appendImg' id="imgappendimg"  runat="server" style="width:300px"/></div>
                            </div>
                        </td>
                         <td class="tdlable">微信收款:
                        </td>
                        <td class="tdvalue">
                             <a href="javascript:void(0)" class="btn btn-info" title="btnUpload" onclick="showUpload(this,1)">选择文件</a>
                            <br />
                            <div   class="row">
                                        <div class='col col-md-5  mainpiccontain' >
                                            <input type='hidden' id="uploadImgWeixin" runat="server"/>
                                            <img class='appendImg' id="imgappendimgWeixin"  runat="server" style="width:300px"/></div>
                            </div>
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
