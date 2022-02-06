# Remnant.Container.Injector
Global container for dependency injection (not using constructor arguments to determine dependency)

Example: using Log4Net

// register with the container

RemContainer
  .Create("MyContainer")
  .Register<ILog>(LogManager.GetLogger("Example");


public class SomeProcess
{
  private ILog _logger;
  
  public SomeProcess()
  {
    _logger = this.Resolve<ILog>();
  }

  public void Run()
  {
    _logger.Info("Process started...");
  }
  
  public void Stop()
  {
    _logger.Info("Process stopped.");
  }
}
