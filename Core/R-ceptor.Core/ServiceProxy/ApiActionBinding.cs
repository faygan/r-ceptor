using System.Net.Http;
using System.Web.Http;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{

    /// <summary>
    /// Store action route template and detailed parameters of route information.
    /// </summary>
    public class ApiActionBinding : IBinding
    {
        public HttpMethod[] AcceptHttpMethods { get; set; }
        public bool IsNonAction { get; set; }

        /// <summary>
        /// Original route templates. Provide by <see cref="OperationContractAttribute"/> or <see cref="RouteAttribute"/>.
        /// </summary>
        public string RouteTemplate { get; set; }

        /// <summary>
        /// Action route meta informations. Detailed route parts.
        /// </summary>
        public ActionRouteCollection ActionParameters { get; set; }

        public ActionResponseTypeContext ResultTypeInfos { get; set; }
        public string ActionRouteName { get; set; }
    }

}