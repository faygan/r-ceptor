using System.Reflection;

namespace Rceptor.Core.ServiceProxy.Provider
{

    public interface IServiceOperationContainer
    {
        OperationDescription GetOperationDescription(string operationMethodName, string routeTemplate = null);
        OperationDescription GetOperationDescription(MethodInfo method);
    }

}
