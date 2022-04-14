using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Service.UserMascotRepository.Grpc;
using Service.Grpc;

namespace Service.UserMascotRepository.Client
{
	[UsedImplicitly]
	public class UserMascotRepositoryClientFactory : GrpcClientFactory
	{
		public UserMascotRepositoryClientFactory(string grpcServiceUrl, ILogger logger) : base(grpcServiceUrl, logger)
		{
		}

		public IGrpcServiceProxy<IUserMascotRepositoryService> GetUserMascotRepositoryService() => CreateGrpcService<IUserMascotRepositoryService>();
	}
}
