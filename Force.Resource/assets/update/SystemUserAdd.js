$(function () {
    $(".ajax-submit").click(function () {
        var usecode = $("#id").data("code");
        var account = $(".account").val();
        var name = $(".name").val();
        var address = $(".address").val();
        var legalperson = $(".legalperson").val();
        var phone = $(".phone").val();
        var Level = $("#Level").val();
        var Nature = $("#Nature").val();
        var hosClass = $("#hosClass").val();
        var EmergencyNumber = $(".EmergencyNumber").val();
        var Prebed = $(".Prebed").val();
        var OpenBed = $(".OpenBed").val();
        var AllEmployees = $(".AllEmployees").val();
        var TechnicalStaffnum = $(".TechnicalStaffnum").val();
        var Department = $(".Department").val();
        var Contacts = $(".Contacts").val();
        var Tel = $(".Tel").val();
        var qq = $(".qq").val();
        var isUse = $("#isUse").val();
        var id = $("#id").val();
        App.blockUI();
        if (account == "", name == "" || address == "" || Contacts == "" || Department == "" || qq == "") {
            App.unblockUI();
            addAlert("#msg-alert", "danger", "医院名称,地址,联系人,联系部门,QQ为必填项！");
            return false;
        }
        if (!(/^[A-Za-z0-9]+$/.test(account))) {

            App.unblockUI();
            addAlert("#msg-alert", "danger", "账户只能由字母或数字组成~");
            return false;
        }
        //if (!(/^\w+([-+.]\w+)*@@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email))) {

        //    App.unblockUI();
        //    addAlert("#msg-alert", "danger", "请输入正确的邮箱！");
        //    return false;
        //}
        $.ajax({
            url: "/systemuser/save",
            data: {
                usecode: usecode,
                account: account,
                name: name,
                address: address,
                legalperson: legalperson,
                phone: phone,
                level: Level,
                nature: Nature,
                hosclass: hosClass,
                emergencynumber: EmergencyNumber,
                prebed: Prebed,
                openbed: OpenBed,
                allemployees: AllEmployees,
                technical: TechnicalStaffnum,
                department: Department,
                contacts: Contacts,
                tel: Tel,
                qq: qq,
                isUse: isUse,
                id: id
            },
            dataType: "json",
            type: "post",
            success: function (result) {
                App.unblockUI();
                if (result.status == 1) {
                    addAlert("#msg-alert", "success", "账户添加成功！初始密码为：" + result.pwd);
                    _table_systemuser.api().draw(false);
                } else {
                    if (result.msg) {
                        addAlert("#msg-alert", "danger", result.msg);
                    } else {
                        addAlert("#msg-alert", "danger", "添加失败！");
                    }
                }
            }
        })

    });
});
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