#if CSHARP_7_3_OR_NEWER

using System.IO;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar.Headers;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.Rar
{
    internal class SeekableFilePart : RarFilePart
    {
        private readonly Stream stream;
        private readonly string password;

        internal SeekableFilePart(MarkHeader mh, FileHeader fh, Stream stream, string password)
            : base(mh, fh)
        {
            this.stream = stream;
            this.password = password;
        }

        internal override Stream GetCompressedStream()
        {
            stream.Position = FileHeader.DataStartPosition;
#if !NO_CRYPTO
            if (FileHeader.R4Salt != null)
            {
                return new RarCryptoWrapper(stream, password, FileHeader.R4Salt);
            }
#endif
            return stream;
        }

        internal override string FilePartName => "Unknown Stream - File Entry: " + FileHeader.FileName;
    }
}

#endif