using System;
using System.Collections.Generic;

namespace Rceptor.Core.ServiceProxy
{
    public class ContractDescription
    {
        public ContractDescription(Type contractType)
        {
            ContractType = contractType;
        }

        public Type ContractType { get; private set; }
        public string RoutePrefix { get; internal set; }
        public string ServiceName { get; internal set; }
        public ServiceBindingContext ServiceBindingContext { get; set; }

        public IEnumerable<OperationDescription> Operations { get; set; }
    }
}