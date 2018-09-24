(function(w, $) {
	w.urldecode = function(str) {
		return decodeURIComponent((str + '').replace(/\+/g, '%20'));
	};
	w.$rpc = function(path, data, success, failure, error, always) {
		if (path.charAt(0) == '/') path = path.substring(1);
		var it = setTimeout(function() {
				req.abort();
			},
			30000);
		var req = $.ajax({
			url: "/" + path,
			data: data,
			type: "POST",
			dataType: "json",
			success: function(data) {
				clearTimeout(it);
				if (data.Success == true) {
					if ($.isFunction(success)) success(data.Data);
				} else {
					if ($.isFunction(failure)) failure(data);
				}
				if ($.isFunction(always)) always({ data: data });
			},
			error: function(request, status, e) {
				clearTimeout(it);
				if ($.isFunction(error)) {
					error(request.responseJSON);
				}
				if ($.isFunction(always)) always({ request: request, status: status, error: e });
			}
		});
	};
	w.util = {
		stringFormat: function() {
			var args = arguments;
			if (!args.length) return "";
			if (typeof args[0] != "string") return "";
			return args[0].replace(/{\d+}/g,
				function(match, number) {
					var mi = parseInt(match.substr(1, match.length - 2));
					return typeof args[mi + 1] != 'undefined'
						? args[mi + 1]
						: match;
				});
		},
		stringFormatCommon: function() {
			var args = arguments;
			if (!args.length) return "";
			if (typeof args[0] != "string") return "";
			if ($.isArray(args[1])) {
				return args[0].replace(/{\d+}/g,
					function(match, number) {
						var mi = parseInt(match.substr(1, match.length - 2));
						return typeof args[1][mi] != 'undefined'
							? args[mi]
							: match;
					});
			}
			if ($.isPlainObject(args[1])) {
				var argumentObj = args[1];
				return args[0].replace(/{[a-zA-Z0-9_.]+}/g,
					function(match, number) {
						var mi = match.substr(1, match.length - 2);
						var attrArray = $.trim(mi).split('.');
						var tmp = argumentObj;
						_.each(attrArray,
							function(item, index) {
								tmp = tmp[item];
							});
						return typeof tmp != 'undefined' ? tmp : match;
					});
			}
			return args[0].replace(/{\d+}/g,
				function(match, number) {
					var mi = parseInt(match.substr(1, match.length - 2));
					return typeof args[mi + 1] != 'undefined'
						? args[mi + 1]
						: match;
				});
		},

		getRelativePath: function() {
			var url = document.location.toString();
			var arrUrl = url.split("//");

			var start = arrUrl[1].indexOf("/");
			var relUrl = arrUrl[1].substring(start); //stop省略，截取从start开始到结尾的所有字符

			if (relUrl.indexOf("?") != -1) {
				relUrl = relUrl.split("?")[0];
			}
			return relUrl;
		},

		setCookie: function(name, value, expiredays) {
			if (expiredays == undefined) {
				expiredays = 30;
			}
			var exp = new Date();
			exp.setTime(exp.getTime() + expiredays * 24 * 60 * 60 * 1000);
			document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
		},

		getCookie: function(name) {
			var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
			if (arr = document.cookie.match(reg)) {
				return unescape(arr[2]);
			} else {
				return null;
			}
		},

		removeCookie: function(name) {
			var exp = new Date();
			exp.setTime(exp.getTime() - 1);
			var cval = util.getCookie(name);
			if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
		}
	};
	w.QueryStr = function(options) {
		var defaults = {
			name: "",
			defaults: ""
		};
		options = $.extend(defaults, options);
		var result = location.search.match(new RegExp("[\?\&]" + options.name + "=([^\&]+)", "i"));
		if (result == null || result.length < 1) {
			return options.default;
		}
		return result[1];
	}
	w.QueryPath = function(name) {
		var defaults = {
			name: name,
			defaults: "/"
		};
		return QueryStr(defaults);
	}
})(window, jQuery);