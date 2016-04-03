using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Http;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Rceptor.Core.ServiceProxy.Interceptor;

namespace Rceptor.Core.ServiceProxy
{

    public class ChannelFactory<TContract>
        where TContract : class
    {

        #region Properties

        public EndpointAddress EndPoint { get; set; }
        public ServiceBindingContext ServiceContext { get; }

        public Type ContractType { get; }
        public ContractDescription ServiceContract { get; private set; }
        public IEnumerable<OperationDescription> ServiceOperations => ServiceContract?.Operations;


        #endregion

        #region Constructors

        public ChannelFactory(string serviceAddress, ServiceBindingContext context = null)
            : this(new Uri(serviceAddress), context)
        {
        }

        public ChannelFactory(Uri serviceUri, ServiceBindingContext context = null)
            : this(new EndpointAddress(serviceUri), context)
        {
        }

        public ChannelFactory(EndpointAddress endPoint, ServiceBindingContext context = null) : this()
        {
            EndPoint = endPoint;
            ServiceContext = context;
        }

        public ChannelFactory()
        {
            ContractType = typeof(TContract);

            InitializeContract(ContractType);
        }

        #endregion

        #region Factory Methods

        private void InitializeContract(Type contractType)
        {
            ServiceContract = new ContractDescription(contractType);

            var serviceContractAttr = contractType.GetCustomAttribute<ServiceContractAttribute>(true);
            if (serviceContractAttr == null)
                throw new Exception("Couldn' t found ServiceContract attribute on target service..");

            var routePrefix = serviceContractAttr.Prefix;
            var serviceName = serviceContractAttr.ServiceName;

            // If route prefix not provided, check applied RoutePrefix attribute
            if (string.IsNullOrEmpty(routePrefix))
            {
                var routePrefixAttr = contractType.GetCustomAttribute<RoutePrefixAttribute>(false);

                if (routePrefixAttr != null)
                {
                    routePrefix = routePrefixAttr.Prefix;
                }
            }

            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = contractType.Name;
            }

            ServiceContract.ServiceName = serviceName;
            ServiceContract.RoutePrefix = routePrefix;

            var bindingContext = new DefaultServiceBindingContextProvider();
            ServiceContract.ServiceBindingContext = bindingContext.GetContext(ServiceContext);

            var operationsMethods = contractType.GetMethods();

            var operations = operationsMethods
                .Select(methodInfo => new OperationDescription
                {
                    ServiceContract = ServiceContract,
                    MethodInfo = methodInfo,
                    Name = methodInfo.Name
                }).ToArray();

            var bindingFactory = BindingService<ApiActionBindingFactory, ApiActionBindingContext>.GetFactory();

            operations.ForEach(f =>
            {
                if (f.MethodInfo != null && bindingFactory != null)
                {
                    f.ActionBinding = bindingFactory
                        .GetBinding<ApiActionBinding>(new ApiActionBindingContext(f.MethodInfo, ServiceContract.ServiceBindingContext.RouteBuildOptions));
                }
            });

            // Only register api action method..
            ServiceContract.Operations = operations.Where(op => !op.ActionBinding.IsNonAction);
        }

        public TContract CreateChannel(string serviceAddress)
        {
            return CreateChannel(new EndpointAddress(serviceAddress));
        }

        public TContract CreateChannel(Uri serviceUri)
        {
            return CreateChannel(new EndpointAddress(serviceUri));
        }

        public TContract CreateChannel()
        {
            return CreateChannel(EndPoint);
        }

        public TContract CreateChannel(EndpointAddress endPoint)
        {
            if (ServiceContract?.Operations == null)
                throw new Exception("Service contract is not initialized..");
            if (!ServiceContract.Operations.Any())
                throw new Exception("Service contract does not contain any operation..");
            if (endPoint == null && EndPoint == null)
                throw new ArgumentNullException(nameof(endPoint), "Service endpoint not defined..");

            if (endPoint != null)
                EndPoint = endPoint;

            var interceptors = ServiceContract.Operations
                .Select(operation => new OperationContractInterceptor(operation, EndPoint))
                .Cast<IInterceptor>().ToArray();

            var proxyOptions = new ProxyGenerationOptions
            {
                Selector = new ContractInterceptorSelector()
            };

            return new ProxyGenerator()
                .CreateInterfaceProxyWithoutTarget<TContract>(proxyOptions, interceptors);
        }

        #endregion
    }

}