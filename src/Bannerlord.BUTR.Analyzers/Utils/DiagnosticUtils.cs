using Bannerlord.BUTR.Analyzers.Data;

using Microsoft.CodeAnalysis;

using System.Collections.Immutable;

namespace Bannerlord.BUTR.Analyzers.Utils
{
    internal static class DiagnosticUtils
    {
        private static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, Location location, ImmutableDictionary<string, string?>? properties, params string[] messageArgs)
        {
            return Diagnostic.Create(descriptor, location, properties, messageArgs);
        }

        public static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, GenericContext context, params string[] messageArgs)
        {
            return CreateDiagnostic(descriptor, ImmutableDictionary<string, string?>.Empty, context, messageArgs);
        }
        public static Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, ImmutableDictionary<string, string?>? properties, GenericContext context, params string[] messageArgs)
        {
            return CreateDiagnostic(descriptor, context.GetLocation(), properties, messageArgs);
        }
    }
}