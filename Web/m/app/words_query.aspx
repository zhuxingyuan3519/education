<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="words_query.aspx.cs" Inherits="Web.m.app.words_query" %>

<%--单词查询--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .form-control {
            padding: 0px;
        }

        .padding-left-right-0 {
          font-size:12px;
          color:#040404;
        }
        .sp_checked{color:red;}
     .clearfix{height:10px;}
    </style>
    <script type="text/javascript">
        $(function () {
        });
        function query() {
            layerLoading();
            var userInfo = {
                type: 'queryWord',
                word: $("#nWords").val()
            };
            var result = GetAjaxString(userInfo);
            closeLayerLoading();
            if (result != "0") {
                $("#div_result").html("");
                if (result == "-1") {
                    layerAlert("对不起，您无权限查询");
                    return false;
                }
                var data = JSON.parse(result);
                if (data.length > 0) {
                    $.each(data, function (n, value) {
                        //console.log(value.English);
                        var appendHtml = "<div class=\"row bg-wh\">";
                        appendHtml += "<div class=\"col-sm-4 col-xs-4 padding-left-right-0\">" + value.English + "</div>";
                        appendHtml += "<div class=\"col-sm-3 col-xs-3  padding-left-right-0\">" + value.Phonetic + "</div>";
                        appendHtml += "<div class=\"col-sm-5 col-xs-5  padding-left-right-0\">" + value.Chinese + "</div>";
                        if (value.HotWord != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12\">" + value.HotWord + "</div>";
                        }
                        if (value.Module1 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">模块拆分：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Module1 + "</div>";
                        }
                        if (value.Association1 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">情景联想：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Association1 + "</div>";
                        }

                        if (value.Module2 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">模块拆分：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Module2 + "</div>";
                        }
                        if (value.Association2 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">情景联想：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Association2 + "</div>";
                        }

                        if (value.Module3 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">模块拆分：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Module3 + "</div>";
                        }
                        if (value.Association3 != "") {
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\" style=\"font-weight:bolder\">情景联想：</div>";
                            appendHtml += "<div class=\"col-sm-12 col-xs-12  padding-left-right-0\">" + value.Association3 + "</div>";
                        }

                        appendHtml += "</div>";
                        appendHtml += "<div class=\"clearfix\"></div>";
                        $("#div_result").append(appendHtml);
                    });
                }
                else {
                    layerAlert("系统中不存在该单词");
                }
            }
            else {
                layerAlert("查询失败，请重试");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="padding: 20px">
        <div class="col-sm-9 col-xs-9  padding-left-right-0">
            <input id="nWords" placeholder="请输入中文/英文" type="text" class="form-control"  style="text-align:center"/>
        </div>
        <div class="col-sm-3 col-xs-3  padding-left-right-0" style="padding-top: 1px;padding-left: 5px !important;">
            <input type="button" class="btn btn-block btn-sm btn-info" style="padding: 6px 10px;" onclick="query()" value="查询" />
        </div>
    </div>
    <div id="div_result">
 
</div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
   
    </script>

</asp:Content>
