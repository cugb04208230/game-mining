using System.ComponentModel;

namespace DataRepository.Enums
{
	public enum MemberLevel
	{
	}

	public enum MemberType
	{
		[Description("普通用户")]Normal=1,
		[Description("服务中心")] CallCenter =2
	}
}
