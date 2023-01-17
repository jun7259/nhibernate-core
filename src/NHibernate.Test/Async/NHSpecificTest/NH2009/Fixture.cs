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
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2009
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task PropertyRefToJoinedTableAsync()
		{
			BlogPost savedBlogPost = new BlogPost();

			using (ISession session = OpenSession())
			{
				User user1 = new User();
				user1.FullName = "First User";
				user1.UserName = "User1";
				await (session.SaveAsync(user1));

				User user2 = new User();
				user2.FullName = "Second User";
				user2.UserName = "User2";
				await (session.SaveAsync(user2));

				savedBlogPost.Title = "Post 1";
				savedBlogPost.Poster = user1;
				await (session.SaveAsync(savedBlogPost));

				await (session.FlushAsync());
				session.Clear();
			}
			
			using (ISession session = OpenSession())
			{
				var user = await (session.GetAsync<BlogPost>(savedBlogPost.ID));
			}

			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				await (session.DeleteAsync("from User"));
				await (session.DeleteAsync("from BlogPost"));
				await (tx.CommitAsync());
			}
		}
	}
}
