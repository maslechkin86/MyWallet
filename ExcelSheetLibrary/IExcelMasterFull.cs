namespace ExcelLibrary {
	using System;

	#region Interface: IExcelMasterFull

	public interface IExcelMasterFull {

		#region Methods: Public

		bool Visible { get; set; }

		void Close();

		void SaveAndClose();

		void OpenBook(string fileName, string sheetName);

		void OpenBook(string fileName, int sheetNum);

		void CreateNewBook();

		void ShowBook();

		void SaveBook();

		void CloseBook();

		void SetSheet(string name);

		void RunMacros(string name);

		void AddRowAbove(int num);

		void CopyRowAbove(int num);

		void DeleteRow(int row);

		void WriteText(int row, int col, object sValue);

		void WriteDate(int row, int col, DateTime sValue);

		void WriteDate(int row, int col, DateTime sValue, string format);

		void Write(int row, int col, object sObj, string sNumberFormat);

		void WriteNumber(int row, int col, object sObj, string sNumberFormat);

		void WriteNumber(int row, int col, object sObj);

		void WriteDate(int row, int col, object sObj, string sDateFormat);

		void WriteFormula(int row, int col, string sValue);

		void WriteFormula(int row, int col, string sValue, string sNumberFormat);

		#endregion

	}

	#endregion

}
