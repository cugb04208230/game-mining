// JavaScript Document
$(window).load(function () {
    $('#status').fadeOut();
    $('#preloader').delay(350).fadeOut('slow');
    $('img[data-original]').lazyload();
});
function isWeiXin() {
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        return true;
    } else {
        return false;
    }
}
$(document).ready(function () {
    $('header').next().css('margin-top', $('header').height());
    $('footer').prev().css('margin-bottom', $('footer').outerHeight());
    $('.cui-tabnum7').each(function () {
        var s = $(this).siblings().length;
        if (s > 0) {
            $(this).width((100 / s) + '%');
        }
    });
    $('header .dropDown').css({ 'max-height': ($(window).height() - $('header').height()) + 'px', 'overflow-y': 'scroll' });
    $('.aBack').click(function () { //返回上一页
        //location = document.referrer;
        history.go(-1);
    });
    $('.goTop').click(function () { // 返回顶部
        $(window).scrollTop(0);
    });
    if ($('#deposit[deposit]').length == 1) {
        if (GetCookie('username') == '') {
            $('#deposit').remove();
        } else if ($('#deposit').attr('deposit') == 0) {
            $('#deposit').find('a').attr('href', 'javascript:;').click(function () {
                alert('余额不足，请先充值。');
            });
        }
    }
    if (isWeiXin()) {
        $('#alipay,#waptenpay,#H5wxpaybox').remove();
    }
    if ($("#wxpaybox").length > 0 || $("#H5wxpaybox").length > 0) {
        var wxWapPay = false; //是否开通了微信Wap支付
        if (!isWeiXin() && !wxWapPay) {
            $("#wxpaybox").remove();
            //return;
        }
        /*if ($('#wxpaybox').is('[paypara]') == false) {
        return;
        }
        if ($('#wxpaybox').attr('autoclick') == 'wxpay') {
        $(window).unbind('load');
        }*/
        /*if (!isWeiXin()) {//如要开通wap支付权限，目前这个方式还未公开
        $.get(webpath + 'sys/ajax/weixin/getWeiXinPayPara.ashx?type=NATIVE&' + $('#wxpaybox').attr('paypara'), function (json) {
        if (typeof (json.err) != "undefined") {
        $('#wxpaybox').html(json.err);
        return;
        }
        var payurl = 'weixin://wap/pay?appid=' + json.appid + '&noncestr=' + json.noncestr + '&package=WAP&prepayid=' + json.prepayid + '&sign=' + json.sign + '&timestamp=' + json.timestamp;
        if ($('#wxpaybox').attr('autoclick') == 'wxpay') {
        location.href = payurl;
        } else {
        $('#wxpaybox').click(function () {
        location.href = payurl;
        });
        }
        });
        return;
        }*/
        if (!isWeiXin()) {//H5支付
            $.ajax({
                url: webpath + 'sys/ajax/weixin/getWeiXinPayPara.ashx?type=MWEB&' + $('#H5wxpaybox').attr('paypara'),
                cache: false,
                dataType: 'json',
                context: $(this),
                success: function (json) {
                    if (typeof (json.err) != "undefined") {
                        $('#H5wxpaybox').html(json.err);
                        return;
                    }
                    var payurl = json.mweb_url;
                    if ($('#H5wxpaybox').attr('autoclick') == 'wxpay') {
                        location.href = payurl;
                    } else {
                        $('#H5wxpaybox').click(function () {
                            location.href = payurl;
                        });
                    }
                }
            });
            return;
        }
        if ($('#PayParam_err').length == 1) {
            alert($('#PayParam_err').val()); return;
        }
        // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            if (typeof (WeixinJSBridge) == "undefined") {
                alert("WeixinJSBridge is undefined")
                $("#wxpaybox").hide();
            }
            //公众号支付
            $('#getBrandWCPayRequest').click(function (e) {
               WeixinJSBridge.invoke('getBrandWCPayRequest', {
                    "appId": $('#PayParam_appid').val(), //公众号名称，由商户传入
                    "timeStamp": $('#PayParam_timestamp').val(), //时间戳
                    "nonceStr": $('#PayParam_noncestr').val(), //随机串
                    "package": $('#PayParam_package').val(), //扩展包
                    "signType": "MD5", //微信签名方式:1.SHA1
                    "paySign": $('#PayParam_sign').val() //微信签名
                }, function (res) {
                    $('#status,#preloader').hide();
                    $('img[data-original]').lazyload();
                    if (res.err_msg == "get_brand_wcpay_request:ok") {
                        alert("支付成功");
                        window.location.href = webpath + $("#wapdir").val() + "member/";
                    } else {
                        if (res.err_msg == "get_brand_wcpay_request:cancel") {
                            //location = location;
                        } else {
                            if (res.err_desc.indexOf('跨号') != -1) {
                                $.get(webpath + 'sys/ajax/weixin/getWeiXinPayPara.ashx?type=NATIVE&' + $('#wxpaybox').attr('paypara'), function (json) {
                                    if (typeof (json.err) != "undefined") {
                                        $('#wxpaybox').html(json.err);
                                        return;
                                    }
                                    $('#wxpaybox').html('<img src="' + webpath + 'sys/qrCode.aspx?url=' + encodeURIComponent(json.payurl) + '"/>');
                                    alert('请长按，扫码支付。');
                                }, 'json');
                                return;
                            }
                            var str = '';
                            for (var x in res) {
                                str += x + ':' + res[x] + ',';
                            }
                            alert(str);
                        }
                    }
                    // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回ok，但并不保证它绝对可靠。
                    //因此微信团队建议，当收到ok返回时，向商户后台询问是否收到交易成功的通知，若收到通知，前端展示交易成功的界面；若此时未收到通知，商户后台主动调用查询订单接口，查询订单的当前状态，并反馈给前端展示相应的界面。
                });
            });
            if ($('#wxpaybox').attr('autoclick') == 'wxpay') {
                $('#getBrandWCPayRequest').click();
            }
        }, false);
    }
    bindFavorites();
    bindContact();
});
var ContactJson = null;
function bindContact() {
    $('body').on('click', '.selectContact', function () {
        if (ContactJson == null) {
            $.ajax({
                url: webpath + 'sys/ajax/user/Contact.ashx',
                cache: false,
                dataType: 'json',
                context: $(this),
                success: function (json) {
                    ContactJson = {};
                    ContactJson.item = json;

                    if (json.length == 0) {
                        $('body').append('<div class="certificateList" id="selectContact" style=" display:none;"><div class="hd">没有联系人</div></div>');
                        $('#selectContact').click(function () {
                            var index = layer.getIndex(this);
                            layer.close(index);
                        });
                    }
                    var tmp = '<div class="certificateList" id="selectContact" style=" display:none;"><div class="hd">请选择</div><ul>';
                    $.each(json, function (i, n) {
                        tmp += '<li contactid="' + n.UC_ID + '">' + n.UC_RealName + '</li>';
                    });
                    tmp += '</ul></div>';
                    $('body').append(tmp);
                    $('#selectContact li').click(function () {
                        var contactid = $(this).attr('contactid'), c;
                        $.each(ContactJson.item, function (i, n) {
                            if (n.UC_ID == contactid) c = n;
                        });
                        var n = ContactJson.obj.prev();
                        n.val(c.UC_RealName);
                        var id = n.attr('id');
                        var t = id.indexOf('_contact_') != -1 ? '_contact_' : '_name_';
                        if (c.UC_CardId != '') {
                            $('#' + id.replace(t, '_card_')).val(c.UC_CardId);
                            $('#' + id.replace(t, '_cardtype_')).val('身份证');
                            $('#' + id.replace(t, '_card_')).prev().text('身份证');
                        } else if (c.UC_Passport != '') {
                            $('#' + id.replace(t, '_card_')).val(c.UC_Passport);
                            $('#' + id.replace(t, '_cardtype_')).val('护照');
                            $('#' + id.replace(t, '_card_')).prev().text('护照');
                        }
                        var index = layer.getIndex(this);
                        layer.close(index);
                    });
                    $(this).click();
                }
            });
            return;
        }
        ContactJson.obj = $(this);
        $.layer({
            closeBtn: [0, false], //去掉默认关闭按钮
            type: 1,
            title: false, //不显示默认标题栏
            shade: [0.5, '#AAA', true],
            area: ['180px', '260px'],
            page: { dom: '#selectContact' }
        });
    });
    if (GetCookie('username') == '') {
        $('.selectContact').hide();
    }

}
function dropDown() { //模拟下拉
    $('.dropMenu input').click(function (event) { //下拉框下拉
        event.stopPropagation(); //取消事件冒泡
        $('.dropMenu dl').hide();
        $(this).next('dl').show();

        //下拉项是左右两栏的情况，不足两栏时候补足
        var num = $(this).next('dl.column').find('dd').length;
        if (num % 2 == 1) { $(this).next('dl').append('<dd></dd>'); }
    });

    $('.dropMenu dd[tag!="0"]').click(function () {//下拉框操作
        var text = $(this).text();
        var val = $(this).attr('val');
        $(this).parent().hide();
        $(this).parent().parent().find('input').val(text);
        $(this).parent().parent().find("input[type='hidden']").val(val);
    });

    $('.dropMenu dt[tag!="0"]').click(function () {//下拉框操作
        $(this).parent().hide();
        $(this).parent().parent().find('input').val('');
        $(this).parent().parent().find("input[type='hidden']").val('0');
    });

    $(document).click(function (event) { $('.dropMenu dl').hide(); });  //点击空白处或者自身隐藏下拉层
}

