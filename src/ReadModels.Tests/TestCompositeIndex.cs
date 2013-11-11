using System;
using System.Linq;
using NUnit.Framework;
using ReadModels.Core;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;

namespace ReadModels.Tests
{
	[TestFixture]
	public class TestCompositeIndex
	{
		[Test]
		public void CreatesKeysForTwoSingleValueIndexes()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName()
			};
			var composite = new CompositeIndex<Person>(indexes);
			var person = new Person{Id = 1, FirstName = "A", LastName = "B"};
			
			var keys = composite.CreateKeys(person);

			Assert.AreEqual(1, keys.Count());
			Assert.AreEqual("PERSON:FIRSTNAME:A|PERSON:LASTNAME:B", keys.ElementAt(0));
		}

		[Test]
		public void CreatesKeysForThreeSingleValueIndexes()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName(),
				new BirthYear()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var person = new Person { Id = 1, FirstName = "A", LastName = "B", DateOfBirth = new DateTime(1970, 1, 1) };

			var keys = composite.CreateKeys(person);

			Assert.AreEqual(4, keys.Count());
		}

		[Test]
		public void CreatesKeysForOneSingleValueAndOneMultiValueIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new Locations()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var person = new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } };
			var keys = composite.CreateKeys(person);

			Assert.AreEqual(4, keys.Count());
		}

		[Test]
		public void CreatesKeysForTwoSingleValueAndOneMultiValueIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName(),
				new Locations()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var person = new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } };
			var keys = composite.CreateKeys(person);

			Assert.AreEqual(11, keys.Count());
		}

		[Test]
		public void CreatesSameKeyRegardlessOfOrderOfIndexes()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstName(),
				new LastName()
			};
			var composite = new CompositeIndex<Person>(indexes);
			var person = new Person { Id = 1, FirstName = "A", LastName = "B" };

			var key1 = composite.CreateKeys(person).ElementAt(0);

			var indexes2 = new IIndex<Person>[]
			{ 
				new LastName(),
				new FirstName()
			};
			var composite2 = new CompositeIndex<Person>(indexes2);

			var key2 = composite2.CreateKeys(person).ElementAt(0);

			Assert.AreEqual(key1, key2);
		}
	}
}
