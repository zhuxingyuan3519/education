<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseList.aspx.cs" Inherits="Web.Admin.Course.CourseList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        ul li {
            list-style: none;
        }
        .commonSearchKey {
            width: 100px;
            padding-left:2px !important;
            padding-right:2px  !important;
        }
        .div_paeizhi {
            width: 50%;
            padding-top:10px;
    float: left;}
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/CourseList.ashx";
        $(function () {
            doSearch();
        });
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            SearchByCondition(searchPram);
        }
        function seeDetail(code) {
            callhtml('Course/CourseEdit?id=' + code, '名人大咖信息');
        }
        function addNew() {
            callhtml('Course/CourseEdit', '新增名人大咖');
        }

        function setPrize(code, name, obj) {
            $('.chkbox').each(function () {
                $(this).prop("checked","");
            });
            var userInfo = {
                type: 'getCoursePrizeList',
                code: code,
            };
            var result = GetAjaxString(userInfo);
            if (result != '0') {
                var jsonArray = JSON.parse(result);
                $.each(jsonArray.Rows, function (index,item) {
                    $("#chk_"+item.Code).prop("checked","checked");
                });
            }

            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['选择需要配置的奖项', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['去配置', '取消'],
                area: ['380px', '295px'],
                yes: function (index) {
                    //获取到选中的项目
                    var chkvalues = '';
                    $(".div_paeizhi").each(function () {
                        var chk = $(this).find("input[type='checkbox']");
                        if (chk.prop('checked')) {
                            chkvalues += chk.val() + ';';
                        }
                    });
                    if (chkvalues == '') {
                        layer.alert('请选择配置项目');
                        return false;
                    }
                    layer.close(index);
                    callhtml('Course/CourseSHMoney?ccode=' + code + '&chk=' + chkvalues,name+ '课程奖项配置');
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });

        }
        

        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteCourse',
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
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                      <input  id="nName" placeholder="名人大咖名称"  type="text" class="sinput commonSearchKey" />
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
                <input type="button" value="新增" class="btn btn-danger" onclick="addNew()" />
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
                        名人大咖名称
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
                    <li class="tdlable"  >
                   <asp:Repeater ID="rep_shmoneyList" runat="server">
                       <ItemTemplate>
                           <div class="div_paeizhi">
                           <input type="checkbox" class="chkbox" id="chk_<%#Eval("Code") %>"  value="<%#Eval("Code") %>" /><%#Eval("Name") %>
                               </div>
                       </ItemTemplate>
                   </asp:Repeater>
                    </li>
                </ul>

    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Name }}</td>
              <td>
                  <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].Code }}')"/>&nbsp;
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
               <input type='button' class='btn btn-danger hidden' value='奖项配置' onclick="setPrize('{{ d[i].Code }}','{{ d[i].Name }}', this)"/>
              </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
