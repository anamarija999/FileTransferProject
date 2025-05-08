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
    public interface IFileChunkReader
    {
        /// <summary>
        /// Asynchronously reads a file in 1MB chunks and adds them to a thread-safe queue.
        /// Each chunk is hashed using MD5.
        /// </summary>
        /// <param name="filePath">Path to the source file.</param>
        /// <param name="queue">Blocking collection to store the file chunks.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task ReadChunksAsync(string filePath, ChannelWriter<FileChunk> writer);
    }

}
