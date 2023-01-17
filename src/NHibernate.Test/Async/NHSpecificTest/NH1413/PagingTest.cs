﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1413
{
	using System.Threading.Tasks;
	[TestFixture]
	public class PagingTestAsync : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			using(ISession session = OpenSession())
			using(ITransaction t = session.BeginTransaction())
			{
				await (session.PersistAsync(new Foo("Foo1", DateTime.Today.AddDays(5))));
				await (session.PersistAsync(new Foo("Foo2", DateTime.Today.AddDays(1))));
				await (session.PersistAsync(new Foo("Foo3", DateTime.Today.AddDays(3))));
				await (t.CommitAsync());
			}

			DetachedCriteria criteria = DetachedCriteria.For(typeof (Foo));
			criteria.Add(Restrictions.Like("Name", "Foo", MatchMode.Start));
			criteria.AddOrder(Order.Desc("Name"));
			criteria.AddOrder(Order.Asc("BirthDate"));
			using (ISession session = OpenSession())
			{
				ICriteria icriteria = criteria.GetExecutableCriteria(session);
				icriteria.SetFirstResult(0);
				icriteria.SetMaxResults(2);
				Assert.That(2, Is.EqualTo((await (icriteria.ListAsync<Foo>())).Count));
			}

			using (ISession session = OpenSession())
			using (ITransaction t = session.BeginTransaction())
			{
				await (session.DeleteAsync("from Foo"));
				await (t.CommitAsync());
			}
		}
	}
}