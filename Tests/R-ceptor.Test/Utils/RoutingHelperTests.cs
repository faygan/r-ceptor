using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rceptor.Core.Utils;
using Service.PersonelService.Models;

namespace Rceptor.Test.Utils
{
    [TestClass()]
    public class RoutingHelperTests
    {

        [TestMethod]
        public void get_object_uri_address_test()
        {
            var queryObject = new PersonQueryContext
            {
                PersonId = null,
                DeptId = 3,
                StartWithName = "L"
            };

            var objectUri = RoutingHelper.ConvertRouteData2UriAddress(queryObject);

            Assert.IsNotNull(objectUri);
        }

    }
}