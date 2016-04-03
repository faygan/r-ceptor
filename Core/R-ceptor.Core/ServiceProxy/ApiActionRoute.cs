using System;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{
    public class ApiActionRoute : IRoute
    {
        public object DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public string TypeConstraint { get; set; }
        public string RouteConstraint { get; set; }
        public Type ParameterType { get; set; }

        public bool InBody { get; set; }
        public string Name { get; set; }
        public bool IsVariable { get; set; }
        public bool IsComplexType { get; set; }

        /// <summary>
        /// Provided by parameter order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Action method parameter add to route address as uri parameter
        /// </summary>
        public bool AsUriAddress { get; set; }
    }
}