using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Core
{
	public class IndexComposer<T>
	{
		//private List<string> _indexKeys = new List<string>();

		private IDictionary<IIndex<T>, string[]> _indexes = new Dictionary<IIndex<T>, string[]>();

		//public void AddIndex(IIndex<T> index, string value)
		//{
		//	if (_indexKeys.Any() && !index.IsComposable)
		//		throw new InvalidOperationException("Query can only contain a single index when using a non-composable index.");
		//	_indexKeys.Add(index.FindKey(value));
		//}

		public void AddIndex(IIndex<T> index, params string[] values)
		{
			_indexes.Add(index, values);

		}

		public string CreateCompositeIndex()
		{
			var indexes = _indexes.Keys.OrderBy(i => i.Id);
			var indexKeys = new List<string>();
			foreach (var index in indexes)
			{
				var values = _indexes[index];
				foreach (var value in values)
				{
					indexKeys.Add(index.FindKey(value));
				}
			}
			return CompositeIndex<T>.MakeCompositeKey(indexKeys.ToArray());
		}
	}
}
