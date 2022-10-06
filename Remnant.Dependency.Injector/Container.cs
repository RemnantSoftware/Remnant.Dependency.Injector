using Remnant.Dependency.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remnant.Dependency.Injector
{
	/// <summary>
	/// The container is constructed and added to your app domain for global access.
	/// 	The object class is extended with 'Resolve<TType> method and any object anywhere in your code can call:
	///	var instance = Resolve<TType>();
	/// </summary>
	public sealed class Container
	{
		private static string _name;
		private static IContainer _container = null;

		/// <summary>
		/// Construct the global domain container. There can only be one container constructed within your app domain.
		/// There is no need to keep a reference to the container after you registered all your transients/singletons.
		/// </summary>
		/// <param name="name">Provide a name for the container</param>
		/// <param name="container">If not specified then Remnant's own container is used, otherwise the container you want to use</param>
		/// <returns>Returns the container instance</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static IContainer Create(string name, IContainer container = null)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("The name of the container cannot be null or empty.");

			if (AppDomain.CurrentDomain.GetData(name) != null ||
				AppDomain.CurrentDomain.GetData("RemnantContainer") != null)
				throw new InvalidOperationException("There is already a container created. Only one container allowed in the app domain.");

			_name = name;

			_container = container ?? new RemnantContainer();
			AppDomain.CurrentDomain.SetData(name, _container);
			AppDomain.CurrentDomain.SetData("RemnantContainer", name);
			return _container;
		}

		/// <summary>
		/// Getcontainer instance
		/// </summary>
		private static void ValidateContainer()
		{
				if (_container == null)
					throw new ApplicationException("The container has not been created. Please use Container.Create([container name]) first.");
		}

		/// <summary>
		/// Resolve using static on container class
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		/// <returns>Returns a singleton instance of the specified type</returns>
		public static TType Resolve<TType>()
			where TType : class
		{
			ValidateContainer();

			if (_container == null)
				throw new InvalidOperationException("The container is not created. First call 'Create()'.");

			return _container?.Resolve<TType>();
		}

		private Container()
		{
		}

		/// <summary>
		/// The name of the container
		/// </summary>
		public string Name => _name;

		/// <summary>
		/// Register a singleton using a specified type to resolve
		/// </summary>
		/// <typeparam name="TType">The type that will be used to resolve the singleton entry</typeparam>
		/// <param name="instance">The singleton instance</param>
		/// <returns>Returns the container</returns>
		public static IContainer Register<TType>(object instance) where TType : class
		{
			ValidateContainer();

			_container.Register<TType>(instance);
			return _container;
		}

		/// <summary>
		/// Register a singleton with the container
		/// </summary>
		/// <param name="type">The type that will be used to resolve the singleton entry</param>
		/// <param name="instance">The singelton instance</param>
		/// <returns>Returns the container</returns>
		public static IContainer Register(Type type, object instance)
		{
			ValidateContainer();

			_container.Register(type, instance);
			return _container;
		}

		/// <summary>
		/// Register a singleton with the container
		/// </summary>
		/// <param name="instance">The singelton instance</param>
		/// <returns>Returns the container</returns>
		public static IContainer Register(object instance)
		{
			ValidateContainer();

			_container.Register(instance.GetType(), instance);
			return _container;
		}

		/// <summary>
		/// Register a singleton type with the container
		/// </summary>
		/// <typeparam name="TType">The type that will be used to resolve and construct entry</typeparam>
		/// <returns>Returns the container</returns>
		public static IContainer Register<TType>() where TType : class, new()
		{
			Register<TType, TType>();
			return _container;
		}

		/// <summary>
		/// Register a singleton type with the container
		/// </summary>
		/// <typeparam name="TType">The type that will be used to resolve entry</typeparam>
		/// <typeparam name="TObject">The type that will be constructed and return on resolve</typeparam>
		/// <returns>Returns the container</returns>
		public static IContainer Register<TType, TObject>()
			where TType : class
			where TObject : class, new()
		{
			ValidateContainer();

			_container.Register<TType, TObject>();
			return _container;
		}

		/// <summary>
		/// Deregister a container entry using generic type
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		public static IContainer DeRegister<TType>()
			where TType : class
		{
			ValidateContainer();

			_container.DeRegister<TType>();
			return _container;
		}

		/// <summary>
		/// Deregister a container entry using instance
		/// </summary>
		/// <param name="instance">The type of instance that will be removed from the container</param>
		public static IContainer DeRegister(object instance)
		{
			ValidateContainer();

			_container.DeRegister(instance);
			return _container;
		}

		/// <summary>
		/// Clear container from all registeries
		/// </summary>
		public static IContainer Clear()
		{
			ValidateContainer();

			_container.Clear();
			return _container;
		}

		/// <summary>
		/// Resolve type to get the instance
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		/// <returns>Returns a transient or singleton instance of the specified type</returns>
		public static TType ResolveInstance<TType>()
			where TType : class
		{
			ValidateContainer();

			return _container.ResolveInstance<TType>();	
		}

		/// <summary>
		/// Returns the internal container that to be used for direct access
		/// </summary>
		/// <typeparam name="TContainer"></typeparam>
		/// <returns></returns>
		/// <exception cref="InvalidCastException">Specify the type for the internal container. Exception will be thrown if casting fails.</exception>
		public static TContainer InternalContainer<TContainer>() where TContainer : class
		{
			ValidateContainer();

			var internalContainer = _container.InternalContainer<TContainer>();

			if (internalContainer == null)
				throw new InvalidCastException($"The internal container is of type {internalContainer.GetType().Name} and cannot be cast to {typeof(TContainer).Name}");

			return internalContainer;
		}
	}
}
