function changetabcolor(title) {
    if (title)
        $("#mempay").prepend("<div class=\"alert alert-danger\" style='margin-bottom:0px;'><strong>" + title + "</strong></div>");
    $("#finance").find("table").addClass('table table-bordered table-striped table-white');
    $("#mempay .control").addClass('alert');
    $("#mempay .control").find(".ssubmit").addClass('btn btn-success');
    $("#mempay .control .pay").addClass('btn btn-success');
    $("#mempay .control .cheeckbox table").find("td").css('padding-right', '5px');
    $("#mempay").find(".ui_table").find("table").addClass(' table table-bordered table-hover').css("background-color", "rgb(252, 252, 252)");
    $("#mempay").find(".ui_table").find("th").css('background-color', '#c72a25').css('color', '#FFFFFF');
    $("#finance").find(".normal_btnok").addClass('btn btn-success');
    $(".ui_table_control .pn").find("a").addClass('btn btn-warning');

}
var unique_callhtml_title = '';
function callhtml(url, title) {
    unique_callhtml_title = title;
    if (url.indexOf("?") > -1)
        url += "&r=" + Math.random();
    else
        url += "?r=" + Math.random();
    //if (RunAjaxGetKey('VerifyUrl', url) == 'TRUE') {
    //verifypsd(function () {
    setTimeout(function () { $("#container").load(url, function () { changetabcolor(title); }); }, 10);
    //});
    //}
    //else {
    //    setTimeout(function () { $("#container").load(url, function () { changetabcolor(title); }); }, 10);
    //}
}

function checkForm() {
    var result = true;
    $("[require-type]").each(function () {
        var rtype = $(this).attr("require-type");
        var ignore = $(this).attr("require-ignore");
        var rMsg = $(this).attr("require-msg");
        if (typeof (rMsg) == 'undefined') {
            rMsg = '';
        }
        if (typeof (ignore) != 'undefined') {
            return true;
        }
        var value = $.trim($(this).val());
        if (rtype == "int") {
            if (!(/(^\d+$)/.test(value)) || value < 0) {
                v5.error(rMsg + '：只能输入正整数', '1', 'true');
                result = false;
                $(this).focus();
                return false;
            }
        }
        if (rtype == "decimal") {
            if (!$.isNumeric(value)) {
                v5.error(rMsg + '：只能输入数字', '1', 'true');
                result = false;
                $(this).focus();
                return false;
            }
        }
        if (rtype == "require") {
            if (value == '') {
                v5.error(rMsg + '：不能为空', '1', 'true');
                result = false;
                $(this).focus();
                return false;
            }
        }
    });
    return result;
}


