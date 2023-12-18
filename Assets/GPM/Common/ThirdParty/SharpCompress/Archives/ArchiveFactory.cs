#if CSHARP_7_3_OR_NEWER

using System;
using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Archives.GZip;
using Gpm.Common.ThirdParty.SharpCompress.Archives.Rar;
using Gpm.Common.ThirdParty.SharpCompress.Archives.SevenZip;
using Gpm.Common.ThirdParty.SharpCompress.Archives.Tar;
using Gpm.Common.ThirdParty.SharpCompress.Archives.Zip;
using Gpm.Common.ThirdParty.SharpCompress.Common;
using Gpm.Common.ThirdParty.SharpCompress.Compressors.LZMA;
using Gpm.Common.ThirdParty.SharpCompress.Readers;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives
{
    public class ArchiveFactory
    {
        /// <summary>
        /// Opens an Archive for random access
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="readerOptions"></param>
        /// <returns></returns>
        public static IArchive Open(Stream stream, ReaderOptions readerOptions = null)
        {
            stream.CheckNotNull("stream");
            if (!stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException("Stream should be readable and seekable");
            }
            readerOptions = readerOptions ?? new ReaderOptions();
            if (ZipArchive.IsZipFile(stream, null))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return ZipArchive.Open(stream, readerOptions);
            }
            stream.Seek(0, SeekOrigin.Begin);
            if (SevenZipArchive.IsSevenZipFile(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return SevenZipArchive.Open(stream, readerOptions);
            }
            stream.Seek(0, SeekOrigin.Begin);
            if (GZipArchive.IsGZipFile(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return GZipArchive.Open(stream, readerOptions);
            }
            stream.Seek(0, SeekOrigin.Begin);
            if (RarArchive.IsRarFile(stream, readerOptions))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return RarArchive.Open(stream, readerOptions);
            }
            stream.Seek(0, SeekOrigin.Begin);
            if (TarArchive.IsTarFile(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return TarArchive.Open(stream, readerOptions);
            }
            throw new InvalidOperationException("Cannot determine compressed stream type. Supported Archive Formats: Zip, GZip, Tar, Rar, 7Zip, LZip");
        }

        public static IWritableArchive Create(ArchiveType type)
        {
            switch (type)
            {
                case ArchiveType.Zip:
                {
                    return ZipArchive.Create();
                }
                case ArchiveType.Tar:
                {
                    return TarArchive.Create();
                }
                case ArchiveType.GZip:
                {
                    return GZipArchive.Create();
                }
                default:
                {
                    throw new NotSupportedException("Cannot create Archives of type: " + type);
                }
            }
        }

#if !NO_FILE

        /// <summary>
        /// Constructor expects a filepath to an existing file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="options"></param>
        public static IArchive Open(string filePath, ReaderOptions options = null)
        {
            filePath.CheckNotNullOrEmpty("filePath");
            return Open(new FileInfo(filePath), options);
        }

        /// <summary>
        /// Constructor with a FileInfo object to an existing file.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="options"></param>
        public static IArchive Open(FileInfo fileInfo, ReaderOptions options = null)
        {
            fileInfo.CheckNotNull("fileInfo");
            options = options ?? new ReaderOptions { LeaveStreamOpen = false };
            using (var stream = fileInfo.OpenRead())
            {
                if (ZipArchive.IsZipFile(stream, null))
                {
                    return ZipArchive.Open(fileInfo, options);
                }
                stream.Seek(0, SeekOrigin.Begin);
                if (SevenZipArchive.IsSevenZipFile(stream))
                {
                    return SevenZipArchive.Open(fileInfo, options);
                }
                stream.Seek(0, SeekOrigin.Begin);
                if (GZipArchive.IsGZipFile(stream))
                {
                    return GZipArchive.Open(fileInfo, options);
                }
                stream.Seek(0, SeekOrigin.Begin);
                if (RarArchive.IsRarFile(stream, options))
                {
                   return RarArchive.Open(fileInfo, options);
                }
                stream.Seek(0, SeekOrigin.Begin);
                if (TarArchive.IsTarFile(stream))
                {
                    return TarArchive.Open(fileInfo, options);
                }
                throw new InvalidOperationException("Cannot determine compressed stream type. Supported Archive Formats: Zip, GZip, Tar, Rar, 7Zip");
            }
        }

        /// <summary>
        /// Extract to specific directory, retaining filename
        /// </summary>
        public static void WriteToDirectory(string sourceArchive, string destinationDirectory,
                                            ExtractionOptions options = null)
        {
            using (IArchive archive = Open(sourceArchive))
            {
                foreach (IArchiveEntry entry in archive.Entries)
                {
                    entry.WriteToDirectory(destinationDirectory, options);
                }
            }
        }
#endif
    }
}

#endif