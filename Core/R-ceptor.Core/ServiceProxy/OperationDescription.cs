using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{
    public class OperationDescription : IRequestContextProvider, IRouteAddressBuilder
    {

        #region Properties

        /// <summary>
        /// Base of operation method
        /// </summary>
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Service contract context
        /// </summary>
        public ContractDescription ServiceContract { get; set; }

        /// <summary>
        /// Original route template
        /// </summary>
        public string Route => ActionBinding?.RouteTemplate;

        /// <summary>
        /// Method name or action name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Operation method information, route template, route entries, http methods  etc..
        /// </summary>
        public ApiActionBinding ActionBinding { get; set; }

        #endregion

        #region IRouteAddressBuilder Members

        public IReadOnlyList<KeyValuePair<string, RouteDataInformation>> GetRouteDataInformation(params object[] callArguments)
        {
            var targetMethod = MethodInfo;

            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));

            var routeInfos = new List<KeyValuePair<string, RouteDataInformation>>();
            var arguments = callArguments ?? new object[] { };

            // Check method parameter count and method call argument
            var parameterCount = targetMethod.GetParameters().Length;

            if (parameterCount != arguments.Length)
                throw new ArgumentException("Invoked method has different arguments. Check named arguments of method.");

            var methodParametersInfo = (from p in targetMethod.GetParameters()
                                        select new
                                        {
                                            p.Name,
                                            p.Position,
                                            Value = arguments[p.Position],
                                            p.DefaultValue,
                                            p.HasDefaultValue
                                        }).ToArray();

            var routeEntries = ActionBinding.ActionParameters.OrderBy(r => r.Order);

            foreach (var routeEntry in routeEntries)
            {
                // Dont't use request content data as route parameters.
                if (routeEntry.InBody)
                    continue;

                var routeInfo = new RouteDataInformation(routeEntry);

                if (!routeEntry.IsVariable)
                {
                    routeInfo.SetRouteData(routeEntry.Name);
                }
                else
                {
                    var parameterInfo = methodParametersInfo.FirstOrDefault(f => f.Name == routeEntry.Name);

                    if (parameterInfo == null)
                        continue;

                    routeInfo.SetRouteData(parameterInfo.Value ?? parameterInfo.DefaultValue);

                    if (!routeInfo.HasValue)
                    {
                        // If not pass arguments for parameter, set argument from route entry default value.
                        if (routeEntry.InTemplate && routeEntry.DefaultValue != null)
                        {
                            routeInfo.SetRouteData(routeEntry.DefaultValue);
                        }
                    }
                }

                routeInfos.Add(new KeyValuePair<string, RouteDataInformation>(routeEntry.Name, routeInfo));
            }

            return routeInfos;
        }

        public string BuildActionRoute(params object[] callArguments)
        {
            return BuildActionRouteAddress(callArguments ?? new object[] { });
        }

        public string BuildActionRouteWithContract(params object[] callArguments)
        {
            return CompleteActionUri(callArguments ?? new object[] { });
        }

        #endregion

        #region IRequestContextProvider Members

        public RestRequestContext GetRequestContext(params object[] callArguments)
        {
            var bindingContext = ServiceContract.ServiceBindingContext;

            var context = new RestRequestContext
            {
                ApiMethods = ActionBinding.AcceptHttpMethods,
                ActionUri = CompleteActionUri(callArguments),
                ContentData = ActionBinding.ActionParameters.Where(p => p.InBody)
                        .ToDictionary(r => r.Name, r => callArguments[r.Order]),
                MediaTypes = bindingContext.SupportedMediaTypes,
                Formatters = bindingContext.Formatters
            };

            return context;
        }

        #endregion

        #region Helpers

        private string CompleteActionUri(object[] arguments)
        {
            var routePrefix = ServiceContract.RoutePrefix;
            var actionRoute = BuildActionRouteAddress(arguments);

            var serviceUri = "";

            if (!string.IsNullOrEmpty(routePrefix) && !string.IsNullOrEmpty(actionRoute))
                serviceUri = $"{routePrefix}/{actionRoute}";
            else if (string.IsNullOrEmpty(routePrefix))
                serviceUri = $"{actionRoute}";
            else if (string.IsNullOrEmpty(actionRoute))
                serviceUri = $"{routePrefix}";

            return serviceUri;
        }

        private string BuildActionRouteAddress(object[] arguments)
        {
            var routeInfos = GetRouteDataInformation(arguments);

            if (!routeInfos.Any())
            {
                // If not provided routes, set action method name as default route.
                return ActionBinding.ActionRouteName;
            }

            var inRouteAddressList = new List<string>();
            var querySegmentAddressList = new List<RouteDataInformation>();

            foreach (var info in routeInfos.OrderBy(o => o.Value.RouteEntry.Order))
            {
                if (!info.Value.HasValue)
                    continue;

                var routeInfo = info.Value;

                if (!routeInfo.IsQuerySegment)
                {
                    inRouteAddressList.Add(routeInfo.RouteData.ToString());
                }
                else
                {
                    querySegmentAddressList.Add(routeInfo);
                }
            }

            var routedAddress = "";
            var queryAddress = "";

            if (inRouteAddressList.Any())
                routedAddress = inRouteAddressList.Aggregate((p, pp) => $"{p}/{pp}");

            if (querySegmentAddressList.Any())
                queryAddress = querySegmentAddressList
                    .Select(item =>
                        item.IsComplexType
                            // If route data is complex type, complete addrfess from only serialized route data.
                            ? $"{item.RouteData}"
                            // If route data is not complex type, set query address from name and value.
                            : $"{item.Name}={item.RouteData}")
                    .Aggregate((f, s) => $"{f}&{s}");

            string completeActionRouteAddress = null;

            // Merge routed and query segments address.
            if (!string.IsNullOrEmpty(routedAddress))
            {
                if (!string.IsNullOrEmpty(queryAddress))
                {
                    completeActionRouteAddress = $"{routedAddress}?{queryAddress}";
                }
                else
                {
                    completeActionRouteAddress = $"{routedAddress}";
                }
            }
            else if (!string.IsNullOrEmpty(queryAddress))
            {
                completeActionRouteAddress = $"?{queryAddress}";
            }

            return completeActionRouteAddress;
        }

        #endregion

    }

}