function dateDropDown() { //模拟下拉
    $('.dateDropMenu input').click(function (event) { //下拉框下拉
        event.stopPropagation(); //取消事件冒泡
        $('.dateDMenu dl').hide();
        $(this).next('dl').show();

        //下拉项是左右两栏的情况，不足两栏时候补足
        var num = $(this).next('dl.column').find('dd').length;
        if (num % 2 == 1) { $(this).next('dl').append('<dd></dd>'); }
    });

    $('.dateDropMenu dd[tag!="0"]').click(function () {//下拉框操作
        var text = $(this).text();
        var val = $(this).attr('val');
        $(this).parent().hide();
        $(this).parent().parent().find('input').val(text);
        $(this).parent().parent().find("input[type='hidden']").val(val);
    });

    $('.dateDropMenu dt').click(function () {//下拉框操作
        $(this).parent().hide();
        $(this).parent().parent().find('input').val('');
        $(this).parent().parent().find("input[type='hidden']").val('0');
    });
}

function bindScroll() {
    $(window).unbind('scroll').bind("scroll", function () {
        var winHeight = $(window).height();
        var scrollTop = $(document).scrollTop();
        var bodyHeight = document.body.scrollHeight;
        var bottom = $('footer').outerHeight();
        if (scrollTop + winHeight > bodyHeight - bottom) {//load data
            $(window).unbind("scroll");
            getData();
        }
    });
}

