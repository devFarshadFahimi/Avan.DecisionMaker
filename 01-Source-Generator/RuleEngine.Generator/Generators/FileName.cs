using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace RuleEngine.Generator.Generators;

[Generator]
public class DependencyInjectorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Where(static cls => cls is not null);

        var semanticTargets = classDeclarations
            .Combine(context.CompilationProvider)
            .Select((tuple, _) => AnalyzeClass(tuple.Left, tuple.Right))
            .Where(static result => result is not null);

        context.RegisterSourceOutput(semanticTargets.Collect(), (spc, classInfos) =>
        {
            var registrations = new StringBuilder();

            foreach (var info in classInfos)
            {
                if (info is null)
                {
                    continue;
                }

                foreach (var iface in info.ServiceInterfaces)
                {
                    bool isOpenGeneric = iface.IsGenericType && iface.TypeArguments.Any(arg => arg.TypeKind == TypeKind.TypeParameter);

                    if (isOpenGeneric)
                    {
                        // Remove `<T>` part to create unbound generic type name
                        var openIfaceName = iface.ConstructUnboundGenericType().ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        var openImplName = info.ClassSymbol.ConstructUnboundGenericType().ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                        registrations.AppendLine($"        services.{info.Lifetime}(typeof({openIfaceName}), typeof({openImplName}));");
                    }
                    else
                    {
                        registrations.AppendLine($"        services.{info.Lifetime}<{iface.ToDisplayString()}, {info.ClassSymbol.ToDisplayString()}>();");
                    }
                }
            }

            var source = $$"""
                using Microsoft.Extensions.DependencyInjection;
                namespace BusinessMakerFramework.SourceGenerator.AutoDI;

                public static class AutoDIRegistrationExtension
                {
                    public static IServiceCollection AddServicesByConvention(this IServiceCollection services)
                    {
                        {{registrations}}
                        return services;
                    }
                }
                """;

            spc.AddSource("AutoDIRegistrationExtension.g.cs", SourceText.From(source, Encoding.UTF8));
        });
    }

    private static ServiceRegistrationInfo? AnalyzeClass(ClassDeclarationSyntax classDecl, Compilation compilation)
    {
        var semanticModel = compilation.GetSemanticModel(classDecl.SyntaxTree);
        var classSymbol = semanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
        if (classSymbol is null || classSymbol.IsAbstract)
        {
            return null;
        }

        var lifetime = classSymbol.AllInterfaces.FirstOrDefault(i =>
            i.Name == "IScopedService" || i.Name == "ITransientService" || i.Name == "ISingletonService");

        if (lifetime is null)
        {
            return null;
        }

        var lifetimeMethod = lifetime.Name switch
        {
            "IScopedService" => "AddScoped",
            "ITransientService" => "AddTransient",
            "ISingletonService" => "AddSingleton",
            _ => null
        };

        if (lifetimeMethod is null)
        {
            return null;
        }

        var serviceInterfaces = classSymbol.AllInterfaces
            .Where(i => i.Name != "IScopedService" && i.Name != "ITransientService" && i.Name != "ISingletonService")
            .ToList();

        return new ServiceRegistrationInfo
        {
            ClassSymbol = classSymbol,
            ServiceInterfaces = serviceInterfaces,
            Lifetime = lifetimeMethod
        };
    }

    private record ServiceRegistrationInfo
    {
        public INamedTypeSymbol ClassSymbol { get; set; } = null!;
        public List<INamedTypeSymbol> ServiceInterfaces { get; set; } = [];
        public string Lifetime { get; set; } = null!;
    }
}
