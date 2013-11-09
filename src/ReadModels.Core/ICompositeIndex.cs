
using System.Collections.Generic;
namespace ReadModels.Core
{
	public interface ICompositeIndex<T> : IIndex<T>
	{
		//string[] GetIndividualIndexKeys(T entity);
		//IDictionary<string, IEnumerable<IIndex<T>>> GetKeySets(T entity);
		bool HasIndexes { get; }
	}
}
