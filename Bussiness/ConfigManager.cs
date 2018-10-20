using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BaseClasses;
using BaseClasses.Attributes;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using Common.Util;
using DataRepository.Entities;

namespace Bussiness
{
	public class ConfigManager : BaseManager
	{
		public ConfigManager(MiddleTier middleTier) : base(middleTier)
		{
		}

		/// <summary>
		/// 配置记录查询
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public QueryResult<ConfigRecord> Query(ConfigRecordQuery query)
		{
			return DataBase.Query(query);
		}

		/// <summary>
		/// 获取记录详情
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ConfigRecord GetById(long id)
		{
			return DataBase.Get<ConfigRecord>(id);
		}


		/// <summary>
		/// 读取配置
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Read<T>() where T : new()
		{
			return (T) Read(typeof(T));
		}


		private object Read(Type type)
		{
			var configType = type;
			var records = Query(new ConfigRecordQuery {Catalog = configType.FullName,PageSize = int.MaxValue});
			var instance = CreateDefault(type);
			var properties = configType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public |
			                                          BindingFlags.SetProperty);
			foreach (var record in records.List)
			{
				var property = properties.FirstOrDefault(p => p.Name == record.Name);
				if (property == null) continue;
				try
				{
					if (property.PropertyType.IsEnum)
					{
						int.TryParse(record.Value, out var real);
						property.SetValue(instance, real);
					}
					else
					{
						var value = Convert.ChangeType(record.Value, property.PropertyType);
						property.SetValue(instance, value);
					}
				}
				catch (Exception)
				{
					//Ignore;
				}
			}
			return instance;
		}

		/// <summary>
		/// 更新配置
		/// </summary>
		/// <param name="record"></param>
		public void Update(ConfigRecord record)
		{
			DataBase.Update(record);
		}

		public void Update(long id, string value)
		{
			var configRecord = DataBase.Get<ConfigRecord>(id);
			if (configRecord == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			var businessConfig = new BusinessConfig();
			businessConfig.SetPropertyValue(configRecord.Name, value);
			configRecord.Value = value;
			DataBase.Update(configRecord);
			MiddleTier.BusinessConfig = null;
		}

		private object CreateDefault(Type type)
		{
			var instance = Activator.CreateInstance(type);
			var properties =
				type.GetProperties(
					BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
			foreach (var property in properties)
			{
				var defaultAttr = property.GetCustomAttribute<DefaultValueAttribute>();
				if (defaultAttr != null)
				{
					try
					{
						property.SetValue(instance, defaultAttr.Value);
					}
					catch (Exception)
					{
						// ignored
					}
				}
				else
				{
					if (property.PropertyType == typeof(string))
					{
						property.SetValue(instance, "");
					}
				}
			}
			return instance;
		}

		/// <summary>
		/// 初始化配置
		/// </summary>
		public void Synchronize()
		{
			var configTypes = TypeFinder.SetScope(AppDomain.CurrentDomain.GetAssemblies())
				.Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && t.Name.EndsWith("BusinessConfig"));
			foreach (var type in configTypes)
			{
				Synchronize(type);
			}
		}

		private void Synchronize(Type type)
		{
			var properties = type
				.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
				.Where(p => p.CanRead && p.CanWrite);
			var now = DateTime.Now;
			var notExistNames = properties.Select(e => e.Name).ToList();
			var notExists = DataBase.GetList<ConfigRecord>(e => !notExistNames.Contains(e.Name));
			notExists.ForEach(item => DataBase.Delete(item));
			foreach (var property in properties)
			{
				var exist = Query(new ConfigRecordQuery()
				{
					Catalog = type.FullName,
					Name = property.Name,
					PageIndex = int.MaxValue
				});
				if (exist.Count > 0)
				{
				}
				else
				{
					var record = new ConfigRecord()
					{
						Catalog = type.FullName,
						Name = property.Name,
						CreatedAt = now,
						Description = property.Name,
						FriendlyName = property.Name,
						LastModifiedAt = now,
						Value = "",
					};
					var defaultAttr = property.GetCustomAttribute<DefaultValueAttribute>();
					if (defaultAttr != null)
					{
						record.Value = defaultAttr.Value.ToString();
					}
					else
					{
						switch (Type.GetTypeCode(type))
						{
							case TypeCode.DateTime:
								record.Value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
								break;
							case TypeCode.Boolean:
								record.Value = "true";
								break;
							case TypeCode.Decimal:
							case TypeCode.Int32:
							case TypeCode.Int64:
								record.Value = "0";
								break;
							case TypeCode.String:
								record.Value = string.Empty;
								break;
							default:
								record.Value = string.Empty;
								break;
						}
					}
					var description = property.GetCustomAttribute<DescriptionAttribute>();
					if (description != null)
					{
						record.FriendlyName = description.Description;
						record.Description = description.Description;
					}
					var configTypeCode = property.GetCustomAttribute<ConfigTypeCodeAttribute>();
					if (configTypeCode != null)
					{
						record.ValueTypeCode = configTypeCode.TypeCode;
					}
					else
					{
						switch (Type.GetTypeCode(type))
						{
							case TypeCode.DateTime:
								record.ValueTypeCode = ConfigTypes.DateTime;
								break;
							case TypeCode.Boolean:
								record.ValueTypeCode = ConfigTypes.Text;
								break;
							case TypeCode.Decimal:
								record.ValueTypeCode = ConfigTypes.Number;
								break;
							case TypeCode.Int32:
								record.ValueTypeCode = ConfigTypes.Integer;
								break;
							case TypeCode.String:
								record.ValueTypeCode = ConfigTypes.Text;
								break;
							default:
								record.ValueTypeCode = ConfigTypes.Text;
								break;
						}
					}
					DataBase.Save(record);
				}
			}
		}
	}

