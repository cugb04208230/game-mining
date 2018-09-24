namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 留言板-查询
	/// </summary>
	public class MemberMessageBoardModel
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
	/// 留言板-提交
	/// </summary>
	public class MemberMessageBoardSubModel
	{
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public string Content { set; get; }
	}
}