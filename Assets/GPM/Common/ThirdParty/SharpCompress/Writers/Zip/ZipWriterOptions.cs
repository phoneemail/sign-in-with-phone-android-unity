#if CSHARP_7_3_OR_NEWER

using Gpm.Common.ThirdParty.SharpCompress.Archives;
using Gpm.Common.ThirdParty.SharpCompress.Common;
using Gpm.Common.ThirdParty.SharpCompress.Compressors.Deflate;

namespace Gpm.Common.ThirdParty.SharpCompress.Writers.Zip
{
    public class ZipWriterOptions : WriterOptions
    {
        public ZipWriterOptions(CompressionType compressionType)
            : base(compressionType)
        {
        }

        internal ZipWriterOptions(WriterOptions options)
            : base(options.CompressionType)
        {
            LeaveStreamOpen = options.LeaveStreamOpen;
            ArchiveEncoding = options.ArchiveEncoding;

            var writerOptions = options as ZipWriterOptions;
            if (writerOptions != null)
            {
                UseZip64 = writerOptions.UseZip64;
                DeflateCompressionLevel = writerOptions.DeflateCompressionLevel;
                ArchiveComment = writerOptions.ArchiveComment;
            }
        }
        /// <summary>
        /// When CompressionType.Deflate is used, this property is referenced.  Defaults to CompressionLevel.Default.
        /// </summary>
        public CompressionLevel DeflateCompressionLevel { get; set; } = CompressionLevel.Default;

        public string ArchiveComment { get; set; }

        /// <summary>
        /// Sets a value indicating if zip64 support is enabled. 
		/// If this is not set, individual stream lengths cannot exceed 4 GiB.
		/// This option is not supported for non-seekable streams.
		/// Archives larger than 4GiB are supported as long as all streams
		/// are less than 4GiB in length.
        /// </summary>
        public bool UseZip64 { get; set; }
    }
}

#endif