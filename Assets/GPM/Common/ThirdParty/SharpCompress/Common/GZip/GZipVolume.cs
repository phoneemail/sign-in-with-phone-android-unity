#if CSHARP_7_3_OR_NEWER

using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Readers;

namespace Gpm.Common.ThirdParty.SharpCompress.Common.GZip
{
    public class GZipVolume : Volume
    {
        public GZipVolume(Stream stream, ReaderOptions options)
            : base(stream, options)
        {
        }

#if !NO_FILE
        public GZipVolume(FileInfo fileInfo, ReaderOptions options)
            : base(fileInfo.OpenRead(), options)
        {
            options.LeaveStreamOpen = false;
        }
#endif

        public override bool IsFirstVolume => true;

        public override bool IsMultiVolume => true;
    }
}

#endif