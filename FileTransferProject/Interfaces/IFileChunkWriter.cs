using FileTransferProject.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FileTransferProject.Interfaces
{
    public interface IFileChunkWriter
    {
        Task WriteChunks(string filePath, ChannelReader<FileChunk> reader);
    }
}
