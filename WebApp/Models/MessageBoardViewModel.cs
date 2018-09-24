namespace WebApp.Models
{
	public class MemberMessageBoardModel
	{
		public int? PageIndex { set; get; }

		public int? PageSize { set; get; }
	}

	public class MemberMessageBoardSubModel
	{
		public string Title { get; set; }

		public string Content { set; get; }
	}
}