$(function () {
    //拦截form,在form提交前进行验证
    $('form').unbind('blur').bind('submit', beforeSubmit);

    //为带有valType属性的元素初始化提示信息并注册onblur事件
    $.each($("[valType]"), function (i, n) {
        $(n).poshytip({
            className: 'tip-yellowsimple',
            content: $(n).attr('msg'),
            showOn: 'none',
            alignTo: 'target',
            alignX: 'right',
            alignY: 'center',
            offsetX: -101,
            offsetY: 10
        });
        $(n).unbind('blur').bind('blur', validateBefore);
    });

    //定义一个验证器
    $.Validator = function (para) {

    }

    $.Validator.ajaxValidate = function () {
        return beforeSubmit();
    }

    //验证的方法
    $.Validator.match = function (para) {
        //定义默认的验证规则
        var defaultVal = {
            NUMBER: "^[0-9]*$",
            TEL: "^0(10|2[0-5789]|\\d{3})-\\d{7,8}$",
            IP: "^((\\d|[1-9]\\d|1\\d\\d|2[0-4]\\d|25[0-5]|[*])\\.){3}(\\d|[1-9]\\d|1\\d\\d|2[0-4]\\d|25[0-5]|[*])$",
            MOBILE: "^1((([3578])\\d{9})|(4[57]\\d{8}))$",
            MAIL: "^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$",
            IDENTITY: "((11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|50|51|52|53|54|61|62|63|64|65|71|81|82|91)\\d{4})((((19|20)(([02468][048])|([13579][26]))0229))|((20[0-9][0-9])|(19[0-9][0-9]))((((0[1-9])|(1[0-2]))((0[1-9])|(1\\d)|(2[0-8])))|((((0[1,3-9])|(1[0-2]))(29|30))|(((0[13578])|(1[02]))31))))((\\d{3}(x|X))|(\\d{4}))",
            CHINESE: "^([\u4E00-\uFA29]|[\uE7C7-\uE7F3])*$",
            URL: "^http[s]?://[\\w\\.\\-]+$",
            KardID: "^[0-9]{16,}$",
            IdCard:"^(\d{15}|\d{17}[0-9X])$"
        };
        var flag = false;
        if (para.rule == 'OTHER') {//自定义的验证规则匹配
            flag = new RegExp(para.regString).test(para.data);
        } else {
            if (para.rule in defaultVal) {//默认的验证规则匹配
                flag = new RegExp(defaultVal[para.rule]).test(para.data);
            }
        }

        return flag;
    }

    //为jquery扩展一个doValidate方法，对所有带有valType的元素进行表单验证，可用于ajax提交前自动对表单进行验证
    $.extend({
        doValidate: function () {
            return $.Validator.ajaxValidate();
        }
    });

});

//输入框焦点离开后对文本框的内容进行格式验证
function validateBefore() {
    //验证通过标识
    var flag = true;
    //获取验证类型
    var valType = $(this).attr('valType');
    //获取验证不通过时的提示信息
    var msg = $(this).attr('msg');
    //自定义的验证字符串
    var regString;

    if (valType == 'OTHER') {//如果类型是自定义，则获取自定义的验证字符串
        regString = $(this).attr('regString');
        flag = $(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType'), regString: $(this).attr('regString') });
    } else if (valType == 'VALIDATECODE') { //验证码
        if ($.isFunction(getCheckProofUrl)) {
            var rurl = getCheckProofUrl(); //验证码校验地址,get方式
            if ($.trim(rurl) == "") {
                flag = false;
            } else {
                var curObj = $(this)
                $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                    if (typeof (o) != "undefined" && o == "true") {
                        curObj.poshytip('hide');
                        falg = true;
                        curObj.attr('ov', curObj.val());
                    } else {
                        flag = false;
                        //先清除原来的tips
                        curObj.poshytip('hide');
                        //如果验证没有通过，显示tips
                        if (!flag) {
                            curObj.poshytip('show');
                        }
                        //flag = false; 
                    }
                }
                });
            }
        } else {
            flag = false;
        }
    } else {//如果类型不是自定义，则匹配默认的验证规则进行验证
        if ($(this).attr('valType') == 'required') {//不能为空的判断
            if ($(this).val() == '') {
                flag = false;
            } else if ($.trim($(this).attr("realname")) != "") {
                flag = (/^(([\u4E00-\uFA29\uE7C7-\uE7F3]+)|([a-zA-Z· ]+))$/.test($(this).val()));
                if (!flag) {
                    $(this).poshytip('update', $(this).attr("realname"));
                    $(this).poshytip('show');
                }
            }
        } else if ($(this).attr('valType') == 'MOBILE') {
            flag = $(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType') });
            if (flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已注册
                var rurl = getCheckMobileUrl(); //验证码手机地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(this);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            curObj.poshytip('hide');
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('hide');
                            curObj.poshytip('update', curObj.attr("msg2"));
                            if (!flag) { curObj.poshytip('show'); }
                        }
                    }
                    });
                    return;
                }
            } else {
                $(this).poshytip('hide');
                $(this).poshytip('update', $(this).attr("msg"));
                if (!flag) { $(this).poshytip('show'); }
                return;
            }
        } else if ($(this).attr('valType') == 'iMOBILE') {
            flag = $(this).val() == '';
            if (!flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已注册
                var rurl = getCheckiMobileUrl(); //验证码手机地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(this);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            curObj.poshytip('hide');
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('hide');
                            curObj.poshytip('update', curObj.attr("msg2"));
                            if (!flag) { curObj.poshytip('show'); }
                        }
                    }
                    });
                    return;
                }
            } else {
                $(this).poshytip('hide');
                $(this).poshytip('update', $(this).attr("msg"));
                if (!flag) { $(this).poshytip('show'); }
                return;
            }
        } else if ($(this).attr('valType') == 'MAIL') {
            flag = $(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType') });
            if (flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已注册
                var rurl = getCheckEMailUrl(); //验证码邮箱地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(this);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            curObj.poshytip('hide');
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('hide');
                            curObj.poshytip('update', curObj.attr("msg2"));
                            if (!flag) { curObj.poshytip('show'); }
                        }
                    }
                    });
                    return;
                }
            } else {
                $(this).poshytip('hide');
                $(this).poshytip('update', $(this).attr("msg"));
                if (!flag) { $(this).poshytip('show'); }
                return;
            }
        } else {//已定义规则的判断
            flag = $(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType') });
        }
    }
    //先清除原来的tips
    $(this).poshytip('hide');
    //如果验证没有通过，显示tips
    if (!flag) { $(this).poshytip('show'); }
}

