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

		[SetUp]
		public void SetUp()
		{
			_mockEntityIndexer = new Mock<IEntityIndexer<Person>>();
			_mockEntityRepository = new Mock<IEntityRepository<Person>>();
		}

		[Test]
		public void StoresNewEntity()
		{
			_mockEntityRepository.Setup(p => p.GetById(1)).Returns(null as Person);
			var persister = new EntityPersister<Person>(_mockEntityRepository.Object, _mockEntityIndexer.Object);

			var Person = new Person { Id = 1 };
			persister.Store(Person);

			_mockEntityRepository.Verify(r => r.Add(Person), Times.Once());
			_mockEntityRepository.Verify(r => r.Delete(Person), Times.Never());
			_mockEntityIndexer.Verify(i => i.AddEntries(Person), Times.Once());
		}

		[Test]
		public void StoresExistingEntity()
		{
			var Person = new Person { Id = 1 };

			_mockEntityRepository.Setup(p => p.GetById(1)).Returns(Person);
			
			var persister = new EntityPersister<Person>(_mockEntityRepository.Object, _mockEntityIndexer.Object);

			persister.Store(Person);

			_mockEntityRepository.Verify(r => r.Add(Person), Times.Once());
			_mockEntityRepository.Verify(r => r.Delete(Person), Times.Once());
			_mockEntityIndexer.Verify(i => i.AddEntries(Person), Times.Once());
		}
	}
}
