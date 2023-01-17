﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.VersionTest.Db.MsSQL
{
	[TestFixture]
	public class LazyVersionTestAsync : TestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		protected override string[] Mappings
		{
			get { return new[] { "VersionTest.Db.MsSQL.ProductWithVersionAndLazyProperty.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		[Test(Description = "NH-3589")]
		public async System.Threading.Tasks.Task CanUseVersionOnEntityWithLazyPropertyAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				await (session.SaveAsync(new ProductWithVersionAndLazyProperty { Id = 1, Summary = "Testing, 1, 2, 3" }));

				await (session.FlushAsync());

				session.Clear();

				var p = await (session.GetAsync<ProductWithVersionAndLazyProperty>(1));

				p.Summary += ", 4!";

				await (session.FlushAsync());
			}
		}
	}
}
