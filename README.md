# Remnant Dependency Injector (beta)

## Overview

- The injection is a pull pattern unlike all other DI containers which follows a push pattern.
- The pull pattern has no need to declare constructor arguments for DI, and also no hierarchical DI wiring is required.
- The container is globally instantiated within your current app domain.
- Anywhere, any place in your code the container can be requested to resolve the object needed.
- An extension method 'Resolve<<TType>>' is implemented on 'object' to allow any objects to call the container. 
- You can use the [Inject] attribute on fields which will automatically inject the object.
- The pull pattern nullifies the need for transient objects (cause no wiring dependencies needed). So basically only singletons need to be registered.

> **Note**: To use [Inject] attribute to decorate class fields, the class must be specified as partial.
        
> **Note**: The unit tests in the Tests project must be run per DI container for testing (because only one registered container is allowed in the app domain). 

## Nuget packages:

- Core package excluding the analyzer: 

        Install-Package Remnant.Dependency.Injector -Version 1.0.4

- Anaylzer package to use [Inject] attribute: 

        Install-Package Remnant.Dependency.Injector.Analyzer -Version 1.0.3

> **Note**: I suggest you install both packages and use [Inject] attribute for injection.
        
## Adapters for other DI containers:
        
> https://github.com/RemnantSoftware/Remnant.Dependency.Unity
        
> https://github.com/RemnantSoftware/Remnant.Dependency.Ninject
        
> https://github.com/RemnantSoftware/Remnant.Dependency.Autofac
        
> https://github.com/RemnantSoftware/Remnant.Dependency.CastleWindsor
        
> https://github.com/RemnantSoftware/Remnant.Dependency.SimpleInjector
  
  ~ Additional adapters for other DI solutions can be coded by implementing the interface 'IContainer'. 
        
## Usage:

### Container

Construct the container and register components:

```csharp
class Program
{
    public static async Task Main()
    {
          Container
            .Create("MyContainer")
            .Register<ILog>(new MyLogger())
            .Register<IRepository>(new MyRepository());
    }
}
```
        
If you need to perform additional registrations/resolves at other places, you can access the instance of the container as follows:
```csharp        
class Program
{
    public static async Task Initialize()
    {
          Container
            .Instance
            .Register<IConfiguration>(new AzureAppConfig());
    }
}
```
 
----------        
        
### Manually Resolve (not using the [Inject] attribute)

Call resolve to obtain objects from the container:

```csharp
// Example: using the core base object
var logger = new object().Resolve<ILog>();
```


```csharp
// Example: how to resolve on field declaration
public class PurchaseOrder
{
    private readonly ILog _logger = Container.Resolve<ILog>();
    private readonly IRepository _repository = Container.Resolve<IRepository>();
}
```

```csharp
// Example: how to resolve on class constructor
public class PurchaseOrder
{
    private readonly ILog _logger;
    private readonly IRepository _repository;
    
    public PurchaseOrder()
    {
        _logger = this.Resolve<ILog>();
        _repository = this.Resolve<IRepository>();
    }
}
```

-------
        
### [Inject] Attribute

By decorating your fields with the inject attribute, you dont have to specify 'Resolve' explicitly.
Remnant will use the roslyn code generator to scan fields with the attribute, and automatically generate the code.
But that means you must specify your class as partial.
        
#### Important, if your class has already a constructor method implemented: 
The analyzer can't generate a constructor with the injection code because the method exists already. 
In that case the Analyzer generates a public 'Inject' method, as well as a static 'Create' method on the class. 
So you can call the static 'Create' method which will create an instance of the class and it calls the 'Inject' method to perform the injection. 
Or you can instantiate the class and call the 'Inject' method yourself. Keep in mind that your constructor implementation all the injected 
fields will be null unless you explicitly call the method 'Inject' on your constructor before accessing the fields.


```csharp
// Example of specifying the type
public partial class PurchaseOrder
{
    [Inject(typeof(ILog))]
    private readonly ILog _logger;
    
    [Inject(typeof(IRepository))] 
    private readonly IRepository _repository;
}
```

The 'Type' passed to the [Inject] attribute is optional, and the underlying decorated class field's data type is inferred

```csharp
// Example of using the inferred field's data type
public partial class PurchaseOrder
{
    [Inject]
    private readonly ILog _logger;
    
    [Inject] 
    private readonly IRepository _repository;
}
```

```csharp
// Example of using the static 'Create', fields cannot be read only
public partial class PurchaseOrder
{
    [Inject]
    private ILog _logger;
    
    [Inject] 
    private IRepository _repository;
        
    public PurchaseOrder()
    {
       //...do some stuff   
    }
}
        
var purchaseOrder = PurchaseOrder.Create();
```
        
```csharp
// Example of calling 'Inject' explicitly, fields cannot be read only
public partial class PurchaseOrder
{
    [Inject]
    private ILog _logger;
    
    [Inject] 
    private IRepository _repository;
    
    public PurchaseOrder()
    {
        Inject();
        
       //...do some stuff   
    }
}
```
