// MIT License
//
// Copyright (c) 2018 fastwildcard
// Copyright (c) 2020 Albeoris
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// https://github.com/fastwildcard/fastwildcard

using System;

namespace Albeoris.Framework.System
{
    public sealed class Wildcard
    {
        private const Char SingleWildcardCharacter = '?';
        private const Char MultiWildcardCharacter = '*';

        private static readonly Char[] WildcardCharacters = {SingleWildcardCharacter, MultiWildcardCharacter};

        public String Pattern { get; }
        public StringComparison StringComparison { get; }

        public Wildcard(String pattern, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (pattern is null) throw new ArgumentNullException(nameof(pattern));
            if (pattern == String.Empty) throw new ArgumentEmptyException(nameof(pattern));
            
            Pattern = pattern;
            StringComparison = stringComparison;
        }

        /// <summary>
        /// Returns true if the input string <paramref name="str"/> matches the given wildcard pattern.
        /// </summary>
        /// <param name="str">Input string to match on</param>
        /// <returns>True if a match is found, false otherwise</returns>
        public Boolean IsMatch(String str)
        {
            return IsMatchInternal(str, Pattern, StringComparison);
        }

        /// <summary>
        /// Returns true if the input string <paramref name="str"/> matches the given wildcard pattern <paramref name="pattern"/>.
        /// </summary>
        /// <param name="str">Input string to match on</param>
        /// <param name="pattern">Wildcard pattern to match with</param>
        /// <param name="stringComparison">String comparison settings to use</param>
        /// <returns>True if a match is found, false otherwise</returns>
        public static Boolean IsMatch(String str, String pattern, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (pattern is null) throw new ArgumentNullException(nameof(pattern));
            if (pattern == String.Empty) throw new ArgumentEmptyException(nameof(pattern));

            return IsMatchInternal(str, pattern, stringComparison);
        }

        private static Boolean IsMatchInternal(String str, String pattern, StringComparison stringComparison)
        {
            // Uninitialised string never matches
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            // Multi character wildcard matches everything
            if (pattern == "*")
                return true;

            // Empty string does not match
            if (str.Length == 0)
                return false;

            var strIndex = 0;

            for (var patternIndex = 0; patternIndex < pattern.Length; patternIndex++)
            {
                var patternCh = pattern[patternIndex];

                if (strIndex == str.Length)
                {
                    // At end of pattern for this longer string so always matches '*'
                    return patternCh == '*' && patternIndex == pattern.Length - 1;
                }

                // Character match
                var strCh = str[strIndex];
                var patternChEqualsStrAtIndex = stringComparison == StringComparison.Ordinal
                    ? patternCh.Equals(strCh)
                    : patternCh.ToString().Equals(strCh.ToString(), stringComparison);

                if (patternChEqualsStrAtIndex)
                {
                    strIndex++;
                    continue;
                }

                // Single wildcard match
                if (patternCh == '?')
                {
                    strIndex++;
                    continue;
                }

                // No match
                if (patternCh != '*')
                    return false;

                // Multi character wildcard - last character in the pattern
                if (patternIndex == pattern.Length - 1)
                    return true;

                // Match pattern to input string character-by-character until the next wildcard (or end of string if there is none)
                var patternChMatchStartIndex = patternIndex + 1;

                var nextWildcardIndex = pattern.IndexOfAny(WildcardCharacters, patternChMatchStartIndex);
                var patternChMatchEndIndex = nextWildcardIndex == -1
                    ? pattern.Length - 1
                    : nextWildcardIndex - 1;

                var comparisonLength = patternChMatchEndIndex - patternIndex;

                var comparison = pattern.Substring(patternChMatchStartIndex, comparisonLength);
                var skipToStringIndex = str.IndexOf(comparison, strIndex, stringComparison);

                // Handle repeated instances of the same character at end of pattern
                if (comparisonLength == 1 && nextWildcardIndex == -1)
                {
                    var skipCandidateIndex = 0;
                    while (skipCandidateIndex == 0)
                    {
                        var skipToStringIndexNew = skipToStringIndex + 1;
                        skipCandidateIndex = str.IndexOf(comparison, skipToStringIndexNew, stringComparison) - (skipToStringIndexNew);
                        if (skipCandidateIndex == 0)
                        {
                            skipToStringIndex = skipToStringIndexNew;
                        }
                    }
                }

                if (skipToStringIndex == -1)
                    return false;

                strIndex = skipToStringIndex;
            }

            // Pattern processing completed but rest of input string was not
            return strIndex >= str.Length;
        }
    }
}