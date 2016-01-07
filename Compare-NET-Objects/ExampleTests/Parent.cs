using System;
using System.Collections.Generic;

namespace ExampleTests
{
	public class Parent
	{
		public Parent(string name, int value1, int value2)
		{
			Key = Guid.NewGuid();
			Name = name;
			Value1 = value1;
			Value2 = value2;
			m_Children = new List<Child>();
		}

		public Guid Key { get; set; }
		public string Name { get; set; }
		public int Value1 { get; set; }
		public int Value2 { get; set; }

		public void AddChild(string name, int value)
		{
			var child = new Child(name, value, this);
			m_Children.Add(child);
		}

		private readonly List<Child> m_Children;
	}
}