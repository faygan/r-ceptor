using System;
using System.Reflection;

namespace Rceptor.Core.Utils
{

    public static class TypeHelper
    {

        internal static bool IsEquivalent(this MethodInfo targetMethod, MethodInfo method)
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
