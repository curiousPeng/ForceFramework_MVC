function NotifyAlert(type, msg) {
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
function MetAlert(container, type, msg) {
    //$("html,body").animate({ scrollTop: 0 }, 500);
    App.alert({
        container: container, // alerts parent container(by default placed after the page breadcrumbs)
        type: type,  // alert's type
        message: msg,  // alert's message
        close: "true", // make alert closable
        closeInSeconds: "0" // auto close after defined seconds
    });
}

function operation(e) {
    var el = $(e);
    var url = el.data("url");
    var value = el.data("value").split(",");
    var valueData = "{";
    for (var i = 0; i < value.length; i++) {
        valueData += value[i] + ":" + el.data(value[i]) + ",";
    }
    valueData = valueData.slice(0, valueData.length - 1);
    valueData += "}";
    $(e).confirmation({
        singleton: true,
        popout: true,
        btnOkLabel: '是',
        btnCancelLabel: '否',
        onConfirm: function () {
            App.blockUI();
            $.ajax({
                url: url,
                data: eval('(' + valueData + ')'),
                dataType: "json",
                type: "POST",
                success: function (result) {
                    if (result.code === 1) {
                        NotifyAlert("success", "操作成功啦~");
                        window.setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    } else {
                        App.unblockUI();
                        if (result.msg) {
                            NotifyAlert("danger", result.msg);
                        } else {
                            NotifyAlert("danger", "操作失败！");
                        }
                    }
                },
                error: function () {
                    App.unblockUI();
                    NotifyAlert("danger", "操作失败！");
                }
            });
        }
    });
}