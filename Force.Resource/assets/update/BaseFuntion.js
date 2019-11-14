///模态框
function modal_click(e, static) {
    if (static != false) {
        $.fn.modal.defaults.backdrop = "static";
    }
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
    // $("#user-right-menu .dropdown-menu").hide();
    // create the backdrop and wait for next modal to be triggered
    $('body').modalmanager('loading');
    var el = $(e);
    $modal.load(el.attr('data-url'), '', function () {
        $modal.modal();
    });

}
///非弹框，页面展示提示信息
function addAlert(container, type, msg,time) {
   var newtime = time || "3000";
    App.alert({
        container: container, // alerts parent container(by default placed after the page breadcrumbs)
        type: type,  // alert's type
        message: msg,  // alert's message
        close: "true", // make alert closable
        closeInSeconds: newtime // auto close after defined seconds
    });
}