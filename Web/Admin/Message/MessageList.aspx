<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageList.aspx.cs" Inherits="Web.Admin.Message.MessageList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tUrl = "Handler/MessageList.ashx";
        var remark = '<%=Request.QueryString["mark"]%>';
        doSearch();
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            searchPram.push({ name: "remark", value: remark });
            SearchByCondition(searchPram);
        }
        function seeDetail(code) {
            callhtml('Member/MemberEdit.aspx?code=' + code, '会员信息');
        }
        function seeNotice(args) {
            layer.open({
                type: 2,
                title: '通知信息',
                shadeClose: true,
                shade: 0.8,
                maxmin: true, //开启最大化最小化按钮
                area: [($(window).width() - 200) + 'px', ($(window).height() - 100) + 'px'],
                content: '/Admin/Message/MessageDetail.aspx?mcode=' + args,
                btn: ['回复消息', '关闭']
              , yes: function (index) {
                  //弹出iframe，显示详细的
                  var responsemsg = parent.window['layui-layer-iframe' + index].updatenongjiinfo(); //调用iframe层方法 
                  if ($.trim(responsemsg) == '') {
                      layer.alert("回复内容不能为空！");
                      return;
                  }
                  var responseInfo = {
                      type: 'responseMsg',
                      reMsg: responsemsg,
                      mCode: args
                  };

                  var result = GetAjaxString(responseInfo);
                  if (result == "1") {
                      layer.alert("回复成功！");
                      layer.close(index);
                  }
                  else
                      layer.alert("回复失败！");
              }
            , btn2: function (index, layero) {
                //按钮【按钮二】的回调
                layer.close(index);
            }
          , cancel: function (index) {
              layer.close(index);
          }
            });
        }
        function deleteNotice(code) {
            if (confirm("确定要删除吗？删除之后该条信息里面的回复消息也将删除")) {
                var responseInfo = {
                    type: 'deleteMsg',
                    pram: code
                };

                var result = GetAjaxString(responseInfo);
                if (result == "1") {
                    layer.alert("删除成功！");
                    doSearch();
                }
                else
                    layer.alert("删除失败！");
            }
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                <input type="button" value="查询" class="ssubmit btn btn-success" onclick="doSearch()" />
                <input  id="nTitle" name="txtKey" placeholder="发送人昵称"  type="text" class="sinput" style="width:100px" />
                <input  id="nName" name="txtKey" placeholder="接收人昵称"  type="text" class="sinput" style="width:100px" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
              <thead> <tr>
                   <th>序号
                    </th>
                  <th>消息内容
                    </th>
                    <th>发送时间
                    </th>
                    <th>发送人
                    </th>
                 <th>接收人
                    </th>
                    <th>操作
                    </th>
               </tr></thead>
                 <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
               <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
        </div>
          <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td>
               <td>{{ d[i].Message}}</td>
              <td>{{ d[i].CutTime }}</td>
              <td>{{ d[i].SendName }}</td>
              <td>{{ d[i].ReceiveName}}</td>
              <td>
                    <input type='button' class='btn btn-info' value='查看' onclick="seeNotice('{{ d[i].Code }}')"/>&nbsp;
                 <input type='button' class='btn btn-danger' value='删除' onclick="deleteNotice('{{ d[i].Code }}', this)"/>
              </td>
          </tr>
        {{# } }}
         </script>
    </div>
</body>
</html>
