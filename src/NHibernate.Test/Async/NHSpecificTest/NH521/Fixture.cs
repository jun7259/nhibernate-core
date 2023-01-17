﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH521
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return TestDialect.SupportsEmptyInsertsOrHasNonIdentityNativeGenerator;
		}

		[Test]
		public async Task AttachUninitProxyCausesInitAsync()
		{
			// First, save a ReferringEntity (which will have a ReferenceToLazyEntity)
			int id = 0;

			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				id = (int) await (session.SaveAsync(new ReferringEntity()));
				await (transaction.CommitAsync());
			}

			// Then, load the ReferringEntity (its ReferenceToLazyEntity will be uninit)
			LazyEntity uninitEntity = null;

			using (ISession session = OpenSession())
			{
				uninitEntity = (await (session.LoadAsync(typeof(ReferringEntity), id)) as ReferringEntity).ReferenceToLazyEntity;
			}

			Assert.IsFalse(
				NHibernateUtil.IsInitialized(uninitEntity),
				"The reference to a lazy entity is not unitialized at the loading of the referring entity.");

			// Finally, lock the uninitEntity and test if it gets initialized
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				await (session.LockAsync(uninitEntity, LockMode.None));

				Assert.IsFalse(
					NHibernateUtil.IsInitialized(uninitEntity),
					"session.Lock() causes initialization of an unitialized entity.");

				Assert.AreEqual(LockMode.None, session.GetCurrentLockMode(uninitEntity));

				Assert.IsFalse(
					NHibernateUtil.IsInitialized(uninitEntity),
					"session.GetCurrentLockMode() causes initialization of an unitialized entity.");

				await (session.DeleteAsync("from System.Object"));
				await (transaction.CommitAsync());
			}
		}
	}
}