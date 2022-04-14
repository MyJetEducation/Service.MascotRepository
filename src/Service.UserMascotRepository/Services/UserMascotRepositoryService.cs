using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.MarketProduct.Domain.Models;
using Service.ServerKeyValue.Grpc;
using Service.ServerKeyValue.Grpc.Models;
using Service.UserMascotRepository.Grpc;
using Service.UserMascotRepository.Grpc.Models;

namespace Service.UserMascotRepository.Services
{
	public class UserMascotRepositoryService : IUserMascotRepositoryService
	{
		private readonly ILogger<UserMascotRepositoryService> _logger;
		private readonly IGrpcServiceProxy<IServerKeyValueService> _serverKeyValueService;

		private static Func<string> KeyUserMascot => Program.ReloadedSettings(model => model.KeyUserMascot);

		public UserMascotRepositoryService(ILogger<UserMascotRepositoryService> logger, IGrpcServiceProxy<IServerKeyValueService> serverKeyValueService)
		{
			_logger = logger;
			_serverKeyValueService = serverKeyValueService;
		}

		public async ValueTask<MascotProductsGrpcResponse> GetMascotProducts(GetMascotProductsGrpcRequest request) => new MascotProductsGrpcResponse
		{
			Products = await GetProducts(request.UserId)
		};

		public async ValueTask<CommonGrpcResponse> AddMascotProducts(AddMascotProductsGrpcRequest request)
		{
			MarketProductType[] requestProducts = request.Products;
			if (requestProducts.IsNullOrEmpty())
			{
				_logger.LogError("No products filled for adding as mascot products for user: {user}, request: {@request}", request.UserId, request);

				return CommonGrpcResponse.Fail;
			}

			MarketProductType[] userProducts = await GetProducts(request.UserId);
			if (userProducts.Intersect(requestProducts).Any())
			{
				_logger.LogError("User {user} already has mascot products filled in request: {@request}", request.UserId, request);

				return CommonGrpcResponse.Fail;
			}

			MarketProductType[] finalProducts = userProducts
				.Union(requestProducts)
				.Distinct()
				.ToArray();

			return await _serverKeyValueService.TryCall(service => service.Put(new ItemsPutGrpcRequest
			{
				UserId = request.UserId,
				Items = new[]
				{
					new KeyValueGrpcModel
					{
						Key = KeyUserMascot.Invoke(),
						Value = JsonSerializer.Serialize(finalProducts)
					}
				}
			}));
		}

		public async ValueTask<MarketProductType[]> GetProducts(string userId)
		{
			ValueGrpcResponse response = await _serverKeyValueService.Service.GetSingle(new ItemsGetSingleGrpcRequest
			{
				UserId = userId,
				Key = KeyUserMascot.Invoke()
			});

			string value = response?.Value;

			return value == null
				? Array.Empty<MarketProductType>()
				: JsonSerializer.Deserialize<MarketProductType[]>(value);
		}
	}
}