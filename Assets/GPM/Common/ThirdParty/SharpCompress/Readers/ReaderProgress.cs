#if CSHARP_7_3_OR_NEWER



using System;
using Gpm.Common.ThirdParty.SharpCompress.Common;

namespace Gpm.Common.ThirdParty.SharpCompress.Readers
{
    public class ReaderProgress
    {
        private readonly IEntry _entry;
        public long BytesTransferred { get; }
        public int Iterations { get; }

        public int PercentageRead => (int)Math.Round(PercentageReadExact);
        public double PercentageReadExact => (float)BytesTransferred / _entry.Size * 100;

        public ReaderProgress(IEntry entry, long bytesTransferred, int iterations)
        {
            _entry = entry;
            BytesTransferred = bytesTransferred;
            Iterations = iterations;
        }
    }
}


#endif