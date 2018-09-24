using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseClasses.Extensions;
using DataRepository;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 收益记录查询
	/// </summary>
	public class MemberIncomeListViewModel
	{
		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class MemberIncomeListItemModel
	{
		/// <summary>
		/// 收益类型枚举
		/// </summary>
		public MemberIncomeType Type { set; get; }

		/// <summary>
		/// 收益类型描述
		/// </summary>
		public string TypeDesc
		{
			get
			{
				var language = LanguageHelper.GetLanguage();
				return language == LanguageType.English ? Type.ToString() : Type.GetDescription();
			}
		}

		/// <summary>
		/// 时间
		/// </summary>
		public DateTime CreatedAt { set; get; }

		/// <summary>
		/// 详情
		/// </summary>
		public string Content { set; get; }
	}
}