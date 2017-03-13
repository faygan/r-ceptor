using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using Rceptor.Core.ServiceProxy;

namespace Rceptor.Core.ServiceClient
{
    public class ServiceResponse : IServiceResponse
    {

        private readonly ActionResponseTypeContext _typeContext;
        private readonly IEnumerable<MediaTypeFormatter> _formatters;

        public ServiceResponse(HttpResponseMessage responseMessage, ActionResponseTypeContext typeContext = null, IEnumerable<MediaTypeFormatter> formatters = null)
        {
            _typeContext = typeContext;
            _formatters = formatters;

            HttpResponse = responseMessage;
        }

        public HttpResponseMessage HttpResponse { get; protected set; }

        public async Task<string> GetRawContent()
        {
            if (HttpResponse == null)
                throw new ArgumentNullException(nameof(HttpResponse));

            if (HttpResponse.IsSuccessStatusCode)
            {
                return await HttpResponse.Content.ReadAsStringAsync();
            }

            return null;
        }

        public T GetContentObject<T>()
        {
            var result = GetContentObject();
            return (T)result;
        }

        public object GetContentObject()
        {
            if (HttpResponse == null)
                throw new ArgumentNullException(nameof(HttpResponse));
            if (_typeContext == null)
                throw new ArgumentNullException(nameof(_typeContext));
            if (_typeContext.ServiceReplyType == null)
                throw new ArgumentException("Must be provided service reply type. Check service implementation.");

            var httpContent = HttpResponse.Content;

            if (httpContent != null)
            {
                Task<object> contentObject;

                try
                {
                    contentObject = httpContent.ReadAsAsync(_typeContext.ServiceReplyType, _formatters);
                }
                catch (UnsupportedMediaTypeException typeEx)
                {
                    throw new HttpRequestException(typeEx.Message);
                }
                catch (Exception ex)
                {
                    throw new HttpException(ex.Message);
                }

                return contentObject?.Result;
            }

            return null;
        }

        public bool IsSuccess()
        {
            if (HttpResponse != null)
                return HttpResponse.IsSuccessStatusCode;
            return false;
        }
    }
}