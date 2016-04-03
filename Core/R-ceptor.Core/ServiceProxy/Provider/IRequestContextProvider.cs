using System.Reflection;
using Rceptor.Core.ServiceClient;

namespace Rceptor.Core.ServiceProxy.Provider
{
    public interface IRequestContextProvider
    {
        RestRequestContext GetRequestContext(object[] arguments, MethodInfo invokeMethod = null);
    }
}