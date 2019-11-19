var Login = function () {
    var handleLogin = function () {
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
                    required: "用户名必须填写"
                },
                password: {
                    required: "密码必须填写"
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   
                addAlert("#bootstrap_alerts", "danger", "用户名和密码必须输入");
            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                //form.submit(); // form validation success, call ajax form submit
                login();
            }
        });

        $('.login-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.login-form').validate().form()) {
                    //$('.login-form').submit(); //form validation success, call ajax form submit
                    login();
                }
                return false;
            }
        });
    };
    var login = function () {
        App.blockUI();
        $.ajax({
            url: $(".login-form").attr("action"),
            type: "post",
            dataType: "json",
            data: { account: $("input[name='username']").val(), password: $("input[name='password']").val() },
            success: function (result) {
                App.unblockUI();
                if (result.code == 1) {
                    window.location.href = "/home/index";
                } else {
                    addAlert("#bootstrap_alerts", "danger", result.msg);
                }
            },
            errror: function () {
                addAlert("#bootstrap_alerts", "danger", "登录请求失败！请检查网络或联系管理员！");
            }
        });
    };
    var addAlert = function (container, type, msg) {
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
    var handleForgetPassword = function () {
        $('.forget-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                email: {
                    required: true,
                    email: true
                }
            },

            messages: {
                email: {
                    required: "Email is required."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                addAlert("#bootstrap_alerts", "danger", "暂不支持此功能！");
                return;
                //form.submit();
            }
        });

        $('.forget-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.forget-form').validate().form()) {
                    //$('.forget-form').submit();
                    addAlert("#bootstrap_alerts", "danger", "暂不支持此功能！");
                }
                return false;
            }
        });

        jQuery('#forget-password').click(function () {
            jQuery('.login-form').hide();
            jQuery('.forget-form').show();
        });

        jQuery('#back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.forget-form').hide();
        });

    }

    var handleRegister = function () {

        function format(state) {
            if (!state.id) { return state.text; }
            var $state = $(
                '<span><img src="../assets/global/img/flags/' + state.element.value.toLowerCase() + '.png" class="img-flag" /> ' + state.text + '</span>'
            );

            return $state;
        }

        if (jQuery().select2 && $('#country_list').size() > 0) {
            $("#country_list").select2({
                placeholder: '<i class="fa fa-map-marker"></i>&nbsp;Select a Country',
                templateResult: format,
                templateSelection: format,
                width: 'auto',
                escapeMarkup: function (m) {
                    return m;
                }
            });


            $('#country_list').change(function () {
                $('.register-form').validate().element($(this)); //revalidate the chosen dropdown value and show error or success message for the input
            });
        }

        $('.register-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {

                fullname: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                address: {
                    required: true
                },
                city: {
                    required: true
                },
                country: {
                    required: true
                },

                username: {
                    required: true
                },
                password: {
                    required: true
                },
                rpassword: {
                    equalTo: "#register_password"
                },

                tnc: {
                    required: true
                }
            },

            messages: { // custom messages for radio buttons and checkboxes
                tnc: {
                    required: "Please accept TNC first."
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                if (element.attr("name") == "tnc") { // insert checkbox errors after the container                  
                    error.insertAfter($('#register_tnc_error'));
                } else if (element.closest('.input-icon').size() === 1) {
                    error.insertAfter(element.closest('.input-icon'));
                } else {
                    error.insertAfter(element);
                }
            },

            submitHandler: function (form) {
                //form.submit();
                addAlert("#bootstrap_alerts", "danger", "暂不支持此功能！");
            }
        });

        $('.register-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.register-form').validate().form()) {
                    //$('.register-form').submit();
                    addAlert("#bootstrap_alerts", "danger", "暂不支持此功能！");
                }
                return false;
            }
        });

        jQuery('#register-btn').click(function () {
            jQuery('.login-form').hide();
            jQuery('.register-form').show();
        });

        jQuery('#register-back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.register-form').hide();
        });
    }

    return {
        //main function to initiate the module
        init: function () {

            handleLogin();
            handleForgetPassword();
            handleRegister();

        }

    };

}();

jQuery(document).ready(function () {
    Login.init();
});