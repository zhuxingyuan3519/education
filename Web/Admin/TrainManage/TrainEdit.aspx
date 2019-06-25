<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainEdit.aspx.cs" Inherits="Web.Admin.TrainManage.TrainEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编码维护</title>
    <style type="text/css">
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

        .img_coverimg {
            height: 150px;
            width: 160px;
        }

        .div_dex {
            padding: 5px;
        }

            .div_dex input[type='text'] {
                width: 60%;
            }

            .div_chengyu  input[type='text'] {
                width: 75%;
            }

        .div_add_code {
            border: solid #9E9EA9 1px;
            border-right: none;
            border-bottom: none;
        }

        #div_coverImg {
            border-bottom: solid #9E9EA9 1px;
        }
    </style>
    <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
    <script src="/js/uploadifive/jquery.uploadifive.js" type="text/javascript"></script>
    <script type="text/javascript">

        function resetList() {
            callhtml('TrainManage/TrainList', '编码规则');
        }


        function deletePic(code, dtype, obj) {
            layer.confirm('确定要删除吗?', function (index) {
                var userInfo = {
                    type: 'DeleteTrainCoding',
                    pram: code,
                    dtype: dtype
                };
                var result = GetAjaxString(userInfo);
                if (result == "1") {
                    $(obj).parent().parent().remove();
                    layer.msg("删除成功");
                }
                else {
                    layer.alert("删除失败，请重试");
                }

            });
        }
        function loadUploadify(iyt) {
            var filetypes = 'image/*';
            $('#file_upload').uploadifive({
                'auto': false,
                //'fileObjName': 'fileData', //传递到服务器的file取到的name。默认是fileData,可以不填
                'formData': { 'uptype': 'train' },  //需要传递到服务器的数据，不用时注掉
                'buttonText': '选择文件',
                'queueID': 'queue',
                'multi': true,
                //'uploadLimit': 1,
                // 'queueSizeLimit': 1,
                'uploadScript': '/Handler/UploadExcel.ashx',
                'fileType': filetypes,
                'onInit': function (file, data) {
                    $('#file_upload').uploadifive('clearQueue');
                },
                'onQueueComplete': function (file, data) {
                    $('#file_upload').uploadifive('clearQueue');
                },
                'onUploadComplete': function (file, data) {
                    //console.log(JSON.stringify(file));
                    //console.log(JSON.stringify(data));
                    var appendHtml = "<div class='col col-md-2 div_add_code' >";
                    appendHtml += "<div><span onclick=\"deletePic('" + data + "',0,this)\"  style=\"font-size:16px\">X</span> </div>";
                    appendHtml += "<div><img  src='/Attachment/Train_Thumb/" + data + "' class='img_coverimg' /><input type='hidden' value='" + data + "' class='txt-coverimg' /></div>";
                    appendHtml += "<div class='div_dex'>编码编号:<input type='text' class='txt-PKCode' require-type='require' require-msg='编码编号'/></div>";
                    appendHtml += "<div class='div_dex'>编码名称:<input type='text' class='txt-CodeName'  require-type='require' require-msg='编码名称'/></div>";
                    appendHtml += "<div class='div_dex'>编码顺序:<input type='text' class='txt-codeSort'  require-type='int' require-msg='编码排序'/></div>";
                    if ($("#ddl_codeType").val() == "3") {
                        //扑克编码需要增加黑红梅芳选项、增加扑克数字
                        appendHtml += "<div class='div_dex'>扑克数字:<input type='text' class='txt-pukeNum'  require-type='require' require-msg='扑克数字'/></div>";
                        appendHtml += "<div class='div_dex'><input type='hidden' value='' class='txt-puketype' /><input  type='radio' onclick='checkPuKe(1,this)' name='" + data + "' valie='1' />黑&nbsp;<input type='radio' onclick='checkPuKe(2,this)' name='" + data + "' valie='2' />红&nbsp;<input type='radio'  onclick='checkPuKe(3,this)'  name='" + data + "' valie='3' />梅&nbsp;<input type='radio' onclick='checkPuKe(4,this)'  name='" + data + "' valie='4' />方 </div>";
                    }
                  else if ($("#ddl_codeType").val() == "4") {
                        //超级密码下加一个成语
                      appendHtml += "<div class='div_dex div_chengyu'>成语:<input type='text' class='txt-pukeNum'  /></div>";
                    }
                    appendHtml += "</div>";
                    $("#div_coverImg").append(appendHtml);

                    //$('#file_upload').uploadifive('clearQueue');
                    //$('#myModal').modal('toggle');
                },
                'onUploadError': function (file, data) {
                    alert(data);
                },
                'onQueueComplete': function (uploads) {     //队列中的所有文件完成上传后触发。attempts:在上次上载操作中尝试的文件上载次数    successful:上次上载操作中成功上载文件的数量  errors:在上次上载操作中返回错误的文件上载数  count:从此UploadiFive实例上载的文件总数
                    $("#div_coverImg").children(".div_add_code:last-child").css("border-right", "solid #9E9EA9 1px");
                }
                //,'onAddQueueItem': function (file) {
                //    alert('The file ' + file.name + ' was added to the queue!');
                //}
            });
        }

        function checkPuKe(iup, obj) {
            $(obj).parent().find(".txt-puketype").val(iup);
        }

        function showUpload(obj, itype) {
            loadUploadify(itype);
            $('#file_upload').uploadifive('clearQueue');
            //$("#file_upload").uploadifyCancel(queueID);

            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['上传图片', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['开始上传', '取消'],
                area: ['620px', '195px'],
                yes: function (index) {
                    uploadFiles();
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }
        function uploadFiles() {
            //var myid = 1234;
            //$('#file_upload').data('uploadifive').settings.formData = { 'ID': myid };   //动态更改formData的值 ,需要时可以这样用
            $('#file_upload').uploadifive('upload');
        }


        function setRadioDefaultChecked() {
            $(".txt-puketype").each(function () {
                var checkVal = $(this).val();
                //console.log(checkVal);
                $(this).parent().find("input[type='radio'][value='" + checkVal + "']").prop("checked", "checked");
            });
        }


        $(function () {
            console.log("oning")
            setRadioDefaultChecked();
            loadUploadify(1);
        });
        $(document).ready(function () {
        });

    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />
                <div class="row">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <label>
                                编码类型:<select id="ddl_codeType" runat="server" disabled="disabled">
                                    <option value="">--密码类型--</option>
                                    <option value="1">数字编码</option>
                                    <option value="2">字母编码</option>
                                    <option value="3">扑克编码</option>
                                    <option value="4">超级密码</option>
                                </select></label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <a href="javascript:void(0)" class="btn  gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,1)">上传编码图片</a>
                        </div>
                    </div>
                </div>
                <div class="row" id="div_coverImg">
                    <asp:Repeater ID="rep_list" runat="server">
                        <ItemTemplate>
                            <div class='col col-md-2 div_add_code' style="text-align: right">
                                <div><span onclick="deletePic('<%#Eval("Code") %>',1,this)" style="font-size: 16px">X</span> </div>
                                <div>
                                    <img src='/Attachment/Train_Thumb/<%#Eval("CodeImage") %>' class='img_coverimg' /><input type='hidden' value='<%#Eval("CodeImage") %>' class='txt-coverimg' />
                                </div>
                                <div class='div_dex'>编码编号:<input type='text' value="<%#Eval("PKCode") %>" class='txt-PKCode' require-type="require" require-msg="编码编号" /></div>
                                <div class='div_dex'>编码名称:<input type='text' value="<%#Eval("CodeName") %>" class='txt-CodeName' require-type="require" require-msg="编码名称" /></div>
                                <div class='div_dex'>编码顺序:<input type='text' value="<%#Eval("Sort") %>" class='txt-codeSort' require-type="int" require-msg="排序" /></div>

                               
                                 <%#Eval("CodeType").ToString()=="3"?"<div class='div_dex'>扑克数字:<input type='text' class='txt-pukeNum'  value='"+Eval("PuKeNum")+"' require-type='require' require-msg='扑克数字'/></div><div class='div_dex'><input type='hidden' value='" + Eval("Remark")  + "' class='txt-puketype' /><input  type='radio' onclick='checkPuKe(1, this)' name='" + Eval("CodeImage")  + "' value='1' />黑&nbsp;<input type='radio' onclick='    checkPuKe(2, this)' name='" + Eval("CodeImage")  + "'   value='2'/>红&nbsp;<input type='radio'  onclick='    checkPuKe(3, this)'  name='" + Eval("CodeImage")  + "'  value='3'/>梅&nbsp;<input type='radio' onclick='    checkPuKe(4, this)'  name='" + Eval("CodeImage")  + "'  value='4' />方 </div>":"" %>
                                 <%#Eval("CodeType").ToString()=="4"?"<div class='div_dex div_chengyu'>成语:<input type='text' class='txt-pukeNum'  value='"+Eval("PuKeNum")+"'  /></div>":"" %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                             <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
                        </div>
                    </div>
                </div>




                <div id="layLoginUI" class="layer_notice layui-layer-wrap" style="display: none; width: 98%">
                    <div class="row list-card">
                        <div class="marg addmarg" id="queue"></div>
                        <div class="marg addmarg" style="padding-left: 20px">
                            <input id="file_upload" name="file_upload" type="file" multiple="false" />
                        </div>
                        <div class="marg addmarg" style="padding-left: 20px">
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">

   
        function checkChange() {
            if (!checkForm())
                return;
            //构建json数组
            var josnArry = [];
            var PKCode, CodeName, codeSort, coverimg, jsonObj, codeRemark = '',pukeNum='';
            var codeType = $("#ddl_codeType").val();
            if (codeType == "") {
                layer.alert("请选择密码类型");
                return false;
            }
            var checkval = false;
            $(".div_add_code").each(function () {
                PKCode = $(this).find(".txt-PKCode").val();
                CodeName = $(this).find(".txt-CodeName").val();
                codeSort = $(this).find(".txt-codeSort").val();
                coverimg = $(this).find(".txt-coverimg").val();
                if (codeType == "3") {
                    codeRemark = $(this).find(".txt-puketype").val();
                    pukeNum = $(this).find(".txt-pukeNum").val();
                    if (codeRemark == '') {
                        checkval = true;
                        return false;
                    }
                    if (pukeNum == '') {
                        checkval = true;
                        return false;
                    }
                }
                else if (codeType == "4") {
                    pukeNum = $(this).find(".txt-pukeNum").val();
                }
                //console.log(codeRemark);
                if (PKCode == '' || CodeName == '') {
                    checkval = true;
                    return false;
                }
                //加入数组
                jsonObj = { "CodeType": codeType, "PKCode": PKCode, "CodeName": CodeName, "CodeImage": coverimg, "Sort": codeSort, "Remark": codeRemark,"PuKeNum":pukeNum };
                josnArry.push(jsonObj);
            });
            if (checkval) {
                layer.alert("请检查编码或名称是否都已完善");
                return false;
            }
            //return false;
            var userInfo = {
                type: 'SaveTrainCoding',
                pram: JSON.stringify(josnArry)
            };
            var result = GetAjaxString(userInfo);
            if (result == "1") {
                layer.alert("保存成功");
            }
            else {
                layer.alert("保存失败，请重试");
            }
        }
    </script>
</body>
</html>
