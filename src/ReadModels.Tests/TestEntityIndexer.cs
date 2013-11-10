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
				new FirstName(),
				new Locations()
			};

			var indexer = new EntityIndexer<Person>(_mockIndexUpdater.Object, indexes);

			var person = new Person { Id = 1 };

			indexer.AddEntries(person);

			_mockIndexUpdater.Verify(i => i.AddEntry(indexes[0], person), Times.Once());
			_mockIndexUpdater.Verify(i => i.AddEntry(indexes[1], person), Times.Once());
			_mockIndexUpdater.Verify(i => i.AddComposite(It.IsAny<ICompositeIndex<Person>>(), person), Times.Once());
		}

		[Test]
		public void RemovesIndexEntriesForEachIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new Locations()
			};

			var indexer = new EntityIndexer<Person>(_mockIndexUpdater.Object, indexes);

			var person = new Person { Id = 1 };

			indexer.RemoveEntries(person);

			_mockIndexUpdater.Verify(i => i.RemoveEntry(indexes[0], person), Times.Once());
			_mockIndexUpdater.Verify(i => i.RemoveEntry(indexes[1], person), Times.Once());
			_mockIndexUpdater.Verify(i => i.RemoveEntry(It.IsAny<ICompositeIndex<Person>>(), person), Times.Once());
		}
	}
}
