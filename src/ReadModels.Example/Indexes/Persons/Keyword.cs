using System.Collections.Generic;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class Keyword : PersonIndex
	{
		public override IEnumerable<string> CreateKeys(Person entity)
		{
			return new string[]
			{
				CreateKey(entity.FirstName),
				CreateKey(entity.LastName),
				CreateKey(entity.FullName)
			};
		}
	}
}
