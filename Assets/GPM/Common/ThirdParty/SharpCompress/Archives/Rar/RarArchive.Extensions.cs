#if CSHARP_7_3_OR_NEWER

using System.Linq;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.Rar
{
    public static class RarArchiveExtensions
    {
        /// <summary>
        /// RarArchive is the first volume of a multi-part archive.  If MultipartVolume is true and IsFirstVolume is false then the first volume file must be missing.
        /// </summary>
        public static bool IsFirstVolume(this RarArchive archive)
        {
            return archive.Volumes.First().IsFirstVolume;
        }

        /// <summary>
        /// RarArchive is part of a multi-part archive.
        /// </summary>
        public static bool IsMultipartVolume(this RarArchive archive)
        {
            return archive.Volumes.First().IsMultiVolume;
        }
    }
}

#endif