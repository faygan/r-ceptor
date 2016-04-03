using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Castle.Core.Internal;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{

    public class DefaultRouteProvider : IRouteProvider
    {

        #region Fields

        private readonly MethodInfo _actionMethod;

        #endregion

        #region Properties

        public string RouteTemplate { get; }

        public bool IsRouteProvided { get; }

        #endregion

        #region Constructors

        public DefaultRouteProvider(MethodInfo actionMethod)
        {
            if (actionMethod == null)
                throw new ArgumentNullException(nameof(actionMethod));

            _actionMethod = actionMethod;

            bool isRouteProvided;
            RouteTemplate = GetRouteTemplate(out isRouteProvided);
            IsRouteProvided = isRouteProvided;
        }

        #endregion

        #region IRouteProvider Members

        public ActionRouteCollection GetRoutes(ActionRouteGenerationOptions routeOptions)
        {
            var routes = new ActionRouteCollection();

            // Complete routes from method parameters. For request content data and not in route templates..
            var methodRoutes = new ActionRouteCollection(BuildRouteFromActionMethod(routeOptions.StringTypesDefaultRouteGenerationType));

            if (IsRouteProvided)
            {
                var metaRoutes = new List<ApiActionRoute>();

                var routesFromTemplate = BuildRouteFromRouteTemplate();

                var methodRoutesExceptTemplate = methodRoutes
                    .Where(p => routesFromTemplate.All(t => t.Name != p.Name))
                    .ToArray();

                methodRoutesExceptTemplate.ForEach(route =>
                {
                    if (!route.InBody && !route.IsComplexType)
                    {
                        route.AsUriAddress = true;
                    }
                });

                // Route meta data from route template description
                metaRoutes.AddRange(routesFromTemplate);

                // Route meta data from method parameters.
                metaRoutes.AddRange(methodRoutesExceptTemplate);

                routes.AddRange(metaRoutes);
            }
            else
            {
                methodRoutes.ForEach(route =>
                {
                    if (!route.InBody && !route.IsComplexType)
                    {
                        route.AsUriAddress = true;
                    }
                });

                routes.AddRange(methodRoutes);
            }

            return routes;
        }

        #endregion

        #region Routes Data Generation

        private IEnumerable<ApiActionRoute> BuildRouteFromActionMethod(ApiRouteAddressType stringTypesDefaultRouteGenerationType)
        {
            var routes = _actionMethod.GetParameters()
                .Select((parameter, i) =>
                {
                    var route = new ApiActionRoute
                    {
                        Order = parameter.Position,
                        IsVariable = true,
                        Name = parameter.Name,
                        IsOptional = parameter.IsOptional,
                        DefaultValue = parameter.DefaultValue,
                        ParameterType = parameter.ParameterType
                    };

                    if (route.ParameterType.IsValueType)
                    {
                        route.IsComplexType = false;
                    }
                    else
                    {
                        if (route.ParameterType == typeof(string))
                        {
                            route.IsComplexType = stringTypesDefaultRouteGenerationType == ApiRouteAddressType.RESTFull;
                        }
                        else
                        {
                            route.IsComplexType = true;
                        }
                    }

                    if (route.IsComplexType)
                    {
                        route.InBody = !parameter.HasAttribute<FromUriAttribute>();
                    }
                    else
                    {
                        route.InBody = parameter.HasAttribute<FromBodyAttribute>();
                    }

                    return route;
                });

            return routes;
        }

        private IEnumerable<ApiActionRoute> BuildRouteFromRouteTemplate()
        {
            if (string.IsNullOrEmpty(RouteTemplate)) yield break;

            var routeParts = RouteTemplate.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in routeParts)
            {
                yield return GetApiActionRoutePartInfo(part);
            }
        }

        public ApiActionRoute GetApiActionRoutePartInfo(string routePartTemplate)
        {
            var routePart = new ApiActionRoute
            {
                IsVariable = routePartTemplate.StartsWith("{") && routePartTemplate.EndsWith("}")
            };

            if (!routePart.IsVariable)
            {
                routePart.Name = routePartTemplate;
                return routePart;
            }

            var startPos = routePartTemplate.IndexOf("{", StringComparison.Ordinal);
            var endPos = routePartTemplate.LastIndexOf("}", StringComparison.Ordinal);

            try
            {
                var routeInfo = routePartTemplate.Substring(startPos + 1, endPos - 1);
                var routeDescription = routeInfo.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (routeDescription.Length > 0)
                {
                    routePart.Name = routeDescription[0];
                    routePart.Order = GetActionMethodParameterOrder(routePart.Name);
                }

                if (routeDescription.Length > 1)
                {
                    routePart.IsOptional = routeDescription[1].Contains("?");
                    routePart.TypeConstraint = routeDescription[1];

                    var defaultValueInfo = routeDescription[1].Split('=');

                    if (defaultValueInfo.Length > 1)
                    {
                        routePart.DefaultValue = defaultValueInfo[1];
                    }
                }

                if (routeDescription.Length > 2)
                {
                    routePart.RouteConstraint = routeDescription[2];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred in getting route informations. {ex.Message}");
            }

            return routePart;
        }

        private int GetActionMethodParameterOrder(string parameterName)
        {
            var parameterOrder = -1;

            var parameters = _actionMethod.GetParameters();
            var routePartParameter = parameters.FirstOrDefault(p => p.Name == parameterName);

            if (routePartParameter != null)
            {
                parameterOrder = routePartParameter.Position;
            }

            return parameterOrder;
        }

        private string GetRouteTemplate(out bool existsRouteAttr)
        {
            existsRouteAttr = false;

            string routeTemplate = null;

            var operationContractAttribute = (IHttpRouteInfoProvider)_actionMethod.GetCustomAttribute<OperationContractAttribute>();
            if (!string.IsNullOrEmpty(operationContractAttribute?.Template))
            {
                routeTemplate = operationContractAttribute.Template;
                existsRouteAttr = true;
            }

            if (!string.IsNullOrEmpty(routeTemplate))
                return routeTemplate;

            var routeAttributes = _actionMethod.GetCustomAttributes<RouteAttribute>();
            var firstRoute = routeAttributes?.OrderBy(r => r.Order).FirstOrDefault();

            if (firstRoute != null)
            {
                routeTemplate = firstRoute.Template;
                existsRouteAttr = true;
            }

            return routeTemplate;
        }

        #endregion

    }
}