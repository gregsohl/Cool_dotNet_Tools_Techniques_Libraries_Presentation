using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace DotNetNumbers
{
    public class DotNetNumbers
    {
		[DllExport("Add", CallingConvention.Cdecl)]
		public static int Add(int a, int b)
		{
			return a + b;
		}

		[DllExport("Subtract", CallingConvention.Cdecl)]
		public static int Subtract(int a, int b)
		{
			return a - b;
		}

		[DllExport("FormatString", CallingConvention.Cdecl)]
		public static string Format(string formatString, int value)
		{
			return string.Format(formatString, value);
		}
    }
}
