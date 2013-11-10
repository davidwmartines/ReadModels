using System.Collections.Generic;

namespace ReadModels.Core
{
	public interface IIndex<T> 
	{
		IEnumerable<string> CreateKeys(T entity);
		string FindKey(string propertyValue);
		bool IsComposable { get; }
		string Name { get; }
	}
}
