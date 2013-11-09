using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class LocationOrderByLastName : PersonIndexOrderByLastName
	{
		public override IEnumerable<string> CreateKeys(Person entity)
		{
			if (entity.Locations != null)
				return entity.Locations.Select(o => CreateKey(o.LocationId.ToString(CultureInfo.InvariantCulture)));

			return new string[0];
		}

		public override int Id
		{
			get { return 104; }
		}
	}
}
