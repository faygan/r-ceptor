using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceClient;
using Service.PersonelService.Models;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class ChannelFactoryTests
    {
        private readonly Uri _endPointAddress = new Uri("http://localhost:49400");

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void channel_factory_operations_test()
        {
            var serviceFactory = new Rceptor.Core.ServiceProxy.ChannelFactory<IPersonService>
            {
                EndPoint = new EndpointAddress(_endPointAddress)
            };

            foreach (var operation in serviceFactory.ServiceOperations)
            {
                Console.WriteLine($"Method Name: {operation.MethodInfo.Name}");
                Console.WriteLine($"Route: {operation.Route}");
                Console.WriteLine("------------------------------------------");
            }

            Assert.IsTrue(serviceFactory.ServiceOperations.Any());
        }

        [TestMethod]
        public void get_dynamic_proxy_test()
        {
            var personService = new ChannelFactory<IPersonService>()
                .CreateChannel(new EndpointAddress(_endPointAddress));

            Assert.IsNotNull(personService);
        }

        [TestMethod]
        public void service_proxy_samples_method_invoke_test()
        {
            var personService =
                new Rceptor.Core.ServiceProxy.ChannelFactory<IPersonService>()
                .CreateChannel(new EndpointAddress(_endPointAddress));

            Assert.IsNotNull(personService);

            var person = personService.GetPerson(1).GetContentObject<PersonDto>();
            Assert.IsNotNull(person);
        }
    }
}