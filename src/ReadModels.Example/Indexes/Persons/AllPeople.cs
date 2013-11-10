using System.Collections.Generic;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class AllPeople: PersonIndex
	{
		public override IEnumerable<string> CreateKeys(Person entitiy)
		{
			yield return CreateKey(string.Empty);
		}

		public override string FindKey(string propertyValue)
		{
			return CreateKey(string.Empty);
		}

		public override bool IsComposable
		{
			get
			{
				return false;
			}
		}
	}
}
