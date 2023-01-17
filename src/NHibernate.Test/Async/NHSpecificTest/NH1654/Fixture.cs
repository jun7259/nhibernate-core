﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1654
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task TestAsync()
		{
			int employeeId;
			using (ISession sess = OpenSession())
			using (ITransaction tx = sess.BeginTransaction())
			{
				var emp = new Employee();
				emp.Id = 1;
				emp.FirstName = "John";

				await (sess.SaveAsync(emp));

				await (tx.CommitAsync());

				employeeId = emp.Id;
			}

			using (ISession sess = OpenSession())
			using (ITransaction tx = sess.BeginTransaction())
			{
				var load = await (sess.LoadAsync<Employee>(employeeId));
				Assert.AreEqual("John", load.FirstNameFormula);

				await (tx.CommitAsync());
			}

			using (ISession sess = OpenSession())
			using (ITransaction tx = sess.BeginTransaction())
			{
				await (sess.DeleteAsync("from Employee"));
				await (tx.CommitAsync());
			}
		}
	}
}