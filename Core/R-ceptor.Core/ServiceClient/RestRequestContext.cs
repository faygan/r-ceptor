using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Rceptor.Core.ServiceProxy;

namespace Rceptor.Core.ServiceClient
{

    public class RestRequestContext
    {

        public HttpMethod[] ApiMethods { get; set; }
        public string ActionUri { get; set; }
        public HttpMethod DefaultMethod => ApiMethods?.FirstOrDefault();
        public IDictionary<string, object> ContentData { get; set; }
        public ICollection<MediaTypeHeaderValue> MediaTypes { get; set; }
        public ICollection<MediaTypeFormatter> Formatters { get; set; }

        public HttpRequestMessage GetRequestMessage()
        {
            var requestMessage = new HttpRequestMessage(DefaultMethod, ActionUri);
            
            #region Accept media types

            foreach (var mediaType in MediaTypes)
            {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType.MediaType));
            }

            #endregion

            #region Request content

            if (ContentData != null && ContentData.Any())
            {
                HttpContent requestContent;

                var contentObjectCount = ContentData.Count;

                if (contentObjectCount == 1)
                {
                    var contentObject = ContentData.First().Value;
                    var negotiation = ServiceBindingContext.Negotiate(requestMessage, Formatters, contentObject.GetType());

                    requestContent = new ObjectContent(contentObject.GetType(), contentObject, negotiation.Formatter, negotiation.MediaType);
                }
                else
                {
                    // Support multipart content
                    // TODO : Should be used MultipartContentAttribute

                    var multiContent = new MultipartFormDataContent("---- rceptor-multipart-content-splitter ----");

                    foreach (var routeData in ContentData)
                    {
                        var negotiation = ServiceBindingContext.Negotiate(requestMessage, Formatters, routeData.Value.GetType());

                        var content = new ObjectContent(routeData.Value.GetType(), routeData.Value,
                            negotiation.Formatter, negotiation.MediaType);

                        multiContent.Add(content, routeData.Key);
                    }

                    requestContent = multiContent;
                }

                requestMessage.Content = requestContent;
            }

            #endregion

            return requestMessage;
        }
    }
}