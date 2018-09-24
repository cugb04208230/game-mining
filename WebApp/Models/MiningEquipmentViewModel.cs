using System.Collections.Generic;
using DataRepository.Entities;
using DataRepository.Enums;

namespace WebApp.Models
{
	public class MiningEquipmentViewModel
	{
		public List<MiningEquipment> List { set; get; }
	}

	public class MemberBuyMiningEquipmentModel
	{
		public MiningEquipmentType MiningEquipmentType { set; get; }
	}

	public class MemberMiningEquipmentModel
	{
		public int? PageIndex { set; get; }

		public int? PageSize { set; get; }
	}

	public class MemberCollectModel
	{
		public long Id { get; set; }
	}

}