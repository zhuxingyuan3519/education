<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="user_addinfo.aspx.cs" Inherits="Web.m.app.user_addinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
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
        .text-title{text-align:left !important;padding-left:5%}
    </style>
    <script type="text/javascript">
        function attrSelcet(va, level) {
            var addInfo = {
                //type: 'GetAddressInfo',
                type: 'GetNewAddressInfo',
                code: va,
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
                if (level == "20") {
                    ddlcity.append("<option value=''>-市-</option>");
                    $("#ddl_Zone").empty();
                }
                if (level == "30")
                    ddlcity.append("<option value=''>-区县-</option>");
                var data = JSON.parse(result);
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row list-card">
        <div class="marg">
            <span class="col-sm-12 col-xs-12" style="height: 100px">
                <a href="javascript:void(0)" data-toggle="modal" data-target="#myModal" style="text-decoration: none">
                    <img id="img_pic" src="../images/home4.png" style="    width: 80px;    height: 80px;" runat="server"  class="img-circle"/><br />
                    上传头像</a>
                <input type="hidden" runat="server" id="uploadImg" />
            </span>
        </div>

        <div id="div_org_info" runat="server">
               <div class="marg">
                     <span class="col-sm-12 col-xs-12 text-left text-title">
                      负责人姓名：
                  </span>
                <span class="col-sm-12 col-xs-12">
                    <input runat="server" type="text" class="form-control" id="txt_orgMName" placeholder="负责人" />
                </span>
            </div>

              <div class="marg">
                  <span class="col-sm-12 col-xs-12 text-left text-title">
                      负责人电话：
                  </span>
                <span class="col-sm-12 col-xs-12">
                    <input runat="server" type="text" class="form-control" id="txt_OrgTel" placeholder="负责人电话" />
                </span>
            </div>

               <div class="marg">
                     <span class="col-sm-12 col-xs-12 text-left text-title">
                      机构名称：
                  </span>
                <span class="col-sm-12 col-xs-12">
                    <input runat="server" type="text" class="form-control" id="txt_orgMID" placeholder="机构名称" disabled="disabled" />
                </span>
            </div>

               <div class="marg">
                      <span class="col-sm-12 col-xs-12 text-left text-title">
                      机构代码：
                  </span>
                <span class="col-sm-12 col-xs-12 ">
                    <input runat="server" type="text" class="form-control" id="txt_orgBranch" placeholder="机构代码"  disabled="disabled"/>
                </span>
            </div>

        </div>

        <div class="marg">
               <span class="col-sm-12 col-xs-12 text-left text-title">
                      所在地址：
                  </span>
            <span class="col-sm-4 col-xs-4">
                <select class="form-control required" id="ddl_Province" runat="server" onchange="cityChange(this,20)" style="padding: 0px">
                </select>
            </span>

            <span class="col-sm-4 col-xs-4 padding-left-right-0 ">
                <select class="form-control required" id="ddl_City" runat="server" onchange="cityChange(this,30)" style="padding: 0px">
                </select>
            </span>

            <span class="col-sm-4 col-xs-4">
                <select class="form-control required" id="ddl_Zone" runat="server" onchange="cityChange(this,40)" style="padding: 0px">
                </select>
            </span>
        </div>

        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input runat="server" type="text" class="form-control" id="txt_address" placeholder="具体地址" />
            </span>
        </div>

        <div id="div_member_info" runat="server">
            <div class="marg">
                  <span class="col-sm-12 col-xs-12 text-left text-title">
                      孩子姓名：
                  </span>
                <span class="col-sm-6 col-xs-6" >
                    <input runat="server" type="text" class="form-control" id="txt_Mname" placeholder="孩子姓名" />
                </span>
                <span class="col-sm-6 col-xs-6">
                    <select class="form-control required" id="ddl_gread" runat="server">
                    </select>
                </span>
            </div>
          
            <div class="marg">
                <span class="col-sm-12 col-xs-12 text-left text-title">家长及联系人信息</span>
                <span class="col-sm-6 col-xs-6 " style="padding-right: 0px">
                    <select class="form-control required" id="ddl_relationName" runat="server">
                        <option value="父亲">父亲</option>
                        <option value="母亲">母亲</option>
                        <option value="其他">其他</option>
                    </select>
                </span>
                <span class="col-sm-6 col-xs-6 ">
                    <input runat="server" type="text" class="form-control" id="txt_father" placeholder="家长姓名" />
                </span>
            </div>

            <div class="marg">
                   <span class="col-sm-12 col-xs-12 text-left text-title">
                      家长电话：
                  </span>
                <span class="col-sm-12 col-xs-12">
                    <input runat="server" type="text" class="form-control" id="txt_fatherTel" placeholder="家长电话" />
                </span>
            </div>
        </div>


        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">提交</a>
            </span>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div class="row list-card">
                    <div class="marg addmarg" id="queue"></div>
                    <div class="marg addmarg" style="padding-left: 20px">
                        <input id="file_upload" name="file_upload" type="file" multiple="false" />
                    </div>
                    <div class="marg addmarg" style="padding-left: 20px">
                        <input type="button" onclick="uploadFiles()" class="btn  btn-success btn-sm" style="width: 30%;" value="开始上传" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script src="/js/uploadifive/jquery.uploadifive.js"></script>
    <script type="text/javascript">

        $(function () {
            loadUploadify();
        });
        function loadUploadify() {
            var filetypes = 'image*';
            $('#file_upload').uploadifive({
                'auto': false,
                //'fileObjName': 'fileData', //传递到服务器的file取到的name。默认是fileData,可以不填
                'formData': { 'uptype': 'asset' },  //需要传递到服务器的数据，不用时注掉
                'buttonText': '选择图片',
                'queueID': 'queue',
                'multi': false,
                //'uploadLimit': 1,
                'queueSizeLimit': 1,
                'uploadScript': '/Handler/UploadExcel.ashx',
                'fileType': filetypes,
                'onUploadComplete': function (file, data) {
                    $("#uploadImg").val("/Attachment/" + data);
                    $("#img_pic").attr("src", "/Attachment/" + data);
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
        function showUpload() {
            $('#file_upload').uploadifive('clearQueue');
            //$("#file_upload").uploadifyCancel(queueID);
        }
        function uploadFiles() {
            $('#file_upload').uploadifive('upload');
        }


        function setupChange() {
            if (!checkForm())
                return false;

            layerLoading();
            var rek = 'user_addinfo';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("提交失败");
                    else if (info == "1") {  //提交成功
                        layerMsg("提交成功");
                        setTimeout(function () {
                            //自动跳转到“登录页面
                            window.location = "main_mine";
                        }, 2000);
                    }
                    else
                        layerAlert("注册失败，请重试");
                }
            });
        }


    </script>
</asp:Content>
