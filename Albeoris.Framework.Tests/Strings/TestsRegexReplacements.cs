using System;
using System.Text;
using System.Text.RegularExpressions;
using Albeoris.Framework.Strings;
using NUnit.Framework;

namespace Albeoris.Framework.Tests.Strings
{
    public sealed class TestsRegexReplacements
    {
        private RegexReplacements _regex;

        [SetUp]
        public void Setup()
        {
            _regex = new RegexReplacements();
            _regex.AddWord("Hello", "{Hello}", RegexOptions.Compiled);
            _regex.AddWord("World", "{World}", RegexOptions.Compiled);
            _regex.AddWord("Hello World", "{Hello World}", RegexOptions.Compiled);
        }

        [Test(ExpectedResult = "")]
        public String Empty() => _regex.ReplaceAll(text: "");

        [Test(ExpectedResult = "Text")]
        public String NoMatch() => _regex.ReplaceAll(text: "Text");

        [Test(ExpectedResult = "{Hello World}")]
        public String LongMatch() => _regex.ReplaceAll(text: "Hello World");

        [Test(ExpectedResult = "!{Hello World}!")]
        public String LongMatchAndText() => _regex.ReplaceAll(text: "!Hello World!");

        [Test(ExpectedResult = "!{Hello} {Hello World} {World}!")]
        public String MatchesAndText() => _regex.ReplaceAll(text: "!Hello Hello World World!");
    }
}