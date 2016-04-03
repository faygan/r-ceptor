namespace Rceptor.Core.ServiceProxy.Provider
{
    public interface IBindingFactory<in TContext>
    {
        T GetBinding<T>(TContext context) where T : IBinding;
    }
}