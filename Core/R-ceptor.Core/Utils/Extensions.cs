using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rceptor.Core.Utils
{

    internal static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static bool HasAttribute<T>(this ParameterInfo parameter)
            where T : Attribute
        {
            return parameter.GetCustomAttribute<T>(false) != null;
        }
    }
}
