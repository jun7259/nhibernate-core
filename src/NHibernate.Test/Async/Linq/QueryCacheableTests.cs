﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Cfg;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq;
using NHibernate.Transform;
using NUnit.Framework;

namespace NHibernate.Test.Linq
{
	using System.Threading.Tasks;
	[TestFixture]
	public class QueryCacheableTestsAsync : LinqTestCase
	{
		protected override void Configure(Configuration cfg)
		{
			cfg.SetProperty(Environment.UseQueryCache, "true");
			cfg.SetProperty(Environment.GenerateStatistics, "true");
			base.Configure(cfg);
		}

		[Test]
		public async Task QueryIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var x = await ((from c in db.Customers
					 select c)
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			var x2 = await ((from c in db.Customers
					  select c)
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task QueryIsCacheable2Async()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var x = await ((from c in db.Customers
					 select c)
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			var x2 = await ((from c in db.Customers
					  select c).ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0), "Unexpected cache hit count");
		}

		[Test]
		public async Task QueryIsCacheable3Async()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var x = await ((from c in db.Customers.WithOptions(o => o.SetCacheable(true))
					 select c).ToListAsync());

			var x2 = await ((from c in db.Customers
					  select c).ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0), "Unexpected cache hit count");
		}

		[Test]
		public async Task QueryIsCacheableWithRegionAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			await (Sfi.EvictQueriesAsync("test"));
			await (Sfi.EvictQueriesAsync("other"));

