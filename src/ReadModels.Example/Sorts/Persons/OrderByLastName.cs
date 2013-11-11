using ReadModels.Example.Model;

namespace ReadModels.Example.Sorts.Persons
{
	public class OrderByLastName : PersonSort
	{
		protected override string GetValueToSortBy(Person entity)
		{
			return entity.LastName + " " + entity.FirstName;
		}
	}
}
