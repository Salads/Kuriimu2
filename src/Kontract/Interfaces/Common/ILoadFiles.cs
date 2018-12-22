﻿using System;
using System.IO;

namespace Kontract.Interfaces.Common
{
    /// <inheritdoc />
    /// <summary>
    /// This interface allows a plugin to load files.
    /// </summary>
    public interface ILoadFiles : IDisposable
    {
        /// <summary>
        /// Loads the given file.
        /// </summary>
        /// <param name="filename">The file to be loaded.</param>
        void Load(StreamInfo filename);
    }

    public class StreamInfo
    {
        public Stream FileData { get; set; }
        public string FileName { get; set; }
    }
}