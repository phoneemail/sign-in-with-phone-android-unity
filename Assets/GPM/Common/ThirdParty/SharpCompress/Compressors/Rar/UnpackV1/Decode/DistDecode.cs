#if CSHARP_7_3_OR_NEWER

namespace Gpm.Common.ThirdParty.SharpCompress.Compressors.Rar.UnpackV1.Decode
{
    internal class DistDecode : Decode
    {
        internal DistDecode()
            : base(new int[PackDef.DC])
        {
        }
    }
}

#endif