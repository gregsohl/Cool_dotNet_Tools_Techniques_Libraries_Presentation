using System;
using System.IO;
using Gios.PDF.SplitMerge;

namespace SamplePdfSplit
{
	class Program
	{
		static void Main(string[] args)
		{
			string inputFile = args[0];
			string outputFile = args[1];
			int pageNumber = Int32.Parse(args[2]);

			string target = Path.Combine(Path.GetDirectoryName(outputFile),
				Path.GetFileNameWithoutExtension(outputFile) + " - " + pageNumber.ToString("00000") + ".pdf");

			using (var inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				var pdfParser = PdfParser.Parse(inputStream);

				using (Stream s = new FileStream(target, FileMode.Create, FileAccess.Write))
				{
					var pdfMerger = new PdfMerger(s);
					pdfMerger = new PdfMerger(s);
					pdfMerger.Add(pdfParser, new int[] { pageNumber });
					pdfMerger.Finish();
				}
			}
		}
	}
}
