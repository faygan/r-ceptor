
namespace Rceptor.Core.ServiceProxy.Provider
{
    public interface IRouteProvider
    {
        string RouteTemplate { get; }
        ActionRouteCollection GetRoutes(ActionRouteGenerationOptions routeOptions);
    }
}