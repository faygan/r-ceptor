namespace Rceptor.Core.ServiceClient
{

    public static class ServiceResponseExtensions
    {

        public static T GetContentObject<T>(this IServiceResponse response)
        {
            var result = response.GetContentObject();
            return (T) result;
        }

    }
}