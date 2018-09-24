using System;

namespace BaseClasses.Attributes
{
	/// <inheritdoc />
	/// <summary>
	/// 配置类型属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ConfigTypeCodeAttribute : Attribute
	{
		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="typeCode"></param>
		public ConfigTypeCodeAttribute(string typeCode)
		{
			TypeCode = typeCode;
		}

		/// <summary>
		/// 
		/// </summary>
		public string TypeCode { get; }

		/// <summary>
		/// 
		/// </summary>
		public Type OptionType { get; set; }
	}
}
