using System.Collections.Generic;
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
				_redisClient.AddItemToSet(setId, value);
				UpdateStats(setId);
			}
		}

		private static string IdValue(T entity)
		{
			return entity.GetId<T>().ToString();
		}


		private void UpdateStats(string setId)
		{
			if (!Stats.ContainsKey(setId))
				Stats.Add(setId, 0);
			Stats[setId] = _redisClient.GetSetCount(setId);
		}

		public void AddComposite(ICompositeIndex<T> compositeIndex, T entity)
		{
			foreach (var setId in compositeIndex.CreateKeys(entity))
			{
				_redisClient.StoreIntersectFromSets(setId, setId.Split('|'));
				UpdateStats(setId);
			}
		}

		public void RemoveEntry(IIndex<T> index, T entity)
		{
			var value = IdValue(entity);
			foreach(var setId in index.CreateKeys(entity))
				_redisClient.RemoveItemFromSet(setId, value);	
		}
	}
}
