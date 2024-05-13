using System;
using System.Collections.Generic;

namespace NetFlow.DocumentationHelper.Library.Models
{
    public class DocumentationAssembly
    {
        public string AssemblyName;
        public IEnumerable<Type> Types;
    }
}