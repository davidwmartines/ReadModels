using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Core
{
	internal class PowerSet<T>
	{
		public IEnumerable<IEnumerable<T>> Sets
		{
			get;
			private set;
		}

		public PowerSet(IEnumerable<T> source)
		{
			Sets = GetPowerSet(source);
		}

		private static IList<IList<T>> GetPowerSet(IEnumerable<T> list)
		{
			IList<IList<T>> powerset = new List<IList<T>>() { new List<T>() };
			foreach (T item in list)
				foreach (IList<T> set in powerset.ToArray())
					powerset.Add(new List<T>(set) { item });
			return powerset;
		}
	}
}
