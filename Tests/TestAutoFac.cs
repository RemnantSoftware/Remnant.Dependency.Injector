using NUnit.Framework;
using Remnant.Dependency.Autofac;
using Remnant.Dependency.Injector;
using Remnant.Dependeny.Injector.Tests.TestObjects;

namespace Remnant.Dependeny.Injector.Tests
{
	public class TestAutoFac : DITests
	{
		public TestAutoFac()
		{
			var autofac = new Autofac.ContainerBuilder();
			var adapter = new AutofacAdapter(autofac);
			Container.Create("MyContainer", adapter);
			Container.Instance.Register<IAnimal>(new Dog());
			Container.Instance.Register<Dog>(new Dog());

			autofac.Build() ;
			_container = Container.Instance;

			Assert.IsNotNull(adapter.Resolve<IAnimal>());
			Assert.IsNotNull(adapter.Resolve<Dog>());
			Assert.IsNotNull(autofac.Resolve<IAnimal>());
			Assert.IsNotNull(autofac.Resolve<Dog>());
		}
	}
}