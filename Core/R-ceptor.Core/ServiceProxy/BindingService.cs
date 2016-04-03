using Rceptor.Core.ServiceProxy.Provider;

namespace Rceptor.Core.ServiceProxy
{
    public class BindingService<TFactory, TContext> where TFactory : IBindingFactory<TContext>, new()
    {
        public static TFactory GetFactory()
        {
            return new TFactory();
        }
    }
}