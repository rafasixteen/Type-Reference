using System;

namespace Rafasixteen.TypeReference.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ForceIncludeTypeAttribute : Attribute
    {
        public Type[] Types { get; }
        
        public ForceIncludeTypeAttribute(params Type[] types) => Types = types;
    }
}