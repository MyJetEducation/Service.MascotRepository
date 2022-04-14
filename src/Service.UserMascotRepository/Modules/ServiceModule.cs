using Autofac;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.ServiceBus;
using MyServiceBus.Abstractions;
using MyServiceBus.TcpClient;
using Service.ServerKeyValue.Client;
using Service.ServiceBus.Models;

namespace Service.UserMascotRepository.Modules
{
	public class ServiceModule : Module
	{
		private const string QueueName = "MyJetEducation-UserMascotRepository";

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterServerKeyValueClient(Program.Settings.ServerKeyValueServiceUrl, Program.LogFactory.CreateLogger(typeof(ServerKeyValueClientFactory)));

			MyServiceBusTcpClient serviceBusClient = builder.RegisterMyServiceBusTcpClient(Program.ReloadedSettings(e => e.ServiceBusReader), Program.LogFactory);
			builder.RegisterMyServiceBusSubscriberBatch<NewMascotProductServiceBusModel>(serviceBusClient, NewMascotProductServiceBusModel.TopicName, QueueName, TopicQueueType.Permanent);
		}
	}
}