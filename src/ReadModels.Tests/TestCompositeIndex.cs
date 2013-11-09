﻿using System;
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
				new FirstNameOrderByLastName(),
				new LastNameOrderByLastName()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var Person = new Person{Id = 1, FirstName = "A", LastName = "B"};

			var keys = composite.CreateKeys(Person);

			Assert.AreEqual(1, keys.Count());
		}

		[Test]
		public void CreatesKeysForThreeSingleValueIndexes()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstNameOrderByLastName(),
				new LastNameOrderByLastName(),
				new BirthYear()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var Person = new Person { Id = 1, FirstName = "A", LastName = "B", DateOfBirth = new DateTime(1970, 1, 1) };

			var keys = composite.CreateKeys(Person);

			Assert.AreEqual(4, keys.Count());
		}

		[Test]
		public void CreatesKeysForOneSingleValueAndOneMultiValueIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstNameOrderByLastName(),
				new LocationOrderByLastName()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var Person = new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } };
			var keys = composite.CreateKeys(Person);

			Assert.AreEqual(4, keys.Count());
		}

		[Test]
		public void CreatesKeysForTwoSingleValueAndOneMultiValueIndex()
		{
			var indexes = new IIndex<Person>[]
			{ 
				new FirstNameOrderByLastName(),
				new LastNameOrderByLastName(),
				new LocationOrderByLastName()
			};

			var composite = new CompositeIndex<Person>(indexes);

			var Person = new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } };
			var keys = composite.CreateKeys(Person);

			Assert.AreEqual(11, keys.Count());
		}
	}
}