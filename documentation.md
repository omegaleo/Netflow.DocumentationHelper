# NetFlow.DocumentationHelper.Library
## DocumentationHelperTool
### GenerateDocumentation (bool generateForPackageAssembly)
Generates a List of objects of type DocumentationStructure that contain the following fields:\
**AssemblyName**: Name of the main Assembly, used to identify the root namespace\
**ClassName**: Name of the class, used to identify the upper level object\
**Title**: Title what we're generating documentation for\
**Description**: Description of what we're generating documentation for, this can contain usage examples and can use the args array to pass names(e.g.: This method uses this methodology)\
\
*Note: If generateForPackageAssembly is set to true, this will generate documentation for the library as well.*

*Parameters:* \ generateForPackageAssembly - Generate documentation for the DocumentationHelper library as well?