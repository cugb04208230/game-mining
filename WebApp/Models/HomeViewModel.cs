using System.Collections.Generic;
using DataRepository.Entities;

namespace WebApp.Models
{
	public class HomeViewModel
	{
		public Member Member { get; set; }

		public int InUseMemberCount { set; get; }

		public int ActiveMemberCount { set; get; }

		public int EquipmentCount { set; get; }

		public IEnumerable<Notice> Notices { set; get; }

		public string QrCode { set; get; }
	}
}