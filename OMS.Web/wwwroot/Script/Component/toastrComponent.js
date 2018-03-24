var UIToastr = function () {
    return {
        //main function to initiate the module
        init: function () {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "positionClass": "toast-top-right",
                "onclick": null,
                "showDuration": "1000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
        }
    };
}();

jQuery(document).ready(function () {
    UIToastr.init();
});

var alertInfo = function (msg) {
    toastr["info"](msg, "通知");
}
var alertSuccess = function (msg) {
   toastr["success"](msg, "成功");
}
var alertWarn = function (msg) {
    toastr["warning"](msg, "警告");
}
var alertError = function (msg) {
    toastr["error"](msg, "错误");
}