using System;

namespace BaseClasses.Attributes
{
	public class JobAttribute : Attribute
	{
		public string Cron { set; get; }
	}
}
