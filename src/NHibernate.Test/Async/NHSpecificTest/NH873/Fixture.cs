﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NHibernate.Cfg;
using NUnit.Framework;
using Environment=NHibernate.Cfg.Environment;

namespace NHibernate.Test.NHSpecificTest.NH873
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync
	{
		[Test]
		public async Task CacheDisabledAsync()
		{
			Configuration cfg = new Configuration();
			cfg.SetProperty(Environment.UseSecondLevelCache, "false");
			cfg.SetProperty(Environment.UseQueryCache, "false");
			cfg.SetProperty(Environment.CacheProvider, null);
			await (cfg.BuildSessionFactory().CloseAsync());
		}
	}
}