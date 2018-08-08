namespace MyWallet.Banking
{
	using System;
	using System.IO;

	public class FileReportSource : IReportSource {

		#region Methods: Public

		/// <inheritdoc />
		/// <summary>
		/// Gets the lines from specified file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Array of lines.</returns>
		public Array GetLines(string filePath) {
			return File.ReadAllLines(filePath);
		}

		#endregion

	}

}