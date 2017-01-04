using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
#if !WINDOWS_UWP
    public static class TypeExtensions
    {
        public static Type GetTypeInfo(this Type t)
        {
            return t;
        }
    }
#endif
}
