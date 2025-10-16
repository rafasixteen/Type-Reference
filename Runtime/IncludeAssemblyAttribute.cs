using System;

namespace Rafasixteen.TypeReference.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class IncludeAssemblyAttribute : Attribute
    {
        public string AssemblyName { get; }

        public IncludeAssemblyAttribute(string assemblyName) => AssemblyName = assemblyName;
    }
}