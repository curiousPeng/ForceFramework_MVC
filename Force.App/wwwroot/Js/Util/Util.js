﻿function AddAlert(type, msg) {
    $.bootstrapGrowl(msg, {
        ele: 'body', // which element to append to
        type: type, // (null, 'info', 'danger', 'success', 'warning')
        offset: {
            from: 'top',
            amount: 100
        }, // 'top', or 'bottom'
        align: 'right', // ('left', 'right', or 'center')
        width: 250, // (integer, or 'auto')
        delay: 5000, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
        allow_dismiss: true, // If true then will display a cross to close the popup.
        stackup_spacing: 10 // spacing between consecutively stacked growls.
    });
}
function modal_click(e, d = '#ajax-modal') {
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
    var $modal = $(d);
    // create the backdrop and wait for next modal to be triggered
    $('body').modalmanager('loading');
    var el = $(e);
    $modal.load(el.attr('data-url'), '', function () {
        $modal.modal();
    });

}
//function operation(e) {
//    var el = $(e);
//    var url = el.attr("data-url");
//    var value = el.attr("data-value").split(",");
//    var valueData = [];
//    for (var i = 0; i < value.length; i++) {
//        var tmp = {};
//        tmp[value[i]] = el.attr("data-" + value[i]);
//        valueData.push(tmp);
//    }
//    $(el.attr('data-toggle')).confirmation({
//        singleton: true,
//        popout: true,
//        btnOkLabel: '是',
//        btnCancelLabel: '否',
//        onConfirm: function () {
//            App.blockUI();
//            $.ajax({
//                url: url,
//                data: valueData,
//                dataType: "json",
//                type: "POST",
//                success: function (result) {
//                    App.unblockUI();
//                    if (result.status === 1) {
//                        AddAlert("success", "操作成功啦~");
//                        window.location.reload();
//                    } else {
//                        if (result.msg) {
//                            AddAlert("danger", result.msg);
//                        } else {
//                            AddAlert("danger", "操作失败！");
//                        }
//                    }
//                },
//                error: function () {
//                    App.unblockUI();
//                    AddAlert("danger", "操作失败！");
//                }
//            });
//        }
//    });
//}