using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rceptor.Test.Core
{
    [TestClass]
    public class LambdaDelegationTest
    {
        [TestMethod]
        public void generate_proxy_with_lambda_expression()
        {
            //ProxyHelper.BuildProxy<IPersonService, string>(service => service.GetPersonName(100));
        }
    }
}