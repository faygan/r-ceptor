using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class OperationDescriptionTests
    {
        [TestMethod]
        public void get_action_route_template_test()
        {
            var contractType = typeof(IPersonService);
            var method = contractType.GetMethod("SetPersonDebit");

            var routeProvider = new DefaultRouteProvider(method);
            var template = routeProvider.RouteTemplate;
            Console.WriteLine("Route template is : {0}", template);

            Assert.IsTrue(!string.IsNullOrEmpty(template));
        }

        [TestMethod]
        public void get_action_route_parameters_test()
        {
            // [Route("{personId:int}/debit/{debit:decimal}")]

            var contractType = typeof(IPersonService);
            var method = contractType.GetMethod("GetPerson");

            var routeBuildOptions = new ActionRouteGenerationOptions
            {
                StringTypesDefaultRouteGenerationType = ApiRouteAddressType.RESTFull
            };

            var routeProvider = new DefaultRouteProvider(method);
            var template = routeProvider.RouteTemplate;
            var routeParts = routeProvider.GetRoutes(routeBuildOptions);

            Console.WriteLine("Route template : {0}", template);

            var apiActionRoutes = routeParts.ToArray();
            foreach (var part in apiActionRoutes)
            {
                Console.WriteLine(
                    "Action route part -> name: {0}  , isvariable: {1}   , typeConstraint: {2}  order: {3}",
                    part.Name, part.IsVariable, part.TypeConstraint, part.Order);
            }

            Assert.IsTrue(apiActionRoutes.Any());
        }

        [TestMethod]
        public void get_action_route_parameters_for_get_persons_departmans_info_test()
        {
            // [Route("{personId:int}/debit/{debit:decimal}")]

            var contractType = typeof(IPersonService);
            var method = contractType.GetMethod("GetPersonsDepartmansInfo");


            var routeProvider = new DefaultRouteProvider(method);
            var template = routeProvider.RouteTemplate;
            var routeBuildOptions = new ActionRouteGenerationOptions { StringTypesDefaultRouteGenerationType = ApiRouteAddressType.RESTFull };

            var routeParts = routeProvider.GetRoutes(routeBuildOptions);

            Console.WriteLine("Route template : {0}", template);

            var apiActionRoutes = routeParts.ToArray();
            foreach (var part in apiActionRoutes)
            {
                Console.WriteLine(
                    "Action route part -> name: {0}  , isvariable: {1}   , typeConstraint: {2}  order: {3}",
                    part.Name, part.IsVariable, part.TypeConstraint, part.Order);
            }

            Assert.IsTrue(apiActionRoutes.Any());
        }

        [TestMethod]
        public void get_action_route_parameters_for_get_active_persons_by_query_context__test()
        {
            // [Route("{personId:int}/debit/{debit:decimal}")]

            var contractType = typeof(IPersonServiceRouteLess);
            var method = contractType.GetMethod("GetActivePersonsByContext");

            var routeBuildOptions = new ActionRouteGenerationOptions
            {
                StringTypesDefaultRouteGenerationType = ApiRouteAddressType.RESTFull
            };

            var routeProvider = new DefaultRouteProvider(method);
            var template = routeProvider.RouteTemplate;
            var routeParts = routeProvider.GetRoutes(routeBuildOptions);

            Console.WriteLine("Route template : {0}", template);

            var apiActionRoutes = routeParts.ToArray();
            foreach (var part in apiActionRoutes)
            {
                Console.WriteLine(
                    "Action route part -> name: {0}  , isvariable: {1}   , typeConstraint: {2}  order: {3}",
                    part.Name, part.IsVariable, part.TypeConstraint, part.Order);
            }

            var asUriRoute = apiActionRoutes.FirstOrDefault(f => f.Name == "onlyActive");

            Assert.IsTrue(asUriRoute != null);
            Assert.IsTrue(asUriRoute.AsUriAddress);
        }
    }
}