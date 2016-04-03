using System.Linq;

namespace Rceptor.Core.Utils
{

    public static class RoutingHelper
    {

        public static string ConvertRouteData2UriAddress(object routeObject)
        {
            if (routeObject == null)
                return null;

            var routeObjectType = routeObject.GetType();

            if (routeObjectType.IsValueType)
                return null;

            var properties = routeObjectType.GetProperties();

            var uriData = (from property in properties
                           let name = property.Name
                           let value = property.GetValue(routeObject, null)
                           where value != null
                           select $"{name}={value}")
                .ToList();

            return uriData.Aggregate((f, s) => $"{f}&{s}");
        }

    }

}
