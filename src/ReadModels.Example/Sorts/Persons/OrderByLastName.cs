using ReadModels.Example.Model;

namespace ReadModels.Example.Sorts.Persons
{
	public class OrderByLastName : PersonSort
	{
		public override string FindValue(Person entity)
		{
			return entity.LastName + " " + entity.FirstName;
		}
	}
}
