<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordList.aspx.cs" Inherits="Web.Admin.TrainManage.WordList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>词库列表</title>
    <style type="text/css">
        ul li {
            list-style: none;
        }

        .commonSearchKey {
            width: 100px;
            padding-left: 2px !important;
            padding-right: 2px !important;
        }
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/ImportWordList.ashx";
        //searchPram = [];
        //SearchByCondition(searchPram);

        $(function () {
            doSearch(0);
        })

        //查询，加入参数
        function doSearch(isexport) {
            searchPram = [];
            searchPram.push({ name: "word", value: "1" });
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            if (isexport == "1") {
                searchPram.push({ name: "export", value: "1" });
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                //ajax获取
                $.ajax({
                    type: "post", cache: false, url: tUrl, dataType: "json", data: searchPram, success: function (res) {
                        layer.close(index);
                        if (res.isSuccess == "false")
                            layer.alert(res.msg);
                        else
                          window.location.href = res.msg;
                    }
                });
                //layer.close(loadIndex);
            }
            else {
                SearchByCondition(searchPram);
            }
        }
        var hidType = $("#hidType").val();
        var addMsg = "学员";
        if (hidType == "Training") {
            addMsg = "机构";
        }
        else if (hidType == "Agent") {
            addMsg = "代理商";
        }
        function seeDetail(code) {
            callhtml('TrainManage/EditWord?id=' + code,  '单词修改');
        }
        function addNew() {
            callhtml('TrainManage/EditWord', '添加单词');
        }

        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteWord',
                code: code,
            };
            var result = GetAjaxString(userInfo);
            if (result == "1") {
                alert("删除成功");
                $(obj).parent().parent().remove();
            }
            else if (result == "0") {
                alert("删除失败，请重试！");
            }
            else {
                alert(result);
            }
        }

        function trunLogPage() {
            layer.closeAll();
            var mid = $("#btnSeeLog").data("mid");
            callhtml('Log/LogList.aspx?mid=' + mid, '操作日志');
        }

        function seeRank() {
            var url = '/Admin/Member/StructNet.aspx';
            window.open(url);
        }


        function moreOperate(obj) {
            var conte = $(obj).parent().find(".div_Module1").html();
            $("#li_showContaine").html(conte);
            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['记忆方法', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['关闭', '取消'],
                area: ['520px', '395px'],
                yes: function (index) {
                    layer.close(index);
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }


    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                 <input  id="nTitle" placeholder="英文"  type="text" class="sinput commonSearchKey" />
                      <input  id="nName" placeholder="中文"  type="text" class="sinput commonSearchKey" />

                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch(0)" />
                <input type="button" value="上传" class="btn btn-info" onclick="callhtml('TrainManage/ImportWord', '上传词典');" />
                 <input type="button" value="导出" class="btn btn-info" onclick="doSearch(1);" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                  <thead>
                <tr>
                    <th>
                        序号
                    </th>
                      <th>
                        英文
                    </th>
                    <th>
                        音标
                    </th>
                     <th style="width:25%">
                        中文
                    </th>
                     <th>
                        记忆方法
                    </th>
                     <th>
                        操作
                    </th>
                </tr>  </thead>
                      <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                  <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
        </div>

           <ul class="layer_notice layui-layer-wrap" id="layLoginUI" style="display: none;">
                    <li style="height:10px;width:100%"></li>
                    <li class="tdlable"  id="li_showContaine">

                    </li>
                </ul>

    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].English }}</td>
              <td>{{ d[i].Phonetic }}</td>  
               <td>{{ d[i].Chinese }}</td>
                    <td>
                        <div class="div_Module1 hidden">
                            {{ d[i].Module1 }}<br />
                            {{ d[i].Module2 }}<br />
                            {{ d[i].Module3 }}<br />
                            {{ d[i].Module4 }}
                        </div>
                         <input type='button' class='btn btn-default' value='查看' onclick="moreOperate(this)"/>&nbsp;</td>
              <td>
                  <input type='button' class='btn btn-info' value='修改' onclick="seeDetail('{{ d[i].Code }}')"/>
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
