using System.Collections.Generic;

namespace ReadModels.Core
{
	public class EntitySorter<T> : IEntitySorter<T>
	{
		private readonly ISortUpdater<T> _sortUpdater;
		private readonly IEnumerable<ISort<T>> _sorts;
		
		public EntitySorter(ISortUpdater<T> sortUpdater, IEnumerable<ISort<T>> sorts)
		{
			_sortUpdater = sortUpdater;
			_sorts = sorts;
		}

		public void AddEntries(T entity)
		{
			foreach (var sort in _sorts)
				_sortUpdater.AddEntry(sort, entity);
		}

		public void RemoveEntries(T entity)
		{
			foreach (var sort in _sorts)
				_sortUpdater.RemoveEntry(sort, entity);
		}
	}
}
