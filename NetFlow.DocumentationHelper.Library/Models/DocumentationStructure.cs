using System;
using System.Collections.Generic;
using NetFlow.DocumentationHelper.Library.Attributes;

namespace NetFlow.DocumentationHelper.Library.Models
{

    public class DocumentationStructure
    {
        public string AssemblyName;
        public string ClassName;
        public List<DocumentationDescription> Descriptions;

        public DocumentationStructure(string assemblyName, string className)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            Descriptions = new List<DocumentationDescription>();
        }

        public void AddDescription(DocumentationDescription description)
        {
            Descriptions.Add(description);
        }
    }

    public class DocumentationDescription
    {
        public string Title;
        public string Description;
        public string[] Parameters;

        [Documentation(nameof(DocumentationDescription), "Description of the current Documented object", new []
        {
            "title - Name of the object",
            "description - Description of what the object does",
            "parameters - The parameters for the object (in case of a method with parameters)"
        })]
        public DocumentationDescription(string title, string description, string[] parameters = default)
        {
            Title = title;
            Description = description;
            Parameters = parameters ?? Array.Empty<string>();
        }
    }
}