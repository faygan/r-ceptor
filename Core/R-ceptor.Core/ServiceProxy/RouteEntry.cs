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
        /// Action method parameter add to action uri as query parameter.
        /// </summary>
        public bool IsQuerySegment { get; set; }

        public object DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public string TypeConstraint { get; set; }
        public string RouteConstraint { get; set; }
        public Type ParameterType { get; set; }

        public bool IsComplexType
        {
            get
            {
                // If route entry is not a variable it is a static route part. And it is not a complex type.
                if (!IsVariable)
                    return false;

                if (ParameterType != null)
                    return !ParameterType.IsValueType
                        && ParameterType != typeof(string);

                return false;
            }
        }

        /// <summary>
        /// Route entry is provided by route tamplate.
        /// </summary>
        public bool InTemplate { get; set; }
    }
}