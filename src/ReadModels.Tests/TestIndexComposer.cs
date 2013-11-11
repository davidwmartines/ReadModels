using System.Linq;
using NUnit.Framework;
using ReadModels.Core;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestIndexComposer
	{
		[Test]
		public void CreatesSameKeyAsComposite()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName()
			};
			var composite = new CompositeIndex<Person>(indexes);
			var person = new Person { Id = 1, FirstName = "A", LastName = "B" };
			var writeKey = composite.CreateKeys(person).ElementAt(0);

			var composer = new IndexComposer<Person>();
			composer.AddIndex(new FirstName(), "A");
			composer.AddIndex(new LastName(), "B");
			var readKey = composer.CreateCompositeIndex();

			Assert.AreEqual(writeKey, readKey);
		}

		[Test]
		public void CreatesSameKeyAsCompositeIfIndexesOrderedDifferently()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName()
			};
			var composite = new CompositeIndex<Person>(indexes);
			var person = new Person { Id = 1, FirstName = "A", LastName = "B" };
			var writeKey = composite.CreateKeys(person).ElementAt(0);

			var composer = new IndexComposer<Person>();
			composer.AddIndex(new LastName(), "B");
			composer.AddIndex(new FirstName(), "A");
			
			var readKey = composer.CreateCompositeIndex();

			Assert.AreEqual(writeKey, readKey);
		}
	}
}
