using System.Runtime.Serialization;
using Service.MarketProduct.Domain.Models;

namespace Service.UserMascotRepository.Grpc.Models
{
	[DataContract]
	public class AddMascotProductsGrpcRequest
	{
		[DataMember(Order = 1)]
		public string UserId { get; set; }

		[DataMember(Order = 2)]
		public MarketProductType[] Products { get; set; }
	}
}