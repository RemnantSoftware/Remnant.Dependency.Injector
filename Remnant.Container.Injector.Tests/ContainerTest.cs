using NUnit.Framework;
using System;

namespace Remnant.Container.Injector.Tests
{
	public class ContainerTests 
	{
		private static RemContainer _container;

		public ContainerTests()
		{
			_container = RemContainer.Create("Remnant.Test.Container");
		}

		[Test]
		public void Test_transient_resolve()
		{
			_container.Register<Dog>();
			_container.Register<Cat>();

			var dog = _container.Resolve<Dog>();
			Assert.IsTrue(dog.Name == "Dog");

			var cat = _container.Resolve<Cat>();
			Assert.IsTrue(cat.Name == "Cat");
		}

		[Test]
		public void Test_transient_animal_sound_with_field_resolve()
		{
			_container.Clear();
			_container.Register<IAnimal, Dog>();

			var animalSound = new AnimalSoundObject();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Dog().MakeSound());

			_container.DeRegister<IAnimal>();
			_container.Register<IAnimal, Cat>();

			animalSound = new AnimalSoundObject();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Cat().MakeSound());
		}

		[Test]
		public void Test_transient_animal_sound_with_constructor_resolve()
		{
			_container.Clear();
			_container.Register<IAnimal, Dog>();

			var animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Dog().MakeSound());

			_container.DeRegister<IAnimal>();
			_container.Register<IAnimal, Cat>();

			animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Cat().MakeSound());
		}


		[Test]
		public void Test_singleton_animal_sound()
		{
			_container.Clear();
			_container.Register<IAnimal>(new Dog());

			var animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Dog().MakeSound());

			_container.DeRegister<IAnimal>();
			_container.Register<IAnimal>(new Cat());

			animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Cat().MakeSound());
		}

		[Test]
		public void Test_singleton_mix_transient_animal_sound()
		{
			_container.Clear();
			_container.Register<IAnimal>(new Dog());

			var animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Dog().MakeSound());

			_container.DeRegister<IAnimal>();
			_container.Register<IAnimal, Cat>();

			animalSound = new AnimalSoundObject2();
			Assert.IsTrue(animalSound.Animal?.MakeSound() == new Cat().MakeSound());
		}

		[Test]
		public void Should_fail_animal_sound_if_not_resolve()
		{
			// cant use Assert.Throws for constructor
			try
			{
				new AnimalSoundObject2();
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is ArgumentException);
				Assert.IsTrue(ex.Message == $"The container cannot resolve requested object '{typeof(IAnimal).FullName}'.");
			}
		}


		//[Test]
		//public void Should_fail_with_no_container()
		//{
		//	// cant use Assert.Throws for constructor
		//	try
		//	{
		//		new AnimalSoundObject2();
		//	}
		//	catch (Exception ex)
		//	{
		//		Assert.IsTrue(ex is NullReferenceException);
		//		Assert.IsTrue(ex.Message == $"There is no container registered with the app domain. Unable to resolve {typeof(IAnimal).FullName}.");
		//	}
		//}

		//[Test]
		//public void Should_fail_if_container_internal_name_is_overwritten()
		//{
		//	AppDomain.CurrentDomain.SetData("RemnantContainer", "NewContainer");

		//	// cant use Assert.Throws for constructor
		//	try
		//	{
		//		new AnimalSoundObject2();
		//	}
		//	catch (Exception ex)
		//	{
		//		Assert.IsTrue(ex is NullReferenceException);
		//		Assert.IsTrue(ex.Message == $"The container registered as 'NewContainer' doesn't exist within the app domain.");
		//	}
		//}
	}
}