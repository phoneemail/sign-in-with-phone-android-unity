#if CSHARP_7_3_OR_NEWER

namespace Gpm.Common.ThirdParty.SharpCompress.Compressors.Rar.UnpackV1.Decode
{
    internal class LitDecode : Decode
    {
        internal LitDecode()
            : base(new int[PackDef.NC])
        {
        }
    }
}

#endif