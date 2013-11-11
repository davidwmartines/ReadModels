using Microsoft.Practices.Unity;
using ReadModels.Core;
using ReadModels.Core.Containers.Unity;
using ReadModels.Core.Redis;
using ReadModels.Example.Indexes.Persons;
using ReadModels.Example.Model;
using ReadModels.Example.Sorts.Persons;
using ServiceStack.Redis;

namespace ReadModels.Tests.IntegrationTests
{
	public class Container
	{
		private IUnityContainer _container;

		public void Configure(IRedisClient redisClient)
		{
			_container = new UnityContainer();

			_container.RegisterType<IRedisClient>(new InjectionFactory(c => redisClient));

			var personIndexRegistry = new UnityIndexRegistry<Person>(_container);
			personIndexRegistry.Register(new AllPeople());
			personIndexRegistry.Register(new LastName());
			personIndexRegistry.Register(new FirstName());
			personIndexRegistry.Register(new Locations());

			var personSortRegistry = new UnitySortRegistry<Person>(_container);
			personSortRegistry.Register(new OrderByFirstName());
			personSortRegistry.Register(new OrderByLastName());

			_container.RegisterType<IEntityRepository<Person>, RedisEntityRepository<Person>>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IIndexUpdater<Person>, RedisIndexUpdater<Person>>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IEntityIndexer<Person>, EntityIndexer<Person>>(new ContainerControlledLifetimeManager());
			_container.RegisterType<ISortUpdater<Person>, RedisSortUpdater<Person>>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IEntitySorter<Person>, EntitySorter<Person>>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IEntityPersister<Person>, EntityPersister<Person>>(new ContainerControlledLifetimeManager());

		}

		public IEntityPersister<T> GetPersister<T>()
		{
			return _container.Resolve<IEntityPersister<T>>();
		}

		public IEntityRepository<T> GetRepo<T>()
		{
			return _container.Resolve<IEntityRepository<T>>();
		}
	}
}