	/// <summary>
	/// 应用配置
	/// </summary>
	public class BusinessConfig
	{
//		/// <summary>
//		/// 收益奖励传递次数
//		/// </summary>
//		[DefaultValue(3)]
//		[Description("收益奖励传递次数")]
//		[ConfigTypeCode(ConfigTypes.Integer)]
//		public int ProfitTransmitTime { set; get; }
//
//		/// <summary>
//		/// 收益传递百分比
//		/// </summary>
//		[DefaultValue(3)]
//		[Description("收益传递百分比")]
//		[ConfigTypeCode(ConfigTypes.Percentage)]
//		public decimal ProfitTransmitPercentage { set; get; }
//
//		/// <summary>
//		/// 购买矿机奖励传递次数
//		/// </summary>
//		[DefaultValue(1)]
//		[Description("购买矿机奖励传递次数")]
//		[ConfigTypeCode(ConfigTypes.Integer)]
//		public int EquipmentBuyRewardTransmitTime { set; get; }
//

//		/// <summary>
//		/// 购买矿机奖励百分比
//		/// </summary>
//		[DefaultValue(10)]
//		[Description("购买矿机奖励百分比")]
//		[ConfigTypeCode(ConfigTypes.Percentage)]
//		public decimal EquipmentBuyRewardPercentage { set; get; }
		
		[DefaultValue("admin")]
		[Description("管理后台账号")]
		[ConfigTypeCode(ConfigTypes.Text)]
		public string BackEndUserName { set; get; }

		[DefaultValue("admin")]
		[Description("管理后台密码")]
		[ConfigTypeCode(ConfigTypes.Text)]
		public string BackEndPassword { set; get; }

		[DefaultValue("http://111.231.77.96:8002")]
		[Description("前端应用地址")]
		[ConfigTypeCode(ConfigTypes.Text)]
		public string WebAppBaseUrl { set; get; }


		[DefaultValue("http://111.231.77.96:8001")]
		[Description("管理后台地址")]
		[ConfigTypeCode(ConfigTypes.Text)]
		public string WebAdminBaseUrl { set; get; }

