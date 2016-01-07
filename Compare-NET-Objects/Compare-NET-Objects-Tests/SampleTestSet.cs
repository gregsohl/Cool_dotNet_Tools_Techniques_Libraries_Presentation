using System;
using System.Data;
using System.Threading;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using SampleTests.TestClasses;

namespace SampleTests
{
	[TestFixture]
	public class SampleTestSet
	{
		private CompareObjects _compare;

		#region Setup/Teardown

		/// <summary>
		/// Code that is run once for a suite of tests
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{

		}

		/// <summary>
		/// Code that is run once after a suite of tests has finished executing
		/// </summary>
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{

		}

		/// <summary>
		/// Code that is run before each test
		/// </summary>
		[SetUp]
		public void Initialize()
		{
			_compare = new CompareObjects();
			_compare.MaxDifferences = 9;
		}

		/// <summary>
		/// Code that is run after each test
		/// </summary>
		[TearDown]
		public void Cleanup()
		{
			_compare = null;
		}
		#endregion

		[Test]
		public void CompareWidgetTest()
		{
			DataSet ds = CreateDataSet("Fruits", "Apple", "Orange", "Bananna");
			Widget w1 = new Widget(true, true, ds);
			Widget w2 = new Widget(true, true, ds);		


			_compare.CompareChildren = true;
			_compare.CompareFields = true;
			_compare.ComparePrivateFields = true;
			_compare.ComparePrivateProperties = true;
			_compare.CompareProperties = true;
			_compare.CompareReadOnly = true;

			//_compare.TypesToIgnore.Add(typeof(System.Reflection.Pointer));
			//_compare.TypesToIgnore.Add(typeof(System.IntPtr));

			_compare.AddCustomComparer(typeof(DataSet), CompareDataSet);

			//_compare.TypeSpecificCompareOptions.Add(
			//    typeof(DataSet), 
			//    new TypeSpecificCompareOptions(false, false, true, true, true, true));
			
			//_compare.ElementsToIgnore.Add("CultureInfo");
			//_compare.ElementsToIgnore.Add("Pointer");
			//_compare.ElementsToIgnore.Add("DefaultViewManager");
			//_compare.ElementsToIgnore.Add("dataViewManager");
			//_compare.ElementsToIgnore.Add("DataViewManagerListItemTypeDescriptor");
			//_compare.ElementsToIgnore.Add("Locale");
			//_compare.ElementsToIgnore.Add("Relations");
			//_compare.ElementsToIgnore.Add("Tables");
			//_compare.ElementsToIgnore.Add("EventHandlerList");
			//_compare.ElementsToIgnore.Add("DataTableCollection");
			//_compare.ElementsToIgnore.Add("DataSetRelationCollection");

			bool compare = _compare.Compare(w1, w2);
			Assert.IsTrue(compare, _compare.DifferencesString);
			
			// Set back to the defaults
			_compare.CompareChildren = true;
			_compare.CompareFields = true;
			_compare.ComparePrivateFields = false;
			_compare.ComparePrivateProperties = false;
			_compare.CompareProperties = true;
			_compare.CompareReadOnly = true;
		}

		private bool CompareDataSet(object object1, object object2)
		{
			DataSet dataSet1 = (DataSet)object1;
			DataSet dataSet2 = (DataSet)object2;

			if (dataSet1.DataSetName != dataSet2.DataSetName)
				return false;

			if (dataSet1.Namespace != dataSet2.Namespace)
				return false;

			if (dataSet1.Prefix != dataSet2.Prefix)
				return false;

			if (dataSet1.CaseSensitive != dataSet2.CaseSensitive)
				return false;

			if (dataSet1.EnforceConstraints != dataSet2.EnforceConstraints)
				return false;

			if (dataSet1.DataSetName != dataSet2.DataSetName)
				return false;

            if (dataSet1.Tables.Count != dataSet2.Tables.Count)
				return false;

			for (int index = 0; index < dataSet1.Tables.Count; index++)
			{
				if (dataSet1.Tables[index].Rows.Count != dataSet1.Tables[index].Rows.Count)
				{
					return false;
				}


			}

			return true;
		}

		private DataSet CreateDataSet(string dataSetName, params string[] products)
		{
			DataSet ds = new DataSet(dataSetName);
			DataTable dt = new DataTable();
			dt.Columns.Add(new DataColumn("ProductName", Type.GetType("System.String")));
			foreach (string entry in products)
			{
				DataRow row = dt.NewRow();
				row["ProductName"] = products;
			}
			dt.AcceptChanges();

			ds.Tables.Add(dt);

			return ds;
		}
	}
}
