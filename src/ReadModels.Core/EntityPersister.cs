using System;
using System.Globalization;

namespace ReadModels.Core
{
	public class EntityPersister<T> : IEntityPersister<T>
	{
		const string _idField = "Id";

		private readonly IEntityRepository<T> _entityRepository;
		private readonly IEntityIndexer<T> _entityIndexer;

		public EntityPersister(IEntityRepository<T> entityRepository, IEntityIndexer<T> entityIndexer)
		{
			_entityRepository = entityRepository;
			_entityIndexer = entityIndexer;
		}

		public void Store(T entity)
		{
			DeleteIfExists(entity);
			_entityRepository.Add(entity);
			_entityIndexer.AddEntries(entity);
		}

		private void DeleteIfExists(T entity)
		{
			var existingEntity = _entityRepository.GetById(GetId(entity));
			if (existingEntity != null)
			{
				_entityIndexer.RemoveEntries(existingEntity);
				_entityRepository.Delete(entity);
			}
		}

		private object GetId(T entity)
		{
			var property = typeof(T).GetProperty(_idField);
			if (property == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Type '{0}' is not compatible with '{1}'.  It must have an ID Property named '{2}'.", typeof(T).Name, GetType().Name, _idField));
			return property.GetValue(entity);
		}
	}
}
