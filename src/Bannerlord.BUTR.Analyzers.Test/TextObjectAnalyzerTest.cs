using Bannerlord.BUTR.Analyzers.Analyzers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading.Tasks;

using TestHelper;

namespace Bannerlord.BUTR.Analyzers.Test
{
    [TestClass]
    public partial class TextObjectAnalyzerTest : BaseTest
    {
        private static ProjectBuilder CreateProjectBuilder() => new ProjectBuilder()
            .WithAnalyzer<TextObjectAnalyzer>()
            .WithCodeFixProvider<TextObjectAnalyzerCodeFixProvider>();

        [TestMethod]
        public async Task Correct()
        {
            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        public static void TestMethod()
        {{
            var testValue = new TextObject(""{{=EFASE3F31}}Test"");
        }}
    }}
}}
").ValidateAsync();
        }

        [TestMethod]
        public async Task Incorrect()
        {
            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = true;

            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        public static void TestMethod()
        {{
            var testValue = new TextObject([||]""Test"");
        }}
    }}
}}
").ShouldFixCodeWith($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        public static void TestMethod()
        {{
            var testValue = new TextObject(""{{=RANDOM}}Test"");
        }}
    }}
}}
").ValidateAsync();

            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = false;
        }
    }
}