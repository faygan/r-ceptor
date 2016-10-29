using System.Reflection;
using Rceptor.Core.ServiceClient;

namespace Rceptor.Core.ServiceProxy.Provider
{
    public interface IRequestContextProvider
    {
        RestRequestContext GetRequestContext(object[] callArguments, MethodInfo invokeMethod = null);
        string GetActionRoute(object[] callArguments);
        string GetCompleteActionRoute(object[] callArguments);
    }
}