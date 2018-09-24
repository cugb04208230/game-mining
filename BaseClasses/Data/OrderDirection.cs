using System.ComponentModel;

namespace BaseClasses.Data
{
	/// <summary>
	/// order direction
	/// for linq or sql orlambda
	/// </summary>
	public enum OrderDirection
	{
		/// <summary>
		/// 升序
		/// </summary>
		[Description("升序")]
		Asc = 1,
		/// <summary>
		/// 降序
		/// </summary>
		[Description("降序")]
		Desc = 2
	}
}
