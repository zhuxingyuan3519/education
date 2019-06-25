<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="main_application.aspx.cs" Inherits="Web.m.app.main_application" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row text-center" style="background-color:#1FC9FF;    margin-right: -15px;
    margin-left: -15px;    padding: 10px;">
      <img id="img_coverImg"  runat="server" style="    width: 60px;    height: 60px;"  class="img-responsive imgshowtitle img-circle" />
        <h4 style="color:white"><%=TModel!=null?TModel.MID:""%></h4>
                <h5 style="color:white"><%=TModel!=null?TModel.Role.Name:""%></h5>
    </div>
    <div class="row">

        <ul class="list-group">

              <li class="list-group-item list-group-li-pater"><a href="ext_main">
                <img src="../images/wdjl.png"  class="indexIcon"/>我的奖励</a></li>

                 <li class="list-group-item list-group-li-pater"><a href="user_addinfo">
                <img src="../images/wdxx.png"  class="indexIcon"/>我的信息</a></li>


            <%if(TModel!=null&&(TModel.RoleCode=="2F"||TModel.RoleCode=="1F"||TModel.RoleCode=="3F")){%>
                  <li class="list-group-item list-group-li-pater"><a href="student_list">
                <img src="../images/wdxy.png"  class="indexIcon"/>我的学员</a></li>

              <li class="list-group-item list-group-li-pater"><a href="select_member">
                <img src="../images/xycx.png"  class="indexIcon"/>学员查询</a></li>

               <li class="list-group-item list-group-li-pater"><a href="reward_center">
                <img src="../images/jlsz.png"  class="indexIcon"/>奖励设置</a></li>

            <%} %>

           
            
                <li class="list-group-item list-group-li-pater"><a href="user_resetpwd">
                <img src="../images/gl7.png"  class="indexIcon"/>密码修改</a></li>

            
             <li class="list-group-item list-group-li-pater"><a   href="user_change">
                <img src="../images/gl2_4.png"  class="indexIcon"/>切换账号</a></li>

            
              <li class="list-group-item list-group-li-pater hidden"><a   href="/loginout">
                <img src="../images/gl2_4.png"  class="indexIcon"/>退出</a></li>




<%--              <li class="list-group-item list-group-li-pater "><a   href="/test">
                <img src="../images/gl2_4.png"  class="indexIcon"/>测试</a></li>--%>

        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function tranToxxfGood() {
            //查看是否是vip会员
            if (isLogin == '1') {
                //登录之后开始轮询消息
                var isvip = '<%=TModel==null?"1": TModel.RoleCode%>';
                if (isvip == 'VIP') {
                    window.location = 'kgj00_goods.aspx';
                }
                else {
                    upMember();
                }
            }
            else {
                //跳转到登录页面
                window.location = 'user_login.aspx';
            }
        }
    </script>
</asp:Content>
