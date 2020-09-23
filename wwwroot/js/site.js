// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}

var time_loader;
var reqDateTime = new Date();
function showLoader(msg, maxSec) {

    reqDateTime = new Date();
    $("#loaderConsole").empty();
    $('#loaderInfo').text(msg);
    $("#mask").fadeIn(300);
    $("#loader").fadeIn(300);

    var time = setInterval(function () {
        var ts_date = new Date().getTime() - reqDateTime.getTime();
        var leaveSec = ts_date / 1000;
        $('#loaderTime').text('耗时:' + Math.round(leaveSec) + "s");
    }, 1000);

    if (maxSec != undefined) {
        setTimeout(function () {
            closeLoader();
            showGrowl('操作超时!', 'danger', 5000);
        }, maxSec * 1000);
    }
}

function addLoaderInfo(msg, type) {

    // Success = 200,
    // Failed = 500,
    // Warning = 400,
    // Info = 100,
    var h5;
    switch (type) {
        case 100:
            //Info
            h5 = $('<h5 style="color: #FFF;">' + msg + "</h5>");
            break;
        case 200:
            //Success
            h5 = $('<h5 style="color: #69d77c;">' + msg + "</h5>");
            break;
        case 400:
            //Warning
            h5 = $('<h5 style="color: #fadd4a;">' + msg + "</h5>");
            break;
        case 500:
            //Warning
            h5 = $('<h5 style="color: #ed6a5e;">' + msg + "</h5>");
            break;
    }
    $('#loaderConsole').append(h5);
}

function closeLoader() {
    reqDateTime = new Date();
    $("#loader").delay(0).fadeOut(200);
    $("#mask").delay(100).fadeOut(200);
    clearInterval(time_loader);
}

function showGrowl(msg, type, delay, align) {
    $.bootstrapGrowl(msg, {
        ele: 'body', // which element to append to
        type: type === undefined ? 'info' : type, // (null, 'info', 'danger', 'success')
        offset: { from: 'top', amount: 20 }, // 'top', or 'bottom'
        align: align === undefined ? 'right' : align, // ('left', 'right', or 'center')
        width: 300, // (integer, or 'auto')
        delay: delay === undefined ? 10000 : delay, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
        allow_dismiss: true, // If true then will display a cross to close the popup.
        stackup_spacing: 10 // spacing between consecutively stacked growls.
    });
}

function post(url, data, funcSucc, funcFail) {
    $.post(url, data === undefined || data === null ? {} : data, function (res, status, xhr) { closeLoader(); funcSucc(res, status, xhr); }).error(function () { closeLoader(); funcFail(); });
}

function clearAllCookie() {



    var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
    if (keys) {
        for (var i = keys.length; i--;)
            document.cookie = keys[i] + '=0;path=/;expires=' + new Date(0).toUTCString();// 清除当前域名路径的有限日期
        document.cookie = keys[i] + '=0;path=/;domain=' + document.domain + ';expires=' + new Date(0).toUTCString();// Domain Name域名 清除当前域名的
        document.cookie = keys[i] + '=0;path=/;domain=baidu.com;expires=' + new Date(0).toUTCString();// 清除一级域名下的或指定的
    }

    // 清空所有的localStorage
    localStorage.clear();
}

function closeWindows() {

    var browserName = navigator.appName;
    var browserVer = parseInt(navigator.appVersion);
    //alert(browserName + " : "+browserVer);

    //document.getElementById("flashContent").innerHTML = "<br>&nbsp;<font face='Arial' color='blue' size='2'><b> You have been logged out of the Game. Please Close Your Browser Window.</b></font>";

    if (browserName == "Microsoft Internet Explorer") {
        var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
        if (ie7) {
            //This method is required to close a window without any prompt for IE7 & greater versions.
            window.open('', '_parent', '');
            window.close();
        }
        else {
            //This method is required to close a window without any prompt for IE6
            this.focus();
            self.opener = this;
            self.close();
        }
    } else {
        //For NON-IE Browsers except Firefox which doesnt support Auto Close
        try {
            this.focus();
            self.opener = this;
            self.close();
        }
        catch (e) {

        }

        try {
            debugger;
            window.open('', '_self', '');
            window.close();
        }
        catch (e) {

        }
    }
}