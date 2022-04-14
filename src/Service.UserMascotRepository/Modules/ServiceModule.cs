using Autofac;
using Microsoft.Extensions.Logging;
using Service.ServerKeyValue.Client;

namespace Service.UserMascotRepository.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterServerKeyValueClient(Program.Settings.ServerKeyValueServiceUrl, Program.LogFactory.CreateLogger(typeof(ServerKeyValueClientFactory)));
		}
	}
}