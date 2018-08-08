namespace MyWallet.Banking {
	using System;
	using System.Text;
	using System.Text.RegularExpressions;
	using iTextSharp.text.pdf;
	using iTextSharp.text.pdf.parser;

	public class PdfFileReportSource : IReportSource {

		private string GetTextFromPDF(string filePath) {
			var text = new StringBuilder();
			using (var reader = new PdfReader(filePath)) {
				for (var page = 1; page <= reader.NumberOfPages; page++) {
					text.Append(PdfTextExtractor.GetTextFromPage(reader, page));
				}
			}
			return text.ToString();
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the lines from specified file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Array of lines.</returns>
		public Array GetLines(string filePath) {
			var source = GetTextFromPDF(filePath);
			var lines = Regex.Split(source, "\r\n|\r|\n");
			return lines;
		}

	}

}
