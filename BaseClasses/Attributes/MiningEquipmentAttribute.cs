using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses.Attributes
{
	public class MiningEquipmentAttribute:Attribute
	{
		public MiningEquipmentAttribute(double price, int expirationDay, double percentage)
		{
			Price = decimal.Parse(price.ToString(CultureInfo.InvariantCulture));
			ExpirationDay = expirationDay;
			Percentage = decimal.Parse(percentage.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// 价格
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// 产品有效期
		/// </summary>
		public int ExpirationDay { set; get; }

		/// <summary>
		/// 收益百分比
		/// </summary>
		public decimal Percentage { set; get; }
	}
}
