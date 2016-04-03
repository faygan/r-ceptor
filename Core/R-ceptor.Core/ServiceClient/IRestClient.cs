using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rceptor.Core.ServiceClient
{

    public interface IRestClient : IDisposable
    {
        Task<HttpResponseMessage> Send(RestRequestContext context);
    }

}