
namespace ReadModels.Core
{
	public interface ISort<T>
	{
		string FindKey(T entity);
		string FindValue(T entity);
		string Name { get; }
		bool IsDescending { get; set; }
		string SortPattern { get; }
		bool IsAlpha { get; }
	}
}
