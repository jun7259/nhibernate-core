﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using log4net.Core;
using NHibernate.Dialect;
using NHibernate.Engine.Query;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq.Functions;
using NUnit.Framework;

namespace NHibernate.Test.Linq
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class WhereTestsAsync : LinqTestCase
	{
		[Test]
		public async Task NoWhereClauseAsync()
		{
			var query = await ((from user in db.Users
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(3));
		}

		[Test]
		public async Task OrWithTrueReducesTo1Eq1ClauseAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "ayende" || true
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(3));
		}

		[Test]
		public async Task AndWithTrueReducesTo1Eq0ClauseAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "ayende" && false
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task WhereWithConstantExpressionAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "ayende"
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task FirstElementWithWhereAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "ayende"
						 select user).FirstAsync());

			Assert.That(query.Name, Is.EqualTo("ayende"));
		}

		[Test]
		public void FirstElementWithQueryThatReturnsNoResultsAsync()
		{
			var users = from user in db.Users
						where user.Name == "xxx"
						select user;

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				{
					return users.FirstAsync();
				});
		}

		[Test]
		public async Task FirstOrDefaultElementWithQueryThatReturnsNoResultsAsync()
		{
			var user = await ((from u in db.Users
						where u.Name == "xxx"
						select u).FirstOrDefaultAsync());

			Assert.That(user, Is.Null);
		}

		[Test]
		public void SingleElementWithQueryThatReturnsNoResultsAsync()
		{
			var users = from user in db.Users
						where user.Name == "xxx"
						select user;

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				{
					return users.SingleAsync();
				});
		}

		[Test]
		public void SingleElementWithQueryThatReturnsMultipleResultsAsync()
		{
			var users = from user in db.Users
						select user;

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				{
					return users.SingleAsync();
				});
		}

		[Test]
		public async Task SingleOrDefaultElementWithQueryThatReturnsNoResultsAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "xxx"
						 select user).SingleOrDefaultAsync());

			Assert.That(query, Is.Null);
		}

		[Test]
		public async Task UsersRegisteredAtOrAfterY2KAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt >= new DateTime(2000, 1, 1)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersRegisteredAtOrAfterY2K_And_Before2001Async()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt >= new DateTime(2000, 1, 1) && user.RegisteredAt <= new DateTime(2001, 1, 1)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersByNameAndRegistrationDateAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name == "ayende" && user.RegisteredAt == new DateTime(2010, 06, 17)
						 select user).FirstOrDefaultAsync());

			Assert.That(query, Is.Not.Null);
			Assert.That(query.Name, Is.EqualTo("ayende"));
			Assert.That(query.RegisteredAt, Is.EqualTo(new DateTime(2010, 06, 17)));
		}

		[Test]
		public async Task UsersRegisteredAfterY2KAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt > new DateTime(2000, 1, 1)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersRegisteredAtOrBeforeY2KAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt <= new DateTime(2000, 1, 1)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersRegisteredBeforeY2KAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt < new DateTime(2000, 1, 1)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersRegisteredAtOrBeforeY2KAndNamedNHibernateAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt <= new DateTime(2000, 1, 1) && user.Name == "nhibernate"
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersRegisteredAtOrBeforeY2KOrNamedNHibernateAsync()
		{
			var query = await ((from user in db.Users
						 where user.RegisteredAt <= new DateTime(2000, 1, 1) || user.Name == "nhibernate"
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task TestDataContextAsync()
		{
			var query = from u in db.Users
						where u.Name == "ayende"
						select u;

			Assert.That(await (query.CountAsync()), Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithNullLoginDateAsync()
		{
			var query = await ((from user in db.Users
						 where user.LastLoginDate == null
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithNonNullLoginDateAsync()
		{
			var query = await ((from user in db.Users
						 where user.LastLoginDate != null
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithDynamicInvokedExpressionAsync()
		{
			//simulate dynamically created where clause
			Expression<Func<User, bool>> expr1 = u => u.Name == "ayende";
			Expression<Func<User, bool>> expr2 = u => u.Name == "rahien";

			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			var dynamicWhereClause = Expression.Lambda<Func<User, bool>>
				(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);

			var query = await (db.Users.Where(dynamicWhereClause).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithComponentPropertiesAsync()
		{
			var query = from user in db.Users
						where user.Component.Property1 == "test1"
						select user;

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithNestedComponentPropertiesAsync()
		{
			var query = from user in db.Users
						where user.Component.OtherComponent.OtherProperty1 == "othertest1"
						select user;

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithAssociatedEntityPropertiesAsync()
		{
			var query = from user in db.Users
						where user.Role.Name == "Admin" && user.Role.IsActive
						select new { user.Name, RoleName = user.Role.Name };

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithEntityPropertiesThreeLevelsDeepAsync()
		{
			var query = from user in db.Users
						where user.Role.Entity.Output != null
						select new
							{
								user.Name,
								RoleName = user.Role.Name,
								user.Role.Entity.Output
							};

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithoutRoleAsync()
		{
			var query = from user in db.Users
						where user.Role == null
						select new { user.Name, RoleName = user.Role.Name };

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithRoleAsync()
		{
			var query = from user in db.Users
						where user.Role != null
						select new { user.Name, RoleName = user.Role.Name };

			var list = await (query.ToListAsync());
			Assert.That(list.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithStringContainsAsync()
		{
			var query = await ((from user in db.Users
						 where user.Name.Contains("yend")
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithStringContainsAndNotNullNameComplicatedAsync()
		{
			// NH-3330
			// Queries in this pattern are apparently generated by 
			// e.g. WCF DS query:
			// http://.../Products()?$filter=substringof(&#39;123&#39;,Code)

			// ReSharper disable SimplifyConditionalTernaryExpression
			var query = db.Users
						  .Where(user =>
								 (user.Name == null ? null : (bool?)user.Name.Contains("123")) == null
									 ? false
									 : (user.Name == null ? null : (bool?)user.Name.Contains("123")).Value);
			// ReSharper restore SimplifyConditionalTernaryExpression

			await (query.ToListAsync());
		}

		[Test]
		[Description("NH-3337")]
		public async Task ProductWithDoubleStringContainsAndNotNullAsync()
		{
			// Consider this WCF DS query will fail:
			// http://.../Products()?$filter=substringof("23",Code) and substringof('2',Name)
			//
			// It will generate a LINQ expression similar to this:
			//
			//.Where(
			//   p =>
			//     ((p.Code == null ? (bool?)null : p.Code.Contains("23"))
			//     &&
			//     (p.Name == null ? (bool?)null : p.Name.Contains("2"))) == null
			//   ?
			//   false
			//   :
			//     ((p.Code == null ? (bool?)null : p.Code.Contains("23"))
			//     &&
			//     (p.Name == null ? (bool?)null : p.Name.Contains("2"))).Value
			//)
			//
			// In C# we cannot use && on nullable booleans, but it is allowed when building
			// expression trees, so we need to construct the query gradually.

			var nullAsNullableBool = Expression.Constant(null, typeof(bool?));
			var valueProperty = typeof (bool?).GetProperty("Value");

			Expression<Func<Product, bool>> quantityIsNull = x => x.QuantityPerUnit == null;
			Expression<Func<Product, bool>> nameIsNull = x => x.Name == null;

			Expression<Func<Product, bool?>> quantityContains23 = x => x.QuantityPerUnit.Contains("box");
			Expression<Func<Product, bool?>> nameContains2 = x => x.Name.Contains("Cha");

			var conjunction = Expression.AndAlso(
				Expression.Condition(quantityIsNull.Body, nullAsNullableBool, quantityContains23.Body),
				Expression.Condition(nameIsNull.Body, nullAsNullableBool, nameContains2.Body)
			);

			var condition = Expression.Condition(
				Expression.Equal(conjunction, Expression.Constant(null)),
				Expression.Constant(false),
				Expression.MakeMemberAccess(conjunction, valueProperty));

			var expr = Expression.Lambda<Func<Product, bool>>(condition, quantityIsNull.Parameters);

			var results = await (db.Products.Where(expr).ToListAsync());
			Assert.That(results, Has.Count.EqualTo(1));
		}

		[Test(Description = "NH-3261")]
		public async Task UsersWithStringContainsAndNotNullNameAsync()
		{
			// ReSharper disable SimplifyConditionalTernaryExpression
			var query = await ((from u in db.Users
						 where u.Name == null ? false : u.Name.Contains("yend")
						 select u).ToListAsync());
			// ReSharper restore SimplifyConditionalTernaryExpression

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test(Description = "NH-3261")]
		public async Task UsersWithStringContainsAndNotNullNameHQLAsync()
		{
			var users = await (session.CreateQuery("from User u where (case when u.Name is null then 'false' else (case when u.Name LIKE '%yend%' then 'true' else 'false' end) end) = 'true'").ListAsync<User>());

			Assert.That(users.Count, Is.EqualTo(1));
		}

		[Test]
		public void StringComparisonParamEmitsWarningAsync()
		{
			Assert.Multiple(
				async () =>
				{
					await (AssertStringComparisonWarningAsync(x => string.Compare(x.CustomerId, "ANATR", StringComparison.Ordinal) <= 0, 2));
					await (AssertStringComparisonWarningAsync(x => x.CustomerId.StartsWith("ANATR", StringComparison.Ordinal), 1));
					await (AssertStringComparisonWarningAsync(x => x.CustomerId.EndsWith("ANATR", StringComparison.Ordinal), 1));
					await (AssertStringComparisonWarningAsync(x => x.CustomerId.IndexOf("ANATR", StringComparison.Ordinal) == 0, 1));
					await (AssertStringComparisonWarningAsync(x => x.CustomerId.IndexOf("ANATR", 0, StringComparison.Ordinal) == 0, 1));
#if NETCOREAPP2_0_OR_GREATER
					await (AssertStringComparisonWarningAsync(x => x.CustomerId.Replace("AN", "XX", StringComparison.Ordinal) == "XXATR", 1));
#endif
				});
		}

		private async Task AssertStringComparisonWarningAsync(Expression<Func<Customer, bool>> whereParam, int expected, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var log = new LogSpy(typeof(BaseHqlGeneratorForMethod)))
			{
				var customers = await (session.Query<Customer>().Where(whereParam).ToListAsync(cancellationToken));

				Assert.That(customers, Has.Count.EqualTo(expected), whereParam.ToString);
				Assert.That(log.GetWholeLog(), Does.Contain($"parameter of type '{nameof(StringComparison)}' is ignored"), whereParam.ToString);
			}
		}

		[Test]
		public async Task UsersWithArrayContainsAsync()
		{
			var names = new[] { "ayende", "rahien" };

			var query = await ((from user in db.Users
						 where names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithListContainsAsync()
		{
			var names = new List<string> { "ayende", "rahien" };

			var query = await ((from user in db.Users
						 where names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test, Description("NH-3413")]
		public async Task UsersWithListContains_MutatingListDoesNotBreakOtherSessionsAsync()
		{
			{
				var names = new List<string> { "ayende", "rahien" };

				var query = await ((from user in db.Users
							 where names.Contains(user.Name)
							 select user).ToListAsync());

				Assert.AreEqual(2, query.Count); 

				names.Clear();
			}

			{
				var names = new List<string> { "ayende" };

				var query = await ((from user in db.Users
							 where names.Contains(user.Name)
							 select user).ToListAsync());

				// This line fails with Expected: 1 But was: 0
				// The SQL in NHProf shows that the where clause was executed as WHERE 1 = 0 as if names were empty
				Assert.AreEqual(1, query.Count);
			}
		}

		[Test]
		public async Task UsersWithEmptyList_NH2400Async()
		{
			var names = new List<string>();

			var query = await ((from user in db.Users
						 where names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task UsersWithEmptyEnumerableAsync()
		{
			var allNames = new List<string> { "ayende", "rahien" };
			var names = allNames.Where(n => n == "does not exist");

			var query = await ((from user in db.Users
						 where names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(0));
		}

		[Test]
		public void WhenTheSourceOfConstantIsICollectionThenNoThrowsAsync()
		{
			ICollection<string> names = new List<string> { "ayende", "rahien" };

			var query = (from user in db.Users
						 where names.Contains(user.Name)
						 select user);

			List<User> result = null;
			Assert.DoesNotThrowAsync(async () =>
				{
					result = await (query.ToListAsync());
				});

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void WhenTheSourceOfConstantIsIListThenNoThrowsAsync()
		{
			IList<string> names = new List<string> { "ayende", "rahien" };

			var query = (from user in db.Users
						 where names.Contains(user.Name)
						 select user);

			List<User> result = null;
			Assert.DoesNotThrowAsync(async () =>
				{
					result = await (query.ToListAsync());
				});

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task TimesheetsWithCollectionContainsAsync()
		{
			var entry = await (session.GetAsync<TimesheetEntry>(1));

			var timesheet = await ((from sheet in db.Timesheets
							 where sheet.Entries.Contains(entry)
							 select sheet).SingleAsync());

			Assert.That(timesheet.Id, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithStringNotContainsAsync()
		{
			var query = await ((from user in db.Users
						 where !user.Name.Contains("yend")
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task UsersWithArrayNotContainsAsync()
		{
			var names = new[] { "ayende", "rahien" };

			var query = await ((from user in db.Users
						 where !names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task UsersWithListNotContainsAsync()
		{
			var names = new List<string> { "ayende", "rahien" };

			var query = await ((from user in db.Users
						 where !names.Contains(user.Name)
						 select user).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task TimesheetsWithCollectionNotContainsAsync()
		{
			var entry = await (session.GetAsync<TimesheetEntry>(1));

			var query = await ((from sheet in db.Timesheets
						 where !sheet.Entries.Contains(entry)
						 select sheet).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task TimesheetsWithEnumerableContainsAsync()
		{
			var user = await (session.GetAsync<User>(1));

			var query = await ((from sheet in db.Timesheets
						 where sheet.Users.Contains(user)
						 select sheet).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task TimesheetsWithEnumerableContainsOnSelectAsync()
		{
			if (Dialect is MsSqlCeDialect)
				Assert.Ignore("Dialect is not supported");

			var value = (EnumStoredAsInt32) 1000;
			var query = await ((from sheet in db.Timesheets
			             where sheet.Users.Select(x => x.NullableEnum2 ?? value).Contains(value)
			             select sheet).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task TimesheetsWithProjectionInSubqueryAsync()
		{
			if (Dialect is MsSqlCeDialect)
				Assert.Ignore("Dialect is not supported");

			var query = await ((from sheet in db.Timesheets
						 where sheet.Users.Select(x => new { Id = x.Id, Name = x.Name }).Any(x => x.Id == 1)
						 select sheet).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task ContainsSubqueryWithCoalesceStringEnumSelectAsync()
		{
			if (Dialect is MsSqlCeDialect || Dialect is SQLiteDialect)
				Assert.Ignore("Dialect is not supported");

			var results =
				await (db.Timesheets.Where(
					  o =>
						  o.Users
						   .Where(u => u.Id != 0.MappedAs(NHibernateUtil.Int32))
						   .Select(u => u.Name == u.Name ? u.Enum1 : u.NullableEnum1.Value)
						   .Contains(EnumStoredAsString.Small))
				  .ToListAsync());

			Assert.That(results.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task SearchOnObjectTypeWithExtensionMethodAsync()
		{
			var query = await ((from o in session.Query<Animal>()
						 select o).OfType<Dog>().ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test(Description = "NH-2206")]
		public async Task SearchOnObjectTypeUpCastWithExtensionMethodAsync()
		{
			var query = await ((from o in session.Query<Dog>()
						 select o).Cast<Animal>().ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test(Description = "NH-2206")]
		public async Task SearchOnObjectTypeCastAsync()
		{
			var query = await ((from Dog o in session.Query<Dog>()
						 select o).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task SearchOnObjectTypeWithIsKeywordAsync()
		{
			var query = await ((from o in session.Query<Animal>()
						 where o is Dog
						 select o).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test(Description = "NH-3946")]
		public async Task PolymorphicSearchOnObjectTypeWithIsKeywordAsync()
		{
			var query = await ((from o in session.Query<Animal>()
						 where o is Mammal
						 select o).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(3));
		}

		[Test(Description = "NH-3845")]
		public async Task PolymorphicSearchOnObjectTypeWithOfTypeAsync()
		{
			var query = await (session.Query<Animal>().OfType<Mammal>().ToListAsync());

			Assert.That(query.Count, Is.EqualTo(3));
		}

		[Test]
		public async Task BitwiseQueryAsync()
		{
			var featureSet = FeatureSet.HasMore;
			var query = await ((from o in session.Query<User>()
						 where (o.Features & featureSet) == featureSet
						 select o).ToListAsync());

			Assert.That(query, Is.Not.Null);
		}

		[Test]
		public async Task BitwiseQuery2Async()
		{
			var featureSet = FeatureSet.HasAll;
			var query = await ((from o in session.Query<User>()
						 where (o.Features & featureSet) == featureSet
						 select o).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task BitwiseQuery3Async()
		{
			var featureSet = FeatureSet.HasThat;
			var query = await ((
				from o in session.Query<User>()
				// When converted to SQL, "undue" parenthesis are stripped out. For most DB, binary operators have same precedence,
				// causing "((o.Features | featureSet) & featureSet)" to be equivalent to "o.Features | featureSet & featureSet"
				// But for MySql, & take precedence on |, wrecking the test for it. So it is needed to write the test in a way
				// such as the parenthesis will be preserved.
				where (featureSet & (o.Features | featureSet)) == featureSet
				select o).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(3));
		}

		[Test(Description = "NH-2375")]
		public async Task OfTypeWithWhereAndProjectionAsync()
		{
			await ((from a in session.Query<Animal>().OfType<Cat>()
			 where a.Pregnant
			 select a.Id).FirstOrDefaultAsync());
		}

		[Test(Description = "NH-2375")]
		public async Task OfTypeWithWhereAsync()
		{
			await ((from a in session.Query<Animal>().OfType<Cat>()
			 where a.Pregnant
			 select a).FirstOrDefaultAsync());
		}

		[Test(Description = "NH-3009")]
		public async Task TimeSheetsWithSamePredicateTwoTimesAsync()
		{
			Expression<Func<Timesheet, bool>> predicate = timesheet => timesheet.Entries.Any(e => e.Id != 1);

			var query = await (db.Timesheets
						  .Where(predicate)
						  .Where(predicate)
						  .ToListAsync());

			Assert.That(query.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task AnimalsWithFathersSerialNumberListContainsAsync()
		{
			var serialNumbers = new List<string> { "5678", "789" };
			var query = await ((from animal in db.Animals
						 where animal.Father != null && serialNumbers.Contains(animal.Father.SerialNumber)
						 select animal).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task AnimalsWithFathersSerialNumberListContainsWithLocalVariableAsync()
		{
			var serialNumbers = new List<string> { "5678", "789" };
			var query = await ((from animal in db.Animals
						 let father = animal.Father
						 where father != null && serialNumbers.Contains(father.SerialNumber)
						 select animal).ToListAsync());

			Assert.That(query.Count, Is.EqualTo(1));
		}

		[Test(Description = "NH-3366")]
		public async Task CanUseCompareInQueryWithNonConstantZeroAsync()
		{
			using (var ls = new SqlLogSpy())
			{
				// Comparison with p.ProductId is somewhat non-sensical - the point
				// is that it should work also when it's not something that can be reduced
				// to a constant zero.
				var result = await (db.Products.Where(p => string.Compare(p.Name.ToLower(), "konbu") < (p.ProductId - p.ProductId)).ToListAsync());

				Assert.That(result, Has.Count.EqualTo(30));

				// This should generate SQL with some nested case expressions - it should not be
				// simplified.
				string wholeLog = ls.GetWholeLog();
				Assert.That(wholeLog, Does.Contain("when lower(product0_.ProductName)="));
			}
		}

		[Test(Description = "NH-3366")]
		[TestCaseSource(typeof(WhereTestsAsync), nameof(CanUseCompareInQueryDataSource))]
		public async Task CanUseCompareInQueryAsync(Expression<Func<Product, bool>> expression, int expectedCount, bool expectCase)
		{
			using (var ls = new SqlLogSpy())
			{
				var result = await (db.Products.Where(expression).ToListAsync());

				Assert.That(result, Has.Count.EqualTo(expectedCount));

				string wholeLog = ls.GetWholeLog();
				Assert.That(wholeLog, expectCase ? Does.Contain("case") : Does.Not.Contain("case"));
			}
		}

		[Test(Description = "NH-3665")]
		public async Task SelectOnCollectionReturnsResultAsync()
		{
			var result = await (db.Animals.Select(x => new
			{
				x.Children
			}).FirstOrDefaultAsync());

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Children, Is.Not.Empty);
		}

		[Test(Description = "GH-1556")]
		public async Task ContainsOnPersistedCollectionAsync()
		{
			var animal = await (session.Query<Animal>().SingleAsync(a => a.SerialNumber == "123"));

			var result = await (session.Query<Animal>()
			                    .Where(e => animal.Children.Contains(e.Father))
			                    .OrderBy(e => e.Id)
			                    .FirstOrDefaultAsync());
			Assert.That(result, Is.Not.Null);
			Assert.That(result.SerialNumber, Is.EqualTo("1121"));
		}

		[Test]
		public async Task CanCompareAggregateResultAsync()
		{
			if (!Dialect.SupportsScalarSubSelects)
			{
				Assert.Ignore(Dialect.GetType().Name + " does not support scalar sub-queries");
			}

			await (session.Query<Customer>()
			       .Select(o => new AggregateDate { Id = o.CustomerId, MaxDate = o.Orders.Max(l => l.RequiredOrderDate)})
			       .Where(o => o.MaxDate <= DateTime.Today && o.MaxDate >= DateTime.Today)
			       .ToListAsync());
		}

		private class AggregateDate
		{
			public string Id { get; set; }

			public DateTime? MaxDate { get; set; }
		}

		private static List<object[]> CanUseCompareInQueryDataSource()
		{
			return new List<object[]>
				{
					// The full set of operators over strings.
					TestRow(p => p.Name.ToLower().CompareTo("konbu") < 0, 30, false),
					TestRow(p => p.Name.ToLower().CompareTo("konbu") <= 0, 31, false),
					TestRow(p => p.Name.ToLower().CompareTo("konbu") == 0, 1, false),
					TestRow(p => p.Name.ToLower().CompareTo("konbu") != 0, 76, false),
					TestRow(p => p.Name.ToLower().CompareTo("konbu") >= 0, 47, false),
					TestRow(p => p.Name.ToLower().CompareTo("konbu") > 0, 46, false),

					// Some of the above with the constant zero as first operator (needs to inverse the operator).
					TestRow(p => 0 <= p.Name.ToLower().CompareTo("konbu"), 47, false),
					TestRow(p => 0 == p.Name.ToLower().CompareTo("konbu"), 1, false),
					TestRow(p => 0 > p.Name.ToLower().CompareTo("konbu"), 30, false),

					// Over integers.
					TestRow(p => p.UnitsInStock.CompareTo(13) < 0, 15, false),
					TestRow(p => p.UnitsInStock.CompareTo(13) >= 0, 62, false),

					// Over floats. (Always include a tolerance with them)
					TestRow(p => p.ShippingWeight.CompareTo(4.980001f) <= 0, 17, false),

					// Over nullable decimals.
					TestRow(p => p.UnitPrice.Value.CompareTo((decimal) 14.00) <= 0, 24, false),
					TestRow(p => 0 >= p.UnitPrice.Value.CompareTo((decimal) 14.00), 24, false),

					// Over nullable DateTime.
					TestRow(p => p.OrderLines.Any(o => o.Order.ShippingDate.Value.CompareTo(DateTime.Now) <= 0), 77, false),
					TestRow(p => p.OrderLines.Any(o => 0 >= o.Order.ShippingDate.Value.CompareTo(DateTime.Now)), 77, false),
				};
		}

		private static object[] TestRow(Expression<Func<Product, bool>> expression, int expectedCount, bool expectCase)
		{
			return new object[]
				{
					expression, expectedCount, expectCase
				};
		}
	}
}
