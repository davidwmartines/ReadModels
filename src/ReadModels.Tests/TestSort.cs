using NUnit.Framework;
using ReadModels.Example.Model;
using ReadModels.Example.Sorts.Persons;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestSort
	{
		[Test]
		public void FindsHashKeyForEntity()
		{
			var sort = new OrderByLastName();
			var person = new Person { Id = 123, FirstName = "Fred", LastName = "Sanford" };
			var key = sort.FindKey(person);
			Assert.AreEqual("sort:Person:123", key);
		}

		[Test]
		public void GetsSortPattern()
		{
			var sort = new OrderByLastName();
			var pattern = sort.SortPattern;
			Assert.AreEqual("sort:Person:*->OrderByLastName", pattern);
		}
	}
}
