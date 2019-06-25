<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchImportWord.aspx.cs" Inherits="Web.Admin.TrainManage.BatchImportWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>批量上传下载单词</title>
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


        .div_dex input[type='text'] {
            width: 60%;
        }
        .div_add_code{padding:5px;}
    </style>
    <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
    <script src="/js/uploadifive/jquery.uploadifive.js" type="text/javascript"></script>
    <script type="text/javascript">

        function resetList() {
            callhtml('TrainManage/WordList', '词库列表');
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
            $("#div_uploadFiles").html("");
            var filetypes = 'xlsx/*';
            $('#file_upload').uploadifive({
                'auto': false,
                //'fileObjName': 'fileData', //传递到服务器的file取到的name。默认是fileData,可以不填
                //'formData': { 'uptype': 'train' },  //需要传递到服务器的数据，不用时注掉
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
                    var appendHtml = "<div class='col col-md-12 div_add_code' >";
                    appendHtml += "<div>" + file.name + "<input type='hidden' value='" + data + "' name='hid_files' require-type='require' require-msg='上传文件'/></div>";
                    $("#div_uploadFiles").append(appendHtml);
                },
                'onUploadError': function (file, data) {
                    alert(data);
                },
                'onQueueComplete': function (uploads) {     //队列中的所有文件完成上传后触发。attempts:在上次上载操作中尝试的文件上载次数    successful:上次上载操作中成功上载文件的数量  errors:在上次上载操作中返回错误的文件上载数  count:从此UploadiFive实例上载的文件总数
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
                title: ['上传文件', 'font-size:18px;background:#5bc0de'],
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


        $(function () {
            loadUploadify(1);
        });
        $(document).ready(function () {
        });

        function setAllChoice(obj) {
            //alert($(obj).prop("checked"));
            if ($(obj).prop("checked")) {
                $("#div_version").find("input[type='checkbox']").prop("checked",true);
            }
            else {
                $("#div_version").find("input[type='checkbox']").prop("checked", false);
            }
        }

    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <p>系统内已维护教材</p>
                            <p>全选<input type="checkbox"  id="chk_all" onclick="setAllChoice(this)" /></p>
                            <div style="height:400px;overflow-y:scroll" id="div_version">
                            <asp:Repeater ID="rep_versionList" runat="server">
                                <ItemTemplate>
                                    <p>
                                        <input type="checkbox" value="<%#Eval("Version") %>*<%#Eval("Grade") %>*<%#Eval("Leavel") %>" /><%#Eval("VersionName") %> <%#Eval("GradeName") %> <%#Eval("LeavelName") %>
                                    </p>
                                </ItemTemplate>
                            </asp:Repeater>
                                </div>
                             <p><input type="button" value="导出教材" class="btn btn-success" />
                                 <input type="button" value="导出教材中未匹配" class="btn btn-success" />
                                  <input type="button" value="导出教材单词顺序" class="btn btn-success" />
                             </p>
                        </div>
                    </div>

                    <div class="col col-md-6">

                        <div class="form-group">
                            <a href="javascript:void(0)" class="btn  gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,1)">批量上传Excel文件</a>
                        </div>
                        <div class="form-group" id="div_fileContainer">
                        </div>

                        <div class="form-group" id="div_uploadFiles">
                           
                        </div>
                    </div>

                </div>
                <div class="row">
                 <div class="col col-md-12">
                        <div class="form-group">
                            <label>
                                修订范围:<select id="ddl_option" runat="server" require-type="require" require-msg="修订范围" onchange="showTip(this)">
                                    <option value="1">只修订单词库</option>
                                    <option value="2">只修订教材版本</option>
                                    <option value="3">同时修订单词库及教材版本</option>
                                    <option value="4">导出新的匹配造句项目</option>
                                </select></label>
                            <label id="lb_tip" style="color: red">导入的Excel只能包含四列，列名分别为：【序号】【单词】【音标】【中文】</label>
                        </div>
                    </div>
                </div>
                <div class="row" id="div_coverImg">
                </div>

                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="刷新页面" onclick="callhtml('TrainManage/BatchImportWord', '批量导入导出');" />&emsp;&emsp;
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
        function showTip(obj) {
            if ($(obj).val() == "4") {
                $("#lb_tip").html("导入的Excel表只能一列，列名为【单词】");
            }
            else {
                $("#lb_tip").html("导入的Excel只能包含四列，列名分别为：【序号】【单词】【音标】【中文】");
            }
        }


        function checkChange() {
            if (!checkForm())
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                cache: false,
                url: 'TrainManage/' + rek + '?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var data = JSON.parse(info);
                    if (data.isSuccess == "false") {
                        layer.alert(data.msg);
                    }
                    else {
                        var option = $("#ddl_option").val();
                        if (option == "4") {
                            //下载
                            window.location.href = data.msg;
                        }
                        else {
                            layer.alert("上传成功！");
                        }
                        //$("#hidId").val(data.msg);
                        //setTimeout(function () {
                        //    callhtml('TrainManage/WordConfig', '混合词维护');
                        //}, 2000);
                    }
                }
            });
        }
    </script>
</body>
</html>
