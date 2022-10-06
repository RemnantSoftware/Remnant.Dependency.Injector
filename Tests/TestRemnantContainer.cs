using NUnit.Framework;
using Remnant.Dependency.Injector;
using Remnant.Dependeny.Injector.Tests.TestObjects;
using System;

namespace Remnant.Dependeny.Injector.Tests
{
	public class TestRemnantContainer
	{
		[Test]
		public void Should_be_able_to_create_container()
		{
			Assert.IsNotNull(Container.Create("MyContainer"));
			Assert.IsNotNull(Container.InternalContainer<RemnantContainer>());
		}

		[Test]
		public void Should_error_if_container_already_exists()
		{
			Assert.Throws<InvalidOperationException>(() => Container.Create("MyContainer"));
		}

		[Test]
		public void Should_be_able_to_resolve_from_object()
		{
			Container.Clear();
			Container.Register<IAnimal>(new Dog());
			Container.Register<Dog>(new Dog());
			Assert.IsTrue(new Dog().Sound == new object().Resolve<IAnimal>().Sound);
			Assert.IsTrue(new Dog().Sound == new object().Resolve<Dog>().Sound);
		}

		[Test]
		public void Should_be_able_to_inject_on_field_declaration()
		{
			Container.Clear();
			Container.Register<IAnimal>(new Dog());
			Container.Register<Dog>(new Dog());
			var animalSound = new AnimalSoundInjectOnField();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());
			Assert.IsTrue(new Dog().Sound == new object().Resolve<Dog>().Sound);

			Container.Clear();
			Container.Register<IAnimal>(new Cat());
			animalSound = new AnimalSoundInjectOnField();
			Assert.IsTrue(new Cat().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_on_constructor_declaration()
		{
			Container.Clear();
			Container.Register<IAnimal>(new Dog());
			var animalSound = new AnimalSoundInjectOnConstructor();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());

			Container.Clear();
			Container.Register<IAnimal>(new Cat());
			animalSound = new AnimalSoundInjectOnConstructor();
			Assert.IsTrue(new Cat().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_using_inject_attribute()
		{
			Container.Clear();
			Container.Register<IAnimal>(new Dog());
			var animalSound = new AnimalSoundInjectUsingAttr();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());

			Container.Clear();
			Container.Register<IAnimal>(new Cat());
			animalSound = new AnimalSoundInjectUsingAttr();
			Assert.IsTrue(new Cat().Sound == animalSound.MakeSound());
		}

		[Test]
		public void Should_be_able_to_inject_using_create_due_to_existing_constructor()
		{
			Container.Clear();
			Container.Register<IAnimal>(new Dog());
			var animalSound = AnimalSoundInjectUsingCreate.Create();
			Assert.IsTrue(new Dog().Sound == animalSound.MakeSound());

			Container.Clear();
			Container.Register<IAnimal>(new Cat());
			animalSound = AnimalSoundInjectUsingCreate.Create();
			Assert.IsTrue(new Cat().Sound == animalSound.MakeSound());
		}
	}
}