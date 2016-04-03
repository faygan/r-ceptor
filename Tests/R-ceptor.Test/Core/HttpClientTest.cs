using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.PersonelService.Models;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class HttpClientTest
    {
        private const string ServiceEndpointAddress = "http://localhost:49400";

        [TestMethod]
        public void http_client_route_test()
        {
            var query = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 2,
                StartWithName = "An"
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServiceEndpointAddress);

                var defaultFormatter = new JsonMediaTypeFormatter();

                var requestContent = new ObjectContent(typeof (PersonQueryContext),
                    query, defaultFormatter, new MediaTypeHeaderValue("application/json"));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                    "/api/person/GetActivePersonsByContext/?onlyActive=True")
                {
                    Content = requestContent
                };

                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 0.5));
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.3));

                var responseMessage = client.SendAsync(requestMessage).Result;

                Assert.IsTrue(responseMessage.IsSuccessStatusCode);
            }
        }
    }
}