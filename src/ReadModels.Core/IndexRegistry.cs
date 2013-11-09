using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace ReadModels.Core
{
	

	public class UnityIndexRegistry<TEntity> : IIndexRegistry<TEntity>
	{
		private IUnityContainer _container;

		public UnityIndexRegistry(IUnityContainer container)
		{
			_container = container;
			_container.RegisterType<IEnumerable<IIndex<TEntity>>>(new InjectionFactory(c => c.ResolveAll<IIndex<TEntity>>().ToArray()));
		}

		public IIndex<TEntity> Find<TIndex>() where TIndex : IIndex<TEntity>
		{
			return _container.Resolve<IIndex<TEntity>>(typeof(TIndex).Name);
		}

		public void Register(IIndex<TEntity> index)
		{
			//index.Id = IndexId.Next();
			_container.RegisterInstance<IIndex<TEntity>>(index.GetType().Name, index);
		}
	}

	//public static class IndexId
	//{
	//	private static int _id;

	//	public static int Next()
	//	{
	//		return ++_id;
	//	}
	//}
}
