using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace ExampleTests
{
    public class CompareObjectsExampleTests
    {
		[Test]
		public void CompareUnmatchedParents()
		{
			var parent1 = new Parent("Parent1", 1, 2);
			var parent2 = new Parent("Parent2", 1, 2);

			var comparer = new CompareObjects();

			bool compare = comparer.Compare(parent1, parent2);

			Assert.IsTrue(compare, comparer.DifferencesString);
		}

		[Test]
		public void CompareMatchedParents()
		{
			var parent1 = new Parent("Parent1", 1, 2);
			var parent2 = new Parent("Parent1", 1, 2);

			var comparer = new CompareObjects();
			AddIgnoreElements(comparer, "Key");

			bool compare = comparer.Compare(parent1, parent2);

			Assert.IsTrue(compare, comparer.DifferencesString);
		}

		[Test]
		public void CompareIgnoreChildParents()
		{
			var parent1 = new Parent("Parent1", 1, 2);
			parent1.AddChild("Child1", 1);

			var parent2 = new Parent("Parent1", 1, 2);

			var comparer = new CompareObjects();
			AddIgnoreElements(comparer, "Key", "m_Children");

			bool compare = comparer.Compare(parent1, parent2);

			Assert.IsTrue(compare, comparer.DifferencesString);
		}

		private void AddIgnoreElements(CompareObjects comparer, params string[] elements)
		{
			if ((elements != null) && (elements.Length > 0))
			{
				comparer.ElementsToIgnore.AddRange(elements);
			}
		}

    }
}
