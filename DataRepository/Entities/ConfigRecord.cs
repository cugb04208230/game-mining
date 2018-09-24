using System;
using BaseClasses.Data;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 配置记录
	/// </summary>
	[Serializable]
	public class ConfigRecord : BaseEntity
	{
		/// <summary>
		/// 目录
		/// </summary>
		public string Catalog { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 显示名称
		/// </summary>
		public string FriendlyName { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 类型码,用于输入控制
		/// </summary>
		public string ValueTypeCode { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public string Value { get; set; }

	}


	public class ConfigRecordQuery : BaseQuery<ConfigRecord>
	{
		/// <summary>
		/// 目录
		/// </summary>
		public string Catalog { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 显示名称
		/// </summary>
		public string FriendlyName { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 类型码,用于输入控制
		/// </summary>
		public string ValueTypeCode { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public string Value { get; set; }
	}

}
