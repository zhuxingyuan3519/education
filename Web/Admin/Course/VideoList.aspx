<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoList.aspx.cs" Inherits="Web.Admin.Course.VideoList" %>
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
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .div_paeizhi {
            width: 50%;
            padding-top: 10px;
            float: left;
        }
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/VideoList.ashx";
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
            callhtml('Course/VideoEdit?id=' + code, '视频信息');
        }
        function addNew() {
            callhtml('Course/VideoEdit', '新增视频');
        }



        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteVideo',
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
                      <input  id="nName" placeholder="视频名称"  type="text" class="sinput commonSearchKey" />
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
                        视频名称
                    </th>
                    <th>
                        视频大小
                    </th>
                     <th>
                        观看等级
                    </th>
                     <th>
                        创建时间
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

    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Title }}</td>
              <td>{{ d[i].SizeString }}</td>
                 <td>{{ d[i].Authority }}</td>
              <td>{{ d[i].CutTime }}</td>
              <td>
                  <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].Code }}')"/>&nbsp;
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
              </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
