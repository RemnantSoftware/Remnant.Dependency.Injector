namespace Remnant.Container.Injector.Tests
{
	public interface IAnimal
	{
		string Name { get; set; }
		string MakeSound();
	}

	public class Dog : IAnimal
	{
		public string Name { get; set; } = "Dog";

		public string MakeSound()
		{
			return "woof";
		}
	}

	public class Cat : IAnimal
	{
		public string Name { get; set; } = "Cat";

		public string MakeSound()
		{
			return "miau";
		}
	}
}
