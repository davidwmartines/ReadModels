namespace ReadModels.Core
{
	public interface IEntityPersister<T>
	{
		void Store(T entity);
	}
}
