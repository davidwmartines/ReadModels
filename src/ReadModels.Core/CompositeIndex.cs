using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Core
{
	public class CompositeIndex<T> : Index<T>, ICompositeIndex<T>
	{
		private List<IIndex<T>> _indexes;

		public CompositeIndex(IEnumerable<IIndex<T>> indexes)
		{
			_indexes = indexes.Where(i=> i.IsComposable).OrderBy(i => i.Name).ToList();
		}

		public override IEnumerable<string> CreateKeys(T entity)
		{
			List<string> indexKeys = new List<string>();
			foreach (var index in _indexes)
			{
				indexKeys.AddRange(index.CreateKeys(entity));
			}
			var powerSet = new PowerSet<string>(indexKeys);
			List<string> compositeKeys = new List<string>();
			foreach (var set in powerSet.Sets)
			{
				if (set.Count() < 2)
					continue;
				compositeKeys.Add(MakeCompositeKey(set.ToArray()));
			}
			return compositeKeys;
		}

		public static string MakeCompositeKey(string[] indexKeys)
		{
			return string.Join("|", indexKeys);
		}

		public bool HasIndexes
		{
			get
			{
				return _indexes.Any();
			}
		}

		public override string FindKey(string propertyValue)
		{
			throw new NotImplementedException("CompositeIndex is for writing only");
		}

		public override string Name
		{
			get { return string.Join(":", _indexes.Select(i => i.Name).ToArray()); }
		}
	}
}
