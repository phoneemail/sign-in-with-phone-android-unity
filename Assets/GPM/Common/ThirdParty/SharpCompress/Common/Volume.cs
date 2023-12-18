#if CSHARP_7_3_OR_NEWER

using System;
using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.IO;
using Gpm.Common.ThirdParty.SharpCompress.Readers;

namespace Gpm.Common.ThirdParty.SharpCompress.Common
{
    public abstract class Volume : IVolume
    {
        private readonly Stream _actualStream;

        internal Volume(Stream stream, ReaderOptions readerOptions)
        {
            ReaderOptions = readerOptions;
            if (readerOptions.LeaveStreamOpen)
            {
                stream = new NonDisposingStream(stream);
            }
            _actualStream = stream;
        }

        internal Stream Stream => _actualStream;

        protected ReaderOptions ReaderOptions { get; }

        /// <summary>
        /// RarArchive is the first volume of a multi-part archive.
        /// Only Rar 3.0 format and higher
        /// </summary>
        public virtual bool IsFirstVolume => true;

        /// <summary>
        /// RarArchive is part of a multi-part archive.
        /// </summary>
        public virtual bool IsMultiVolume => true;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _actualStream.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

#endif