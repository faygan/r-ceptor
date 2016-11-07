using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.ServiceClient;
using Rceptor.Core.ServiceProxy;
using Service.PersonelService.Models;
using Service.PersonelService.ServiceContract;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class PersonServiceLiveTests
    {
        private const string ServiceEndpointAddress = "http://localhost:51579";
        private IPersonService _personService;

        private readonly Func<ServiceBindingContext> _getBindingContext = () =>
            new ServiceBindingContext
            {
                ClientHandlers = new DelegatingHandler[]
                {
                    new ClientHandlerAuthSample(),
                    new ClientHandlerTokenSample()
                }
            };

        [TestInitialize]
        public void InitializeTest()
        {
            var channel = new Rceptor.Core.ServiceProxy.ChannelFactory<IPersonService>(
                new EndpointAddress(ServiceEndpointAddress), _getBindingContext());

            _personService = channel.CreateChannel();
        }


        [TestMethod]
        public void get_person_exists_test()
        {
            var person = _personService.GetPerson(13).GetContentObject<PersonDto>();


            Assert.IsNotNull(person);
            Assert.IsTrue(13 == person.PersonId);
            OutputPersons(new PersonDto[] { person });
        }

        [TestMethod]
        public void get_person_non_exists_test()
        {
            var response = _personService.GetPerson(130);
            var person = response.GetContentObject<PersonDto>();

            Assert.IsNull(person);
            Assert.IsTrue(response.HttpResponse.StatusCode == HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void get_person_with_dept_Id_test()
        {
            var person = _personService.GetPersonWithDeptId(10, 3).GetContentObject<PersonDto>();

            Assert.IsNotNull(person);
            OutputPersons(new[] { person });
        }

        [TestMethod]
        public void get_persons_by_department_test()
        {
            var response = _personService.GetPersonsByDepartment(4);
            var persons = response.GetContentObject<IEnumerable<PersonDto>>();

            Assert.IsNotNull(persons);
            OutputPersons(persons);
        }

        [TestMethod]
        public void get_persons_by_query_context__test()
        {
            var query = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 4,
                StartWithName = "Ma"
            };

            var response = _personService.GetPersonsByContext(query);
            var persons = response.GetContentObject<IEnumerable<PersonDto>>();

            Assert.IsTrue(response.HttpResponse.IsSuccessStatusCode);

            if (response.HttpResponse.StatusCode == HttpStatusCode.OK)
            {
                Assert.IsNotNull(persons);
                OutputPersons(persons);
            }
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

        [TestMethod]
        public void get_persons_payments_info_by_query_context__test()
        {
            var query = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 3,
                StartWithName = "L"
            };

            var response = _personService.GetPersonPaymentInfos(query);
            var payInfos = response.GetContentObject<IEnumerable<PersonPayInfoDto>>();

            Assert.IsTrue(response.HttpResponse.IsSuccessStatusCode);

            if (response.HttpResponse.StatusCode == HttpStatusCode.OK)
            {
                Assert.IsNotNull(payInfos);
                OutputPayInfos(payInfos);
            }
        }

        [TestMethod]
        public void set_persons_payments_multipart_content_test()
        {
            var personQuery = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 3,
                StartWithName = "L"
            };

            var paymentQuery = new PaymentQueryContext
            {
                PayTotal = 15000m,
                PaymentDate = new DateTime(2016, 2, 15)
            };

            var response = _personService.SetPersonPayments(personQuery, paymentQuery);

            Assert.IsTrue(response.HttpResponse.IsSuccessStatusCode);
        }

        [TestMethod]
        public void get_persons_by_name_test()
        {
            var response = _personService.GetPersonsByName("Ma");
            var persons = response.GetContentObject<IEnumerable<PersonDto>>();

            Assert.IsTrue(response.HttpResponse.IsSuccessStatusCode);
            Assert.IsNotNull(persons);

            OutputPersons(persons);
        }


        [TestMethod]
        public void get_person_with_dept_name_test()
        {
            var deptName = "Human Resources";

            var persons = _personService.GetPersonsDepartmansInfo(deptName)
                .GetContentObject<IEnumerable<PersonDto>>();

            Assert.IsNotNull(persons);
            Assert.IsTrue(persons.Any());
            OutputPersons(persons);
        }

        #region output

        private static void OutputPersons(IEnumerable<PersonDto> persons)
        {
            foreach (var person in persons)
            {
                Debug.WriteLine($"{person.PersonId}: {person.Name} - {person.DeptId} - {person.DeptName}");
            }
        }

        private static void OutputPayInfos(IEnumerable<PersonPayInfoDto> pays)
        {
            foreach (var pay in pays)
            {
                Debug.WriteLine($"{pay.Person.PersonId} - {pay.Person.Name} - {pay.Person.DeptName} - {pay.PayTotal} - {pay.PaymentDate}");
            }
        }

        #endregion
    }

    public class ClientHandlerAuthSample : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Auth-Handler", new[] { "userName", "password" });
            return base.SendAsync(request, cancellationToken);
        }
    }

    public class ClientHandlerTokenSample : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-Token-Handler", "tokenData");
            return base.SendAsync(request, cancellationToken);
        }
    }

}