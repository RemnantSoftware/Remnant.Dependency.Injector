using log4net;
using Remnant.Container.Injector;

namespace Remnant.ContainerInjector.Example
{
	public class Customer
	{
		private readonly ILog _logger;
		private readonly EmailService _emailService;

		public Customer()
		{
			_logger = this.Resolve<ILog>();
			_logger.Warn("The logger has been resolved without constructor dependency injection.");
			_logger.Warn("NB: This is just an illustration, NEVER use an email service from within a business domain class! :-)");

			_emailService = this.Resolve<EmailService>();	
		}

		public string Name { get; set; }

		public void SendMessage(string message)
		{
			_emailService.SendEmail(Name, message);
		}
	}
}
