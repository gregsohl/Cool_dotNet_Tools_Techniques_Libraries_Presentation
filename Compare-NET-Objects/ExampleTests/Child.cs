namespace ExampleTests
{
	public class Child
	{
		public Child(string name, int value1, Parent myParent)
		{
			m_MyParent = myParent;
			Name = name;
			Value1 = value1;
		}

		public string Name { get; set; }
		public int Value1 { get; set; }

		private readonly Parent m_MyParent;

	}
}