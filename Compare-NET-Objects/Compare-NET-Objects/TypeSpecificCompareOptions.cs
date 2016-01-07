#region Namespaces

using System;
using System.Collections.Generic;

#endregion Namespaces

namespace KellermanSoftware.CompareNetObjects
{
	/// <summary>
	/// Provides configuration specifying how-to compare a single data type. Various options are 
	/// available for the operation of the CompareObjects class for a data type.
	/// </summary>
	public class TypeSpecificCompareOptions
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSpecificCompareOptions"/> class.
		/// </summary>
		/// <param name="comparePrivateProperties">if set to <c>true</c> [compare private properties].</param>
		/// <param name="comparePrivateFields">if set to <c>true</c> [compare private fields].</param>
		/// <param name="compareChildren">if set to <c>true</c> [compare children].</param>
		/// <param name="compareReadOnly">if set to <c>true</c> [compare read only].</param>
		/// <param name="compareFields">if set to <c>true</c> [compare fields].</param>
		/// <param name="compareProperties">if set to <c>true</c> [compare properties].</param>
		public TypeSpecificCompareOptions(
			bool comparePrivateProperties,
			bool comparePrivateFields,
			bool compareChildren,
			bool compareReadOnly,
			bool compareFields,
			bool compareProperties)
		{
			m_ComparePrivateProperties = comparePrivateProperties;
			m_ComparePrivateFields = comparePrivateFields;
			m_CompareChildren = compareChildren;
			m_CompareReadOnly = compareReadOnly;
			m_CompareFields = compareFields;
			m_CompareProperties = compareProperties;
		}

		#endregion Constructor
		
		#region Public Properties

		/// <summary>
		/// Ignore classes, properties, or fields by name during the comparison.
		/// Case sensitive.
		/// </summary>
		public List<string> ElementsToIgnore
		{
			get { return m_ElementsToIgnore; }
			set { m_ElementsToIgnore = value; }
		}

		/// <summary>
		/// Ignore classes, properties, or fields by name during the comparison.
		/// Case sensitive.
		/// </summary>
		public List<Type> TypesToIgnore
		{
			get { return m_TypesToIgnore; }
			set { m_TypesToIgnore = value; }
		}

		/// <summary>
		/// If true, private properties will be compared. The default is false.
		/// </summary>
		public bool ComparePrivateProperties
		{
			get { return m_ComparePrivateProperties; }
			set { m_ComparePrivateProperties = value; }
		}

		/// <summary>
		/// If true, private fields will be compared. The default is false.
		/// </summary>
		public bool ComparePrivateFields
		{
			get { return m_ComparePrivateFields; }
			set { m_ComparePrivateFields = value; }
		}

		/// <summary>
		/// If true, child objects will be compared. The default is true. 
		/// If false, and a list or array is compared list items will be compared but not their children.
		/// </summary>
		public bool CompareChildren
		{
			get { return m_CompareChildren; }
			set { m_CompareChildren = value; }
		}

		/// <summary>
		/// If true, compare read only properties (only the getter is implemented).
		/// The default is true.
		/// </summary>
		public bool CompareReadOnly
		{
			get { return m_CompareReadOnly; }
			set { m_CompareReadOnly = value; }
		}

		/// <summary>
		/// If true, compare fields of a class (see also CompareProperties).
		/// The default is true.
		/// </summary>
		public bool CompareFields
		{
			get { return m_CompareFields; }
			set { m_CompareFields = value; }
		}

		/// <summary>
		/// If true, compare properties of a class (see also CompareFields).
		/// The default is true.
		/// </summary>
		public bool CompareProperties
		{
			get { return m_CompareProperties; }
			set { m_CompareProperties = value; }
		}

		#endregion Public Properties

		#region Private Fields

		private List<string> m_ElementsToIgnore = new List<string>();
		private List<Type> m_TypesToIgnore = new List<Type>();
		private bool m_ComparePrivateProperties;
		private bool m_ComparePrivateFields;
		private bool m_CompareChildren = true;
		private bool m_CompareReadOnly = true;
		private bool m_CompareFields = true;
		private bool m_CompareProperties = true;

		#endregion Private Fields
	}
}
