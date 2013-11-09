using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class BirthYear : PersonIndex
	{
		public override IEnumerable<string> CreateKeys(Person entity)
		{
			if (entity.DateOfBirth.HasValue)
				return new string[] { CreateKey(entity.DateOfBirth.Value.Year.ToString(CultureInfo.InvariantCulture)) };
			return new string[0];
		}

		public override IEnumerable<Person> SortEntries(IEnumerable<Person> items)
		{
			return items.OrderBy(i => i.DateOfBirth);
		}

		public override int Id
		{
			get { return 101; }
		}
	}
}
