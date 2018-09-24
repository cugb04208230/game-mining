namespace BaseClasses.Data
{
	/// <summary>
	/// 模型基类，只包含主键
	/// </summary>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public abstract class EntityBase<TPrimaryKey>
	{
		/// <summary>
		/// 主键
		/// </summary>
		public virtual TPrimaryKey Id { get; set; }
	}
}
