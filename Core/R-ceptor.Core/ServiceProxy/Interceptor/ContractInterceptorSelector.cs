using System;
using System.Reflection;
using Castle.DynamicProxy;
using Rceptor.Core.Utils;

namespace Rceptor.Core.ServiceProxy.Interceptor
{

    internal class ContractInterceptorSelector : IInterceptorSelector
    {

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            foreach (var interceptor in interceptors)
            {
                var methodInterceptor = interceptor as OperationContractInterceptor;
                if (methodInterceptor == null)
                    continue;
                var d = methodInterceptor.Operation.MethodInfo;
                if (d.IsEquivalent(method))
                    return new[] {interceptor};
            }
            throw new ArgumentException();
        }

    }
}