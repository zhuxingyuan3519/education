<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportWordList.aspx.cs" Inherits="Web.Admin.TrainManage.ImportWordList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>教材词集</title>
    <style type="text/css">
        #mempay .control .search {
    float: left;
}
        ul li {
            list-style: none;
        }

        .commonSearchKey {
            width: 100px;
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .sp_checked {
            color: red;
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

        function deleteVersion() {
            if (confirm("确定要删除吗？")) {
                if ($("#ddl_Version").val() == "" || $("#ddl_Grade").val() == "" || $("#ddl_Leavel").val() == "" || $("#ddl_Unit").val() == "") {
                    alert("请选择教材、年级、学期、章节");
                    return false;
                }

                var userInfo = {
                    type: 'deleteWordVersion',
                    version: $("#ddl_Version").val(),
                    grade: $("#ddl_Grade").val(),
                    leavel: $("#ddl_Leavel").val(),
                    unit: $("#ddl_Unit").val()
                };
                var result = GetAjaxString(userInfo);
                if (result == "1") {
                    alert("删除成功");
                    doSearch(0);
                }
                else {
                    alert("删除失败，请重试");
                }


            }
        }

        //查询，加入参数
        function doSearch(isexport) {
            searchPram = [];

            searchPram.push({ name: $("#ddl_Sort").attr("id"), value: $("#ddl_Sort").val() });
            searchPram.push({ name: $("#ddl_Version").attr("id"), value: $("#ddl_Version").val() });
            searchPram.push({ name: $("#ddl_Grade").attr("id"), value: $("#ddl_Grade").val() });
            searchPram.push({ name: $("#ddl_Leavel").attr("id"), value: $("#ddl_Leavel").val() });
            searchPram.push({ name: $("#ddl_Unit").attr("id"), value: $("#ddl_Unit").val() });
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            if (isexport == "1" || isexport == "2" || isexport == "3") {
                if ($("#ddl_Version").val() == "" || $("#ddl_Grade").val() == "" || $("#ddl_Leavel").val() == "") {
                    alert("教材、年级、学期都需要选择才能下载");
                    return false;
                }
                searchPram.push({ name: "export", value: "1" });
                if (isexport == "2")
                    searchPram.push({ name: "nomatch", value: "1" });
                else if (isexport == "1")
                    searchPram.push({ name: "nomatch", value: "0" });
                else if (isexport == "3") {
                    searchPram.push({ name: "nomatch", value: "0" });
                    searchPram.push({ name: "exportsort", value: "1" });
                }
                else if (isexport == "4") {
                    searchPram.push({ name: "nomatch", value: "0" });
                    searchPram.push({ name: "allsort", value: "1" });
                    searchPram.push({ name: "exportsort", value: "1" });
                }
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
            callhtml('Member/MemberEdit?id=' + code + '&type=' + $("#hidType").val(), addMsg + '信息');
        }
        function addNew() {
            callhtml('Member/MemberEdit?type=' + $("#hidType").val(), '添加' + addMsg);
        }

        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteMemberTrain',
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
                 <select id="ddl_Sort" class="commonSearchKey" >
                 <option value="">--排序规则--</option>
                  <option value="1">字母顺序</option>
                      <option value="2">索引顺序</option>
                </select>
                <select id="ddl_Version" class="commonSearchKey"  runat="server">
            
                </select>
                  <select id="ddl_Grade" class="commonSearchKey"  runat="server">
               
                </select>
                  <select id="ddl_Leavel" class="commonSearchKey"  runat="server">
                  
                </select>

                     <select id="ddl_Unit" class="commonSearchKey"  runat="server">
                  
                </select>

                 <input  id="nTitle" placeholder="英文"  type="text" class="sinput commonSearchKey" />
                      <input  id="nName" placeholder="中文"  type="text" class="sinput commonSearchKey" />
                <br />
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch(0)" />
                <input type="button" value="上传" class="btn btn-info" onclick="callhtml('TrainManage/ImportWord', '上传词典');" />
                 <input type="button" value="导出" class="btn btn-info" onclick="doSearch(1);" />
                   <input type="button" value="导出未匹配" class="btn btn-success" onclick="doSearch(2);" />
                <input type="button" value="导出教材顺序" class="btn btn-success" onclick="doSearch(3);" />
                 <input type="button" value="一键导出所有教材顺序" class="btn btn-danger" onclick="doSearch(4);" />
                  <input type="button" value="删除" class="btn btn-danger" onclick="deleteVersion()" />
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
                        教材版本
                    </th>
                     <th>
                        单元
                    </th>
                     <th>
                        顺序
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
                     <th class="hidden">
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
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Version }}{{ d[i].Grade }}{{ d[i].Leavel }}</td><td>{{ d[i].Unit }}</td>
              <td>{{ d[i].WIndex }}</td><td>{{ d[i].English }}</td>
              <td>{{ d[i].Phonetic }}</td>  
               <td>{{ d[i].Chinese }}</td>
                    <td>
                        <div class="div_Module1 hidden">
                            {{ d[i].Module1 }}<br />
                            {{ d[i].Module2 }}<br />
                            {{ d[i].Module3 }}<br />
                            {{ d[i].Module4 }}
                        </div>
                         <input type='button' class='btn btn-info' value='查看' onclick="moreOperate(this)"/>&nbsp;</td>
              <td  class="hidden">
                  <input type='button' class='btn btn-info' value='修改' onclick="seeDetail('{{ d[i].Code }}')"/>
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
