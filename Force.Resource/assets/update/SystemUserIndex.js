$(function () {
    EducationClassTable();
});

function EducationClassTable() {
    var table = $('#systemuser_table_1');

    var SystemUserTable = table.dataTable({
        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        // Or you can use remote translation file
        "language": {
            "sProcessing": "处理中...",
            "sLengthMenu": "显示 _MENU_ 项结果",
            "sZeroRecords": "没有匹配结果",
            "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
            "sInfoPostFix": "",
            "sSearch": "搜索:",
            "sUrl": "",
            "sEmptyTable": "表中数据为空",
            "sLoadingRecords": "载入中...",
            "sInfoThousands": ",",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
            "oAria": {
                "sSortAscending": ": 以升序排列此列",
                "sSortDescending": ": 以降序排列此列"
            }
        },
        "processing": true,
        "serverSide": true,
        "bSort": false,
        searching: false,
        "ajax": {
            url: '/systemuser/list',
            type: 'POST',
            data: function (d) {
                if ($('#hosname').val() != '') {
                    d.search['name'] = $('#hosname').val();
                }
            }
        },
        columns: [
            { data: 'account' },
            { data: 'name' },
            { data: 'hospitalLevel' },
            { data: 'nature' },
            { data: 'class' },
            { data: 'prebed' },
            { data: 'contacts' },
            { data: 'phone' },
            {
                data: 'status',
                render: function (data, type, row) {
                    var status_text = "<span class='label label-danger task-status' data-status=" + row.status + ">冻结</span>";
                    if (row.status == 1) {
                        status_text = "<span class='label label-success task-status' data-status=" + row.status + ">正常</span>";
                    }
                    return status_text;
                }
            },
            { data: 'createdTime' },
            {
                data: 'useCode',
                render: function (data, type, row) {
                    var text = '无法操作';
                    if (row.useCode != 10000) {
                        text = '<a onclick=\"modal_click(this);\"  class=\"ajax-model-user btn btn-default\" data-url=\"/systemuser/edit?code=' + row.useCode + '\" data-toggle=\"modal\"> 编辑 </a>';
                        if (row.status == 0) {
                            text += '<a href="javascript:;" class=\"btn btn-primary\"  data-container="body" data-toggle="confirmation-changestatus" data-placement="top" data-original-title="是否要恢复此用户" data-status="1" data-id="' + row.useCode + '" data-popout="true" class="padding-5">恢复</a>';
                        } else {
                            text += '<a href="javascript:;" class=\"btn btn-danger\"  data-container="body" data-toggle="confirmation-changestatus" data-placement="top" data-original-title="是否要冻结此用户" data-status="0" data-id="' + row.useCode + '" data-popout="true" class="padding-5">冻结</a>';
                        }
                        text += '<a onclick=\"modal_click(this);\"  class=\"ajax-model-user btn btn-success\" data-url=\"/auth/useredit?user=' + row.useCode + '\" data-toggle=\"modal\">权限设置</a>';
                    }
                    return text;
                }
            }
        ],
        // setup buttons extentension: http://datatables.net/extensions/buttons/
        buttons: [
            //{ extend: 'print', className: 'btn dark btn-outline' },
            //{ extend: 'pdf', className: 'btn green btn-outline' },
            //{ extend: 'csv', className: 'btn purple btn-outline' }
        ],

        // setup responsive extension: http://datatables.net/extensions/responsive/
        responsive: {
            details: {

            }
        },

        //"order": [
        //    [0, 'asc']
        //],

        "lengthMenu": [
            [5, 10, 15, 20, -1],
            [5, 10, 15, 20, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,

        "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

        // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
        // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
        // So when dropdowns used the scrollable div should be removed. 
        //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
    });
    $("#btn_class_search").click(function () {
        SystemUserTable.api().ajax.reload(null, false);
    });
}
function modal_click(e) {
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
function operation() {
    $("[data-toggle='confirmation-changestatus']").confirmation({
        singleton: true,
        popout: true,
        btnOkLabel: '是',
        btnCancelLabel: '否',
        onConfirm: function () {
            App.blockUI();
            $.ajax({
                url: "/main/systemuser/changestatus",
                data: { "id": $(this).data("id"), "status": $(this).data("status") },
                dataType: "json",
                type: "POST",
                success: function (result) {
                    App.unblockUI();
                    if (result.status == 1) {
                        addAlert("#bootstrap_alerts_demo", "success", "操作成功啦~");
                        _table_banner.api().draw(false);
                    } else {
                        if (result.msg) {
                            addAlert("#bootstrap_alerts_demo", "danger", result.msg);
                        } else {
                            addAlert("#bootstrap_alerts_demo", "danger", "操作失败！");
                        }
                    }
                },
                error: function () {
                    App.unblockUI();
                    addAlert("#bootstrap_alerts_demo", "danger", "操作失败！");
                }
            })
        }
    });

}
function addAlert(container, type, msg) {
    //$("html,body").animate({ scrollTop: 0 }, 500);
    App.alert({
        container: container, // alerts parent container(by default placed after the page breadcrumbs)
        type: type,  // alert's type
        message: msg,  // alert's message
        close: "true", // make alert closable
        closeInSeconds: "0" // auto close after defined seconds
    });
}