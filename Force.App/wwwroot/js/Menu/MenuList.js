$(function () {
    UserIndexTable();
});
function repeat(str, n) {

    return new Array(n + 1).join(str);

}

function UserIndexTable() {
    var table = $('#MenuTable');

    moduleTable = table.dataTable({
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
            //url: '/GlobalSettings/GetGameInfoList',
            url: '/Menu/GetMenuList',
            type: 'POST',
            data: function (d) {
                //查询条件参数 
                d.menuStatus = $('#MenuStatusSearch').val() != "" ? $('#MenuStatusSearch').val() : "";
            }
        },
        //页面显示数据字段，绑定数据。
        columns: [
            { data: 'id' },
            {
                data: 'name',
                className: 'text-left content-width-300',
                render: function (data, type, row) {
                    var status_text = "";
                    if (row.parentList == null || row.parentList == "") {
                        status_text = "ㅏ" + " " + row.name;
                    } else {
                        var dept = row.parentList.split(',').length;
                        if (dept > 0) {
                            if (dept == 2) {
                                status_text = "ㅡ" + " " + row.name;
                            } else {
                                status_text = "ㅏ" + repeat("ㅗ", row.parentList.split(',').length - 2) + " " + row.name;
                            }
                        } else {
                            status_text = "ㅏ" + repeat("ㅗ", row.parentList.split(',').length) + " " + row.name;
                        }

                    }
                    return status_text;
                }
            },
            { data: 'parentId' },
            { data: 'icon', className: 'content-width-100' },
            { data: 'sort' },
            {
                data: 'isUse',
                render: function (data, type, row) {
                    var status_text = "";
                    if (row.isUse) {
                        status_text = "<span class='label label-success task-status' data-status=" + row.status + ">已开启</span>";
                    } else {
                        status_text = "<span class='label label-warning task-status' data-status=" + row.status + ">已关闭</span>";
                    }

                    return status_text;
                }
            },
            {
                data: 'type',
                render: function (data, type, row) {
                    var status_text = "";
                    if (row.type == 1) {
                        status_text = "菜单";
                    } else if (row.type == 2) {
                        status_text = "新增";
                    } else if (row.type == 3) {
                        status_text = "编辑";
                    }
                    else if (row.type == 4) {
                        status_text = "删除";
                    }
                    else if (row.type == 5) {
                        status_text = "查询";
                    }
                    else if (row.type == 6) {
                        status_text = "页面";
                    }
                    return status_text;
                }
            },

            { data: 'actionRoute', className: 'content-width-300' },
            //{ data: 'createdTime' },
            {
                data: 'id',
                className: 'content-width-100',
                render: function (data, type, row) {
                    var status_text = '';
                    if (row.parentId == 0) {
                        status_text += '<button class="btn sbold btn-success" type="button" data-id="' + row.id + '" data-url="/menu/menuedit?id=' + row.id + '&parentId=' + row.id + '&parentIsAdd=true" onclick="pageInitModule.modalClick(this)" data-toggle="modal">新增 <i class="fa fa-plus"></i></button>';
                    } else {
                        if (row.isUse) {
                            status_text += '<input type="checkbox" data-id="' + row.id + '" class="make-switch make-switch-' + row.id + '" checked data-on-text="开" data-on-color="success" data-off-color="warning" data-off-text="关">&nbsp;&nbsp;';
                        } else {
                            status_text += '<input type="checkbox" data-id="' + row.id + '" class="make-switch make-switch-' + row.id + '" data-on-text="开" data-on-color="success" data-off-color="warning" data-off-text="关">&nbsp;&nbsp;';
                        }

                        status_text += '<button class="btn btn-danger" type="button"  onmouseover=\"menuDelete(this);\"  data-container="body" data-toggle="confirmation-changestatus" data-placement="top" data-original-title="是否要删除该数据" data-id="' + row.id + '" data-popout="true">删除 <i class="fa fa-trash-o"></i></button>';

                        //status_text += '<button  class="btn btn-default" type="button" data-id="' + row.id + '" onclick="menuDelete(' + row.id + ')">删除</button>';

                        status_text += '<button class="btn sbold btn-info" type="button" data-id="' + row.id + '" data-url="/menu/menuedit?id=' + row.id + '&parentId=' + row.parentId + '" onclick="pageInitModule.modalClick(this)" data-toggle="modal">编辑<i class="fa fa-edit"></i></button>';

                        status_text += '<button class="btn sbold btn-success" type="button" data-id="' + row.id + '" data-url="/menu/menuedit?id=' + row.id + '&parentId=' + row.id + '&parentIsAdd=true" onclick="pageInitModule.modalClick(this)" data-toggle="modal">新增 <i class="fa fa-plus"></i></button>';
                    }
                    return status_text;
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
            [200, 400],
            [200, 400] // change per page values here
        ],
        // set the initial value
        "pageLength": 200,

        "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
        "initComplete": function (settings, json) { //初始化完成后触发,刷新时不会

        }
        ,
        drawCallback: function () {//每行执行是出发改函数，刷新表单后也会触发
            $('.make-switch').bootstrapSwitch({
                onSwitchChange: function (e, data) {
                    var id = e.target.attributes["data-id"].value;
                    isMenu(id);
                }
            });
        }
    });
    $("#btn_class_search").click(function () {
        moduleTable.api().ajax.reload();
    });
}

function isMenu(id) {
    var entity = {
        id: id
    };
    var strUrl = "/Menu/OpIsUse";
    pageInitModule.blockUI();
    pageInitModule.post(entity, 'JSON', strUrl, function (obj) {
        pageInitModule.unblockUI();
        if (obj.status == 1) {
            pageInitModule.ToastrCustom("", "成功", "success", "toast-top-center");
            moduleTable.api().ajax.reload();

            setTimeout(function () {
                history.go(0);
            }, 600);

        } else {
            $('.make-switch-' + id).bootstrapSwitch('state', !$('.make-switch-' + id).bootstrapSwitch('state'), true);

            pageInitModule.ToastrCustom("", obj.msg, "error", "toast-top-center");
        }
    });
}

function menuDelete(e) {
    $(e).confirmation({
        singleton: true,
        popout: true,
        btnOkLabel: '是',
        btnCancelLabel: '否',
        onConfirm: function () {
            var entity = {
                id: $(e).data("id")
            };
            var strUrl = "/Menu/OpDelete";

            pageInitModule.blockUI();
            pageInitModule.post(entity, 'JSON', strUrl, function (obj) {
                pageInitModule.unblockUI();
                if (obj.status == 1) {
                    pageInitModule.ToastrCustom("", "成功", "success", "toast-top-center");
                    moduleTable.api().ajax.reload();
                } else {
                    pageInitModule.ToastrCustom("", obj.msg, "error", "toast-top-center");
                }
            });
        }
    });


}