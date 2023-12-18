#if CSHARP_7_3_OR_NEWER

using System;

namespace Gpm.Common.ThirdParty.SharpCompress.Common
{
    public class CompressedBytesReadEventArgs : EventArgs
    {
        /// <summary>
        /// Compressed bytes read for the current entry
        /// </summary>
        public long CompressedBytesRead { get; internal set; }

        /// <summary>
        /// Current file part read for Multipart files (e.g. Rar)
        /// </summary>
        public long CurrentFilePartCompressedBytesRead { get; internal set; }
    }
}

#endif