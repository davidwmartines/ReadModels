using System.Collections.Generic;
using System.Linq;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public abstract class PersonIndexOrderByLastName : PersonIndex
	{
		public override IEnumerable<Person> SortEntries(IEnumerable<Person> items)
		{
			return items.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
		}
	}
}
