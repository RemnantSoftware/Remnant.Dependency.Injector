using System;

namespace Remnant.Container.Injector
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class InjectAttribute : Attribute
	{

		public InjectAttribute(Type type)
		{
			Type = type;
		}

		public Type Type { get; }
	}
}
