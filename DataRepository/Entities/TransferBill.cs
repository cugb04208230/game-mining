using System;
using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{

	/// <inheritdoc />
	/// <summary>
	/// 转账单（由卖家发起-买家确认-卖家确认并支付）卖家发起时冻结卖家金额，买家确认并支付时扣除买家余额，并增加
	/// </summary>
	public class TransferBill:BaseEntity
    {
		/// <summary>
		/// 转账单类型
		/// </summary>
	    public TransferBillType TransferBillType { get; set; }

		/// <summary>
		/// 出账账户用户名
		/// </summary>
		public string FromMemberUserName { set; get; }

		/// <summary>
		/// 出账账户类型
		/// </summary>
		public AccountType FromAccountType { set; get; }

		/// <summary>
		/// 入账账户用户名
		/// </summary>
		public string ToMemberUserName { set; get; }

		/// <summary>
		/// 入账账户类型
		/// </summary>
		public AccountType ToAccountType { set; get; }

		/// <summary>
		/// 金额
		/// </summary>
		public decimal Amount { set; get; }

		/// <summary>
		/// 转账状态-用户之间转账时需要
		/// </summary>
		public TransferBillStatus Status { set; get; }

		/// <summary>
		/// 求购过期时间
		/// </summary>
		public DateTime ExpiredAt { set; get; }

		/// <summary>
		/// 转账手续费
		/// </summary>
		public decimal ServiceCharge { set; get; }

		/// <summary>
		/// 赠送者
		/// </summary>
		public Member FromMember { set; get; }

		/// <summary>
		/// 求购者
		/// </summary>
		public Member ToMember { set; get; }

		/// <summary>
		/// 求购时间
		/// </summary>
		public DateTime? ToBuyAt { set; get; }

		/// <summary>
		/// 赠送时间
		/// </summary>
		public DateTime? GivedAt { set; get; }

		/// <summary>
		/// 接收时间
		/// </summary>
		public DateTime? ReceivedAt { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? CompletedAt { set; get; }

	}


	/// <inheritdoc />
	/// <summary>
	/// 转账单（由卖家发起-买家确认-卖家确认并支付）卖家发起时冻结卖家金额，买家确认并支付时扣除买家余额，并增加
	/// </summary>
	public class TransferBillQuery : BaseQuery<TransferBill>
	{
		/// <summary>
		/// 转账单类型
		/// </summary>
		public TransferBillType? TransferBillType { get; set; }

		/// <summary>
		/// 出账账户用户名
		/// </summary>
		public string FromMemberUserName { set; get; }
		
		/// <summary>
		/// 入账账户用户名
		/// </summary>
		public string ToMemberUserName { set; get; }

		/// <summary>
		/// 账户名
		/// </summary>
		public string MemberUserName { set; get; }

		/// <summary>
		/// 转账状态-用户之间转账时需要
		/// </summary>
		public TransferBillStatus? Status { set; get; }

		/// <summary>
		/// 过期时间
		/// </summary>
		public DateTime? ExpiredAtFrom { set; get; }
		/// <summary>
		/// 入账账户用户名
		/// </summary>
		public string ToMemberUserNameNot { set; get; }


	}
}
