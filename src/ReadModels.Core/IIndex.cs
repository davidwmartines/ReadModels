using System.Collections.Generic;

namespace ReadModels.Core
{
	public interface IIndex<T> 
	{
		IEnumerable<string> CreateKeys(T entity);
		string FindKey(string propertyValue);
		IEnumerable<T> SortEntries(IEnumerable<T> items);
		bool IsComposable { get; }
		int Id { get; }
	}
}
