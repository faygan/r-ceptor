using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{

    public class DefaultServiceBindingContextProvider : IServiceBindingContextProvider
    {

        public ServiceBindingContext GetContext(ServiceBindingContext currentContext)
        {
            var context = new ServiceBindingContext();

            if (currentContext?.ClientHandlers != null)
            {
                context.ClientHandlers = currentContext.ClientHandlers;
            }
            else
            {
                context.ClientHandlers = new DelegatingHandler[] { };
            }

            if (currentContext?.SupportedMediaTypes != null)
            {
                context.SupportedMediaTypes = currentContext.SupportedMediaTypes;
            }
            else
            {
                context.SupportedMediaTypes = new[] {
                    new MediaTypeWithQualityHeaderValue("application/json"),
                    new MediaTypeWithQualityHeaderValue("application/xml")
                };
            }

            if (currentContext?.Formatters != null)
            {
                context.Formatters = currentContext.Formatters;
            }
            else
            {
                context.Formatters = new MediaTypeFormatterCollection();
            }

            if (currentContext?.RouteBuildOptions != null)
            {
                context.RouteBuildOptions = currentContext.RouteBuildOptions;
            }
            else
            {
                context.RouteBuildOptions = new ActionRouteGenerationOptions
                {
                    StringTypesDefaultRouteGenerationType = ApiRouteAddressType.RESTFull
                };
            }

            return context;
        }

    }
}
