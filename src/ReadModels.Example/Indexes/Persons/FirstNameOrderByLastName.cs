using System.Collections.Generic;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class FirstNameOrderByLastName : PersonIndexOrderByLastName
	{
		public override IEnumerable<string> CreateKeys(Person entitiy)
		{
			yield return CreateKey(entitiy.FirstName);
		}
	}
}
