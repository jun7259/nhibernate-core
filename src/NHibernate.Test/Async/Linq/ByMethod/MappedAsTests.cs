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
using System.Linq;
using System.Text;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.Linq.ByMethod
{
	using System.Threading.Tasks;
	[TestFixture]
	public class MappedAsTestsAsync : LinqTestCase
	{
		[Test]
		public async Task WithUnaryExpressionAsync()
		{
			var num = 1;
			await (db.Orders.Where(o => o.Freight == (-num).MappedAs(NHibernateUtil.Decimal)).ToListAsync());
			await (db.Orders.Where(o => o.Freight == ((decimal) num).MappedAs(NHibernateUtil.Decimal)).ToListAsync());
			await (db.Orders.Where(o => o.Freight == ((decimal?) (decimal) num).MappedAs(NHibernateUtil.Decimal)).ToListAsync());
		}

		[Test]
		public async Task WithNewExpressionAsync()
		{
			var num = 1;
			await (db.Orders.Where(o => o.Freight == new decimal(num).MappedAs(NHibernateUtil.Decimal)).ToListAsync());
		}

		[Test]
		public async Task WithMethodCallExpressionAsync()
		{
			var num = 1;
			await (db.Orders.Where(o => o.Freight == GetDecimal(num).MappedAs(NHibernateUtil.Decimal)).ToListAsync());
		}

		private decimal GetDecimal(int number)
		{
			return number;
		}
	}
}
