using NUnit.Framework;
using Remnant.Dependency.Injector;
using Remnant.Dependency.Interface;
using Remnant.Dependeny.Injector.Tests.TestObjects;
using System;

namespace Remnant.Dependeny.Injector.Tests
{
	public abstract class DITests
	{
		public IContainer _container;

		public void SetContainer(IContainer container)
		{
			_container = container;
			Container.Instance.Register<IAnimal>(new Dog());
			Container.Instance.Register<Dog>(new Dog());
		}

		[Test]
		public void Should_be_able_to_create_container()
		{
			Assert.IsNotNull(_container);
		}

		[Test]
		public void Should_error_if_container_already_exists()
		{
			Assert.Throws<InvalidOperationException>(() => Container.Create("MyContainer"));
		}

		[Test]
		public void Should_be_able_to_resolve_from_object()
		{
			Assert.IsTrue(new Dog().Sound == new object().Resolve<IAnimal>().Sound);
		}

		[Test]
		public void Should_be_able_to_inject_on_field_declaration()
		{
			var animalSound = new AnimalSoundInjectOnField();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_on_constructor_declaration()
		{
			var animalSound = new AnimalSoundInjectOnConstructor();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_using_inject_attribute()
		{
			var animalSound = new AnimalSoundInjectUsingAttr();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_using_create_due_to_existing_constructor()
		{
			var animalSound = AnimalSoundInjectUsingCreate.Create();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());
		}
	}
}