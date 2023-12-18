#if CSHARP_7_3_OR_NEWER

using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Common.SevenZip;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.SevenZip
{
    public class SevenZipArchiveEntry : SevenZipEntry, IArchiveEntry
    {
        internal SevenZipArchiveEntry(SevenZipArchive archive, SevenZipFilePart part)
            : base(part)
        {
            Archive = archive;
        }

        public Stream OpenEntryStream()
        {
            return FilePart.GetCompressedStream();
        }

        public IArchive Archive { get; }

        public bool IsComplete => true;

        /// <summary>
        /// This is a 7Zip Anti item
        /// </summary>
        public bool IsAnti => FilePart.Header.IsAnti;
    }
}

#endif