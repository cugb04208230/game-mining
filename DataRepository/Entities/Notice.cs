using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 系统通知
	/// </summary>
	public class Notice:BaseEntity
	{

		public string Title { get; set; }

		public string Content { get; set; }
		
	}


	/// <inheritdoc />
	/// <summary>
	/// 系统通知
	/// </summary>
	public class NoticeQuery : BaseQuery<Notice>
	{
		public string Title { get; set; }

		public string Content { get; set; }
	}
}
