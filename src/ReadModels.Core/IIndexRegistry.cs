namespace ReadModels.Core
{
	public interface IIndexRegistry<TEntity>
	{
		void Register(IIndex<TEntity> index);
		IIndex<TEntity> Find<TIndex>() where TIndex : IIndex<TEntity>;
	}
}
