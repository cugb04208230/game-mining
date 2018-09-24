using System;
using BaseClasses.Data;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 留言板
	/// </summary>
    public class MessageBoard:BaseEntity
    {
		/// <summary>
		/// 用户Id
		/// </summary>
	    public string MemberUserName { get; set; }

		/// <summary>
		/// 标题
		/// </summary>
		public string Title { set; get; }

		/// <summary>
		/// 内容
		/// </summary>
		public string Content { set; get; }

		/// <summary>
		/// 回复
		/// </summary>
		public string Reply { set; get; }

		/// <summary>
		/// 回复时间
		/// </summary>
		public DateTime? RepliedAt { set; get; }

	}


	/// <inheritdoc />
	/// <summary>
	/// 留言板
	/// </summary>
	public class MessageBoardQuery : BaseQuery<MessageBoard>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { get; set; }
		/// <summary>
		/// 用户名完全匹配
		/// </summary>
		public string MemberUserNameEqual { get; set; }

		/// <summary>
		/// 标题
		/// </summary>
		public string Title { set; get; }

		/// <summary>
		/// 内容
		/// </summary>
		public string Content { set; get; }

		/// <summary>
		/// 回复时间
		/// </summary>
		public DateTime? RepliedAtFrom { set; get; }
		/// <summary>
		/// 回复时间
		/// </summary>
		public DateTime? RepliedAtTo { set; get; }

	}
}
