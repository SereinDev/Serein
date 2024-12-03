using System;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Serein.Tests.Utils;

#pragma warning disable IL3000

public static class CSharpCompilationHelper
{
    private static readonly CSharpCompilationOptions Default = new CSharpCompilationOptions(
        OutputKind.DynamicallyLinkedLibrary
    )
        .WithOptimizationLevel(OptimizationLevel.Release)
        .WithPlatform(Platform.AnyCpu);

    public static void Compile(
        string code,
        string assemblyName,
        string path,
        params MetadataReference[] metadataReferences
    )
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            code,
            CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
        );
        var references = metadataReferences
            .Concat(
                AppDomain
                    .CurrentDomain.GetAssemblies()
                    .Where((assembly) => File.Exists(assembly.Location))
                    .Select((assembly) => MetadataReference.CreateFromFile(assembly.Location))
            )
            .Distinct();

        var compilation = CSharpCompilation.Create(assemblyName, [syntaxTree], references, Default);
        var emitResult = compilation.Emit(path);

        if (!emitResult.Success)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Compilation failed:");
            foreach (var diagnostic in emitResult.Diagnostics)
            {
                stringBuilder.AppendLine("  " + diagnostic.ToString());
            }
            throw new InvalidOperationException(stringBuilder.ToString());
        }
    }
}
