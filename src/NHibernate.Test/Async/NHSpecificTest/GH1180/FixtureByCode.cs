﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH1180
{
	using System.Threading.Tasks;
	//NH-3847
	[TestFixture]
	public class ByCodeFixtureAsync : TestCaseMappingByCode
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();

			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name, m => {m.Type(NHibernateUtil.AnsiString); m.Length(5); });
				rc.Property(x => x.Amount, m => { m.Precision(8); m.Scale(2); });
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.CreateQuery("delete from System.Object").ExecuteUpdate();
				transaction.Commit();
			}
		}

		[Test]
		public async Task StringTypesAsync()
		{
			var whenFalse =
				Dialect is Oracle8iDialect
				//Most dialects allow to return DbType.String and DbType.AnsiString in case statement
				//But Oracle throws 'ORA-12704: character set mismatch' 
					? Projections.Constant("otherstring", NHibernateUtil.AnsiString)
					: Projections.Constant("otherstring");
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				// data
				await (session.SaveAsync(new Entity {Name = "Alpha"}));
				await (session.SaveAsync(new Entity {Name = "Beta"}));
				await (session.SaveAsync(new Entity {Name = "Gamma"}));

				await (transaction.CommitAsync());
			}

			// whenTrue is constant, whenFalse is property
			using (var session = OpenSession())
			{
				ICriteria tagCriteria = session.CreateCriteria(typeof(Entity));

				var conditionalProjection = Projections.Conditional(
					Restrictions.Not(
						Restrictions.Like(nameof(Entity.Name), "B%")),
					//Property - ansi string length 5; contstant - string, length 10
					whenFalse,
					Projections.Property(nameof(Entity.Name)));
				tagCriteria.SetProjection(conditionalProjection);

				// run query
				var results = await (tagCriteria.ListAsync());

				Assert.That(results, Is.EquivalentTo(new[] {"otherstring", "Beta", "otherstring"}));
			}

			// whenTrue is property, whenFalse is constant
			using (var session = OpenSession())
			{
				ICriteria tagCriteria = session.CreateCriteria(typeof(Entity));

				var conditionalProjection = Projections.Conditional(
					Restrictions.Like(nameof(Entity.Name), "B%"),
					Projections.Property(nameof(Entity.Name)),
					whenFalse);
				tagCriteria.SetProjection(conditionalProjection);

				// run query
				var results = await (tagCriteria.ListAsync());

				Assert.That(results, Is.EquivalentTo(new[] {"otherstring", "Beta", "otherstring"}));
			}
		}

		[Test]
		public async Task DecimalTypesAsync()
		{
			//On some dialects (SQLite) Scale mapping is ignored
			var propertyResult = TestDialect.HasBrokenDecimalType ? 42.131m : 42.13m;
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				await (session.SaveAsync(new Entity {Amount = 3.141m}));
				await (session.SaveAsync(new Entity {Amount = 42.131m}));
				await (session.SaveAsync(new Entity {Amount = 17.991m}));

				await (transaction.CommitAsync());
			}

			// whenTrue is constant, whenFalse is property
			using (var session = OpenSession())
			{
				ICriteria tagCriteria = session.CreateCriteria(typeof(Entity));

				var conditionalProjection = Projections.Conditional(
					Restrictions.Not(
						Restrictions.Ge(nameof(Entity.Amount), 20m)),
					//Property scale is 2, make sure constant scale 3 is not lost
					Projections.Constant(20.123m),
					Projections.Property(nameof(Entity.Amount)));
				tagCriteria.SetProjection(conditionalProjection);

				// run query
				var results = await (tagCriteria.ListAsync());

				Assert.That(results, Is.EquivalentTo(new[] {20.123m, propertyResult, 20.123m}));
			}

			// whenTrue is property, whenFalse is constant
			using (var session = OpenSession())
			{
				ICriteria tagCriteria = session.CreateCriteria(typeof(Entity));

				var conditionalProjection = Projections.Conditional(
					Restrictions.Ge(nameof(Entity.Amount), 20m),
					Projections.Property(nameof(Entity.Amount)),
					Projections.Constant(20.123m));
				tagCriteria.SetProjection(conditionalProjection);

				// run query
				var results = await (tagCriteria.ListAsync());

				Assert.That(results, Is.EquivalentTo(new[] {20.123m, propertyResult, 20.123m}));
			}
		}
	}
}
