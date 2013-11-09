using System.Collections.Generic;

namespace ReadModels.Core
{
	public class IndexQueryResult<T>
	{
		public IEnumerable<T> Results { get; set; }
		public long TotalResults { get; set; }
	}
}
