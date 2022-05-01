﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Cfg;
using NHibernate.Exceptions;
using NUnit.Framework;

namespace NHibernate.Test.Generatedkeys.Identity
{
	using System.Threading.Tasks;
	[TestFixture]
	public class IdentityGeneratedKeysTestAsync : TestCase
	{
		protected override string[] Mappings
		{
			get { return new string[] { "Generatedkeys.Identity.MyEntity.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect.SupportsIdentityColumns;
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.GenerateStatistics, "true");
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.CreateQuery("delete from MyChild").ExecuteUpdate();
				s.CreateQuery("delete from MySibling").ExecuteUpdate();
				s.CreateQuery("delete from System.Object").ExecuteUpdate();
				t.Commit();
				s.Close();
			}
		}

		[Test]
		public async Task IdentityColumnGeneratedIdsAsync()
		{
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				var entity1 = new MyEntity("test");
				var id1 = (long) await (s.SaveAsync(entity1));
				var entity2 = new MyEntity("test2");
				var id2 = (long) await (s.SaveAsync(entity2));
				// As 0 may be a valid identity value, we check for returned ids being not the same when saving two entities.
				Assert.That(id1, Is.Not.EqualTo(id2), "identity column did not force immediate insert");
				Assert.That(id1, Is.EqualTo(entity1.Id));
				Assert.That(id2, Is.EqualTo(entity2.Id));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task PersistOutsideTransactionAsync()
		{
			var myEntity1 = new MyEntity("test-save");
			var myEntity2 = new MyEntity("test-persist");
			using (var s = OpenSession())
			{
				// first test save() which should force an immediate insert...
				var initialInsertCount = Sfi.Statistics.EntityInsertCount;
				var id = (long) await (s.SaveAsync(myEntity1));
				Assert.That(
					Sfi.Statistics.EntityInsertCount,
					Is.GreaterThan(initialInsertCount),
					"identity column did not force immediate insert");
				Assert.That(id, Is.EqualTo(myEntity1.Id));

				// next test persist() which should cause a delayed insert...
				initialInsertCount = Sfi.Statistics.EntityInsertCount;
				await (s.PersistAsync(myEntity2));
				Assert.AreEqual(
					initialInsertCount,
					Sfi.Statistics.EntityInsertCount,
					"persist on identity column not delayed");
				Assert.AreEqual(0, myEntity2.Id);

				// an explicit flush should cause execution of the delayed insertion
				await (s.FlushAsync());
				Assert.AreEqual(
					initialInsertCount + 1,
					Sfi.Statistics.EntityInsertCount,
					"delayed persist insert not executed on flush");
				s.Close();
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				await (s.DeleteAsync(myEntity1));
				await (s.DeleteAsync(myEntity2));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task PersistOutsideTransactionCascadedToNonInverseCollectionAsync()
		{
			long initialInsertCount = Sfi.Statistics.EntityInsertCount;
			using (var s = OpenSession())
			{
				MyEntity myEntity = new MyEntity("test-persist");
				myEntity.NonInverseChildren.Add(new MyChild("test-child-persist-non-inverse"));
				await (s.PersistAsync(myEntity));
				Assert.AreEqual(
					initialInsertCount,
					Sfi.Statistics.EntityInsertCount,
					"persist on identity column not delayed");
				Assert.AreEqual(0, myEntity.Id);
				await (s.FlushAsync());
				Assert.AreEqual(
					initialInsertCount + 2,
					Sfi.Statistics.EntityInsertCount,
					"delayed persist insert not executed on flush");
				s.Close();
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from MyChild"));
				await (s.DeleteAsync("from MyEntity"));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task PersistOutsideTransactionCascadedToInverseCollectionAsync()
		{
			long initialInsertCount = Sfi.Statistics.EntityInsertCount;
			using (var s = OpenSession())
			{
				MyEntity myEntity2 = new MyEntity("test-persist-2");
				MyChild child = new MyChild("test-child-persist-inverse");
				myEntity2.InverseChildren.Add(child);
				child.InverseParent = myEntity2;
				await (s.PersistAsync(myEntity2));
				Assert.AreEqual(
					initialInsertCount,
					Sfi.Statistics.EntityInsertCount,
					"persist on identity column not delayed");
				Assert.AreEqual(0, myEntity2.Id);
				await (s.FlushAsync());
				Assert.AreEqual(
					initialInsertCount + 2,
					Sfi.Statistics.EntityInsertCount,
					"delayed persist insert not executed on flush");
				s.Close();
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from MyChild"));
				await (s.DeleteAsync("from MyEntity"));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task PersistOutsideTransactionCascadedToManyToOneAsync()
		{
			long initialInsertCount = Sfi.Statistics.EntityInsertCount;
			using (var s = OpenSession())
			{
				MyEntity myEntity = new MyEntity("test-persist");
				myEntity.Sibling = new MySibling("test-persist-sibling-out");
				await (s.PersistAsync(myEntity));
				Assert.AreEqual(
					initialInsertCount,
					Sfi.Statistics.EntityInsertCount,
					"persist on identity column not delayed");
				Assert.AreEqual(0, myEntity.Id);
				await (s.FlushAsync());
				Assert.AreEqual(
					initialInsertCount + 2,
					Sfi.Statistics.EntityInsertCount,
					"delayed persist insert not executed on flush");
				s.Close();
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from MyEntity"));
				await (s.DeleteAsync("from MySibling"));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task PersistOutsideTransactionCascadedFromManyToOneAsync()
		{
			long initialInsertCount = Sfi.Statistics.EntityInsertCount;
			using (var s = OpenSession())
			{
				MyEntity myEntity2 = new MyEntity("test-persist-2");
				MySibling sibling = new MySibling("test-persist-sibling-in");
				sibling.Entity = myEntity2;
				await (s.PersistAsync(sibling));
				Assert.AreEqual(
					initialInsertCount,
					Sfi.Statistics.EntityInsertCount,
					"persist on identity column not delayed");
				Assert.AreEqual(0, myEntity2.Id);
				await (s.FlushAsync());
				Assert.AreEqual(
					initialInsertCount + 2,
					Sfi.Statistics.EntityInsertCount,
					"delayed persist insert not executed on flush");
				s.Close();
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from MySibling"));
				await (s.DeleteAsync("from MyEntity"));
				await (t.CommitAsync());
				s.Close();
			}
		}

		[Test]
		public async Task QueryOnPersistedEntityAsync([Values(FlushMode.Auto, FlushMode.Commit)] FlushMode flushMode)
		{
			var myEntity = new MyEntity("test-persist");
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.FlushMode = flushMode;

				var initialInsertCount = Sfi.Statistics.EntityInsertCount;
				await (s.PersistAsync(myEntity));
				Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(initialInsertCount),
					"persist on identity column not delayed");
				Assert.That(myEntity.Id, Is.Zero);

				var query = s.Query<MyChild>().Where(c => c.InverseParent == myEntity);
				switch (flushMode)
				{
					case FlushMode.Auto:
						Assert.That(query.ToList, Throws.Nothing);
						break;
					case FlushMode.Commit:
						Assert.That(query.ToList, Throws.Exception.TypeOf(typeof(UnresolvableObjectException)));
						break;
				}
				await (t.CommitAsync());
				s.Close();
			}
		}
	}
}
