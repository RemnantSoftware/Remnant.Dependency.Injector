namespace Remnant.Container.Injector
{
	public class RemContainerObject
	{
		public RemContainerObject(Type type, Type objectType, object @object)
		{
			Name = type.Name;
			ObjectType = objectType;	
			Object = @object;
			Type = type;
		}

		public string Name { get; }
		public object Object { get; }
		public Type ObjectType { get;  }
		public Type Type { get;  }

	}
}