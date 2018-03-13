'use strict';
var express = require('express');
var router = express.Router();
var net = require('net');

var HOST = '139.199.160.173';
var PORT = 2758;

router.get('/', function (req, res) {
    var boxmac = req.param("mac");
    var position = req.param("position");
    var list = position.split(',').map(function (val) {
        return parseInt(val) - 1;
    });
    var client = new net.Socket();
    client.connect(PORT, HOST, function () {
        // 建立连接后立即向服务器发送数据，服务器将收到这些数据 
        //var buff = stringToByte(boxmac);
        //var byteArr = appendCommand(boxmac, position);
        client.write("1111111");
    });
    // 为客户端添加“data”事件处理函数
    // data是服务器发回的数据
    client.on('data', function (data) {
        console.log('DATA: ' + data);
        // 完全关闭连接
        client.destroy();
        res.send("开箱成功");
    });
 
});
module.exports = router;

//function appendCommand(boxmac,position) {
//    var command = "EF0332120102082B3B01383631383533303332303231323232000000000000000000000000110000000000000000000B03CF";
//    return command;
//    return stringToByte(command);
//}

//function stringToByte(str) {
//    var bytes = new Array();
//    var len, c;
//    len = str.length;
//    for (var i = 0; i < len; i++) {
//        c = str.charCodeAt(i);
//        if (c >= 0x010000 && c <= 0x10FFFF) {
//            bytes.push(((c >> 18) & 0x07) | 0xF0);
//            bytes.push(((c >> 12) & 0x3F) | 0x80);
//            bytes.push(((c >> 6) & 0x3F) | 0x80);
//            bytes.push((c & 0x3F) | 0x80);
//        } else if (c >= 0x000800 && c <= 0x00FFFF) {
//            bytes.push(((c >> 12) & 0x0F) | 0xE0);
//            bytes.push(((c >> 6) & 0x3F) | 0x80);
//            bytes.push((c & 0x3F) | 0x80);
//        } else if (c >= 0x000080 && c <= 0x0007FF) {
//            bytes.push(((c >> 6) & 0x1F) | 0xC0);
//            bytes.push((c & 0x3F) | 0x80);
//        } else {
//            bytes.push(c & 0xFF);
//        }
//    }
//    return bytes;


//}


//function byteToString(arr) {
//    if (typeof arr === 'string') {
//        return arr;
//    }
//    var str = '',
//        _arr = arr;
//    for (var i = 0; i < _arr.length; i++) {
//        var one = _arr[i].toString(2),
//            v = one.match(/^1+?(?=0)/);
//        if (v && one.length == 8) {
//            var bytesLength = v[0].length;
//            var store = _arr[i].toString(2).slice(7 - bytesLength);
//            for (var st = 1; st < bytesLength; st++) {
//                store += _arr[st + i].toString(2).slice(2);
//            }
//            str += String.fromCharCode(parseInt(store, 2));
//            i += bytesLength - 1;
//        } else {
//            str += String.fromCharCode(_arr[i]);
//        }
//    }
//    return str;
//}  