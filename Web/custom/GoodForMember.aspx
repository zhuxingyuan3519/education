<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodForMember.aspx.cs" Inherits="Web.custom.GoodForMember" %>
<%--会员特供--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="/js/swiper3.4.1/swiper.min.css" rel="stylesheet" />
    <style type="text/css">
       /*.swiper-container {
        width: 100%;
        height: 300px;
        margin: 20px auto;
    }
    .swiper-slide {
        text-align: center;
        font-size: 18px;
        background: #fff;
        display: -webkit-box;
        display: -ms-flexbox;
        display: -webkit-flex;
        display: flex;
        -webkit-box-pack: center;
        -ms-flex-pack: center;
        -webkit-justify-content: center;
        justify-content: center;
        -webkit-box-align: center;
        -ms-flex-align: center;
        -webkit-align-items: center;
        align-items: center;
    }*/

        .swiper-container {
        width: 100%;
        height: 100%;
    }
    .swiper-slide {
        text-align: center;
        font-size: 18px;
        background: #fff;

        /* Center slide text vertically */
        display: -webkit-box;
        display: -ms-flexbox;
        display: -webkit-flex;
        display: flex;
        -webkit-box-pack: center;
        -ms-flex-pack: center;
        -webkit-justify-content: center;
        justify-content: center;
        -webkit-box-align: center;
        -ms-flex-align: center;
        -webkit-align-items: center;
        align-items: center;
    }


    .append-buttons {
        text-align: center;
        margin-top: 20px;
    }
    .append-buttons a {
        display: inline-block;
        border: 1px solid #007aff;
        color: #007aff;
        text-decoration: none;
        padding: 4px 10px;
        border-radius: 4px;
        margin: 0 10px;
        font-size: 13px;
    }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="banner-in">
        <div class="container">
            <div class="banner-top">
                <h6><a href="index.aspx">首页</a> / <span>会员特供</span> </h6>
            </div>
        </div>
    </div>
    <div class="content">
		<div class="container">
			 <!-- Swiper -->
    <div class="swiper-container">
        <div class="swiper-wrapper">
            <asp:Repeater ID="repList" runat="server">
                <ItemTemplate>
                       <div class="swiper-slide"><img  class="img-responsive showImg" src="/Attachment/<%#Eval("PicUrl") %>" alt=""/></div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <!-- Add Pagination -->
        <div class="swiper-pagination"></div>
        <!-- Add Arrows -->
        <div class="swiper-button-next"></div>
        <div class="swiper-button-prev"></div>
    </div>
    <p class="append-buttons">
        <a href="#" class="prepend-2-slides">Prepend 2 Slides</a>
        <a href="#" class="prepend-slide">Prepend Slide</a>
        <a href="#" class="append-slide">Append Slide</a>
        <a href="#" class="append-2-slides">Append 2 Slides</a>
    </p>
			</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="/js/swiper3.4.1/swiper.min.js"></script>
    <script type="text/javascript">
        var appendNumber = 4;
        var prependNumber = 1;
        var swiper = new Swiper('.swiper-container', {
            pagination: '.swiper-pagination',
            paginationClickable: true,
            pagination: '.swiper-pagination',
            nextButton: '.swiper-button-next',
            prevButton: '.swiper-button-prev'
            //slidesPerView: 3,
            //centeredSlides: true,
            //paginationClickable: true,
            //spaceBetween: 30,
        });
        //document.querySelector('.prepend-2-slides').addEventListener('click', function (e) {
        //    e.preventDefault();
        //    swiper.prependSlide([
        //        '<div class="swiper-slide">Slide ' + (--prependNumber) + '</div>',
        //        '<div class="swiper-slide">Slide ' + (--prependNumber) + '</div>'
        //    ]);
        //});
        //document.querySelector('.prepend-slide').addEventListener('click', function (e) {
        //    e.preventDefault();
        //    swiper.prependSlide('<div class="swiper-slide">Slide ' + (--prependNumber) + '</div>');
        //});
        //document.querySelector('.append-slide').addEventListener('click', function (e) {
        //    e.preventDefault();
        //    swiper.appendSlide('<div class="swiper-slide">Slide ' + (++appendNumber) + '</div>');
        //});
        document.querySelector('.append-2-slides').addEventListener('click', function (e) {
            e.preventDefault();
            swiper.appendSlide([
                '<div class="swiper-slide">Slide ' + (++appendNumber) + '</div>',
                '<div class="swiper-slide">Slide ' + (++appendNumber) + '</div>'
            ]);
        });    </script>
</asp:Content>
