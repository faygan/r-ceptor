using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Rceptor.Core.ServiceProxy
{

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class OperationContractAttribute : Attribute, IActionHttpMethodProvider, IHttpRouteInfoProvider
    {

        #region Members of IActionHttpMethodProvider

        public Collection<HttpMethod> HttpMethods
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpMethod))
                {
                    var methods = HttpMethod.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return new Collection<HttpMethod>(methods.Select(method => new HttpMethod(method)).ToList());
                }

                return new Collection<HttpMethod>();
            }
        }

        #endregion

        #region Constructors

        public OperationContractAttribute(string routeTemplate, string httpMethods, Type replyType)
            : this(routeTemplate, httpMethods)
        {
            ReplyType = replyType;
        }

        public OperationContractAttribute(Type replyType)
        {
            ReplyType = replyType;
        }

        public OperationContractAttribute(string routeTemplate, string httpMethods)
            : this("", routeTemplate, int.MinValue)
        {
            HttpMethod = httpMethods;
        }

        public OperationContractAttribute(string name, string routeTemplate, int order)
        {
            Name = name;
            Template = routeTemplate;
            Order = order;
        }

        #endregion

        #region Properties

        public bool NonAction { get; set; }
        public string HttpMethod { get; set; }
        public Type ReplyType { get; set; }

        #region Members of IHttpRouteInfoProvider

        public string Name { get; }
        public string Template { get; }
        public int Order { get; }

        #endregion

        #endregion
    }
}