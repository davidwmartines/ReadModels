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
			_indexes = indexes.Where(i=> i.IsComposable).OrderBy(i => i.Id).ToList();
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

		//public override IEnumerable<string> CreateKeys(T entity)
		//{
		//	List<IndexKey<T>> indexKeys = new List<IndexKey<T>>();
		//	foreach (var index in _indexes)
		//	{
		//		var keys = index.CreateKeys(entity);
		//		foreach (var key in keys)
		//			indexKeys.Add(new IndexKey<T>(index, key));
		//	}
		//	var powerSet = new PowerSet<IndexKey<T>>(indexKeys);
		//	List<string> compositeKeys = new List<string>();
		//	foreach (var set in powerSet.Sets)
		//	{
		//		if (set.Count() < 2)
		//			continue;
		//		//if (set.Select(ik => ik.Index).Distinct().Count() == 1)
		//		//	continue;
		//		var allKeysInSet = set.Select(ik => ik.Key);
		//		compositeKeys.Add(MakeCompositeKey(allKeysInSet.ToArray()));
		//	}
		//	return compositeKeys;
		//}

		//public IDictionary<string, IEnumerable<IIndex<T>>> GetKeySets(T entity)
		//{
		//	var dictionary = new Dictionary<string, IEnumerable<IIndex<T>>>();
		//	var powerSet = new PowerSet<IIndex<T>>(_indexes);
		//	foreach (var set in powerSet.Sets)
		//	{
		//		if (set.Count() < 2)
		//			continue;
				
		//		List<string> keys = new List<string>();
		//		foreach (var index in set)
		//		{
		//			keys.AddRange(index.CreateKeys(entity));
		//		}

		//		var compositeKey = MakeCompositeKey(keys.ToArray());
				
		//		if (dictionary.ContainsKey(compositeKey))
		//			continue;

		//		dictionary.Add(compositeKey, set);
		//	}
		//	return dictionary;
		//}

		public override IEnumerable<T> SortEntries(IEnumerable<T> items)
		{
			return _indexes.First().SortEntries(items);
		}

		//public string[] GetIndividualIndexKeys(T entity)
		//{
		//	return _indexes.SelectMany(i => i.CreateKeys(entity)).ToArray();
		//}

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

		public IEnumerable<IIndex<T>> ListIndexes()
		{
			return _indexes.AsReadOnly();
		}

		public override string FindKey(string propertyValue)
		{
			throw new NotImplementedException("CompositeIndex is for writing only");
		}

		public override int Id
		{
			get { return _indexes.Sum(i => i.Id); }
		}
	}
}
