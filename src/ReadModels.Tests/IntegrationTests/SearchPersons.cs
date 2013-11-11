using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ReadModels.Core;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;
using ReadModels.Example.Sorts.Persons;

namespace ReadModels.Tests.IntegrationTests
{
	[TestFixture]
	public class SearchPersons : IntegrationTest
	{
		private void LoadPersons()
		{
			var persister = Container.GetPersister<Person>();
			persister.Store(new Person() { Id = 1, FirstName = "Dave", LastName = "Martines" });
			persister.Store(new Person() { Id = 2, FirstName = "William", LastName = "Martines" });
			persister.Store(new Person() { Id = 3, FirstName = "William", LastName = "Smith" });
			persister.Store(new Person() { Id = 4, FirstName = "Dave", LastName = "Jones" });
			persister.Store(new Person() { Id = 5, FirstName = "Robert", LastName = "Smith" });
			persister.Store(new Person() { Id = 6, FirstName = "Who", LastName = "dini" });
			persister.Store(new Person() { Id = 7, FirstName = "Who", LastName = "Dini" });
			persister.Store(new Person() { Id = 8, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1970, 1, 1) });
		}

		private IndexQueryResult<Person> Execute(IndexQuery<Person> query)
		{
			var repo = Container.GetRepo<Person>();
			return repo.Find(query);
		}

		[Test]
		public void FindAllPersonsOrderByLastName()
		{
			LoadPersons();

			var query = new IndexQuery<Person>();
			query.AddIndex(new AllPeople(), string.Empty);
			query.Sort = new OrderByLastName();

			var result = Execute(query);

			Assert.AreEqual(8, result.TotalResults);
			Assert.AreEqual(8, result.Results.Count());

			Assert.AreEqual("dini", result.Results.ElementAt(0).LastName.ToLowerInvariant());
			Assert.AreEqual("dini", result.Results.ElementAt(1).LastName.ToLowerInvariant());
			Assert.AreEqual("doe", result.Results.ElementAt(2).LastName.ToLowerInvariant());
			Assert.AreEqual("jones", result.Results.ElementAt(3).LastName.ToLowerInvariant());
			Assert.AreEqual("martines", result.Results.ElementAt(4).LastName.ToLowerInvariant());
			Assert.AreEqual("martines", result.Results.ElementAt(5).LastName.ToLowerInvariant());
			Assert.AreEqual("smith", result.Results.ElementAt(6).LastName.ToLowerInvariant());
			Assert.AreEqual("smith", result.Results.ElementAt(7).LastName.ToLowerInvariant());
		}

		[Test]
		public void FindAllPersonsOrderByLastNameDesc()
		{
			LoadPersons();

			var query = new IndexQuery<Person>();
			query.AddIndex(new AllPeople(), string.Empty);
			query.Sort = new OrderByLastName() { IsDescending = true };

			var result = Execute(query);

			Assert.AreEqual(8, result.TotalResults);
			Assert.AreEqual(8, result.Results.Count());

			Assert.AreEqual("dini", result.Results.ElementAt(7).LastName.ToLowerInvariant());
			Assert.AreEqual("dini", result.Results.ElementAt(6).LastName.ToLowerInvariant());
			Assert.AreEqual("doe", result.Results.ElementAt(5).LastName.ToLowerInvariant());
			Assert.AreEqual("jones", result.Results.ElementAt(4).LastName.ToLowerInvariant());
			Assert.AreEqual("martines", result.Results.ElementAt(3).LastName.ToLowerInvariant());
			Assert.AreEqual("martines", result.Results.ElementAt(2).LastName.ToLowerInvariant());
			Assert.AreEqual("smith", result.Results.ElementAt(1).LastName.ToLowerInvariant());
			Assert.AreEqual("smith", result.Results.ElementAt(0).LastName.ToLowerInvariant());
		}
	}
}
