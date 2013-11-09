using System.Collections.Generic;
using ReadModels.Example.Model;

namespace ReadModels.Example.Indexes.Persons
{
	public class LastNameOrderByLastName : PersonIndexOrderByLastName
	{
		public override IEnumerable<string> CreateKeys(Person entity)
		{
			yield return CreateKey(entity.LastName);
		}

		public override int Id
		{
			get { return 103; }
		}
	}
}
