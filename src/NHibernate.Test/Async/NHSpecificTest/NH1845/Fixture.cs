﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;
namespace NHibernate.Test.NHSpecificTest.NH1845
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task LazyLoad_Initialize_AndEvictAsync()
		{
			Category category = new Category("parent");
			category.AddSubcategory(new Category("child"));
			await (SaveCategoryAsync(category));

			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				Category loaded = await (session.LoadAsync<Category>(category.Id));
				await (NHibernateUtil.InitializeAsync(loaded.Subcategories[0]));
				await (session.EvictAsync(loaded));
				await (transaction.CommitAsync());
				Assert.AreEqual("child", loaded.Subcategories[0].Name, "cannot access child");
			}
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				// first delete children
				await (session.CreateQuery("delete from Category where Parent != null").ExecuteUpdateAsync());
				// then the rest
				await (session.CreateQuery("delete from Category").ExecuteUpdateAsync());
				await (transaction.CommitAsync());
			}
		}

		private async Task SaveCategoryAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				await (session.SaveOrUpdateAsync(category, cancellationToken));
				await (transaction.CommitAsync(cancellationToken));
			}
		}
	}
}