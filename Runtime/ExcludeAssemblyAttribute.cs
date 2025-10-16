using System;

namespace Rafasixteen.TypeReference.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ExcludeAssemblyAttribute : Attribute
    {
        public string AssemblyName { get; }

        public ExcludeAssemblyAttribute(string assemblyName) => AssemblyName = assemblyName;
    }
}