		[DefaultValue(0)]
		[Description("在线用户人数")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int OnLineMemberCount { set; get; }

		[DefaultValue(0)]
		[Description("排队人数")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int QueuesMemberCount { set; get; }

		[DefaultValue(100)]
		[Description("激活合伙人所需金额")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal ActivePartnerNeedAmount { set; get; }

		[DefaultValue(10)]
		[Description("每个人的精炼设备上限")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal SlagRefiningEquipmentCountLimit { set; get; }
		
		[DefaultValue(10)]
		[Description("赠送精炼设备所需推荐人数")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal SlagRefiningEquipmentRewardInviteMemberCount { set; get; }
		
		[DefaultValue(0.5)]
		[Description("精炼设备前两个精炼百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal SlagRefiningEquipmentTopPercentage { set; get; }


		[DefaultValue(2)]
		[Description("高收益精炼设备的数量")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public int SlagRefiningEquipmentTopCount { set; get; }


		[DefaultValue(0.3)]
		[Description("精炼设备精炼百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal SlagRefiningEquipmentNormalPercentage { set; get; }


		[DefaultValue(1)]
		[Description("精炼消耗矿渣和金的比例")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal RefiningConsumeRatio { set; get; }

		[DefaultValue(365)]
		[Description("精炼设备使用有效天数")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int SlagRefiningEquipmentUseTermDay { set; get; }

		[DefaultValue(5)]
		[Description("采矿设备采集周期")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int MiningEquipmentCollectDay { set; get; }
		
//		[DefaultValue(2)]
//		[Description("采矿设备冻结所需的逾期次数")]
//		[ConfigTypeCode(ConfigTypes.Integer)]
//		public int MiningEquipmentCollectOverDueTime { set; get; }
//
//		[DefaultValue(10)]
//		[Description("采矿设备锁定周期")]
//		[ConfigTypeCode(ConfigTypes.Integer)]
//		public int MiningEquipmentOverDueLockDay { set; get; }

		[DefaultValue(60)]
		[Description("采集结算金的百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal CollectGoldPercentage { set; get; }

		[DefaultValue(20)]
		[Description("采集结算银的百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal CollectSilverPercentage { set; get; }

		[DefaultValue(10)]
		[Description("采集结算铜的百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal CollectCopperPercentage { set; get; }

		[DefaultValue(10)]
		[Description("采集结算矿渣的百分比")]
		[ConfigTypeCode(ConfigTypes.Percentage)]
		public decimal CollectSlagPercentage { set; get; }

		[DefaultValue(10)]
		[Description("用户采矿工数量上限")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int MemberMiningEquipmentType1Limit { set; get; }


		[DefaultValue(10)]
		[Description("用户选金厂数量上限")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int MemberMiningEquipmentType2Limit { set; get; }
		[DefaultValue(10)]
		[Description("用户冶炼厂数量上限")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int MemberMiningEquipmentType3Limit { set; get; }
		/// <summary>
		/// 全球金价
		/// </summary>
		[DefaultValue(10)]
		[Description("全球金价")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public decimal GlobalGoldPrice { set; get; }
		/// <summary>
		/// 激活用户奖励推荐人钻石数量
		/// </summary>
		[DefaultValue(1)]
		[Description("激活用户奖励推荐人钻石数量")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal ActivePartnerRewardDiamondAmount { set; get; }
		/// <summary>
		/// 合伙人购买矿工奖励推荐人钻石数量
		/// </summary>
		[DefaultValue(1)]
		[Description("合伙人购买矿工奖励钻石数量")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal PartnerBuyEquipmentType1RewardDiamondAmount { set; get; }
		/// <summary>
		/// 合伙人购买选金厂奖励推荐人钻石数量
		/// </summary>
		[DefaultValue(5)]
		[Description("合伙人购买选金厂奖励钻石数量")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal PartnerBuyEquipmentType2RewardDiamondAmount { set; get; }
		/// <summary>
		/// 合伙人购买冶炼厂奖励推荐人钻石数量
		/// </summary>
		[DefaultValue(10)]
		[Description("合伙人购买冶炼厂奖励钻石数量")]
		[ConfigTypeCode(ConfigTypes.Number)]
		public decimal PartnerBuyEquipmentType3RewardDiamondAmount { set; get; }


		/// <summary>
		/// 求购有效时间（分钟）
		/// </summary>
		[DefaultValue(300)]
		[Description("求购有效时间（分钟）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int ToBuyExpiredTime { set; get; }


		/// <summary>
		/// 求购有效次数限制
		/// </summary>
		[DefaultValue(2)]
		[Description("求购有效次数限制）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int ToBuyLimitTime { set; get; }


		/// <summary>
		/// 求购有效次数限制
		/// </summary>
		[DefaultValue(10)]
		[Description("报单中心求购有效次数限制）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int CallCenterToBuyLimitTime { set; get; }



		/// <summary>
		/// 普通用户同时求购笔数限制
		/// </summary>
		[DefaultValue(1)]
		[Description("普通用户同时求购笔数限制）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int CurrentNormalToBuyLimitTime { set; get; }
		/// <summary>
		/// 求购中心同时求购笔数限制
		/// </summary>
		[DefaultValue(3)]
		[Description("求购中心同时求购笔数限制）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int CurrentCallToBuyLimitTime { set; get; }


		//		/// <summary>
		//		/// 赠送有效次数限制
		//		/// </summary>
		//		[DefaultValue(2)]
		//		[Description("赠送有效次数限制）")]
		//		[ConfigTypeCode(ConfigTypes.Integer)]
		//		public int GiveLimitTime { set; get; }
		//

		/// <summary>
		/// 每日站内信数量
		/// </summary>
		[DefaultValue(2)]
		[Description("每日站内信数量）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int DailyMessageboardCountLimit { set; get; }

		/// <summary>
		/// 转账信誉值
		/// </summary>
		[DefaultValue(5)]
		[Description("转账信誉值）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int TransferHonor { set; get; }


		/// <summary>
		/// 转账赠送者的信誉值
		/// </summary>
		[DefaultValue(5)]
		[Description("转账赠送者的信誉值）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int TransferGiveHonor { set; get; }

		/// <summary>
		/// 求购金额上限
		/// </summary>
		[DefaultValue(1000)]
		[Description("求购金额上限）")]
		[ConfigTypeCode(ConfigTypes.Integer)]
		public int ToBuyAmountLimit { set; get; }
	}

	/// <summary>
	/// 配置参数类型
	/// </summary>
	public static class ConfigTypes
	{
		[Description("选项")] public const string Option = "option";
		[Description("整数")] public const string Integer = "integer";
		[Description("数字")] public const string Number = "number";
		[Description("文本")] public const string Text = "text";
		[Description("多行文本")] public const string MultilineText = "multiline_text";
		[Description("百分比")] public const string Percentage = "percentage";
		[Description("时间")] public const string DateTime = "datetime";
		[Description("日期")] public const string Date = "date";
		[Description("图片")] public const string Image = "image";
	}
}