using Bannerlord.BUTR.Analyzers.Data;
using Bannerlord.BUTR.Analyzers.Utils;

using Microsoft.CodeAnalysis;

using System.Globalization;

namespace Bannerlord.BUTR.Analyzers
{
    public static class RuleIdentifiers
    {
        public const string IdNotFound = "BLA0001";

        private static string GetHelpUri(string idenfifier) =>
            string.Format(CultureInfo.InvariantCulture, "https://github.com/BUTR/Bannerlord.BUTR.Analyzers/blob/master/docs/Rules/{0}.md", idenfifier);

        internal static readonly DiagnosticDescriptor IdNotFoundRule = new(
            IdNotFound,
            title: "Id was not found!",
            messageFormat: "Id was not found",
            RuleCategories.Usage,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "",
            helpLinkUri: GetHelpUri(IdNotFound));

        internal static Diagnostic ReportIdNotFound(GenericContext context) =>
            DiagnosticUtils.CreateDiagnostic(IdNotFoundRule, context);
    }
}