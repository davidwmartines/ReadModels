
namespace ReadModels.Core
{
	public abstract class Sort<T> : ISort<T>
	{
		protected const string _keyPrefix = "sort:";
		protected readonly string _fullKeyPrefix = string.Concat(_keyPrefix, typeof(T).Name, ":");

		public virtual string FindKey(T entity)
		{
			return string.Concat(_fullKeyPrefix, EntityIdUtility.GetId<T>(entity));
		}

		public string FindValue(T entity)
		{
			return GetValueToSortBy(entity).ToUpperInvariant();
		}

		protected abstract string GetValueToSortBy(T entity);
		
		public virtual string Name
		{
			get
			{
				return GetType().Name;
			}
		}

		public virtual string SortPattern
		{
			get {
				return string.Concat(_fullKeyPrefix, "*->", Name);
			}
		}

		public virtual bool IsDescending
		{
			get;
			set;
		}

		public virtual bool IsAlpha
		{
			get { return true; }
		}
	}
}
