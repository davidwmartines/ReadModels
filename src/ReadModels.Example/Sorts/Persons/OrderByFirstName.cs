using ReadModels.Example.Model;

namespace ReadModels.Example.Sorts.Persons
{
	public class OrderByFirstName : PersonSort
	{
		protected override string GetValueToSortBy(Person entity)
		{
			return entity.FirstName + " " + entity.LastName;
		}
	}
}
