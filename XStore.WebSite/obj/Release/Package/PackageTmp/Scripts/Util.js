var Util = {
    Deserialize: function (utldata) {//反序列化
        var paramsString = utldata.replace("?", "");
        var paramsTempObj = paramsString.split("&");
        var paramsObj = {};

        for (var i = 0; i < paramsTempObj.length; i++) {
            paramsObj[paramsTempObj[i].split("=")[0]] = paramsTempObj[i].split("=")[1]
        }
        return paramsObj;
    }
}

$.ajaxSetup({
	success: function(msg){
		if(!(msg.Status == 200)){
			mui.alert(msg.Msg, '错误', '确定')
		}
	}
});
