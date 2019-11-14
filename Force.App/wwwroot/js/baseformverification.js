/** 
 * 版本: v1.1.0.0
 * 最后更新时间: 2018-11-01 12:12:00
 * 说明: 表单验证插件， 当使用ajax表单提交时，将会在确认按钮上面绑定一个按钮事件，提前做一次基础类型验证。 
 *       如果正则不够时，可自行扩展
 * 使用规则:
 *  1.将插件放置在本身需要执行ajax提交的事件之上
 *  2.在需要执行ajax事件最开的位置添加
 *  if (!dataSubmit) {
        return dataSubmit;
     } else {
        dataSubmit = false;
     }
    3.使用特殊介绍
        需要验证的input标签、select、textarea标签 之上的父级 需要一个class   目前支持 ：base-form-obj、modal-body
        确认按钮上面需要添加一个class:最好是class中最前面  目前支持 js 插件初始化 自定义  formIdObj = 'xxx'  默认：base-form
 *
 * 使用介绍:
 *  1.插件是需要在每个需要验证的标签上面打上特定标识
 * 标识规则:
 *  1.data-ConditionText                                标签名称
 *      value:  string
 *  2.data-isCondition                                  非空时是否验证当前标签
 *      value:  on   off
 *  3.data-ConditionIsRequired                          是否可为空
 *      value:  on   off
 *  4.data-Condition                                    验证规则
 *      value:  单条件 required、phone       多条件 required|phone
 *              条件为时 maxLength 需要添加 额外标识  data-maxLength    value: int
 *              条件为时 minLength 需要添加 额外标识  data-minLength    value: int
 *              条件为时 minNumber 需要添加 额外标识  data-minNumber    value: int
 *              条件为时 maxNumber 需要添加 额外标识  data-maxNumber    value: int
 * 所有验证规则介绍:
 *  required                                            非空
 *  phone                                               手机号
 *  bankcard                                            银行卡
 *  number                                              数值
 *  double
 *  email                                               邮箱
 *  isdate                                              是否时间
 * */

//验证是否通过
var dataSubmit = true;
var reg_template = '{text} 内容 {value} ';
var reg = {
    reg_required_error: reg_template + '不可为空,请输入正确的值',
    reg_sql_error: '请您不要在参数中输入特殊字符和SQL关键字！',
    reg_sql: /select|update|delete|truncate|join|union|exec|insert|drop|count|'|"|=|;|>|<|%/i,
    reg_sql_url: /select|update|delete|truncate|join|union|exec|insert|drop|count|'|"|;|>|<|%/i,
    reg_number: "^[0-9]*$",
    reg_number_error: reg_template + '有误,请输入正确的数值',
    reg_int: "/^[0-9]$",
    reg_int_error: reg_template + '有误,请输入正确的数值',
    reg_phone: /^1(3|4|5|7|8)\d{9}$/,
    reg_phone_error: reg_template + '有误,请输入正确的手机号',
    reg_bankcard_error: reg_template + '有误,请输入正确的银行卡号',
    reg_double: "/^[-\+]?([1-9](\d+)?|0)(\.\d{2})$/",
    reg_double_error: reg_template + '有误,请输入正确的金额',
    reg_gold: /^(([1-9][0-9]*)|(([0]\.\d{1,2}|[1-9][0-9]*\.\d{1,2})))$/,
    reg_gold_error: reg_template + '有误,请输入大于零的金额',
    reg_peoplenumber: /^\d/,
    reg_peoplenumber_error: reg_template + '有误,人数不能少于2人',
    reg_email: /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/,
    reg_email_error: reg_template + '有误,请输入正确的邮箱',
    reg_custom_error: reg_template + '有误,请输入正确的值', //自定义参数错误信息
    reg_length_error: reg_template + '' //自定义参数错误信息
}

var btnClassName = 'base-form'; //确认按钮class名称

var formClassName = 'portlet-body'; //所有标签父级class名称
formClassName += ' .base-form-obj';
var formClassInput = '.base-form-obj input';
formClassInput += ',.modal-body input';
var formClassSelect = '.base-form-obj select';
formClassSelect += ',.modal-body select';
var formClassTextarea = '.base-form-obj textarea';
formClassTextarea += ',.modal-body textarea';