			var x = await ((from c in db.Customers
					 select c)
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("test"))
				.ToListAsync());

			var x2 = await ((from c in db.Customers
					  select c)
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("test"))
				.ToListAsync());

			var x3 = await ((from c in db.Customers
					  select c)
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("other"))
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task CacheableBeforeOtherClausesAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			await (db.Customers
				.WithOptions(o => o.SetCacheable(true))
				.Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			await (db.Customers.Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0), "Unexpected cache hit count");
		}

		[Test]
		public async Task CacheableRegionBeforeOtherClausesAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			await (Sfi.EvictQueriesAsync("test"));
			await (Sfi.EvictQueriesAsync("other"));

			await (db.Customers
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("test"))
				.Where(c => c.ContactName != c.CompanyName).Take(1)
				.ToListAsync());
			await (db.Customers
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("test"))
				.Where(c => c.ContactName != c.CompanyName).Take(1)
				.ToListAsync());
			await (db.Customers
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("other"))
				.Where(c => c.ContactName != c.CompanyName).Take(1)
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task CacheableRegionSwitchedAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			await (Sfi.EvictQueriesAsync("test"));

			await (db.Customers
				.Where(c => c.ContactName != c.CompanyName).Take(1)
				.WithOptions(o => o.SetCacheable(true).SetCacheRegion("test"))
				.ToListAsync());

			await (db.Customers
				.Where(c => c.ContactName != c.CompanyName).Take(1)
				.WithOptions(o => o.SetCacheRegion("test").SetCacheable(true))
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task GroupByQueryIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var c = await (db
				.Customers
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			c = await (db
				.Customers
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.ToListAsync());

			c = await (db
				.Customers
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task GroupByQueryIsCacheable2Async()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var c = await (db
				.Customers
				.WithOptions(o => o.SetCacheable(true))
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.ToListAsync());

			c = await (db
				.Customers
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.ToListAsync());

			c = await (db
				.Customers
				.WithOptions(o => o.SetCacheable(true))
				.GroupBy(x => x.Address.Country)
				.Select(x => x.Key)
				.ToListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task CanBeCombinedWithFetchAsync()
		{
			//NH-2587
			//NH-3982 (GH-1372)

			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			await (db.Customers
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			await (db.Orders
				.WithOptions(o => o.SetCacheable(true))
				.ToListAsync());

			await (db.Customers
			   .WithOptions(o => o.SetCacheable(true))
				.Fetch(x => x.Orders)
				.ToListAsync());

			await (db.Orders
				.WithOptions(o => o.SetCacheable(true))
				.Fetch(x => x.OrderLines)
				.ToListAsync());

			var customer = await (db.Customers
				.WithOptions(o => o.SetCacheable(true))
				.Fetch(x => x.Address)
				.Where(x => x.CustomerId == "VINET")
				.SingleOrDefaultAsync());

			customer = await (db.Customers
				.WithOptions(o => o.SetCacheable(true))
				.Fetch(x => x.Address)
				.Where(x => x.CustomerId == "VINET")
				.SingleOrDefaultAsync());

			Assert.That(NHibernateUtil.IsInitialized(customer.Address), Is.True, "Expected the fetched Address to be initialized");
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(5), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(5), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task FetchIsCachableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			Order order;

			using (var s = Sfi.OpenSession())
			using (var t = s.BeginTransaction())
			{
				order = (await (s.Query<Order>()
				         .WithOptions(o => o.SetCacheable(true))
				         .Fetch(x => x.Customer)
				         .FetchMany(x => x.OrderLines)
				         .ThenFetch(x => x.Product)
				         .ThenFetchMany(x => x.OrderLines)
				         .Where(x => x.OrderId == 10248)
				         .ToListAsync()))
				         .First();

				await (t.CommitAsync());
			}

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(1), "Unexpected cache miss count");

			Sfi.Statistics.Clear();

			using (var s = Sfi.OpenSession())
			using (var t = s.BeginTransaction())
			{
				order = (await (s.Query<Order>()
				         .WithOptions(o => o.SetCacheable(true))
				         .Fetch(x => x.Customer)
				         .FetchMany(x => x.OrderLines)
				         .ThenFetch(x => x.Product)
				         .ThenFetchMany(x => x.OrderLines)
				         .Where(x => x.OrderId == 10248)
				         .ToListAsync()))
				         .First();
				await (t.CommitAsync());
			}

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(0), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task FutureFetchIsCachableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			var multiQueries = Sfi.ConnectionProvider.Driver.SupportsMultipleQueries;

			Order order;

			using (var s = Sfi.OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.Query<Order>()
				 .WithOptions(o => o.SetCacheable(true))
				 .Fetch(x => x.Customer)
				 .Where(x => x.OrderId == 10248)
				 .ToFuture();

				order = s.Query<Order>()
				         .WithOptions(o => o.SetCacheable(true))
				         .FetchMany(x => x.OrderLines)
				         .ThenFetch(x => x.Product)
				         .ThenFetchMany(x => x.OrderLines)
				         .Where(x => x.OrderId == 10248)
				         .ToFuture()
				         .ToList()
				         .First();

				await (t.CommitAsync());
			}

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(multiQueries ? 1 : 2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(2), "Unexpected cache miss count");

			Sfi.Statistics.Clear();

			using (var s = Sfi.OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.Query<Order>()
				 .WithOptions(o => o.SetCacheable(true))
				 .Fetch(x => x.Customer)
				 .Where(x => x.OrderId == 10248)
				 .ToFuture();

				order = s.Query<Order>()
				         .WithOptions(o => o.SetCacheable(true))
				         .FetchMany(x => x.OrderLines)
				         .ThenFetch(x => x.Product)
				         .ThenFetchMany(x => x.OrderLines)
				         .Where(x => x.OrderId == 10248)
				         .ToFuture()
				         .ToList()
				         .First();

				await (t.CommitAsync());
			}

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(0), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(2), "Unexpected cache hit count");
		}
		
		[Explicit("Not working. dto.Customer retrieved from cache as uninitialized proxy")]
		[Test]
		public async Task ProjectedEntitiesAreCachableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			var	dto = await (session.Query<Order>()
						.WithOptions(o => o.SetCacheable(true))
						.Where(x => x.OrderId == 10248)
						.Select(x => new { x.Customer, Order = x })
						.FirstOrDefaultAsync());

			Assert.That(dto, Is.Not.Null, "dto should not be null");
			Assert.That(dto.Order, Is.Not.Null, "dto.Order should not be null");
			Assert.That(NHibernateUtil.IsInitialized(dto.Order), Is.True, "dto.Order should be initialized");
			Assert.That(dto.Customer, Is.Not.Null, "dto.Customer should not be null");
			Assert.That(NHibernateUtil.IsInitialized(dto.Customer), Is.True, "dto.Customer from cache should be initialized");

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(1), "Unexpected cache miss count");

			Sfi.Statistics.Clear();
			session.Clear();

			dto = await (session.Query<Order>()
							.WithOptions(o => o.SetCacheable(true))
							.Where(x => x.OrderId == 10248)
							.Select(x => new { x.Customer, Order = x })
							.FirstOrDefaultAsync());

			Assert.That(dto, Is.Not.Null, "dto from cache should not be null");
			Assert.That(dto.Order, Is.Not.Null, "dto.Order from cache should not be null");
			Assert.That(NHibernateUtil.IsInitialized(dto.Order), Is.True, "dto.Order from cache should be initialized");
			Assert.That(dto.Customer, Is.Not.Null, "dto.Customer from cache should not be null");
			Assert.That(NHibernateUtil.IsInitialized(dto.Customer), Is.True, "dto.Customer from cache should be initialized");

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(0), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task CacheHqlQueryWithFetchAndTransformerThatChangeTupleAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			// the combination of query and transformer doesn't make sense.
			// It's simply used as example of returned data being transformed before caching leading to mismatch between 
			// Loader.ResultTypes collection and provided tuple
			var order = await (session.CreateQuery("select o.Employee.FirstName, o from Order o join fetch o.Customer where o.OrderId = :id")
							.SetInt32("id", 10248)
							.SetCacheable(true)
							.SetResultTransformer(Transformers.RootEntity)
							.UniqueResultAsync<Order>());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(1), "Unexpected cache miss count");
			Assert.That(order, Is.Not.Null);
			Assert.That(order.Customer, Is.Not.Null);
			Assert.That(NHibernateUtil.IsInitialized(order.Customer), Is.True);

			session.Clear();
			Sfi.Statistics.Clear();

			order = await (session.CreateQuery("select o.Employee.FirstName, o from Order o join fetch o.Customer where o.OrderId = :id")
							.SetInt32("id", 10248)
							.SetCacheable(true)
							.SetResultTransformer(Transformers.RootEntity)
							.UniqueResultAsync<Order>());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(0), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
			Assert.That(order, Is.Not.Null);
			Assert.That(order.Customer, Is.Not.Null);
			Assert.That(NHibernateUtil.IsInitialized(order.Customer), Is.True);
		}

		private static void AssertFetchedOrder(Order order)
		{
			Assert.That(NHibernateUtil.IsInitialized(order), "Expected the order to be initialized");
			Assert.That(order.Customer, Is.Not.Null, "Expected the fetched Customer to be not null");
			Assert.That(NHibernateUtil.IsInitialized(order.Customer), Is.True, "Expected the fetched Customer to be initialized");
			Assert.That(NHibernateUtil.IsInitialized(order.OrderLines), Is.True, "Expected the fetched  OrderLines to be initialized");
			Assert.That(order.OrderLines, Has.Count.EqualTo(3), "Expected the fetched OrderLines to have 3 items");
			var orderLine = order.OrderLines.First();
			Assert.That(orderLine.Product, Is.Not.Null, "Expected the fetched Product to be not null");
			Assert.That(NHibernateUtil.IsInitialized(orderLine.Product), Is.True, "Expected the fetched Product to be initialized");
			Assert.That(NHibernateUtil.IsInitialized(orderLine.Product.OrderLines), Is.True, "Expected the fetched OrderLines to be initialized");
		}
	}
}