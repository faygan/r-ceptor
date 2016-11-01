using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Castle.Core.Internal;
using Rceptor.Core.ServiceProxy.Provider;
using Rceptor.Core.Utils;

namespace Rceptor.Core.ServiceProxy
{

    /// <summary>
    /// Default route meta provider.
    /// </summary>
    public class DefaultRouteProvider : IRouteInfoProvider
    {

        #region Fields

        private readonly MethodInfo _actionMethod;

        #endregion

        #region Properties

        private bool IsRouteProvided { get; }

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

        public string RouteTemplate { get; }

        public ActionRouteCollection GetRoutes()
        {
            var routes = new ActionRouteCollection();

            // Complete routes from method parameters. For request content data and not provided by route templates..
            var methodRouteEntries = GetMethodParametersAsRouteEntries();
            var methodRoutesCollection = new ActionRouteCollection(methodRouteEntries);

            if (IsRouteProvided)
            {
                var completeRouteEntries = new List<RouteEntry>();

                // Get route entries from route template.
                var routesFromTemplate = GetRouteEntriesFromTemplate();
                var templateRoutes = routesFromTemplate as RouteEntry[] ?? routesFromTemplate.ToArray();

                var methodRoutesExceptTemplate = methodRoutesCollection
                    .Except(templateRoutes, new RouteEntryComparer());

                // Route meta data from route template description.
                completeRouteEntries.AddRange(templateRoutes);

                // Route meta data from method parameters.
                completeRouteEntries.AddRange(methodRoutesExceptTemplate);

                SetOrderRouteEntries(completeRouteEntries.ToArray());

                routes.AddRange(completeRouteEntries);
            }
            else
            {
                routes.AddRange(methodRoutesCollection);
            }

            return new ActionRouteCollection(routes.OrderBy(r => r.Order));
        }

        #endregion

        #region Routes Data Generation

        private IEnumerable<RouteEntry> GetMethodParametersAsRouteEntries()
        {
            var routes = _actionMethod.GetParameters()
                .OrderBy(p => p.Position)
                .Select((parameter, i) =>
                {
                    var route = new RouteEntry
                    {
                        Name = parameter.Name,
                        IsVariable = true,
                        DefaultValue = parameter.DefaultValue,
                        Order = parameter.Position,
                        IsOptional = parameter.IsOptional,
                        ParameterType = parameter.ParameterType,
                        InTemplate = false,
                        IsQuerySegment = true,
                        IsComplexType = !parameter.ParameterType.IsValueType
                    };

                    if (parameter.HasAttribute<FromUriAttribute>())
                    {
                        route.InBody = false;
                    }
                    else
                    {
                        if (parameter.HasAttribute<FromBodyAttribute>())
                        {
                            route.InBody = true;
                            route.IsQuerySegment = false;
                        }
                    }

                    return route;
                });

            return routes;
        }

        public IEnumerable<RouteEntry> GetRouteEntriesFromTemplate()
        {
            if (string.IsNullOrEmpty(RouteTemplate))
                yield break;

            var routeParts = RouteTemplate.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < routeParts.Length; i++)
            {
                yield return Convert2RouteEntry(routeParts[i], i);
            }
        }

        private RouteEntry Convert2RouteEntry(string routePart, int routeIndex)
        {
            var isVariable = routePart.StartsWith("{") && routePart.EndsWith("}");

            if (!isVariable)
            {
                return new RouteEntry
                {
                    Name = routePart,
                    IsQuerySegment = false,
                    IsVariable = false,
                    Order = routeIndex,
                    InBody = false,
                    IsComplexType = false,
                    InTemplate = true
                };
            }

            var routeEntry = new RouteEntry
            {
                IsVariable = true,
                Order = routeIndex,
                IsQuerySegment = false,
                InBody = false,
                IsComplexType = false,
                InTemplate = true
            };

            var startPos = routePart.IndexOf("{", StringComparison.OrdinalIgnoreCase);
            var endPos = routePart.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);

            try
            {
                var routeEntryInfo = routePart.Substring(startPos + 1, endPos - 1);
                var routeDescription = routeEntryInfo.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (routeDescription.Length > 0)
                {
                    routeEntry.Name = routeDescription[0];
                }

                if (routeDescription.Length > 1)
                {
                    routeEntry.IsOptional = routeDescription[1].Contains("?");
                    routeEntry.TypeConstraint = routeDescription[1];

                    var defaultValueInfo = routeDescription[1].Split('=');

                    if (defaultValueInfo.Length > 1)
                    {
                        routeEntry.DefaultValue = defaultValueInfo[1];
                    }
                }

                if (routeDescription.Length > 2)
                {
                    routeEntry.RouteConstraint = routeDescription[2];
                }

                if (!string.IsNullOrEmpty(routeEntry.Name))
                {
                    var methodParameters = _actionMethod.GetParameters();
                    var parameter = methodParameters.FirstOrDefault(p => p.Name == routeEntry.Name);

                    if (parameter != null)
                    {
                        routeEntry.ParameterType = parameter.ParameterType;
                        routeEntry.IsComplexType = !routeEntry.ParameterType.IsValueType;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred in getting route informations. {ex.Message}");
            }

            return routeEntry;
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

        private void SetOrderRouteEntries(RouteEntry[] routeEntries)
        {
            var segmentedEntries = routeEntries.Where(f => f.IsQuerySegment).ToArray();

            if (!segmentedEntries.Any())
                return;

            var parameters = _actionMethod.GetParameters();
            var maxOrderOfEntries = routeEntries.Where(f => !f.IsQuerySegment).Max(p => p.Order);

            foreach (var entry in segmentedEntries)
            {
                var parameter = parameters.FirstOrDefault(p => p.Name == entry.Name);

                if (parameter == null)
                {
                    entry.Order = 999;
                    continue;
                }

                entry.Order = ++maxOrderOfEntries;
            }
        }

        #endregion

    }
}