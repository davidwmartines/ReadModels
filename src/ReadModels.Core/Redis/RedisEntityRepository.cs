using System.Collections;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace ReadModels.Core.Redis
{
	public class RedisEntityRepository<T> : IEntityRepository<T>
		where T : class, new()
	{
		private readonly IRedisClient _redisClient;
		private readonly IRedisTypedClient<T> _typedClient;
		
		public RedisEntityRepository(IRedisClient redisClient)
		{
			_redisClient = redisClient;
			_typedClient = _redisClient.As<T>();
		}

		public T GetById(object id)
		{
			return _typedClient.GetById(id);
		}

		public IEnumerable<T> GetByIds(IEnumerable ids)
		{
			return _typedClient.GetByIds(ids);
		}

		public void Add(T entity)
		{
			_typedClient.Store(entity);
		}

		public void Delete(T entity)
		{
			_typedClient.Delete(entity);
		}

		public void DeleteAll()
		{
			_typedClient.DeleteAll();
		}

		public IndexQueryResult<T> Find(IndexQuery<T> query)
		{
			var result = new IndexQueryResult<T>();
			result.TotalResults = _redisClient.GetSortedSetCount(query.IndexKey, double.NegativeInfinity, double.PositiveInfinity);
			int? skip = null; //TODO calculate from query
			int? take = null;
			var ids = _redisClient.GetRangeFromSortedSetByLowestScore(query.IndexKey, double.NegativeInfinity, double.PositiveInfinity, skip, take);
			result.Results = _redisClient.GetByIds<T>(ids.ToArray());
			return result;
		}
	}
}
