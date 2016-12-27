using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.Models;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class PersonServiceRouteLessLiveTests
    {
        private const string ServiceEndpointAddress = "http://localhost:51579";
        private IPersonServiceRouteLess _personService;

        private readonly Func<ServiceBindingContext> _getBindingContext = () =>
            new ServiceBindingContext
            {
                MessageHandlerProvider = () =>
                {
                    return new DelegatingHandler[]
                    {
                        new ClientHandlerAuthSample(),
                    new ClientHandlerTokenSample()
                    };
                }

            };

        [TestInitialize]
        public void InitializeTest()
        {
            var channel = new Rceptor.Core.
                ServiceProxy.ChannelFactory<IPersonServiceRouteLess>(new EndpointAddress(ServiceEndpointAddress),
                _getBindingContext());

            _personService = channel.CreateChannel();

            Assert.IsNotNull(_personService);
        }



        [TestMethod]
        public void get_person_with_dept_Id_test()
        {
            var person = _personService.GetPersonWithDeptId(10, 3)
                 .GetContentObject<PersonDto>();

            Assert.IsNotNull(person);
            OutputPersons(new[] { person });
        }

        [TestMethod]
        public void get_active_persons_by_query_context__test()
        {
            var query = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 3,
                StartWithName = "L"
            };

            var response = _personService.GetActivePersonsByContext(true, query);
            var persons = response.GetContentObject<IEnumerable<PersonDto>>();

            Assert.IsTrue(response.HttpResponse.IsSuccessStatusCode);

            if (response.HttpResponse.StatusCode == HttpStatusCode.OK)
            {
                Assert.IsNotNull(persons);
                OutputPersons(persons);
            }
        }

        #region output

        private static void OutputPersons(IEnumerable<PersonDto> persons)
        {
            foreach (var person in persons)
            {
                Debug.WriteLine($"{person.Name} - {person.DeptId} - {person.PersonId}");
            }
        }

        private static void OutputPayInfos(IEnumerable<PersonPayInfoDto> pays)
        {
            foreach (var pay in pays)
            {
                Debug.WriteLine($"{pay.Person.PersonId} - {pay.Person.Name} - {pay.Person.DeptId} - {pay.PayTotal} - {pay.PaymentDate}");
            }
        }

        #endregion
    }

}