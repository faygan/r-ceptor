using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.Models;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{

    [TestClass]
    public class AutentiactionServiceTests
    {

        private const string ServiceEndpointAddress = "http://localhost:51579";
        private ChannelFactory<IAuthenticationService> _channel;
        private IAuthenticationService _authService;
        private readonly Func<ServiceBindingContext> _getBindingContext = () => new ServiceBindingContext();

        [TestInitialize]
        public void InitializeTest()
        {
            _channel = new ChannelFactory<IAuthenticationService>(ServiceEndpointAddress,
                _getBindingContext());

            _authService = _channel.CreateChannel();

        }

        [TestMethod]
        public void VerifyUserTokenTest()
        {
            var response = _authService.VerifyUserAuth();

            Assert.IsTrue(response != null && response.IsSuccess());

            var info = response.GetContentObject<VerifyUserTokenModel>();

            Console.WriteLine("Username:" + info.UserName + " Token:" + info.Token);
        }


        [TestMethod]
        public void contract_operaitons_test()
        {
            Assert.IsTrue(_channel.ServiceOperations.Any());

            foreach (var operation in _channel.ServiceOperations)
            {
                var uri = operation.GetRequestContext().ActionUri;
                Console.WriteLine("Operation:" + operation.Name + "    -> " + "Uri:" + uri);
            }
        }

        [TestMethod]
        public void verify_user_token_route_test()
        {
            // [Route("verifyAuth")]

            const string requiredRouteResult = "verifyAuth";
            const string requiredCompleteRouteResult = "api/account/verifyAuth";

            var operation = _channel.GetOperationDescription(_channel.ContractType.GetMethod("VerifyUserAuth"));

            Assert.IsNotNull(operation, "Found operation :)");

            Console.WriteLine("Route template : {0}", operation.Route);

            var actionRouteResult = operation.BuildActionRoute(null);
            var completeActionRouteResult = operation.BuildActionRouteWithContract(null);

            Console.WriteLine("Action rest uri: " + actionRouteResult);
            Console.WriteLine("Action complete rest uri: " + completeActionRouteResult);

            Assert.IsTrue(requiredRouteResult == actionRouteResult, "Route result not matched!..");
            Assert.IsTrue(requiredCompleteRouteResult == completeActionRouteResult, "Route result not matched!..");


        }


        [TestMethod]
        public void authentication_user_route_test()
        {
            // [Route("verifyAuth")]

            const string requiredRouteResult = "auth/sampleScope?userName=admin&pass=1234";
            const string requiredCompleteRouteResult = "api/account/auth/sampleScope?userName=admin&pass=1234";

            var operation = _channel.GetOperationDescription(_channel.ContractType.GetMethod("Authentiation"));

            Assert.IsNotNull(operation, "Found operation :)");

            Console.WriteLine("Route template : {0}", operation.Route);

            object[] actionParameters = {
                "sampleScope",
                "admin",
                "1234"
            };

            var actionRouteResult = operation.BuildActionRoute(actionParameters);
            var completeActionRouteResult = operation.BuildActionRouteWithContract(actionParameters);

            Console.WriteLine("Action rest uri: " + actionRouteResult);
            Console.WriteLine("Action complete rest uri: " + completeActionRouteResult);

            Assert.IsTrue(requiredRouteResult == actionRouteResult, "Route result not matched!..");
            Assert.IsTrue(requiredCompleteRouteResult == completeActionRouteResult, "Route result not matched!..");


        }

        [TestMethod]
        public void authentcation_user_with_context_test()
        {
            var authContext = new AuthContext
            {
                UserName = "John",
                Pass = "Doe"
            };

            var response = _authService.Authentication(authContext, "AuthScope");

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.IsSuccess());


            // Todo : how to check service response content..
        }


        [TestMethod]
        public void authentication_route_data_information_test()
        {
            var operation = _channel.GetOperationDescription("Authentication", "auth/scope/{scope:alpha}/{pass}");

            var routeDataInfos = operation.GetRouteDataInformation("authScope", "johnDoe", "1234!");

            Console.WriteLine("Route: auth/scope/{scope:alpha}/{pass}");
            Console.WriteLine("Method: Authentication(string scope, string userName, string pass)");
            Console.WriteLine("Call Args: authScope, johnDoe, 1234!");

            Console.WriteLine("-----------------------------------------");

            Assert.IsTrue(routeDataInfos.Any());
            Assert.IsTrue(routeDataInfos.Count == 5);

            Console.WriteLine("key:routeData:isquerysegment:isvariable:order");

            foreach (var info in routeDataInfos)
            {
                Console.WriteLine(info.Key + ":" + info.Value.RouteData + ":" + info.Value.IsQuerySegment
                    + ":" + info.Value.RouteEntry.IsVariable + ":" + info.Value.RouteEntry.Order);
            }

        }


        [TestMethod]
        public void authentication_route_data_information_complex_type_test()
        {
            var operation = _channel.GetOperationDescription("Authentication", "auth");

            var authContext = new AuthContext
            {
                UserName = "John",
                Pass = "Doe"
            };


            var routeDataInfos = operation.GetRouteDataInformation(authContext, "AUTHSCOPE");

            Console.WriteLine("Route: auth");
            Console.WriteLine("Method: Authentication([FromUri] AuthContext context, string scope)");
            Console.WriteLine("Call Args: {class:authContext}, AUTHSCOPE");

            Console.WriteLine("-----------------------------------------");

            Assert.IsTrue(routeDataInfos.Any());
            Assert.IsTrue(routeDataInfos.Count == 3);

            Console.WriteLine("key:routeData:isquerysegment:isvariable:order");

            foreach (var info in routeDataInfos)
            {
                Console.WriteLine(info.Key + ":" + info.Value.RouteData + ":" + info.Value.IsQuerySegment
                    + ":" + info.Value.RouteEntry.IsVariable + ":" + info.Value.RouteEntry.Order);
            }


        }


        [TestMethod]
        public void verify_authentication_route_data_information_test()
        {
            var operation = _channel.GetOperationDescription("VerifyAuthentication");

            var authContext = new AuthContext
            {
                UserName = "John",
                Pass = "Doe"
            };


            var routeDataInfos = operation.GetRouteDataInformation(authContext);

            Console.WriteLine("Route: N/A");
            Console.WriteLine("Method: VerifyAuthentication([FromUri] AuthContext context)");
            Console.WriteLine("Call Args: {class:authContext}");

            Console.WriteLine("-----------------------------------------");

            Assert.IsTrue(routeDataInfos.Any());
            Assert.IsTrue(routeDataInfos.Count == 1);

            Console.WriteLine("key:routeData:isquerysegment:isvariable:order");

            foreach (var info in routeDataInfos)
            {
                Console.WriteLine(info.Key + ":" + info.Value.RouteData + ":" + info.Value.IsQuerySegment
                    + ":" + info.Value.RouteEntry.IsVariable + ":" + info.Value.RouteEntry.Order);
            }


        }

        [TestMethod]
        public void authentication_verify_user_auth_action_route_test()
        {
            const string expectedResult = "verifyAuth";

            var operation = _channel.GetOperationDescription("VerifyUserAuth");

            Console.WriteLine("Route: verifyAuth");
            Console.WriteLine("Method: VerifyUserAuth()");
            Console.WriteLine("Call Args: N/A");

            Console.WriteLine("-----------");

            var actionRouteAddress = operation.BuildActionRoute();

            Console.WriteLine("Expected route address: {0}", expectedResult);
            Console.WriteLine("Result route address: {0}", actionRouteAddress);

            Assert.IsTrue(actionRouteAddress == expectedResult);

        }

        [TestMethod]
        public void authentication_verify_authentication_route_test()
        {
            var authContext = new AuthContext
            {
                UserName = "John",
                Pass = "Doe"
            };

            const string expectedResult = "verify?userName=John&pass=Doe";

            var operation = _channel.GetOperationDescription("VerifyAuthentication");

            var actionRouteAddress = operation.BuildActionRoute(authContext);

            Console.WriteLine("Expected route address: {0}", expectedResult);
            Console.WriteLine("Result route address: {0}", actionRouteAddress);

            Assert.IsTrue(string.Equals(actionRouteAddress, 
                expectedResult, StringComparison.CurrentCultureIgnoreCase));

        }

        [TestMethod]
        public void authentication_verify_authentication_complete_route_test()
        {
            var authContext = new AuthContext
            {
                UserName = "John",
                Pass = "Doe"
            };

            const string expectedResult = "api/account/verify?userName=John&pass=Doe";

            var operation = _channel.GetOperationDescription("VerifyAuthentication");

            var actionRouteAddress = operation.BuildActionRouteWithContract(authContext);

            Console.WriteLine("Expected route address: {0}", expectedResult);
            Console.WriteLine("Result route address: {0}", actionRouteAddress);

            Assert.IsTrue(string.Equals(actionRouteAddress,
                expectedResult, StringComparison.CurrentCultureIgnoreCase));

        }

    }

}
