using System;
using System.Web.Http;

namespace Rceptor.Core.ServiceProxy
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ServiceContractAttribute : RoutePrefixAttribute
    {
        public ServiceContractAttribute(string prefix) :
            this(prefix, string.Empty)
        {
        }

        public ServiceContractAttribute(string prefix, string serviceName) :
            base(prefix)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}