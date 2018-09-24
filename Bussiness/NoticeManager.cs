using BaseClasses.Data;
using DataRepository.Entities;

namespace Bussiness
{
	public class NoticeManager:BaseManager
	{
		public NoticeManager(MiddleTier middleTier) : base(middleTier)
		{
		}

		/// <summary>
		/// 新增通知
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		public void Add(string title, string content)
		{
			DataBase.Save(new Notice
			{
				Content = content,
				Title = title
			});
		}

		public void AddOrUpdate(long? id, string title, string content)
		{
			var notice = DataBase.Get<Notice>(id??0);
			if (notice == null)
			{
				notice = new Notice {Title = title, Content = content};
				DataBase.Save(notice);
			}
			else
			{
				notice.Title = title;
				notice.Content = content;
				DataBase.Update(notice);
			}
		}

		/// <summary>
		/// 删除通知
		/// </summary>
		/// <param name="id"></param>
		public void Delete(long id)
		{
			DataBase.Delete<Notice>(e=>e.Id==id);
		}

		/// <summary>
		/// 通知查询
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public QueryResult<Notice> Query(NoticeQuery query)
		{
			return DataBase.Query(query);
		}

	}
}
