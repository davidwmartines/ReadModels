using Moq;
using NUnit.Framework;
using ReadModels.Example.Model;

namespace ReadModels.Core.Tests
{
	[TestFixture]
	public class TestEntityPersister
	{
		private Mock<IEntityRepository<Person>> _mockEntityRepository;
		private Mock<IEntityIndexer<Person>> _mockEntityIndexer;
		private Mock<IEntitySorter<Person>> _mockEntitySorter;

		[SetUp]
		public void SetUp()
		{
			_mockEntityIndexer = new Mock<IEntityIndexer<Person>>();
			_mockEntityRepository = new Mock<IEntityRepository<Person>>();
			_mockEntitySorter = new Mock<IEntitySorter<Person>>();
		}

		[Test]
		public void StoresNewEntity()
		{
			_mockEntityRepository.Setup(p => p.GetById(1)).Returns(null as Person);
			var persister = new EntityPersister<Person>(_mockEntityRepository.Object, _mockEntityIndexer.Object, _mockEntitySorter.Object);

			var person = new Person { Id = 1 };
			persister.Store(person);

			_mockEntityRepository.Verify(r => r.Add(person), Times.Once());
			_mockEntityRepository.Verify(r => r.Delete(person), Times.Never());
			_mockEntityIndexer.Verify(i => i.AddEntries(person), Times.Once());
			_mockEntitySorter.Verify(i => i.AddEntries(person), Times.Once());
		}

		[Test]
		public void StoresExistingEntity()
		{
			var person = new Person { Id = 1 };

			_mockEntityRepository.Setup(p => p.GetById(1)).Returns(person);
			
			var persister = new EntityPersister<Person>(_mockEntityRepository.Object, _mockEntityIndexer.Object, _mockEntitySorter.Object);

			persister.Store(person);

			_mockEntityRepository.Verify(r => r.Add(person), Times.Once());
			_mockEntityRepository.Verify(r => r.Delete(person), Times.Once());
			_mockEntityIndexer.Verify(i => i.AddEntries(person), Times.Once());
			_mockEntitySorter.Verify(i => i.AddEntries(person), Times.Once());
		}
	}
}
