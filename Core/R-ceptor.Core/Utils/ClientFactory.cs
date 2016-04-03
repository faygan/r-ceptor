using System.Net.Http;
using System.ServiceModel;

namespace Rceptor.Core.Utils
{

    public static class ClientFactory
    {

        public static HttpClient CreateClient(EndpointAddress endPoint, params DelegatingHandler[] clientHandlers)
        {
            var client = HttpClientFactory.Create(clientHandlers);
            client.BaseAddress = endPoint.Uri;
            return client;
        }
    }

}
