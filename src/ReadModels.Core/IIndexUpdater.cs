namespace ReadModels.Core
{
	public interface IIndexUpdater<T>
	{
		void AddEntry(IIndex<T> index, T entity);
		void AddComposite(ICompositeIndex<T> compositeIndex, T entity);
		void RemoveEntry(IIndex<T> index, T entity);
	}
}