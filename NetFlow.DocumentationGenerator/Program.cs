
using Octokit;
using NetFlow.DocumentationHelper.Library.Helpers;

class Program
{
    static async Task Main(string[] args)
    {
        var docs = DocumentationHelperTool.GenerateDocumentation(true);
        
        var client = new GitHubClient(new ProductHeaderValue("unityflow"));
        var tokenAuth = new Credentials(Environment.GetEnvironmentVariable("UNITYFLOW_SECRET"));
        client.Credentials = tokenAuth;

        var filePath = "documentation.md";
        
        if (!File.Exists(filePath))
        {
            await File.WriteAllTextAsync(filePath, "");
        }

        var documentation = docs
            .GroupBy(doc => doc.AssemblyName)
            .Select(x =>
            {
                var assembly = $"# {x.Key}{Environment.NewLine}";

                var assemblyDocs = x.Select(doc =>
                {
                    var documentation = $"## {doc.ClassName}{Environment.NewLine}";

                    foreach (var desc in doc.Descriptions)
                    {
                        if (!string.Equals(doc.ClassName, desc.Title))
                        {
                            documentation += $"### {desc.Title}{Environment.NewLine}";
                        }

                        documentation += $"{desc.Description}{Environment.NewLine}";

                        if (desc.Parameters.Any())
                        {
                            documentation +=
                                $"*Parameters:*{Environment.NewLine}{string.Join(Environment.NewLine, desc.Parameters)}";
                        }
                    }

                    return documentation;
                });

                assembly += string.Join(Environment.NewLine, assemblyDocs);
                
                return assembly;
            });

        
        /**foreach (var doc in docs)
        {
            documentation += $"# {doc.AssemblyName}{Environment.NewLine}  ";
            documentation += $"## {doc.ClassName}{Environment.NewLine}  ";

            foreach (var desc in doc.Descriptions)
            {
                documentation += $"### {desc.Title}{Environment.NewLine}  ";
                documentation += $"{desc.Description}{Environment.NewLine}  ";
            }
        }**/
        
        File.WriteAllText(filePath, string.Join(Environment.NewLine, documentation));
    }
}
