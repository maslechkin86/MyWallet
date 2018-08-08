namespace ExcelLibrary.Test {
	using System.Data;

	#region Class: FakeExcelMaster

	public class FakeExcelMaster : IExcelMaster {

		#region Fields: Protected

		protected bool Visible;

		protected bool Open;

		protected bool NeedSave;

		protected string FileName;

		#endregion

		#region Fields: Public

		public DataSet ds;

		public DataTable dt;

		#endregion

		#region Constructors: Public

		public FakeExcelMaster() {
			Visible = false;
			Open = false;
			NeedSave = false;
			ds = new DataSet();
		}

		#endregion

		#region Methods: Public

		public void Close() {
			Open = false;
		}

		public void SaveAndClose() {
			NeedSave = false;
			Close();
		}

		public void AddRowAbove(int num) {
			dt.Rows.InsertAt(dt.NewRow(), num);
		}

		public void CopyRowAbove(int num) {
			//throw new NotImplementedException();
		}

		public void DeleteRow(int row) {
			dt.Rows.RemoveAt(row);
		}

		public void OpenBook(string file_name, string sheet_name) {
			FileName = file_name;
			dt = ds.Tables.Add(sheet_name);
			for(int i = 0; i < 15; i++)
				dt.Columns.Add();
			for(int i = 0; i < 20; i++)
				dt.Rows.Add(dt.NewRow());
			Open = true;
		}

		public void OpenBook(string file_name, int sheet_num) {
			OpenBook(file_name, "Sheet" + sheet_num.ToString());
		}

		public void ShowBook() {
			Visible = true;
		}

		public void WriteText(int row, int col, object sValue) {
			dt.Rows[row][col] = sValue.ToString();
		}

		public void WriteNumber(int row, int col, object sObj, string sNumberFormat) {
			dt.Rows[row][col] = "Number:" + sObj.ToString() + "Format:" + sNumberFormat;
		}

		public void WriteNumber(int row, int col, object sObj) {
			dt.Rows[row][col] = "Number:" + sObj.ToString();
		}

		public void WriteFormula(int row, int col, string sValue, string sNumberFormat) {
			dt.Rows[row][col] = "Formula:" + sValue + "Format:" + sNumberFormat;
		}

		public object GetCellDisplayValue(int row, int col) {
			return dt.Rows[row][col];
		}

		#endregion

	}

	#endregion

}
