using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Core
{
	public class IndexComposer<T>
	{
		private IDictionary<IIndex<T>, string[]> _indexes = new Dictionary<IIndex<T>, string[]>();

		public void AddIndex(IIndex<T> index, params string[] values)
		{
			_indexes.Add(index, values);
		}

		public string CreateCompositeIndex()
		{
			var indexes = _indexes.Keys.OrderBy(i => i.Name);
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
