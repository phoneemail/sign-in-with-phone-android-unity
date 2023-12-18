#if CSHARP_7_3_OR_NEWER

using System;
using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Writers;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives
{
    public interface IWritableArchive : IArchive
    {
        void RemoveEntry(IArchiveEntry entry);

        IArchiveEntry AddEntry(string key, Stream source, bool closeStream, long size = 0, DateTime? modified = null);

        void SaveTo(Stream stream, WriterOptions options);
    }
}

#endif