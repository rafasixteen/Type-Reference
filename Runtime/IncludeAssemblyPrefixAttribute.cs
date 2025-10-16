using System;

namespace Rafasixteen.TypeReference.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class IncludeAssemblyPrefixAttribute : Attribute
    {
        public string[] Prefixes { get; }
        
        public IncludeAssemblyPrefixAttribute(params string[] prefixes) => Prefixes = prefixes;
    }
}