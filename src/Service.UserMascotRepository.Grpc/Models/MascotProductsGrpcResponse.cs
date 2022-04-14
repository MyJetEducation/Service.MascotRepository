using System.Runtime.Serialization;
using Service.MarketProduct.Domain.Models;

namespace Service.UserMascotRepository.Grpc.Models
{
	[DataContract]
	public class MascotProductsGrpcResponse
	{
		[DataMember(Order = 1)]
		public MarketProductType[] Products { get; set; }
	}
}