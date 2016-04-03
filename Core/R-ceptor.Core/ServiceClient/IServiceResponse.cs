using System.Net.Http;

namespace Rceptor.Core.ServiceClient
{

    public interface IServiceResponse
    {
        HttpResponseMessage HttpResponse { get; }
        object GetContentObject();
    }

}