<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master"  AutoEventWireup="true" Inherits="Web.m.app.ext_link" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style type="text/css">
        .hiddentext {
            position: absolute;
            left: -9999px;
            top: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="contact" style="padding: 1.5em 0px;">
        <div class="container">
                <div class="contact-top heading">
                <h2>分享推广</h2>
                <p>分享您的专属推广二维码，就有机会获得意外财富！</p>
            </div>
            <div class="contact-bottom">
                <div class="col-md-6 contact-left">
                    	<h5  id="spLink" runat="server"  style="word-break:break-all"></h5>
                    <img id="imgEcode" style="padding-top: 10px;    padding-bottom: 10px;border:none"   src="/Handler/QRCode.ashx?mid=<%=TModel.ID.ToString() %>" />
				  <textarea id="hiddenText" class="hiddentext"></textarea>
                </div>
                <div class="col-md-6 contact-left">
                    <div class="contact-top heading">
                <p>分享您的专属推广二维码，就有机会获得意外财富！</p>
                        <p>
                           <input type="button"  id="fenxian" value="复制链接" />
                            </p>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
      <script type="text/javascript">
          function toHaiBao() {
              var branch = '<%=MethodHelper.CommonHelper.DESEncrypt(TModel.ID.ToString()) %>';
              var webtitle = '<%=Service.CacheService.GlobleConfig.Contacter %>';
              var isshownav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value %>';
              if (isshownav == "1")
                  window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=/share?registcode=" + branch;
              else
                  window.location = "/share?registcode=" + branch;
          }
          $(function () {
              //$("#nav_footer").hide();
              $("#hiddenText").val($("#spLink").text());
              document.getElementById("fenxian").addEventListener("click", function () {
                  copyToClipboard(document.getElementById("spLink"));
                  layerAlert("复制成功，可直接去粘贴！");
              });
          });
          function copyToClipboard(elem) {
              var target = document.getElementById("hiddenText");
              target.textContent = elem.value;
              console.log(target.textContent);
              select(target);
              document.execCommand("copy");
          }
          function select(element) {
              var selectedText;

              if (element.nodeName === 'INPUT' || element.nodeName === 'TEXTAREA') {
                  element.focus();
                  element.setSelectionRange(0, element.value.length);

                  selectedText = element.value;
              }
              else {
                  if (element.hasAttribute('contenteditable')) {
                      element.focus();
                  }
                  var selection = window.getSelection();
                  var range = document.createRange();

                  range.selectNodeContents(element);
                  selection.removeAllRanges();
                  selection.addRange(range);
                  selectedText = selection.toString();
              }
              return selectedText;
          }
  </script>
</asp:Content>
