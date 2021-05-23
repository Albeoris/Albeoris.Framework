using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Albeoris.Framework.Strings
{
    public sealed class RegexReplacements
    {
        private readonly List<KeyValuePair<Regex, String>> _list = new();

        public void Add(Regex from, String to)
        {
            AddInternal(from, to);
        }

        public void AddWord(String from, String to, RegexOptions options)
        {
            String pattern = @$"\b{Regex.Escape(from)}\b";
            Regex regex = new(pattern, options);
            AddInternal(regex, to);
        }

        public String ReplaceAll(String text)
        {
            Int32 currentIndex = 0;
            Match? currentMatch = null;
            String? currentTo = null;
            StringBuilder? sb = null;

            while (true)
            {
                currentMatch = null;
                currentTo = null;

                foreach (var pair in _list)
                {
                    Regex regex = pair.Key;

                    Match m = regex.Match(text, currentIndex);
                    if (!m.Success)
                        continue;

                    String to = pair.Value;

                    if (currentMatch is null
                        || (currentMatch.Index > m.Index)
                        || (currentMatch.Index == m.Index && currentMatch.Length < m.Length))
                    {
                        currentMatch = m;
                        currentTo = to;
                    }
                }

                if (currentMatch is null)
                    break;

                sb ??= new StringBuilder();
                
                sb.Append(text, currentIndex, currentMatch.Index - currentIndex);
                currentIndex = currentMatch.Index;
                
                sb.Append(currentMatch.Result(currentTo!));
                currentIndex += currentMatch.Length;
            }

            if (sb is null)
                return text;

            sb.Append(text, currentIndex);
            return sb.ToString();
        }

        private void AddInternal(Regex from, String to)
        {
            _list.Add(new KeyValuePair<Regex, String>(from, to));
        }
    }
}