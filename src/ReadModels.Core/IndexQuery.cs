namespace ReadModels.Core
{
	public class IndexQuery<T>
	{
		const int _defaultPageSize = 50;

		private IndexComposer<T> _indexComposer = new IndexComposer<T>();
		
		public int? PageSize { get; set; }

		public int? PageNumber { get; set; }

		public ISort<T> Sort { get; set; }

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

		public int? CalculateSkip()
		{
			if (PageNumber.HasValue)
			{
				return ((PageNumber.Value - 1) * (PageSize ?? _defaultPageSize));
			}
			return PageNumber;	
		}
	}
}
