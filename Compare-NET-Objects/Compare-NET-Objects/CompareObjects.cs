#region Includes
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
#endregion

//This software is provided free of charge from from Kellerman Software.
//It may be used in any project, including commercial for sale projects.
//
//Check out our other great software at www.kellermansoftware.com:
// *  Free Quick Reference Pack for Developers
// *  Free Sharp Zip Wrapper
// *  NUnit Test Generator
// * .NET Caching Library
// * .NET Email Validation Library
// * .NET FTP Library
// * .NET Encryption Library
// * .NET Logging Library
// * Themed Winform Wizard
// * Unused Stored Procedures
// * AccessDiff

#region License
//Microsoft Public License (Ms-PL)

//This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.

//1. Definitions

//The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.

//A "contribution" is the original software, or any additions or changes to the software.

//A "contributor" is any person that distributes its contribution under this license.

//"Licensed patents" are a contributor's patent claims that read directly on its contribution.

//2. Grant of Rights

//(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.

//(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

//3. Conditions and Limitations

//(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.

//(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.

//(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.

//(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.

//(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
#endregion License

namespace KellermanSoftware.CompareNetObjects
{

	#region Delegates

	/// <summary>
	/// Delegate for custom comparisons
	/// </summary>
	public delegate bool CustomComparerDelegate(object object1, object object2);

	/// <summary>
	/// Delegate for custom comparisons that returns a list of differences
	/// </summary>
	public delegate List<String> CustomComparerDelegateWithDifferenceRecording(object object1, object object2);

	#endregion Delegates

    /// <summary>
    /// Class that allows comparison of two objects of the same type to each other.  Supports classes, lists, arrays, dictionaries, child comparison and more.
    /// </summary>
    public class CompareObjects
    {
        #region Class Variables

		private List<String> _differences = new List<String>();
        private List<object> _parents = new List<object>();
		private List<string> _elementsToIgnore = new List<string>();
        private bool _comparePrivateProperties = false;
        private bool _comparePrivateFields = false;
        private bool _compareChildren = true;
        private bool _compareReadOnly = true;
        private bool _compareFields = true;
        private bool _compareProperties = true;
        private int _maxDifferences = 1;
		private List<Type> _typesToIgnore = new List<Type>();
    	private Dictionary<Type, TypeSpecificCompareOptions> _typeSpecificCompareOptions = new Dictionary<Type, TypeSpecificCompareOptions>();
		private Dictionary<Type, Delegate> _customComparers = new Dictionary<Type, Delegate>();
		
		#endregion Class Variables

		#region Public Properties

		/// <summary>
        /// Ignore classes, properties, or fields by name during the comparison.
        /// Case sensitive.
        /// </summary>
        public List<string> ElementsToIgnore
        {
            get { return _elementsToIgnore; }
            set { _elementsToIgnore = value; }
        }

		/// <summary>
		/// Ignore classes, properties, or fields by name during the comparison.
		/// Case sensitive.
		/// </summary>
		public List<Type> TypesToIgnore
		{
			get { return _typesToIgnore; }
			set { _typesToIgnore = value; }
		}

		/// <summary>
        /// If true, private properties will be compared. The default is false.
        /// </summary>
        public bool ComparePrivateProperties
        {
            get { return _comparePrivateProperties; } 
            set { _comparePrivateProperties = value; }
        }

        /// <summary>
        /// If true, private fields will be compared. The default is false.
        /// </summary>
        public bool ComparePrivateFields
        {
            get { return _comparePrivateFields; }
            set { _comparePrivateFields = value; }
        }

        /// <summary>
        /// If true, child objects will be compared. The default is true. 
        /// If false, and a list or array is compared list items will be compared but not their children.
        /// </summary>
        public bool CompareChildren
        {
            get { return _compareChildren; } 
            set { _compareChildren = value; }
        }

        /// <summary>
        /// If true, compare read only properties (only the getter is implemented).
        /// The default is true.
        /// </summary>
        public bool CompareReadOnly
        {
            get { return _compareReadOnly; }
            set { _compareReadOnly = value; }
        }

