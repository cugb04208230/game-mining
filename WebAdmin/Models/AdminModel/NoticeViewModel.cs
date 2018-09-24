using System.ComponentModel.DataAnnotations;
using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{
	/// <summary>
	/// 系统通知页
	/// </summary>
	public class NoticeViewModel
	{
		/// <summary>
		/// 查询关键字
		/// </summary>
		public string Text { set; get; }

		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 通知列表
		/// </summary>
		public QueryResult<Notice> Notices { set; get; }
	}

	/// <summary>
	/// 获取系统通知
	/// </summary>
	public class NoticePostModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		public long? Id { set; get; }

		/// <summary>
		/// 标题
		/// </summary>
		[Required(ErrorMessage = "标题必须填写", AllowEmptyStrings = false)]
		public string Title { set; get; }

		/// <summary>
		/// 内容
		/// </summary>
		[Required(ErrorMessage = "内容必须填写", AllowEmptyStrings = false)]
		public string Content { set; get; }
	}
}