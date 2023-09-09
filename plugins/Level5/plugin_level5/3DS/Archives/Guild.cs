using System.Collections.Generic;
using System.IO;
using Komponent.IO;
using Komponent.IO.Streams;
using Kontract.Models.Archive;
using plugin_level5.Compression;
using System.Linq;
using System;

namespace plugin_level5._3DS.Archives
{
    class Guild
    {
        private int _fileCount;
        public int _archiveSize;

        public IList<IArchiveFileInfo> Load(Stream input)
        {
            using var br = new BinaryReaderX(input, true);

            int _archiveSize = 0;

            // File's overall header data.
            _fileCount = br.ReadInt32();
            _archiveSize = br.ReadInt32();
            br.BaseStream.Position += 8;

            // Read all file entries (header data, no content)
            List<GuildArchiveFileEntryHeader> entries = new List<GuildArchiveFileEntryHeader>();
            for(int i = 0; i < _fileCount; i++)
            {
                entries.Add(br.ReadType<GuildArchiveFileEntryHeader>());
            }

            // Add file content
            var result = new List<IArchiveFileInfo>();
            for (var i = 0; i < entries.Count; i++)
            {
                var fileStream = new SubStream(input, entries[i].fileOffset, entries[i].fileSize);
                var fileName = entries[i].fileName;

                result.Add(new GuildArchiveFileInfo(fileStream, fileName, i));
            }

            return result;
        }

        public void Save(Stream output, IList<IArchiveFileInfo> files)
        {
            using var bw = new BinaryWriterX(output);
            var _files = files.Cast<GuildArchiveFileInfo>();

            int _numFiles = files.Count;
            int _archiveSize = files.Aggregate(0, (totalSize, file) => totalSize += (int)file.FileSize);

            // Write archive header
            bw.Write(_numFiles);
            bw.Write(_archiveSize);
            bw.WritePadding(8);

            const int OFFSET_SIZE = 4;
            const int FILESIZE_SIZE = 4;
            const int PADDING_SIZE = 8;
            const int FILENAME_SIZE = 64;

            uint fileEntryHeaderSize = OFFSET_SIZE + FILESIZE_SIZE + PADDING_SIZE + FILENAME_SIZE;
            uint totalFileEntrySize = (uint)(_numFiles * fileEntryHeaderSize);

            // Write file entry headers (no file content)
            for (int x = 0; x < files.Count; x++)
            {
                IArchiveFileInfo file = files[x];
                uint leftoverHeaderSpace = (uint)(totalFileEntrySize - (x * fileEntryHeaderSize));

                bw.Write((uint)bw.BaseStream.Position + leftoverHeaderSpace); // File Offset
                bw.Write((uint)file.FileSize);
                bw.WritePadding(8);

                string fileName = file.FilePath.ToString();
                bw.WriteString(fileName, System.Text.Encoding.ASCII);

                // Each filepath is 64 byte fixed length, so we pad out the rest.
                if (fileName.Length < FILENAME_SIZE)
                {
                    bw.WritePadding(FILENAME_SIZE - fileName.Length);
                }
            }

            // Write file data, no padding!
            foreach (GuildArchiveFileInfo file in _files)
            {
                file.SaveFileData(output);
            }
        }
    }
}
