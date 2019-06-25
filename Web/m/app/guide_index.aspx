<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="guide_index.aspx.cs" Inherits="Web.m.app.guide_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            setNavIndexChecked("4");
        });
    </script>
    <style type="text/css">
        .liabout{padding-left:20px;}
        @media (min-width: 768px) and (min-width: 992px) and (min-width: 1200px) {
            .list-group-container li {
                width: 60% !important;
            }
            .list-group-container li a{background-size: 5%;}
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="row personer text-center  hidden-lg hidden-md hidden-sm" style="background-image: url(../images/pensoner_mine_bg.jpg); background-size: contain;">
        <img src="../images/gl3.png" class="img-responsive imgshowtitle" id="img_to_click" />
        <h4 >关于我们</h4>
    </div>

      <div class="row bg-wh  hidden-xs">
        <h5><b>|</b>关于我们  </h5>
    </div>

    <div class="row">
        <ul class="list-group list-group-container">
                <li class="list-group-item  list-group-li-pater "><a href="guide_content.aspx?code=commonquestion" class="liabout">
                    常见问题</a></li>
           
            <li class="list-group-item"><a href="guide_content.aspx?code=aboutus"  class="liabout">
                关于我们</a></li>
            <li class="list-group-item"><a href="guide_content.aspx?code=serviceprotrol"  class="liabout">
                服务协议</a></li>

                    <li class="list-group-item"><a href="guide_contact.aspx"  class="liabout">
                联系我们</a></li>


            <li class="list-group-item  list-group-li-pater" >
                <a href="guide_content.aspx?code=version"  class="liabout">
                    版本信息</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
