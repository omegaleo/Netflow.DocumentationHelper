using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NetFlow.DocumentationHelper.Library.Attributes;
using NetFlow.DocumentationHelper.Library.Models;

namespace NetFlow.DocumentationHelper.Library.Helpers
{

    public static class DocumentationHelperTool
    {
        [Documentation("GenerateDocumentation (bool generateForPackageAssembly)",
            @"Generates a List of objects of type DocumentationStructure that contain the following fields:\
**AssemblyName**: Name of the main Assembly, used to identify the root namespace\
**ClassName**: Name of the class, used to identify the upper level object\
**Title**: Title what we're generating documentation for\
**Description**: Description of what we're generating documentation for, this can contain usage examples and can use the args array to pass names(e.g.: This method uses this methodology)\
\
*Note: If generateForPackageAssembly is set to true, this will generate documentation for the library as well.*\",
            new []
            {
                "generateForPackageAssembly - Generate documentation for the DocumentationHelper library as well?"
            })]
        
        public static IEnumerable<DocumentationStructure> GenerateDocumentation(bool generateForPackageAssembly = false)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a =>
                    new DocumentationAssembly()
                    {
                        AssemblyName = a.GetName(false)?.Name ?? nameof(a),
                        Types = GetTypesWithDocumentationAttribute(a)
                    }).Where(x => x.Types.Any()).ToList();

            if (!generateForPackageAssembly)
            {
                assemblies =
                    assemblies.Where(x => x.AssemblyName != typeof(DocumentationHelperTool).Assembly.GetName().Name)
                        .ToList();
            }

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.Types)
                {
                    var doc = GetDocumentation(type, assembly.AssemblyName);
                    yield return doc;
                }
            }
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        static DocumentationStructure GetDocumentation(Type type, string assemblyName)
        {
            var classDocs = type.GetCustomAttributes(typeof(DocumentationAttribute), true)
                .Select(x => (DocumentationAttribute)x);
            
            var fieldDocs = type.GetFields().SelectMany(f =>
                f.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            var propertyDocs = type.GetProperties().SelectMany(p =>
                p.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            var methodDocs = type.GetMethods().SelectMany(m =>
                m.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            
            var docStructure = new DocumentationStructure(assemblyName, type.Name);

            foreach (var doc in classDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args));
            }

            foreach (var doc in fieldDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args));
            }

            foreach (var doc in propertyDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args));
            }

            foreach (var doc in methodDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args));
            }

            return docStructure;
        }
        
        static Dictionary<string, string> loadedXmlDocumentation =
            new Dictionary<string, string>();
        public static void LoadXmlDocumentation(string xmlDocumentation)
        {
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlDocumentation)))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                    {
                        string raw_name = xmlReader["name"];
                        loadedXmlDocumentation[raw_name] = xmlReader.ReadInnerXml();
                    }
                }
            }
        }
        
        static IEnumerable<Type> GetTypesWithDocumentationAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var classHasDoc = type.GetCustomAttributes(typeof(DocumentationAttribute), true).Any();
                var fieldsHaveDoc = type.GetFields()
                    .Any(f => f.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());
                var typesHaveDoc = type.GetProperties()
                    .Any(p => p.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());
                var methodsHaveDoc = type.GetMethods()
                    .Any(m => m.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());

                if (classHasDoc || fieldsHaveDoc || typesHaveDoc || methodsHaveDoc)
                {
                    yield return type;
                }
            }
        }
    }
}