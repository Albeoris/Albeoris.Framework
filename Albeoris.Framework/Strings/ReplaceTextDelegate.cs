using System;
using System.Text;

namespace Albeoris.Framework.Strings
{
    public delegate String ReplaceTextDelegate(String str, StringBuilder word, ref Int32 index, ref Int32 length);
}