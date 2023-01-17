﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using NUnit.Framework;
using NHibernate.Linq;

namespace NHibernate.Test.Linq
{
	using System.Threading.Tasks;

	[TestFixture]
	public class CustomExtensionsExampleAsync : LinqTestCase
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			configuration.LinqToHqlGeneratorsRegistry<MyLinqToHqlGeneratorsRegistry>();
		}

		[Test]
		public async Task CanUseObjectEqualsAsync()
		{
			var users = await (db.Users.Where(o => ((object) EnumStoredAsString.Medium).Equals(o.NullableEnum1)).ToListAsync());
			Assert.That(users.Count, Is.EqualTo(2));
			Assert.That(users.All(c => c.NullableEnum1 == EnumStoredAsString.Medium), Is.True);
		}

		[Test(Description = "GH-2963")]
		public async Task CanUseComparisonWithExtensionOnMappedPropertyAsync()
		{
			if (!TestDialect.SupportsTime)
			{
				Assert.Ignore("Time type is not supported");
			}

			var time = DateTime.UtcNow.GetTime();
			await (db.Users.Where(u => u.RegisteredAt.GetTime() > time).Select(u => u.Id).ToListAsync());
		}

		[Test]
		public async Task CanUseMyCustomExtensionAsync()
		{
			var contacts = await ((from c in db.Customers where c.ContactName.IsLike("%Thomas%") select c).ToListAsync());
			Assert.That(contacts.Count, Is.GreaterThan(0));
			Assert.That(contacts.All(c => c.ContactName.Contains("Thomas")), Is.True);
		}
	}
}