using System.Collections.Generic;
using ServiceStack.Redis;

namespace ReadModels.Core.Redis
{
	public class RedisSortUpdater<T> : ISortUpdater<T>
	{
		private readonly IRedisClient _redisClient;

		public IList<string> SortEntriesCreated
		{
			get;
			private set;
		}

		public RedisSortUpdater(IRedisClient redisClient)
		{
			_redisClient = redisClient;
			SortEntriesCreated = new List<string>();
		}

		public void AddEntry(ISort<T> sort, T entity)
		{
			var hashId = sort.FindKey(entity);
			var key = sort.Name;
			var value = sort.FindValue(entity);
			_redisClient.SetEntryInHash(hashId, sort.Name, sort.FindValue(entity));
			SortEntriesCreated.Add(string.Concat(hashId, " ", key, " " , value));
		}

		public void RemoveEntry(ISort<T> sort, T entity)
		{
			_redisClient.RemoveEntryFromHash(sort.FindKey(entity), sort.Name);
		}
	}
}
