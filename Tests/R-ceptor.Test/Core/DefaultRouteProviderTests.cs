using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class DefaultRouteProviderTests
    {

        [TestMethod]
        public void authentication_service_authentication_method_route_build_tests()
        {
            // Route definiton : [Route("auth/scope/{scope:alpha}/{pass}")]
            // Method signature: Authentiation(string scope, string userName, string pass)

            var contractType = typeof(IAuthenticationService);
            var method = contractType.GetMethod("Authentication");

            var routeBuildOptions = new ActionRouteGenerationOptions
            {
                StringTypesDefaultRouteGenerationType = ApiRouteAddressType.RESTFull
            };

            var routeProvider = new DefaultRouteProvider(method);
            var routeParts = routeProvider.GetRoutes(routeBuildOptions);

            Console.WriteLine("Route template : {0}", routeProvider.RouteTemplate);
            Console.WriteLine("Method signature: Authentiation(string scope, string userName, string pass)");

            Console.WriteLine("");

            var apiActionRoutes = routeParts.ToArray();

            Console.WriteLine(setColumn(20, "--------------------", "-") + setColumn(20, "--------------------", "-") + setColumn(20, "--------------------", "-") + setColumn(20, "--------------------", "-") + setColumn(20, "--------------------", "-"));
            Console.WriteLine(setColumn(20, "Name") + setColumn(20, "IsVariable") + setColumn(20, "InTemplate") + setColumn(20, "Order") + setColumn(20, "IsQuerySegment"));
            Console.WriteLine(setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------"));

            foreach (var part in apiActionRoutes)
            {
                Console.WriteLine("{0}{1}{2}{3}{4}",
                    setColumn(20, part.Name), setColumn(20, part.IsVariable),
                    setColumn(20, part.InTemplate), setColumn(20, part.Order), setColumn(20, part.IsQuerySegment));

                Console.WriteLine(setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------") + setColumn(20, "--------------------"));
            }

            Assert.IsTrue(apiActionRoutes.Any());
        }

        string setColumn(int len, object val, string sfx = "|")
        {
            return val.ToString().PadRight(len, ' ') + sfx;
        }
    }
}
