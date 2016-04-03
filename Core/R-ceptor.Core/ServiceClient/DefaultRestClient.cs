using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Rceptor.Core.Utils;

namespace Rceptor.Core.ServiceClient
{

    public class DefaultRestClient : RestClient
    {

        #region Constructors

        public DefaultRestClient(EndpointAddress endPoint)
            : base(endPoint, null)
        {
        }

        public DefaultRestClient(EndpointAddress endPoint, params DelegatingHandler[] messageHandlers)
            : base(endPoint, messageHandlers)
        {
        }

        #endregion 

        #region Send

        protected override async Task<HttpResponseMessage> SendAsync(RestRequestContext context)
        {
            HttpResponseMessage responseMessage;

            using (var client = ClientFactory.CreateClient(EndPoint, ClientHandlers))
            {
                var request = context.GetRequestMessage();

                try
                {
                    responseMessage = await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    responseMessage = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return await Task.FromResult(responseMessage);
        }

        #endregion


    }
}