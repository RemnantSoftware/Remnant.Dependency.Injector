using System;

namespace Remnant.Dependency.Injector
{
	public class ContainerObject
	{
		public ContainerObject(Type type, Type objectType, object @object)
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