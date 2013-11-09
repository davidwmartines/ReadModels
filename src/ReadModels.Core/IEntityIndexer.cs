namespace ReadModels.Core
{
	public interface IEntityIndexer<T>
	{
		void AddEntries(T entity);
		void RemoveEntries(T entity);
	}
}
