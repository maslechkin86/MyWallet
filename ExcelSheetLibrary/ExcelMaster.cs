namespace ExcelLibrary {
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Runtime.InteropServices;
	using System.Reflection;
	using Microsoft.Office.Interop.Excel;

	#region Class: ExcelMaster

	public class ExcelMaster : IDisposable, IExcelMasterFull, IExcelMaster {

		#region Constants: Public

		public const string TextFormat = "@";

		public const string NumberFormat = "#,##0.00";

		public const string DateFormat = "m/d/yyyy";

		#endregion

		#region Fields: Private

		private Application _application;

		private Workbooks _books;

		private Workbook _book;

		private Worksheet _sheet;

		private readonly object _misValue = Missing.Value;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelMaster"/> class.
		/// </summary>
		public ExcelMaster() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelMaster"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="sheetName">Name of the sheet.</param>
		public ExcelMaster(string fileName, string sheetName) {
			OpenBook(fileName, sheetName);
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ExcelMaster"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible {
			set { _application.Visible = value; }
			get { return _application.Visible; }
		}

		#endregion

		#region Methods: Private

		private string[] ConvertToStringArray(Array values) {
			var dimension = values.GetLength(1);
			var theArray = new string[dimension + 1];
			if(values.Rank == 1) {
			} else {
				for(int i = 1; i <= dimension; i++) {
					theArray[i - 1] = (values.GetValue(1, i) ?? "").ToString();
				}
			}
			return theArray;
		}

		private void ReleaseObject(object obj) {
			try {
				Marshal.ReleaseComObject(obj);
				obj = null;
			} catch(Exception) {
				obj = null;
			} finally {
				GC.Collect();
			}
		}

		private void NAR(object o) {
			try {
				Marshal.ReleaseComObject(o);
			} finally {
				o = null;
			}
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Opens the book.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="sheetName">Name of the sheet.</param>
		public void OpenBook(string fileName, string sheetName) {
			using(new ExcelUILanguageHelper()) {
				_application = new Application();
				_books = _application.Workbooks;
				_book = _books.Open(fileName, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue);
				_sheet = (Worksheet)_book.Worksheets[sheetName];
			}
		}
		/// <summary>
		/// Opens the book.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="sheetNumber">The sheet number.</param>
		public void OpenBook(string fileName, int sheetNumber) {
			using(new ExcelUILanguageHelper()) {
				_application = new Application();
				_books = _application.Workbooks;
				_book = _books.Open(fileName, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue);
				_sheet = (Worksheet)_book.Worksheets.Item[sheetNumber];
			}
		}

		/// <summary>
		/// Creates the new book.
		/// </summary>
		public void CreateNewBook() {
			using(new ExcelUILanguageHelper()) {
				_application = new Application { Visible = true };
				_books = _application.Workbooks;
				_book = _application.Workbooks.Add();
				_sheet = (Worksheet)_book.Worksheets[1];
			}
		}

		/// <summary>
		/// Shows the book.
		/// </summary>
		public void ShowBook() {
			_application.Visible = true;
		}

		/// <summary>
		/// Saves the book.
		/// </summary>
		public void SaveBook() {
			using(new ExcelUILanguageHelper()) {
				_book.Save();
			}
		}
		/// <summary>
		/// Closes the book.
		/// </summary>
		public void CloseBook() {
			using(new ExcelUILanguageHelper()) {
				_book.Close();
				_application.Quit();
			}
		}

		/// <summary>
		/// Runs the specified excel process.
		/// </summary>
		/// <param name="path">The path.</param>
		public static void Run(string path) {
			if(System.IO.File.Exists(path)) {
				var excelProcess = new Process { StartInfo = { FileName = "excel.exe", Arguments = "\"" + path + "\"" } };
				excelProcess.Start();
			}
		}

		public void SetSheet(string name) {
			using(new ExcelUILanguageHelper()) {
				_sheet = (Worksheet)_book.Worksheets[name];
			}
		}

		/// <summary>
		/// Runs the macros.
		/// </summary>
		/// <param name="macroName">Name of the macro.</param>
		public void RunMacros(string macroName) {
			using(new ExcelUILanguageHelper()) {
				_book.Application.Run(macroName, _misValue, _misValue, _misValue,
					_misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue,
					_misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue,
					_misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue);
			}
		}

		/// <summary>
		/// Selects the range.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="col">The col.</param>
		public void SelectRange(int row, int col) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).Select();
			}
		}

		/// <summary>
		/// Sets the range value.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="col">The col.</param>
		/// <param name="value">The array of values.</param>
		public void SetRangeValue(int row, int col, string[,] value) {
			using(new ExcelUILanguageHelper()) {
				var startCell = (Range)_sheet.Cells[row, col];
				var endCell = (Range)_sheet.Cells[row + value.GetLength(0) - 1, col + value.GetLength(1) - 1];
				var writeRange = _sheet.Range[startCell, endCell];
				writeRange.Value2 = value;
			}
		}

		public void SetRangeNumValue(int row, int col, object[,] value) {
			using(new ExcelUILanguageHelper()) {
				var startCell = (Range)_sheet.Cells[row, col];
				var endCell = (Range)_sheet.Cells[row + value.GetLength(0) - 1, col + value.GetLength(1) - 1];
				var writeRange = _sheet.Range[startCell, endCell];
				writeRange.Value2 = value;
			}
		}

		/// <summary>
		/// Gets the range value.
		/// </summary>
		/// <param name="leftTopCellLink">The left top cell link.</param>
		/// <param name="rightBottomCellLink">The right bottom cell link.</param>
		/// <returns>Range value System.String[].</returns>
		public string[] GetRangeValue(string leftTopCellLink, string rightBottomCellLink) {
			using(new ExcelUILanguageHelper()) {
				var range = _sheet.Range[leftTopCellLink, rightBottomCellLink];
				return ConvertToStringArray((Array)range.Cells.Value);
			}
		}

		/// <summary>
		/// Gets the range value.
		/// </summary>
		/// <param name="rowIndex1">The from row index.</param>
		/// <param name="colIndex1">The from col index.</param>
		/// <param name="rowIndex2">The to row index.</param>
		/// <param name="colIndex2">The to col index.</param>
		/// <returns>Range value System.String[].</returns>
		public string[] GetRangeValue(int rowIndex1, int colIndex1, int rowIndex2, int colIndex2) {
			return GetRangeValue(GetCellAddress(rowIndex1, colIndex1), GetCellAddress(rowIndex2, colIndex2));
		}

		/// <summary>
		/// Gets the cell address (R1C1).
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <returns>Cell link System.String.</returns>
		public static string GetCellAddress(int rowIndex, int colIndex) {
			int dividend = colIndex;
			string columnName = String.Empty;
			while(dividend > 0) {
				int modulo = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modulo).ToString(CultureInfo.InvariantCulture) + columnName;
				dividend = ((dividend - modulo) / 26);
			}
			string range = columnName + rowIndex.ToString(CultureInfo.InvariantCulture);
			return range;
		}

		/// <summary>
		/// Gets the range address (R1C1).
		/// </summary>
		/// <param name="rowIndex1">The from row index.</param>
		/// <param name="colIndex1">The from col index.</param>
		/// <param name="rowIndex2">The to row index.</param>
		/// <param name="colIndex2">The to col index.</param>
		/// <returns>Range link System.String[].</returns>
		public static string GetRangeAddress(int rowIndex1, int colIndex1, int rowIndex2, int colIndex2) {
			return string.Format("{0}:{1}", GetCellAddress(rowIndex1, colIndex1), GetCellAddress(rowIndex2, colIndex2));
		}

		/// <summary>
		/// Adds the row above.
		/// </summary>
		/// <param name="rowNumber">The row number.</param>
		public void AddRowAbove(int rowNumber) {
			using(new ExcelUILanguageHelper()) {
				Range range = ((Range)_sheet.Cells[rowNumber + 1, 1]).EntireRow;
				range.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
			}
		}

		/// <summary>
		/// Copies the row above.
		/// </summary>
		/// <param name="rowNumber">The row number.</param>
		public void CopyRowAbove(int rowNumber) {
			using(new ExcelUILanguageHelper()) {
				Range range1 = ((Range)_sheet.Cells[rowNumber, 1]).EntireRow;
				Range range2 = ((Range)_sheet.Cells[rowNumber + 1, 1]).EntireRow;
				range1.Copy(_misValue);
				range2.PasteSpecial(XlPasteType.xlPasteAll, XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
			}
		}

		/// <summary>
		/// Deletes the row.
		/// </summary>
		/// <param name="rowNumber">The row number.</param>
		public void DeleteRow(int rowNumber) {
			using(new ExcelUILanguageHelper()) {
				Range range = ((Range)_sheet.Cells[rowNumber, 1]).EntireRow;
				range.Delete(XlInsertShiftDirection.xlShiftDown);
			}
		}

		/// <summary>
		/// Creates the comment.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <param name="commentText">The comment text.</param>
		public void CreateComment(int rowIndex, int colIndex, string commentText) {
			using(new ExcelUILanguageHelper()) {
				var cell = (Range)_sheet.Cells[rowIndex, colIndex];
				cell.AddComment(commentText);
			}
		}

		/// <summary>
		/// Deletes the comment.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		public void DeleteComment(int rowIndex, int colIndex) {
			using(new ExcelUILanguageHelper()) {
				var cell = (Range)_sheet.Cells[rowIndex, colIndex];
				cell.ClearComments();
			}
		}

		/// <summary>
		/// Deletes the comment.
		/// </summary>
		/// <param name="rowIndex1">The from row index.</param>
		/// <param name="colIndex1">The from col index.</param>
		/// <param name="rowIndex2">The to row index.</param>
		/// <param name="colIndex2">The to col index.</param>
		public void DeleteComment(int rowIndex1, int colIndex1, int rowIndex2, int colIndex2) {
			using(new ExcelUILanguageHelper()) {
				var startCell = (Range)_sheet.Cells[rowIndex1, colIndex1];
				var endCell = (Range)_sheet.Cells[rowIndex2, colIndex2];
				var cell = _sheet.Range[startCell, endCell];
				cell.ClearComments();
			}
		}

		/// <summary>
		/// Gets the integer cell value.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <returns>Integer cell value System.Int32.</returns>
		public int GetIntCellValue(int rowIndex, int colIndex) {
			var value = (GetCellDisplayValue(rowIndex, colIndex) ?? "0").ToString();
			return int.Parse(value);
		}

		/// <summary>
		/// Gets the cell display value.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <returns>Cell display value System.Object.</returns>
		public object GetCellDisplayValue(int rowIndex, int colIndex) {
			using(new ExcelUILanguageHelper()) {
				return ((Range)_sheet.Cells[rowIndex, colIndex]).Value2;
			}
		}

		/// <summary>
		/// Gets the cell value.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <returns>Cell value System.Object.</returns>
		public object GetCellValue(int rowIndex, int colIndex) {
			using(new ExcelUILanguageHelper()) {
				return ((Range)_sheet.Cells[rowIndex, colIndex]).Value;
			}
		}

		/// <summary>
		/// Gets the cell text.
		/// </summary>
		/// <param name="rowIndex">Index of the row.</param>
		/// <param name="colIndex">Index of the col.</param>
		/// <returns>Cell text System.Object.</returns>
		public object GetCellText(int rowIndex, int colIndex) {
			using(new ExcelUILanguageHelper()) {
				return ((Range)_sheet.Cells[rowIndex, colIndex]).Text;
			}
		}

		public void SaveAndClose() {
			SaveBook();
			Close();
		}

		public void Close() {
			using(new ExcelUILanguageHelper()) {
				if(_book != null) {
					_book.Close();
				}
				if(_application != null) {
					_application.Quit();
				}
				ReleaseObject(_sheet);
				ReleaseObject(_book);
				ReleaseObject(_books);
				ReleaseObject(_application);
				NAR(_application);
				int i1 = Marshal.FinalReleaseComObject(_sheet);
				int i2 = Marshal.FinalReleaseComObject(_book);
				int i3 = Marshal.FinalReleaseComObject(_books);
				int i4 = Marshal.FinalReleaseComObject(_application);
				_sheet = null;
				_book = null;
				_books = null;
				_application = null;
				GC.GetTotalMemory(false);
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
				GC.GetTotalMemory(true);
			}
		}

		public void Dispose() {
			if(_application != null) {
				Marshal.ReleaseComObject(_application);
			}
			GC.GetTotalMemory(true);
		}

		public void WriteText(int row, int col, object sValue) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).NumberFormat = TextFormat;
				((Range)_sheet.Cells[row, col]).Value2 = (sValue ?? "").ToString();
			}
		}

		public void WriteDate(int row, int col, DateTime sValue) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).Value2 = sValue;
				((Range)_sheet.Cells[row, col]).NumberFormat = DateFormat;
			}
		}


		public void WriteDate(int row, int col, DateTime sValue, string format) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).Value2 = sValue;
				((Range)_sheet.Cells[row, col]).NumberFormat = format;
			}
		}

		public void Write(int row, int col, object sObj, string sNumberFormat) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).NumberFormat = sNumberFormat;
				((Range)_sheet.Cells[row, col]).Value2 = sObj;
			}
		}

		public void WriteNumber(int row, int col, object sObj, string sNumberFormat) {
			string sValue = (sObj ?? "").ToString();
			using(new ExcelUILanguageHelper()) {
				if((sValue == ""))
					sValue = "0";
				((Range)_sheet.Cells[row, col]).NumberFormat = sNumberFormat;
				((Range)_sheet.Cells[row, col]).Value2 = decimal.Parse(sValue.Replace(',', '.'));
			}
		}

		public void WriteNumber(int row, int col, object sObj) {
			string sValue = (sObj ?? "").ToString();
			using(new ExcelUILanguageHelper()) {
				if(sValue == "")
					sValue = "0";
				((Range)_sheet.Cells[row, col]).Value2 = decimal.Parse(sValue.Replace(',', '.'));
				((Range)_sheet.Cells[row, col]).NumberFormat = NumberFormat;
			}
		}

		public void WriteDate(int row, int col, object sObj, string sDateFormat) {
			using(new ExcelUILanguageHelper()) {
				if(sObj is DateTime) {
					var dt = (DateTime)sObj;
					((Range)_sheet.Cells[row, col]).Value = dt.ToOADate();
					((Range)_sheet.Cells[row, col]).NumberFormat = sDateFormat;
				}
			}
		}

		public void WriteFormula(int row, int col, string sValue) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).Formula = sValue;
			}
		}

		public void WriteFormula(int row, int col, string sValue, string sNumberFormat) {
			using(new ExcelUILanguageHelper()) {
				((Range)_sheet.Cells[row, col]).Formula = sValue;
				((Range)_sheet.Cells[row, col]).NumberFormat = sNumberFormat;
			}
		}

		public void WriteTable(int x1, int y1, Array data1) {
			using(new ExcelUILanguageHelper()) {
				var data2 = new string[100, 100];
				for(int i = 0; i < 99; i++)
					for(int j = 0; j < 99; j++)
						data2[i, j] = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
				_sheet.Range[_sheet.Cells[x1, y1], _sheet.Cells[100, 100]].Value2 = data2;
			}
		}

		public void WriteTable(string sValue, int r, int c) {
			using(new ExcelUILanguageHelper()) {
				if(sValue == null)
					return;
				sValue = sValue.Replace(',', '.').Replace('\n', ' ');
				var rows = sValue.Split('\r');
				var j = 0;
				var provider = CultureInfo.InvariantCulture;
				foreach(var row in rows) {
					string[] cells = row.Split('|');
					int i = 0;
					((Range)_sheet.Cells[r + j, c + i]).EntireRow.NumberFormat = j == 0 ? "MMMM yyyy" : "#,##0.00";

					foreach(var cell in cells) {
						if(cell.TrimStart() != "") {
							if((j == 0) & (cell.TrimStart() != ""))
								((Range)_sheet.Cells[r + j, c + i]).Value2 = DateTime.ParseExact(cell.TrimStart(), "dd.MM.yyyy", provider);
							if(j > 0) {
								((Range)_sheet.Cells[r + j, c + i]).Value2 = decimal.Parse(cell.TrimStart() != "" ? cell.TrimStart() : "0.00");
							}
						}
						i++;
					}
					j++;
				}
			}
		}

		//TODO Почистить. Реализовать дублированиые методы R1C1 адресации.
		//TODO Вписать методы установки границ в существующую идиологию 
		public static string GetExcelColumnName(int columnNumber) {
			var dividend = columnNumber;
			var columnName = String.Empty;
			while(dividend > 0) {
				int modulo = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modulo).ToString(CultureInfo.InvariantCulture) + columnName;
				dividend = (dividend - modulo) / 26;
			}
			return columnName;
		}
		// Установка одной границы ячейки
		public void SetCellBorder(string address, XlBordersIndex index, XlLineStyle lineStyle, XlBorderWeight weight) {
			object fRange = _sheet.GetType().InvokeMember(
					"Range", BindingFlags.GetProperty, null, _sheet, new object[] { address });

			object border = fRange.GetType().InvokeMember(
					"Borders", BindingFlags.GetProperty, null, fRange, new object[] { index });

			border.GetType().InvokeMember(
					"LineStyle", BindingFlags.SetProperty, null, border, new object[] { lineStyle });

			border.GetType().InvokeMember(
					"Weight", BindingFlags.SetProperty, null, border, new object[] { weight });

			border = null;
		}

		// Установка всех границ ячейки
		public void SetCellBorderAll(string address, XlBordersIndex[] index, XlLineStyle[] lineStyle, XlBorderWeight[] weight) {
			for(int i = 0; i <= index.Length - 1; i++)
				SetCellBorder(address, index[i], lineStyle[i], weight[i]);
		}

		#endregion

	}

	#endregion

}
