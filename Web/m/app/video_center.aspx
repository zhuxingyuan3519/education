<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="video_center.aspx.cs" Inherits="Web.m.app.video_center" %>
<%--视频列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .form-control {
            padding: 0px;
        }
        .list-border{border:none;}
        .padding-left-right-0 {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .div_in_list span {
            /*padding-top: 5px;*/
        }

        .img_coverimg {
            height: 180px !important;
            width: 100% !important;
        }
        .img2{  height: 90px !important;}
        .video_title{position: absolute;
    color: #fff;
    font-weight: bolder;
    padding: 5px 0px 0px 10px;}
        .video_title2{font-size:10px;}
        .more-see{width: 6%;
    float: right;
    margin-right: 10px;
    margin-top: 2px;}
        .v-remark{padding-top:5px  !important;color:#131111}
          .see-count{padding-left: 0px;    color: #9e9c9c;    font-size: 10px;text-align:center;padding-top:6px  !important;}

           .div-playing{position: absolute;
    padding-top: 55px;
    width: 100%;
    text-align: center;}
            .div-playing img{width:72px;height:72px;}

             .div-playing2{position: absolute;
    padding-top: 25px;
    width: 100%;
    text-align: center;}
            .div-playing2 img{width:40px;height:40px;}


    </style>
         <script type="text/javascript">
          
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b>免费专区
           <a href="video_list?type=0"> <img src="../images/more.png" class="more-see" /></a>
        </h5>
    </div>
    <div   class="row " >
        <asp:Repeater ID="rep_listMember" runat="server">
            <ItemTemplate>
                  <div class="col-sm-12 col-xs-12 div_in_list list-border">
				    <a href="video_star?code=<%#Eval("Code") %>">   <span class="col-sm-12 col-xs-12 padding-left-right-0">
                        <div class="video_title"><%#Eval("Title") %></div>
                         <div class="div-playing">  <img src="../images/nowplaying.png" /></div>
                        <img src="/Attachment/Video/<%#Eval("CoverImage") %>"  class="img_coverimg"/></span>
				        <span class="col-sm-8 col-xs-8 text-left v-remark"><%#Eval("Remark") %> </span>
                          <span class="col-sm-4 col-xs-4 text-left see-count">已有<%#Eval("Sort") %>人观看 </span>
                        </a>
			</div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clearfix"></div>


    
     <div class="row bg-wh">
        <h5 style="    border-top: 1px solid #eee;"><b>|</b>学员专区
           <a href="video_list?type=1"><img src="../images/more.png" class="more-see" /></a>  
        </h5>
    </div>
    <div   class="row " >
        <asp:Repeater ID="rep_StudentList" runat="server">
            <ItemTemplate>
                  <div class="col-sm-6 col-xs-6 div_in_list list-border">
				    <a href="video_star?code=<%#Eval("Code") %>"> 
                          <span class="col-sm-12 col-xs-12 padding-left-right-0">
                         <div class="video_title video_title2"><%#Eval("Title") %></div>
                               <div class="div-playing2">  <img src="../images/nowplaying.png" /></div>
                              <img src="/Attachment/Video/<%#Eval("CoverImage") %>"  class="img_coverimg img2"/></span>
				          <span class="col-sm-8 col-xs-8 text-left v-remark"><%#Eval("Remark") %> </span>
                          <span class="col-sm-4 col-xs-4 text-left see-count"><%#Eval("Sort") %>人观看 </span>
                        </a>
			</div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clearfix"></div>

   

       <div class="row bg-wh">
        <h5 style="    border-top: 1px solid #eee;"><b>|</b>VIP专区
           <a href="video_list?type=2"><img src="../images/more.png" class="more-see" /></a>  
        </h5>
    </div>
    <div   class="row " >
        <asp:Repeater ID="rep_listVIP" runat="server">
            <ItemTemplate>
                  <div class="col-sm-6 col-xs-6 div_in_list list-border">
				    <a href="video_star?code=<%#Eval("Code") %>"> 
                          <span class="col-sm-12 col-xs-12 padding-left-right-0">
                         <div class="video_title video_title2"><%#Eval("Title") %></div>
                               <div class="div-playing2">  <img src="../images/nowplaying.png" /></div>
                              <img src="/Attachment/Video/<%#Eval("CoverImage") %>"  class="img_coverimg img2"/></span>
				          <span class="col-sm-8 col-xs-8 text-left v-remark"><%#Eval("Remark") %> </span>
                          <span class="col-sm-4 col-xs-4 text-left see-count"><%#Eval("Sort") %>人观看 </span>
                        </a>
			</div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="clearfix"></div>


   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
