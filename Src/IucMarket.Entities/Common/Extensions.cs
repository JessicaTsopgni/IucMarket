using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Entities.Common
{
    public static class Extensions
    {
            public static string ToUcFirst(this string s)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }
                s = s.ToLower();
                char[] a = s.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                return new string(a);
            }

    }
}
