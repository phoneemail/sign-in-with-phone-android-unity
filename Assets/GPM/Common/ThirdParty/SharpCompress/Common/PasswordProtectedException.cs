#if CSHARP_7_3_OR_NEWER

using System;

namespace Gpm.Common.ThirdParty.SharpCompress.Common
{
    public class PasswordProtectedException : ExtractionException
    {
        public PasswordProtectedException(string message)
            : base(message)
        {
        }

        public PasswordProtectedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

#endif