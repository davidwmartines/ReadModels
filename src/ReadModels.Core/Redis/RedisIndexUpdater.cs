using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Utils;
using ServiceStack.Redis;

namespace ReadModels.Core.Redis
{
	public class RedisIndexUpdater<T> : IIndexUpdater<T>
	{
		private readonly IRedisClient _redisClient;

		public IDictionary<string, long> Stats
		{
			get;
			private set;
		}

		public RedisIndexUpdater(IRedisClient redisClient)
		{
			_redisClient = redisClient;
			Stats = new Dictionary<string, long>();
		}

		public void AddEntry(IIndex<T> index, T entity)
		{
			var value = IdValue(entity);
			foreach (var setId in index.CreateKeys(entity))
			{
				_redisClient.AddItemToSortedSet(setId, value);
				UpdateSort(index, setId);
				UpdateStats(setId);
			}
		}

		private static string IdValue(T entity)
		{
			return entity.GetId<T>().ToString();
		}

		private void UpdateSort(IIndex<T> index, string setId)
		{
			var score = 0D;
			var ids = _redisClient.GetAllItemsFromSortedSet(setId);
			var allEntities = _redisClient.As<T>().GetByIds(ids);
			var sorted = index.SortEntries(allEntities);
			sorted.ToList()
				.ForEach(e =>
				{
					var value = IdValue(e);
					_redisClient.RemoveItemFromSortedSet(setId, value);
					_redisClient.AddItemToSortedSet(setId, value, ++score);
				});
		}

		private void UpdateStats(string setId)
		{
			if (!Stats.ContainsKey(setId))
				Stats.Add(setId, 0);
			Stats[setId] = _redisClient.GetSortedSetCount(setId);
		}

		public void AddComposite(ICompositeIndex<T> compositeIndex, T entity)
		{
			foreach (var setId in compositeIndex.CreateKeys(entity))
			{
				_redisClient.StoreIntersectFromSortedSets(setId, setId.Split('|'));
				UpdateSort(compositeIndex, setId);
				UpdateStats(setId);
			}
		}

		public void RemoveEntry(IIndex<T> index, T entity)
		{
			var value = IdValue(entity);
			foreach(var setId in index.CreateKeys(entity))
				_redisClient.RemoveItemFromSortedSet(setId, value);	
		}
	}
}
