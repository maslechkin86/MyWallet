namespace MyWallet.ConsoleApplication {
	using System;
	using System.Linq;
	using Domain.Concrete;
	using Domain.Entities;

	class Program {
		static void Main(string[] args) {
			using(var db = new MyWalletDbContext()) {
				//Console.Write("Enter a name for a new Account: ");
				//var name = Console.ReadLine();
				//var blog = new Account { Name = name };
				//db.Accounts.Add(blog);
				//db.SaveChanges();	 
				var query = from b in db.Accounts
										orderby b.Name
										select b;
				Console.WriteLine("All accounts in the database:");
				foreach(var item in query) {
					Console.WriteLine(item.Name);
				}
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
		}
	}
}
