#if CSHARP_7_3_OR_NEWER

namespace Gpm.Common.ThirdParty.SharpCompress.Common.Rar.Headers
{
    internal interface IRarHeader 
    {
        HeaderType HeaderType { get; }
    }
}

#endif