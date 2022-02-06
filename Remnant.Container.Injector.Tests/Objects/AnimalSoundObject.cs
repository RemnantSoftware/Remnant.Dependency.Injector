namespace Remnant.Container.Injector.Tests
{
	public class AnimalSoundObject
	{
		private IAnimal? _animal = RemContainer.Resolve<IAnimal>();

		public AnimalSoundObject()
		{
		}

		public IAnimal? Animal=> _animal;
	}

	public class AnimalSoundObject2
	{
		private IAnimal? _animal;

		public AnimalSoundObject2()
		{
			_animal = this.Resolve<IAnimal>();
		}

		public IAnimal? Animal => _animal;
	}
}
