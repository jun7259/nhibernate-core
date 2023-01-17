﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH1413
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class ByCodeFixtureAsync : TestCaseMappingByCode
	{
		private int _parentId;

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<EntityParent>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.Identity));
				rc.Property(x => x.Name);
				rc.Bag(x => x.Children, m =>
				{
					m.Cascade(Mapping.ByCode.Cascade.All);
					m.Inverse(true);
				}, a => a.OneToMany()
				);
			});

			mapper.Class<EntityChild>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.Identity));
				rc.Property(x => x.Name);
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var parent = new EntityParent
				{
					Name = "InitialParent",
				};
				session.Save(parent);

				session.Flush();
				transaction.Commit();
				_parentId = parent.Id;
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Delete("from System.Object");

				session.Flush();
				transaction.Commit();
			}
		}

		[Theory]
		public async Task SessionIsDirtyShouldNotTriggerCascadeSavingAsync(bool beginTransaction)
		{
			Sfi.Statistics.IsStatisticsEnabled = true;
			using (var session = OpenSession())
			using (beginTransaction ? session.BeginTransaction() : null)
			{
				var parent = await (GetParentAsync(session));
				var entityChild = new EntityChild
				{
					Name = "NewListElem"
				};

				//parent.Children is cascaded
				parent.Children.Add(entityChild);

				Sfi.Statistics.Clear();
				var isDirty = await (session.IsDirtyAsync());

				Assert.That(Sfi.Statistics.EntityInsertCount, Is.EqualTo(0), "Dirty has triggered an insert");
				Assert.That(isDirty, "ISession.IsDirty() call should return true.");
				if (Dialect.SupportsIdentityColumns)
				{
					Assert.That(
						entityChild.Id,
						Is.EqualTo(0),
						"Transient objects should not be saved by ISession.IsDirty() call (expected 0 as Id)");
				}
			}
		}

		private Task<EntityParent> GetParentAsync(ISession session, CancellationToken cancellationToken = default(CancellationToken))
		{
			return session.GetAsync<EntityParent>(_parentId, cancellationToken);
		}
	}
}