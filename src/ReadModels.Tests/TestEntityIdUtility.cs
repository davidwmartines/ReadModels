using System;
using NUnit.Framework;
using ReadModels.Core;
using ReadModels.Example.Model;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestEntityIdUtility
	{
		[Test]
		public void GetsIdValue()
		{
			var person = new Person { Id = 123 };
			var idValue = EntityIdUtility.GetId<Person>(person);
			Assert.AreEqual(123, idValue);
		}

		class Incompatible
		{
			public string A
			{
				get;
				set;
			}
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ThrowsWhenNoIdProperty()
		{
			var example = new Incompatible { A = "test" };
			EntityIdUtility.GetId<Incompatible>(example);
		}
	}
}
