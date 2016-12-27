using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using Rceptor.Core.ServiceClient;

namespace Rceptor.Core.ServiceProxy
{
    public class ServiceInvoker
    {
        public static IServiceResponse InvokeOperation(OperationDescription operation, MethodInfo invokeMethod,
            object[] arguments, EndpointAddress endPoint)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var requestContext = operation.GetRequestContext(arguments);
            var bindingContext = operation.ServiceContract.ServiceBindingContext;

            return InvokeOperation(requestContext, endPoint, operation.ActionBinding.ResultTypeInfos, bindingContext);
        }

        public static IServiceResponse InvokeOperation(RestRequestContext context, EndpointAddress endPoint,
            ActionResponseTypeContext responseTypeContext, ServiceBindingContext serviceBindingContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            HttpResponseMessage responseMessage = null;

            try
            {
                var clientHandlers = serviceBindingContext.MessageHandlerProvider();

                using (var client = new DefaultRestClient(endPoint, clientHandlers.ToArray()))
                {
                    responseMessage = client.Send(context);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Rest method invoke error. {0}", ex.Message);
            }

            return new ServiceResponse(responseMessage, responseTypeContext, serviceBindingContext.Formatters);
        }
    }
}