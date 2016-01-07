using System;
using System.Data;

namespace SampleTests.TestClasses
{
	public class Widget
	{
		public bool IsGood { get; set; }
		public bool IsBad { get; set; }

		public DataSet MyDataSet;

		public Widget()
		{
		}

		public Widget(bool isGood, bool isBad)
			: this()
		{
			IsGood = isGood;
			IsBad = isBad;
		}

		public Widget(bool isGood, bool isBad, DataSet data)
			: this(isGood, isBad)
		{
			MyDataSet = data;
		}
	}
}