function stopPropagation(e) {//防止Tab切换冒泡
    if (e.stopPropagation) {
        e.stopPropagation();
    } else {
        e.cancelBubble = true;
    }
}

function calReviewScore(score, total, target) {
    return score == 0 ? 5 : Math.round(parseFloat(score * target / total), 2);
}

function bindFavorites() {
    var favoritesObj = $('[favorites]');
    if (favoritesObj.length == 0) return;
    var Modules = {};
    favoritesObj.each(function () {
        var val = $(this).attr('favorites').split(',');
        if (typeof (Modules[val[0]]) === 'undefined') {
            Modules[val[0]] = val[1];
        } else {
            Modules[val[0]] += ',' + val[1];
        }
    });
    favoritesObj.unbind("click").click(function () {
        if ($(this).attr('status') == 1) {
            if ($(this).attr('favorites').indexOf('travelstj') != -1) {
                alert('您已推荐,无需重复推荐.');
            } else {
                alert('您已收藏,无需重复收藏.');
            }
            return;
        }
        var val = $(this).attr('favorites').split(',');
        var act = '';
        var mid = 0;
        if (val[0] == 'travelstj') {
            act = 'tj';
            val[0] = 'travels';
        } else if (val[0].indexOf('city') != -1) {
            act = val[0].replace('city', '');
            val[0] = 'city';
        } else if (val[0].indexOf('generic_') != -1) {
            mid = val[0].replace('generic_', '');
            val[0] = 'generic';
        }
        if (GetCookie('username') == '' && act != 'tj') {
            alert('请先登录!');
            return;
        }
        var url;
        if (val[0] == 'city') {
            url = webpath + 'sys/ajax/cityFavorites.ashx?id=' + val[1] + '&act=' + act;
        } else {
            url = webpath + 'sys/ajax/' + val[0] + '/Favorites.ashx?id=' + val[1] + '&act=' + act + '&mid=' + mid;
        }
        $.ajax({ url: url,
            cache: false, context: this,
            success: function (data) {
                $(this).attr('status', 1);
                var obji = $(this).find('.fa');
                if (obji.length == 0) {
                    $(this).addClass("on");
                } else {
                    var c = 'heart';
                    if (obji.hasClass('fa-star-o') || obji.hasClass('fa-star')) c = 'star';
                    obji.removeClass('fa-' + c + '-o').addClass('fa-' + c);
                }
            }
        });
    });

    for (var key in Modules) {
        var m = key;
        var act = 'get';
        var mid = 0;
        if (m == 'travelstj') {
            act = 'gettj';
            m = 'travels';
        } else if (key.indexOf('city') != -1) {
            act = 'get' + key.replace('city', '');
            m = 'city';
        } else if (key.indexOf('generic_') != -1) {
            mid = key.replace('generic_', '');
            m = 'generic';
        }
        var url;
        if (m == 'city') {
            url = webpath + 'sys/ajax/cityFavorites.ashx?act=' + act + '&id=' + Modules[key] + '&callback=?';
        } else {
            url = webpath + 'sys/ajax/' + m + '/Favorites.ashx?act=' + act + '&id=' + Modules[key] + '&mid=' + mid + '&callback=?';
        }
        $.getJSON(url,
                function (json) {
                    for (var i = 0; i < json.length; i++) {
                        var t = json[i];
                        var obj = favoritesObj.filter('[favorites="' + t.type + ',' + t.adid + '"]');
                        obj.attr('status', t.status);
                        var obji = obj.find('.fa');
                        if (obji.length == 0) {
                            if (t.status == 1) {
                                obj.addClass("on")
                            } else {
                                obj.removeClass("on")
                            }
                        } else {
                            var c = 'heart';
                            if (obji.hasClass('fa-star-o') || obji.hasClass('fa-star')) c = 'star';
                            if (t.status == 1) {
                                obj.find('.fa').removeClass('fa-' + c + '-o').addClass('fa-' + c);
                            } else {
                                obj.find('.fa').removeClass('fa-' + c).addClass('fa-' + c + '-o');
                            }
                        }
                    }
                }
            );
    }
}