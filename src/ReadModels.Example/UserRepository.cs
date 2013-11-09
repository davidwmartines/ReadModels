using System;
using System.Collections.Generic;
using System.Globalization;
using ReadModels.Example.Model;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;
using ServiceStack.Common;

namespace ReadModels.Example
{
	public class UserRepository
	{
		const string UserTypeName = "User";
		const string UserFirstNameProperty = "FirstName";
		const string UserLastNameProperty = "LastName";

		private readonly IRedisClient _redis;
		private readonly IRedisTypedClient<User> _users;

		public UserRepository(IRedisClient redisClient)
		{
			_redis = redisClient;
			_users = _redis.As<User>();
		}

		public void DeleteAll()
		{
			_redis.DeleteAll<User>();
		}

		public IEnumerable<User> ListAll()
		{
			return _users.GetAll();
		}

		public User ReadUser(long id)
		{
			return _users.GetById(id);
		}

		//public IEnumerable<User> FindUsers(string firstName)
		//{
		//	var ids = _redis.GetAllItemsFromSet(GetIndexKey(UserFirstNameProperty, firstName));
		//	return _redis.GetByIds<User>(ids.ToArray());
		//}

		public IEnumerable<User> FindUsers(string firstName, string lastName)
		{
			//Method 1.
			//var indexKey = GetIndexKey(UserFirstNameProperty, firstName);
			//var ids = _redis.GetAllItemsFromSet(indexKey);

			//indexKey = GetIndexKey(UserLastNameProperty, lastName);
			//ids.IntersectWith(_redis.GetAllItemsFromSet(indexKey));


			//Method 2.
			//var ids = _redis.GetIntersectFromSets(
			//	GetIndexKey(UserFirstNameProperty, firstName),
			//	GetIndexKey(UserLastNameProperty, lastName));

			//Method 3.
			var indices = GetUserSearchIndexKeys(firstName, lastName);
			var ids = _redis.GetIntersectFromSets(indices);

			return _redis.GetByIds<User>(ids.ToArray());	
		}

		private string[] GetUserSearchIndexKeys(string firstName, string lastName)
		{
			List<string> keys = new List<string>();
			if (!string.IsNullOrEmpty(firstName))
				keys.Add(GetIndexKey(UserFirstNameProperty, firstName));
			if (!string.IsNullOrEmpty(lastName))
				keys.Add(GetIndexKey(UserLastNameProperty, lastName));
			return keys.ToArray();
		}

		private static string GetIndexKey(string propertyName, string propertyValue)
		{
			return UrnId.Create<User>(propertyName, propertyValue);
		}

		public void AddUser(User user)
		{
			if (user.Id == default(int))
				user.Id = Convert.ToInt32(_users.GetNextSequence());
			_users.Store(user);
			CreateIndexes(user);
		}

		private void CreateIndexes(User user)
		{
			AddIndex(user, UserFirstNameProperty, user.FirstName);
			AddIndex(user, UserLastNameProperty, user.LastName);
		}

		public void UpdateUser(User user)
		{
			_users.Store(user);
			CreateIndexes(user);
		}

		private void AddIndex(User user, string propertyName, string propertyValue)
		{
			if (!string.IsNullOrEmpty(propertyValue))
			{
				var key = GetIndexKey(propertyName, propertyValue);
				_redis.AddItemToSet(key, user.Id.ToString(CultureInfo.InvariantCulture));
			}
		}

		public void DeIndex(User user)
		{
			RemoveIndex(user, UserFirstNameProperty, user.FirstName);
			RemoveIndex(user, UserLastNameProperty, user.LastName);
		}

		private void RemoveIndex(User user, string propertyName, string propertyValue)
		{
			if (!string.IsNullOrEmpty(propertyValue))
			{
				var key = GetIndexKey(propertyName, propertyValue);
				_redis.RemoveItemFromSet(key, user.Id.ToString(CultureInfo.InvariantCulture));
			}
		}

		public void Delete(User user)
		{
			DeIndex(user);
			_users.Delete(user);
		}

	//	private void IndexBy<T>(T instance, Expression<Func<T, Object>> propertyExpression) where T : IIdentifiable
	//	{
	//		var propertyInfo = GetPropertyInfo(instance, propertyExpression);
	//		var propertyName = propertyInfo.Name;
	//		var value = propertyInfo.GetValue(instance);
	//		if (value != null)
	//		{
	//			var setId = UrnId.CreateWithParts(typeof(T).Name, propertyName, value.ToString());
	//			_redis.AddItemToSet(setId, instance.Id.ToString(CultureInfo.InvariantCulture));
	//		}
	//	}

	//	private static PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
	//	{
	//		Type type = typeof(TSource);

	//		MemberExpression member = propertyLambda.Body as MemberExpression;
	//		if (member == null)
	//			throw new ArgumentException(string.Format(
	//				"Expression '{0}' refers to a method, not a property.",
	//				propertyLambda.ToString()));

	//		PropertyInfo propInfo = member.Member as PropertyInfo;
	//		if (propInfo == null)
	//			throw new ArgumentException(string.Format(
	//				"Expression '{0}' refers to a field, not a property.",
	//				propertyLambda.ToString()));

	//		if (type != propInfo.ReflectedType &&
	//			!type.IsSubclassOf(propInfo.ReflectedType))
	//			throw new ArgumentException(string.Format(
	//				"Expresion '{0}' refers to a property that is not from type {1}.",
	//				propertyLambda.ToString(),
	//				type));

	//		return propInfo;
	//	}
	}
}