//submit之前对所有表单进行验证
function beforeSubmit() {
    var tmpflag = true;
    $.each($("[valType],input[varType]"), function (i, n) {
        //清除可能已有的提示信息
        $(n).poshytip('hide'); var flag = true;
        if ($(n).attr("valType") == 'required') {//对不能为空的文本框进行验证
            if ($(n).val() == '') {
                //显示tips			
                $(n).poshytip('update', $(n).attr("msg"));
                flag = false;
            } else if ($.trim($(n).attr("realname")) != "") {
                flag = (/^([\u4e00-\u9fa5]+)$/.test($(n).val()));
                if (!flag) {
                    $(n).poshytip('update', $(n).attr("realname"));
                }
            }
        } else if ($(n).attr("valType") == 'OTHER') {//对自定义的文本框进行验证
            if (!($(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType'), regString: $(this).attr('regString') }))) {
                flag = false;
            }
        } else if ($(n).attr("valType") == 'VALIDATECODE') { //验证码
            if ($(n).val() == '') flag = false;
        } else if ($(n).attr('valType') == 'MOBILE') {
            flag = $(n).val() != '' && $.Validator.match({ data: $(n).val(), rule: $(n).attr('valType') });
            if (flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已注册
                var rurl = getCheckMobileUrl(); //验证码手机地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(n);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('update', curObj.attr("msg2"));
                        }
                    }
                    });
                }
            } else {
                $(this).poshytip('update', $(this).attr("msg"));
            }
        } else if ($(n).attr('valType') == 'iMOBILE') {
            flag = $(n).val() == '';
            if (!flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已存在
                var rurl = getCheckiMobileUrl(); //验证码手机地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(n);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('update', curObj.attr("msg2"));
                        }
                    }
                    });
                }
            } else {
                $(this).poshytip('update', $(this).attr("msg"));
            }
        } else if ($(this).attr('valType') == 'MAIL') {
            flag = $(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType') });
            if (flag && $.trim($(this).attr("notcheck")) == "") { //判断是否已注册
                var rurl = getCheckEMailUrl(); //验证码邮箱地址,get方式
                if ($.trim(rurl) == "") {
                    flag = false;
                } else {
                    var curObj = $(this);
                    $.ajax({ url: rurl, type: "get", async: false, success: function (o) {
                        if (typeof (o) != "undefined" && o.toLowerCase() == "true") {
                            falg = true;
                        } else {
                            flag = false;
                            curObj.poshytip('update', curObj.attr("msg2"));
                        }
                    }
                    });
                }
            } else {
                $(this).poshytip('update', $(this).attr("msg"));
            }
        } else {//对使用已定义规则的文本框进行验证			
            if (!($(this).val() != '' && $.Validator.match({ data: $(this).val(), rule: $(this).attr('valType') }))) {
                flag = false;
            }
        }
        if (!flag) {
            $(n).poshytip('show');
            tmpflag = false;
        }
    });
    return tmpflag;
}

//下面是测试代码，不属于验证器的功能代码之内
//用原型的方式来模拟js的类
function Validators() {

}

Validators.prototype.subByJs = function (e) {
    if ($.doValidate()) {
        alert('验证通过');
        //todo
    }
}