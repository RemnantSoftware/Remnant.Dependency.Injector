# Remnant Container Injector

##Overview

- The injection is a pull pattern unlike all other DI containers which follows a push pattern.
- The pull pattern has no need to declare constructor arguments for DI and no hierarchical DI wiring.
- The container is globally instantatied within the current app domain.
- Anywhere, anyplace in your code the container can be requested (pull) to resolve your request.
- A extension 'Resolve' method is implemented on 'object' to allow any object to request an object from the container. 
- You can use the [Inject] attribute on fields which will automatically the type that is requested.
- The pull pattern nullifies the need for transient objects (no wiring required). So basically only singletons need to be registered.

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

Call resolve to obtain objects from the container:

```csharp

// for example the core base object can be used 
var logger = new object().Resolve<ILog>();

// an example how to resolve on field declaration
public class PurchaseOrder
{
    private readonly IRepository _repository = RemContainer.Resolve<ILog>();
}

// an example how to resolve on class constructor
public class PurchaseOrder
{
    private readonly IRepository _repository;
    
    public PurchaseOrder()
    {
        _repository = this.Resolve<ILog>();
    }
}


```
