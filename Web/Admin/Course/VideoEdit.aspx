<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoEdit.aspx.cs" Inherits="Web.Admin.Course.VideoEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>视频维护</title>
    <style type="text/css">
              .uploadifive-button {
            float: left;
            margin-right: 10px;
            width:160px !important;
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
                height: 140px;
    width: 60%;
        }   
    </style>
      <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
    <script src="/js/uploadifive/jquery.uploadifive.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            if (confirm("是否确认？")) {
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var rek = '<%=Request.RawUrl%>';
                //获取最后一个/和?之间的内容，就是请求的页面
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $.ajax({
                    type: 'post',
                    url: 'Course/' + rek + '?Action=add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        if (data.isSuccess=="false") {
                            layer.alert(data.msg);
                        }
                        else {
                            layer.alert("创建成功！");
                            $("#hidId").val(data.msg);
                            //setTimeout(function () {
                            //    resetList()
                            //}, 3000);
                        }
                    }
                });
            }
        }
        function resetList() {
            callhtml('Course/VideoList.aspx', '视频列表');
        }


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
        function loadUploadify(iyt) {
            var filetypes = 'video/*';
            var buttonText = "请选择文件";
            if (iyt == 1) {
                filetypes = 'image/*';
                buttonText = "请选择一张视频封面图片";
            }
            else
                buttonText = "请选择要上传的视频文件";
            $('#file_upload').uploadifive({
                'auto': false,
                //'fileObjName': 'fileData', //传递到服务器的file取到的name。默认是fileData,可以不填
                'formData': { 'uptype': 'video' },  //需要传递到服务器的数据，不用时注掉
                'buttonText': buttonText,
                'queueID': 'queue',
                'multi': false,
                //'uploadLimit': 1,
                'queueSizeLimit': 1,
                'uploadScript': '/Handler/UploadExcel.ashx',
                'fileType': filetypes,
                'onInit': function() {
                    this.buttonText = buttonText;
                },
                'onUploadComplete': function (file, data) {
                    if (iyt == 1) {
                        $("#hid_coverImg").val(data);
                        $("#div_coverImg").html("<img  src='/Attachment/Video/" + data + "' class='img_coverimg' />");
                    }
                    else {
                        $("#sp_FileName").html(file.name);
                        $("#hid_uploadName").val(data);
                        $("#hid_realName").val(file.name);
                        $("#hid_fileType").val(file.type);
                        $("#hid_fileSize").val(file.size);
                    }
                    $('#file_upload').uploadifive('clearQueue');
              
                    $('#myModal').modal('toggle');
                },
                'onUploadError': function (file, data) {
                    alert(data);
                }
                //,'onAddQueueItem': function (file) {
                //    alert('The file ' + file.name + ' was added to the queue!');
                //}
            });
        }
        $('#myModal').on('hidden.bs.modal', function (e) {
            $('#file_upload').uploadifive('destroy');
        });
        function showUpload(obj, itype) {
            //alert(itype);
          
            loadUploadify(itype);
            $('#file_upload').uploadifive('clearQueue');
            //$("#file_upload").uploadifyCancel(queueID);
        }
        function uploadFiles() {
            //var myid = 1234;
            //$('#file_upload').data('uploadifive').settings.formData = { 'ID': myid };   //动态更改formData的值 ,需要时可以这样用
            $('#file_upload').uploadifive('upload');
        }
     
        $(document).ready(function () {
            //loadUploadify();
        });

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
                            <label for="txt_Name">视频名称：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Name" require-type="require" require-msg="名称"  style="width:80%"/>
                        </div>
                    </div>
                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_Tel">课程费用：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Fee" value="0" require-type="decimal" require-msg="课程费用" />
                        </div>
                    </div>

                       <div class="col col-md-6 hidden">
                      <div class="form-group hidden">
             <label>是否自动获得代理商资格：</label>
                <select id="ddl_Leavel" runat="server">
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
                    </div>
                  </div>
                   <div class="row">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_Name">视频描述：</label>
                            <textarea id="txt_remark" class="form-inline"  runat="server"   style="width:70%;height:50px"></textarea>
                        </div>
                    </div>
                  </div>

                    <div class="row">
                          <div class="col col-md-6">
                        <div class="form-group">
                            <a href="javascript:void(0)" class="btn  gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,1)">上传视频封面图片</a>
                            <input type="hidden" id="hid_coverImg" runat="server" />
                            <div id="div_coverImg" runat="server"></div>
                        </div>
                        <div class="form-group">
                            <a href="javascript:void(0)" class="btn  gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,0)">上传视频</a>
                            <input type="hidden" id="hid_realName" runat="server" />
                            <input type="hidden" id="hid_uploadName" runat="server" />
                            <input type="hidden" id="hid_fileType" runat="server" />
                            <input type="hidden" id="hid_fileSize" runat="server" />
                            <span id="sp_FileName" runat="server"></span>
                        </div>
                    </div>
                          <div class="col col-md-6">
                         <label for="txt_Name">观看权限：</label>
                              <div>
                                  <asp:Repeater ID="rep_PrivageList" runat="server">
                                      <ItemTemplate>
                                          <input type="checkbox" value="<%#Eval("Code") %>" name="chk_privage" <%# GetCheckStatus(Eval("Code").ToString()) %> /><%#Eval("Name") %>
                                      </ItemTemplate>
                                  </asp:Repeater>
                              </div>
                    </div>
                   </div>

                    <div class="row">
                          <div class="col col-md-12 text-center">
                               <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                             <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
                        </div>

                    </div>

                      
                   </div>

<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div class="row list-card">
                    <div class="marg addmarg" id="queue"></div>
                    <div class="marg addmarg" style="padding-left:20px">
                        <input id="file_upload" name="file_upload" type="file" multiple="false" />
                    </div>
                    <div class="marg addmarg" style="padding-left:20px">
                        <input type="button" onclick="uploadFiles()" class="btn btn-sm btn-success" style="width: 30%;" value="开始上传" />
                    </div>
                </div>
            </div>
        </div>
    </div>

            </form>
        </div>
    </div>
    
</body>
</html>
