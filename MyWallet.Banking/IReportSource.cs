namespace MyWallet.Banking
{
	using System;

	public interface IReportSource
	{

		#region Methods: Public

		/// <summary>
		/// Gets the lines from specified file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Array of lines.</returns>
		Array GetLines(string filePath);

		#endregion

	}
}