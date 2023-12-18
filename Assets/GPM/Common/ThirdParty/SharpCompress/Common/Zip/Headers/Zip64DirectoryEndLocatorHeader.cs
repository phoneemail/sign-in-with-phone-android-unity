#if CSHARP_7_3_OR_NEWER

using System.IO;

namespace Gpm.Common.ThirdParty.SharpCompress.Common.Zip.Headers
{
    internal class Zip64DirectoryEndLocatorHeader : ZipHeader
    {
        public Zip64DirectoryEndLocatorHeader()
            : base(ZipHeaderType.Zip64DirectoryEndLocator)
        {
        }

        internal override void Read(BinaryReader reader)
        {
            FirstVolumeWithDirectory = reader.ReadUInt32();
            RelativeOffsetOfTheEndOfDirectoryRecord = (long)reader.ReadUInt64();
            TotalNumberOfVolumes = reader.ReadUInt32();
        }

        public uint FirstVolumeWithDirectory { get; private set; }

        public long RelativeOffsetOfTheEndOfDirectoryRecord { get; private set; }

        public uint TotalNumberOfVolumes { get; private set; }
    }
}

#endif