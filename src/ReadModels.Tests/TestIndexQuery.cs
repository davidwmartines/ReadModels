using NUnit.Framework;
using ReadModels.Core;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestIndexQuery
	{
		[Test]
		public void GetsIndexKey()
		{
			var query = new IndexQuery<Person>();
			query.AddIndex(new FirstName(), "A");
			query.AddIndex(new LastName(), "B");
			var key = query.IndexKey;
			Assert.AreEqual("PERSON:FIRSTNAME:A|PERSON:LASTNAME:B", key);
		}

		[Test]
		public void CalculatesSkipAsNullWhenNoPagingSpecified()
		{
			var query = new IndexQuery<Person>();
			var skip = query.CalculateSkip();
			Assert.AreEqual(default(int?), skip);
		}

		[Test]
		public void CalculatesSkipForFirstPage()
		{
			var query = new IndexQuery<Person>();
			query.PageNumber = 1;
			query.PageSize = 20;
			var skip = query.CalculateSkip();
			Assert.AreEqual(0, skip);
		}

		[Test]
		public void CalculatesSkipForSecondPage()
		{
			var query = new IndexQuery<Person>();
			query.PageNumber = 2;
			query.PageSize = 20;
			var skip = query.CalculateSkip();
			Assert.AreEqual(20, skip);
		}
	}
}
