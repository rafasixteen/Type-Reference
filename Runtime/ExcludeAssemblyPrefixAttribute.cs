using System;

namespace Rafasixteen.TypeReference.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ExcludeAssemblyPrefixAttribute : Attribute
    {
        public string[] Prefixes { get; }
        
        public ExcludeAssemblyPrefixAttribute(params string[] prefixes) => Prefixes = prefixes;
    }
}