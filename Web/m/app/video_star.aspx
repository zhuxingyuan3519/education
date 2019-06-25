<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="video_star.aspx.cs" Inherits="Web.m.app.video_star" %>

<%--视频播放--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/common/vide7.1.0/css/video-js.min.css" rel="stylesheet" />
     <script src="/common/js/base64.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b><%=VideoName %>
        </h5>
    </div>

    <div class="row">
        <div class="col-md-12 col-sm-12 padding-left-right-0">
            <video id="my-video" class="video-js vjs-default-skin vjs-big-play-centered" controls="controls" preload="auto" poster="<%=VideoCoverImg %>" data-setup="">
                <source />
            </video>
        </div>
    </div>
     <div class="row">
         <div class="col-md-12 col-sm-12" style="    padding: 5px;    color: black;">
             <%=VideoRemark %>
         </div>
            <div class="col-md-12 col-sm-12" style="    padding-left: 0px;    color: #9e9c9c;    font-size: 10px;">
                已有<%=VideoCount %>人观看
            </div>
     </div>

    <div id="appendView" class="row " style="padding-top: 10px">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script src="/common/vide7.1.0/js/video.min.js"></script>
    <script src="/common/vide7.1.0/js/zh-CN.js"></script>

    <script type="text/javascript">
        $(function () {
            //$("#my-video").css("width", "100%").css("height", "300");
            var videoUrl = '<%=VideoPath%>';
            if (videoUrl == '') {
                layerAlert("对不起，您当前级别无权观看此视频，详情请咨询客服！");
            }
            //videoUrl = base64decode(videoUrl);
            var myPlayer = videojs('my-video', {
                autoplay: false,
                height: '300px',
                width: $(document.body).width(),
                sources: [{
                    src: videoUrl,
                    type: '<%=VideoType%>'
                }]
            });
            //videojs("my-video").ready(function () {
            //    var myPlayer = this;
            //$("#my-video source").attr("src", videoUrl);
            //myPlayer.src(videoUrl);
            ////myPlayer.autoplay = false;
            //myPlayer.load(videoUrl);
            //myPlayer.play();
            //});
        });


    </script>

</asp:Content>
