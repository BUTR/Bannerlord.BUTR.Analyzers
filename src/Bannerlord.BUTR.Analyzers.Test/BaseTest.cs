namespace Bannerlord.BUTR.Analyzers.Test
{
    public class BaseTest
    {
        protected static readonly string BannerlordBase = @"
namespace TaleWorlds.Localization
{
    using System.Collections.Generic;

    public class TextObject
    {
        public TextObject(string value = """", Dictionary<string, object> attributes = null) { }
        public TextObject(int value, Dictionary<string, object> attributes = null) { }
        public TextObject(float value, Dictionary<string, object> attributes = null) { }
    }
}

namespace Unknown
{
    using System.Collections.Generic;

    public static class TextObjectHelper
    {
        public static object Create(string value = """", Dictionary<string, object> attributes = null) => null;
        public static object Create(int value, Dictionary<string, object> attributes = null) => null;
        public static object Create(float value, Dictionary<string, object> attributes = null) => null;
    }
}
";
    }
}