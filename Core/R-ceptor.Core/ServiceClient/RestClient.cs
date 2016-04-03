using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace Rceptor.Core.ServiceClient
{

    public abstract class RestClient : IRestClient
    {

        protected EndpointAddress EndPoint;
        protected DelegatingHandler[] ClientHandlers;

        #region Constructors

        protected RestClient(EndpointAddress endPoint)
            : this(endPoint, null)
        {
            EndPoint = endPoint;
        }

        protected RestClient(EndpointAddress endPoint, params DelegatingHandler[] messageHandlers)
        {
            EndPoint = endPoint;
            ClientHandlers = messageHandlers;
        }

        #endregion 

        #region Send

        public async Task<HttpResponseMessage> Send(RestRequestContext context)
        {
            var response = await SendAsync(context);
            if (response != null)
                return response;
            return
                await new Task<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        #endregion

        #region Generics

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            ClientHandlers.ForEach(h => h.Dispose());
        }

        #endregion

        #region Abstract methods

        protected abstract Task<HttpResponseMessage> SendAsync(RestRequestContext context);

        #endregion

    }
}