#if CSHARP_7_3_OR_NEWER

using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar.Headers;

namespace Gpm.Common.ThirdParty.SharpCompress.Compressors.Rar
{
    internal interface IRarUnpack
    {
        void DoUnpack(FileHeader fileHeader, Stream readStream, Stream writeStream);
        void DoUnpack();

        // eg u/i pause/resume button
        bool Suspended { get; set; }

        long DestSize { get; }
        int Char { get; }
        int PpmEscChar { get; set; }
    }
}


#endif