using System.Collections.Generic;

namespace ReadModels.Core
{
	public interface ICompositeIndex<T> : IIndex<T>
	{
		bool HasIndexes { get; }
	}
}
