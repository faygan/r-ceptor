using System.Collections.Generic;
using System.Reflection;

namespace Rceptor.Core.ServiceProxy.Provider
{

    public interface IRouteAddressBuilder
    {
        IReadOnlyList<KeyValuePair<string, RouteDataInformation>>  GetRouteDataInformation(object[] callArguments);
        string BuildActionRoute(object[] callArguments);
        string BuildActionRouteWithContract(object[] callArguments);
    }

}