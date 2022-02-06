// See https://aka.ms/new-console-template for more information
using log4net;
using log4net.Config;
using Remnant.Container.Injector;
using Remnant.ContainerInjector.Example;
using System.Text;

Console.WriteLine("Hello, welcome to a different way to get transient and singleton objects within your application!");
Console.WriteLine("Most dependency looks at constructors and the entire class DI hierarchical must be wired from the root.");
Console.WriteLine("Using the Remnant container, you don't need to do that...");
Console.WriteLine();
Console.WriteLine();


// need encoding for log4net
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

XmlConfigurator.Configure(configFile: new FileInfo("log4net.config"));
var log = LogManager.GetLogger("Example");
log.Debug("Log 4 net is operational.");

// lets create the container (no need to keep a reference once registeries are done)
RemContainer
	.Create("ExampleContainer")
	.Register<ILog>(log)                  // is registered as a singleton, note log is of LogImpl, but resolve as ILog
	.Register(new EmailService())     // is registered as a singleton
	.Register<Customer>()             // is registered as a transient
	.Register(new List<Customer> { new Customer { Name = "John" }, new Customer { Name = "Jane" } });

// get transient from the container
var customer = RemContainer.Resolve<Customer>();
customer.Name = "Neill";
customer.SendMessage("Hey! time for a beer!!");

// or create directly, no need to use the container
customer = new Customer { Name = "Verreynne" };
customer.SendMessage("Hey! time for a beer!!");

Console.WriteLine();
Console.WriteLine("As you can see above, the log dependency is already resolved, and there is no need to pass down on constructors via root wiring. This means we don't really have the need anymore to register transient objects with a container.  Cause the container doesn't have to do any DI wiring.");
Console.WriteLine();

Console.WriteLine("Use container to get list of customers: var customers = Container.Resolve<List<Customer>>();");
var customers = RemContainer.Resolve<List<Customer>>();
customers.ForEach(c => Console.WriteLine(c.Name));
Console.WriteLine();

Console.WriteLine("Use any object to get list of customers: customers = new object().Resolve<List<Customer>>();");
customers = new object().Resolve<List<Customer>>();
customers.ForEach(c => Console.WriteLine(c.Name));

Console.WriteLine();
Console.WriteLine("Press any to exit...");
Console.ReadKey();