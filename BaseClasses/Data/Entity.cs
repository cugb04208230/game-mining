using System;
using System.ComponentModel;

namespace BaseClasses.Data
{
	/// <inheritdoc />
	/// <summary>
	/// 业务模型基类
	/// </summary>
    public class BaseEntity:EntityBase<long>
    {
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        [DefaultValue(true)]
        public bool IsEnabled { set; get; }

	    /// <summary>
	    /// 创建时间
	    /// </summary>
	    public DateTime CreatedAt { set; get; }

		/// <summary>
		/// 最后更新时间
		/// </summary>
		public DateTime LastModifiedAt { set; get; }
	}
}
