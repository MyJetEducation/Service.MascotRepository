using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Models;
using Service.ServiceBus.Models;
using Service.UserMascotRepository.Grpc;
using Service.UserMascotRepository.Grpc.Models;

namespace Service.UserMascotRepository.Jobs
{
	public class NewMascotProductNotificator
	{
		private readonly ILogger<NewMascotProductNotificator> _logger;
		private readonly IUserMascotRepositoryService _userMascotRepositoryService;

		public NewMascotProductNotificator(ILogger<NewMascotProductNotificator> logger,
			ISubscriber<IReadOnlyList<NewMascotProductServiceBusModel>> subscriber, IUserMascotRepositoryService userMascotRepositoryService)
		{
			_logger = logger;
			_userMascotRepositoryService = userMascotRepositoryService;
			subscriber.Subscribe(HandleEvent);
		}

		private async ValueTask HandleEvent(IReadOnlyList<NewMascotProductServiceBusModel> events)
		{
			foreach (NewMascotProductServiceBusModel message in events)
			{
				string userId = message.UserId;

				_logger.LogInformation("New mascot product for user: {userId}", userId);

				CommonGrpcResponse response = await _userMascotRepositoryService.AddMascotProducts(new AddMascotProductsGrpcRequest
				{
					UserId = userId,
					Products = new[] {message.Product}
				});

				if (response.IsSuccess != true)
					_logger.LogError("Can't add new mascot product for user: {userId}, request: {@request}", userId, message);
			}
		}
	}
}