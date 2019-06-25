<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaperList.aspx.cs" Inherits="Web.Admin.Evaluation.PaperList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>测评试卷</title>
    <style type="text/css">
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
        .ddlpaper{width:180px;margin-top:10px;}
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Evaluation/PaperList?action=query";
        //searchPram = [];
        //SearchByCondition(searchPram);

        $(function () {
            doSearch(0);
        })

        function deleteVersion() {
            if (confirm("确定要删除吗？")) {
                if ($("#ddl_Version").val() == "" || $("#ddl_Grade").val() == "" || $("#ddl_Leavel").val() == "") {
                    alert("请选择教材、年级、学期");
                    return false;
                }

                var userInfo = {
                    type: 'deleteWordVersion',
                    version: $("#ddl_Version").val(),
                    grade: $("#ddl_Grade").val(),
                    leavel: $("#ddl_Leavel").val()
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
            searchPram.push({ name: $("#ddl_Version").attr("id"), value: $("#ddl_Version").val() });
            searchPram.push({ name: $("#ddl_Grade").attr("id"), value: $("#ddl_Grade").val() });
            searchPram.push({ name: $("#ddl_Leavel").attr("id"), value: $("#ddl_Leavel").val() });
            searchPram.push({ name: $("#ddl_unit").attr("id"), value: $("#ddl_unit").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            SearchByCondition(searchPram);
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


        function moreOperate() {
            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['添加测评试卷', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['保存', '取消'],
                area: ['480px', '300px'],
                yes: function (index) {
                    //保存操作
                    var userInfo = {
                        version: $("#ddl_PaperVersion").val(),
                        grade: $("#ddl_PaperGrade").val(),
                        leavel: $("#ddl_PaperLeavel").val(),
                        unit: $("#ddl_PaperUnit").val()
                    };
                    var loadingindex = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                    $.ajax({
                        type: 'post',
                        cache: false,
                        url: 'Evaluation/PaperList?Action=add',
                        data: userInfo,
                        success: function (info) {
                            layer.close(loadingindex);
                            var data = JSON.parse(info);
                            if (data.isSuccess == "false") {
                                layer.alert(data.msg);
                            }
                            else {
                                layer.alert(data.msg);
                                doSearch(0);
                            }
                        }
                    });

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
                <select id="ddl_Version" class="commonSearchKey"  runat="server">
                </select>
                  <select id="ddl_Grade" class="commonSearchKey"  runat="server">
                </select>
                  <select id="ddl_Leavel" class="commonSearchKey"  runat="server">
                </select>

                    <select id="ddl_unit" class="commonSearchKey"  runat="server">
                </select>
                   <input  id="nName" placeholder="试卷名称"  type="text" class="sinput commonSearchKey" />

                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch(0)" />
                <input type="button" value="新增" class="btn btn-info" onclick="moreOperate()" />
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
                        试卷名称
                    </th>
                     <th>
                        单词数量
                    </th>
                     <th>
                        测评人数
                    </th>
                     <th>
                        创建时间
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
                    <li class="tdlable">
                        版本：  <select id="ddl_PaperVersion" class="ddlpaper"  runat="server">
                                     </select>
                    </li>
                 <li class="tdlable" >
                        年级：  <select id="ddl_PaperGrade"   class="ddlpaper" runat="server">
                                     </select>
                    </li>
                 <li class="tdlable">
                        章节：  <select id="ddl_PaperLeavel" class="ddlpaper"  runat="server">
                                     </select>
                    </li>
                 <li class="tdlable">
                        单元：  <select id="ddl_PaperUnit"  class="ddlpaper" runat="server">
                                     </select>
                    </li>
                </ul>

    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Name }}
              <td>{{ d[i].WordCount }}</td><td>{{ d[i].EvaluationCount }}</td>
              <td>{{ d[i].CreatedTime }}</td>  
              <td  class="hidden">
                  <input type='button' class='btn btn-info' value='修改' onclick="seeDetail('{{ d[i].Code }}')"/>
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
