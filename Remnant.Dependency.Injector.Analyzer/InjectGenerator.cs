using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remnant.Dependency.Injector
{
	[Generator]
	class DIGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var classes = context.Compilation.SyntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

			foreach (var @class in classes)
			{
				var fieldNodes = (from f in @class.DescendantNodes().OfType<FieldDeclarationSyntax>()
													from d in f.Declaration.Variables
													from attrs in f.AttributeLists
													from attr in attrs.Attributes
													where attr.Name + "Attribute" == "InjectAttribute"
													select f).ToList();

				if (fieldNodes.Any())
				{
					var constructor = @class.Members.OfType<ConstructorDeclarationSyntax>()
							.Where(x => x.ParameterList.Parameters.Count == 0).FirstOrDefault();

					var sb = new StringBuilder();

					var ns = @class.Parent as NamespaceDeclarationSyntax;

					sb.AppendLine("using Remnant.Dependency.Injector;");
					sb.AppendLine($"namespace {ns.Name}");
					sb.AppendLine("{");
					sb.AppendLine($"\tpublic partial class {@class.Identifier.Text}");
					sb.AppendLine("\t{");

					sb.AppendLine($"\t\tpublic static {@class.Identifier.Text} Create()");
					sb.AppendLine("\t\t{");
					sb.AppendLine($"\t\t\tvar instance = new {@class.Identifier.Text}();");
					sb.AppendLine("\t\t\tinstance.Inject();");
					sb.AppendLine("\t\t\treturn instance;");
					sb.AppendLine("\t\t}");

					if (constructor == null)
					{
						sb.AppendLine($"\t\tpublic {@class.Identifier.Text}()");
						sb.AppendLine("\t\t{");
						sb.AppendLine("\t\t\tInject();");
						sb.AppendLine("\t\t}");
					}

					sb.AppendLine($"\t\tprivate void Inject() {{");
					GenerateResolve(context, fieldNodes, sb);
					sb.AppendLine("\t\t}");
					sb.AppendLine("\t}"); //class
					sb.AppendLine("}"); //ns

					context.AddSource($"{@class.Identifier.Text}.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
				}
			}
		}

		private static void GenerateResolve(GeneratorExecutionContext context, List<FieldDeclarationSyntax> fieldNodes, StringBuilder sb)
		{
			foreach (var fieldDeclaration in fieldNodes)
			{
				foreach (var variable in fieldDeclaration.Declaration.Variables)
				{
					var fieldSymbol = variable.Identifier.Text;
					var symbol = context.Compilation.GetSemanticModel(variable.SyntaxTree).GetDeclaredSymbol(variable) as IFieldSymbol;
					var attr = symbol.GetAttributes().FirstOrDefault(a => a.GetType().FullName == "Remnant.Dependency.Injector.InjectAttribute");
					var injectType = symbol.Type.Name;

					if (attr != null)
					{
						injectType = attr.ConstructorArguments[0].Value.ToString();
					}

					sb.AppendLine($"\t\t\t{fieldSymbol} = this.Resolve<{injectType}>();");
				}
			}
		}
	}
}
