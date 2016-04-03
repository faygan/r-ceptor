using System;
using System.Reflection;
using System.Web.Http.Description;

namespace Rceptor.Core.ServiceProxy
{

    public class ActionResponseTypeContext
    {

        public ActionResponseTypeContext(MethodInfo actionMethodInfo)
        {
            if (actionMethodInfo == null)
                throw new ArgumentNullException(nameof(actionMethodInfo));

            InitializeTypeInfos(actionMethodInfo);
        }

        public Type MethodReturnType { get; private set; }
        public ParameterInfo ReturnParameter { get; private set; }
        public Type ServiceReplyType { get; private set; }

        private void InitializeTypeInfos(MethodInfo methodInfo)
        {
            MethodReturnType = methodInfo.ReturnType;
            ReturnParameter = methodInfo.ReturnParameter;

            var contractProvider = methodInfo.GetCustomAttribute<OperationContractAttribute>();
            if (contractProvider != null)
                ServiceReplyType = contractProvider.ReplyType;
            else
            {
                var responseTypeProvider = methodInfo.GetCustomAttribute<ResponseTypeAttribute>();
                if (responseTypeProvider != null)
                    ServiceReplyType = responseTypeProvider.ResponseType;
            }
        }
    }
}