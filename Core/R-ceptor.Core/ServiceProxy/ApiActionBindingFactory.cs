using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{

    public class ApiActionBindingFactory : IBindingFactory<ApiActionBindingContext>
    {
        public T GetBinding<T>(ApiActionBindingContext context) where T : IBinding
        {
            if (context?.ActionMethod == null)
                throw new ArgumentNullException(nameof(context));

            var binding = new ApiActionBinding();

            var operationProvider = context.ActionMethod.GetCustomAttribute<OperationContractAttribute>();

            binding.IsNonAction = operationProvider?.NonAction ??
                                  context.ActionMethod.GetCustomAttribute<NonActionAttribute>(false) != null;

            if (binding.IsNonAction)
                return (T)(object)binding;

            binding.AcceptHttpMethods = GetActionHttpMethods(context.ActionMethod);
            binding.ResultTypeInfos = new ActionResponseTypeContext(context.ActionMethod);

            var actionName = context.ActionMethod.GetCustomAttribute<ActionNameAttribute>(true);

            binding.ActionRouteName = !string.IsNullOrEmpty(actionName?.Name)
                ? actionName.Name
                : context.ActionMethod.Name.ToLowerInvariant();

            var routeProvider = new DefaultRouteProvider(context.ActionMethod);
            binding.RouteTemplate = routeProvider.RouteTemplate;
            binding.ActionParameters = routeProvider.GetRoutes(context.RouteBuildOptions);

            return (T)(object)binding;
        }

        private static HttpMethod[] GetActionHttpMethods(MemberInfo method)
        {
            var httpMethods = new HttpMethod[] { };

            var methodProvider = (IActionHttpMethodProvider)method.GetCustomAttribute<OperationContractAttribute>();

            if (methodProvider == null || !methodProvider.HttpMethods.Any())
            {
                methodProvider = method.GetCustomAttribute<AcceptVerbsAttribute>();
            }

            if (methodProvider == null || !methodProvider.HttpMethods.Any())
            {
                methodProvider = method.GetCustomAttribute<HttpDeleteAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpGetAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpHeadAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpOptionsAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpPatchAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpPostAttribute>();
            }

            if (methodProvider == null)
            {
                methodProvider = method.GetCustomAttribute<HttpPutAttribute>();
            }


            if (methodProvider != null)
            {
                httpMethods = methodProvider.HttpMethods.ToArray();
            }
            else
            {
                if (method.Name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
                {
                    httpMethods = new[] { HttpMethod.Get };
                }
                else if (
                    method.Name.StartsWith("post", StringComparison.InvariantCultureIgnoreCase) ||
                    method.Name.StartsWith("save", StringComparison.InvariantCultureIgnoreCase))
                {
                    httpMethods = new[] { HttpMethod.Post };
                }
                else if (
                    method.Name.StartsWith("put", StringComparison.InvariantCultureIgnoreCase) ||
                    method.Name.StartsWith("update", StringComparison.InvariantCultureIgnoreCase) ||
                    method.Name.StartsWith("set", StringComparison.InvariantCultureIgnoreCase))
                {
                    httpMethods = new[] { HttpMethod.Put };
                }
                else if (method.Name.StartsWith("delete", StringComparison.InvariantCultureIgnoreCase))
                {
                    httpMethods = new[] { HttpMethod.Delete };
                }
            }

            if (!httpMethods.Any())
                httpMethods = new[] { HttpMethod.Get };

            return httpMethods;
        }
    }

}