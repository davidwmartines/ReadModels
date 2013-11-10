using ReadModels.Example.Model;

namespace ReadModels.Example.Sorts.Persons
{
	public class OrderByFirstName : PersonSort
	{
		public override string FindValue(Person entity)
		{
			return entity.FirstName + " " + entity.LastName;
		}
	}
}
