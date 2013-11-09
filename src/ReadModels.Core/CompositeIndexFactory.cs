using System.Collections.Generic;
using System.Linq;

namespace HelloRedis
{
	public class CompositeIndexFactory<T> : ICompositeIndexFactory<T>
		where T : IIdentifiable
	{	
		public IEnumerable<ICompositeIndex<T>> CreateComposites(IEnumerable<IIndex<T>> sourceIndexes)
		{
			var composableIndexes = sourceIndexes.Where(i => i.IsComposable).ToArray();
			var powerSet = new PowerSet<IIndex<T>>(composableIndexes);
			var composites = new List<CompositeIndex<T>>();
			foreach (var set in powerSet.Sets)
			{
				if (set.Count() < 2)
					continue;

				if(composites.Any(c => c.HasIndexes && 
					c.ListIndexes().Count() == set.Count() &&
					c.ListIndexes().Except(set).Count() == 0 ))
					continue;

				var composite = new CompositeIndex<T>(set);
				composites.Add(composite);
				
			}
			return composites;
		}
	}
}
