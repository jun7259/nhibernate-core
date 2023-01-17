﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using System;

namespace NHibernate.Test.NHSpecificTest.NH3571Generic
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : TestCase
	{
		protected override string MappingsAssembly => "NHibernate.Test";

		protected override string[] Mappings => new[] {"NHSpecificTest.NH3571Generic.Mappings.hbm.xml"};

		/// <summary>
		/// push some data into the database
		/// Really functions as a save test also 
		/// </summary>
		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var tran = session.BeginTransaction())
			{
				var product = new Product {ProductId = "1"};
				product.Details.Properties["Name"] = "First Product";
				product.Details.Properties["Description"] = "First Description";

				session.Save(product);

				product = new Product {ProductId = "2"};
				product.Details.Properties["Name"] = "Second Product";
				product.Details.Properties["Description"] = "Second Description";

				session.Save(product);

				product = new Product {ProductId = "3"};
				product.Details.Properties["Name"] = "val";
				product.Details.Properties["Description"] = "val";

				session.Save(product);

				tran.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var tran = session.BeginTransaction())
			{
				session.CreateQuery("delete from Product").ExecuteUpdate();
				tran.Commit();
			}
		}

		[Test]
		public async Task CanQueryDynamicComponentInComponentAsync()
		{
			using (var session = OpenSession())
			{
				var product = await ((
					from p in session.Query<Product>()
					where (string) p.Details.Properties["Name"] == "First Product"
					select p
				).SingleAsync());

				Assert.That(product, Is.Not.Null);
				Assert.That(product.Details.Properties["Name"], Is.EqualTo("First Product"));
			}
		}	
		
		[Test(Description = "GH-3150")]
		public async Task CanQueryDynamicComponentInComponentByInterfaceAsync()
		{
			using (var session = OpenSession())
			{
				var product = await ((
					from p in session.Query<IProduct>()
					where (string) p.Details.Properties["Name"] == "First Product"
					select p
				).SingleAsync());

				Assert.That(product, Is.Not.Null);
				Assert.That(product.Details.Properties["Name"], Is.EqualTo("First Product"));
			}
		}

		[Test]
		public async Task MultipleQueriesShouldNotCacheAsync()
		{
			using (var session = OpenSession())
			{
				// Query by name
				var product1 = await ((
					from p in session.Query<Product>()
					where (string) p.Details.Properties["Name"] == "First Product"
					select p
				).SingleAsync());
				Assert.That(product1.ProductId, Is.EqualTo("1"));

				// Query by description (this test is to verify that the dictionary
				// index isn't cached from the query above.
				var product2 = await ((
					from p in session.Query<Product>()
					where (string) p.Details.Properties["Description"] == "Second Description"
					select p
				).SingleAsync());
				Assert.That(product2.ProductId, Is.EqualTo("2"));
			}
		}
	}
}