namespace ReadModels.Core
{
	public interface ISortUpdater<T>
	{
		void AddEntry(ISort<T> sort, T entity);
		void RemoveEntry(ISort<T> sort, T entity);
	}
}
