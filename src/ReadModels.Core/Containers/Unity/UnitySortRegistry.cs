using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace ReadModels.Core.Containers.Unity
{
	public class UnitySortRegistry<TEntity>
	{
		private IUnityContainer _container;

		public UnitySortRegistry(IUnityContainer container)
		{
			_container = container;
			_container.RegisterType<IEnumerable<ISort<TEntity>>>(new InjectionFactory(c => c.ResolveAll<ISort<TEntity>>().ToArray()));
		}

		public void Register(ISort<TEntity> index)
		{
			_container.RegisterInstance<ISort<TEntity>>(index.Name, index);
		}
	}
}