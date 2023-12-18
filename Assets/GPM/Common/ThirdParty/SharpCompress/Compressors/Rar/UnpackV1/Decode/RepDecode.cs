#if CSHARP_7_3_OR_NEWER

namespace Gpm.Common.ThirdParty.SharpCompress.Compressors.Rar.UnpackV1.Decode
{
    internal class RepDecode : Decode
    {
        internal RepDecode()
            : base(new int[PackDef.RC])
        {
        }
    }
}

#endif