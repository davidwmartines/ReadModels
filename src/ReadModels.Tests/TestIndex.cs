using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestIndex
	{

		[Test]
		public void GetsName()
		{
			var index = new LastName();
			var name = index.Name;
			Assert.AreEqual("LastName", name);
		}

		[Test]
		public void FindsKeyForValue()
		{
			var index = new LastName();
			var key = index.FindKey("Smith");
			KeyTester.AssertIndexKey(typeof(Person), "LastName", "Smith", key);
		}

		[Test]
		public void CreatesKeyForEntity()
		{
			var index = new LastName();
			var person = new Person() { LastName = Guid.NewGuid().ToString() };
			var keys = index.CreateKeys(person);

			Assert.AreEqual(1, keys.Count());
			var key = keys.ElementAt(0);
			KeyTester.AssertIndexKey(typeof(Person), "LastName", person.LastName, key);
		}
	}
}
