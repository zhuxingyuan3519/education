 <%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="ext_center.aspx.cs" Inherits="Web.m.app.ext_center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .indexIconleft{width:50%;}
        .list-row{   
    padding-top: 20px;
    padding-bottom: 20px;
    border-radius: 10px;
    margin-left: 5px;
    margin-right: 5px;
        }
        .list-des{
    font-size: 20px;color:#fff;}
    </style>
    <script type="text/javascript">
        $(function () {
            if (roleCode != "Teacher") {
                $("#a_teacherLink").remove();
            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height:20px"></div>
    <a href="ext_link" class="btn-block ">
        <div class="row list-row" style=" background-color: #FF8F00;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_tghb.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                推广海报
            </div>
        </div>
    </a>



      <a href="ext_teacherlink" class="btn-block " id="a_teacherLink">
        <div class="row list-row" style=" background-color: #08C1C8;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_jstj.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                教师推广
            </div>
        </div>
    </a>


        <a href="guide_content?code=h5dzhb" class="btn-block ">
        <div class="row list-row" style=" background-color: #77C54E;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_h5dzhb.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                H5电子广告
            </div>
        </div>
    </a>

         <a href="guide_content?code=hb" class="btn-block ">
        <div class="row list-row" style=" background-color: #FF286F;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_hb.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                海报
            </div>
        </div>
    </a>

        <a href="guide_content?code=xcd" class="btn-block ">
        <div class="row list-row" style=" background-color: #3551AE;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_xcy.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                宣传单
            </div>
        </div>
    </a>


         <a href="guide_content?code=sp" class="btn-block ">
        <div class="row list-row" style=" background-color: #009683;">
            <div class="col-sm-4 col-xs-4 text-right">
                <img src="../images/tj_sp.png" class="indexIconleft" />
            </div>
            <div class="col-sm-8 col-xs-8 list-des">
                视频
            </div>
        </div>
    </a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
