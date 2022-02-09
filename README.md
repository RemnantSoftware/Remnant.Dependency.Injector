# Remnant Container Injector

## Overview

- The injection is a pull pattern unlike all other DI containers which follows a push pattern.
- The pull pattern has no need to declare constructor arguments for DI, and also no hierarchical DI wiring is required.
- The container is globally instantiated within your current app domain.
- Anywhere, anyplace in your code the container can be requested to resolve your request.
- A extension method 'Resolve' is implemented on 'object' to allow any objects to call the container. 
- You can use the [Inject] attribute on fields which will automatically inject the object.
- The pull pattern nullifies the need for transient objects (cause no wiring dependencies needed). So basically only singletons need to be registered.

> **Note**: The design and implementation is still in beta, nuget pacakge(s) will be published later.
> Also class names not finalized and may change.

## Usage:

### Container

Construct the container and register components:

```csharp
class Program
{
    public static async Task Main()
    {
        RemContainer
            .Create("MyContainer")
            .Register<ILog>(new MyLogger())
            .Register<IRepository>(new MyRepository());
    }
}
```

### Resolve

Call resolve to obtain objects from the container:

```csharp
// Example: using the core base object
var logger = new object().Resolve<ILog>();
```


```csharp
// Example: how to resolve on field declaration
public class PurchaseOrder
{
    private readonly ILog _logger = RemContainer.Resolve<ILog>();
    private readonly IRepository _repository = RemContainer.Resolve<IRepository>();
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

## Usage of '[Inject]' Attribute

By decorating your fields with the inject attribute, you dont have to specify 'Resolve' explicitly.
Remnant will use the roslyn code generator to scan fields with the attribute, and automatically generate the code.
But that means you must specify your class as partial.


```csharp
// Example how to resolve on class constructor
public partial class PurchaseOrder
{
    [Inject(typeof(ILog))]
    private readonly ILog _logger;
    
    [Inject(typeof(IRepository))] 
    private readonly IRepository _repository;
}
```

-> the attribute argument I can derive from the field type, but I am still deciding if I should make it optional or remove it...

