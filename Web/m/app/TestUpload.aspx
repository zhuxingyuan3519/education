<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="TestUpload.aspx.cs" Inherits="Web.m.app.TestUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="/common/webuploader/webuploader.css" rel="stylesheet" />
    <script src="/common/webuploader/webuploader.js"></script>
      <script type="text/javascript">
          // 文件上传  
          $(function () {
              var $list = $('#thelist'),
                 $btn = $('#ctlBtn'),
                 state = 'pending',
                 uploader;
              uploader = WebUploader.create({

                  // 不压缩image  
                  resize: false,

                  // swf文件路径  
                  swf: 'UploadResource/Uploader.swf',

                  // 文件接收服务端。  
                  server: '/Handler/UploadFiles.ashx',

                  // 选择文件的按钮。可选。  
                  // 内部根据当前运行是创建，可能是input元素，也可能是flash.  
                  pick: '#picker'
              });

              // 当有文件添加进来的时候  
              uploader.on('fileQueued', function (file) {
                  $list.append('<div id="' + file.id + '" class="item">' +
                      '<h4 class="info">' + file.name + '</h4>' +
                      '<p class="state">等待上传...</p>' +
                  '</div>');
              });

              // 文件上传过程中创建进度条实时显示。  
              uploader.on('uploadProgress', function (file, percentage) {
                  var $li = $('#' + file.id),
                      $percent = $li.find('.progress .progress-bar');

                  // 避免重复创建  
                  if (!$percent.length) {
                      $percent = $('<div class="progress progress-striped active">' +
                        '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                        '</div>' +
                      '</div>').appendTo($li).find('.progress-bar');
                  }

                  $li.find('p.state').text('上传中');

                  $percent.css('width', percentage * 100 + '%');
              });

              uploader.on('uploadSuccess', function (file, data) {
                  //alert(JSON.stringify(data));
                  $('#' + file.id).append("<img src='/Attachment/" + data.id + "' class='appendImg'/>");
                  $('#' + file.id).find('p.state').text('已上传');
              });

              uploader.on('uploadError', function (file) {
                  $('#' + file.id).find('p.state').text('上传出错');
              });

              uploader.on('uploadComplete', function (file) {
                  $('#' + file.id).find('.progress').fadeOut();
              });

              uploader.on('all', function (type) {
                  if (type === 'startUpload') {
                      state = 'uploading';
                  } else if (type === 'stopUpload') {
                      state = 'paused';
                  } else if (type === 'uploadFinished') {
                      state = 'done';
                  }

                  if (state === 'uploading') {
                      $btn.text('暂停上传');
                  } else {
                      $btn.text('开始上传');
                  }
              });

              $btn.on('click', function () {
                  if (state === 'uploading') {
                      uploader.stop();
                  } else {
                      uploader.upload();
                  }
              });
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>申请提现  <a href="tx_list.aspx" class="btn btn-success btn-sm" style="padding: 3px 8px;float:right;margin-right:5px">进度查询</a></h5>
    </div>
    <div class="row marg list-border itemlist">
        <div class="col-sm-12 col-xs-12">
            现金总额：<%=TotalMoney %>&nbsp;&nbsp;可提现总额：<span id="spCanApplyMoney"><%=GetCanTXMoney %></span>&nbsp;&nbsp;
             手续费：<%=GetTXFloat %>
        </div>
    </div>
    <div class="row list-card">
        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">申请金额</div>
            <div class="col-sm-9 col-xs-9 marg">
                <input type="text" class="form-control" id="txtMoney" require-msg="申请金额" require-type="decimal" placeholder="<%=Service.CacheService.GlobleConfig.MinTXMoney %>元以上可以提现" />
            </div>
        </div>

        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left" style="margin-top: 15px">提现到</div>
            <div class="col-sm-9 col-xs-9 marg" style="padding-bottom: 15px">
                <input type="radio" name="rdTx" value="1" checked="checked" style="margin-top: 13px;" onclick="showTXUpload('1', '2')" />支付宝&emsp;
                <input type="radio" name="rdTx" value="2" style="margin-top: 13px;"  onclick="showTXUpload('2', '1')" />微信
            </div>
        </div>


        <div class="marg tx1">
            <div class="col-sm-3 col-xs-3 margs text-left">
                支付宝<br />
                提现信息
            </div>
            <div class="col-sm-9 col-xs-9 marg">
                <input type="button" value="上传支付宝收款二维码" class="btn btn-success btn-sm" title="btnUpload" data-toggle="modal" data-target="#myModal"  />
            </div>
               <div  class="col-sm-12 col-xs-12 marg mainpiccontain" >
                    <input type='hidden' id="uploadImg" runat="server" class="uploadImg" />
                    <img class='appendImg' id="imgappendimg" runat="server" />
                </div>
        </div>
        <div class="marg tx2" style="display:none">
            <div class="col-sm-3 col-xs-3 margs text-left">
                微信<br />
                提现信息
            </div>
            <div class="col-sm-9 col-xs-9 marg">
                <a href="javascript:void(0)" class="btn btn-success btn-sm" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,1)">上传微信收款二维码</a>
            </div>
            <div  class="col-sm-12 col-xs-12 marg mainpiccontain" >
                    <input type='hidden' id="uploadImgWeixin" runat="server" class="uploadImgWeixin" />
                    <img class='appendImg' id="imgappendimgWeixin" runat="server" />
                </div>
        </div>


              </div>
    <div>
                    
      <div id="uploader" class="wu-example">  
        <div id="thelist" class="uploader-list"></div>  
        <div class="btns">  
        <div id="picker">选择文件</div>  
            <input id="ctlBtn" class="btn btn-default" value="开始上传" />
        </div>  
     </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
  
</asp:Content>
