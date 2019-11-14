$(function () {
    $("#btnLogin").click(function () {
        var Account = $("#Account").val();
        var Password = $("#Password").val();

        if (Account == null || Account == "") {
            pageInitModule.ToastrCustom("", "账号不能为空", "error", "toast-top-center");
            return;
        }
        if (Password == null || Password == "") {
            pageInitModule.ToastrCustom("", "密码不能为空", "error", "toast-top-center");
            return;
        } 
        var entity = {};
        entity.Account = Account;
        entity.Password = Password;

        var RequestUrl = "/Login/SignIn";
        pageInitModule.blockUI();//锁
        pageInitModule.post(entity, "JSON", RequestUrl, function (obj) {
            pageInitModule.unblockUI();//解锁
            if (obj.code == 0) {
                _cookie.set("token", obj.Token, 24);
                pageInitModule.ToastrCustom("", "成功", "success", "toast-top-center");
                setTimeout(function () {
                    location.href = "/Home/Index";
                }, 400);

            } else {
                pageInitModule.ToastrCustom("", obj.msg, "error", "toast-top-center");
            }
        });
    });

    //点击回车模拟点击登录按钮
    $("#Password").keydown(function (e) {
        if ((e || event).keyCode == 13) {
            $("#btnLogin").click();
        }
    });
});