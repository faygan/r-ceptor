using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy.Provider;
using Rceptor.Core.Utils;

namespace Rceptor.Core.ServiceProxy
{
    public class OperationDescription : IRequestContextProvider
    {

        #region Properties

        public MethodInfo MethodInfo { get; set; }
        public ContractDescription ServiceContract { get; set; }
        public string Route => ActionBinding?.RouteTemplate;
        public string Name { get; set; }
        public ApiActionBinding ActionBinding { get; set; }

        #endregion

        #region IRequestContextProvider Members

        public string GetActionRoute(object[] callArguments)
        {
            return BuildActionRoute(MethodInfo, callArguments ?? new object[] { });
        }

        public string GetCompleteActionRoute(object[] callArguments)
        {
            return CompleteActionUri(MethodInfo, callArguments ?? new object[] { });
        }

        public RestRequestContext GetRequestContext(object[] callArguments, MethodInfo invokeMethod = null)
        {
            var arguments = callArguments ?? new object[] { };
            var bindingContext = ServiceContract.ServiceBindingContext;

            var context = new RestRequestContext
            {
                ApiMethods = ActionBinding.AcceptHttpMethods,
                ActionUri = CompleteActionUri(MethodInfo ?? invokeMethod, arguments),
                ContentData = ActionBinding.ActionParameters.Where(p => p.InBody)
                        .ToDictionary(r => r.Name, r => arguments[r.Order]),
                MediaTypes = bindingContext.SupportedMediaTypes,
                Formatters = bindingContext.Formatters
            };

            return context;
        }

        private string CompleteActionUri(MethodInfo method, IReadOnlyList<object> arguments)
        {
            var routePrefix = ServiceContract.RoutePrefix;
            var actionRoute = BuildActionRoute(method, arguments);

            var serviceUri = "";

            if (!string.IsNullOrEmpty(routePrefix) && !string.IsNullOrEmpty(actionRoute))
                serviceUri = $"{routePrefix}/{actionRoute}";
            else if (string.IsNullOrEmpty(routePrefix))
                serviceUri = $"{actionRoute}";
            else if (string.IsNullOrEmpty(actionRoute))
                serviceUri = $"{routePrefix}";

            return serviceUri;
        }

        private string BuildActionRoute(MethodInfo method, IReadOnlyList<object> arguments)
        {
            var targetMethod = method;

            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));

            var routeParametersData = (from p in targetMethod.GetParameters()
                                       select new
                                       {
                                           p.Name,
                                           p.Position,
                                           Value = arguments?[p.Position],   // TODO: check length..
                                           p.DefaultValue,
                                           p.HasDefaultValue
                                       }).ToArray();

            var rootRoutes = new Dictionary<string, string>();
            var complexTypeRoutes = new Dictionary<string, string>();
            var uriParameterRoutes = new Dictionary<string, string>();

            #region Loop action parameters

            foreach (var routePart in ActionBinding.ActionParameters)
            {
                var existsComplexTypeQuery = false;

                // Dont't use request content data as route parameters.
                if (routePart.InBody)
                    continue;

                string routePartValue = null;

                if (routePart.IsVariable)
                {
                    var routeData = routeParametersData.FirstOrDefault(f => f.Name == routePart.Name);

                    if (routeData == null)
                        continue;

                    if (routeData.Value != null)
                    {
                        if (!routePart.IsComplexType)
                        {
                            if (routePart.IsQuerySegment)
                            {
                                uriParameterRoutes.Add(routePart.Name, routeData.Value.ToString());
                            }
                            else
                            {
                                routePartValue = routeData.Value.ToString();
                            }
                        }
                        else
                        {
                            routePartValue = RoutingHelper.ConvertRouteData2UriAddress(routeData.Value);

                            existsComplexTypeQuery = true;
                        }
                    }
                }
                else
                {
                    routePartValue = routePart.Name;
                }

                if (routePartValue != null)
                {
                    if (!existsComplexTypeQuery)
                    {
                        rootRoutes.Add(routePart.Name, routePartValue);
                    }
                    else
                    {
                        complexTypeRoutes.Add(routePart.Name, routePartValue);
                    }
                }
            }

            #endregion 

            var actionRoute = "";

            if (rootRoutes.Any() || uriParameterRoutes.Any())
            {
                //if (ActionBinding.OperationAddressType == ApiRouteAddressType.RESTFull)
                if (1 == 1)
                {
                    actionRoute = rootRoutes.Values.AsEnumerable().Aggregate((p, pp) => $"{p}/{pp}");

                    string complexTypeUri = null;

                    if (complexTypeRoutes.Any())
                        complexTypeUri = complexTypeRoutes.Values.Aggregate((f, s) => $"{f}/{s}");

                    string uriParameterUri = null;

                    if (uriParameterRoutes.Any())
                        uriParameterUri = uriParameterRoutes
                            .Select(item => $"{item.Key}={item.Value}")
                            .Aggregate((f, s) => $"{f}&{s}");

                    if (!string.IsNullOrEmpty(complexTypeUri))
                    {
                        actionRoute = $"{actionRoute}/?{complexTypeUri}";
                    }
                    else if (!string.IsNullOrEmpty(uriParameterUri))
                    {
                        actionRoute = $"{actionRoute}/?{uriParameterUri}";
                    }
                }
                else
                {
                    if (rootRoutes.Any())
                    {
                        // If routes not provided, set action method name as route prefix or route
                        var methodAsRoutePrefix = ActionBinding.ActionRouteName;

                        actionRoute = rootRoutes
                            .Select(f => $"{f.Key}={f.Value}")
                            .Aggregate((f, s) => $"{f}&{s}");

                        actionRoute = !string.IsNullOrEmpty(actionRoute)
                                                    ? $"{methodAsRoutePrefix}/?{actionRoute}"
                                                    : $"{methodAsRoutePrefix}";
                    }
                    else
                    {
                        if (uriParameterRoutes.Any())
                        {
                            var uriParameterUri = uriParameterRoutes
                                .Select(item => $"{item.Key}={item.Value}")
                                .Aggregate((f, s) => $"{f}&{s}");

                            actionRoute = $"?{uriParameterUri}";
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(actionRoute))
                {
                    // If not provided routes, set action method name as default route 
                    actionRoute = ActionBinding.ActionRouteName;
                }
            }

            return actionRoute;
        }

        #endregion
    }
}