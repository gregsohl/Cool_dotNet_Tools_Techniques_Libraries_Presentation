using System;
using System.IO;
using Gios.PDF.SplitMerge;

namespace SamplePdfMerge
{
	class Program
	{
		static void Main(string[] args)
		{
			string inputFile = args[0];
			string outputFile = args[1];
			int iterations = Int32.Parse(args[2]);

			// Merge the generated pdfs
			using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
			{
				var merger = new PdfMerger(outputStream);
				for (int count = 1; count <= iterations; count++ )
				{
					using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						var pdfParser = PdfParser.Parse(inputStream);
						merger.Add(pdfParser, null);
					}
				}

				merger.Finish();
			}
		}
	}
}
