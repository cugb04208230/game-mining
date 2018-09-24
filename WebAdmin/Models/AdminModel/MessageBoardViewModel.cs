using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{
	/// <summary>
	/// 留言板页面查询模型
	/// </summary>
	public class MessageBoardViewModel
	{
		/// <summary>
		/// 关键字
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
		/// 留言板列表
		/// </summary>
		public QueryResult<MessageBoard> MessageBoards { set; get; }
	}

	/// <summary>
	/// 留言板回复
	/// </summary>
	public class MessageBoardReplyModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 回复内容
		/// </summary>
		public string Reply { set; get; }
	}
}