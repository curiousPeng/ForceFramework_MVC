$(function () {
    //post 调用
    //pageInitModule.post(data, 'JSON', strUrl, function (obj) {

    //});  

    //get 调用
    //pageInitModule.get({ "test": "1234" }, "/GlobalSettings/GetTo", function (obj) {
    //    alert(obj);
    //});

    //调用提示框
    //pageInitModule.Toastr("标题","内容","success");

});

/**
 * 公用函数
 * @param {any} mod
 */
var pageInitModule = (function (mod) {
    /**
     * post请求
     * @param {string} data 参数值组合
     * @param {string} dataType 类型
     * @param {string} url 地址
     * @param {string} successfn 返回函数
     */
    mod.post = function (data, dataType, url, successfn) {

        $.ajax({
            type: "POST",
            url: url,
            dataType: dataType,
            data: data,
            success: function (obj) {
                if (obj.status == 110) {
                    location.href = "/Home/ErrorMessage?message=" + mod.encodeUnicode(obj.msg);
                    return;
                }
                if (obj.status == 2) {
                    location.href = "/Login/Index";
                    return;
                }

                return successfn(obj);
            },
            error: function (er) {
                pageInitModule.unblockUI();
            }
        });
    };

    /**
     * get请求
     * @param {string} data 参数值组合
     * @param {string} url 地址
     * @param {string} seccessfn 返回函数
     */
    mod.get = function (data, url, seccessfn) {
        $.get(url, data, function (obj) {

            if (obj.status == 3) {
                location.href = "/Home/ErrorMessage?message=" + mod.encodeUnicode(obj.msg);
                return;
            }
            if (obj.status == 2) {
                location.href = "/Login/Index";
                return;
            }

            seccessfn(obj);
        });
    };

    /**
     * get请求 - 只有值时
     * @param {string} data 参数值组合
     * @param {string} url 地址
     * @param {string} seccessfn 返回函数
     */
    mod.get2 = function (data, url, seccessfn) {
        $.get(url, data, function (obj) {
            seccessfn(obj);
        });
    };

    mod.GetQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    //type："success"，"info","warning","error"
    mod.Toastr = function (Title, Msg, Type) {
        var toastCount = 0;
        var toastIndex = toastCount++;
        var shortCutFunction = Type;
        var msg = Msg;
        var title = Title || "";
        toastr.options = {
            closeButton: $('#closeButton').prop('checked'),
            debug: $('#debugInfo').prop('checked'),
            positionClass: 'toast-top-center',
            onclick: null
        };

        if ($('#addBehaviorOnToastClick').prop('checked')) {
            toastr.options.onclick = function () {
                alert('You can perform some custom action after a toast goes away');
            };
        }

        //显示持续时间
        toastr.options.showDuration = 1000;
        //隐藏持续时间
        toastr.options.hideDuration = 1000;
        //时间到
        toastr.options.timeOut = 2000;
        //延长时间
        toastr.options.extendedTimeOut = 1000;
        //显示缓和
        toastr.options.showEasing = 'swing';
        //隐藏缓和
        toastr.options.hideEasing = 'linear';
        //显示方法
        toastr.options.showMethod = 'fadeIn';
        //隐藏方法
        toastr.options.hideMethod = 'fadeOut';

        $("#toastrOptions").text("Command: toastr[" + shortCutFunction + "](\"" + msg + (title ? "\", \"" + title : '') + "\")\n\ntoastr.options = " + JSON.stringify(toastr.options, null, 2));

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        $('#clearlasttoast').click(function () {
            toastr.clear($toastlast);
        });
    }


    mod.ToastrCustom = function (title, msg, type, showLocation) {
        var toastCount = 0;
        var toastIndex = toastCount++;
        var shortCutFunction = type;
        var msg = msg;
        var title = title || "";

        toastr.options = {
            closeButton: $('#closeButton').prop('checked'),
            debug: $('#debugInfo').prop('checked'),
            positionClass: showLocation || 'toast-top-right',
            onclick: null
        };

        if ($('#addBehaviorOnToastClick').prop('checked')) {
            toastr.options.onclick = function () {
                alert('You can perform some custom action after a toast goes away');
            };
        }

        //显示持续时间
        toastr.options.showDuration = 1000;
        //隐藏持续时间
        toastr.options.hideDuration = 1000;
        //时间到
        toastr.options.timeOut = 2000;
        //延长时间
        toastr.options.extendedTimeOut = 1000;
        //显示缓和
        toastr.options.showEasing = 'swing';
        //隐藏缓和
        toastr.options.hideEasing = 'linear';
        //显示方法
        toastr.options.showMethod = 'fadeIn';
        //隐藏方法
        toastr.options.hideMethod = 'fadeOut';

        $("#toastrOptions").text("Command: toastr[" + shortCutFunction + "](\"" + msg + (title ? "\", \"" + title : '') + "\")\n\ntoastr.options = " + JSON.stringify(toastr.options, null, 2));

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        $('#clearlasttoast').click(function () {
            toastr.clear($toastlast);
        });
    }

    mod.getJsParam = function () {
        var js = document.getElementsByTagName("script");
        //得到当前引用a.js一行的script，并把src用'?'分隔成数组
        var arraytemp = js[js.length - 1].src.split('?');
        var obj = new Object();
        //如果不带参数，则不执行下面的代码
        if (arraytemp.length > 1) {
            var params = arraytemp[1].split('&');
            for (var i = 0; i < params.length; i++) {
                var parm = params[i].split("=");
                //将key和value定义给obj
                obj[parm[0]] = parm[1];
                // alert(parm[0] + "=" + parm[1]);
            }
        }
        return obj;
    }

    mod.isPhone = function (phone) {
        //手机号正则
        var phoneReg = /(^1[3|4|5|7|8]\d{9}$)|(^09\d{8}$)/;
        //电话
        var phoneTemp = $.trim(phone);
        if (!phoneReg.test(phoneTemp)) {
            return false;
        }
        return true;
    }

    // 转为unicode 编码
    mod.encodeUnicode = function (str) {
        var res = [];
        for (var i = 0; i < str.length; i++) {
            res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4);
        }
        return "\\u" + res.join("\\u");
    }

    // 解码
    mod.decodeUnicode = function (str) {
        str = str.replace(/\\/g, "%");
        return unescape(str);
    }

    ///模态框
    mod.modalClick = function (e, static) {
        $.fn.modal.defaults.backdrop = "static";
        $.fn.modal.defaults.spinner = $.fn.modalmanager.defaults.spinner =
            '<div class="loading-spinner" style="width: 200px; margin-left: -100px;">' +
            '<div class="progress progress-striped active">' +
            '<div class="progress-bar" style="width: 100%;"></div>' +
            '</div>' +
            '</div>';
        // general settings

        $.fn.modalmanager.defaults.resize = true;

        //ajax demo:
        var $modal = $('#ajax-modal');
        $("#user-right-menu .dropdown-menu").hide();
        // create the backdrop and wait for next modal to be triggered
        $('body').modalmanager('loading');
        var el = $(e);
        $modal.load(el.attr('data-url'), '', function () {
            $modal.modal();
        });
    }

    mod.modalClick2 = function (e, showModal) {
        $.fn.modal.defaults.backdrop = "static";
        $.fn.modal.defaults.spinner = $.fn.modalmanager.defaults.spinner =
            '<div class="loading-spinner" style="width: 200px; margin-left: -100px;">' +
            '<div class="progress progress-striped active">' +
            '<div class="progress-bar" style="width: 100%;"></div>' +
            '</div>' +
            '</div>';
        // general settings

        $.fn.modalmanager.defaults.resize = true;

        //ajax demo:
        var $modal = $('#' + showModal);
        $("#user-right-menu .dropdown-menu").hide();
        // create the backdrop and wait for next modal to be triggered
        $('body').modalmanager('loading');
        var el = $(e);
        $modal.load(el.attr('data-url'), '', function () {
            $modal.modal();
        });
    }

    mod.loginOut = function (data, dataType, url, successfn) {

        mod.post(null, "Json", "/Login/UserOut", function () {
            location.href = "/Login/Index";
        });
    };
    mod.loginTimeOut = function () {

        mod.post(null, "Json", "/Login/UserOutTimeout", function (obj) {

            if (obj.status == 1) {
                //location.href = "/Home/Index";
                return;
            }
        });
    };

    mod.blockUI = function (options) {
        options = $.extend(true, {}, options);
        var html = '';
        
        if (options.animate) {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>' + '</div>';
        } else if (options.iconOnly) {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + serverSestting.resourceUrl + '/assets/layouts/layout3/img/loading-spinner-grey.gif" align=""></div>';
        } else if (options.textOnly) {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
        } else {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + serverSestting.resourceUrl + '/assets/layouts/layout3/img/loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
        }

        if (options.target) { // element blocking
            var el = $(options.target);
            if (el.height() <= ($(window).height())) {
                options.cenrerY = true;
            }
            el.block({
                message: html,
                baseZ: options.zIndex ? options.zIndex : 999999,
                centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                css: {
                    top: '10%',
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                    opacity: options.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        } else { // page blocking
            $.blockUI({
                message: html,
                baseZ: options.zIndex ? options.zIndex : 999999,
                css: {
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                    opacity: options.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        }
    },
        mod.unblockUI = function (target) {
            if (target) {
                $(target).unblock({
                    onUnblock: function () {
                        $(target).css('position', '');
                        $(target).css('zoom', '');
                    }
                });
            } else {
                $.unblockUI();
            }
        }

    return mod;
})(window.pageInitModule || {});

var moduleTable;