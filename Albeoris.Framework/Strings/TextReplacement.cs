using System;
using System.Text;

namespace Albeoris.Framework.Strings
{
    public sealed class TextReplacement
    {
        public readonly ReplaceTextDelegate Replace;

        private TextReplacement(String replacement)
        {
            Replace = (String str, StringBuilder word, ref Int32 index, ref Int32 length) => replacement;
        }

        private TextReplacement(ReplaceTextDelegate replacement)
        {
            Replace = replacement;
        }

        public static implicit operator TextReplacement(String replacement)
        {
            return new TextReplacement(replacement);
        }

        public static implicit operator TextReplacement(ReplaceTextDelegate replacement)
        {
            return new TextReplacement(replacement);
        }
    }
}