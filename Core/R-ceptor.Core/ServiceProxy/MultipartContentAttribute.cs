using System;

namespace Rceptor.Core.ServiceProxy
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
    public class MultipartContentAttribute : Attribute
    {

        public MultipartContentAttribute(Type contentType, string name)
        {
            ContentType = contentType;
            Name = name;
        }

        public Type ContentType { get; set; }
        public string Name { get; set; }

    }

}
