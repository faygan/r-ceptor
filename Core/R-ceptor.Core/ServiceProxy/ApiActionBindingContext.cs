using System.Reflection;

namespace Rceptor.Core.ServiceProxy
{
    public class ApiActionBindingContext
    {
        public ApiActionBindingContext(MethodInfo actionMethod, ActionRouteGenerationOptions routeBuildOptions)
        {
            ActionMethod = actionMethod;
        }

        public MethodInfo ActionMethod { get; set; }
    }
}