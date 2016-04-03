using System.Linq;
using System.Net.Http;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{

    public class ApiActionBinding : IBinding
    {
        public HttpMethod[] AcceptHttpMethods { get; set; }
        public bool IsNonAction { get; set; }
        public string RouteTemplate { get; set; }
        public ActionRouteCollection ActionParameters { get; set; }
        public ActionResponseTypeContext ResultTypeInfos { get; set; }
        public string ActionRouteName { get; set; }

        public ApiRouteAddressType OperationAddressType
        {
            get
            {
                var addressType = ApiRouteAddressType.UriParameter;

                if (ActionParameters != null)
                {
                    if (ActionParameters.Any(r => r.IsVariable) && !string.IsNullOrEmpty(RouteTemplate))
                        addressType = ApiRouteAddressType.RESTFull;

                }

                return addressType;
            }
        }

    }

}