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

        public HttpResponseMessage Send(RestRequestContext context)
        {
            var response = SendInternal(context);
            if (response != null)
                return response;
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        public async Task<HttpResponseMessage> SendAsync(RestRequestContext context)
        {
            return await new Task<HttpResponseMessage>(() => Send(context));
        }

        #endregion

        #region Generics

        public virtual void Dispose()
        {
            ClientHandlers.ForEach(h => h.Dispose());
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Abstract

        protected abstract HttpResponseMessage SendInternal(RestRequestContext context);

        #endregion

    }
}