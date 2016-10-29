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
                var uri = operation.GetRequestContext(null).ActionUri;
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

            var actionRouteResult = operation.GetActionRoute(null);
            var completeActionRouteResult = operation.GetCompleteActionRoute(null);

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

            var actionRouteResult = operation.GetActionRoute(actionParameters);
            var completeActionRouteResult = operation.GetCompleteActionRoute(actionParameters);

            Console.WriteLine("Action rest uri: " + actionRouteResult);
            Console.WriteLine("Action complete rest uri: " + completeActionRouteResult);

            Assert.IsTrue(requiredRouteResult == actionRouteResult, "Route result not matched!..");
            Assert.IsTrue(requiredCompleteRouteResult == completeActionRouteResult, "Route result not matched!..");


        }


    }

}
