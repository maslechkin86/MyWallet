namespace MyWallet.Domain.Tests.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;

	#region Class: FakeDbSet

	public class FakeDbSet<T> : IDbSet<T> where T : class
	{

		#region Fields: Private

		private readonly IQueryable _query;

		#endregion

		#region Constructors: Public

		public FakeDbSet() {
			Local = new ObservableCollection<T>();
			_query = Local.AsQueryable();
		}

		#endregion

		#region Properties: Private

		Type IQueryable.ElementType => _query.ElementType;

		Expression IQueryable.Expression => _query.Expression;

		IQueryProvider IQueryable.Provider => _query.Provider;

		#endregion

		#region Properties: Public

		public ObservableCollection<T> Local { get; }

		#endregion

		#region Methods: Private

		IEnumerator IEnumerable.GetEnumerator() {
			return Local.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return Local.GetEnumerator();
		}

		#endregion

		#region Methods: Public

		public virtual T Find(params object[] keyValues) {
			throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
		}

		public T Add(T item) {
			Local.Add(item);
			return item;
		}

		public T Remove(T item) {
			Local.Remove(item);
			return item;
		}

		public T Attach(T item) {
			Local.Add(item);
			return item;
		}

		public T Detach(T item) {
			Local.Remove(item);
			return item;
		}

		public T Create() {
			return Activator.CreateInstance<T>();
		}
		public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T {
			return Activator.CreateInstance<TDerivedEntity>();
		}

		#endregion

	}

	#endregion

}