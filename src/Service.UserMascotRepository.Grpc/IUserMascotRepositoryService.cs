using System.ServiceModel;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.UserMascotRepository.Grpc.Models;

namespace Service.UserMascotRepository.Grpc
{
	[ServiceContract]
	public interface IUserMascotRepositoryService
	{
		[OperationContract]
		ValueTask<MascotProductsGrpcResponse> GetMascotProducts(GetMascotProductsGrpcRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> AddMascotProducts(AddMascotProductsGrpcRequest request);
	}
}