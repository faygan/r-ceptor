using System.Reflection;

namespace Rceptor.Core.ServiceProxy
{
    public class ApiActionBindingContext
    {
        public ApiActionBindingContext(MethodInfo actionMethod, ActionRouteGenerationOptions routeBuildOptions)
        {
            ActionMethod = actionMethod;
            RouteBuildOptions = routeBuildOptions;
        }

        public MethodInfo ActionMethod { get; set; }
        public ActionRouteGenerationOptions RouteBuildOptions { get; set; }
    }
}