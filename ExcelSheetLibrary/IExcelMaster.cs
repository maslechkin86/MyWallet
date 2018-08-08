namespace ExcelLibrary {

	#region Interface: IExcelMaster

	public interface IExcelMaster {

		#region Methods: Public

		void Close();

		void SaveAndClose();

		void AddRowAbove(int num);

		void CopyRowAbove(int num);

		void DeleteRow(int row);

		void OpenBook(string fileName, string sheetName);

		void OpenBook(string fileName, int sheetNum);

		void ShowBook();

		void WriteText(int row, int col, object sValue);

		void WriteNumber(int row, int col, object sObj, string sNumberFormat);

		void WriteNumber(int row, int col, object sObj);

		void WriteFormula(int row, int col, string sValue, string sNumberFormat);

		object GetCellDisplayValue(int row, int col);

		#endregion

	}

	#endregion

}
