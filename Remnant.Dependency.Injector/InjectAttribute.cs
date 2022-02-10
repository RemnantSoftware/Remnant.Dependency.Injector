using System;

namespace Remnant.Dependency.Injector
{
	/// <summary>
	/// Inject field by specifying the 'Type' explicitly, other wise the field declaration type will be used
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class InjectAttribute : Attribute
	{

		/// <summary>
		/// Construct attribute
		/// </summary>
		public InjectAttribute()
		{
		}


		/// <summary>
		/// Construct attribute with the 'Type' to resolve
		/// </summary>
		/// <param name="type"></param>
		public InjectAttribute(Type type)
		{
			Type = type;
		}

		/// <summary>
		/// The 'Type' to resolve
		/// </summary>
		public Type Type { get; }
	}
}
