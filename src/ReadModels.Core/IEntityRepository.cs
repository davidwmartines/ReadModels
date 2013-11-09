using System.Collections;
using System.Collections.Generic;

namespace ReadModels.Core
{
	public interface IEntityRepository<T>
	{
		T GetById(object id);
		IEnumerable<T> GetByIds(IEnumerable ids);
		void Add(T entity);
		void Delete(T entity);
		void DeleteAll();
		IndexQueryResult<T> Find(IndexQuery<T> query);
	}

}
