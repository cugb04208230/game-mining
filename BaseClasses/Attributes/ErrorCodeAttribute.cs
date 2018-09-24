using System;

namespace BaseClasses.Attributes
{
	public class ErrorCodeAttribute:Attribute
	{
		public string Alert { set; get; }

		public string NextAction { set; get; }
	}
}