if (typeof formIdObj != "undefined" && formIdObj != "") {
    btnClassName = formIdObj;
}

if (typeof formClassObj != "undefined" && formClassObj != "") {
    formClassInput = "."+formClassObj+" input";
    formClassSelect = "." + formClassObj +" select";
    formClassTextarea = "." + formClassObj +" textarea";
}

$("." + btnClassName).click(function () {
    
    formIdObj = "";
    var errorInfo = "";
   
    var inputList = $(formClassInput);

    if ($(formClassSelect).length > 0) {
        inputList = inputList.add($(formClassSelect));
    }

    if ($(formClassTextarea).length > 0) {
        inputList = inputList.add($(formClassTextarea));
    }

    for (var i = 0; i < inputList.length; i++) {
        if (inputList.eq(i).data().condition != null) {

            var isTrue = true; //当前是否有条件验证通过
            var thisObj = inputList.eq(i);//当前对象
            var thisValue = inputList.eq(i).val().trim();//当前对象内容
            var thisValueTemp = ""; //临时返回值
            var thisconditiontext = thisObj.data("conditiontext") != undefined ? thisObj.data("conditiontext") : ""; //当前标签名称
            var thisIscondition = thisObj.data("iscondition") != undefined ? thisObj.data("iscondition") : ""; //当前验证协议
            var thisIsRequired = thisObj.data("conditionisrequired") != undefined ? thisObj.data("conditionisrequired") : ""; //是否可为空

            var isdate = false; //区间时间验证
            var dateStart = "";//区间开始时间
            var dateEnd = "";//区间结束时间

            var isPas = false; //输入密码是否正确
            var pasStart = "";//开始输入密码
            var pasEnd = "";//最后确认密码

            //数据库敏感词验证
            if (thisValue != null && thisValue != "") {
                if (reg.reg_sql.test(thisValue)) {
                    isTrue = false;
                    errorInfo = reg.reg_sql_error;
                }
            }

            var conList = thisObj.data().condition.split("|"); //当前验证条件list
            if (conList.length > 0) {
                for (var y = 0; y < conList.length; y++) {
                    switch (conList[y]) {
                        case "required":
                            if (thisValue == null || thisValue == "") {
                                isTrue = false;
                                errorInfo = reg.reg_required_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);
                            }
                            break;
                        case "phone":
                            if (isTrue) {
                                errorInfo = reg.reg_phone_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = verifyByReg(thisValue, reg.reg_phone);
                            }
                            break;
                        case "bankcard":
                            errorInfo = reg.reg_bankcard_error.replace("{text}", thisconditiontext);
                            errorInfo = errorInfo.replace("{value}", thisValueTemp);
                            if (!checkbankcard(thisValue)) {
                                isTrue = false;
                            };
                            break;
                        case "number":
                            if (isTrue) {
                                errorInfo = reg.reg_number_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = verifyByReg(thisValue, reg.reg_number);
                            }
                            break;
                        case "int":
                            if (isTrue) {
                                errorInfo = reg.reg_int_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = verifyByReg(thisValue, reg.reg_int);
                            }
                            break;
                        case "double":
                            if (isTrue) {
                                errorInfo = reg.reg_double_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = verifyByReg(thisValue, reg.reg_double);
                            }
                            break;
                        case "email":
                            if (isTrue) {
                                errorInfo = reg.reg_email_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = verifyByReg(thisValue, reg.reg_email);
                            }
                            break;
                        case "isdate":
                            if (isTrue) {
                                errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isTrue = validateDate(thisValue);
                            }
                            break;
                        case "datenow":
                            if (isTrue) {
                                errorInfo = reg.reg_length_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", "大于等于当前日期");

                                isTrue = validateDate(thisValue);
                                var pdate = new Date();

                                var strDate = pdate.getFullYear() + "/" + (pdate.getMonth() + 1) + "/" + pdate.getDate();

                                if (isTrue && new Date(thisValue) >= new Date(strDate)) {
                                    isTrue = true;
                                } else {
                                    isTrue = false;
                                }
                            }

                            break;
                        case "sectionDate_start":
                            if (isTrue) {
                                errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                isdate = validateDate(thisValue);
                                dateStart = thisValue;

                                if (thisValue == "" && thisIsRequired == "on") {
                                    isdate = true;
                                }

                                if (thisIscondition != "" && thisIscondition == "off") {
                                    isdate = true;
                                }
                            }
                            break;
                        case "sectionDate_end":
                            if (isTrue) {
                                errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                dateEnd = thisValue;
                                if (isdate && validateDate(thisValue)) {
                                    if (new Date(dateStart) > dateEnd) {
                                        isdate = false;
                                    }
                                }
                                if (thisValue == "" && thisIsRequired == "on") {
                                    isdate = true;
                                }
                                if (thisIscondition != "" && thisIscondition == "off") {
                                    isdate = true;
                                }
                            }
                            break;
                        case "password_start":
                            if (isTrue) {
                                errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                if (thisValue == "") {
                                    isPas = false;
                                }
                                pasStart = thisValue;

                                if (thisValue == "" && thisIsRequired == "on") {
                                    isPas = true;
                                }

                                if (thisIscondition != "" && thisIscondition == "off") {
                                    isPas = true;
                                }
                            }
                            break;
                        case "password_end":
                            if (isTrue) {
                                errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                pasEnd = thisValue;
                                if (isPas && pasStart != pasEnd) {
                                    isPas = false;
                                }

                                if (thisValue == "" && thisIsRequired == "on") {
                                    isdate = true;
                                }
                                if (thisIscondition != "" && thisIscondition == "off") {
                                    isdate = true;
                                }
                            }
                            break;
                        case "minLength":
                            var minLength = thisObj.data("minlength");
                            if (isTrue) {
                                errorInfo = reg.reg_length_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", "最小 " + minLength + "位");

                                if (isNaN(minLength)) {
                                    minLength = 0;
                                } else {
                                    minLength = parseInt(minLength);
                                }

                                if (thisValue.length >= minLength) {
                                    isTrue = true;
                                } else {
                                    isTrue = false;
                                }
                            }
                            break;

                        case "maxLength":
                            var maxLength = thisObj.data("maxlength");
                            if (isTrue) {
                                errorInfo = reg.reg_length_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", "最大 " + maxLength + "位");

                                if (isNaN(maxLength)) {
                                    maxLength = 0;
                                } else {
                                    maxLength = parseInt(maxLength);
                                }

                                if (thisValue.length <= maxLength) {
                                    isTrue = true;
                                } else {
                                    isTrue = false;
                                }
                            }
                            break;
                        case "gold":
                            if (isTrue) {
                                errorInfo = reg.reg_gold_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);
                                isTrue = reg.reg_gold.test(thisValue);
                            }
                            break;
                        case "peoplenumber":
                            if (isTrue) {
                                errorInfo = reg.reg_peoplenumber_error.replace("{text}", thisconditiontext);
                                errorInfo = errorInfo.replace("{value}", thisValueTemp);
                                if (reg.reg_gold.test(thisValue)) {
                                    if (Number(thisValue) < 2) {
                                        isTrue = false;
                                    } else {
                                        isTrue = true;
                                    }
                                } else {
                                    isTrue = false;
                                }
                            }
                            break;
                        case "minNumber":
                            if (isTrue) {
                                var minNumber = thisObj.data("minnumber");

                                errorInfo = reg.reg_length_error.replace("{text}", thisconditiontext);
                                var objValue = "最小 " + minNumber + " 数值";

                                if (minNumber != undefined && minNumber != "") {
                                    if (isNaN(minNumber)) {
                                        isTrue = false;
                                        objValue = "规则不正确";
                                    } else {
                                        if (isNaN(thisValue)) {
                                            objValue = "请输入正确数值";
                                        } else {
                                            if (parseInt(thisValue) >= parseInt(minNumber)) {
                                                isTrue = true;
                                            } else {
                                                isTrue = false;
                                            }
                                        }
                                    }
                                }
                                errorInfo = errorInfo.replace("{value}", objValue);
                            }
                            break;
                        case "maxNumber":
                            if (isTrue) {
                                var maxNumber = thisObj.data("maxnumber");
                                errorInfo = reg.reg_length_error.replace("{text}", thisconditiontext);
                                var objValue = "最大 " + maxNumber + " 数值";

                                if (maxNumber != undefined && maxNumber != "") {
                                    if (isNaN(maxNumber)) {
                                        isTrue = false;
                                        objValue = "规则不正确";
                                    } else {
                                        if (isNaN(thisValue)) {
                                            objValue = "请输入正确数值";
                                        } else {
                                            if (parseInt(thisValue) <= parseInt(maxNumber)) {
                                                isTrue = true;
                                            } else {
                                                isTrue = false;
                                            }
                                        }
                                    }
                                }
                                errorInfo = errorInfo.replace("{value}", objValue);
                            }
                            break;
                        default:
                            //thisObj.data("conditionreg")  当前额外正则验证规则 - 对单个
                            if (thisObj.data("condition") != undefined && thisObj.data("conditionreg") != undefined) {
                                if (isTrue) {
                                    errorInfo = reg.reg_custom_error.replace("{text}", thisconditiontext);
                                    errorInfo = errorInfo.replace("{value}", thisValueTemp);

                                    var reg_custom = thisObj.data("conditionreg");
                                    if (thisValue == null || thisValue == "") {
                                        isTrue = false;
                                    } else {
                                        isTrue = verifyByReg(thisValue, reg_custom);
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            if (thisValue == "" && thisIsRequired == "on") {
                isTrue = true;
            }

            if (thisIscondition != "" && thisIscondition == "off") {
                isTrue = true;
            }

            if (!isTrue) {
                dataSubmit = isTrue;
                pageInitModule.ToastrCustom("", errorInfo, "error", "toast-top-center");
                return;
            }

            dataSubmit = isTrue;
        }
    }
});

function verifyByReg(value, reg, errorInfo) {
    if (value != '' & value != '') {
        if ((typeof reg == 'string')) {
            var regExp = new RegExp(reg);
            return regExp.test(value);
        } else {
            return reg.test(value);
        }
    }
    return false;
}

function validateDate(dateStr) {
    if (dateStr == "") {
        return false;
    }

    if (new Date(dateStr).toTimeString() == "Invalid Date") {
        return false;
    } 
    return true;
}

//银行卡号码检测
function checkbankcard(banknumber) {
    var bankno = banknumber;
    var lastNum = bankno.substr(bankno.length - 1, 1); //取出最后一位（与luhn进行比较）
    var first15Num = bankno.substr(0, bankno.length - 1); //前15或18位
    var newArr = new Array();
    for (var i = first15Num.length - 1; i > -1; i--) { //前15或18位倒序存进数组
        newArr.push(first15Num.substr(i, 1));
    }
    var arrJiShu = new Array(); //奇数位*2的积 <9
    var arrJiShu2 = new Array(); //奇数位*2的积 >9
    var arrOuShu = new Array(); //偶数位数组
    for (var j = 0; j < newArr.length; j++) {
        if ((j + 1) % 2 == 1) { //奇数位
            if (parseInt(newArr[j]) * 2 < 9) arrJiShu.push(parseInt(newArr[j]) * 2);
            else arrJiShu2.push(parseInt(newArr[j]) * 2);
        } else //偶数位
            arrOuShu.push(newArr[j]);
    }

    var jishu_child1 = new Array(); //奇数位*2 >9 的分割之后的数组个位数
    var jishu_child2 = new Array(); //奇数位*2 >9 的分割之后的数组十位数
    for (var h = 0; h < arrJiShu2.length; h++) {
        jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
        jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
    }

    var sumJiShu = 0; //奇数位*2 < 9 的数组之和
    var sumOuShu = 0; //偶数位数组之和
    var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
    var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
    var sumTotal = 0;
    for (var m = 0; m < arrJiShu.length; m++) {
        sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
    }

    for (var n = 0; n < arrOuShu.length; n++) {
        sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
    }

    for (var p = 0; p < jishu_child1.length; p++) {
        sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
        sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
    }
    //计算总和
    sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

    //计算luhn值
    var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
    var luhn = 10 - k;

    if (lastNum == luhn) {
        return true;
    } else {
        return false;
    }
} 