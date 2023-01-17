﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2065
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
        protected override void OnSetUp()
        {
            using (var s = OpenSession())
            using (var t = s.BeginTransaction())
            {
                var person = new Person
                {
                    Children = new HashSet<Person>()
                };
                s.Save(person);
                var child = new Person();
                s.Save(child);
                person.Children.Add(child);

                t.Commit();
            }
        }

        protected override void OnTearDown()
        {
            using (var s = OpenSession())
            using (var t = s.BeginTransaction())
            {
                s.Delete("from Person");
                t.Commit();
            }
        }

		[Test]
		public async Task GetGoodErrorForDirtyReassociatedCollectionAsync()
		{
			Person person;
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				person = await (s.GetAsync<Person>(1));
				await (NHibernateUtil.InitializeAsync(person.Children));
				await (t.CommitAsync());
			}

			person.Children.Clear();

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				Assert.That(
					() =>
					{
						return s.LockAsync(person, LockMode.None);
					},
					Throws.TypeOf<HibernateException>()
					      .And.Message.EqualTo(
						      "reassociated object has dirty collection: NHibernate.Test.NHSpecificTest.NH2065.Person.Children"));
			}
		}
	}
}