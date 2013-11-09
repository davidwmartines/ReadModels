
namespace ReadModels.Core
{
	public class IndexQuery<T>
	{
		private IndexComposer<T> _indexComposer = new IndexComposer<T>();
		
		public int? PageSize { get; set; }

		public int? PageIndex { get; set; }

		public void AddIndex(IIndex<T> index, params string[] propertyValues)
		{
			_indexComposer.AddIndex(index, propertyValues);
		}

		public string IndexKey
		{
			get
			{
				return _indexComposer.CreateCompositeIndex();
			}
		}
	}
}
