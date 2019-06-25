/*高德地图相关接口，依赖于变色龙app提供的定位服务*/
$(function () {
    //定位
    if (is_kingkr_obj()) { //如果是在APP里打开的
        getLocation("gpscallback")
    }

    ////作为测试
    //var result = '{ "Longitude": "111.36335", "Latitude": "35.218231" }';
    //gpscallback(result);
});
//定位之后解析
function gpscallback(result) {
    //GPS坐标
    var jsonObj = JSON.parse(result);
    var x = jsonObj.Longitude;// lonArray[3];// locatl.Longitude;//经度
    var y = jsonObj.Latitude;// lonArray[7];//locatl.Latitude;//纬度
    var locationStr = x + ',' + y;
    $.get("http://restapi.amap.com/v3/geocode/regeo?key=220d34b6309a0d313caaa1d2b41af109&output=json&location=" + locationStr, function (data) {
        //解析json
        var province = data.regeocode.addressComponent.province; //省
        var city = data.regeocode.addressComponent.city; //市
        var cityCode = data.regeocode.addressComponent.citycode; //市编号
        var zone = data.regeocode.addressComponent.district; //区
        var adcode = data.regeocode.addressComponent.adcode; //区编号
        var township = data.regeocode.addressComponent.township; //街道

        var appendHtml = "<input type='hidden' name='hid_location_province' value='" + province + "'>";
        appendHtml += "<input type='hidden' name='hid_location_city' value='" + (city == '' ? province : city) + "'>";
        appendHtml += "<input type='hidden' name='hid_location_zone' value='" + zone + "'>";
        appendHtml += "<input type='hidden' name='hid_location_town' value='" + township + "'>";
        appendHtml += "<input type='hidden' name='hid_location_citycode' value='" + cityCode + "'>";
        appendHtml += "<input type='hidden' name='hid_location_adcode' value='" + adcode + "'>";
        appendHtml += "<input type='hidden' name='hid_location_pointer' value='" + locationStr + "'>";
        $("#form1").append(appendHtml);
        //alert(appendHtml);
        //alert(JSON.stringify(data));
    });
}
