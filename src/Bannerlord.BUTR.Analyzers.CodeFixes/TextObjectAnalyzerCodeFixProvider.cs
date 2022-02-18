using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bannerlord.BUTR.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class TextObjectAnalyzerCodeFixProvider : CodeFixProvider
    {
        public static bool OverrideRandomForTests = false;

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
            RuleIdentifiers.IdNotFound
        );

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeToFix = root?.FindNode(context.Span, getInnermostNodeForTie: true);
            if (nodeToFix == null)
                return;

            var title = "Generate a Random Id";
            var codeAction = CodeAction.Create(
                title,
                ct => FixId(context.Document, nodeToFix, context.CancellationToken),
                equivalenceKey: title);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }

        private static async Task<Document> FixId(Document document, SyntaxNode nodeToFix, CancellationToken cancellationToken)
        {
            var chars = Array.Empty<char>()
                .Concat(Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char) x))
                .Concat(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char) x))
                .Concat(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char) x))
                .ToArray();
            var random = new Random();
            var id = OverrideRandomForTests ? "RANDOM" : new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());

            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            var generator = editor.Generator;

            if (nodeToFix is LiteralExpressionSyntax literal)
            {
                var text = literal.GetText().ToString().Trim('"');
                editor.ReplaceNode(nodeToFix, generator.LiteralExpression($"{{={id}}}{text}"));
            }

            return editor.GetChangedDocument();
        }
    }
}