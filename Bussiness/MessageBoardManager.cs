using System;
using BaseClasses;
using BaseClasses.Data;
using BaseClasses.Filters;
using DataRepository.Entities;

namespace Bussiness
{
	/// <summary>
	/// 留言板管理
	/// </summary>
	public class MessageBoardManager:BaseManager
	{
		public MessageBoardManager(MiddleTier middleTier) : base(middleTier)
		{
		}

		public QueryResult<MessageBoard> Query(MessageBoardQuery query)
		{
			return DataBase.Query(query);
		}

		public void SubMessageBoard(string userName, string title, string content)
		{
			var member = MiddleTier.MemberManager.GetMember(userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			if (DataBase.Count<MessageBoard>(e=>e.MemberUserName==userName&&e.CreatedAt>DateTime.Now.Date)>=MiddleTier.BusinessConfig.DailyMessageboardCountLimit)
			{
				throw new PlatformException(ErrorCode.DailyMessageboardCountLimit);
			}
			DataBase.Save(new MessageBoard{Content = content,Title = title,MemberUserName = userName,Reply = "",RepliedAt =null });
		}

		public void Reply(long id, string reply)
		{
			var message = DataBase.Get<MessageBoard>(id);
			if (message == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			message.RepliedAt=DateTime.Now;
			message.Reply = reply;
			DataBase.Update(message);
		}
	}
}
