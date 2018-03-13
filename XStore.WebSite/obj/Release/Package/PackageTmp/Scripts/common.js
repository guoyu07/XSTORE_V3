function system_alert(message) {
    layer.open({
        title: ['系统提示', 'background-color:#F60; color:#fff;'],
        content: message,
        btn: '确定'
    });
}
function system_confirm(message) {
   
    layer.open({
        title: ['系统提示', 'background-color:#F60; color:#fff;'],
        content: message,
        btn: ['确认', '取消'],
        yes: function (index) {
            layer.close(index);
            return true;

        }
      
    });
    return false;

}