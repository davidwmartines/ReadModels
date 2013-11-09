
namespace ReadModelsWithKeyValueStore
{
	internal class IndexKey<T>
	{
		public IndexKey(IIndex<T> index, string key)
		{
			Index = index;
			Key = key;
		}
		public IIndex<T> Index { get; set; }
		public string Key { get; set; }
	}
}
