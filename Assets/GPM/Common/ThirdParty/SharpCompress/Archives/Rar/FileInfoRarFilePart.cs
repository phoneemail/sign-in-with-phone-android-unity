#if CSHARP_7_3_OR_NEWER


#if CSHARP_7_3_OR_NEWER

#if !NO_FILE
using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar.Headers;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.Rar
{
    internal class FileInfoRarFilePart : SeekableFilePart
    {
        internal FileInfoRarFilePart(FileInfoRarArchiveVolume volume, string password, MarkHeader mh, FileHeader fh, FileInfo fi)
            : base(mh, fh, volume.Stream, password)
        {
            FileInfo = fi;
        }

        internal FileInfo FileInfo { get; }

        internal override string FilePartName
        {
            get
            {
                return "Rar File: " + FileInfo.FullName
                       + " File Entry: " + FileHeader.FileName;
            }
        }
    }
}
#endif

#endif

#endif