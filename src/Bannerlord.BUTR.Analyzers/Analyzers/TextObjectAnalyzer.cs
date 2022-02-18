using Bannerlord.BUTR.Analyzers.Data;
using Bannerlord.BUTR.Analyzers.Utils;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

using System;
using System.Collections.Immutable;

namespace Bannerlord.BUTR.Analyzers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TextObjectAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            RuleIdentifiers.IdNotFoundRule
        );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.ObjectCreation);
        }

        private static void AnalyzeInvocation(OperationAnalysisContext context)
        {
            var operation = (IObjectCreationOperation) context.Operation;

            if (NameFormatter.ReflectionName(operation.Type) != "TaleWorlds.Localization.TextObject") return;

            if (operation.Arguments.Length == 2)
            {
                var textArg = operation.Arguments[0];
                if (textArg.Value is ILiteralOperation { ConstantValue: { HasValue: true, Value: string value } })
                {
                    if (!IsValidTextObject(value.AsSpan()))
                    {
                        var ctx = new GenericContext(context.Compilation, () => textArg.Syntax.GetLocation(), context.ReportDiagnostic);
                        context.ReportDiagnostic(RuleIdentifiers.ReportIdNotFound(ctx));
                    }
                }
            }
        }

        private static readonly string Start = "{=";

        private static bool IsValidTextObject(ReadOnlySpan<char> value)
        {
            if (value.IndexOf(Start.AsSpan()) is -1) return false;

            if (value.IndexOf('}') is var val && (val == -1 || val >= value.Length)) return false;

            return true;
        }
    }
}