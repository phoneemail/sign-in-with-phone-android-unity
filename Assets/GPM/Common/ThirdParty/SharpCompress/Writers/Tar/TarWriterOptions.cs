#if CSHARP_7_3_OR_NEWER

using Gpm.Common.ThirdParty.SharpCompress.Common;

namespace Gpm.Common.ThirdParty.SharpCompress.Writers.Tar
{
    public class TarWriterOptions : WriterOptions
    {
        /// <summary>
        /// Indicates if archive should be finalized (by 2 empty blocks) on close.
        /// </summary>
        public bool FinalizeArchiveOnClose { get; }

        public TarWriterOptions(CompressionType compressionType, bool finalizeArchiveOnClose)
            : base(compressionType)
        {
            FinalizeArchiveOnClose = finalizeArchiveOnClose;
        }

        internal TarWriterOptions(WriterOptions options) : this(options.CompressionType, true)
        {
            ArchiveEncoding = options.ArchiveEncoding;
        }
    }
}

#endif