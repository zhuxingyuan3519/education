<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="student_add.aspx.cs" Inherits="Web.m.app.student_add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .form-control[disabled] {
            background-color: #5cb85c !important;
        }
           @media (min-width: 768px) and (min-width: 992px)  and (min-width: 1200px)  {
  .marg > img{    width: 5% !important;}
  .gree{margin-top:0px}
  .list-card{width:60%;     padding-top:20px}
}
    </style>
    <script type="text/javascript">
        $(function () {
            var code = '<%=Request.QueryString["code"] %>';
            var teachercode = '<%=Request.QueryString["teachercode"] %>';
            if (teachercode != "") {
                window.localStorage.setItem("teachercode", teachercode);
                $("#hid_teacherMID").val(teachercode);
            }
            else {
                teachercode = window.localStorage.getItem("teachercode");
                if (teachercode != null && teachercode != '') {
                    var rek = 'user_regist';
                    //获取最后一个/和?之间的内容，就是请求的页面
                    $.ajax({
                        type: 'post',
                        url: rek + '?Action=GET',
                        data: 'mtjcode=' + teachercode,
                        success: function (info) {
                            $("#txt_TeacherMID").val(info);
                        }
                    });
                }
            }
              //alert(code);
              if (code != '') {
                  window.localStorage.setItem("mtjcode", code);
                  $("#txt_MTJ").val(code);
                  window.location = '/m/app/main_mine';
              }
              else {
                  code = window.localStorage.getItem("mtjcode");
                  if (code != null && code != '') {
                      var rek = 'user_regist';
                      //获取最后一个/和?之间的内容，就是请求的页面
                      $.ajax({
                          type: 'post',
                          url: rek + '?Action=GET',
                          data: 'mtjcode=' + code,
                          success: function (info) {
                              $("#txt_MTJMID").val(info);
                          }
                      });
                  }
              }
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row bg-wh">
        <h5><b>|</b>开通学员
        </h5>
    </div>

     <div class="row list-card">
                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input type="text" class="form-control" placeholder="学员手机号" id="txt_MID" runat="server" require-type="phone" require-msg="学员手机号" />
                    </span>
                </div>

                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input id="txt_Name" type="text" runat="server"  class="form-control" placeholder="学员姓名" />
                    </span>
                </div>


                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input id="txt_Password" type="password" runat="server" require-type="require" require-msg="设置密码" class="form-control" placeholder="请设置密码" />

                    </span>
                </div>

                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input id="txt_Password2" type="password" runat="server" require-type="require" require-msg="确认密码" class="form-control" placeholder="请确认密码" />
                    </span>
                </div>

                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input type="text" class="form-control" id="txt_MTJMID" runat="server" placeholder="推荐人推荐码" require-type="require" require-msg="推荐人推荐码" />
                        <input id="txt_MTJ" type="hidden" runat="server" />
                    </span>
                </div>

            <div class="marg hidden">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input type="text" class="form-control" id="txt_TeacherMID" runat="server" placeholder="老师推荐码" />
                        <input id="hid_teacherMID" type="hidden" runat="server" />
                    </span>
                </div>

                  <div class="marg hidden">
                    <span class="col-sm-12 col-xs-12 marg">
                        <input type="text" class="form-control" id="txt_Company" runat="server" placeholder="机构代码" />
                        <input id="hid_companyId" type="hidden" runat="server" />
                    </span>
                </div>


                <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg">
                        <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">注册</a>
                    </span>
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script type="text/javascript">
             function setupChange() {
                 if (!checkForm())
                     return false;
                 if ($.trim($("#txt_MTJMID").val()) == '') {
                     layerAlert("推荐人代码不能为空，如不知道代码请联系您的推荐人！");
                     return false;
                 }


                 layerLoading();
                 var rek = 'student_add';
                 //获取最后一个/和?之间的内容，就是请求的页面
                 $.ajax({
                     type: 'post',
                     url: rek + '?Action=ADD',
                     data: $('#form1').serializeArray(),
                     success: function (ret) {
                         closeLayerLoading();
                         var info = ret.split('*')[0];
                         if (info == "0")
                             layerAlert("注册失败");
                         else if (info == "1") {  //提交成功
                             layerMsg("注册成功！！");
                             setTimeout(function () {
                                 //自动跳转到“登录页面
                                 window.location = "apply_up?mid=" + ret.split('*')[1];
                             }, 2000);
                         }
                         else if (info == "-2")
                             layerAlert("不存在该老师！");
                         else if (info == "-3")
                             layerAlert("不存在该推荐人！");
                         //else if (info == "4")
                         //    layerAlert("不存在该老师！");
                         else if (info == "-1")
                             layerAlert("该账号已被注册过！");
                         else
                             layerAlert("注册失败，请重试");
                     }
                 });
             }

         
    </script>
</asp:Content>
