using System.ComponentModel;

namespace BaseClasses
{
	public enum ErrorCode
	{
		[Description("服务器打了个盹~")] SystemError = 1000,
		[Description("授权信息失败，请重新登录")] AuthFailed = 1001,
		[Description("无效的用户名或者密码")] ErrorUserNameOrPassword = 1002,
		[Description("该用户名已存在")] UserNameIsExisted = 1003,
		[Description("邀请用户信息错误")] InviterError = 1004,
		[Description("账户余额不足")] AccountBalanceNotEnough = 1005,
		[Description("该用户名不存在")] UserNameIsNotExisted = 1006,
		[Description("账号和姓名不匹配")] UserNameIsNotMatchName = 1007,
		[Description("交易单类型不合法")] TransferBillTypeError = 1008,
		[Description("参数名不合法")] PropertyNameInvalid = 1009,
		[Description("邀请人账户余额不足")] InviterAccountBalanceNotEnough = 1010,
		[Description("错误的编号")] ErrorId = 1011,
		[Description("交易单状态错误")] TransferBillStatusError = 1012,
		[Description("交易单买家信息不正确")] TransferBillFromMemberError = 1013,
		[Description("交易单卖家信息不正确")] TransferBillToMemberError = 1014,
		[Description("该设备还不能收取")] MiningEquipmentCollectError = 1015,
		[Description("一天只能精炼一次")] RefiningTooBusy = 1016,
		[Description("错误的值类型")] ErrorValueType = 1017,
		[Description("设备数量达到上限")] EquipmentCountLimit = 1018,
		[Description("邀请关系有误")] ErrorInvitRelationship = 1019,
		[Description("每日求购次数达到上限")] ToBuyTimeLimit = 1020,
		[Description("该状态不能够取消")] ErrorCancelStatus = 1021,
		[Description("赠送次数达到上限")] GiveTimeLimit = 1022,
		[Description("业务逻辑错误")] LogicError = 1023,
		[Description("周日哦，休息中")] SundayToRest = 1024,
		[Description("营业时间为每日6：00-23：00")] BusinessHoursError = 1025,
		[Description("账号已冻结，请联系管理人员")] AccountIsFreezing = 1026,
		[Description("账号已锁定，请联系管理人员")] AccountIsLocking = 1027,
		[Description("每日站内信数量达到上限")] DailyMessageboardCountLimit = 1028,
		[Description("账号已查封，请联系管理人员")] AccountIsSealUp = 1029,
		[Description("账号还未激活，暂时无法登陆")] AccountIsUnActived = 1030,
		[Description("用户已激活")] UserIsActived = 1031,
		[Description("危险的提交内容")] DangerousContent = 1032,
		[Description("手机号已存在")] MobileIsExisted = 1033,
		[Description("必须购买完采矿设备才可以发起求购")] MustBuyMiningEquipment = 1034,
		[Description("当前求购次数达到上限")] CurrentToBuyTimeLimit = 1035,
		[Description("验证码错误")] VerifyCodeError = 1036,
		[Description("确认密码错误")] ConfirmCodeError = 1037,
		[Description("")] MobileRegexError=1307,
		[Description("求购金额限制错误")] ToBuyAmountLimitError = 1308
	}
}
