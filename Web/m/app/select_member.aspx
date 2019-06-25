<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="select_member.aspx.cs" Inherits="Web.m.app.select_member" %>
<%--查询会员信息--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
          .form-control{padding:0px}
          .padding-left-right-0 {
    padding-left: 2px !important;
    padding-right: 2px !important;
}
          .div_in_list span{padding-top:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b>会员查询
        </h5>
    </div>

    <div class="row  bg-wh">
        <div class="col-sm-10 col-xs-10 padding-left-right-0">
                <input  id="txt_mid" placeholder="请输入您要查询的账号或姓名"  type="text" class="form-control text-center" 
                    style="background-image: url(../images/search.png); background-repeat: no-repeat; background-size: contain;    padding-left: 22px;" />
        </div>
    <div class="col-sm-2 col-xs-2  padding-left-right-0"  style="padding-top: 1px;" >
        <input type="button" class="btn btn-block btn-sm btn-info" style="padding:6px 10px;"  onclick="selectMember()" value="查询" />
    </div>
</div>

    <div id="appendView"  class="row " style="padding-top:10px">
       
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
   <script type="text/javascript">
       function selectMember() {
           layerLoading();
           var userInfo = {
               type: 'selectMember',
               nickname: $("#txt_mid").val()
           };
           var result = GetAjaxString(userInfo);
           var data = eval(result);
           var html = "<div class=\"col-sm-12 col-xs-12 div_in_list list-border\">";
           if (data.length == 0) {
               html += "<span class=\"col-sm-12 col-xs-12 text-center\" style='color:red'> 未查到会员信息！</span>";
           }
           else {
               for (var index = 0; index < data.length; index++) {
                   var val = data[index];
                   html += "<span class=\"col-sm-12 col-xs-12 text-left\">  姓名：" + val.MName + "&nbsp;&nbsp; &nbsp;&nbsp; 电话：" + val.MID + "</span>";
                   html += "<span class=\"col-sm-12 col-xs-12 text-left \">级别：" + val.Role + " &nbsp;&nbsp;</span>";
               }
           }
           html += "</div>";
           $("#appendView").html(html);
           closeLayerLoading();
       }
   </script>
</asp:Content>
