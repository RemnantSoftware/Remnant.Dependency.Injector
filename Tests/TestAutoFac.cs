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
			Container.Create("MyContainer", new AutofacAdapter(autofac));
			Container.Instance.Register<IAnimal>(new Dog());

			autofac.Build() ;
			_container = Container.Instance;

			Assert.IsNotNull(autofac.Resolve<IAnimal>());
		}
	}
}