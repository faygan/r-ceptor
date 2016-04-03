using System.ServiceModel;
using Castle.DynamicProxy;

namespace Rceptor.Core.ServiceProxy.Interceptor
{

    public class OperationContractInterceptor : IInterceptor
    {

        private readonly EndpointAddress _endPoint;
        public OperationDescription Operation;

        public OperationContractInterceptor(OperationDescription operation, EndpointAddress endPoint = null)
        {
            Operation = operation;
            _endPoint = endPoint;
        }

        public void Intercept(IInvocation invocation)
        {
            if (Operation == null)
                return;

            var response = ServiceInvoker.InvokeOperation(Operation, invocation.Method, invocation.Arguments, _endPoint);
            invocation.ReturnValue = response;
        }
    }
}