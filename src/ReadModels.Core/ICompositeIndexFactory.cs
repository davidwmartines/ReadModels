using System.Collections.Generic;

namespace HelloRedis
{
	public interface ICompositeIndexFactory<T>
		where T : IIdentifiable
	{
		IEnumerable<ICompositeIndex<T>> CreateComposites(IEnumerable<IIndex<T>> sourceIndexes);
	}
}
