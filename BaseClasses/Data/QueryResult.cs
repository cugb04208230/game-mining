using System.Collections.Generic;
using Newtonsoft.Json;

namespace BaseClasses.Data
{
	/// <summary>
	/// 列表查询返回
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class QueryResult<TEntity>
	{
        /// <summary>
        /// 查询参数
        /// </summary>
        [JsonIgnore]
        public BaseQuery<TEntity> Query { set; get; }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        public IList<TEntity> List { get; set; }
		/// <summary>
		/// 总数量
		/// </summary>
		public int Count { get; set; }
	}
}
