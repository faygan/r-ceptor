using System;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{
    public class RouteEntry : IRoute
    {
        public string Name { get; set; }
        public bool IsVariable { get; set; }
        public bool InBody { get; set; }

        /// <summary>
        /// Provided by method parameter order or route entry index.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Action method parameter add to action uri as query parameter
        /// </summary>
        public bool IsQuerySegment { get; set; }

        public object DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public string TypeConstraint { get; set; }
        public string RouteConstraint { get; set; }
        public Type ParameterType { get; set; }
        public bool IsComplexType { get; set; }

        /// <summary>
        /// Route entry is provided by route tamplate.
        /// </summary>
        public bool InTemplate { get; set; }
    }
}