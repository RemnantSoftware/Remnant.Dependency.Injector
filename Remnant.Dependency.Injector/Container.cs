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
		private static Container _container = null;
		private readonly List<ContainerObject> _containerObjects = new List<ContainerObject>();

		/// <summary>
		/// Construct the global domain container. There can only be one container constructed within your app domain.
		/// There is no need to keep a reference to the container after you registered all your transients/singletons.
		/// </summary>
		/// <param name="name">Provide a name for the container</param>
		/// <returns>Returns the container instance</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static Container Create(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("The name of the container cannot be null or empty.");

			if (AppDomain.CurrentDomain.GetData(name) != null ||
				AppDomain.CurrentDomain.GetData("RemnantContainer") != null)
				throw new InvalidOperationException("There is already a container created. Only one container allowed in the app domain.");

			_name = name;

			_container = new Container();
			AppDomain.CurrentDomain.SetData(name, _container);
			AppDomain.CurrentDomain.SetData("RemnantContainer", name);
			return _container;
		}


		public static Container Instance
		{
			get
			{
				if (_container == null)
					throw new ApplicationException("The container has not been created. Please use Container.Create([container name]) first.");
				return _container;
			}
		}

		/// <summary>
		/// Resolve using static on container class
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		/// <returns>Returns a transient or singleton instance of the specified type</returns>
		public static TType Resolve<TType>()
			where TType : class
		{
			return _container?.Resolve<TType>();
		}

		private Container()
		{
		}

		private ContainerObject AddObject(ContainerObject containerObject)
		{
			_containerObjects.Add(containerObject);
			return containerObject;
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
		public Container Register<TType>(object instance) where TType : class
		{
			AddObject(new ContainerObject(typeof(TType), instance.GetType(), instance));
			return this;
		}

		/// <summary>
		/// Register a singleton with the container
		/// </summary>
		/// <param name="instance">The singelton instance</param>
		/// <returns>Returns the container</returns>
		public Container Register(object instance)
		{
			AddObject(new ContainerObject(instance.GetType(), instance.GetType(), instance));
			return this;
		}

		/// <summary>
		/// Register a transient with the container
		/// </summary>
		/// <typeparam name="TType">The type that will be used to resolve and construct entry</typeparam>
		/// <returns>Returns the container</returns>
		public Container Register<TType>() where TType : class, new()
		{
			Register<TType, TType>();
			return this;
		}

		/// <summary>
		/// Register a transient with the container
		/// </summary>
		/// <typeparam name="TType">The type that will be used to resolve entry</typeparam>
		/// <typeparam name="TObject">The type that will be constructed and return on resolve</typeparam>
		/// <returns>Returns the container</returns>
		public Container Register<TType, TObject>()
			where TType : class
			where TObject : class, new()
		{
			//Register<TType>(new TObject());

			_containerObjects.Add(new ContainerObject(typeof(TType), typeof(TObject), null));

			return this;
		}

		/// <summary>
		/// Deregister a container entry using generic type
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		public Container DeRegister<TType>()
			where TType : class
		{
			var containerObject = _containerObjects.FirstOrDefault((m => m.Type == typeof(TType)));

			if (containerObject != null)
			{
				_containerObjects.Remove(containerObject);
			}
			return this;
		}

		/// <summary>
		/// Deregister a container entry using instance
		/// </summary>
		/// <param name="instance">The type of instance that will be removed from the container</param>
		public Container DeRegister(object instance)
		{
			var containerObject = _containerObjects.FirstOrDefault((m => m.Type == instance.GetType()));

			if (containerObject != null)
			{
				_containerObjects.Remove(containerObject);
			}
			return this;
		}

		/// <summary>
		/// Clear container from all registeries
		/// </summary>

		public Container Clear()
		{
			_containerObjects.Clear();
			return this;
		}


		/// <summary>
		/// Resolve type to get the instance
		/// </summary>
		/// <typeparam name="TType">The type that was registered</typeparam>
		/// <returns>Returns a transient or singleton instance of the specified type</returns>
		public TType ResolveInstance<TType>()
			where TType : class
		{
			ContainerObject containerObject = null;

			if (_containerObjects.Exists(m => m.Type == typeof(TType)))
				containerObject = _containerObjects.FirstOrDefault(m => m.Type == typeof(TType));

			if (containerObject == null)
				throw new ArgumentException($"The container cannot resolve requested object '{typeof(TType).FullName}'.");

			return containerObject.Object != null
				? (TType)containerObject.Object
				: (TType)Activator.CreateInstance(containerObject.ObjectType);	
		}
	}
}
