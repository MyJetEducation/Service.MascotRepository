using Autofac;
using Microsoft.Extensions.Logging;
using Service.UserMascotRepository.Grpc;
using Service.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.UserMascotRepository.Client
{
	public static class AutofacHelper
	{
		public static void RegisterUserMascotRepositoryClient(this ContainerBuilder builder, string grpcServiceUrl, ILogger logger)
		{
			var factory = new UserMascotRepositoryClientFactory(grpcServiceUrl, logger);

			builder.RegisterInstance(factory.GetUserMascotRepositoryService()).As<IGrpcServiceProxy<IUserMascotRepositoryService>>().SingleInstance();
		}
	}
}
