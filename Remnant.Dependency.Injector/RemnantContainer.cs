using Remnant.Dependency.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remnant.Dependency.Injector
{
	public sealed class RemnantContainer : IContainer
	{
		private readonly List<ContainerObject> _containerObjects = new List<ContainerObject>();

		public RemnantContainer()
		{

		}

		public static TType Resolve<TType>()
			where TType : class
		{
			return Resolve<TType>();
		}

		private ContainerObject AddObject(ContainerObject containerObject)
		{
			_containerObjects.Add(containerObject);
			return containerObject;
		}

		public IContainer Register<TType>(object instance) where TType : class
		{
			AddObject(new ContainerObject(typeof(TType), instance.GetType(), instance));
			return this;
		}

		public IContainer Register(Type type, object instance)
		{
			AddObject(new ContainerObject(type, instance.GetType(), instance));
			return this;
		}

		public IContainer Register(object instance)
		{
			AddObject(new ContainerObject(instance.GetType(), instance.GetType(), instance));
			return this;
		}

		public IContainer Register<TType>() where TType : class, new()
		{
			Register<TType, TType>();
			return this;
		}

		public IContainer Register<TType, TObject>()
			where TType : class
			where TObject : class, new()
		{
			_containerObjects.Add(new ContainerObject(typeof(TType), typeof(TObject), null));
			return this;
		}

		public IContainer DeRegister<TType>()
			where TType : class
		{
			var containerObject = _containerObjects.FirstOrDefault((m => m.Type == typeof(TType)));

			if (containerObject != null)
			{
				_containerObjects.Remove(containerObject);
			}
			return this;
		}

		public IContainer DeRegister(object instance)
		{
			var containerObject = _containerObjects.FirstOrDefault((m => m.Type == instance.GetType()));

			if (containerObject != null)
			{
				_containerObjects.Remove(containerObject);
			}
			return this;
		}

		public IContainer Clear()
		{
			_containerObjects.Clear();
			return this;
		}

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
