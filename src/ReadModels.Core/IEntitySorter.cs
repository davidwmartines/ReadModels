namespace ReadModels.Core
{
	public interface IEntitySorter<T>
	{
		void AddEntries(T entity);
		void RemoveEntries(T entity);
	}
}
