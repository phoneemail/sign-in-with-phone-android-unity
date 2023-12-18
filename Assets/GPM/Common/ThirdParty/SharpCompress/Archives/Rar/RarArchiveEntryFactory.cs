#if CSHARP_7_3_OR_NEWER

using System.Collections.Generic;
using Gpm.Common.ThirdParty.SharpCompress.Common.Rar;

namespace Gpm.Common.ThirdParty.SharpCompress.Archives.Rar
{
    internal static class RarArchiveEntryFactory
    {
        private static IEnumerable<RarFilePart> GetFileParts(IEnumerable<RarVolume> parts)
        {
            foreach (RarVolume rarPart in parts)
            {
                foreach (RarFilePart fp in rarPart.ReadFileParts())
                {
                    yield return fp;
                }
            }
        }

        private static IEnumerable<IEnumerable<RarFilePart>> GetMatchedFileParts(IEnumerable<RarVolume> parts)
        {
            var groupedParts = new List<RarFilePart>();
            foreach (RarFilePart fp in GetFileParts(parts))
            {
                groupedParts.Add(fp);

                if (!fp.FileHeader.IsSplitAfter)
                {
                    yield return groupedParts;
                    groupedParts = new List<RarFilePart>();
                }
            }
            if (groupedParts.Count > 0)
            {
                yield return groupedParts;
            }
        }

        internal static IEnumerable<RarArchiveEntry> GetEntries(RarArchive archive,
                                                                IEnumerable<RarVolume> rarParts)
        {
            foreach (var groupedParts in GetMatchedFileParts(rarParts))
            {
                yield return new RarArchiveEntry(archive, groupedParts);
            }
        }
    }
}

#endif