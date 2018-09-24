using System;
using System.Collections.Generic;

namespace BaseClasses.Data
{
	/// <summary>
	/// represents query conditions object against a entity
	/// </summary>
	[Serializable]
	public class BaseQuery
	{
		/// <summary>
		/// ctor.
		/// </summary>
		public BaseQuery()
		{
			Includes = new string[0];
			DirectionList = new KeyValuePair<string, OrderDirection>[0];
		}

		/// <summary>
		/// 主键
		/// </summary>
		public long? Id { get; set; }
		/// <summary>
		/// represents query if contains BaseEntity.Id
		/// </summary>
		public long[] Ids { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// 创建时间范围
        /// </summary>
        public DateTime? CreatedAtFrom { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreatedAtTo { get; set; }
		/// <summary>
		/// 更新时间范围
		/// </summary>
		public DateTime? LastModifiedAtFrom { get; set; }
		/// <summary>
		/// 更新时间范围
		/// </summary>
		public DateTime? LastModifiedAtTo { get; set; }
		/// <summary>
		/// 页码（参数）
		/// </summary>
		public int? PageIndex { get; set; }
		/// <summary>
		/// 页大小（参数）
		/// </summary>
		public int? PageSize { get; set; }

		/// <summary>
		/// take for 
		/// </summary>
		public int Take => PageSize??10;

		/// <summary>
		/// skip number
		/// </summary>
		public int Skip => ((PageIndex ?? 1) - 1) * Take;
		/// <summary>
		/// 
		/// </summary>
		public List<OrderField> OrderBys { set; get; }

		/// <summary>
		/// 
		/// </summary>
		public string[] Includes { get; set; }

		/// <summary>
		/// 方向列表
		/// </summary>
		public KeyValuePair<string, OrderDirection>[] DirectionList { get; set; }
	}

	/// <summary>
	/// 排序对象
	/// </summary>
	public class OrderField
	{
		public OrderField(string orderBy="Id", OrderDirection orderDirection = Data.OrderDirection.Desc)
		{
			OrderBy = orderBy;
			OrderDirection = orderDirection;
		}

		/// <summary>
		/// order field
		/// </summary>
		public string OrderBy { get; set; }
		/// <summary>
		/// order direction
		/// </summary>
		public OrderDirection? OrderDirection { get; set; }
	}

	/// <inheritdoc />
	/// <summary>
	/// 查询条件
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	[Serializable]
	public class BaseQuery<TEntity> : BaseQuery
	{
		/// <summary>
		/// typeof(TEntity)
		/// </summary>
		public Type EntityType => typeof(TEntity);
	}
}
