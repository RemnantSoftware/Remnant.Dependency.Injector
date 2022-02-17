using Remnant.Dependency.Interface;
using System;

namespace Remnant.Dependency.Injector
{
	public static class ExtendObject
	{
		/// <summary>
		/// Resolve using the global container
		/// </summary>
		/// <typeparam name="TType">The type to resolve</typeparam>
		/// <param name="source"></param>
		/// <returns>Returns a transient or singleton instance</returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static TType Resolve<TType>(this object source)
			where TType : class
		{
			var containerName = AppDomain.CurrentDomain.GetData("RemnantContainer");
		
			if (string.IsNullOrEmpty((string)containerName))
				throw new NullReferenceException($"There is no container registered with the app domain. Unable to resolve {typeof(TType).FullName}.");

			var container = AppDomain.CurrentDomain.GetData((string)containerName);

			if (container == null || container as IContainer == null)
				throw new NullReferenceException($"The container registered as '{containerName}' doesn't exist within the app domain.");

			var instance = (container as IContainer).ResolveInstance<TType>();

			if (instance == null)
				throw new ArgumentException($"The container cannot resolve requested object '{typeof(TType).FullName}'.");

			return instance;

		}
	}
}
