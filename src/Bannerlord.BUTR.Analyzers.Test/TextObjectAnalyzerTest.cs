using Bannerlord.BUTR.Analyzers.Analyzers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading.Tasks;

using TestHelper;

namespace Bannerlord.BUTR.Analyzers.Test
{
    [TestClass]
    public class TextObjectAnalyzerTest : BaseTest
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
        public async Task Correct_Const()
        {
            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        private const string TestConst = ""{{=EFASE3F31}}Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject(TestConst);
        }}
    }}
}}
").ValidateAsync();
        }

        [TestMethod]
        public async Task Correct_LocalConst()
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
            const string TestConst = ""{{=EFASE3F31}}Test"";
            var testValue = new TextObject(TestConst);
        }}
    }}
}}
").ValidateAsync();
        }

        [TestMethod]
        public async Task Correct_Static()
        {
            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        private static string TestStatic = ""{{=EFASE3F31}}Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject(TestStatic);
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

        [TestMethod]
        public async Task Incorrect_Const()
        {
            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = true;

            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        private const string TestConst = ""Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject([||]TestConst);
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
        private const string TestConst = ""{{=RANDOM}}Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject(TestConst);
        }}
    }}
}}
").ValidateAsync();

            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = false;
        }

        [TestMethod]
        public async Task Incorrect_LocalConst()
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
            const string TestConst = ""Test"";
            var testValue = new TextObject([||]TestConst);
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
            const string TestConst = ""{{=RANDOM}}Test"";
            var testValue = new TextObject(TestConst);
        }}
    }}
}}
").ValidateAsync();

            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = false;
        }

        [TestMethod]
        public async Task Incorrect_Static()
        {
            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = true;

            await CreateProjectBuilder().WithSourceCode($@"
{BannerlordBase}

namespace Bannerlord.Localization.Analyzer.Test
{{
    using TaleWorlds.Localization;

    class TestClass
    {{
        private static readonly string TestStatic = ""Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject([||]TestStatic);
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
        private static readonly string TestStatic = ""{{=RANDOM}}Test"";
        public static void TestMethod()
        {{
            var testValue = new TextObject(TestStatic);
        }}
    }}
}}
").ValidateAsync();

            TextObjectAnalyzerCodeFixProvider.OverrideRandomForTests = false;
        }
    }
}