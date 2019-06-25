// Ajax通用请求与返回值方法集 2011-02(zh)
Function.prototype.bind = function (obj) {
    var method = this;
    temp = function () {
        return method.apply(obj, arguments);
    }
    return temp;
}
function GetAdminAjaxString(type, pramname) {
    var link = "/Admin/ajax/ajax.aspx?type=" + type + "&pram=" + pramname;     //Ajax调用的页面URL
    return $.ajax({ url: link, async: false }).responseText;
}

function GetAjaxString(dataParam) {
    var link = "/Admin/ajax/ajax";     //Ajax调用的页面URL
    return $.ajax({ url: link, async: false, type: 'POST', data: dataParam }).responseText;
}

function GetAjaxString2(type,dataParam) {
    var link = "ajax/ajax.aspx?type=" + type;     //Ajax调用的页面URL
    return $.ajax({ url: link, async: false, type: 'POST', data: dataParam }).responseText;
}
function GetNewAjaxString(dataParam) {
    var link = "../ajax.aspx";     //Ajax调用的页面URL
    return $.ajax({ url: link, async: false, type: 'POST', data: dataParam }).responseText;
}
