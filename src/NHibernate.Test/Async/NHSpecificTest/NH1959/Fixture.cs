﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1959
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			using(ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from ClassB");
				s.Delete("from ClassA");
				tx.Commit();
			}
		}

		[Test]
		public async Task StartWithEmptyDoAddAndRemoveAsync()
		{
			ClassB b = new ClassB();
			ClassA a = new ClassA();
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.SaveAsync(a));
				await (s.SaveAsync(b));
				await (tx.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				ClassA loadedA = await (s.GetAsync<ClassA>(a.Id));
				ClassB loadedB = await (s.GetAsync<ClassB>(b.Id));
				loadedA.TheBag.Add(loadedB);
				loadedA.TheBag.Remove(loadedB);
				await (tx.CommitAsync());
			}

			using (ISession s = OpenSession())
				Assert.AreEqual(0, (await (s.GetAsync<ClassA>(a.Id))).TheBag.Count);
		}

		[Test]
		public async Task StartWithEmptyDoAddAndRemoveAtAsync()
		{
			ClassB b = new ClassB();
			ClassA a = new ClassA();
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.SaveAsync(a));
				await (s.SaveAsync(b));
				await (tx.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				ClassA loadedA = await (s.GetAsync<ClassA>(a.Id));
				ClassB loadedB = await (s.GetAsync<ClassB>(b.Id));
				loadedA.TheBag.Add(loadedB);
				loadedA.TheBag.RemoveAt(0);
				await (tx.CommitAsync());
			}

			using (ISession s = OpenSession())
				Assert.AreEqual(0, (await (s.GetAsync<ClassA>(a.Id))).TheBag.Count);
		}
	}
}