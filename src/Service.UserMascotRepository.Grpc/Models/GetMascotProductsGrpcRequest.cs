using System.Runtime.Serialization;

namespace Service.UserMascotRepository.Grpc.Models
{
	[DataContract]
	public class GetMascotProductsGrpcRequest
	{
		[DataMember(Order = 1)]
		public string UserId { get; set; }
	}
}