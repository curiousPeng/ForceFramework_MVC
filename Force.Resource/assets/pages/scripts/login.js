var Login = function() {

    var handleLogin = function() {

        $('.login-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                },
                remember: {
                    required: false
                }
            },

            messages: {
                username: {
                    required: "用户名必须填写."
                },
                password: {
                    required: "密码必须填写."
                }
            },

            invalidHandler: function(event, validator) { //display error alert on form submit   
                 addAlert("#bootstrap_alerts", "danger","用户名和密码必须输入");
            },

            highlight: function(element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function(label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function(error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function(form) {
                //form.submit(); // form validation success, call ajax form submit
                App.blockUI();
                $.ajax({
                    url: $(".login-form").attr("action"),
                    type: "post",
                    dataType: "json",
                    data: { account: $("input[name='username']").val(), pwd: $("input[name='password']").val() },
                    success: function (result) {
                        App.unblockUI();
                        if (result.status == 1) {
                            if (result.pwdStrength < 2) {
                                window.location.href = "/home/firstlogin";
                            } else {
                                window.location.href = "/productpackage/index";
                            }
                        } else {
                            addAlert("#bootstrap_alerts", "danger", result.msg);
                        }
                    },
                    errror: function () {
                        addAlert("#bootstrap_alerts", "danger", "登录请求失败！请检查网络或联系管理员！");
                    }
                });
            }
        });

        /*$('.login-form input').keypress(function(e) {
            if (e.which == 13) {
                if ($('.login-form').validate().form()) {
                    $('.login-form').submit(); //form validation success, call ajax form submit
                }
                return false;
            }
        });*/
    }
    var addAlert = function(container, type, msg) {
        App.alert({
            container: container, // alerts parent container(by default placed after the page breadcrumbs)
            type: type,  // alert's type
            message: msg,  // alert's message
            close: "true", // make alert closable
            closeInSeconds: 3 // auto close after defined seconds
        });
    }
    var handleRegister = function() {
        jQuery('#register-btn').click(function() {
            addAlert("#bootstrap_alerts", "danger","需要创建账户请联系管理员哦!");

        });

    
    }
    var drag = function(){
            $('#drag').drag();
    }
    return {
        //main function to initiate the module
        init: function() {
            handleLogin();
            handleRegister();
            drag();
        }

    };

}();

jQuery(document).ready(function() {
    Login.init();
});