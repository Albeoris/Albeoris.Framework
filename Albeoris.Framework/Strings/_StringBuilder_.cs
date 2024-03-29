﻿using System;
using System.Text;

namespace Albeoris.Framework.Strings
{
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once InconsistentNaming
    public static class _StringBuilder_
    {
        public static void Clear(this StringBuilder self)
        {
            self.Length = 0;
        }
        
        public static StringBuilder Append(this StringBuilder self, String text, Int32 startIndex)
        {
            var length = text.Length - startIndex;
            self.Append(text, startIndex, length);

            return self;
        }
        
        public static StringBuilder AppendFormatLine(this StringBuilder self, String format, params Object[] args)
        {
            self.AppendFormat(format, args);
            self.AppendLine();

            return self;
        }

        public static Int32 IndexOf(this StringBuilder self, Int32 offset, Char ch)
        {
            for (Int32 i = offset; i < self.Length; i++)
            {
                if (self[i] == ch)
                    return i;
            }

            return -1;
        }

        public static Int32 IndexFromEnd(this StringBuilder self, Int32 offset, Char ch)
        {
            for (Int32 i = offset; i >= 0; i--)
            {
                if (self[i] == ch)
                    return i;
            }

            return -1;
        }

        public static String Evict(this StringBuilder self)
        {
            String result = self.ToString();
            self.Length = 0;
            return result;
        }
    }
}