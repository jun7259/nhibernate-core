﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using log4net;
using log4net.Core;
using NUnit.Framework;

namespace NHibernate.Test.IdTest
{
	using System.Threading.Tasks;
	[TestFixture]
	public class AssignedFixtureAsync : IdFixtureBase
	{
		private string[] GetAssignedIdentifierWarnings(LogSpy ls)
		{
			List<string> warnings = new List<string>();

			foreach (string logEntry in ls.GetWholeLog().Split('\n'))
				if (logEntry.Contains("Unable to determine if") && logEntry.Contains("is transient or detached"))
					warnings.Add(logEntry);

			return warnings.ToArray();
		}

		protected override string TypeName
		{
			get { return "Assigned"; }
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.CreateQuery("delete from Child").ExecuteUpdate();
				s.CreateQuery("delete from Parent").ExecuteUpdate();
				t.Commit();
			}
		}

		[Test]
		public async Task SaveOrUpdate_SaveAsync()
		{
			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Children = new List<Child>(),
					};

				await (s.SaveOrUpdateAsync(parent));
				await (t.CommitAsync());

				long actual = await (s.CreateQuery("select count(p) from Parent p").UniqueResultAsync<long>());
				Assert.That(actual, Is.EqualTo(1));

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(1));
				Assert.IsTrue(warnings[0].Contains("parent"));
			}
		}

		[Test]
		public async Task SaveNoWarningAsync()
		{
			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Children = new List<Child>(),
					};

				await (s.SaveAsync(parent));
				await (t.CommitAsync());

				long actual = await (s.CreateQuery("select count(p) from Parent p").UniqueResultAsync<long>());
				Assert.That(actual, Is.EqualTo(1));

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(0));
			}
		}

		[Test]
		public async Task SaveOrUpdate_UpdateAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				await (s.SaveAsync(new Parent() { Id = "parent", Name = "before" }));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Name = "after",
					};

				await (s.SaveOrUpdateAsync(parent));
				await (t.CommitAsync());

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(1));
				Assert.IsTrue(warnings[0].Contains("parent"));
			}

			using (ISession s = OpenSession())
			{
				Parent parent = await (s.CreateQuery("from Parent").UniqueResultAsync<Parent>());
				Assert.That(parent.Name, Is.EqualTo("after"));
			}
		}

		[Test]
		public async Task UpdateNoWarningAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				await (s.SaveAsync(new Parent() { Id = "parent", Name = "before" }));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Name = "after",
					};

				await (s.UpdateAsync(parent));
				await (t.CommitAsync());

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(0));
			}

			using (ISession s = OpenSession())
			{
				Parent parent = await (s.CreateQuery("from Parent").UniqueResultAsync<Parent>());
				Assert.That(parent.Name, Is.EqualTo("after"));
			}
		}

		[Test]
		public async Task InsertCascadeAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				await (s.SaveAsync(new Child() { Id = "detachedChild" }));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Children = new List<Child>(),
					};

				parent.Children.Add(new Child() { Id = "detachedChild", Parent = parent });
				parent.Children.Add(new Child() { Id = "transientChild", Parent = parent });

				await (s.SaveAsync(parent));
				await (t.CommitAsync());

				long actual = await (s.CreateQuery("select count(c) from Child c").UniqueResultAsync<long>());
				Assert.That(actual, Is.EqualTo(2));

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(2));
				Assert.IsTrue(warnings[0].Contains("detachedChild"));
				Assert.IsTrue(warnings[1].Contains("transientChild"));
			}
		}

		[Test]
		public async Task InsertCascadeNoWarningAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				await (s.SaveAsync(new Child() { Id = "persistedChild" }));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger(typeof(AssignedFixtureAsync).Assembly, "NHibernate"), Level.Warn))
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();

				Parent parent =
					new Parent()
					{
						Id = "parent",
						Children = new List<Child>(),
					};

				await (s.SaveAsync(parent));

				Child child1 = await (s.LoadAsync<Child>("persistedChild"));
				child1.Parent = parent;
				parent.Children.Add(child1);

				Child child2 = new Child() { Id = "transientChild", Parent = parent };
				await (s.SaveAsync(child2));
				parent.Children.Add(child2);

				await (t.CommitAsync());

				long actual = await (s.CreateQuery("select count(c) from Child c").UniqueResultAsync<long>());
				Assert.That(actual, Is.EqualTo(2));

				string[] warnings = GetAssignedIdentifierWarnings(ls);
				Assert.That(warnings.Length, Is.EqualTo(0));
			}
		}
	}
}