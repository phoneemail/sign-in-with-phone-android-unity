#if CSHARP_7_3_OR_NEWER


#if NET35
using System;

namespace Gpm.Common.ThirdParty.SharpCompress
{
    internal static class EnumExtensions
    {
        public static bool HasFlag(this Enum enumRef, Enum flag)
        {
            long value = Convert.ToInt64(enumRef);
            long flagVal = Convert.ToInt64(flag);

            return (value & flagVal) == flagVal;
        }
    }
}
#endif

#endif