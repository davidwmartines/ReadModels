using NUnit.Framework;
using ReadModels.Example.Model;
using ServiceStack.Redis;

namespace ReadModels.Tests.IntegrationTests
{
	[TestFixture]
	public class StoreAndRead : IntegrationTest
	{
		[Test]
		public void CanStoreAndReadAnEntity()
		{
			var person = new Person { Id = 99, FirstName = "Bob", LastName = "Dobbs" };

			var persister = Container.GetPersister<Person>();
			persister.Store(person);

			var repo = Container.GetRepo<Person>();

			var found = repo.GetById(99);

			Assert.IsNotNull(found);

			Assert.AreEqual(person.Id, found.Id);
			Assert.AreEqual(person.FirstName, found.FirstName);
			Assert.AreEqual(person.LastName, found.LastName);
		}
	}
}
