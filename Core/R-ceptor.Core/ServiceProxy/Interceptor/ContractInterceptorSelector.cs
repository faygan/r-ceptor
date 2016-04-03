using System;
using System.Reflection;
using Castle.DynamicProxy;

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
                if (IsEquivalent(d, method))
                    return new[] {interceptor};
            }
            throw new ArgumentException();
        }

        private static bool IsEquivalent(MethodInfo targetMethod, MethodInfo method)
        {
            if (!targetMethod.Name.Equals(method.Name, StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (!method.ReturnType.IsAssignableFrom(targetMethod.ReturnType))
                return false;
            var parameters = method.GetParameters();
            var dp = targetMethod.GetParameters();
            if (parameters.Length != dp.Length)
                return false;
            for (var i = 0; i < parameters.Length; i++)
            {
                //BUG: does not take into account modifiers (like out, ref...)
                if (!parameters[i].ParameterType.IsAssignableFrom(dp[i].ParameterType))
                    return false;
            }
            return true;
        }
    }
}