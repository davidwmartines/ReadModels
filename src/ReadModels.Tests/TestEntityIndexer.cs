using Moq;
using NUnit.Framework;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;

namespace ReadModels.Core.Tests
{
	[TestFixture]
	public class TestEntityIndexer
	{
		private Mock<IIndexUpdater<Person>> _mockIndexUpdater;

		[SetUp]
		public void SetUp()
		{
			_mockIndexUpdater = new Mock<IIndexUpdater<Person>>();
		}

		[Test]
		public void AddIndexEntriesForEachIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstNameOrderByLastName(),
				new LocationOrderByLastName()
			};

			var indexer = new EntityIndexer<Person>(_mockIndexUpdater.Object, indexes);

			var Person = new Person { Id = 1 };

			indexer.AddEntries(Person);

			_mockIndexUpdater.Verify(i => i.AddEntry(indexes[0], Person), Times.Once());
			_mockIndexUpdater.Verify(i => i.AddEntry(indexes[1], Person), Times.Once());
			_mockIndexUpdater.Verify(i => i.AddComposite(It.IsAny<ICompositeIndex<Person>>(), Person), Times.Once());
		}

		[Test]
		public void RemovesIndexEntriesForEachIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstNameOrderByLastName(),
				new LocationOrderByLastName()
			};

			var indexer = new EntityIndexer<Person>(_mockIndexUpdater.Object, indexes);

			var Person = new Person { Id = 1 };

			indexer.RemoveEntries(Person);

			_mockIndexUpdater.Verify(i => i.RemoveEntry(indexes[0], Person), Times.Once());
			_mockIndexUpdater.Verify(i => i.RemoveEntry(indexes[1], Person), Times.Once());
			_mockIndexUpdater.Verify(i => i.RemoveEntry(It.IsAny<ICompositeIndex<Person>>(), Person), Times.Once());
		}
	}
}
