﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Test.TypesTest
{
	using System.Threading.Tasks;
	/// <summary>
	/// Summary description for BinaryBlobTypeFixture.
	/// </summary>
	[TestFixture]
	public class BinaryBlobTypeFixtureAsync : TypeFixtureBase
	{
		protected override string TypeName
		{
			get { return "BinaryBlob"; }
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			ISession s = OpenSession();
			BinaryBlobClass b = new BinaryBlobClass();
			b.BinaryBlob = UnicodeEncoding.UTF8.GetBytes("foo/bar/baz");
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			s.Close();

			s = OpenSession();
			b = (BinaryBlobClass) await (s.LoadAsync(typeof(BinaryBlobClass), b.Id));
			ObjectAssert.AreEqual(UnicodeEncoding.UTF8.GetBytes("foo/bar/baz"), b.BinaryBlob);
			await (s.DeleteAsync(b));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ReadWriteLargeBlobAsync()
		{
			ISession s = OpenSession();
			BinaryBlobClass b = new BinaryBlobClass();
			b.BinaryBlob = UnicodeEncoding.UTF8.GetBytes(new string('T', 10000));
			await (s.SaveAsync(b));
			await (s.FlushAsync());
			s.Close();

			s = OpenSession();
			b = (BinaryBlobClass) await (s.LoadAsync(typeof(BinaryBlobClass), b.Id));
			ObjectAssert.AreEqual(UnicodeEncoding.UTF8.GetBytes(new string('T', 10000)), b.BinaryBlob);
			await (s.DeleteAsync(b));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task ReadWriteZeroLenAsync()
		{
			object savedId;
			using (ISession s = OpenSession())
			{
				BinaryBlobClass b = new BinaryBlobClass();
				b.BinaryBlob = Array.Empty<byte>();
				savedId = await (s.SaveAsync(b));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var b = await (s.GetAsync<BinaryBlobClass>(savedId));
				Assert.That(b.BinaryBlob, Is.Not.Null.And.Length.EqualTo(0));
				await (s.DeleteAsync(b));
				await (s.FlushAsync());
			}
		}
	}
}