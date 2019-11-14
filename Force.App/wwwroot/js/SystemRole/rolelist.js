$(function () {
    datePick.timeSection2("RoleTimeSection", "endTime", "", "", "365");
    UserIndexTable();
});
function UserIndexTable() {
    var table = $('#RoleTable');

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
            url: '/SystemRole/GetRoleList',
            type: 'POST',
            data: function (d) {
                //查询条件参数
                d.roleName = $('#roleNameSearch').val() != "" ? $('#roleNameSearch').val() : "";
                d.roleId = $('#roleIdSearch').val() != "" ? $('#roleIdSearch').val() : "";
                if ($('#roleIdSearch').val() != "" && !isNaN($('#roleIdSearch').val())) {
                    d.roleId = $('#roleIdSearch').val();
                } else {
                    d.roleId = "";
                    $('#roleIdSearch').val("");
                }

                d.startTime = $('#RoleTimeSection').data("start") != "" ? $('#RoleTimeSection').data("start") : "";
                d.endTime = $('#RoleTimeSection').data("end") != "" ? $('#RoleTimeSection').data("end") : "";
            }
        },
        //< th class= "all text-center" > 权限ID</th >
        //<th class="all text-center">权限名称</th>
        //<th class="all text-center">创建时间</th>
        //<th class="all text-center">操作</th>
        //页面显示数据字段，绑定数据。
        columns: [
            { data: 'id' },
            { data: 'name' },
            { data: 'createdTime' },
            {
                data: 'id',
                render: function (data, type, row) {
                    var status_text = '';
                    //status_text += '<button  class="btn btn-default" type="button" data-id="' + row.id + '" onclick="OpDelete(' + row.id + ')">删除</button>';

                    status_text += '<button  class="btn btn-danger" type="button"  onmouseover=\"OpDelete(this);\"  data-container="body" data-toggle="confirmation-changestatus" data-placement="top" data-original-title="是否要删除该数据" data-id="' + row.id + '" data-popout="true">删除 <i class="fa fa-trash-o"></i></button>';

                    status_text += '<button class="btn sbold btn-info" type="button" data-id="' + row.id + '" data-url="/SystemRole/RoleEdit?id=' + row.id + '" onclick="pageInitModule.modalClick(this)" data-toggle="modal">编辑 <i class="fa fa-edit"></i></button>';

                    status_text += '<button class="btn sbold btn-info" type="button" data-id="' + row.id + '" data-url="/SystemRole/RoleMenu?id=' + row.id + '" onclick="pageInitModule.modalClick2(this,\'ModalRoleMenu\')" data-toggle="modal">权限绑定</button>';

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
            [10, 20, 30, 50, 100],
            [10, 20, 30, 50, 100] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,

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

function OpDelete(e) {
    $(e).confirmation({
        singleton: true,
        popout: true,
        btnOkLabel: '是',
        btnCancelLabel: '否',
        onConfirm: function () {
            var entity = {
                id: $(e).data("id")
            };
            var strUrl = "/SystemRole/OpDelete";
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