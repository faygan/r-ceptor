using System.Net.Http;
using System.Threading.Tasks;

namespace Rceptor.Core.ServiceClient
{

    public interface IServiceResponse
    {
        HttpResponseMessage HttpResponse { get; }
        object GetContentObject();
        T GetContentObject<T>();
        Task<string> GetRawContent();
        bool IsSuccess();
    }

}