<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeList.aspx.cs" Inherits="Web.Admin.Message.NoticeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var NType = '<%=NType %>';
        var Bank = '<%=bank %>';
        var CType = '<%=type %>';
        $("#nType").val(NType);
        $("#nBank").val(Bank);
        $("#nCType").val(CType);
        tUrl = "/Admin/Handler/NoticeList.ashx";
        searchPram = [];
        searchPram.push({ name: $("#nType").attr("id"), value: $("#nType").val() });
        searchPram.push({ name: $("#nBank").attr("id"), value: $("#nBank").val() });
        searchPram.push({ name: $("#nCType").attr("id"), value: $("#nCType").val() });
        SearchByCondition(searchPram);
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nType").attr("id"), value: $("#nType").val() });
            searchPram.push({ name: $("#nBank").attr("id"), value: $("#nBank").val() });
            searchPram.push({ name: $("#nCType").attr("id"), value: $("#nCType").val() });
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            SearchByCondition(searchPram);
        }
        function setPublish() {
            if (NType == "1")
                callhtml('Message/NoticeAdd.aspx?id=' + NType, '发布公告');
            else if (NType == "2") {
                if (Bank == "") {
                    if (CType == '0')
                        callhtml('Message/NoticeAdd.aspx?id=' + NType + '&type=' + CType, '发布信用卡常识');
                    else if (CType == '1')
                        callhtml('Message/NoticeAdd.aspx?id=' + NType + '&type=' + CType, '发布信用卡提额技术');
                }
                else {
                    if (CType == '0')
                        callhtml('Message/NoticeAdd.aspx?id=' + NType + '&bank=' + Bank + '&type=' + CType, '发布信用卡常识');
                    else if (CType == '1')
                        callhtml('Message/NoticeAdd.aspx?id=' + NType + '&bank=' + Bank + '&type=' + CType, '发布信用卡提额技术');
                }
            }
            else {
                var titlebook = $(".alert-danger strong").html();
                //alert(titlebook);
                //if (NType == "3")
                    callhtml('Message/NoticeAdd.aspx?id=' + NType, titlebook);
                //else if (NType == "4")
                //    callhtml('Message/NoticeAdd.aspx?id=' + NType, '发布信用贷款');
                //else if (NType == "5")
                //    callhtml('Message/NoticeAdd.aspx?id=' + NType, '发布新手指南');
                //else if (NType == "6")
                //    callhtml('Message/NoticeAdd.aspx?id=' + NType, '发布信贷口子');
            }
        }
        function toSeeDetail(id) {
            callhtml('Message/NoticeModify.aspx?id=' + id + '&bank=' + Bank + '&type=' + CType, '修改');
        }

        function agentSeeDetail(id) {
            var title = $(".alert-danger").find("strong").html();
            callhtml('Message/NoticeDetail.aspx?id=' + id, title);
        }
        function toDelete(id) {
            if (!confirm("确定要删除吗？")) return false;
            var userInfo = {
                type: 'deleteNotice',
                pram: id,
            };
            var result = GetAjaxString(userInfo);
            if (result == "1") {
                alert("删除成功");
                doSearch();
            }
            else {
                alert("删除失败，请重试");
            }
        }
        function returnLast() {
            var lasturl = getCookie('lasturl');
            var lasturltitle = getCookie('lasturlname');
            callhtml(lasturl, lasturltitle);
        }
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }

    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="select">
                <input type="hidden" id="nType" name="txtKey" class="commonSearchKey" />
                <input type="hidden" id="nBank" name="txtKey" class="commonSearchKey" />
                <input type="hidden" id="nCType" name="txtKey" class="commonSearchKey" />
            </div>
          
            <div class="search" id="DivSearch" >
                <input id="nTitle" name="txtKey"  placeholder ="请输入标题" type="text" class=" commonSearchKey" />
                <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
                 <%if(TModel.RoleCode=="Manage"||TModel.RoleCode=="Admin"){ %>
                   <input type="button" value="发布" class=" btn btn-info" onclick="setPublish()" />
                  <input type="button" value="返回" class=" btn btn-danger hidden" onclick=" returnLast();" />
                   <%} %>
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                <thead> <tr>
                    <th>序号
                    </th>
                    <th width="50%">标题
                    </th>
                    <th>发布日期
                    </th>
                    <th>浏览次数
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
               <td>{{ d[i].NTitle}}</td>
              <td>{{ d[i].CutTime }}</td>
              <td>{{ d[i].NClicks }}</td>
              <td>
                    <%if(TModel.RoleCode=="Manage"||TModel.RoleCode=="Admin"){ %>
                    <input type='button' class='btn btn-info' value='查看' onclick="toSeeDetail('{{ d[i].ID }}')"/>&nbsp;
                    <input type='button' class='btn btn-danger' value='删除' onclick="toDelete('{{ d[i].ID }}')"/>
                  <%} else{ %>
                  <input type='button' class='btn btn-info' value='查看' onclick="agentSeeDetail('{{ d[i].ID }}')"/>
                  <%} %>
              </td>
          </tr>
        {{# } }}
         </script>
    </div>
</body>
</html>
