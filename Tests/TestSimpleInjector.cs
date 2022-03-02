using NUnit.Framework;
using Remnant.Dependency.Injector;
using Remnant.Dependency.SimpleInjector;
using Remnant.Dependeny.Injector.Tests.TestObjects;

namespace Remnant.Dependeny.Injector.Tests
{
	public class TestSimpleInjector : DITests
	{
		public TestSimpleInjector()
		{
			var adapter = new SimpleInjectorAdapter(new global::SimpleInjector.Container());
			SetContainer(Container.Create("MyContainer", adapter));

			Assert.IsNotNull(adapter.Resolve<IAnimal>());
		}
	}
}