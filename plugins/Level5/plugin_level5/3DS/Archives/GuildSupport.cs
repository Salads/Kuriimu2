using System.Collections.Generic;
using System.IO;
using System.Linq;
using Komponent.IO;
using Komponent.IO.Attributes;
using Kontract.Kompression.Configuration;
using Kontract.Models.Archive;

namespace plugin_level5._3DS.Archives
{
    class GuildArchiveFileInfo : ArchiveFileInfo
    {
        public int FileId { get; }

        public GuildArchiveFileInfo(Stream fileData, string filePath, int fileId) : base(fileData, filePath)
        {
            FileId = fileId;
        }

        public GuildArchiveFileInfo(Stream fileData, string filePath, int fileId, IKompressionConfiguration configuration, long decompressedSize) : base(fileData, filePath, configuration, decompressedSize)
        {
            FileId = fileId;
        }
    }

    class GuildArchiveFileEntryHeader
    {
        public int fileOffset;
        public int fileSize;
        public long padZero;

        [FixedLength(64)]
        public string fileName;
    }

    class GuildSupport
    {

    }
}
