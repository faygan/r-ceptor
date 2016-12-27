using System.Net.Http;
using System.ServiceModel;

namespace Rceptor.Core.Utils
{

    public static class ClientFactory
    {

        public static HttpClient CreateClient(EndpointAddress endPoint, params DelegatingHandler[] clientHandlers)
        {
            // TODO:  HttpClient default handler (new HttpClientHandler()) could be set from service context..

            var client = HttpClientFactory.Create(new HttpClientHandler(), clientHandlers);
            client.BaseAddress = endPoint.Uri;
            return client;
        }

    }

}

