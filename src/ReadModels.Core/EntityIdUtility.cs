using System;
using System.Globalization;

namespace ReadModels.Core
{
	public static class EntityIdUtility
	{
		const string _idField = "Id";

		public static object GetId<T>(T entity)
		{
			var property = typeof(T).GetProperty(_idField);
			if (property == null)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The Type '{0}' is not compatible with '{1}'.  It must have an ID Property named '{2}'.", typeof(T).Name, typeof(EntityIdUtility).Name, _idField));
			return property.GetValue(entity);
		}
	}
}
