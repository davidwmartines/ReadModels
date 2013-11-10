using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace ReadModels.Core.Containers.Unity
{
	public class UnityIndexRegistry<TEntity>
	{
		private IUnityContainer _container;

		public UnityIndexRegistry(IUnityContainer container)
		{
			_container = container;
			_container.RegisterType<IEnumerable<IIndex<TEntity>>>(new InjectionFactory(c => c.ResolveAll<IIndex<TEntity>>().ToArray()));
		}

		public void Register(IIndex<TEntity> index)
		{
			_container.RegisterInstance<IIndex<TEntity>>(index.Name, index);
		}
	}
}
