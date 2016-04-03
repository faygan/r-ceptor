
namespace Rceptor.Core.ServiceProxy.Provider
{

    public interface IServiceBindingContextProvider
    {
        ServiceBindingContext GetContext(ServiceBindingContext currentContext);
    }

}
