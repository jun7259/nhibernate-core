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
using NUnit.Framework;

namespace NHibernate.Test.SqlTest.Custom
{
	using System.Threading.Tasks;
	[TestFixture]
	public abstract class CustomStoredProcSupportTestAsync : CustomSQLSupportTestAsync
	{
		[Test]
		public async Task ScalarStoredProcedureAsync()
		{
			ISession s = OpenSession();
			IQuery namedQuery = s.GetNamedQuery("simpleScalar");
			namedQuery.SetInt64("number", 43L);
			IList list = await (namedQuery.ListAsync());
			object[] o = (object[])list[0];
			Assert.AreEqual(o[0], "getAll");
			Assert.AreEqual(o[1], 43L);
			s.Close();
		}

		[Test]
		public async Task ParameterHandlingAsync()
		{
			ISession s = OpenSession();

			IQuery namedQuery = s.GetNamedQuery("paramhandling");
			namedQuery.SetInt64(0, 10L);
			namedQuery.SetInt64(1, 20L);
			IList list = await (namedQuery.ListAsync());
			object[] o = (Object[])list[0];
			Assert.AreEqual(o[0], 10L);
			Assert.AreEqual(o[1], 20L);

			namedQuery = s.GetNamedQuery("paramhandling_mixed");
			namedQuery.SetInt64(0, 10L);
			namedQuery.SetInt64("second", 20L);
			list = await (namedQuery.ListAsync());
			o = (object[])list[0];
			Assert.AreEqual(o[0], 10L);
			Assert.AreEqual(o[1], 20L);
			s.Close();
		}

		[Test]
		public async Task EntityStoredProcedureAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			Organization ifa = new Organization("IFA");
			Organization jboss = new Organization("JBoss");
			Person gavin = new Person("Gavin");
			Employment emp = new Employment(gavin, jboss, "AU");
			await (s.SaveAsync(ifa));
			await (s.SaveAsync(jboss));
			await (s.SaveAsync(gavin));
			await (s.SaveAsync(emp));

			IQuery namedQuery = s.GetNamedQuery("selectAllEmployments");
			IList list = await (namedQuery.ListAsync());
			Assert.IsTrue(list[0] is Employment);
			await (s.DeleteAsync(emp));
			await (s.DeleteAsync(ifa));
			await (s.DeleteAsync(jboss));
			await (s.DeleteAsync(gavin));

			await (t.CommitAsync());
			s.Close();
		}
	}
}