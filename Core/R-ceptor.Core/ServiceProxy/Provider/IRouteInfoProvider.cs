
namespace Rceptor.Core.ServiceProxy.Provider
{
    public interface IRouteInfoProvider
    {
        string RouteTemplate { get; }
        ActionRouteCollection GetRoutes(ActionRouteGenerationOptions routeOptions);
    }
}