using System;
using System.Globalization;
using Microsoft.Practices.Unity;
using ReadModels.Core;
using ReadModels.Core.Redis;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace ReadModels.Example
{
	class Program
	{
		const string Host = "localhost";

		static void Main(string[] args)
		{
			try
			{
				using (var redisClient = new RedisClient(Host))
				{

					redisClient.FlushDb();

					//DemoPersonLocations(redisClient);

					//DemoPersons(redisClient);

					//DemoUserRepository(redisClient);

					DemoContainer(redisClient);

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}


			Console.ReadLine();
		}

		private static void DemoContainer(IRedisClient redisClient)
		{
			var container = new UnityContainer();

			container.RegisterType<IRedisClient>(new InjectionFactory(c => redisClient));

			var PersonRegistry = new UnityIndexRegistry<Person>(container);
			PersonRegistry.Register(new AllPersonsOrderByLastName());
			PersonRegistry.Register(new LastNameOrderByLastName());
			PersonRegistry.Register(new FirstNameOrderByLastName());
			PersonRegistry.Register(new LocationOrderByLastName());
			//add additional indexes here...
			container.RegisterInstance<IIndexRegistry<Person>>(PersonRegistry);
			container.RegisterType<IEntityRepository<Person>, RedisEntityRepository<Person>>(new ContainerControlledLifetimeManager());
			container.RegisterType<IIndexUpdater<Person>, RedisIndexUpdater<Person>>(new ContainerControlledLifetimeManager());
			container.RegisterType<IEntityIndexer<Person>, EntityIndexer<Person>>(new ContainerControlledLifetimeManager());
			container.RegisterType<IEntityPersister<Person>, EntityPersister<Person>>(new ContainerControlledLifetimeManager());

			//////

			Console.WriteLine("Creating Persons");
			var persister = container.Resolve<IEntityPersister<Person>>();
			persister.Store(new Person() { Id = 1, FirstName = "Adam", LastName = "Ant", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 } } });
			persister.Store(new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } });
			persister.Store(new Person() { Id = 3, FirstName = "Cal", LastName = "Crumb", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } });
			persister.Store(new Person() { Id = 4, FirstName = "Dom", LastName = "Dog", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 3 } } });
			persister.Store(new Person() { Id = 5, FirstName = "Ernie", LastName = "Eggs", Locations = new PersonLocation[] { new PersonLocation { LocationId = 3 } } });
			persister.Store(new Person() { Id = 26, FirstName = "Zep", LastName = "Zappa" });

			ShowStats(container.Resolve<IIndexUpdater<Person>>() as RedisIndexUpdater<Person>);

			Console.WriteLine("Query for Persons associated to Location 1 and 2");
			var query = new IndexQuery<Person>();
			var indexes = container.Resolve<IIndexRegistry<Person>>();
			query.AddIndex(indexes.Find<LocationOrderByLastName>(), 1.ToString(CultureInfo.InvariantCulture), 2.ToString(CultureInfo.InvariantCulture));
			var repo = container.Resolve<IEntityRepository<Person>>();
			Dump(repo.Find(query));

			Console.WriteLine("Query for all Persons in Location ID 2, LastName='Crumb'");
			query = new IndexQuery<Person>();
			query.AddIndex(indexes.Find<LocationOrderByLastName>(), 2.ToString(CultureInfo.InvariantCulture));
			query.AddIndex(indexes.Find<LastNameOrderByLastName>(), "Crumb");

			Dump(repo.Find(query));

		}

		private static void DemoPersonLocations(RedisClient redisClient)
		{
			var repo = new RedisEntityRepository<Person>(redisClient);
			var indexUpdater = new RedisIndexUpdater<Person>(redisClient);

			var indexes = new IIndex<Person>[]
			{ 
				new AllPersonsOrderByLastName(),
				new FirstNameOrderByLastName(),
				new LastNameOrderByLastName(), 
				new LocationOrderByLastName()
			};

			var indexer = new EntityIndexer<Person>(indexUpdater, indexes);
			var persister = new EntityPersister<Person>(repo, indexer);

			Console.WriteLine("Creating Persons");
			persister.Store(new Person() { Id = 1, FirstName = "Adam", LastName = "Ant", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 } } });
			persister.Store(new Person() { Id = 2, FirstName = "Bart", LastName = "Bug", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } });
			persister.Store(new Person() { Id = 3, FirstName = "Cal", LastName = "Crumb", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 2 } } });
			persister.Store(new Person() { Id = 4, FirstName = "Dom", LastName = "Dog", Locations = new PersonLocation[] { new PersonLocation { LocationId = 1 }, new PersonLocation { LocationId = 3 } } });
			persister.Store(new Person() { Id = 5, FirstName = "Ernie", LastName = "Eggs", Locations = new PersonLocation[] { new PersonLocation { LocationId = 3 } } });
			persister.Store(new Person() { Id = 26, FirstName = "Zep", LastName = "Zappa" });

			Console.WriteLine("Resulting Indexes");
			ShowStats(indexUpdater);

			Console.WriteLine("Query for all Persons in Location ID 1");
			var query = new IndexQuery<Person>();
			query.AddIndex(new LocationOrderByLastName(), 1.ToString(CultureInfo.InvariantCulture));
			Dump(repo.Find(query));

			Console.WriteLine("Query for all Persons in Location ID 2, LastName='Crumb'");
			query = new IndexQuery<Person>();
			query.AddIndex(new LastNameOrderByLastName(), "Crumb");
			query.AddIndex(new LocationOrderByLastName(), 2.ToString(CultureInfo.InvariantCulture));
			Dump(repo.Find(query));

		}

		private static void ShowStats(RedisIndexUpdater<Person> indexUpdater)
		{
			foreach (var key in indexUpdater.Stats.Keys)
			{
				Console.WriteLine("index: {0} - {1} entries", key, indexUpdater.Stats[key]);
			}
		}

		private static void DemoPersons(RedisClient redisClient)
		{

			var repo = new RedisEntityRepository<Person>(redisClient);
			var indexUpdater = new RedisIndexUpdater<Person>(redisClient);

			var indexes = new IIndex<Person>[]
			{ 
				new AllPersonsOrderByLastName(),
				new FirstNameOrderByLastName(),
				new LastNameOrderByLastName() 
			};

			var indexer = new EntityIndexer<Person>(indexUpdater, indexes);
			var persister = new EntityPersister<Person>(repo, indexer);

			Console.WriteLine("Creating Persons");
			persister.Store(new Person() { Id = 1, FirstName = "Dave", LastName = "Martines" });
			persister.Store(new Person() { Id = 2, FirstName = "William", LastName = "Martines" });
			persister.Store(new Person() { Id = 3, FirstName = "William", LastName = "Smith" });
			persister.Store(new Person() { Id = 4, FirstName = "Dave", LastName = "Jones" });
			persister.Store(new Person() { Id = 5, FirstName = "Robert", LastName = "Smith" });
			persister.Store(new Person() { Id = 6, FirstName = "Who", LastName = "Dini" });
			persister.Store(new Person() { Id = 7, FirstName = "Who", LastName = "Dini" });
			persister.Store(new Person() { Id = 8, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1970, 1, 1) });

			Console.WriteLine("Get by ID 2");
			var result = repo.GetById(2);
			Console.WriteLine(result.Dump<Person>());

			Console.WriteLine("Query for all Persons, order by lastname, firstname");
			var query = new IndexQuery<Person>();
			query.AddIndex(new AllPersonsOrderByLastName(), string.Empty);
			Dump(repo.Find(query));

			Console.WriteLine("Query by firstname = 'Dave'");
			query = new IndexQuery<Person>();
			query.AddIndex(new FirstNameOrderByLastName(), "Dave");
			Dump(repo.Find(query));

			Console.WriteLine("Query by firstname = 'Who', lastname='Dini'");
			query = new IndexQuery<Person>();
			query.AddIndex(new FirstNameOrderByLastName(), "Who");
			query.AddIndex(new LastNameOrderByLastName(), "Dini");
			Dump(repo.Find(query));

			Console.WriteLine("Query by firstname = 'William', lastname='Martines'");
			query = new IndexQuery<Person>();
			query.AddIndex(new FirstNameOrderByLastName(), "William");
			query.AddIndex(new LastNameOrderByLastName(), "Martines");
			Dump(repo.Find(query));

			Console.WriteLine("Query by firstname = 'David', lastname='Martines'");
			query = new IndexQuery<Person>();
			query.AddIndex(new FirstNameOrderByLastName(), "David");
			query.AddIndex(new LastNameOrderByLastName(), "Martines");
			Dump(repo.Find(query));

			Console.WriteLine("Update a Person");
			persister.Store(new Person() { Id = 7, FirstName = "Harry", LastName = "Whodini" });

			Console.WriteLine("Query for all Persons, order by lastname, firstname");
			query = new IndexQuery<Person>();
			query.AddIndex(new AllPersonsOrderByLastName(), string.Empty);
			Dump(repo.Find(query));

			Console.WriteLine("Query by firstname = 'Who', lastname='Dini'");
			query = new IndexQuery<Person>();
			query.AddIndex(new FirstNameOrderByLastName(), "Who");
			query.AddIndex(new LastNameOrderByLastName(), "Dini");
			Dump(repo.Find(query));

			Console.WriteLine("Query by lastname = 'Whodini'");
			query = new IndexQuery<Person>();
			query.AddIndex(new LastNameOrderByLastName(), "Whodini");
			Dump(repo.Find(query));

			Console.WriteLine("Query by lastname = 'whodini' (case sensitivity)");
			query = new IndexQuery<Person>();
			query.AddIndex(new LastNameOrderByLastName(), "whodini");
			Dump(repo.Find(query));

		}

		private static void Dump(IndexQueryResult<Person> result)
		{
			Console.WriteLine("Found {0} results", result.TotalResults);
			result.Results.PrintDump();
		}

		private static void DemoUserRepository(RedisClient redisClient)
		{
			var repository = new UserRepository(redisClient);

			repository.DeleteAll();

			repository.AddUser(new User { Id = 9000, FirstName = "David", LastName = "Martines" });
			repository.AddUser(new User { FirstName = "David", LastName = "Jones" });
			repository.AddUser(new User { FirstName = "Will", LastName = "Martines" });
			repository.AddUser(new User { FirstName = "Will", LastName = "Smith" });

			Console.WriteLine("Searching for 'Will'");
			var found = repository.FindUsers("Will", null);
			found.PrintDump();

			Console.WriteLine("Searching for 'David Martines'");
			found = repository.FindUsers("David", "Martines");
			found.PrintDump();

			Console.WriteLine("Updating David to Dave");
			var user = repository.ReadUser(9000);

			//before modifying, de-index
			repository.DeIndex(user);
			user.FirstName = "Dave";
			repository.UpdateUser(user);

			Console.WriteLine("Searching for 'David Martines' again");
			found = repository.FindUsers("David", "Martines");
			found.PrintDump();

			Console.WriteLine("Searching for 'Dave Martines'");
			found = repository.FindUsers("Dave", "Martines");
			found.PrintDump();

			Console.WriteLine("Deleting");
			repository.Delete(user);

			Console.WriteLine("Searching for 'Dave Martines' again");
			found = repository.FindUsers("Dave", "Martines");
			found.PrintDump();

			Console.WriteLine("Searching for 'David'");
			found = repository.FindUsers("David", null);
			found.PrintDump();
		}



	}
}
