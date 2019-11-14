var Update = function () {

    var handleLogin = function () {
        $('.update-pw').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                password1: {
                    required: true
                },
                password2: {
                    required: true
                }
            },
            messages: {
                password1: {
                    required: "密码必须填写."
                },
                password2: {
                    required: "密码必须填写."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

                addAlert("#bootstrap_alerts", "danger", "必须输入密码！");
            },

            highlight: function (element) { // hightlight error inputs
                $(element).closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                var pw_1 = $("input[name='password1']").val();
                var pw_2 = $("input[name='password2']").val();
                if (pw_2 != pw_1) {
                    addAlert("#bootstrap_alerts", "danger", "两次输入密码不同！");
                    return false;
                }
                //form.submit(); // form validation success, call ajax form submit
                submitAction();
            }
        });
        //$('.update-pw input').keypress(function (e) {
        //    if (e.which == 13) {
        //        if ($('.update-pw').validate().form()) {
        //            submitAction();//form validation success, call ajax form submit
        //        }
        //        return false;
        //    }
        //});
    }
    var addAlert = function (container, type, msg) {
        App.alert({
            container: container, // alerts parent container(by default placed after the page breadcrumbs)
            type: type,  // alert's type
            message: msg,  // alert's message
            close: "true", // make alert closable
            closeInSeconds: 3 // auto close after defined seconds
        });
    }
    function submitAction(){
        App.blockUI();
            $.ajax({
                    url: $(".update-pw").attr("action"),
                    type: "post",
                    dataType: "json",
                    data: { pwd1: $("input[name='password1']").val(), pwd2: $("input[name='password2']").val(),usecode:$("#usecode").val() },
                    success: function (result) {
                        App.unblockUI();
                        if (result.status == 1) {
                            addAlert("#bootstrap_alerts_1", "success", "修改成功~");
                             window.location.href = "/home/index";
                        } else {
                            addAlert("#bootstrap_alerts_1", "danger", result.msg);
                        }
                    },
                    errror: function () {
                        addAlert("#bootstrap_alerts_1", "danger", "修改请求失败！请检查网络或联系管理员！");
                    }
                });

    }
    var handleRegister = function () {
        jQuery('.next-button').click(function () {
            App.blockUI();
            $.ajax({
                url: "/home/pwdcheck",
                type: "post",
                dataType: "json",
                data: { pwd: $("input[name='password1']").val()},
                success: function (result) {
                    App.unblockUI();
                    if (result.status == 1) {
                        jQuery('.password_1').hide();
                        jQuery('.password_2').show();
                    } else {
                        addAlert("#bootstrap_alerts", "danger", result.msg);
                    }
                },
                errror: function () {
                    addAlert("#bootstrap_alerts", "danger", "修改请求失败！请检查网络或联系管理员！");
                }
            });
            
        });
        jQuery('.last-button').click(function () {
            jQuery('.password_1').show();
            jQuery('.password_2').hide();
        });

    }
    return {
        //main function to initiate the module
        init: function () {
            handleLogin();
            handleRegister();
        }

    };

}();

jQuery(document).ready(function () {
    Update.init();
});