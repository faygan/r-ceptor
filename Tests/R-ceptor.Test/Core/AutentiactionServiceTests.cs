using System;
using System.Net.Http;
using System.ServiceModel;
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
        private IAuthenticationService _authService;

        private readonly Func<ServiceBindingContext> _getBindingContext = () =>
            new ServiceBindingContext();


        [TestInitialize]
        public void InitializeTest()
        {
            var channel = new Rceptor.Core.ServiceProxy
                .ChannelFactory<IAuthenticationService>(ServiceEndpointAddress,
                _getBindingContext());

            _authService = channel.CreateChannel();
        }


        [TestMethod]
        public void VerifyUserTokenTest()
        {
            var response = _authService.VerifyUserAuth();

            Assert.IsTrue(response != null && response.IsSuccess());

            var info = response.GetContentObject<VerifyUserTokenModel>();

            Console.WriteLine("Username:" + info.UserName + " Token:" + info.Token);
        }

    }

}