        /// <summary>
        /// If true, compare fields of a class (see also CompareProperties).
        /// The default is true.
        /// </summary>
        public bool CompareFields
        {
            get { return _compareFields; }
            set { _compareFields = value; }
        }

        /// <summary>
        /// If true, compare properties of a class (see also CompareFields).
        /// The default is true.
        /// </summary>
        public bool CompareProperties
        {
            get { return _compareProperties; }
            set { _compareProperties = value; }
        }

		/// <summary>
		/// Gets or sets the type specific compare options.
		/// </summary>
		/// <value>The type specific compare options.</value>
    	public Dictionary<Type, TypeSpecificCompareOptions> TypeSpecificCompareOptions
    	{
    		get { return _typeSpecificCompareOptions; }
    	}

    	/// <summary>
        /// The maximum number of differences to detect
        /// </summary>
        /// <remarks>
        /// Default is 1 for performance reasons.
        /// </remarks>
        public int MaxDifferences
        {
            get { return _maxDifferences; }
            set { _maxDifferences = value; }
        }

        /// <summary>
        /// The differences found during the compare
        /// </summary>
        public List<String> Differences
        {
            get { return _differences; }
            set { _differences = value; }
        }

        /// <summary>
        /// The differences found in a string suitable for a textbox
        /// </summary>
        public string DifferencesString
        {
            get
            {
                StringBuilder sb = new StringBuilder(4096);

                sb.Append("\r\nBegin Differences:\r\n");

                foreach (string item in Differences)
                {
                    sb.AppendFormat("{0}\r\n", item);
                }

                sb.AppendFormat("End Differences (Maximum of {0} differences shown).",MaxDifferences);

                return sb.ToString();
            }
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
        /// Compare two objects of the same type to each other.
        /// </summary>
        /// <remarks>
        /// Check the Differences or DifferencesString Properties for the differences.
        /// Default MaxDifferences is 1
        /// </remarks>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns>True if they are equal</returns>
        public bool Compare(object object1, object object2)
        {
            string defaultBreadCrumb = string.Empty;

            Differences.Clear();
            Compare(object1, object2, defaultBreadCrumb);

            return Differences.Count == 0;
		}

		/// <summary>
		/// Adds the custom comparer.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="customComparer">The custom comparer.</param>
		public void AddCustomComparer(Type type, CustomComparerDelegate customComparer)
		{
			_customComparers.Add(type, customComparer);
		}

		/// <summary>
		/// Adds the custom comparer.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="customComparer">The custom comparer.</param>
		public void AddCustomComparerWithDifferenceRecording(Type type, CustomComparerDelegateWithDifferenceRecording customComparer)
		{
			_customComparers.Add(type, customComparer);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
        /// Compare two objects
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb">Where we are in the object hiearchy</param>
        private void Compare(object object1, object object2, string breadCrumb)
        {
            //If both null return true
            if (object1 == null && object2 == null)
                return;

            //Check if one of them is null
            if (object1 == null)
            {
                Differences.Add(string.Format("object1{0} == null && object2{0} != null ((null),{1})", breadCrumb,cStr(object2)));
                return;
            }

            if (object2 == null)
            {
                Differences.Add(string.Format("object1{0} != null && object2{0} == null ({1},(null))", breadCrumb, cStr(object1)));
                return;
            }

            Type t1 = object1.GetType();
            Type t2 = object2.GetType();

            //Objects must be the same type
            if (t1 != t2)
            {
                Differences.Add(string.Format("Different Types:  object1{0}.GetType() != object2{0}.GetType()", breadCrumb));
                return;
            }

        	Delegate customComparer;
        	if (_customComparers.TryGetValue(t1, out customComparer))
        	{
        		CompareWithCustomComparer(object1, object2, breadCrumb, t1, customComparer);
        	}
        	else if (IsIList(t1)) //This will do arrays, multi-dimensional arrays and generic lists
            {
                CompareIList(object1, object2, breadCrumb);
            }
            else if (IsIDictionary(t1))
            {
                CompareIDictionary(object1, object2, breadCrumb);
            }
            else if (IsEnum(t1))
            {
                CompareEnum(object1, object2, breadCrumb);
            }
            else if (IsSimpleType(t1))
            {
                CompareSimpleType(object1, object2, breadCrumb);
            }
            else if (IsClass(t1))
            {
                CompareClass(object1, object2, breadCrumb);
            }
            else if (IsTimespan(t1))
            {
                CompareTimespan(object1, object2, breadCrumb);
            }
            else if (IsStruct(t1))
            {
                CompareStruct(object1, object2, breadCrumb);
            }
            else
            {
                throw new NotImplementedException("Cannot compare object of type " + t1.Name);
            }

        }

    	private void CompareWithCustomComparer(object object1,
    	                                       object object2,
    	                                       string breadCrumb,
    	                                       Type t1,
    	                                       Delegate customComparer)
    	{
    		if (TypesToIgnore.Contains(t1))
    			return;

			CustomComparerDelegateWithDifferenceRecording customComparerDelegateWithDifferenceRecording = customComparer as CustomComparerDelegateWithDifferenceRecording;
			CustomComparerDelegate customComparerDelegate = customComparer as CustomComparerDelegate;

			if (customComparerDelegateWithDifferenceRecording != null)
			{
				// Compare with a custom comparator
				List<string> differences = customComparerDelegateWithDifferenceRecording.Invoke(object1, object2);
				if (differences.Count > 0)
				{
					Differences.AddRange(differences);
				}
			}
			else
			{
				if (customComparerDelegate != null)
				{
					// Compare with a custom comparator
					if (!customComparerDelegate.Invoke(object1, object2))
					{
						Differences.Add(string.Format("object1{0} != object2{0} ({1},{2})", breadCrumb, object1, object2));
					}
				}
				else
				{
					throw new InvalidOperationException("Bad custom comparer specified.");
				}
			}

			if (Differences.Count >= MaxDifferences)
				return;
    	}

    	#region Data Type Detection

    	private bool IsTimespan(Type t)
    	{
    		return t == typeof(TimeSpan);
    	}

    	private bool IsEnum(Type t)
    	{
    		return t.IsEnum;
    	}

    	private bool IsStruct(Type t)
    	{
    		return t.IsValueType;
    	}

    	private bool IsSimpleType(Type t)
    	{
    		return t.IsPrimitive
    		       || t == typeof(DateTime)
    		       || t == typeof(decimal)
    		       || t == typeof(string)
    		       || t == typeof(Guid);

    	}

    	private bool ValidStructSubType(Type t)
    	{
    		return IsSimpleType(t)
    		       || IsEnum(t)
    		       || IsArray(t)
    		       || IsClass(t)
    		       || IsIDictionary(t)
    		       || IsTimespan(t)
    		       || IsIList(t);
    	}

    	private bool IsArray(Type t)
    	{
    		return t.IsArray;
    	}

    	private bool IsClass(Type t)
    	{
    		return t.IsClass;
    	}

    	private bool IsIDictionary(Type t)
    	{
    		return t.GetInterface("System.Collections.IDictionary", true) != null;
    	}

    	private bool IsIList(Type t)
    	{
    		return t.GetInterface("System.Collections.IList", true) != null;
    	}

    	private bool IsChildType(Type t)
    	{
    		return IsClass(t)
    		       || IsArray(t)
    		       || IsIDictionary(t)
    		       || IsIList(t)
    		       || IsStruct(t);
    	}

    	#endregion Data Type Detection
    	
        /// <summary>
        /// Compare a timespan struct
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareTimespan(object object1, object object2, string breadCrumb)
        {
            if (((TimeSpan)object1).Ticks != ((TimeSpan)object2).Ticks)
            {
                Differences.Add(string.Format("object1{0}.Ticks != object2{0}.Ticks", breadCrumb));
            }
        }

        /// <summary>
        /// Compare an enumeration
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareEnum(object object1, object object2, string breadCrumb)
        {
            if (object1.ToString() != object2.ToString())
            {
                string currentBreadCrumb = AddBreadCrumb(breadCrumb, object1.GetType().Name, string.Empty, -1);
                Differences.Add(string.Format("object1{0} != object2{0} ({1},{2})", currentBreadCrumb, object1, object2));
            }
        }

        /// <summary>
        /// Compare a simple type
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareSimpleType(object object1, object object2, string breadCrumb)
        {
            if (object2 == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object2");

            IComparable valOne = object1 as IComparable;
			IComparable valTwo = object2 as IComparable;

			// Treat both null as being equal
			if ((valOne == null) && (valTwo == null))
				return;

            if (valOne == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object1");

            if (valOne.CompareTo(object2) != 0)
            {
                Differences.Add(string.Format("object1{0} != object2{0} ({1},{2})", breadCrumb, object1, object2));
            }
        }

        /// <summary>
        /// Compare a struct
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareStruct(object object1, object object2, string breadCrumb)
        {
            Type t1 = object1.GetType();

            //Compare the fields
            FieldInfo[] currentFields = t1.GetFields();

        	PerformCompareFields(t1, object1, object2, breadCrumb);
        }

        /// <summary>
        /// Compare the properties, fields of a class
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareClass(object object1, object object2, string breadCrumb)
        {
            try
            {
                _parents.Add(object1);
                _parents.Add(object2);
                Type t1 = object1.GetType();

                //We ignore the class name
                if (ElementsToIgnore.Contains(t1.Name))
                    return;

				if (TypesToIgnore.Contains(t1))
					return;

            	if (CompareChildrenForType(t1))
            	{
            		// Compare the properties, if allowed for this object type
            		if (ComparePropertiesForType(t1))
            			PerformCompareProperties(t1, object1, object2, breadCrumb);

            		// Compare the fields, if allowed for this object type
            		if (CompareFieldsForType(t1))
            			PerformCompareFields(t1, object1, object2, breadCrumb);
            	}
            }
            finally
            {
                _parents.Remove(object1);
                _parents.Remove(object2);
            }
        }

        /// <summary>
        /// Compare the fields of a class
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void PerformCompareFields(Type t1,
            object object1,
            object object2,
            string breadCrumb)
        {
            object objectValue1;
            object objectValue2;
            string currentCrumb;

            FieldInfo[] currentFields;
            
            if (ComparePrivateFieldsForType(t1))
                currentFields= t1.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            else
                currentFields= t1.GetFields(); //Default is public instance
                        
            foreach (FieldInfo item in currentFields)
            {
                //Skip if this is a shallow compare
				if (!CompareChildrenForType(t1) && IsChildType(item.FieldType))
                    continue;

                //If we should ignore it, skip it
				if (SkipElementForType(t1, item.Name))
                    continue;

				if (IgnoreTypeForType(t1, item.FieldType))
					return;

                objectValue1 = item.GetValue(object1);
                objectValue2 = item.GetValue(object2);

                bool object1IsParent = objectValue1 != null && (objectValue1 == object1 || _parents.Contains(objectValue1));
                bool object2IsParent = objectValue2 != null && (objectValue2 == object2 || _parents.Contains(objectValue2));

                //Skip fields that point to the parent
                if (((IsClass(item.FieldType)) ||
					 (IsIList(item.FieldType)) ||
					 (IsIDictionary(item.FieldType)))
                    && (object1IsParent || object2IsParent))
                {
                    continue;
                }

                currentCrumb = AddBreadCrumb(breadCrumb, item.Name, string.Empty, -1);

                Compare(objectValue1, objectValue2, currentCrumb);

                if (Differences.Count >= MaxDifferences)
                    return;
            }
        }

        /// <summary>
        /// Compare the properties of a class
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void PerformCompareProperties(Type t1, 
            object object1,
            object object2,
            string breadCrumb)
        {
            object objectValue1 = null;
            object objectValue2 = null;
            string currentCrumb;

            PropertyInfo[] currentProperties;

            if (ComparePrivatePropertiesForType(t1))
                currentProperties = t1.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            else
                currentProperties =  t1.GetProperties(); //Default is public instance            

            foreach (PropertyInfo info in currentProperties)
            {
                //If we can't read it, skip it
                if (info.CanRead == false)
                    continue;

                //Skip if this is a shallow compare
                if (!CompareChildrenForType(t1) && IsChildType(info.PropertyType))
                    continue;

                //If we should ignore it, skip it
				if (SkipElementForType(t1, info.Name))
                    continue;

				if (IgnoreTypeForType(t1, info.PropertyType))
					return;

                //If we should ignore read only, skip it
				if (!CompareReadOnlyForType(info.PropertyType) && 
					(info.CanWrite == false))
                    continue;

            	Exception firstObjectException = null;
				try
				{
					objectValue1 = info.GetValue(object1, null);
				}
				catch(Exception ex)
				{
					firstObjectException = ex;
				}

				try
				{
					objectValue2 = info.GetValue(object2, null);
				}
				catch(Exception ex)
				{
					if (firstObjectException == null)
					{
						throw;
					}

					// If both Gets threw the same exception, then condsider them equal,
					// otherwise re-throw the exception.
					if (!((firstObjectException.GetType() == ex.GetType()) &&
						 (firstObjectException.Message == ex.Message)))
					{
						throw;
					}
				}

            	bool object1IsParent = objectValue1 != null && (objectValue1 == object1 || _parents.Contains(objectValue1));
                bool object2IsParent = objectValue2 != null && (objectValue2 == object2 || _parents.Contains(objectValue2));

                //Skip properties where both point to the corresponding parent
				if (((IsClass(info.PropertyType)) ||
					 (IsIList(info.PropertyType)) ||
					 (IsIDictionary(info.PropertyType)))
					&& (object1IsParent || object2IsParent))
				{
                    continue;
                }

                currentCrumb = AddBreadCrumb(breadCrumb, info.Name, string.Empty, -1);

                Compare(objectValue1, objectValue2, currentCrumb);

                if (Differences.Count >= MaxDifferences)
                    return;
            }
        }

        /// <summary>
        /// Compare a dictionary
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareIDictionary(object object1, object object2, string breadCrumb)
        {
			_parents.Add(object1);
			_parents.Add(object2);

			IDictionary iDict1 = object1 as IDictionary;
            IDictionary iDict2 = object2 as IDictionary;

            if (iDict1 == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object1");

            if (iDict2 == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object2");

            //Objects must be the same length
            if (iDict1.Count != iDict2.Count)
            {
                Differences.Add(string.Format("object1{0}.Count != object2{0}.Count ({1},{2})", breadCrumb,iDict1.Count,iDict2.Count));

                if (Differences.Count >= MaxDifferences)
                    return;
            }

            IDictionaryEnumerator enumerator1 = iDict1.GetEnumerator();
            IDictionaryEnumerator enumerator2 = iDict2.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                string currentBreadCrumb = AddBreadCrumb(breadCrumb, "Key", string.Empty, -1);

				bool object1IsParent = enumerator1.Key != null && (enumerator1.Key == object1 || _parents.Contains(enumerator1.Key));
				bool object2IsParent = enumerator2.Key != null && (enumerator2.Key == object2 || _parents.Contains(enumerator2.Key));

				//Skip keys where both have been previously visited
				if (!((enumerator1.Key == null) || 
					 (!IsClass(enumerator1.Key.GetType())) ||
            	     (!object1IsParent || !object2IsParent)))
            	{
            		Compare(enumerator1.Key, enumerator2.Key, currentBreadCrumb);

            		if (Differences.Count >= MaxDifferences)
            		{
            			return;
            		}
            	}

            	currentBreadCrumb = AddBreadCrumb(breadCrumb, "Value", string.Empty, -1);

				object1IsParent = enumerator1.Value != null && (enumerator1.Value == object1 || _parents.Contains(enumerator1.Value));
				object2IsParent = enumerator2.Value != null && (enumerator2.Value == object2 || _parents.Contains(enumerator2.Value));

				//Skip values where both have been previously visited
				if ((enumerator1.Value != null) &&
					(IsClass(enumerator1.Value.GetType())) &&
					(object1IsParent && object2IsParent))
				{
					continue;
				}

                Compare(enumerator1.Value, enumerator2.Value, currentBreadCrumb);

                if (Differences.Count >= MaxDifferences)
                    return;                
            }

        }

        /// <summary>
        /// Convert an object to a nicely formatted string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string cStr(object obj)
        {
            try
            {                
                if (obj == null)
                    return "(null)";

                if (obj == DBNull.Value)
                    return "System.DBNull.Value";
                
                return obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Compare an array or something that implements IList
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="breadCrumb"></param>
        private void CompareIList(object object1, object object2, string breadCrumb)
        {
			_parents.Add(object1);
			_parents.Add(object2);

            IList ilist1 = object1 as IList;
            IList ilist2 = object2 as IList;

            if (ilist1 == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object1");

            if (ilist2 == null) //This should never happen, null check happens one level up
                throw new ArgumentNullException("object2");

            //Objects must be the same length
            if (ilist1.Count != ilist2.Count)
            {
                Differences.Add(string.Format("object1{0}.Count != object2{0}.Count ({1},{2})", breadCrumb, ilist1.Count, ilist2.Count));

                if (Differences.Count >= MaxDifferences)
                    return;
            }

            IEnumerator enumerator1 = ilist1.GetEnumerator();
            IEnumerator enumerator2 = ilist2.GetEnumerator();
            int count = 0;

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                string currentBreadCrumb = AddBreadCrumb(breadCrumb, string.Empty, string.Empty, count);

				bool object1IsParent = enumerator1.Current != null && (enumerator1.Current == object1 || _parents.Contains(enumerator1.Current));
				bool object2IsParent = enumerator2.Current != null && (enumerator2.Current == object2 || _parents.Contains(enumerator2.Current));

				//Skip properties where both point to the corresponding parent
				if ((enumerator1.Current != null) &&
					(IsClass(enumerator1.Current.GetType())) &&
					(object1IsParent && object2IsParent))
				{
					continue;
				}

                Compare(enumerator1.Current, enumerator2.Current, currentBreadCrumb);

                if (Differences.Count >= MaxDifferences)
                    return;

                count++;
            }
        }

        /// <summary>
        /// Add a breadcrumb to an existing breadcrumb
        /// </summary>
        /// <param name="existing"></param>
        /// <param name="name"></param>
        /// <param name="extra"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string AddBreadCrumb(string existing, string name, string extra, int index)
        {
            bool useIndex= index >= 0;
            bool useName= name.Length > 0;
            StringBuilder sb = new StringBuilder();

            sb.Append(existing);

            if (useName)
            {
                sb.AppendFormat(".");
                sb.Append(name);
            }

            sb.Append(extra);

            if (useIndex)
                sb.AppendFormat("[{0}]", index);

            return sb.ToString();
        }

    	#region Configuration Check Methods

    	private bool ComparePrivatePropertiesForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].ComparePrivateProperties;
    		}

    		return ComparePrivateProperties;
    	}

    	private bool ComparePrivateFieldsForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].ComparePrivateFields;
    		}

    		return ComparePrivateFields;
    	}

    	private bool ComparePropertiesForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].CompareProperties;
    		}

    		return CompareProperties;
    	}

    	private bool CompareFieldsForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].CompareFields;
    		}

    		return CompareFields;
    	}

    	private bool CompareChildrenForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].CompareChildren;
    		}

    		return CompareChildren;
    	}

    	private bool CompareReadOnlyForType(Type type)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].CompareReadOnly;
    		}

    		return CompareReadOnly;
    	}

    	private bool SkipElementForType(Type type, string elementName)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].ElementsToIgnore.Contains(elementName);
    		}

    		return ElementsToIgnore.Contains(elementName);
    	}

    	private bool IgnoreTypeForType(Type type, Type typeToIgnore)
    	{
    		if (TypeSpecificCompareOptions.ContainsKey(type))
    		{
    			return TypeSpecificCompareOptions[type].TypesToIgnore.Contains(typeToIgnore);
    		}

    		return TypesToIgnore.Contains(typeToIgnore);
    	}

    	#endregion Configuration Check Methods
    	
        #endregion Private Methods
    }
}
