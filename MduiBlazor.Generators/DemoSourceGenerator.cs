using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace MduiBlazor.Generators
{
    [Generator]
    public class DemoSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            // define the execution pipeline here via a series of transformations:

            // find all additional files that end with .razor
            IncrementalValueProvider<ImmutableArray<AdditionalText>> files = initContext.AdditionalTextsProvider.Where(file => file.Path.EndsWith(".razor")).Collect();

            // generate a class that contains their values as const strings
            initContext.RegisterSourceOutput(files, GenerateSource);
        }

        public void GenerateSource(SourceProductionContext context, ImmutableArray<AdditionalText> files)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("");
            sb.AppendLine("namespace MduiBlazor.Generators;");
            sb.AppendLine("public static class DemoSnippets");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static string GetRazor(string name)");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\tvar exampleData = new Dictionary<string,string>() {");
            foreach (AdditionalText file in files)
            {
                sb.Append("\t\t");
                SourceText f = file.GetText(context.CancellationToken);
                sb.AppendLine($@"{{ @""{Path.GetFileName(file.Path)}"", @""{f.ToString().Replace(@"""", @"""""")}"" }},");
            }
            sb.AppendLine("\t\t};");
            sb.Append("\t\t");
            sb.AppendLine($@"var foundPair = exampleData.FirstOrDefault(x => x.Key == name + "".razor"");");
            sb.AppendLine("\t\treturn foundPair.Value;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");
            context.AddSource("DemoSnippets.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }
}
