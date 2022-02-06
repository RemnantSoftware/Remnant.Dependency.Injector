using log4net;
using Remnant.Container.Injector;

namespace Remnant.ContainerInjector.Example
{
	public class EmailService
	{
		private readonly ILog? _logger = RemContainer.Resolve<ILog>();


		public void SendEmail(string recipient, string message)
		{
			_logger?.Debug($"Send email to {recipient}: {message}");
		}
	}
}
