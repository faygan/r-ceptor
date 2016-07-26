using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rceptor.Core.ServiceClient
{

    public interface IRestClient : IDisposable
    {
        HttpResponseMessage Send(RestRequestContext context);
        Task<HttpResponseMessage> SendAsync(RestRequestContext context);
    }

}