using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.MarketProduct.Domain.Models;
using Service.UserMascotRepository.Client;
using Service.UserMascotRepository.Grpc;
using Service.UserMascotRepository.Grpc.Models;
using GrpcClientFactory = ProtoBuf.Grpc.Client.GrpcClientFactory;

namespace TestApp
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;
			ILogger<Program> logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new UserMascotRepositoryClientFactory("http://localhost:5001", logger);
			IGrpcServiceProxy<IUserMascotRepositoryService> serviceProxy = factory.GetUserMascotRepositoryService();
			IUserMascotRepositoryService client = serviceProxy.Service;

			var userId = Guid.NewGuid().ToString();

			MascotProductsGrpcResponse get1Response = await client.GetMascotProducts(new GetMascotProductsGrpcRequest
			{
				UserId = userId
			});
			if ((get1Response?.Products).IsNullOrEmpty() == false)
				throw new Exception("New user has products");

			CommonGrpcResponse save1Response = await client.AddMascotProducts(new AddMascotProductsGrpcRequest
			{
				UserId = userId,
				Products = new[]
				{
					MarketProductType.MascotEmotion,
					MarketProductType.MascotSkin
				}
			});
			if (save1Response.IsSuccess != true)
				throw new Exception("Can't save new products");

			MascotProductsGrpcResponse get2Response = await client.GetMascotProducts(new GetMascotProductsGrpcRequest
			{
				UserId = userId
			});
			if ((get2Response?.Products).IsNullOrEmpty())
				throw new Exception("New user has no products after save");

			CommonGrpcResponse save2Response = await client.AddMascotProducts(new AddMascotProductsGrpcRequest
			{
				UserId = userId,
				Products = new[]
				{
					MarketProductType.MascotEmotion,
				}
			});
			if (save2Response.IsSuccess)
				throw new Exception("Invalid saving duplicate product");

			CommonGrpcResponse save3Response = await client.AddMascotProducts(new AddMascotProductsGrpcRequest
			{
				UserId = userId,
				Products = Array.Empty<MarketProductType>()
			});
			if (save3Response.IsSuccess)
				throw new Exception("Invalid saving empty products");

			Console.WriteLine(JsonConvert.SerializeObject(get2Response));

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}