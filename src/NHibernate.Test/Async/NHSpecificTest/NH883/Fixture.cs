﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH883
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			int catId;

			using (ISession sess = OpenSession())
			{
				Cat c = new Cat();
				await (sess.SaveAsync(c));
				catId = c.Id;
				await (sess.FlushAsync());
			}

			using (ISession sess = OpenSession())
			{
				Cat c = (Cat) await (sess.GetAsync(typeof(Cat), catId));
				Cat kitten = new Cat();
				c.Children.Add(kitten);
				kitten.Mother = c;
				await (sess.SaveAsync(kitten));

				// Double flush
				await (sess.FlushAsync());
				await (sess.FlushAsync());

				await (sess.DeleteAsync(c));
				await (sess.DeleteAsync(kitten));
				await (sess.FlushAsync());
			}
		}
	}
}