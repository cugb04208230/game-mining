using System.Collections.Generic;

namespace WebAdmin.Models
{
	/// <summary>
	/// 菜单栏模型
	/// </summary>
	public class LeftMenuModel
	{
		/// <summary>
		/// 说明
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// 地址
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 子菜单
		/// </summary>
		public List<LeftMenuModel> Children { set; get; }
	}

}