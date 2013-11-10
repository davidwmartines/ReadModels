using System;
using System.Globalization;

namespace ReadModels.Core
{
	public class EntityPersister<T> : IEntityPersister<T>
	{
		private readonly IEntityRepository<T> _entityRepository;
		private readonly IEntityIndexer<T> _entityIndexer;
		private readonly IEntitySorter<T> _entitySorter;

		public EntityPersister(IEntityRepository<T> entityRepository, IEntityIndexer<T> entityIndexer, IEntitySorter<T> entitySorter)
		{
			_entityRepository = entityRepository;
			_entityIndexer = entityIndexer;
			_entitySorter = entitySorter;
		}

		public void Store(T entity)
		{
			DeleteIfExists(entity);
			_entityRepository.Add(entity);
			_entityIndexer.AddEntries(entity);
			_entitySorter.AddEntries(entity);
		}

		private void DeleteIfExists(T entity)
		{
			var existingEntity = _entityRepository.GetById(GetId(entity));
			if (existingEntity != null)
			{
				_entityIndexer.RemoveEntries(existingEntity);
				_entitySorter.RemoveEntries(existingEntity);
				_entityRepository.Delete(entity);
			}
		}

		private object GetId(T entity)
		{
			return EntityIdUtility.GetId<T>(entity);
		}
	}
}
