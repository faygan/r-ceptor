using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Rceptor.Core.ServiceProxy
{

    public class ServiceBindingContext
    {
        public ICollection<MediaTypeHeaderValue> SupportedMediaTypes { get; set; }
        public ICollection<MediaTypeFormatter> Formatters { get; set; }
        public ActionRouteGenerationOptions RouteBuildOptions { get; set; }
        public Func<IEnumerable<DelegatingHandler>> MessageHandlerProvider { get; set; }

        public static ContentNegotiationResult Negotiate(HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters, Type contentType)
        {
            var negotiator = new DefaultContentNegotiator();
            return negotiator.Negotiate(contentType, request, formatters);
        }

    }
}
