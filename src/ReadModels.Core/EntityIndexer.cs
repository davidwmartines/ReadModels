using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Core
{
	public class EntityIndexer<T> : IEntityIndexer<T>
	{
		private readonly IIndexUpdater<T> _indexUpdater;
		private readonly IEnumerable<IIndex<T>> _indexes;
		private readonly ICompositeIndex<T> _compositeIndex;

		public EntityIndexer(IIndexUpdater<T> indexUpdater,  IEnumerable<IIndex<T>> indexes)
		{
			_indexes = indexes;
			_indexUpdater = indexUpdater;
			_compositeIndex = new CompositeIndex<T>(_indexes);
		}

		public void AddEntries(T entity)
		{
			foreach (var index in _indexes)
				_indexUpdater.AddEntry(index, entity);

			if (_compositeIndex.HasIndexes)
				_indexUpdater.AddComposite(_compositeIndex, entity);
		}

		public void RemoveEntries(T entity)
		{
			foreach (var index in _indexes)
				_indexUpdater.RemoveEntry(index, entity);

			if (_compositeIndex.HasIndexes)
				_indexUpdater.RemoveEntry(_compositeIndex, entity);
		}
	}
}
