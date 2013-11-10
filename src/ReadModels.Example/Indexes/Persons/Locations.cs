using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class Locations : PersonIndex
	{
		public override IEnumerable<string> CreateKeys(Person entity)
		{
			if (entity.Locations != null)
				return entity.Locations.Select(o => CreateKey(o.LocationId.ToString(CultureInfo.InvariantCulture)));

			return new string[0];
		}
	}
}
