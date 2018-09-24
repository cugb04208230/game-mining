using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using BaseClasses;
using BaseClasses.Util;
using DataRepository.Entity;

namespace Bussiness
{
	public class SmsManager:BaseManager
	{
		public SmsManager(MiddleTier middleTier) : base(middleTier)
		{
		}


		public void SendSms(string destMobile, string code)
		{

			String product = "Dysmsapi"; //短信API产品名称
			String domain = "dysmsapi.aliyuncs.com"; //短信API产品域名
			String accessKeyId = MiddleTier.BusinessConfig.SmsAccessKeyId;
			String accessKeySecret = MiddleTier.BusinessConfig.SmsAccessKeySecret; //你的accessKeySecret
			IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
			DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
			IAcsClient acsClient = new DefaultAcsClient(profile);
			SendSmsRequest request = new SendSmsRequest();
			try
			{
				//必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
				request.PhoneNumbers = destMobile;
				//必填:短信签名-可在短信控制台中找到
				request.SignName = MiddleTier.BusinessConfig.SmsSignName;
				//必填:短信模板-可在短信控制台中找到
				request.TemplateCode = MiddleTier.BusinessConfig.SmsTemplateCode;
				//可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
				request.TemplateParam = JsonUtilities.Serialize(new {code = code});
				//可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
				request.OutId = "0";
				//请求失败这里会抛ClientException异常
				SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
			}
			catch (Exception e)
			{
				MiddleTier.LogManager.Error(e);
			}
		}


		/// <summary>
		/// 验证验证码是否正确
		/// </summary>
		/// <param name="destMobile"></param>
		/// <param name="verifyCode"></param>
		/// <param name="codeType"></param>
		/// <param name="isIngoreUsed"></param>
		/// <returns></returns>
		public bool CheckVerifyCode(string destMobile, string verifyCode, int codeType = 0, bool isIngoreUsed = true)
		{
			SmsVerifyCode smsVerifyCode = DataBase.Get<SmsVerifyCode>(e => e.Mobile == destMobile && e.Code == verifyCode && e.CodeType == codeType);
			if (smsVerifyCode == null)
				return false;
			if (!isIngoreUsed && smsVerifyCode.IsUsed)
				return false;
			bool isTimeOut = IsVerifyCodeTimeOut(smsVerifyCode);
			if (isTimeOut)
				return false;
			smsVerifyCode.IsUsed = true;
			DataBase.Update(smsVerifyCode);
			return true;

		}

		/// <summary>
		/// 判断是否能够发送验证码，默认间隔60秒发一次
		/// </summary>
		/// <param name="smsVerifyCode"></param>
		/// <returns></returns>
		private bool IsSendTooFast(SmsVerifyCode smsVerifyCode)
		{
			if (smsVerifyCode == null) return false;
			long now = DateUtility.ConvertDateTimeLong(DateTime.UtcNow);
			if (now < smsVerifyCode.NextSendTime)
				return true;
			return false;
		}


		/// <summary>
		/// 判断验证码是否超时
		/// </summary>
		/// <param name="smsVerifyCode"></param>
		/// <returns></returns>
		private bool IsVerifyCodeTimeOut(SmsVerifyCode smsVerifyCode)
		{
			if (smsVerifyCode == null) return false;
			long now = DateUtility.ConvertDateTimeLong(DateTime.UtcNow);
			if (now > smsVerifyCode.TimeOut)
				return true;
			return false;
		}

		/// <summary>
		/// 判断发送的验证码是否达到上限（每天）
		/// </summary>
		/// <param name="smsVerifyCode"></param>
		/// <returns></returns>
		private bool IsSendVerifyCodeLimited(SmsVerifyCode smsVerifyCode)
		{
			if (smsVerifyCode == null) return false;
			DateTime now = DateTime.Now.Date;
			DateTime sendDate = DateTime.Parse(smsVerifyCode.CurrentDate);
			if (now > sendDate)
				return false;
			if (now == sendDate)
			{
				if (smsVerifyCode.SendAmount >= MiddleTier.BusinessConfig.VerifyCodeLimited)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destMobile"></param>
		/// <param name="codeType">0-注册验证码</param>
		/// <returns>0-发送成功 1-1分钟内不能重复发送 2-今日发送达到上限 3-发送失败</returns>
		public int SendVerifyCode(string destMobile, int codeType)
		{
			string code = RandomUtil.GenerateVerfiyCode();
			SmsVerifyCode smsVerifyCode = DataBase.Get<SmsVerifyCode>(e => e.Mobile == destMobile && e.CodeType == codeType);
			if (smsVerifyCode != null)
			{
				//判断有没有连续发送
				bool isSendTooFast = IsSendTooFast(smsVerifyCode);
				if (isSendTooFast)
					return 1;
				//判断今天是否已经达到发送上限
				bool isOverLimited = IsSendVerifyCodeLimited(smsVerifyCode);
				if (isOverLimited)
					return 2;
				string nowCurrentData = DateTime.Now.ToString("yyyy-MM-dd");
				if (nowCurrentData == smsVerifyCode.CurrentDate)
				{
					smsVerifyCode.SendAmount += 1;
				}
				else
				{
					smsVerifyCode.SendAmount = 1;
					smsVerifyCode.CurrentDate = nowCurrentData;
				}
				smsVerifyCode.Code = code;
				smsVerifyCode.IsUsed = false;
				smsVerifyCode.UpdateDate = DateTime.Now;
				smsVerifyCode.TimeOut = DateUtility.ConvertDateTimeLong(DateTime.UtcNow.AddSeconds(MiddleTier.BusinessConfig.VerifyCodeTimeOut));
				smsVerifyCode.NextSendTime = DateUtility.ConvertDateTimeLong(DateTime.UtcNow.AddSeconds(MiddleTier.BusinessConfig.VerifyCodeSendInterval));
				DataBase.Update<SmsVerifyCode>(smsVerifyCode);
			}
			else
			{
				smsVerifyCode = new SmsVerifyCode();
				smsVerifyCode.Code = code;
				smsVerifyCode.Mobile = destMobile;
				smsVerifyCode.CodeType = codeType;
				smsVerifyCode.CreateDate = DateTime.Now;
				smsVerifyCode.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
				smsVerifyCode.IsUsed = false;
				smsVerifyCode.SendAmount = 1;
				smsVerifyCode.TimeOut = DateUtility.ConvertDateTimeLong(DateTime.UtcNow.AddSeconds(MiddleTier.BusinessConfig.VerifyCodeTimeOut));
				smsVerifyCode.NextSendTime = DateUtility.ConvertDateTimeLong(DateTime.UtcNow.AddSeconds(MiddleTier.BusinessConfig.VerifyCodeSendInterval));
				smsVerifyCode.UpdateDate = DateTime.Now;
				DataBase.Save<SmsVerifyCode>(smsVerifyCode);
			}
			//提交到短信网关
			try
			{
				SendSms(destMobile, code);
			}
			catch
			{
				return 3;
			}
			return 0;
		}
	}
}
