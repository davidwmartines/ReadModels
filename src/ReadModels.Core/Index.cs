using System.Collections.Generic;

namespace ReadModels.Core
{
	public abstract class Index<T> : IIndex<T>
	{
		public abstract IEnumerable<string> CreateKeys(T entity);

		public abstract IEnumerable<T> SortEntries(IEnumerable<T> items);

		public virtual string FindKey(string propertyValue)
		{
			return CreateKey(propertyValue);
		}

		public abstract int Id { get; }

		protected string CreateKey(string propertyValue)
		{
			return string.Concat(Id, ":", propertyValue);
		}

		public virtual bool IsComposable
		{
			get
			{
				return true;
			}
		}
	}
}
