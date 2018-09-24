using System;

namespace BaseClasses.Attributes
{
	public class TradeSummaryAttribute:Attribute
	{
		public string ToSummary { set; get; }

		public string FromSummary { set; get; }

		public TradeSummaryAttribute( string fromSummary, string toSummary)
		{
			ToSummary = toSummary;
			FromSummary = fromSummary;
		}
	}
}
